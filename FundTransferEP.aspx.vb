Imports System.Data
Imports System.Net
Imports System.IO

Partial Class App_UI_Application_Pages_FundTransferEP

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


            Dim sql1 As String = "Select Balance from M_GenerateEP"
            Dim dt1 As New DataTable
            dt1 = objDAL.GetData(sql1)
            If (dt1.Rows.Count > 0) Then
                Session("EPBalance") = dt1.Rows(0)("Balance")
                If (Val(TxtFund.Text) > Val(dt1.Rows(0)("Balance"))) Then
                    scrName = "<SCRIPT language='javascript'>alert('Insufficient balance Please Try Again.!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    Exit Sub
                End If
            End If

            Dim query As String = ""
            Dim formNo As String
            Dim voucherNo As String = ""

            formNo = TxtFormNo.Text

            lblError.Text = ""
            BtnFundTransfer.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            ElseIf Val(TxtFund.Text) <= "0" Then
                lblError.Text = "Enter Fund Value."
                Exit Sub
            Else
            End If
            Dim Remark As String = ""
            Dim sql As String = "select IsNull (Max(VoucherNo+1),1) as VoucherNo from TrnVoucher"
            Dim dt As New DataTable
            dt = objDAL.GetData(sql)
            If (dt.Rows.Count > 0) Then
                voucherNo = dt.Rows(0)("VoucherNo")

            End If
            ' Dim amoutNar As String = "Amount Transfer from " & Session("UserName") & " "
            If RbtAccount.SelectedValue = "C" Then
                Remark = TxtFund.Text & " EP. Credited In " & Rbtnwallet.SelectedItem.Text & " To IdNo " & TxtIDNo.Text & "  "
                query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId)values" & _
        "('" & voucherNo & "',Getdate(),0,'" & formNo & "', '" & TxtFund.Text & "',  '" & TxtRemarks.Text & "','Credit/" & formNo & "','" & Rbtnwallet.SelectedValue & "',GetDate(),'C',Convert(Varchar,GetDate(),112), " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "')"
                query = query & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Wallet Transfer ','" & Rbtnwallet.SelectedItem.Text & " Credit Transfer','" & Remark & "',Getdate(),'" & formNo & "')"

                query &= "  Exec Sp_GenerateEP 'Debit', '" & TxtFund.Text & "'"
                query &= "Insert into M_GenerateEPHistory (Formno,EP,VcType,Remark,Balance) Values"
                query &= "('" & formNo & "','" & TxtFund.Text & "','D','" & Remark & "','" & (Val(Session("EPBalance")) - Val(TxtFund.Text)) & "' )"



            ElseIf RbtAccount.SelectedValue = "D" Then
                Remark = TxtFund.Text & " EP. Debited In " & Rbtnwallet.SelectedItem.Text & " To IdNo " & TxtIDNo.Text & "  "
                query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId)values" & _
     "('" & voucherNo & "',Getdate(),'" & formNo & "',0, '" & TxtFund.Text & "',  '" & TxtRemarks.Text & "','Debit/" & formNo & "','" & Rbtnwallet.SelectedValue & "',GetDate(),'D',Convert(Varchar,GetDate(),112), " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "')"
                query = query & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
       "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Wallet Transfer ','" & Rbtnwallet.SelectedItem.Text & " Debit Transfer','" & Remark & "',Getdate(),'" & formNo & "')"

                query &= "  Exec Sp_GenerateEP 'Credit', '" & TxtFund.Text & "'"
                query &= "Insert into M_GenerateEPHistory (Formno,EP,VcType,Remark,Balance) Values"
                query &= "('" & formNo & "','" & TxtFund.Text & "','C','" & Remark & "','" & (Val(Session("EPBalance")) + Val(TxtFund.Text)) & "')"

            End If
            Dim K As String = " Begin Try Begin Transaction " & query & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "

            'objDAL.SaveData(Query)
            If objDAL.SaveData(K) <> 0 Then
                ' SSendsms()
                FillEP()
                scrName = "<SCRIPT language='javascript'>alert('Amount " & RbtAccount.SelectedItem.Text & " Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : LblMemName.Text = "" : LblAmount.Text = ""

                BtnFundTransfer.Enabled = True
            End If

            BtnFundTransfer.Enabled = True
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
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


    Public Sub FillEP()
        Sql = "  Select Credit,Debit,Balance from M_GenerateEP"
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim Dt As New DataTable
        Dt = objDAL.GetData(Sql)
        gv.DataSource = Dt
        gv.DataBind()
    End Sub





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Session("AStatus") = "OK" Then
                Session("PageName") = " Wallet / Wallet Transfer  "
                If Not Page.IsPostBack Then
                    FillWallet()
                    FillEP()
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


End Class
