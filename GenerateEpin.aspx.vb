Imports System.Data
Partial Class App_UI_Application_Pages_GenerateEpin
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim quantity As String
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Public Sub FillKit()
        If Session("CompId") = "1038" Then
            Sql = "Select * From (Select 0 As KitID,'-- Select Package --' As KitName Union ALL " & _
" Select kitId,KitName From M_KitMaster Where (KitId=1 Or ActiveStatus='Y') And RowStatus='Y' and plantype<>2 ) As Temp Order By kitId"

        ElseIf Session("CompId") = "1107" Then
            Sql = "Select * From (Select 0 As KitID,'-- Select Package --' As KitName Union ALL Select kitId,KitName From M_KitMaster WHERE ActiveStatus = 'Y' AND RowStatus = 'Y' AND KitId <> 1 ) AS Temp Order By kitId"


        Else
            Sql = "Select * From (Select 0 As KitID,'-- Select Package --' As KitName Union ALL Select kitId,KitName From M_KitMaster Where (KitId=1 Or ActiveStatus='Y') And RowStatus='Y' ) As Temp Order By kitId"

        End If
        Dim Dt As New DataTable
        Dt = obj.GetData(Sql)
        CmbKit.DataSource = Dt
        CmbKit.DataTextField = "KitName"
        CmbKit.DataValueField = "KitId"
        CmbKit.DataBind()
    End Sub

    Private Sub getStock()
        lblStock.innerHtml = "<span style=""color:red""><i>Available Stock</i></span> <br />"
        Dim Dt As New DataTable
        Sql = "Select A.KitName,IsNULL(Count(B.FormNo),0) As Stock From M_KitMaster As A Left Join M_FormGeneration As B On A.kitID=B.ProdID And B.GeneratedBy='' and B.LastModified='' and B.FCode='WR' and B.SoldBy='WR' And B.ActiveStatus='N' WHERE (A.ActiveStatus='Y' Or KitId=1) And A.RowStatus='Y'  Group by A.KitName,A.KitID Order by A.KitID"
        Dt = obj.GetData(Sql)
        Dim i As Integer = 1
        For Each dr As DataRow In Dt.Rows
            lblStock.innerHtml = lblStock.innerHtml & i & ". " & dr("KitName") & " : <span style=""color:blue""><i>" & dr("Stock") & "</i></span><br />"
            i = i + 1
        Next
    End Sub

    Private Sub FillDetail()
        Dim Dt As New DataTable

        Sql = "Select Top " & TxtQty.Text & " A.FormNo,A.ScratchNo,dbo.FormatDate(A.StockDate,'dd-MMM-yyyy') As StockDate,B.KitName From M_FormGeneration As A,M_KitMaster As B Where A.ProdID=B.KitID And A.UserID=" & Session("UserID") & " And A.ProdID=" & CmbKit.SelectedValue & " Order by A.TransNo Desc"
        Dt = obj.GetData(Sql)
        GvBatchMaster.DataSource = Dt
        GvBatchMaster.DataBind()
    End Sub

    Protected Sub BtnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerate.Click
        Try
            lblError.Text = ""
            BtnGenerate.Enabled = False
            Dim Remark As String = ""
            If CmbKit.SelectedValue = 0 Then
                lblError.Text = "Invalid Package."
            ElseIf TxtQty.Text = "" Then
                lblError.Text = "Enter Quantity."
            ElseIf Val(TxtQty.Text) <= 0 Then
                lblError.Text = "Invalid Quantity."
            Else
                Remark = TxtQty.Text & " Pin Generate of Package " & CmbKit.SelectedItem.Text & ""
                quantity = TxtQty.Text
                Sql = "Exec Generate_EPins " & CmbKit.SelectedValue & "," & TxtQty.Text & "," & Session("UserID") & ";"
                Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp)Values" & _
      "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','EPin Generate ','Epin Generate','" & Remark & "',Getdate())"

                If obj.SaveData(Sql) <> 0 Then
                    lblError.Text = TxtQty.Text & " ePin stock of " & CmbKit.SelectedItem.Text & " have been sucessfully generated."
                    FillStock()
                    FillDetail()

                    'BtnExport.Visible = True
                End If
            End If

            BtnGenerate.Enabled = True
            TxtQty.Text = 0
            CmbKit.SelectedIndex = -1
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
        
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then

            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Not Page.IsPostBack Then
                Session("PageName") = "Epin / Generate Epin"
                FillKit()
                'getStock()
            End If
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        lblError.Text = ""
        BtnGenerate.Enabled = True
        TxtQty.Text = 0
        CmbKit.SelectedIndex = 0
        GvBatchMaster.DataSource = Nothing
        GvBatchMaster.DataBind()
    End Sub

    Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
      Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Sql = "Select Top " & quantity & " A.FormNo,A.ScratchNo,dbo.FormatDate(A.StockDate,'dd-MMM-yyyy') As StockDate,B.KitName From M_FormGeneration As A,M_KitMaster As B Where A.ProdID=B.KitID And A.UserID=" & Session("UserID")
            dtTemp = New DataTable
            dtTemp = obj.GetData(Sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("GenerateEpin.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub

    Protected Sub BtnExportStock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportStock.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Sql = "Select A.KitName,IsNULL(Count(B.FormNo),0) As Stock From M_KitMaster As A Left Join M_FormGeneration As B On A.kitID=B.ProdID And B.GeneratedBy='' and B.LastModified='' and B.FCode='WR' and B.SoldBy='WR' And B.ActiveStatus='N' WHERE A.ActiveStatus='Y' And A.RowStatus='Y'  Group by A.KitName,A.KitID Order by A.KitID"

            dtTemp = New DataTable
            dtTemp = obj.GetData(Sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("AvailableStock.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
        Dim sw As New System.IO.StringWriter
        Dim htw As System.Web.UI.HtmlTextWriter
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
        Response.Charset = ""
        dg.EnableViewState = False
        htw = New HtmlTextWriter(sw)
        dg.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub

    Protected Sub CmbKit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbKit.SelectedIndexChanged
        
        FillStock()
        'For Each dr As DataRow In Dt.Rows
        '    lblStock.InnerHtml = lblStock.InnerHtml & i & ". " & dr("KitName") & " : <span style=""color:blue""><i>" & dr("Stock") & "</i></span><br />"
        '    i = i + 1
        'Next
    End Sub
    Private Sub FillStock()
        lblStock.InnerHtml = "<span style=""color:red""><i>Available Stock</i></span> <br />"
        Dim Dt As New DataTable
        Sql = "Select A.KitName,A.Bv as KitBv,A.kitAmount as MRP,IsNULL(Count(B.FormNo),0) As Stock From M_KitMaster As A Left Join M_FormGeneration As B On A.kitID=B.ProdID And B.GeneratedBy='' and B.LastModified='' and B.FCode='WR' and B.SoldBy='WR' And B.ActiveStatus='N' And B.Iscancel='N'  WHERE (A.ActiveStatus='Y' Or KitId=1) And A.RowStatus='Y'  and a.KitId='" & CmbKit.SelectedValue & "' Group by A.KitName,A.KitID ,A.Bv,A.KitAmount Order by A.KitID"
        Dt = obj.GetData(Sql)
        GrdStock.DataSource = Dt
        GrdStock.DataBind()
        GrdStock.Visible = True
        trstock.Visible = True
    End Sub

End Class
