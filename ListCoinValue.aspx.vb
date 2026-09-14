Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class ListCoinValue
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
                FillReport()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillReport()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String = String.Empty
            sql = " Sp_GetCoinMAster 0,'GetAll'"
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











End Class
