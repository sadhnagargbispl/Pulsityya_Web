Imports System.Data

Partial Class ClubTree
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
        Response.Redirect("Home.aspx")
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim formno As String
        formno = GetFormNo()
        Dim depthlevel As String = txtDeptlevel.Text
        BindData(formno)
        TreeFrame.Attributes.Item("src") = "ClubRefTree.aspx?DownLineFormNo=" & Val(formno.ToString()) & "&deptlevel=" & Val(depthlevel.ToString())
    End Sub

    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtDownLineFormNo.Text
        Dim qry As String = "Select FormNo from " & objDal.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDal.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        End If
        Return formno
    End Function

    Protected Sub BindData(ByVal Formno As String)
        Dim str As String
        Dim obj As DAL
        Dim Dt As DataTable
        Dt = New DataTable
        obj = New DAL((HttpContext.Current.Session("MlmDatabase" & Session("CompID"))))
        ' str = "Select * From v#PoolChart Where FormNo='" & Formno & "'Order by MLEvel "
        str = "Select a.*,Case when a.RemainId=0 then Replace(Convert(Varchar,b.LevelDate,106),' ','-') else '' end as LevelDate " & _
           " From v#PoolChart as a, V#PoolLevel as b Where a.FormNo=b.FormNo and a.Mlevel=b.Mlevel And a.Formno='" & Formno & "' Order by mlevel"

        Dt = obj.GetData(str)
        Gvdata.DataSource = Dt
        Gvdata.DataBind()
        Session("TreeData") = Dt
    End Sub

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
                    Dim formno As String
                    formno = GetFormNo()
                    If formno <> "" Then
                        Dim level As String
                        level = 4
                        TreeFrame.Attributes.Item("src") = "ClubRefTree.aspx?DownLineFormNo=" & Val(formno.ToString()) & "&deptlevel=" & Val(level.ToString())
                    End If
                End If
            End If
        End If

    End Sub
End Class
