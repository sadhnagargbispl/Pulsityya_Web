Imports System.Data
Imports System.Data.SqlClient
Partial Class Reward_SP
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim conn As New SqlConnection
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("RewardList")
        GvData.DataBind()
    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillData()
    End Sub
    Protected Sub FillData()
        Try
            Dim formno As String = "0"
            If Chkmemid.Checked Then
                formno = GetFormNo()
            End If

            Dim qry1 As String = ""

            qry1 = "Exec SP_RewardReward '" & formno & "','" & ddllist.SelectedValue & "', '" & txtFromDate.Text & "','" & TxtToDate.Text & "'"
            dtData = New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dtData = objDAL.GetData(qry1)

            GvData.DataSource = dtData
            GvData.DataBind()
            Session("RewardList") = dtData
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim formno As String
        Dim qry As String = "Exec Sp_GetFormno '" & txtMemId.Text & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemId.Text = ""
        End If
        Return formno
    End Function

    Private Sub FillReward()
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim S As String = ""
        S = "Exec Sp_FillRewardDropwown"
        Dim dt As New DataTable
        dt = objDAL.GetData(S)
        ddllist.DataSource = dt
        ddllist.DataTextField = "Reward"
        ddllist.DataValueField = "rewardid"
        ddllist.DataBind()
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                FillReward()
            Else
                Response.Redirect("logout.aspx")
            End If
        End If
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""

            If Chkmemid.Checked Then
                formno = GetFormNo()
            End If
            Dim qry1 As String = ""
            qry1 = "Exec SP_RewardReward '" & formno & "','" & ddllist.SelectedValue & "', '" & txtFromDate.Text & "','" & TxtToDate.Text & "'"
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("Reward.xls", dg)

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



End Class
