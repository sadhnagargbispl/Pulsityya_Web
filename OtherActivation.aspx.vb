Imports System.Data
Imports System.Net
Imports System.IO

Partial Class OtherActivation

    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Sql As String = ""
    Dim scrName As String
    Dim Ds As DataSet

    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString


    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click

        Try

            '  CheckIdNo()

            Dim query As String = ""
            Dim formNo As String


            formNo = TxtFormNo.Text

            lblError.Text = ""
            BtnFundTransfer.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                scrName = "<SCRIPT language='javascript'>alert('Enter Member Id! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                Exit Sub
            ElseIf TxtSponsor.Text = "" Then
                scrName = "<SCRIPT language='javascript'>alert('Enter Sponsor Id! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                Exit Sub
            End If


            ' Remark = TxtFund.Text & " Rs. Credited In " & Rbtnwallet.SelectedItem.Text & " To IdNo " & TxtIDNo.Text & "  "
            '        query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId)values" & _
            '"('" & voucherNo & "',Getdate(),0,'" & formNo & "', '" & TxtFund.Text & "',  '" & TxtRemarks.Text & "','Credit/" & formNo & "','" & Rbtnwallet.SelectedValue & "',GetDate(),'C',Convert(Varchar,GetDate(),112), " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "')"
            query = "Exec NormalActivateId '" & Val(TxtFormNo.Text) & "','" & Val(TxtRefFormno.Text) & "' ;"
            query = query & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
     "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Normal ID In Tree ','Normal Id In Tree for Idno " & TxtIDNo.Text.Trim & " ','Normal Id In Tree for Idno " & TxtIDNo.Text.Trim & " ',Getdate(),'" & formNo & "')"


            Dim K As String = " Begin Try Begin Transaction " & query & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "

            'objDAL.SaveData(Query)
            If objDAL.SaveData(K) <> 0 Then
                ' SSendsms()

                scrName = "<SCRIPT language='javascript'>alert('Id placed In Tree Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                TxtSponsor.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRefFormno.Text = "" : TxtName.Text = ""
                TxtFormNo.Text = ""
                '   fillpackage()
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
    Private Function CheckIdNo() As String
        Dim sql As String = ""
        sql = " select a.idno,a.Memfirstname+' '+a.memlastName as MemberName,isnull(b.Idno,'') as sponsorid,a.formno," & _
        " a.Refformno,a.JoinTYpe,a.ActiveStatus " & _
        " from M_memberMaster as a Left Join M_membermaster as b On a.Refformno=b.Formno where a.Idno='" & TxtIDNo.Text & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(sql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("JoinType").trim <> "S" Then
                If dt.Rows(0)("ActiveStatus") = "Y" Then

                sql = "select * from R_MemtreeRelation where formnodwn='" & dt.Rows(0)("formno") & "'"
                    Dim dt1 As New DataTable
                    dt1 = objDAL.GetData(sql)
                    If dt1.Rows.Count > 0 Then
                        scrName = "<SCRIPT language='javascript'>alert('Id Already In Tree!! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                        TxtSponsor.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRefFormno.Text = "" : TxtName.Text = ""
                        TxtFormNo.Text = ""
                        BtnFundTransfer.Enabled = True
                        TxtSponsor.Enabled = True
                    Else
                        TxtSponsor.Text = dt.Rows(0)("sponsorid")
                        TxtFormNo.Text = dt.Rows(0)("Formno")
                        TxtRefFormno.Text = dt.Rows(0)("RefFormno")
                        TxtName.Text = dt.Rows(0)("MemberName")
                        If TxtSponsor.Text.Trim <> "" Then
                            TxtSponsor.Enabled = False
                        Else
                            TxtSponsor.Enabled = True
                        End If
                        BtnFundTransfer.Enabled = True

                    End If
                Else

                    scrName = "<SCRIPT language='javascript'>alert('Member Id Not Activated!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                    TxtSponsor.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRefFormno.Text = "" : TxtName.Text = ""
                    TxtFormNo.Text = ""
                    TxtSponsor.Enabled = True
                    BtnFundTransfer.Enabled = True

                End If

            Else

                scrName = "<SCRIPT language='javascript'>alert('Member Id is not Normal!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                TxtSponsor.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRefFormno.Text = "" : TxtName.Text = ""
                TxtFormNo.Text = ""
                BtnFundTransfer.Enabled = True

            End If

        Else
            scrName = "<SCRIPT language='javascript'>alert('Invalid Member Id!! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
            TxtSponsor.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRefFormno.Text = "" : TxtName.Text = ""
            TxtFormNo.Text = ""
            BtnFundTransfer.Enabled = True


        End If
    End Function
    

    Private Function CheckSponsorIdNo() As String
        Dim sql As String = ""
        sql = " select a.idno,a.Memfirstname+' '+a.memlastName as MemberName,a.formno," & _
        " a.Refformno,a.JoinTYpe,a.ActiveStatus " & _
        " from M_memberMaster as a  where  a.Idno='" & TxtSponsor.Text.Trim & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(sql)
        If dt.Rows.Count > 0 Then
            sql = "select * from R_MemtreeRelation where formnodwn='" & dt.Rows(0)("formno") & "'"
            Dim dt1 As New DataTable
            dt1 = objDAL.GetData(sql)
            If dt1.Rows.Count = 0 Then
                scrName = "<SCRIPT language='javascript'>alert(' Sponsor Id is not in tree!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                TxtSponsor.Text = "" : TxtRefFormno.Text = ""
                BtnFundTransfer.Enabled = True
            Else
                TxtRefFormno.Text = dt.Rows(0)("Formno")

                BtnFundTransfer.Enabled = True

            End If
            Else
            scrName = "<SCRIPT language='javascript'>alert('Invalid Sponsor Id!! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
            TxtSponsor.Text = "" : TxtRefFormno.Text = ""
            BtnFundTransfer.Enabled = True


        End If
    End Function










    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Session("AStatus") = "OK" Then
                Session("PageName") = " Wallet / Wallet Transfer  "
                If Not Page.IsPostBack Then
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






    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        CheckIdNo()
        'If Check_IdNo() = False Then
        '    scrName = "<SCRIPT language='javascript'>alert('Invalid IdNo!! ');" & "</SCRIPT>"
        '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
        '    '          
        'End If

    End Sub





    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Response.Redirect("Home.aspx")
    End Sub

    Protected Sub TxtSponsor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtSponsor.TextChanged
        CheckSponsorIdNo()
    End Sub
End Class
