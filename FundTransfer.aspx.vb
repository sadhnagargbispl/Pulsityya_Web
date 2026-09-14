Imports System.Data
Imports System.Net
Imports System.IO

Partial Class App_UI_Application_Pages_FundTransfer

    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Sql As String = ""
    Dim scrName As String
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString


    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click

        Try
            If Rbtnwallet.SelectedValue = "" Then
                scrName = "<SCRIPT language='javascript'>alert('Please Select Wallet Type!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                Exit Sub
            End If

            Dim query As String = ""
            Dim formNo As String
            Dim voucherNo As String = ""

            formNo = TxtFormNo.Text

            lblError.Text = ""
            BtnFundTransfer.Enabled = False

            'If Trim(TxtIDNo.Text) = "" Then
            '    lblError.Text = "Enter Member ID."
            '    Exit Sub
            'ElseIf Val(TxtFund.Text) <= "0" Then
            '    lblError.Text = "Enter Fund Value."
            '    Exit Sub
            'Else
            'End If
            Dim Remark As String = ""
            Dim sql As String = "select IsNull (Max(VoucherNo+1),1) as VoucherNo from TrnVoucher"
            Dim dt As New DataTable
            dt = objDAL.GetData(sql)
            If (dt.Rows.Count > 0) Then
                voucherNo = dt.Rows(0)("VoucherNo")

            End If
            ' Dim amoutNar As String = "Amount Transfer from " & Session("UserName") & " "
            If RbtAccount.SelectedValue = "C" Then
                Remark = TxtFund.Text & " Rs. Credited In " & Rbtnwallet.SelectedItem.Text & " To IdNo " & TxtIDNo.Text & "  "
                'Below commit 08 Nov 2023 by Hemraj
                '        query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId)values" & _
                '"('" & voucherNo & "',Getdate(),0,'" & formNo & "', '" & TxtFund.Text & "',  '" & TxtRemarks.Text & "','Credit/" & formNo & "','" & Rbtnwallet.SelectedValue & "',GetDate(),'C',Convert(Varchar,GetDate(),112), " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "')"
                '        query = query & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                ' "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Wallet Transfer ','" & Rbtnwallet.SelectedItem.Text & " Credit Transfer','" & Remark & "',Getdate(),'" & formNo & "')"

                query = query & "Exec sp_funtransfer '" & formNo & "','" & Format(Now, "dd-MMM-yyyy") & "',0,'" & formNo & "', '" & TxtFund.Text & "', " & _
              "'" & TxtRemarks.Text & "','Credit/" & formNo & "','" & Rbtnwallet.SelectedValue & "', " & _
              "'C', " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "','" & Val(Session("UserID")) & "'" & _
              " ,'" & Session("UserName") & "','" & Rbtnwallet.SelectedItem.Text & " Credit Transfer','" & Remark & "','" & formNo & "', " & HdnCheckTrnns.Value & ""
            ElseIf RbtAccount.SelectedValue = "D" Then
                Remark = TxtFund.Text & " Rs. Debited In " & Rbtnwallet.SelectedItem.Text & " To IdNo " & TxtIDNo.Text & "  "

                'Below commit 08 Nov 2023 by Hemraj
                '           query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId)values" & _
                '"('" & voucherNo & "',Getdate(),'" & formNo & "',0, '" & TxtFund.Text & "',  '" & TxtRemarks.Text & "','Debit/" & formNo & "','" & Rbtnwallet.SelectedValue & "',GetDate(),'D',Convert(Varchar,GetDate(),112), " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "')"
                '           query = query & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                '  "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Wallet Transfer ','" & Rbtnwallet.SelectedItem.Text & " Debit Transfer','" & Remark & "',Getdate(),'" & formNo & "')"
                query = "Exec sp_funtransfer '" & formNo & "','" & Format(Now, "dd-MMM-yyyy") & "','" & formNo & "',0, '" & TxtFund.Text & "', " & _
                "'" & TxtRemarks.Text & "','Debit/" & formNo & "','" & Rbtnwallet.SelectedValue & "','D'," & _
                " " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "','" & Val(Session("UserID")) & "' " & _
                " ,'" & Session("UserName") & "','" & Rbtnwallet.SelectedItem.Text & " Debit Transfer','" & Remark & "','" & formNo & "', " & HdnCheckTrnns.Value & ""
            End If
            Dim K As String = " Begin Try Begin Transaction " & query & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "

            'objDAL.SaveData(Query)
            If objDAL.SaveData(K) <> 0 Then

                If (Session("CompID") = 1026) Then
                    If (RbtAccount.SelectedValue = "C" And Rbtnwallet.SelectedValue = "P") Then
                        SendSMS(LblMemName.Text, TxtFund.Text, LblMobl.Text)
                    End If
                End If
                ' SSendsms()
                Dim scrName As String = "<SCRIPT language='javascript'>alert('Amount " & RbtAccount.SelectedItem.Text & " Successfully!!'); window.location.href='fundtransfer.aspx';</SCRIPT>"
                'scrName = "<SCRIPT language='javascript'>alert('Amount " & RbtAccount.SelectedItem.Text & " Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : LblMemName.Text = "" : LblAmount.Text = ""

                BtnFundTransfer.Enabled = True
            Else
                scrName = "<SCRIPT language='javascript'>alert('Amount Transfer UnSuccessfully!! ');location.replace('fundtransfer.aspx');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                BtnFundTransfer.Enabled = False
            End If

            'BtnFundTransfer.Enabled = True
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub

    Private Sub SendSMS(ByVal Name As String, ByVal Amount As String, ByVal Mobile As String)
        Dim client As New WebClient
        Dim baseurl As String
        Dim data As Stream
       
        Dim sms As String = " Dear " & Name & ", We have successfully transferred Rs. " & Amount & " in Your Petrol Card For visit https://mohrslife.com/."
        Try
            baseurl = "http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & Trim(Mobile) & "&SenderId=" & Session("ClientId") & ""
            data = client.OpenRead(baseurl)
            Dim reader As New StreamReader(data)
            Dim s As String
            s = reader.ReadToEnd()
            data.Close()
            reader.Close()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub FillWallet()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetWallettype")
            Rbtnwallet.DataSource = Ds.Tables(0)
            Rbtnwallet.DataValueField = "Actype"
            Rbtnwallet.DataTextField = "Walletname"
            Rbtnwallet.DataBind()
        Catch ex As Exception
        End Try
    End Sub








    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            'Me.BtnFundTransfer.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnFundTransfer))
            Try
                Dim str = "exec('Create table Trnfundtransferbyadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
 "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnfundtransferbyadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
                Dim i As Integer = 0
                i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)


                Dim str1 = "Exec('CREATE PROCEDURE [dbo].[sp_funtransfer] (   " & _
