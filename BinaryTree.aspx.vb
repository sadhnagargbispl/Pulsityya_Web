Imports System.Data

Partial Class BinaryTree
    Inherits System.Web.UI.Page
    Dim objDal As DAL
    Dim objGen As clsGeneral = New clsGeneral

    'Protected Sub Reset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Reset.Click
    '    TreeFrame.Attributes.Item("src") = "NewTree.aspx?DownLineFormNo=" & Val(Session("FormNo")).ToString
    'End Sub
    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    TreeFrame.Attributes.Item("src") = "NewTree.aspx?DownLineFormNo=" & DownLineFormNo.Value
    'End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        ' Response.Redirect("cpindex.aspx")
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'Dim formno As String
        'formno = GetFormNo()
        'Dim depthlevel As String = txtDeptlevel.Text
        'TreeFrame.Attributes.Item("src") = "NewTree.aspx?DownLineFormNo=" & Val(formno.ToString()) & "&deptlevel=" & Val(depthlevel.ToString())
        ShowTree(Val(txtDeptlevel.Text))
    End Sub
    Protected Sub BtnStepAbove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnStepAbove.Click
        Dim scrname As String = ""
        If Not Session("Upliner") Is Nothing Then
            Dim uplnformno As String = Session("Upliner")
            If uplnformno = 0 Then
                scrname = "<SCRIPT language='javascript'>alert('No Upliner Id !! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Incorrect", scrname, False)
                Exit Sub
            Else
                Response.Redirect("NewTree.aspx?DownLineFormNo=" & uplnformno)
                'TreeFrame.Attributes.Item("src") = "Newtree.aspx?DownLineFormNo=" & uplnformno & "&deptlevel=" & Val(4)
            End If

        End If

    End Sub
    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        Dim UplnFormno As String = ""

        idNo = txtDownLineFormNo.Text
        Dim qry As String = "Select FormNo,UplnFormno from " & objDal.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDal.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
            UplnFormno = dt.Rows(0)("UplnFormNo")
            If UplnFormno <> 1 Then
                Session("Upliner") = UplnFormno
            End If
        End If
        Return formno
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") <> "OK" Then
            Response.Redirect("Default.aspx")
        Else
            Session("PageName") = "Member / Member Tree"

        End If
        If Request.QueryString.HasKeys Then
            If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                If Not Page.IsPostBack Then
                    txtDownLineFormNo.Text = Request.QueryString("key").ToString
                    ShowTree(6)
                End If
            End If
        End If
    End Sub

    Private Sub ShowTree(ByVal Level As String)
        Dim formno As String
        formno = GetFormNo()
        Dim scrname As String = ""
        If formno <> "" Then

            Response.Redirect("NewTree.aspx?DownLineFormNo=" & Val(formno.ToString()) & "&deptlevel=" & Val(Level.ToString()))
            'TreeFrame.Attributes.Item("src") = "NewTree.aspx?DownLineFormNo=" & Val(formno.ToString()) & "&deptlevel=" & Val(Level.ToString())
        Else
            scrname = "<SCRIPT language='javascript'>alert('Enter correct Member Id !! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Incorrect", scrname, False)
        End If
    End Sub
End Class
