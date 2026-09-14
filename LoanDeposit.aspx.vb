Imports System.Data
Imports System.Data.SqlClient
Partial Class LoanDeposit
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim scrname As String = ""
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Player Master"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack And Request.QueryString.HasKeys Then
                Dim Key As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("key"), " ", "+")))

                LblDeviceId.Text = Key

                Dim FormNo As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("FormNo"), " ", "+")))
                LblFormNo.Text = FormNo
                Dim EmiMonth As Integer = Val(Crypto.Decrypt(Replace(Request.QueryString("EmiMonth"), " ", "+")))
                LblFormNo.Text = EmiMonth
                BindData(Key)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Try


            Dim StateCode, scrname As String
            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            StateCode = DirectCast(GVRw.FindControl("Reqno"), Label).Text
            Dim remark As String = DirectCast(GVRw.FindControl("TxtRemark"), TextBox).Text
            Dim formno As String = DirectCast(GVRw.FindControl("LblFormno"), Label).Text
            Dim amount As String = DirectCast(GVRw.FindControl("LblAmount"), Label).Text
            Dim emino As String = DirectCast(GVRw.FindControl("emino"), Label).Text
            Dim sql As String = "exec Sp_ActivateMemberAdmin '" & Val(StateCode.ToString()) & "','" & Val(emino.ToString()) & "','" & remark.ToString() & "'"
             Dim updateEffect As Integer = objDAL.UpdateData(Sql)
            If updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Loan Deposit Successfuly!');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Not able to deposit EMI! ');" & "</SCRIPT>"
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
            BindData(LblDeviceId.Text)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub BindData(ByVal DeviceID As String)
        Try
            Dim _Condition As String = ""

            Dim Qry As String = " exec sp_LoanDetail " & DeviceID & " "
            dtData = New DataTable
            dtData = objDAL.GetData(Qry)

            dtData.AcceptChanges()
            GvData.DataSource = dtData
            GvData.DataBind()


        Catch ex As Exception

        End Try
    End Sub
End Class
