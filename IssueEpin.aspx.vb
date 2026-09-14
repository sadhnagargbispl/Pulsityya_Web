Imports System.Data
Imports System.Net
Imports System.IO
Imports ClosedXML.Excel

Partial Class App_UI_Application_Pages_IssueEpin
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String
    Dim objGen As clsGeneral = New clsGeneral
    Public Sub FillKit()
        If Session("CompId") = "1038" Then
            Sql = "Select * From (Select 0 As KitID,'-- Select Package --' As KitName,'N' as OldKit Union ALL Select kitId,KitName,OldKit From M_KitMaster Where (KitId=1 Or ActiveStatus='Y') And RowStatus='Y' and Plantype<>2) As Temp Order By kitId"

        Else
            Sql = "Select * From (Select 0 As KitID,'-- Select Package --' As KitName,'N' as OldKit Union ALL Select kitId,KitName,OldKit From M_KitMaster Where (KitId=1 Or ActiveStatus='Y') And RowStatus='Y') As Temp Order By kitId"

        End If
        Dim Dt As New DataTable
        Dt = obj.GetData(Sql)
        If Dt.Rows.Count > 0 Then
            LblProductAvail.Text = Dt.Rows(0)("OldKit")
            CmbKit.DataSource = Dt
            CmbKit.DataTextField = "KitName"
            CmbKit.DataValueField = "KitId"
            CmbKit.DataBind()
        End If

    End Sub

    Private Sub getStock()
        lblStock.InnerHtml = "<span style=""color:red""><i>Available Stock</i></span> <br />"
        Dim Dt As New DataTable
        Sql = "Select A.KitName,IsNULL(Count(B.FormNo),0) As Stock,A.KitAmount,A.Bv as KitBv From M_KitMaster As A Left Join M_FormGeneration As B On A.kitID=B.ProdID And B.GeneratedBy='' and B.LastModified='' and B.FCode='WR' and B.SoldBy='WR' And B.ActiveStatus='N' Where (A.ActiveStatus='Y' or KitId=1) And A.RowStatus='Y' Group by A.KitName,A.KitID,A.BV,A.KitAmount Order by A.KitID"
        Dt = obj.GetData(Sql)
        Dim i As Integer = 1
        'For Each dr As DataRow In Dt.Rows
        '    lblStock.InnerHtml = lblStock.InnerHtml & i & ". " & dr("KitName") & " : <span style=""color:blue""><i>" & dr("Stock") & "</i></span><br />"
        '    i = i + 1
        Session("Stock") = Dt
        GrdStock.DataSource = Dt
        GrdStock.DataBind()
        'Next
    End Sub


    Private Sub FillDetail()
        Dim Dt As New DataTable
        Sql = "Select T.InvoiceNo,T.FCode,A.FormNo,A.ScratchNo,dbo.FormatDate(T.RecTimeStamp,'dd-MMM-yyyy') As IssueDt,B.KitName From M_FormGeneration As A," & _
 "(Select Top " & Val(TxtQty.Text) & " * FROM TrnFormGeneration ORDER BY RecTimeStamp DESC) as T,M_KitMaster As B " & _
 " Where A.ProdID=B.KitID AND A.FormNo=T.FormNo Order by A.TransNo Desc"
        Dt = obj.GetData(Sql)
        GvBatchMaster.DataSource = Dt
        GvBatchMaster.DataBind()
        'GvBatchMaster.Visible = True
    End Sub

    Protected Sub BtnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerate.Click
        Try
            Dim msg As String = ""
            lblError.Text = ""
            Dim str As String = ""
            BtnGenerate.Enabled = False
            Dim Remark As String = TxtQty.Text & "Epin of Package " & CmbKit.SelectedItem.Text & " Issue To IdNo:" & TxtIDNo.Text & " "
            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            ElseIf CmbKit.SelectedValue = 0 Then
                lblError.Text = "Invalid Package."
                Exit Sub
            ElseIf TxtQty.Text = "" Then
                lblError.Text = "Enter Quantity."
                Exit Sub
            ElseIf Val(TxtQty.Text) <= 0 Then
                lblError.Text = "Invalid Quantity."
                Exit Sub
            Else
                If Check_IdNo() = False Then
                    lblError.Text = "Invalid Member ID."
                    Exit Sub
                End If
                If Check_Stock() = False Then
                    lblError.Text = "Please check stock."
                End If
                If LblProductAvail.Text = "Y" Then
                    msg = CheckProdStockAvail(Val(CmbKit.SelectedValue), Val(TxtQty.Text))
                    If msg <> "" Then
                        If msg.Contains("Please attach Products with Kit.") = True Then
                            lblError.Text = msg
                            scrname = "<SCRIPT language='javascript'>alert('" & msg & "');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Availability!!", scrname, False)
                            Exit Sub
                        Else
                            lblError.Text = "Available Stock for Product: " & msg & " is not Sufficient."
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Availability!!", scrname, False)
                            Exit Sub
                        End If
                    End If
                End If


                Sql = "Exec Sp_IssueEpins '" & Trim(TxtIDNo.Text) & "'," & CmbKit.SelectedValue & "," & Val(TxtQty.Text) & ",'" & TxtRemark.Text & "'," & Session("UserID") & ";"
                Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Issue Epin ','Issue EPin','" & Remark & "',Getdate(),'" & LblFormno.Text & "')"

                If obj.SaveData(Sql) <> 0 Then
                    lblError.Text = TxtQty.Text & " ePin have been successfully issued to " & TxtIDNo.Text & "."
                    FillStock()
                    FillDetail()
                    TxtRemark.Text = ""
                    'BtnExport.Visible = True
                    'SSendsms()
                    'RSendsms()
                End If
            End If
            BtnGenerate.Enabled = True
            TxtIDNo.Text = ""
            TxtQty.Text = 0
            LblMemName.Text = ""
            LblStockChk.Text = ""
            CmbKit.SelectedIndex = -1
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Session("AStatus") = "OK" Then
                If Not Page.IsPostBack Then
                    Session("PageName") = "Epin / Issue EPin"
                    FillKit()
                    'getStock()
                    'FillDetail()
                End If
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        lblError.Text = ""
        BtnGenerate.Enabled = True
        TxtQty.Text = 0
        CmbKit.SelectedIndex = 0
        GvBatchMaster.DataSource = Nothing
        GvBatchMaster.DataBind()
        BtnExport.Visible = False
    End Sub

    Private Sub SSendsms()
        Try
            Dim client As WebClient
            Dim baseurl As String = ""
            Dim Data As System.IO.Stream
            Dim reader As StreamReader
            Dim s As String
            Dim amount As String = ""
            Dim value As String = ""
            Dim dt As DataTable
            dt = New DataTable

            Dim str As String = "select KitAmount from M_KitMaster where  KitId='" & CmbKit.SelectedValue & "'"
            dt = obj.GetData(str)
            If dt.Rows.Count > 0 Then
                amount = Val(dt.Rows(0)("KitAmount"))
                value = Val(amount) * Val(TxtQty.Text)
            End If
            Dim Msg As String = "Dear " & LblMemName.Text & " you have received " & TxtQty.Text & "  Epin of value Rs." & value & " to " & TxtIDNo.Text & ". Visit " & Session("CompWeb") & " for more details."
            '  Dim Msg As String = "Congratulations," & TxtQty.Text & "Pin Of " & CmbKit.SelectedItem.Text & " has been Issued on Your Id " & TxtIDNo.Text & " From " & Session("CompName") & "."
            client = New WebClient()
            'baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & LblMemMobl.Text & "&msg=" & Msg & ""
            'baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & Msg & "&Contacts=" & LblMemMobl.Text & "&SenderId=" & Session("ClientId") & ""
            baseurl = Session("SmsAPI") & "username=" & Session("SmsId") & "&password=" & Session("SmsPass") & "&Sender=" & Session("ClientId") & "&to=" & LblMemMobl.Text & "&message=" & Msg & "&format=text&unique=1"

            Data = client.OpenRead(baseurl)
            reader = New StreamReader(Data)
            s = reader.ReadToEnd()
            Data.Close()
            reader.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        'If PostXml = "OK" Then

        'End If
    End Sub
    Private Sub RSendsms()
        Try
            Dim client As WebClient
            Dim baseurl As String = ""
            Dim Data As System.IO.Stream
            Dim reader As StreamReader
            Dim s As String
            Dim amount As String = ""
            Dim value As String = ""
            Dim dt As DataTable
            dt = New DataTable

            Dim str As String = "select KitAmount from M_KitMaster where  KitId='" & CmbKit.SelectedValue & "'"
            dt = obj.GetData(str)
            If dt.Rows.Count > 0 Then
                amount = Val(dt.Rows(0)("KitAmount"))
                value = Val(amount) * Val(TxtQty.Text)
            End If
            Dim Msg As String = "Dear " & Session("UserName") & ", you have transferred  " & TxtQty.Text & " Epin of value Rs." & value & " to " & TxtIDNo.Text & " on " & Now & ""
            '  Dim Msg As String = "Congratulations," & TxtQty.Text & "Pin Of " & CmbKit.SelectedItem.Text & " has been Issued on Your Id " & TxtIDNo.Text & " From " & Session("CompName") & "."
            client = New WebClient()
            '  baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Session("Mobile") & "&msg=" & Msg & ""
            ' baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & Msg & "&Contacts=" & Session("Mobile") & "&SenderId=" & Session("ClientId") & ""
            baseurl = Session("SmsAPI") & "username=" & Session("SmsId") & "&password=" & Session("SmsPass") & "&Sender=" & Session("ClientId") & "&to=" & Session("Mobile") & "&message=" & Msg & "&format=text&unique=1"

            Data = client.OpenRead(baseurl)
            reader = New StreamReader(Data)
            s = reader.ReadToEnd()
            Data.Close()
            reader.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        'If PostXml = "OK" Then

        'End If
    End Sub
    Private Function Check_IdNo() As Boolean
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName , Mobl,Formno From " & obj.tblMemberMaster & " WHERE IDNO='" & Trim(TxtIDNo.Text) & "'"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            TxtIDNo.Text = ""
            BtnGenerate.Enabled = False
            LblFormno.Text = ""
            Return False
        Else
            LblMemName.Text = Dt_.Rows(0)("MemName")
            LblMemMobl.Text = Dt_.Rows(0)("Mobl")
            LblFormno.Text = Dt_.Rows(0)("Formno")

            LblMemName.ForeColor = Drawing.Color.Black
            BtnGenerate.Enabled = True
            Return True
        End If
    End Function

    Private Function Check_Stock() As Boolean
        Sql = "Select Count(*) as StkQty FROM M_FormGeneration WHERE GeneratedBy<>'Y' AND LastModified<>'Y' And ActiveStatus = 'N' And  Iscancel = 'N' AND ProdId='" & Val(CmbKit.SelectedValue) & "'"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            LblStockChk.Text = "Stock not Found."
            BtnGenerate.Enabled = False
            Return False
        Else
            If Dt_.Rows(0)("StkQty") < Val(TxtQty.Text) Then
                LblStockChk.Text = "Available Stock for " & CmbKit.SelectedItem.Text & " is " & Dt_.Rows(0)("StkQty").ToString()
                TxtQty.Text = "0"
                BtnGenerate.Enabled = False
                Return False
            Else
                LblStockChk.Text = ""
                BtnGenerate.Enabled = True
                Return True
            End If
        End If
    End Function

    Private Function CheckProdStockAvail(ByVal KitID As Integer, ByVal Qty As Integer) As String
        Dim Msg As String = ""
        Sql = "Select a.*,b.ProductName,CASE WHEN a.AvailQty<ReqQty THEN b.ProductName ELSE '' END as NAProd FROM" & _
