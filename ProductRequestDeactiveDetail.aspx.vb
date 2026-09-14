Imports System.Data.SqlClient
Imports System.Data
Imports ClosedXML.Excel
Imports System.IO

Partial Class ProductRequestDeactiveDetail
    Inherits System.Web.UI.Page

    Dim strquery As String
    Dim dt As DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Dim formno As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then

            If Page.IsPostBack = False Then
                PaymentDetails()

            End If

        Else
            Response.Redirect("Logout.aspx")
            Response.End()
        End If
    End Sub
    Private Function GetFormNo(ByVal idno As String) As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Dim idNo As String
        Dim formno As String
        'idNo = Trim(txtMemberID.Text)
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemberID.Text = ""
        End If
        Return formno
    End Function

    'Private Sub PaymentDetails()
    '    Try
    '        '  DgReceivedPin.DataSource = Nothing
    '        ' DgReceivedPin.DataBind()
    '        Dim dt As New DataTable
    '        Dim col As String = ""
    '        Dim obj As DAL
    '        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        Dim Condition As String = ""

    '        If (HttpContext.Current.Session("InvDatabase" & Session("CompID")) = "AA") Then
    '            strquery = "select Orderno, replace(convert(varchar,orderdate,106),' ','-') as orderdate,OrderAmt as OrderAmount,OrderQty ,WalletAmt as Pinwallet," & _
    '      " OtherAmt as OtherAmt,Case when ActiveStatus='Y'  and DispatchStatus='C' then 'Dispatched' when ActiveStatus='D' then 'Rejected' else 'Pending'   " & _
    '       " end as status,Case when Ordertype='T' then 'Activation' else 'Repurchase' end as KitName,BV  from TrnOrder where Formno='" & Val(Session("Formno")) & "'"

    '        Else
    '            'strquery = " Select   Isnull(b.userBillno,a.orderNo) as orderNo,"
    '            'strquery &= " Replace( Convert(Varchar,a.orderDate,106),' ','-') As OrderDate,a.Orderqty,a.orderamt  as OrderAmount,"
    '            'strquery &= " WalletAmt as Pinwallet,OtherAmt as OtherAmt,"
    '            'strquery &= " Case when a.Ordertype='T' then 'Activation' else 'Repurchase' end as KitName,  a.Bv,"
    '            'strquery &= " (Case When a.DispatchStatus  = 'N' And a.ActiveStatus = 'Y'  Then 'Pending' "
    '            'strquery &= " When a.DispatchStatus  = 'C' And a.ActiveStatus = 'Y'  Then 'Dispatch'"
    '            'strquery &= " When a.DispatchStatus  = 'N' And a.ActiveStatus = 'D'  Then 'Reject'"
    '            'strquery &= " When a.DispatchStatus  = 'C' And a.ActiveStatus = 'D'  Then 'Reject' End) As Status  "
    '            'strquery &= " from trnorder as a With(Nolock) "
    '            'strquery &= " Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain  as b With(Nolock) on Convert(Varchar, a.orderno) = Convert(Varchar,b.orderno)"
    '            'strquery &= " Where(a.FormNo = '" & Val(Session("Formno")) & "')"
    '            'strquery &= " Order by  a.orderDate Desc"

    '            strquery = " select Cast(Orderno as varchar) Orderno, replace(convert(varchar,orderdate,106),' ','-') as orderdate,OrderAmt as OrderAmount,"
    '            strquery &= " OrderQty ,WalletAmt as Pinwallet ,OtherAmt as OtherAmt,"
    '            strquery &= " Case when ActiveStatus='Y'  and DispatchStatus='C' then 'Dispatched' when ActiveStatus='D' then 'Rejected' else 'Pending'    "
    '            strquery &= " end as status,Case when Ordertype='T' then 'Activation' else 'Repurchase' end as KitName  "
    '            If (Session("CompID") = 1007) Then
    '                strquery &= " ,PV As BV"
    '            Else

    '                strquery &= "  ,BV"
    '            End If

    '            strquery &= " from TrnOrder where Formno='" & Val(Session("Formno")) & "'"
    '            strquery &= " Union All"

    '            strquery &= " Select BillNo as Orderno,replace(convert(varchar,BillDate,106),' ','-') as orderdate,NetPayable as OrderAmount,"
    '            strquery &= " TotalQty as OrderQty ,0 as Pinwallet ,0 as OtherAmt,"
    '            strquery &= " 'Dispatched'  as status ,Case when Billtype='B' then 'Activation' else 'Repurchase' end as KitName "

    '            If (Session("CompID") = 1007) Then
    '                strquery &= " ,Pvvalue  BV"
    '            Else

    '                strquery &= "  ,Bvvalue as BV"
    '            End If
    '            strquery &= "  from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..trnbillmain Where Formno='" & Val(Session("Formno")) & "'"


    '        End If
    '        dt = obj.GetData(strquery)
    '        'DgReceivedPin.CurrentPageIndex = 0

    '        RptDirects.DataSource = dt
    '        RptDirects.DataBind()
    '        Session("ReceivedPin") = dt


    '    Catch ex As Exception
    '        Response.Write(ex.Message & "SideB")
    '    End Try
    'End Sub

    Private Sub PaymentDetails()
        Try
            '  DgReceivedPin.DataSource = Nothing
            ' DgReceivedPin.DataBind()
            Dim dt As New DataTable
            Dim col As String = ""
            Dim col2 As String = ""
            Dim col9 As String = ""
            Dim obj As DAL
            Dim Idno As String = "0"
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Condition As String = ""
            If txtMemberID.Text <> "" Then
                Idno = txtMemberID.Text.Trim
                'formno = GetFormNo(Idno)
            Else
                Idno = "0"
            End If
            If (Session("CompID") = "1066") Then
                strquery = " Sp_MyPurchasedeactive '" & Idno & "','" & txtfrmdate.Text & "','" & txttodate.Text & "'"
            Else
                If (HttpContext.Current.Session("InvDatabase" & Session("CompID")) = "AA") Then
                    strquery = "select Orderno, replace(convert(varchar,orderdate,106),' ','-') as orderdate,OrderAmt as OrderAmount,OrderQty ,WalletAmt as Pinwallet," & _
              " OtherAmt as OtherAmt,Case when ActiveStatus='Y'  and DispatchStatus='C' then 'Dispatched' when ActiveStatus='D' then 'Rejected' else 'Pending'   " & _
               " end as status,Case when Ordertype='T' then 'Activation' else 'Repurchase' end as KitName,BV  from TrnOrder where Formno='" & Val(Session("Formno")) & "'"

                Else


                    strquery = " select Cast(Orderno as varchar) Orderno, replace(convert(varchar,orderdate,106),' ','-') as orderdate,OrderAmt as OrderAmount,"
                    strquery &= " OrderQty ,WalletAmt as Pinwallet ,OtherAmt as OtherAmt,"
                    strquery &= " Case when ActiveStatus='Y'  and DispatchStatus='C' then 'Dispatched' when ActiveStatus='D' then 'Rejected' else 'Pending'    "
                    strquery &= " end as status,Case when Ordertype='T' then 'Activation' else 'Repurchase' end as KitName,  "

                    strquery &= " '' as CourierName, '' as DocketNo, '' as DocketDate,'#' as Website,BV,PV"



                    strquery &= " from TrnOrder where Formno='" & Val(Session("Formno")) & "' and DispatchStatus <>'C' "
                    strquery &= " Union All"

                    strquery &= " Select Cast(UserBillNo as varchar) as Orderno,replace(convert(varchar,BillDate,106),' ','-') as orderdate,NetPayable as OrderAmount,"
                    strquery &= " TotalQty as OrderQty ,0 as Pinwallet ,0 as OtherAmt,"
                    strquery &= " 'Dispatched'  as status ,Case when Billtype='B' then 'Activation' else 'Repurchase' end as KitName, "
                    strquery &= " a.CourierName, Scratch as DocketNo, Replace(Convert(varchar,isnull(DocketDate,''),106),' ','-') as DocketDate,Website,Bvvalue as BV,Pvvalue as PV "

                    strquery &= "  from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..trnbillmain as a ," & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_CourierMaster as b Where Formno='" & Val(Session("Formno")) & "'"
                    strquery &= " And a.CourierId = b.CourierId "

                End If

            End If

            dt = obj.GetData(strquery)
            If dt.Rows.Count > 0 Then

                btnExport.Visible = True
               
            End If

            'RptDirects.DataSource = dt
            'RptDirects.DataBind()
            GvData.DataSource = dt
            GvData.DataBind()

            Session("Gvdata") = dt


        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try
    End Sub



    Private Shared Function Base64Encode(ByVal plainText As String) As String
        Dim plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText)
        Return System.Convert.ToBase64String(plainTextBytes)
    End Function

    Private Shared Function Base64Decode(ByVal base64EncodedData As String) As String
        Dim base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData)
        Return System.Text.Encoding.UTF8.GetString(base64EncodedBytes)
    End Function



    Protected Sub btnsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        PaymentDetails()
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            If Session("CompID") = "1066" Then
                PaymentDetails()
                'Else

            End If
            ExportExcel()

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")

        End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("Gvdata")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Report")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=ProductRequestDeactiveReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Orderno, Idno, scrname As String
        'Dim msgRslt As MsgBoxResult = MsgBox("Jig tool succussfully registered. Do you want to continue register jigtool? .", MsgBoxStyle.YesNo)
        'If msgRslt = MsgBoxResult.Yes Then


        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)

        Orderno = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        Idno = DirectCast(GVRw.FindControl("lblidno"), Label).Text
        formno = GetFormNo(Idno)
        'Dim Sql As String = "exec sp_Iddeactive " & Val(Session("Formno")) & ",'" & Orderno & "' "
        Dim Sql As String = "exec sp_Iddeactive " & formno & ",'" & Orderno & "' "

        Dim obj As DAL
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        Dim updateEffect As Integer = 0
        'below commit 19 Jan 2023
        updateEffect = obj.UpdateData(Sql)

        'Dim updateEffect As Integer = obj.GetData(Sql)
        If updateEffect <> 0 Then
            'If dt.Rows.Count > 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Row Data! ');" & "</SCRIPT>"
        End If
        'ElseIf msgRslt = MsgBoxResult.No Then
        'scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
        'End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
        'BindData()
        PaymentDetails()
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("Gvdata")
        GvData.DataBind()
    End Sub
End Class
