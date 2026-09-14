Imports System.Data
Imports System.Net
Imports System.IO
Partial Class WithdrawalStatus
    Inherits System.Web.UI.Page

    Dim objDAL As DAL
    Dim Sql As String = ""

    Dim scrname As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            Session("PageName") = " user /Withdrawal Status  "
            If Not Page.IsPostBack Then
                GetStatus()
            End If
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub

    Private Sub GetStatus()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim dt As DataTable = New DataTable()
            Dim Str As String = " select Case When IsWithDra = 'Y' Then 'Kyc Active' else 'Kyc Deactive' End As Status  from m_KycPer"
            dt = objDAL.GetData(Str)
            If (dt.Rows.Count > 0) Then
                lblStatus.Text = dt.Rows(0)("Status").ToString()
            End If

        Catch ex As Exception

        End Try
    End Sub



    Protected Sub Btnsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnsubmit.Click
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim Str As String = ""
        Str = " Exec  Sp_EditKycPer '" & Rbtstatus.SelectedValue & "','" & Session("UserID") & "','" & Session("UserName") & "' ;"
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Str)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('UnSuccessfully! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
        GetStatus()

    End Sub
End Class
