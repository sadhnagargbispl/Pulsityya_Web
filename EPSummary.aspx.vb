Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_EPSummary
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
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

            Dim sql As String = String.Empty
            sql = " Exec Sp_EPSummary "
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            GvData.DataSource = dt
            GvData.DataBind()

            Session("GData") = dt
        Catch ex As Exception

        End Try
    End Sub

 
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex

        FillReport()

    End Sub


 

End Class


