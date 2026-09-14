Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddUser
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim UserIdQS As String
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
        If String.IsNullOrEmpty(Request("UserId")) = False Then
            UserIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("UserId")))
        End If
        If Not Page.IsPostBack Then
            ClearAll()

            ' Setting up values for GroupId and IP Address
            txtGrpID.Text = Session("GroupId")
            txtGrpName.Text = Session("GroupName")
            ''txtPswd.Attributes.Add("value", "Password")
            'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
            txtIPAdrs.Text = txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
            If String.IsNullOrEmpty(Request("UserId")) = False Then
                BtnSave.Text = "Modify"
                BindData()
            End If
        End If

        ' Setting up values for GroupId and IP Address
        '    txtGrpID.Text = Session("grpID")
        '    txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select a.*,b.GroupName From " + objDAL.tblUserMaster + " as a,M_UserGroupMaster as b" & _
        " Where a.GroupId=b.GroupId and a.UserId='" & UserIdQS & "' AND a.RowStatus='Y' and b.activeStatus='Y' and b.RowStatus='Y' "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtGrpID.Text = Dt.Rows(0)("GroupId")
            txtGrpName.Text = Dt.Rows(0)("GroupName")

            txtUsrName.Text = Dt.Rows(0)("UserName")

            txtPswd.Text = Dt.Rows(0)("Passw")
            txtPswd.Attributes.Add("value", Dt.Rows(0)("Passw"))
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            TxtMobileNo.Text = Dt.Rows(0)("MobileNo")
            txtUserID.Text = Dt.Rows(0)("UserId")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
            'If Val(Session("GroupId")) <> 1 Then
            '    txtUsrName.Enabled = False
            '    TxtMobileNo.Enabled = False
            '    txtRemarks.Enabled = False
            '    txtUsrName.Enabled = False
            'Else
            '    txtUsrName.Enabled = True
            '    TxtMobileNo.Enabled = True
            '    txtRemarks.Enabled = True
            '    txtUsrName.Enabled = True

            'End If
        End If
    End Sub

    Private Sub ClearAll()
        txtUsrName.Text = ""
        txtUserID.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        txtGrpID.Text = ""
        TxtMobileNo.Text = ""
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        If checkuser() Then


            If rdblist.SelectedIndex = 0 Then
                txtActiveStatus.Text = "Y"
            Else
                txtActiveStatus.Text = "N"
            End If
            If String.IsNullOrEmpty(Request("UserId")) = False Then
                Sql = "Update " & objDAL.tblUserMaster & " SET RowStatus='N' Where UserId='" & UserIdQS & "';"
                Sql = Sql & " Insert into " & objDAL.tblUserMaster + "(GroupId,UserId,UserName,Passw,Remarks,ActiveStatus,LastModified,UserCode,UsrId,IPAdrs,RowStatus,MobileNo)" & _
                " Values('" & Val(txtGrpID.Text) & "','" & txtUserID.Text & "','" & txtUsrName.Text & "','" & txtPswd.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "'," & _
                " 'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & TxtMobileNo.Text & "')"
            Else

                Sql = " Insert into " & objDAL.tblUserMaster + "(GroupId,UserId,UserName,Passw,Remarks,ActiveStatus,LastModified,UserCode,UsrId,IPAdrs,RowStatus,MobileNo) " & _
          " Select '" & Val(txtGrpID.Text) & "',Case When Max(UserId) Is Null Then '1' Else Max(UserId)+1 END as UserId,'" & txtUsrName.Text & "','" & txtPswd.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & TxtMobileNo.Text & "' From " & objDAL.tblUserMaster

            End If

            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Sql)

            If String.IsNullOrEmpty(Request("UserId")) = False And updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
            ElseIf updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
            End If

            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        Else
            scrname = "<SCRIPT language='javascript'>alert('User Name Already Exist!!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        End If

    End Sub
    Private Function checkuser() As Boolean
        Dim sql As String = ""
        If (Request("UserId")) = "" Then
            sql = "select * from M_UserMaster where UserName='" & txtUsrName.Text.Trim & "' and activeStatus='Y' and RowStatus='Y'"
        Else
            sql = "select * from M_UserMaster where UserName='" & txtUsrName.Text.Trim & "' and activeStatus='Y' and RowStatus='Y' and Userid <>'" & Val(UserIdQS) & "'"
        End If



        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub txtUsrName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUsrName.TextChanged
        If checkuser() = True Then
        Else
            scrname = "<SCRIPT language='javascript'>alert('User Name Already Exist!!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        End If
    End Sub
End Class
