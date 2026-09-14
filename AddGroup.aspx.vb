Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddGroup
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim GroupIdQS As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("GroupId")) = False Then
            GroupIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("GroupId")))
        End If
        If Not Page.IsPostBack Then
            clearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("GroupId")) = False Then
                    BtnSave.Text = "Modify"
                    BindData()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From " + objDAL.tblUserGrpMaster + " Where GroupId='" & GroupIdQS & "' AND rowStatus='Y'"
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtGrpName.Text = Dt.Rows(0)("GroupName")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtGrpID.Text = Dt.Rows(0)("GroupId")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub
    Private Function checkuser() As Boolean
        Dim sql As String = ""
        If (Request("GroupId")) = "" Then
            sql = "select * from M_UserGroupMaster where GroupName='" & txtGrpName.Text.Trim & "' and activeStatus='Y' and RowStatus='Y'"
        Else
            sql = "select * from M_UserGroupMaster where GroupName='" & txtGrpName.Text.Trim & "' and activeStatus='Y' and RowStatus='Y' and GroupId <>'" & Val(GroupIdQS) & "'"
        End If

        ''objDAL = New DAL
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count = 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
        If checkuser() Then


            If String.IsNullOrEmpty(Request("GroupId")) = False Then
                Sql = "Update " & objDAL.tblUserGrpMaster & " SET RowStatus='N' Where GroupId='" & GroupIdQS & "';"
                Sql = Sql & " Insert into " & objDAL.tblUserGrpMaster + "(GroupId,GroupName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) Values('" & Val(txtGrpID.Text) & "','" & txtGrpName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y')"
                'Qry = Qry & "insert into " & objDAL.tblUserGrpMaster & " (GroupId,GroupName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) values('" & ddlUselectGroup.SelectedValue.ToString() & "','" & txtRGName.Text & "','" & txtURemarks.Text & "','" & status & "','Updated Existing data','" & txtUserCode.Text & "','" & Val(txtUserId.Text) & "','" & txtIPAdrs.Text & "','Y') "
            Else
                Sql = "Insert into " + objDAL.tblUserGrpMaster + "(GroupId,GroupName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) Select Case When Max(GroupId) Is Null Then '1' Else Max(GroupId)+1 END as GroupId,'" & txtGrpName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y' From " & objDAL.tblUserGrpMaster
            End If

            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Sql)

            If String.IsNullOrEmpty(Request("GroupId")) = False And updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
            ElseIf updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
            End If

            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        Else
            scrname = "<SCRIPT language='javascript'>alert('Group Already Exist!! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        End If

    End Sub


    Private Sub ClearAll()
        txtGrpName.Text = ""
        txtGrpID.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
    End Sub

    Protected Sub txtGrpName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrpName.TextChanged
        If checkuser() = True Then
        Else
            scrname = "<SCRIPT language='javascript'>alert('Group Name Already Exist!!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        End If
    End Sub
End Class
