
Partial Class MainMaster
    Inherits System.Web.UI.MasterPage
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

        End If
    End Sub
End Class

