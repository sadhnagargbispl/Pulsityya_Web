Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_AdminIndex
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" And Session("UserPermission") = 1 Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindData()
        End If
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        Dim sql As String = "Select A.IDNo,A.MemFirstName As MemName,dbo.formatDate(A.Doj,'dd-MMM-yyyy') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo,A.Passw,C.IDNo As RefIDNo," & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" class=""fancybox fancybox.iframe"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr," & _
" Case When A.ActiveStatus='Y' then 'Active' Else 'Pending' End As Status,CASE WHEN a.ActiveStatus='Y' THEN ISNULL(Replace(Convert(varchar,A.UpgradeDate,106),' ','-'),'') ELSE '' END as UpgrdDate,D.KitName,'Rs 0.00/-' As Balance From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("MemberData") = dtData
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MemberData")
        GvData.DataBind()
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""
        If ddlSearch.SelectedValue = "0" Then
            Exit Sub
        ElseIf ddlSearch.SelectedValue = "StateName" Then
            Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "DOJ" Then
            Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "MemName" Then
            Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "RMemName" Then
            Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "KitName" Then
            Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "RMemID" Then
            Condition = " Where c.IDNO = '" & txtSrchText.Text & "' "
        ElseIf ddlSearch.SelectedValue = "IDNo" Then
            Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
        Else
            Condition = " Where a." & ddlSearch.SelectedValue & " like '%" & txtSrchText.Text & "%'"
        End If
        BindData(Condition)
    End Sub

    'Protected Sub SetMenus(ByVal sender As Object, ByVal e As System.EventArgs)

    '    'myMenu.InnerHtml = "<li class=""edit""><a href=""#Account"">View Account</a></li>" & _
    '    '"<li class=""cut separator""><a href=""#Profile"">Update Profile</a></li>" & _
    '    '"<li class=""cut separator""><a href=""#Tree"">View Tree</a></li>" & _
    '    '"<li class=""paste separator""><a href=""#Activate"">Activate</a></li>" & _
    '    '"<li class=""paste separator""><a href=""#Tax"">Print Tax Invoice</a></li>" & _
    '    '"<li class=""paste separator""><a href=""#Distr"">Become Distributor</a></li>" & _
    '    '"<li class=""paste separator""><a href=""#Cour"">Courier Detail</a></li>" & _
    '    '"<li class=""delete separator""><a href=""#SMS"">Send SMS</a></li>" & _
    '    '"<li class=""quit separator""><a href=""#Block"">Block Now</a></li>" & _
    '    '"<li class=""quit separator""><a href=""#BlockTree"">Block Tree</a></li>" & _
    '    '"<li class=""quit separator""><a href=""#UnBlock"">Unblock</a></li>" & _
    '    '"<li class=""quit separator""><a href=""#UnblockTree"">Unblock Tree</a></li>" & _
    '    '"<li class=""quit separator""><a href=""#Reward"">Reward Status</a></li>" & _
    '    '"<li class=""quit separator""><a href=""#Incentive"">Incentive Detail</a></li>" & _
    '    '"<li class=""quit separator""><a href=""#Status"">Complete Status</a></li>"
    'End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound

    End Sub
End Class
