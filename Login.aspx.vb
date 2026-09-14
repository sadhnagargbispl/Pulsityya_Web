Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports System.Data

Partial Class Login
    Inherits System.Web.UI.Page
    Dim uid As String
    Dim Pwd As String
    Dim strScript As String
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'BtnLogin.Attributes.Add("OnClick", " return validate();")
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If Not Page.IsPostBack Then
            uid = Request("uid")
            Pwd = Request("pwd")
            uid = Replace(Replace(Replace(Trim(uid), "'", ""), "=", ""), ";", "")
            Pwd = Replace(Replace(Replace(Trim(Pwd), "'", ""), "=", ""), ";", "")
            If ((uid <> Nothing) And (Pwd <> Nothing)) Then
                enterHomePg()
            End If
        End If
    End Sub

    Private Sub enterHomePg()
        If Len(uid) > 0 And Len(Pwd) > 0 Then
            Dim qry As String = "Select a.* from " & objDAL.tblUserMaster & " as a where UserName='" & uid & "' and Passw='" & Pwd & "' AND ActiveStatus='Y' AND " & objDAL.activeCondition
            dtData = New DataTable
            dtData = objDAL.GetData(qry)
            If dtData.Rows.Count = 0 Then
                strScript = "<script language='javascript'>alert('Please Enter valid UserName or Password.');</script>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                Response.Redirect("Default.aspx")
            Else
                Session("AStatus") = "OK"
                Session("UserID") = dtData.Rows(0)("UserId")
                Session("UserName") = dtData.Rows(0)("UserName")
                Session("GroupID") = dtData.Rows(0)("GroupId")
                Session("AdminPassw") = dtData.Rows(0)("Passw")

                'Update Login time and status
                Dim sql As String = "Update " & objDAL.tblUserMaster & " set LastLoginTime='" & DateAndTime.Now.ToString() & "',LoginStatus='Y' where UserName='" & uid & "' and Passw='" & Pwd & "' AND " + objDAL.activeCondition
                Dim a As Integer = objDAL.UpdateData(sql)

                'Setting up session variables
                Session("grpID") = ""
                Session("grdIndex") = 0
                Session("UserPermission") = 1
                'Redirect to home page
                Dim adminHome As String = "Home.aspx"
                Response.Redirect(adminHome)
            End If
lblError:
            Response.Write(Err.Description)
        End If
    End Sub

End Class
