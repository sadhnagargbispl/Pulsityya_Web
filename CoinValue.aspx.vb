Imports System.Data
Partial Class App_UI_Application_Pages_CoinValue
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnLegShift_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLegShift.Click
        Try
            Leg_Shift()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Private Sub FillData(ByVal Id As Integer)
        Try
            Dim str As String = ""
            Dim dt As DataTable = New DataTable()
            str = " Exec Sp_GetCoinMAster '" & Id & "','Get'"
            dt = obj.GetData(str)
            If (dt.Rows.Count > 0) Then
                txtCoin.Text = dt.Rows(0)("CValue")
                txtRemark.Text = dt.Rows(0)("Remark")
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Select Vaild Id !!!');location.replace('ListcoinValue.aspx');", True)
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Leg_Shift()
        Try
            Dim str As String = ""
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If (Request.QueryString("id") = Nothing) Then
                str = " Exec Sp_AddEditCoinMAster 0,1, '" & txtCoin.Text & "','" & txtRemark.Text & "','Y', '" & Session("UserID") & "','" & Session("UserName") & "' "
            Else
                str = " Exec Sp_AddEditCoinMAster '" & Val(Request.QueryString("id")) & "',1, '" & txtCoin.Text & "','" & txtRemark.Text & "','Y', '" & Session("UserID") & "','" & Session("UserName") & "' "
            End If
            Dim i As Integer = 0
            i = obj.SaveData(str)
            If (i > 0) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record  Save successfully !!!');location.replace('ListcoinValue.aspx');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record  Already Exists !!!');", True)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then
                    Session("PageName") = "Member / Coin Value"
                    FillData(Val(Request.QueryString("id")))
                Else
                    Response.Redirect("Logout.aspx")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
