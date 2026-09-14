
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_ApporoveOffineVoucher
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member / Apporve Offine Voucher"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                If Session("AStatus") = "OK" Then

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BindData()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = " Exec Sp_GetofflineVoucher '" & txtMemId.Text.Trim() & "','" & RbtSearch.SelectedValue & "'"
            Dim dt As DataTable = New DataTable()
            dt = objDAL.GetData(str)
            If (dt.Rows.Count > 0) Then
                GvData.DataSource = dt
                GvData.DataBind()
                BtnVerifiy.Enabled = True
            Else
                BtnVerifiy.Enabled = False
                GvData.DataSource = dt
                GvData.DataBind()
            End If
        Catch ex As Exception
        End Try

    End Sub



    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Try
            BindData()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnVerifiy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVerifiy.Click
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim scrname As String = ""
            Dim Chk As CheckBox
            Dim cnt As Integer
            Dim updateeffect As Integer
            Dim LblId As New Label
            Dim LblMobl As New Label
            Dim LblFromDate As New Label
            Dim LblTodate As New Label
            Dim txtRemark As New TextBox
            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                If Chk.Checked Then



                    Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
                    Dim FormNo As String = DirectCast(Gvr.FindControl("LblGrpID"), Label).Text
                    Dim DateOn As String = DirectCast(Gvr.FindControl("LblIdno"), Label).Text
                    Dim Remark As String = DirectCast(Gvr.FindControl("txtRemark"), TextBox).Text



                    Dim Sql As String = " Update M_Vouchar Set  IsPaid ='Y',PaidDate = Getdate(),Remark = '" & Remark & "' "
                    Sql &= " Where Id= '" & Id & "' And Formno = '" & FormNo & "'"

                    updateeffect = objDAL.SaveData(Sql)
                    'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                    cnt = cnt + 1
                End If
            Next

            If updateeffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('" & cnt & " Voucher Paid Successfully.');" & "</SCRIPT>"

            Else
                scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
            End If
            Me.RegisterStartupScript("MyAlert", scrname)


            BindData()
        Catch ex As Exception

        End Try
    End Sub
End Class


