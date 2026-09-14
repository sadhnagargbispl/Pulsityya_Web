Imports System.Data
Imports System.Net
Imports System.IO

Partial Class AddEMI

    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim Sql As String = ""
    Dim scrName As String
    Dim Ds As DataSet

    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString


    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click

        Try


            Dim query As String = ""
            Dim formNo As String


            formNo = TxtFormNo.Text

            lblError.Text = ""
            BtnFundTransfer.Enabled = False

            If Trim(TxtIDNo.Text) = "" Then
                scrName = "<SCRIPT language='javascript'>alert('Enter Member Id! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                Exit Sub
            ElseIf Val(TxtFund.Text) <= "0" Then
                scrName = "<SCRIPT language='javascript'>alert('Enter Amount! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                Exit Sub
            ElseIf Val(TxtMonth.Text) <= 0 Then
                scrName = "<SCRIPT language='javascript'>alert('Enter Month ! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                Exit Sub
                'ElseIf Val(TxtROI.Text) <= 0 Then
                '    scrName = "<SCRIPT language='javascript'>alert('Enter ROI ! ');" & "</SCRIPT>"
                '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
            End If
            Dim Remark As String = ""
            Dim reqno As Integer = 0
            Dim dt As New DataTable
            Sql = "select isnull(Max(ReqNo)+1,10001)as Reqno from TrnEMi"
            dt = objDAL.GetData(Sql)
            If dt.Rows.Count > 0 Then
                reqno = dt.Rows(0)("ReqNo")
            End If

            query = "insert into TrnEmi(ReqNo,ReqDate,Crto,Drto,Amount,EMIMonth,Remark,RefNo,Userid,EmiType,RectimeStamp,ActiveStatus)" & _
                    " Values(" & reqno & ",Getdate(),'" & Val(TxtFormNo.Text) & "',0,'" & Val(TxtFund.Text) & "'," & _
                    " '" & TxtMonth.Text & "','" & TxtRemarks.Text & "','EMI/'+ Cast( " & reqno & " as Varchar)," & _
                    " '" & Val(Session("userid")) & "','R',Getdate(),'Y')"
            For i = 1 To Val(TxtMonth.Text)
                query = query & "insert into TrnEmi(ReqNo,ReqDate,Crto,Drto,Amount,EMIMonth,Remark,RefNo,Userid,EmiType,RectimeStamp,ActiveStatus)" & _
                    " Values((select isnull(Max(ReqNo)+1,10001) from TrnEMi),DateAdd(Month," & i & ",Getdate()),0,'" & Val(TxtFormNo.Text) & "','" & Val(TxtROI.Text) & "'," & _
                    " '" & i & "','" & i & " EMI Added Against '+ Cast(" & reqno & " as Varchar),'EMI/'+ Cast(" & reqno & " as Varchar)," & _
                    " '" & Val(Session("userid")) & "','R',Getdate(),'N')"
            Next
            ' Remark = TxtFund.Text & " Rs. Credited In " & Rbtnwallet.SelectedItem.Text & " To IdNo " & TxtIDNo.Text & "  "
            '        query = "insert into TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo, Amount,Narration,RefNo, AcType,RecTimeStamp, VType,SessID,WSessID,UserId)values" & _
            '"('" & voucherNo & "',Getdate(),0,'" & formNo & "', '" & TxtFund.Text & "',  '" & TxtRemarks.Text & "','Credit/" & formNo & "','" & Rbtnwallet.SelectedValue & "',GetDate(),'C',Convert(Varchar,GetDate(),112), " & Session("CurrentSessn") & ",'" & Val(Session("UserID")) & "')"
            'query = "insert into TrnEMI(formno,Amount,ROI,ROIMonth,RectimeStamp,ActiveStatus,Remarks)" & _
            '" values('" & formNo & "','" & Val(TxtFund.Text) & "','" & TxtROI.Text & "','" & TxtMonth.Text & "',Getdate(),'Y','" & TxtRemarks.Text.Trim & "') "
            query = query & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
     "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','ADD EMI ','EMI Added To Idno " & TxtIDNo.Text.Trim & " ','EMI Added To Idno " & TxtIDNo.Text.Trim & " of Amount " & TxtFund.Text & " for " & TxtMonth.Text & " month ',Getdate(),'" & formNo & "')"


            Dim K As String = " Begin Try Begin Transaction " & query & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "

            'objDAL.SaveData(Query)
            If objDAL.SaveData(K) <> 0 Then
                ' SSendsms()

                scrName = "<SCRIPT language='javascript'>alert('EMI Added Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                'lblError.Text = "Amount " & RbtAccount.SelectedItem.Text & " Successfully!!"
                TxtFund.Text = "" : TxtIDNo.Text = "" : TxtFormNo.Text = "" : TxtRemarks.Text = "" : TxtName.Text = ""
                TxtMonth.Text = ""
                TxtROI.Text = ""
                TxtRemarks.Text = ""
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











    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            TxtFund.Attributes.Add("autocomplete", "off")
            TxtIDNo.Attributes.Add("autocomplete", "off")
            TxtMonth.Attributes.Add("autocomplete", "off")
            TxtROI.Attributes.Add("autocomplete", "off")
            TxtRemarks.Attributes.Add("autocomplete", "off")
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Session("AStatus") = "OK" Then
                Session("PageName") = " Wallet / Wallet Transfer  "
                If Not Page.IsPostBack Then
                    '   FillWallet()
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



    Private Function Check_IdNo() As Boolean
        Try
            Sql = "Select  MemFirstName+' '+ MemLastName as MemName,a.FormNo,Mobl,Count(b.Crto) as EMICount " & _
            " From " & objDAL.tblMemberMaster & " as a Left Join TrnEMI as b on a.Formno=b.Crto " & _
            " WHERE IDNO='" & Trim(TxtIDNo.Text) & "' Group by MemFirstName,MemLastName,a.FormNo,Mobl"
            Dim Dt_ As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dt_ = objDAL.GetData(Sql)
            If Dt_.Rows.Count = 0 Then
                TxtName.Text = " "
                TxtFormNo.Text = ""
                TxtIDNo.Text = ""
                TxtMonth.Text = ""
                TxtROI.Text = ""
                TxtRemarks.Text = ""
                BtnFundTransfer.Enabled = False
                scrName = "<SCRIPT language='javascript'>alert('Invalid IdNo!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)

                Return False
            Else
                If Dt_.Rows(0)("EMIcount") = 0 Then
                    TxtName.Text = Dt_.Rows(0)("MemName")

                    TxtFormNo.Text = Dt_.Rows(0)("FormNo")
                    BtnFundTransfer.Enabled = True
                    Return True
                Else
                    TxtName.Text = " "
                    TxtFormNo.Text = ""
                    TxtIDNo.Text = ""
                    TxtMonth.Text = ""
                    TxtROI.Text = ""
                    TxtRemarks.Text = ""
                    BtnFundTransfer.Enabled = False
                    scrName = "<SCRIPT language='javascript'>alert('Already give EMI of this idno!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)

                    Return False

                End If

                'heckBalance()

            End If


        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try


    End Function


    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Check_IdNo()
        'If Check_IdNo() = False Then
        '    scrName = "<SCRIPT language='javascript'>alert('Invalid IdNo!! ');" & "</SCRIPT>"
        '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
        '    '          
        'End If

    End Sub





    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Response.Redirect("EMIReport.aspx")
    End Sub

    Protected Sub TxtMonth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtMonth.TextChanged
        Try

       
            If Val(TxtMonth.Text) > 0 Then
                If Val(TxtFund.Text) > 0 Then
                    TxtROI.Text = Format(Val(TxtFund.Text) / Val(TxtMonth.Text), "0.00")
                Else
                    TxtROI.Text = 0.0
                End If
            Else
                TxtROI.Text = 0.0
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TxtFund_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtFund.TextChanged
        Try
            If Val(TxtMonth.Text) > 0 Then
                If Val(TxtFund.Text) > 0 Then
                    TxtROI.Text = Format(Val(TxtFund.Text) / Val(TxtMonth.Text), "0.00")
                Else
                    TxtROI.Text = 0.0
                End If
            Else
                TxtROI.Text = 0.0
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
