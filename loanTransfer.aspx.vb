Imports System.Data
Imports System.Net
Imports System.IO

Partial Class App_UI_Application_Pages_FundTransfer

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
                lblError.Text = "Enter Loan Amount"
                Exit Sub
            Else
                lblError.Text = ""
            End If
            If RntLoan.SelectedValue = "D" Then
                If Txtloan.Text = "" Then
                    lblError.Text = "Enter Loan Recovery %"
                    Exit Sub
                Else
                    lblError.Text = ""
                End If
            End If

            If TxtRemarks.Text = "" Then
                lblError.Text = "Enter Remarks"
                Exit Sub
            Else
                lblError.Text = ""
            End If
            Dim Remark As String = ""
            Dim LoanNo As String = ""
            If Request("key") IsNot Nothing And Request("LoanNo") IsNot Nothing Then
                txformno = Request("key")
                txLoanNo = Request("LoanNo")
                '  Dim sql As String = "Insert into TempTrnLoan from TrnLoan where LoanNo='"& LoanNo &"'"Update TrnLoan Set "
                Dim Qry As String = "Insert Into TempTrnLoan Select *,GetDate()," & Session("UserId") & " From TrnLoan Where Formno='" & Val(txformno) & "'"
                Qry = Qry & " Update TrnLoan Set LoanPercent='" & Txtloan.Text & "' where Formno='" & Val(txformno) & "'"
                If objDAL.SaveData(Qry) Then
                    scrName = "<SCRIPT language='javascript'>alert('Loan Update Successfully!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                    BtnFundTransfer.Enabled = True : TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : LblMemName.Text = "" : LblAmount.Text = "" : Txtloan.Text = ""

                End If
            Else

                Dim sql As String = "select Sum(VoucherNo) as VoucherNo,Sum(LoanNo) as LoanNo " & _
                " from (select 0 as VoucherNo,IsNull (Max(LoanNo+1),100001) as LoanNo from TrnLoan" & _
                " Union all " & _
                "select IsNull (Max(VoucherNo+1),1) as VoucherNo,0 as LoanNo from TrnVoucher) as Temp"
                Dim dt As New DataTable

                dt = objDAL.GetData(sql)
                If (dt.Rows.Count > 0) Then
                    voucherNo = dt.Rows(0)("VoucherNo")
                    LoanNo = dt.Rows(0)("LoanNo")
                    BtnFundTransfer.Enabled = False
                End If
                ' Dim amoutNar As String = "Amount Transfer from " & Session("UserName") & " "
                If RntLoan.SelectedValue = "D" Then
                    Remark = TxtFund.Text & " Rs. Loan Transfer To IdNo " & TxtIDNo.Text & "  "
                    query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId,Balance)values" & _
                "('" & voucherNo & "',Getdate(),0,'" & formNo & "','" & TxtFund.Text & "','" & Remark & "','" & LoanNo & "','L',GetDate(),'C'," & _
                " Convert(Varchar,GetDate(),112), " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "',(select Balance+" & Val(TxtFund.Text) & " from Dbo.ufnGetBalance('" & formNo & "','L')));"
                    query = query & "Insert Into TempTrnLoan Select *,GetDate()," & Session("UserId") & " From TrnLoan Where Formno='" & Val(formNo) & "'"
                    query = query & " Update TrnLoan Set LoanPercent='" & Txtloan.Text & "' where Formno='" & Val(formNo) & "'"

                    query = query & "insert into TrnLoan(LoanNo,Formno,LoanDate,Sessid,Dsessid, Amount,LoanPercent,Remark,RecTimeStamp,UserId)values" & _
                " ('" & LoanNo & "','" & formNo & "',Getdate()," & Session("CurrentSessn") & ",Convert(Varchar,GetDate(),112), '" & TxtFund.Text & "','" & Txtloan.Text & "',  '" & TxtRemarks.Text & "',GetDate(),'" & Val(Session("UserID")) & "')"
                    query = query & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
             "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Loan Transfer ','Loan Transfer','" & Remark & "',Getdate(),'" & formNo & "')"
                Else
                    query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId,balance)values" & _
            "('" & voucherNo & "',Getdate(),'" & formNo & "',0,'" & TxtFund.Text & "','" & TxtRemarks.Text & "','" & LoanNo & "','L',GetDate(),'D',Convert(Varchar,GetDate(),112)," & _
            " " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "',(select Balance-" & Val(TxtFund.Text) & " from Dbo.ufnGetBalance('" & formNo & "','L')));"
                    query = query & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                                "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Loan Transfer ','Loan Debit Transfer','" & Remark & "',Getdate(),'" & formNo & "')"

                End If
                '  End If

                'objDAL.SaveData(Query)
                If objDAL.SaveData(query) <> 0 Then
                    ' SSendsms()

                    scrName = "<SCRIPT language='javascript'>alert('Loan Transfer Successfully!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                    BtnFundTransfer.Enabled = True : TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : LblMemName.Text = "" : LblAmount.Text = "" : Txtloan.Text = ""
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
                If Request("key") IsNot Nothing And Request("LoanNo") IsNot Nothing Then

                    txformno = Request("key")
                    txLoanNo = Request("LoanNo")
                    BtnFundTransfer.Text = "Update Loan"
                    FillDetail()
                End If
            End If
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub
    Protected Sub FillDetail()
        Dim Dt As DataTable
        Dt = New DataTable
        Dim str As String = " select * from TrnLoan as a, M_MemberMaster as b where  a.Formno=b.Formno and a.Formno='" & txformno & "' and LoanNo='" & txLoanNo & "' "
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dt = objDAL.GetData(str)
        If Dt.Rows.Count > 0 Then
            TxtIDNo.Text = Dt.Rows(0)("Idno")
            TxtFund.Text = Dt.Rows(0)("Amount")
            Txtloan.Text = Dt.Rows(0)("LoanPercent")
            TxtRemarks.Text = Dt.Rows(0)("Remark")
            Txtloan.ReadOnly = False
            TxtIDNo.ReadOnly = True
            TxtFund.ReadOnly = True
            TxtRemarks.ReadOnly = True
            RntLoan.Enabled = False

        End If

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


    Protected Sub RntLoan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RntLoan.SelectedIndexChanged
        If RntLoan.SelectedValue = "D" Then
            PLoanPercent.Visible = True
        Else
            PLoanPercent.Visible = False
        End If
    End Sub
End Class
