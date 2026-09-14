Imports System.Data
Partial Class App_UI_Application_Pages_UserIdUpdate
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then
                    Session("PageName") = "Member / Update User ID "
                Else
                    Response.Redirect("Logout.aspx")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

   

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Try
            Dim dt As DataTable = New DataTable()
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = "Select Formno,IDno,MemfirstName From M_MemberMaster Where UPPER(IDno) = '" & TxtIDNo.Text.Trim().ToUpper() & "'"
            dt = obj.GetData(str)
            If (dt.Rows.Count > 0) Then
                TxtIDNo.Text = dt.Rows(0)("IDno").ToString()
                hdnFormno.Value = dt.Rows(0)("Formno").ToString()
                LblMemName.Text = dt.Rows(0)("MemfirstName").ToString()
            Else
                TxtIDNo.Text = ""
                hdnFormno.Value = ""
                LblMemName.Text = ""
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('User Id Invaild.!!!');", True)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub txtNewUserID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewUserID.TextChanged
        Try
            Dim dt As DataTable = New DataTable()
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = "Select Formno,IDno,MemfirstName From M_MemberMaster Where UPPER(IDno) = '" & txtNewUserID.Text.Trim().ToUpper() & "'"
            dt = obj.GetData(str)
            If (dt.Rows.Count > 0) Then
                txtNewUserID.Text = ""
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('User Id already exists.!!');", True)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnLegShift_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLegShift.Click
        Try

            Dim dt As DataTable = New DataTable()
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str1 As String = "Select Formno,IDno,MemfirstName From M_MemberMaster Where UPPER(IDno) = '" & TxtIDNo.Text.Trim().ToUpper() & "'"
            dt = obj.GetData(str1)
            If (dt.Rows.Count > 0) Then
                TxtIDNo.Text = dt.Rows(0)("IDno").ToString()
                hdnFormno.Value = dt.Rows(0)("Formno").ToString()
                LblMemName.Text = dt.Rows(0)("MemfirstName").ToString()
            Else
                TxtIDNo.Text = ""
                hdnFormno.Value = ""
                LblMemName.Text = ""
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('User Id Invaild.!!!');", True)
                Exit Sub
            End If


            Dim dt1 As DataTable = New DataTable()
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str12 As String = "Select Formno,IDno,MemfirstName From M_MemberMaster Where UPPER(IDno) = '" & txtNewUserID.Text.Trim().ToUpper() & "'"
            dt1 = obj.GetData(str12)
            If (dt1.Rows.Count > 0) Then
                txtNewUserID.Text = ""
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('User Id already exists.!!!');", True)
                Exit Sub
            End If

            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = "Update M_MemberMAster Set Idno = '" & txtNewUserID.Text.Trim() & "' Where Formno = '" & hdnFormno.Value & "';"
            str &= " insert Into UserIdUpdateHistory(Formno,OLdIdno,NewIDno,ModifiedBY)"
            str &= " Values('" & hdnFormno.Value & "','" & TxtIDNo.Text.Trim() & "','" & txtNewUserID.Text.Trim() & "','" & Val(Session("UserID")) & "')"
            Dim x As Integer = obj.SaveData(str)
            If (x > 0) Then
                TxtIDNo.Text = ""
                hdnFormno.Value = ""
                LblMemName.Text = ""
                txtNewUserID.Text = ""
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('User Id Update Successfully.!!!');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('User Id Invaild.!!!');", True)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
