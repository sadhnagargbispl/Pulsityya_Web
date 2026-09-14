Imports System.Data.SqlClient
Imports System.Data
Partial Class ProductRequestDetail
    Inherits System.Web.UI.Page
    Dim conn As New SqlConnection
    Dim Comm As New SqlCommand
    Dim Adp As SqlDataAdapter
    Dim strquery As String
    Dim dt As DataTable
    Dim Ds As DataSet
    Dim ObjDal As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("AStatus") = "OK" Then
            Session("PageName") = " Epin / Product Request Detail"
            LblError.Text = ""
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub



    Private Sub PaymentDetails()
        Try
            DgReceivedPin.DataSource = Nothing
            DgReceivedPin.DataBind()
            Dim dt As New DataTable
            Dim col As String = ""
            Dim obj As DAL
            obj = New DAL((HttpContext.Current.Session("MlmDatabase" & Session("CompID"))))
            Dim Condition As String = ""
            Dim Formno As String = ""

            conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            conn.Open()
            If txtFromDate.Text <> "" Then
                Condition = Condition & "AND Cast(Convert(varchar,OrderDate,106) as Datetime) >='" & txtFromDate.Text & "'  "
            End If
            If TxtToDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(varchar,OrderDate,106) as Datetime)<='" & TxtToDate.Text & "' "

            End If
            If txtMemberId.Text <> "" Then

                Formno = GetFormNo()
                Condition = Condition & " and formNo='" & Formno & "'"
            End If
            strquery = " select Idno,MemFirstName as MemberName,orderno,Replace(Convert(varchar,OrderDate,106),' ','-') as OrderDate,OrderQty,OrderAmt,BankAmt," & _
            " OtherAmt,WalletAmt,Remark,Case when  DispatchStatus='C' and IsConfirm='Y' then 'Dispatched' when DispatchStatus<>'C' and Isconfirm='Y' " & _
            " then 'Pending' when IsConfirm='R' then 'Rejected' end as Dispatchstatus , " & _
            " '' as changeDispatch,Case when OrderType='O' then 'Repurchase' else 'Joining' end as Status  from Trnorder  where 1=1 " & Condition & " Order by OrderDate Desc "

            dt = obj.GetData(strquery)
            DgReceivedPin.CurrentPageIndex = 0

            DgReceivedPin.CurrentPageIndex = 0
            DgReceivedPin.DataSource = dt
            DgReceivedPin.DataBind()
            If dt.Rows.Count > 0 Then
                LblError.Text = ""
                NoData.Visible = False
                DgReceivedPin.Visible = True
            Else
                NoData.Visible = True
                DgReceivedPin.Visible = False

            End If


        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try
    End Sub
    Private Function GetFormNo() As String
        ObjDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String = ""
        idNo = txtMemberId.Text
        Dim qry As String = "Select FormNo from M_MemberMaster where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = ObjDal.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblError.Visible = True
            txtMemberId.Text = ""
        End If
        Return formno
    End Function

    Protected Sub DgReceivedPin_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DgReceivedPin.PageIndexChanged
        DgReceivedPin.CurrentPageIndex = e.NewPageIndex
        DgReceivedPin.DataSource = Session("ReceivedPin")
        DgReceivedPin.DataBind()
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        PaymentDetails()
    End Sub

    Protected Sub BtnSearch_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BtnSearch.Command

    End Sub

    Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid

            Dim sql As String
            Dim formNo As String
            Dim condition As String = ""
            Dim condition1 As String = ""


            If txtFromDate.Text <> "" Then
                condition = condition & "AND Cast(Convert(varchar,OrderDate,106) as Datetime) >='" & txtFromDate.Text & "'  "
            End If
            If TxtToDate.Text <> "" Then
                condition = condition & " And Cast(Convert(varchar,OrderDate,106) as Datetime)<='" & TxtToDate.Text & "' "

            End If
            If txtMemberId.Text = "" Then

            Else
                formNo = GetFormNo()
                condition = condition & " and formNo='" & formNo & "'"
            End If


            ObjDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            sql = " select Idno,MemFirstName as MemberName,orderno,Replace(Convert(varchar,OrderDate,106),' ','-') as OrderDate,OrderQty,OrderAmt," & _
           " Remark,Case when  DispatchStatus='C' and IsConfirm='Y' then 'Dispatched' when DispatchStatus<>'C' and Isconfirm='Y' " & _
            " then 'Pending' when IsConfirm='R' then 'Rejected' end as Dispatchstatus , " & _
           " Case when OrderType='O' then 'Repurchase' else 'Joining' end as Status  from Trnorder  where 1=1 " & condition & " Order by OrderDate Desc "


            dtTemp = New DataTable
            dtTemp = ObjDal.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("OrderDetail.xls", dg)

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

    Protected Sub txtMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberId.TextChanged
        GetFormNo()
    End Sub
End Class
