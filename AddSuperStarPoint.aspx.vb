Imports System.Data
Imports System.Net
Imports System.IO

Partial Class AddSuperStarPoint

    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim query As String = ""
            Dim formNo As String
            Dim voucherNo As String = ""
            Dim scrName As String


            lblError.Text = ""
            BtnFundTransfer.Enabled = False

            If Val(TxtFund.Text) <= "0" Then
                lblError.Text = "Enter Super Star Amount."
                Exit Sub
            Else
            End If
            Dim Remark As String = ""
            query = " Update M_SuperStar Set ActiveStatus='N' where Sessid=Convert(Varchar,GetDate(),112);"
            query = query & " Insert into M_SuperStar(Sessid,amount,Remark,RecTimeStamp,ActiveStatus,Rankid) Values " & _
            " (Convert(Varchar,GetDate(),112),'" & Val(TxtFund.Text) & "','" & TxtRemarks.Text & "',Getdate(),'Y',1)"
            query = query & " Insert into M_SuperStar(Sessid,amount,Remark,RecTimeStamp,ActiveStatus,Rankid) Values " & _
           " (Convert(Varchar,GetDate(),112),'" & Val(TxtFund.Text) * 2 & "','" & TxtRemarks.Text & "',Getdate(),'Y',2)"

            'objDAL.SaveData(Query)
            If objDAL.SaveData(query) <> 0 Then
                ' SSendsms()

                scrName = "<SCRIPT language='javascript'>alert('Super Star Amount Added in  Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                TxtFund.Text = ""
            End If

            scrName = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrName, False)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
            End If
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub





End Class
