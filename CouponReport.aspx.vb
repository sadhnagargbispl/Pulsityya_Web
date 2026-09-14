Imports System.Data
Imports System.Data.SqlClient
Partial Class CouponReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim conn As New SqlConnection
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                btnExport.Enabled = False
            Else
                Response.Redirect("logout.aspx")
            End If
        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("CouponDetail")
        GvData.DataBind()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillData()
    End Sub
    Protected Sub FillData()
        Dim formno As String = "0"
        Dim CouponCode As String = "0"
        Dim startDate As String = ""
        Dim endDate As String = ""
        Dim currentDate As DateTime = DateTime.Now
        Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")
        If txtFromDate.Text = "" Then
            startDate = "12-oct-2017"
        Else
            startDate = txtFromDate.Text
        End If
        If TxtToDate.Text = "" Then
            endDate = formattedDate
        Else
            endDate = TxtToDate.Text
        End If
        If TxtCouponCode.Text <> "" Then
            CouponCode = TxtCouponCode.Text
        End If
        Dim qry1 As String = ""
        If txtMemId.Text <> "" Then
            formno = GetFormNo()
        End If
        qry1 = " Exec Sp_GetCouponDetailAdmin '" & formno & "','" & startDate & "','" & endDate & "','N','" & CouponCode & "'"
        dtData = New DataTable
        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, qry1).tables(0)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("CouponDetail") = dtData
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String = ""
        Dim formno As String = ""
        idNo = txtMemId.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo = '" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
            lblErr.Visible = False
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemId.Text = ""
        End If
        Return formno
    End Function
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim dtTemp As New DataTable
        Dim dg As New DataGrid
        Dim formno As String = "0"
        Dim CouponCode As String = "0"
        Dim startDate As String = ""
        Dim endDate As String = ""
        Dim currentDate As DateTime = DateTime.Now
        Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")
        If txtFromDate.Text = "" Then
            startDate = "12-oct-2017"
        Else
            startDate = txtFromDate.Text
        End If
        If TxtToDate.Text = "" Then
            endDate = formattedDate
        Else
            endDate = TxtToDate.Text
        End If
        If TxtCouponCode.Text <> "" Then
            CouponCode = TxtCouponCode.Text
        End If
        Dim qry1 As String = ""
        If txtMemId.Text <> "" Then
            formno = GetFormNo()
        End If
        qry1 = " Exec Sp_GetCouponDetailAdmin '" & formno & "','" & startDate & "','" & endDate & "','Y','" & CouponCode & "'"
        dtTemp = New DataTable
        dtTemp = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, qry1).tables(0)
        dg.DataSource = dtTemp
        dg.DataBind()
        ExportToExcel("CouponReport.xls", dg)

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
End Class