" (Select ISNULL(FCode,'" & Session("WRPartyCode") & "') as FCode,b.Barcode,b.ProdID,ISNULL(SUM(a.Qty),0) as AvailQty," & Qty & "*  b.Qty as ReqQty " & _
" FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..Im_CurrentStock a RIGHT JOIN M_KitProductDetail b ON a.ProdId=b.ProdId AND a.Barcode=b.Barcode AND FCode ='" & Session("WRPartyCode") & "'" & _
" WHERE   b.KitId='" & KitID & "' And b.ActiveStatus='Y' and b.RowStatus='Y'" & _
" GROUP BY ISNULL(FCode,'" & Session("WRPartyCode") & "') ,b.Barcode,b.ProdID, b.Qty) as a," & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as b" & _
" WHERE a.ProdID=b.ProdID"
        Dim Dt_ As New DataTable
        Dt_ = obj.GetData(Sql)
        If Dt_.Rows.Count = 0 And ((KitID <> 1) And (KitID <> 12) And (KitID <> 13) And (KitID <> 23) And (KitID <> 24) And (KitID <> 28) And (KitID <> 29) And (KitID <> 31) And (KitID <> 32)) Then
            Msg = "Please attach Products with Kit."
            'ElseIf KitID = 13 Then
            '    Msg = ""
        Else

            For i As Integer = 0 To Dt_.Rows.Count - 1
                If Dt_.Rows(i)("NAProd").ToString <> "" Then
                    Msg = Msg & Dt_.Rows(i)("NAProd").ToString & ", "
                End If
            Next
            If Msg <> "" Then
                Msg = Msg.Substring(0, Len(Msg) - 2)
            End If
        End If
        Return Msg
    End Function

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()
    End Sub

    Protected Sub TxtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtQty.TextChanged
        Check_Stock()
    End Sub

    Protected Sub GrdStock_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdStock.PageIndexChanging
        GrdStock.PageIndex = e.NewPageIndex
        GrdStock.DataSource = Session("Stock")
        GrdStock.DataBind()
    End Sub

    Protected Sub BtnExpStock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExpStock.Click
        Dim dtTemp As New DataTable
        Dim Dg As New DataGrid
        Dg.DataSource = Session("Stock")
        dg.DataBind()
        ExportExcel()
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("Stock")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "KitStock")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=KitStock.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
    

    Protected Sub CmbKit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbKit.SelectedIndexChanged
        FillStock()
    End Sub
    Private Sub FillStock()
        lblStock.InnerHtml = "<span style=""color:red""><i>Available Stock</i></span> <br />"
        Dim Dt As New DataTable
        Sql = "Select A.KitName,IsNULL(Count(B.FormNo),0) As Stock,A.KitAmount,A.Bv as KitBv,a.oldKit " & _
        " From M_KitMaster As A Left Join M_FormGeneration As B On A.kitID=B.ProdID And B.GeneratedBy='' and B.LastModified=''" & _
        " and B.FCode='WR' and B.SoldBy='WR' And B.ActiveStatus='N' And B.Iscancel='N'  Where (A.ActiveStatus='Y' or KitId=1) And A.RowStatus='Y' " & _
        " and a.KitId='" & CmbKit.SelectedValue & "' Group by A.KitName,A.KitID,A.BV,A.KitAmount,a.OldKit Order by A.KitID"
        Dt = obj.GetData(Sql)
        Dim i As Integer = 1
        'For Each dr As DataRow In Dt.Rows
        '    lblStock.InnerHtml = lblStock.InnerHtml & i & ". " & dr("KitName") & " : <span style=""color:blue""><i>" & dr("Stock") & "</i></span><br />"
        '    i = i + 1
        If Dt.Rows.Count > 0 Then
            LblProductAvail.Text = Dt.Rows(0)("OldKit")
            Session("Stock") = Dt
            GrdStock.DataSource = Dt
            GrdStock.DataBind()
        End If

    End Sub
End Class
