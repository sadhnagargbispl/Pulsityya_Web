Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class OfferDetails
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    'Dim objDAL As New DAL
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'objDAL = New DAL()
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            ' lblView.Visible = False
            If Session("AStatus") = "OK" Then
                BindData(Convert.ToInt32(Request.QueryString("id")))
            End If
        End If
    End Sub
    Private Sub BindData(ByVal OfferId As Integer)
        Try
            Dim str As String = " exec Sp_MemberOfferDetail " & OfferId & " "
            Dim dt As DataTable = New DataTable()
            dt = objDAL.GetData(str)
            If (dt.Rows.Count > 0) Then
                gv.DataSource = dt
                gv.DataBind()
            Else
                gv.DataSource = dt
                gv.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
