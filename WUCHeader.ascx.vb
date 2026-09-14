Imports System.Data

Partial Class WUCHeader
    Inherits System.Web.UI.UserControl
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'LblUserName.Text = "Welcome " & Session("UserName")
        'BindData()
        ' Image2.ImageUrl = Session("Logo").ToString()
        'Image2.Src = Session("Logo").ToString()
    End Sub

    'Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
    '    Dim sql As String = ""
    '    If String.IsNullOrEmpty(txtSearch.Text) Then
    '        sql = "select * from M_CustomerMaster where Custname='" & txtSearch.Text & ""

    '    End If
    'End Sub
    Protected Sub BindData()
        Dim sql As String = ""
        Dim dt As DataTable
        Dim obj As DAL
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dt = New DataTable
        Dim str As String = "select Count(*) as totalmember from M_MemberMaster"
        dt = obj.GetData(str)
        If dt.Rows.Count > 0 Then
            'LblRegCustomer.Text = dt.Rows(0)("totalmember")
        End If

        'LblCount.Text = Val(LblRegCustomer.Text)

    End Sub
    'Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
    '    Session("Search") = txtSearch.Text
    '    Response.Redirect("MemberProfile.aspx")
    'End Sub
End Class
