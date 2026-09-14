Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class App_UI_Application_Pages_UserPermission
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "User / User Permission"
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            lblMsg.Visible = False
            ' Fill Group dropdown
            Dim qry1 As String = " Select * from M_UserGroupMaster Where ActiveStatus='Y' AND RowStatus='Y' order by groupid "
            objModuleFun.FillCombo(qry1, ddlGroup, "GroupName", "GroupId")
            ' Fill grid with menu
            BindData()
            ' NOTE: Login user ka Session("GroupId") ko chhedna NAHI hai
            ' (WUCMenu sidebar usi se banta hai). Permission page ke liye alag session:
            Session("SelectedPermGroupId") = ddlGroup.SelectedValue.ToString()
            Session("SelectedPermGroupName") = ddlGroup.SelectedItem.Text
        End If
    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '    objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

    '    If Not Page.IsPostBack Then
    '        lblMsg.Visible = False

    '        ' Fill Group dropdown
    '        Dim qry1 As String = "Select * from " & objDAL.tblUserGrpMaster & " Where ActiveStatus='Y' AND " & objDAL.activeCondition
    '        objModuleFun.FillCombo(qry1, ddlGroup, "GroupName", "GroupId")

    '        ' Fill grid with menu
    '        BindData()

    '        Session("GroupId") = ddlGroup.SelectedValue.ToString()
    '        Session("GroupName") = ddlGroup.SelectedItem.Text
    '        Session("grpID") = ddlGroup.SelectedValue.ToString()
    '    End If
    'End Sub
    Public Sub BindData()
        dtData = New DataTable

        ' Pehle Session se try karo (WUCMenu ne bhara hoga)
        If Session("Menu") IsNot Nothing Then
            dtData = CType(Session("Menu"), DataTable)
        Else
            ' Fallback: seedha table se
            Dim qry As String = "Select MenuId, MenuName, ParentId, Hierar " & _
                                "from M_CompWiseWebMenuMaster " & _
                                "where ActiveStatus='Y' AND RowStatus='Y' " & _
                                "AND CompanyID='" & Session("CompID") & "' " & _
                                "order by Hierar, MenuId"
            dtData = objDAL.GetData(qry)
        End If

        GvData.DataSource = dtData
        GvData.DataBind()

        Session("GData") = dtData
        SetCheckBoxValue()
    End Sub
    ' ===== Menu list (M_CompWiseWebMenuMaster se, company-wise) =====
    'Public Sub BindData()
    '    Dim qry As String = "Select MenuId, MenuName, ParentId, Hierar " & _
    '                        "from M_CompWiseWebMenuMaster " & _
    '                        "where ActiveStatus='Y' AND RowStatus='Y' " & _
    '                        "order by Hierar, MenuId"

    '    dtData = New DataTable
    '    dtData = objDAL.GetData(qry)

    '    GvData.DataSource = dtData
    '    GvData.DataBind()

    '    Session("GData") = dtData
    '    SetCheckBoxValue()
    'End Sub

    Protected Sub btnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow.Click
        BindData()
        lblMsg.Visible = False
    End Sub

    ' ===== Save permission (M_UserPermissionMaster) =====
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ' Pehle is group ki purani permission RowStatus='N' kar do
        Dim qry As String = "Update M_UserPermissionMaster set RowStatus='N' " & _
                            "where GroupId='" & Val(ddlGroup.SelectedValue.ToString()) & "';"

        Dim Chk As CheckBox
        Dim Lblmenu As Label

        If GvData.Rows.Count > 0 Then
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkMenuPermission"), CheckBox)
                Lblmenu = DirectCast(Gvr.FindControl("lblMenuId"), Label)

                If Chk.Checked = True Then
                    qry = qry & " insert into M_UserPermissionMaster(GroupId, MenuId, RowStatus, ActiveStatus) " & _
                                "values('" & Val(ddlGroup.SelectedValue.ToString()) & "','" & Val(Lblmenu.Text) & "','Y','Y');"
                End If
            Next

            Dim a As Integer = objDAL.UpdateData(qry)
            If a <> 0 Then
                lblMsg.Text = "Permission set for the selected group successfully."
                lblMsg.Visible = True
                lblMsg.ForeColor = Drawing.Color.Green
            Else
                lblMsg.Text = "Permission not set for the selected group"
                lblMsg.Visible = True
                lblMsg.ForeColor = Drawing.Color.Red
            End If
        End If

        BindData()
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
        Session("grdIndex") = GvData.PageIndex
        SetCheckBoxValue()
    End Sub

    ' ===== Selected group ki saved permission ke hisaab se checkbox tick =====
    Private Sub SetCheckBoxValue()
        Dim strlist As New List(Of String)
        Dim dtPermission As New DataTable

        Dim qry As String = "Select MenuId from M_UserPermissionMaster " & _
                            "where RowStatus='Y' AND ActiveStatus='Y' " & _
                            "AND GroupId='" & Val(ddlGroup.SelectedValue.ToString()) & "' " & _
                            "order by MenuId"

        dtPermission = objDAL.GetData(qry)

        For Each row As DataRow In dtPermission.Rows
            strlist.Add(row("MenuId").ToString())
        Next

        Dim Chk As CheckBox
        Dim Lbl As Label
        If GvData.Rows.Count > 0 Then
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkMenuPermission"), CheckBox)
                Lbl = DirectCast(Gvr.FindControl("lblMenuId"), Label)
                If strlist.Contains(Lbl.Text) = True Then
                    Chk.Checked = True
                Else
                    Chk.Checked = False
                End If
            Next
        End If
    End Sub

    ' ===== Parent menu row highlight =====
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblParent As Label = DirectCast(e.Row.FindControl("lblParentId"), Label)
            If lblParent IsNot Nothing AndAlso Val(lblParent.Text) = 0 Then
                ' Parent row
                e.Row.CssClass = "parent-row"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "Black"
                Next
            Else
                e.Row.CssClass = "child-row"
            End If
        End If
    End Sub

End Class