" @formno  numeric(18,0),   " & _
" @Voucherdate datetime,   " & _
" @crto numeric(18,0),@drto numeric(18,0), @Amount numeric(18,2),  " & _
" @Narration varchar(max), @Refno varchar(250), " & _
" @AcType char(1),@VType char(1), " & _
" @WSessID int, @Txtaremark nvarchar(Max), " & _
" @UserId Numeric(18,0), " & _
" @UserName nvarchar(200), " & _
" @PageName nvarchar(200),  " & _
" @Activity nvarchar(200),  " & _
" @Remark nvarchar(Max) ,    " & _
" @Hdntransid numeric(18,0)  " & _
" )  " & _
" AS  " & _
" BEGIN      " & _
" DECLARE @LocalError INT,  " & _
" @ErrorMessage VARCHAR(4000)  " & _
" DECLARE @Identity INT     " & _
" DECLARE @MainIdentity INT  " & _
" BEGIN TRY       " & _
" BEGIN TRANSACTION TestTransaction  " & _
" begin    " & _
" Declare @MaxVouNo As Numeric(18,0);      " & _
" Select @MaxVouNo=Cast(Convert(nvarchar(20),GetDate(),112)+Replace(Convert(nvarchar(20),GetDate(),114),'':'','''') as Numeric(18,0)); " & _
" Insert into Trnfundtransferbyadmin (Transid)values(@Hdntransid)        " & _
" INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,RecTimeStamp,VTYpe,SessID,WSessID) " & _
" values(@MaxVouNo,@Voucherdate,@crto,@drto,@Amount,@Narration,@Refno,@AcType,Getdate(),@VType,Convert(Varchar,Getdate(),112),@WSessID)   " & _
" insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,Memberid) " & _
" Values(@UserId,@UserName,@PageName,@Activity,@Remark,Getdate(),@formno)  " & _
" end " & _
" COMMIT TRANSACTION TestTransaction      " & _
"    Select ''Success'' As result, 1 As Count    " & _
" RETURN     " & _
" END TRY    " & _
" BEGIN CATCH   " & _
" SELECT @LocalError = ERROR_NUMBER(),@ErrorMessage = ERROR_MESSAGE()   " & _
" Select ''Faild'' As result, 1 As Count  " & _
" IF (XACT_STATE()) <> 0     " & _
" BEGIN ROLLBACK TRANSACTION TestTransaction " & _
" END    " & _
" RAISERROR (''sp_funtransfer: %d: %s'',16,1,@LocalError,@ErrorMessage);   " & _
" RETURN (0)   " & _
" END CATCH   " & _
" END ') "
                Dim j As Integer = 0
                j = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str1)

            Catch ex As Exception

            End Try



            If Session("AStatus") = "OK" Then
                Session("PageName") = " Wallet / Wallet Transfer  "
                If Not Page.IsPostBack Then
                    HdnCheckTrnns.Value = GenerateRandomString(6)
                    FillWallet()

                End If
            Else
                Response.Redirect("logout.aspx")

            End If

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try




    End Sub

    Public Function GenerateRandomString(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function
    Private Sub SSendsms()
        Try
            Dim client As WebClient
            Dim baseurl As String = ""
            Dim Data As System.IO.Stream
            Dim reader As StreamReader
            Dim s As String
            Dim Msg As String = " You have Recieve Fund Rs." & TxtFund.Text & " From " & Session("CompName") & " On Your Id No " & TxtFormNo.Text & ""
            client = New WebClient()
            baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & LblMobl.Text & "&msg=" & Msg & ""

            'baseurl = "http://smsnew.bispl.co.in/sendurlcomma.asp?user=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&senderid=" & Session("ClientId") & "&mobileno=" & MobileNo.Text & "&msgtext=" & Msg
            ' baseurl = "http://www.unicel.in/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & SMobileNo & "&msg=" & Msg
            Data = client.OpenRead(baseurl)
            reader = New StreamReader(Data)
            s = reader.ReadToEnd()
            Data.Close()
            reader.Close()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

        'If PostXml = "OK" Then

        'End If
    End Sub


    Private Function Check_IdNo() As Boolean
        Try
            Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From " & objDAL.tblMemberMaster & " WHERE IDNO='" & Trim(TxtIDNo.Text) & "'"
            Dim Dt_ As New DataTable
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
                BtnFundTransfer.Enabled = True
                CheckBalance()
                Return True
            End If


        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try


    End Function
    Protected Sub CheckBalance()
        Try
            Dim str As String = " Select Balance From dbo.ufnGetBalance(" & TxtFormNo.Text & ",'" & Rbtnwallet.SelectedValue & "')"
            Dim dt As DataTable
            dt = New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(str)
            If dt.Rows.Count > 0 Then
                LblAmount.Text = "Available Balance : " & dt.Rows(0)("Balance")
                LblAmount.Visible = True
                'LblAmount.ForeColor = Drawing.Color.Black

            Else

                LblAmount.Visible = False
            End If

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try



    End Sub

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()

    End Sub


    Protected Sub Rbtnwallet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Rbtnwallet.SelectedIndexChanged
        If TxtIDNo.Text <> "" Then
            CheckBalance()
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
End Class
