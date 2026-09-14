Imports System.Data

Partial Class App_UI_Application_Pages_ChangePassword
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim BankCodeQS As String
    Dim sql As String
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        ' txtIPAddress.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAddress.Text = objModuleFun.GetVisitorIPAddress()
        If Not Page.IsPostBack Then
            lblMsg.Visible = False
            txtNewPswd.Text = ""
            txtOldPaswd.Text = ""
            txtReNewPswd.Text = ""
        End If
    End Sub

    Protected Sub btnchngpswd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchngpswd.Click
        Dim userId As Integer = Val(Session("UserID"))
        If String.Equals(txtNewPswd.Text, txtReNewPswd.Text) = False Then
            lblMsg.Text = "New Password does not match. Please enter it again."
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Red
            txtNewPswd.Text = ""
            txtOldPaswd.Text = ""
            txtReNewPswd.Text = ""
        Else
            Dim qry As String = "Select * from " & objDAL.tblUserMaster & " where UserId='" & userId & "' AND ActiveStatus='Y' AND " & objDAL.activeCondition
            Dt = New DataTable
            Dt = objDAL.GetData(qry)
            If Dt.Rows.Count > 0 Then
                If String.Equals(Dt.Rows(0)("passw").ToString(), txtOldPaswd.Text) = False Then
                    lblMsg.Text = "Old Password does not match. Please enter it again."
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    txtNewPswd.Text = ""
                    txtOldPaswd.Text = ""
                    txtReNewPswd.Text = ""
                Else
                    txtGrpID.Text = Dt.Rows(0)("GroupId")
                    txtusername.Text = Dt.Rows(0)("UserName")
                    txtRemarks.Text = Dt.Rows(0)("Remarks")
                    sql = "Update " & objDAL.tblUserMaster & " SET RowStatus='N' Where UserId='" & userId & "';"
                    sql = sql & " Insert into " & objDAL.tblUserMaster + "(GroupId,UserId,UserName,Passw,Remarks,ActiveStatus,LastModified,UserCode,UsrId,IPAdrs,RowStatus) Values('" & Val(txtGrpID.Text) & "','" & userId & "','" & txtusername.Text & "','" & txtNewPswd.Text & "','" & txtRemarks.Text & "','Y','Password changed by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAddress.Text & "','Y')"
                    Dim a As Integer = objDAL.UpdateData(sql)
                    If a <> 0 Then
                        lblMsg.Text = "Password Changed successfully."
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Drawing.Color.Green
                    End If
                End If
            End If
        End If
    End Sub
End Class
