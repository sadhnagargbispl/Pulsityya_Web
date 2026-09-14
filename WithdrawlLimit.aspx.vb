Imports System.Data
Imports System.Net
Imports System.IO

Partial Class WithdrawlLimit

    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Sql As String = ""
    Dim txformno As String = ""
    Dim txLoanNo As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click
        Try
            Dim query As String = ""
            Dim formNo As String
            Dim voucherNo As String = ""
            Dim scrName As String
            formNo = TxtFormNo.Text

            lblError.Text = ""

            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            Else
                lblError.Text = ""
            End If

            If Val(TxtFund.Text) <= "0" Then
                lblError.Text = "Enter Withdrawl Limit Amount"
                Exit Sub
            Else
                lblError.Text = ""
            End If
            
            If TxtRemarks.Text = "" Then
                lblError.Text = "Enter Remarks"
                Exit Sub
            Else
                lblError.Text = ""
            End If
            Dim Remark As String = ""
            If Request("key") IsNot Nothing Then
                txformno = Request("key")
                Dim Qry As String = "" '"Insert Into TempTrnLoan Select *,GetDate()," & Session("UserId") & " From TrnLoan Where Formno='" & Val(txformno) & "'"
                Qry = " Update M_WithdrawlLimit Set ActiveStatus='N' where Formno='" & Val(txformno) & "'"

                Qry = Qry & "Insert into M_WithdrawlLimit (formno,AmountLimit,Remark,Userid,ActiveStatus)Values " & _
                "('" & Val(TxtFormNo.Text) & "','" & Val(TxtFund.Text) & "','" & TxtRemarks.Text.Trim & "','" & Val(Session("Userid")) & "','Y')"
                If objDAL.SaveData(Qry) Then
                    scrName = "<SCRIPT language='javascript'>alert('Withdrawl Limit Update Successfully!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                    BtnFundTransfer.Enabled = True : TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : LblMemName.Text = "" : LblAmount.Text = ""

                End If
            Else
                query = "Insert into M_WithdrawlLimit (formno,AmountLimit,Remark,Userid,ActiveStatus)Values " & _
               "('" & Val(TxtFormNo.Text) & "','" & Val(TxtFund.Text) & "','" & TxtRemarks.Text.Trim & "','" & Val(Session("Userid")) & "','Y')"


                If objDAL.SaveData(query) <> 0 Then
                    ' SSendsms()

                    scrName = "<SCRIPT language='javascript'>alert('Withdrawl Limit Set Successfully!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                    BtnFundTransfer.Enabled = True : TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : LblMemName.Text = "" : LblAmount.Text = ""
                End If

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
                If Request("key") IsNot Nothing Then

                    txformno = Request("key")
                    BtnFundTransfer.Text = "Update"
                    'FillDetail()
                End If
            End If
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub
    Protected Sub FillDetail()
        Try

            Dim Dt As DataTable
            Dt = New DataTable
            Dim str As String = " select * from M_WithdrawlLimit as a, M_MemberMaster as b where  a.Formno=b.Formno and a.Formno='" & txformno & "' and Activestatus='Y' "
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dt = objDAL.GetData(str)
            If Dt.Rows.Count > 0 Then
                TxtIDNo.Text = Dt.Rows(0)("Idno")
                TxtFund.Text = Dt.Rows(0)("Amount")
                TxtRemarks.Text = Dt.Rows(0)("Remark")
                TxtIDNo.ReadOnly = True
                TxtFund.ReadOnly = True
                TxtRemarks.ReadOnly = True

            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub SSendsms()
        Try
            Dim client As WebClient
            Dim baseurl As String = ""
            Dim Data As System.IO.Stream
            Dim reader As StreamReader
            Dim s As String
            Dim Msg As String = " You have Recieve Fund Rs." & TxtFund.Text & " From " & Session("CompName") & " On Your Id No " & TxtFormNo.Text & ""
            client = New WebClient()
            'baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & LblMobl.Text & "&msg=" & Msg & ""
            baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & Msg & "&Contacts=" & LblMobl.Text & "&SenderId=" & Session("ClientId") & ""

            'baseurl = "http://smsnew.bispl.co.in/sendurlcomma.asp?user=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&senderid=" & Session("ClientId") & "&mobileno=" & MobileNo.Text & "&msgtext=" & Msg
            ' baseurl = "http://www.unicel.in/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & SMobileNo & "&msg=" & Msg
            Data = client.OpenRead(baseurl)
            reader = New StreamReader(Data)
            s = reader.ReadToEnd()
            Data.Close()
            reader.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'If PostXml = "OK" Then

        'End If
    End Sub


    Private Function Check_IdNo() As Boolean
        Try
            Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From M_Membermaster WHERE IDNO='" & Trim(TxtIDNo.Text) & "' and IsBlock='N'"
            Dim Dt_ As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            Dt_ = objDAL.GetData(Sql)
            If Dt_.Rows.Count = 0 Then
                LblMemName.Text = " Please enter correct Member ID."
                LblMemName.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                BtnFundTransfer.Enabled = False
                Return False
            Else
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblMobl.Text = Dt_.Rows(0)("Mobl")
                LblMemName.ForeColor = Drawing.Color.Black
                TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                lblError.Text = ""
                BtnFundTransfer.Enabled = True
                Return True
            End If

        Catch ex As Exception

        End Try

    End Function
    Protected Sub CheckBalance()
        Dim str As String = " Select Balance From dbo.ufnGetBalance(" & TxtFormNo.Text & ",'M')"
        Dim dt As DataTable
        dt = New DataTable
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dt = objDAL.GetData(str)
        If dt.Rows.Count > 0 Then
            LblAmount.Text = "Available Balance : " & dt.Rows(0)("Balance")
            LblAmount.Visible = True
            LblAmount.ForeColor = Drawing.Color.Black
        Else

            LblAmount.Visible = False
        End If

    End Sub

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()
        'CheckBalance()
    End Sub


    
End Class
