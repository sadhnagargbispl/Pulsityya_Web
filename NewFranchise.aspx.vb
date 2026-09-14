Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class NewFranchise
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
                'Session("PageName") = "Member / Update Member Profile"
                '' UserStatus.InnerHtml = "<p>Welcome " & Session("MemName") & "(" & Session("FormNo") & ") To" & Session("CompName") & " </p>"

            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then
                ' FillCityPinDetail()

            End If


        Catch ex As Exception

        End Try
    End Sub




    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            Dim sql As String = String.Empty
            Dim scrname As String = String.Empty
            Dim x As Integer = 0
            sql = " Insert Into M_Makefranchise (IDno,FormNo,Name,Remark,Rectimestamp,ActiveStatus,FranchiseDate) "
            sql &= " Values('" & txtMemberId.Text.Trim() & "','" & hdnFormno.Value & "','" & lblMemberName.Text.Trim & "',"
            sql &= " '" & TxtRemark.Text.Trim() & "',Getdate(),'Y',Getdate());"
            sql &= " Update  M_MemberMaster  Set  Fld5 = 'Y' Where  Formno = '" & hdnFormno.Value & "' "

            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            x = objDAL.SaveData(sql)
            If (x > 0) Then
                txtMemberId.Text = String.Empty
                lblMemberName.Text = String.Empty
                hdnFormno.Value = ""
                TxtRemark.Text = ""
                btnSubmit.Enabled = True
                scrname = "<SCRIPT language='javascript'>alert('Record Save successfully.!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Record Save successfully.!!');", True)

            Else


            End If


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btncencal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncencal.Click
        Try
            txtMemberId.Text = String.Empty
            lblMemberName.Text = String.Empty
            hdnFormno.Value = ""
            TxtRemark.Text = ""
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub txtMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberId.TextChanged
        Try
            Dim sql As String = String.Empty
            Dim scrname As String = String.Empty

            sql = " Select COUNT(*) cnt from M_Makefranchise Where IDno = '" & txtMemberId.Text.Trim() & "' "
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            If (Val(dt.Rows(0)("cnt")) > 0) Then
                txtMemberId.Text = String.Empty
                lblMemberName.Text = ""
                hdnFormno.Value = 0
                TxtRemark.Text = ""

                scrname = "<SCRIPT language='javascript'>alert('This Member already exists in franchise.!!' );" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('This Member already exists in franchise.!!');", True)
            Else
                sql = " Select Formno,Prefix +' '+ MemFirstName As  MemNAme,ActiveStatus from M_MemberMaster Where IDNO = '" & txtMemberId.Text.Trim() & "' "
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = objDAL.GetData(sql)
                If (dt.Rows.Count > 0) Then
                    If dt.Rows(0)("ActiveStatus") = "Y" Then
                        lblMemberName.Text = dt.Rows(0)("MemNAme")
                        hdnFormno.Value = dt.Rows(0)("Formno")
                    Else
                        lblMemberName.Text = ""
                        hdnFormno.Value = 0
                        TxtRemark.Text = ""
                        txtMemberId.Text = String.Empty

                        scrname = "<SCRIPT language='javascript'>alert('Member ID. Not Activated!!' );" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Member ID. Not Activated.');", True)

                    End If
                Else
                    lblMemberName.Text = ""
                    hdnFormno.Value = 0
                    txtMemberId.Text = String.Empty
                    TxtRemark.Text = ""

                    scrname = "<SCRIPT language='javascript'>alert('Invaild Member ID.!!' );" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Invaild Member ID.');", True)
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    
End Class
