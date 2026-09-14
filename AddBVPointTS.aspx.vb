Imports System.Data
Imports System.Net
Imports System.IO

Partial Class AddBVPointTS

    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            Me.BtnFundTransfer.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnFundTransfer))
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
            End If
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub

    Private Function DisableTheButton(ByVal pge As Control, ByVal btn As Control) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ")
        sb.Append("this.value = 'Please wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(pge.Page.GetPostBackEventReference(btn))
        sb.Append(";")
        Return sb.ToString()
    End Function

    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim query As String = ""
            Dim formNo As String
            Dim voucherNo As String = ""
            Dim scrName As String
            formNo = TxtFormNo.Text

            lblError.Text = ""
            BtnFundTransfer.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            ElseIf Val(TxtFund.Text) <= "0" Then
                lblError.Text = "Enter PV Value."
                Exit Sub
            Else
            End If
            Dim Remark As String = ""

            query = " Insert into TrnBV(Sessid,Formno,LegNo,BV,Remark,RecTimeStamp,ActiveStatus,Bvtype,Dsessid,PV,UserId,MSessid) Values " & _
            " ('" & Session("CurrentSessn") & "','" & Val(TxtFormNo.Text) & "','" & Val(RbtLeg.SelectedValue) & "'," & _
            " '" & Val(TxtFund.Text) & "','" & TxtRemarks.Text & "',Getdate(),'Y','" & RbtType.SelectedValue & "',Convert(Varchar,Getdate(),112),'" & Val(TxtFund.Text) & "','" & Val(Session("userid")) & "',(select isnull(Max(Sessid),1) from M_monthSessnmaster))"
            If RbtType.SelectedValue = "T" Then
                query = query & ";Insert into Repurchincome(Sessid,Formno,BillNo,Billdate,Repurchincome,Imported,BillType,SoldBy,MSessid,KitId,Dsessid,Remarks,PVValue)" & _
                            " values('" & Session("CurrentSessn") & "','" & Val(TxtFormNo.Text) & "','',Getdate(),'" & Val(TxtFund.Text) & "','N','T','WR'," & _
                           "  (select isnull(Max(Sessid),1) from M_monthSessnmaster),0,Convert(varchar,Getdate(),112),'" & TxtRemarks.Text.Trim & "','" & Val(TxtFund.Text) & "') "

            End If
            Dim K As String = " Begin Try Begin Transaction " & query & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "

            If objDAL.SaveData(K) <> 0 Then
                ' SSendsms()

                scrName = "<SCRIPT language='javascript'>alert('BV Point Added in " & RbtLeg.SelectedItem.Text & " Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : LblMemName.Text = ""
            End If

            scrName = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrName, False)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

  



    Private Function Check_IdNo() As Boolean
        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From " & objDAL.tblMemberMaster & " WHERE IDNO='" & Trim(TxtIDNo.Text) & "'"
        Dim Dt_ As New DataTable
        Dim dt1 As DataTable
        Dt_ = objDAL.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            LblMemName.Text = " Please enter correct Member ID."
            LblMemName.ForeColor = Drawing.Color.Red
            TxtIDNo.Text = ""
            BtnFundTransfer.Enabled = False
            Return False
        Else
            Sql = " Select * from TrnBV where Formno='" & Dt_.Rows(0)("Formno") & "' and DSessid=Convert(varchar,Getdate(),112) and ActiveStatus='Y' and BvType='" & RbtType.SelectedValue & "'"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt1 = New DataTable
            dt1 = objDAL.GetData(Sql)
            If dt1.Rows.Count > 0 Then
                LblMemName.Text = " This Id Already Have " & RbtType.SelectedItem.Text.Trim & " PV In Current Session"
                BtnFundTransfer.Enabled = False
                Return False
            Else
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblMobl.Text = Dt_.Rows(0)("Mobl")
                LblMemName.ForeColor = Drawing.Color.Black
                TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                BtnFundTransfer.Enabled = True
                Return True
            End If
        End If
    End Function


    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()

    End Sub


    Protected Sub RbtType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtType.SelectedIndexChanged
        Check_IdNo()
    End Sub
End Class
