Imports System.Data
Imports System.Data.SqlClient
Partial Class DispatchStatus
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim CatIDQS As String
    Dim SType As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("logout.aspx")
        End If
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("OrderID")) = False Then
            CatIDQS = Request("OrderID")
            SType = Request("type")
        End If
        Dim str = "exec('Create table Trnrejectbyadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
 "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnfundtransferbyadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
        Dim i As Integer = 0
        i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
        If Not Page.IsPostBack Then
            If Session("UserName").ToString().ToUpper() = "ADMIN" Then
                BtnSave.Text = "Dispatch"
            Else
                BtnSave.Text = "Send OTP"
            End If
            Session("OtpCount") = 0
            Session("OtpTime") = Nothing
            Session("Retry") = Nothing
            Session("OTP_") = Nothing
            HdnCheckTrnns.Value = GenerateRandomString(6)
            If Session("AStatus") = "OK" Then
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        ' txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub
    Function CreateOtpMessage(ByVal email As String) As String
        Dim masked As String = MaskEmail(email)
        Return "OTP sent to " & masked & "."
    End Function
    Function MaskEmail(ByVal email As String) As String
        Dim emailPattern As String = "^[^@\s]+@[^@\s]+\.[^@\s]+$"
        If Not Regex.IsMatch(email, emailPattern) Then
            Return email
        End If
        Dim atPos As Integer = email.IndexOf("@"c)
        If atPos <= 0 Then Return email
        Dim localPart As String = email.Substring(0, atPos)
        Dim domainPart As String = email.Substring(atPos)
        Dim visibleCount As Integer = Math.Min(5, localPart.Length)
        Dim visible As String = localPart.Substring(0, visibleCount)
        Dim starsCount As Integer = Math.Max(0, localPart.Length - visibleCount)
        Dim stars As String = New String("*"c, starsCount)
        Return visible & stars & domainPart
    End Function
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        If txtDispatchDate.Text = "" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Dispatch Date.!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Exit Sub
        ElseIf TxtDispatchRemark.Text = "" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Dispatch Remark.!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Exit Sub
        End If
        If Session("compid") = "1108" Or Session("compid") = "1103" Or Session("compid") = "1105" Then
            Dim Sql As String
            Dim updateeffects As Integer
            Dim StrSql As String = "Insert into Trnrejectbyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
            updateeffects = objDAL.SaveData(StrSql)
            If updateeffects > 0 Then
                If rdblist.SelectedIndex = 0 Then
                    txtActiveStatus.Text = "Y"
                Else
                    txtActiveStatus.Text = "N"
                End If
                Sql = "Update purchaseReq  set UserStatus = '" & rdblist.SelectedValue & "',ApproveRemark = '" & TxtDispatchRemark.Text & "',Approvedate = '" & txtDispatchDate.Text & "', "
                Sql &= "LastModified = 'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' where id = '" & Request("OrderID") & "' AND formno = '" & Request("FormNo") & "' "
                Dim updateEffect As Integer = 0
                updateEffect = objDAL.UpdateData(Sql)
                If updateEffect > 0 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Successfully Dispatch.!'); window.parent.location.href='Dispatchproductreport.aspx';", True)
                    Exit Sub
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Data not Dispatch.!'); window.parent.location.href='Dispatchproductreport.aspx';", True)
                    Exit Sub
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Try Againg After Some Time.!'); window.parent.location.href='Dispatchproductreport.aspx';", True)
                Exit Sub
            End If
        Else
            If Session("UserName").ToString().ToUpper() = "ADMIN" Then
                Dim Sql As String
                Dim updateeffects As Integer
                Dim StrSql As String = "Insert into Trnrejectbyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
                updateeffects = objDAL.SaveData(StrSql)
                If updateeffects > 0 Then
                    If rdblist.SelectedIndex = 0 Then
                        txtActiveStatus.Text = "Y"
                    Else
                        txtActiveStatus.Text = "N"
                    End If
                    Sql = "Exec [DispatchOrder] '" & Request("OrderID") & "','','0','','','','" & txtDispatchDate.Text & "','" & rdblist.SelectedValue & "','" & TxtDispatchRemark.Text & "','" & rdblist.SelectedValue & "' "
                    Dim updateEffect As Integer = 0
                    updateEffect = objDAL.UpdateData(Sql)
                    If updateEffect > 0 Then
                        If SType = "A" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Successfully Dispatch.!'); window.parent.location.href='ApproveProductRequest.aspx';", True)
                            Exit Sub
                        Else
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Successfully Dispatch.!'); window.parent.location.href='WithoutApproveProductReport.aspx';", True)
                            Exit Sub
                        End If
                    Else
                        If SType = "A" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Data not Dispatch.!'); window.parent.location.href='ApproveProductRequest.aspx';", True)
                            Exit Sub
                        Else
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Data not Dispatch.!'); window.parent.location.href='WithoutApproveProductReport.aspx';", True)
                            Exit Sub
                        End If
                    End If
                Else
                    If SType = "A" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Try Againg After Some Time.!'); window.parent.location.href='ApproveProductRequest.aspx';", True)
                        Exit Sub
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Try Againg After Some Time.!'); window.parent.location.href='WithoutApproveProductReport.aspx';", True)
                        Exit Sub
                    End If
                End If
            Else
                Dim strScript As String
                Dim dtData As New DataTable
                Dim qry As String = "Select a.* from " & objDAL.tblUserMaster & " as a where UserName = '" & Session("UserName") & "' AND ActiveStatus='Y' AND " & objDAL.activeCondition
                dtData = New DataTable
                dtData = objDAL.GetData(qry)
                If dtData.Rows.Count = 0 Then
                    strScript = "<SCRIPT language='javascript'>alert('Please Enter valid User ID.');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                    Return
                Else
                    Session("USerEmail") = dtData.Rows(0)("Email")
                End If
                Dim OTP_ As Integer = 0
                Dim rs As New Random()
                OTP_ = rs.Next(100001, 999999)
                If Session("OTP_") Is Nothing Then
                    If SendMail(OTP_.ToString(), Session("USerEmail")) Then
                        Session("OtpTime") = DateTime.Now.AddMinutes(5)
                        Session("Retry") = "1"
                        Session("OTP_") = OTP_
                        Dim i As Integer = 0
                        Dim query As String = ""
                        query = "INSERT INTO AdminLogin (UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID) "
                        query &= "VALUES ('0', '', '" & TxtOTP.Text & "', '0', '" & OTP_ & "', GETDATE(), '" & OTP_ & "', "
                        query &= "'" & Session("USerEmail") & "')"
                        i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, query))
                        If i > 0 Then
                            DVOtp.Visible = True
                            DVLginF.Visible = False
                            DVOtpLogin.Visible = True
                            LblEmailMsg.Text = CreateOtpMessage(Session("USerEmail"))
                            Dim scrname As String = "<script language='javascript'>alert('OTP Sent On Mail');</script>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                            Return
                        Else
                            Dim scrname As String = "<script language='javascript'>alert('Try Later');</script>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                            Return
                        End If
                    Else
                        Dim scrname As String = "<script language='javascript'>alert('OTP Try Later');</script>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                        Return
                    End If
                End If
            End If
        End If
    End Sub
    Public Function SendMail(ByVal otp As String, ByVal emailAddress As String) As Boolean
        Try
            Dim strMsg As String = ""
            'Dim emailAddress As String = "gangaram3575@gmail.com"
            Dim sendFrom As New System.Net.Mail.MailAddress(Session("CompMail").ToString())
            Dim sendTo As New System.Net.Mail.MailAddress(emailAddress)
            Dim myMessage As New System.Net.Mail.MailMessage(sendFrom, sendTo)
            strMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%""> " & _
                    "<tr>" & _
                    "<td>" & _
                    "<span style=""color: #990000; font-weight: bold;"">" & "" & Session("CompName") & "" & ",</span><br />" & _
                    "<div><br />" & _
                    "Confirm Login : " & _
                    "<br />" & _
                    "<a href=""" & "" & Session("AdminWeb") & "" & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & "" & Session("CompWeb") & "" & "</a><br />" & _
                    "<br />You recently try to login Admin Panel.<br />" & _
                "Please enter the below OTP to successfully login.<br />" & _
             "<span style=""color: #0099FF; font-weight: bold;"">OTP : " & otp & "</span><br />" & _
                                     "<br />" & _
                    "<br />" & _
                    "</div></td>" & _
                    "</tr>" & _
                   "</table>"

            myMessage.Subject = "Confirm Login"
            myMessage.Body = strMsg
            myMessage.IsBodyHtml = True
            Dim smtp As New System.Net.Mail.SmtpClient(Session("MailHost").ToString())
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
            smtp.Credentials = New System.Net.NetworkCredential(Session("CompMail").ToString(), Session("MailPass").ToString())
            smtp.Send(myMessage)
            txtDispatchDate.Enabled = False
            TxtDispatchRemark.Enabled = False
            rdblist.Enabled = False
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub BtnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLogin.Click
        Dim strScript As String
        Dim dtData As New DataTable
        If TxtOTP.Text = "" Then
            strScript = "<SCRIPT language='javascript'>alert('Please Enter OTP.');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
            Return
        End If
        Try
            Dim scrname As String = ""
            Dim email As String = ""
            Dim dt As New DataTable()
            Dim transPassw As String = TxtOTP.Text.Trim()
            Dim dt1 As New DataTable()
            Dim emailAddress As String = Session("USerEmail")
            Session("OtpCount") = Convert.ToInt32(Session("OtpCount")) + 1
            If Session("OTP_") IsNot Nothing AndAlso Session("OTP_").ToString() = transPassw Then
                Dim query As String = "SELECT TOP 1 * FROM AdminLogin AS a WHERE EmailID = '" & emailAddress.ToString().Trim() & "' AND emailotp = '" & transPassw & "' ORDER BY AID DESC"
                dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, query).Tables(0)
                If dt1.Rows.Count > 0 Then
                    Dim Sql As String
                    Dim updateeffects As Integer
                    Dim StrSql As String = "Insert into Trnrejectbyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
                    updateeffects = objDAL.SaveData(StrSql)
                    If updateeffects > 0 Then
                        If rdblist.SelectedIndex = 0 Then
                            txtActiveStatus.Text = "Y"
                        Else
                            txtActiveStatus.Text = "N"
                        End If
                        Sql = "Exec [DispatchOrder] '" & Request("OrderID") & "','','0','','','','" & txtDispatchDate.Text & "','" & rdblist.SelectedValue & "','" & TxtDispatchRemark.Text & "','" & rdblist.SelectedValue & "' "
                        Dim updateEffect As Integer = 0
                        updateEffect = objDAL.UpdateData(Sql)
                        If updateEffect > 0 Then
                            If SType = "A" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Successfully Dispatch.!'); window.parent.location.href='ApproveProductRequest.aspx';", True)
                                Exit Sub
                            Else
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Successfully Dispatch.!'); window.parent.location.href='WithoutApproveProductReport.aspx';", True)
                                Exit Sub
                            End If
                        Else
                            If SType = "A" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Data not Dispatch.!'); window.parent.location.href='ApproveProductRequest.aspx';", True)
                                Exit Sub
                            Else
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Data not Dispatch.!'); window.parent.location.href='WithoutApproveProductReport.aspx';", True)
                                Exit Sub
                            End If
                        End If
                    Else
                        If SType = "A" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Try Againg After Some Time.!'); window.parent.location.href='ApproveProductRequest.aspx';", True)
                            Exit Sub
                        Else
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Try Againg After Some Time.!'); window.parent.location.href='WithoutApproveProductReport.aspx';", True)
                            Exit Sub
                        End If
                    End If
                End If
            Else
                TxtOTP.Text = ""
                If Convert.ToInt32(Session("OtpCount")) >= 3 Then
                    Session("OtpCount") = 0
                    scrname = "<SCRIPT language='javascript'>alert('You have tried 3 times with invalid OTP.\nPlease generate OTP again.');location.replace('Default.aspx');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                    Return
                Else
                    scrname = "<script language='javascript'>alert('Invalid OTP.');</script>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                    Return
                End If
            End If

        Catch ex As Exception
            ' Handle exception (log or display message)
        End Try
    End Sub
End Class
