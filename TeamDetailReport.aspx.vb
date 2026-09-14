Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class TeamDetailReport
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
                'Session("PageName") = "Member / Update Member Profile"
                '' UserStatus.InnerHtml = "<p>Welcome " & Session("MemName") & "(" & Session("FormNo") & ") To" & Session("CompName") & " </p>"
            Else
                Response.Redirect("logout.aspx")
            End If
            If Not Page.IsPostBack Then
                'FillReport()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillReport()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String = String.Empty
            sql = " exec Sp_Report '" & txtMember.Text.ToString().Trim() & "','" & txtFromDate.Text.ToString().Trim() & "','" & TxtToDate.Text.ToString().Trim() & "'"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            If (dt.Rows.Count > 0) Then
                GvData.DataSource = dt
                GvData.DataBind()
            Else
                GvData.DataSource = dt
                GvData.DataBind()
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('No record found.!!');", True)
            End If
        Catch ex As Exception

        End Try
    End Sub











    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            FillReport()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            '     Dim condition As String = ""
        
         
            Dim qry1 As String = ""
            qry1 = "exec Sp_Report '" & txtMember.Text.ToString().Trim() & "','" & txtFromDate.Text.ToString().Trim() & "','" & TxtToDate.Text.ToString().Trim() & "'"
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("TeamDetailreport.xls", dg)

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


    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        Try
            GvData.PageIndex = e.NewPageIndex
            FillReport()
        Catch ex As Exception

        End Try
    End Sub
End Class
