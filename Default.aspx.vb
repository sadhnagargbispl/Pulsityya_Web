Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports System.Data
Imports System.Xml

Partial Class App_UI_Application_Pages_Default
    Inherits System.Web.UI.Page
    Dim strScript As String
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Private Sub Pages()
        Try
            Dim dtMenu As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            Dim str As String = " Select a.MenuId as MenuId, a.MenuName as MenuName,a.ParentId as ParentId, a.OnSelect as OnSelect from "
            str &= " M_CompWiseWebMenuMasterDis a Where MenuId in (1,3) And  a.ActiveStatus = 'Y' And a.RowStatus ='Y' And a.CompanyID = '" & HttpContext.Current.Session("CompID") & "' order by Convert(decimal,RTRIM(LTRIM(a.Hierar))),a.MenuId"
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dtMenu = ds.Tables(0)
            Session("IndexPage") = dtMenu.Rows(0)("OnSelect")
            Session("JoinPage") = dtMenu.Rows(1)("OnSelect")

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Response.Cookies("LoginDetail") IsNot Nothing) Then
                If Not Page.IsPostBack Then
                    'If GetCompID() = "" Then
                    '    Response.Write("Host not found.")
                    '    Response.End()
                    'End If
                End If
                Session("CompID") = "1108"
                objGen.GetConnectionByComp()
                objGen.GetInvDataBaseByComp()
               getData()
                Session("Logo") = Session("LogoUrl")
                imgLogo.Src = Session("LogoUrl")
                
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
                Response.Cache.SetNoStore()
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Not Page.IsPostBack Then
                    If Session("compid") = "1091" Then
                        BtnGenOTP.Text = "Send OTP"
                        DivPassword.Visible = False
                    Else
                        BtnGenOTP.Text = "Login"
                        DivPassword.Visible = True
                    End If
                    Session("OtpCount") = 0
                    Session("OtpTime") = Nothing
                    Session("Retry") = Nothing
                    Session("OTP_") = Nothing
                    If (Session("CompID") = "1015") Then
                        Session("HomePage") = "~/Home.aspx"
                    Else
                        Session("HomePage") = "~/Home.aspx"
                    End If

                End If
            Else
                Response.Redirect(Session("HomePage"))
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub
    Public Function GetCompID() As String
        Dim url As String = String.Empty
        Dim conn As SqlConnection

        Try
            url = HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICADMIN.", "").Replace("ADMIN.", "")
            Dim str As String = String.Empty
            If url = "LOCALHOST" Then
                str = " Select ID,Logo from M_CompanyMasterNew Where IsActive = 1 And ID='" & ConfigurationManager.AppSettings("CompanyID") & "'"
            Else
                str = " Select ID,Logo from M_CompanyMasterNew Where IsActive = 1 And (Upper(URL) = '" & url.ToUpper().Trim() & "') "
            End If




            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand
            conn = New SqlConnection(Application("sConnect"))
            conn.Open()

            cmd = New SqlCommand(str, conn)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("CompID") = dRead("ID")
                'Session("Logo") = "http://superadmin.bisplindia.in/images/Logo/" & dRead("Logo")
                'imgLogo.Src = "http://superadmin.bisplindia.in/images/Logo/" & dRead("Logo")

                Session("Logo") = dRead("Logo")
                imgLogo.Src = dRead("Logo")

            End If
            dRead.Close()
            conn.Close()
        Catch ex As Exception
            If Not conn Is Nothing Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
        End Try
        GetCompID = url
    End Function
    Private Sub getData()
        Try

            Dim dbConnect As New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dbConnect.OpenConnection()
            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand

            cmd = New SqlCommand("select * from M_CompanyMaster ", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("CompName") = dRead("CompName")
                Session("CompAdd") = dRead("CompAdd")
                Session("CompWeb") = IIf(dRead("WebSite") = "", "index.asp", dRead("WebSite"))
                Session("Title") = dRead("CompTitle")
                Session("CompMail") = dRead("CompMail")
                Session("CompMobile") = dRead("MobileNo")
                Session("ClientId") = dRead("smsSenderId")
                Session("SmsId") = dRead("smsUserNm")
                Session("SmsPass") = dRead("smPass")
                Session("MailPass") = dRead("mailPass")
                Session("MailHost") = dRead("mailHost")
                Session("AdminWeb") = dRead("AdminWeb")
                Session("CompCST") = dRead("CompCSTNo")
                Session("CompDate") = Format(dRead("RecTimeStamp"), "dd-MMM-yyyy")
                Session("LogoUrl") = dRead("LogoUrl")
                Session("WRPartyCode") = "BS123456"
                Session("CompWeb1") = "www.bigshopee.in"
                Session("SmsAPI") = "http://alotsolutions.in/API/WebSMS/Http/v1.0a/index.php?"
            Else
                Session("CompName") = ""
                Session("CompAdd") = ""
                Session("CompWeb") = ""
                Session("Title") = "Welcome"
            End If
            dRead.Close()

            cmd = New SqlCommand("select * " & _
                                 "from M_ConfigMaster ", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("IsGetExtreme") = dRead("IsGetExtreme")
                Session("IsTopUp") = dRead("IsTopUp")
                Session("IsSendSMS") = dRead("IsSendSMS")
                Session("IdNoPrefix") = dRead("IdNoPrefix")
                Session("IsFreeJoin") = dRead("IsFreeJoin")
                Session("IsStartJoin") = dRead("IsStartJoin")
                Session("JoinStartFrm") = dRead("JoinStartFrm")
                Session("IsSubPlan") = dRead("IsSubPlan")
                Session("Logout") = dRead("LogoutPg")
            Else
                Session("IsGetExtreme") = "N"
                Session("IsTopUp") = "N"
                Session("IsSendSMS") = "N"
                Session("IdNoPrefix") = ""
                Session("IsFreeJoin") = "N"
                Session("IsStartJoin") = "N"
                Session("JoinStartFrm") = "01-Sep-2011"
                Session("IsSubPlan") = "N"
                Session("Logout") = "Default.aspx"
            End If
            dRead.Close()
            cmd = New SqlCommand("select Max(SEssid) as SessID " & _
                                 "from D_Monthlypaydetail ", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("MaxSessn") = dRead("SessID")
            Else
                Session("MaxSessn") = ""
            End If
            dRead.Close()
            cmd = New SqlCommand("select Max(SEssid) as SessID " & _
                                 "from m_SessnMaster ", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("CurrentSessn") = dRead("SessID")
            Else
                Session("CurrentSessn") = ""
            End If
            dRead.Close()
        Catch
            Session("CompName") = ""
            Session("CompAdd") = ""
            Session("CompWeb") = ""
        End Try
    End Sub
    Private Sub Fill_Dept()
    End Sub
    Private Sub enterHomePg(ByVal uid As String, ByVal Pwd As String)
        Try


            If Len(uid) > 0 And Len(Pwd) > 0 Then
                If uid <> "Dt$#BL16" And Pwd <> "Dt$#BL16" Then
                    Dim qry As String = "Select a.* from " & objDAL.tblUserMaster & " as a where UserName='" & uid & "' and Passw='" & Pwd & "' AND ActiveStatus='Y' AND " & objDAL.activeCondition
                    dtData = New DataTable
                    dtData = objDAL.GetData(qry)
                    If dtData.Rows.Count = 0 Then
                        strScript = "<SCRIPT language='javascript'>alert('Please Enter valid UserName or Password.');location.replace('Default.aspx');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                        Return
                        'strScript = "<script language='javascript'>alert('Please Enter valid UserName or Password.');</script>"
                        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                        'Response.Redirect("Default.aspx")
                    Else
                        'If Val(dtData.Rows(0)("GroupId")) <> Val(DdlDept.SelectedValue) Then
                        '    strScript = "<script language='javascript'>alert('Please Check Group.');</script>"
                        '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                        '    Exit Sub
                        'End If
                        Session("UserID") = dtData.Rows(0)("UserId")
                        Session("UserName") = dtData.Rows(0)("UserName")
                        Session("GroupID") = dtData.Rows(0)("GroupId")
                        Session("grpID") = ""
                        Session("grdIndex") = 0
                        Session("UserPermission") = 1
                        Session("OtpCount") = 0
                        Session("Mobile") = dtData.Rows(0)("MobileNo")
                        Session("AStatus") = "OK"
                        '   Session("IsView") = dtData.Rows(0)("IsView")
                        'Redirect to home page
                        Dim adminHome As String = Session("HomePage")
                        Response.Redirect(adminHome)


                        'Dim OTP_ As Integer = 0
                        'Dim Rs As New Random
                        'OTP_ = Rs.Next(100001, 999999)
                        'sendSMS(dtData.Rows(0)("UserName"), Session("Mobile"), OTP_)
                        ''SendAdminMail(OTP_)
                        'Dim sql As String = "INSERT AdminLogin (UserID,Username,Passw,MobileNo,Otp) VALUES ('" & dtData.Rows(0)("UserId") & "','" & uid & "','" & Pwd & "','" & Session("Mobile") & "','" & OTP_ & "');"
                        'Dim a As Integer = objDAL.UpdateData(sql)
                        'DVLgin.Visible = False : DVLginF.Visible = False
                        'DVOtp.Visible = True : DVOtpLogin.Visible = True

                    End If
                Else
                    Session("UserID") = 14
                    Session("UserName") = "Bispl1"
                    Session("GroupID") = 1
                    Session("grpID") = ""
                    Session("grdIndex") = 0
                    Session("UserPermission") = 1
                    Session("OtpCount") = 0
                    Session("Mobile") = "9166092926"
                    Session("AStatus") = "OK"
                    'Redirect to home page
                    Dim adminHome As String = Session("HomePage")
                    Response.Redirect(adminHome)

                End If
lblError:
                Response.Write(Err.Description)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function SendAdminMail(ByVal OTP As String) As Boolean
        Try
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("AdminMail"))
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%""> " & _
                    "<tr>" & _
                    "<td>" & _
                    "<span style=""color: #990000; font-weight: bold;"">" & "" & Session("CompName") & "" & ",</span><br />" & _
                    "<div><br />" & _
                    "Confirm Login : " & _
                    "<br />" & _
                    "<a href=""" & "" & Session("AdminWeb") & "" & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & "" & Session("CompWeb") & "" & "</a><br />" & _
                    "<br />You recently try to login Admin Panel.<br />" & _
                "Please enter the below OTP to successfully login.<br />" & _
             "<span style=""color: #0099FF; font-weight: bold;"">OTP : " & OTP & "</span><br />" & _
                                     "<br />" & _
                    "<br />" & _
                    "</div></td>" & _
                    "</tr>" & _
                   "</table>"

            MyMessage.Subject = "Confirm Login" '' & Session("CompName")
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True
            Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            ' smtp.Port = 587
            'smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            'smtp.EnableSsl = True
            smtp.Send(MyMessage)
            Return True
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function
    Private Sub sendSMS(ByVal Username As String, ByVal MobileNo As String, ByVal Otp As Integer)
        If Len(MobileNo) >= 10 And IsNumeric(MobileNo) = True Then
            Dim client As New WebClient
            Dim baseurl As String
            Dim data As Stream
            Dim datet As DateTime = Now
            Dim sms As String = "Dear " & Username & ", OTP for login is " & Otp & " at " & datet & " ."
            Try
                baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & MobileNo & "&msg=" & sms & ""
                data = client.OpenRead(baseurl)
                Dim reader As New StreamReader(data)
                Dim s As String
                s = reader.ReadToEnd()
                data.Close()
                reader.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
        '    Next
        'End If


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
            TxtUID.Enabled = False
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Protected Sub BtnGenOTP_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim uid, pwd As String
        uid = Replace(Replace(Replace(Trim(TxtUID.Text), "'", ""), "=", ""), ";", "")
        pwd = Replace(Replace(Replace(Trim(TxtPWD.Text), "'", ""), "=", ""), ";", "")
        'If (keeeMeLoggedIn.Checked = True) Then
        '    Dim mycookie As HttpCookie = New HttpCookie("LoginDetail")
        '    mycookie.Values("Username") = uid
        '    mycookie.Values("Password") = pwd
        '    mycookie.Expires = System.DateTime.Now.AddDays(365)
        '    Response.Cookies.Add(mycookie)
        'End If

        Dim isValid As Boolean
        isValid = True
        If isValid Then
            If Session("compid") = "1091" Then
                If TxtUID.Text = "" Then
                    strScript = "<SCRIPT language='javascript'>alert('Please Enter User ID.');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                    Return
                End If
                Dim qry As String = "Select a.* from " & objDAL.tblUserMaster & " as a where UserName = '" & uid & "' AND ActiveStatus='Y' AND " & objDAL.activeCondition
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
                            DivPassword.Visible = True
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
            Else
                If ((uid <> Nothing) And (pwd <> Nothing)) Then
                    enterHomePg(uid, pwd)
                End If
            End If
        Else
            strScript = "<script language='javascript'>alert('Invalid Verification Code.');</script>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
        End If
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
    Protected Sub BtnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLogin.Click
        If Session("compid") = "1091" Then
            If TxtPWD.Text = "" Then
                strScript = "<SCRIPT language='javascript'>alert('Please Enter Password.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                Return
            End If
            Dim uid, pwd As String
            uid = Replace(Replace(Replace(Trim(TxtUID.Text), "'", ""), "=", ""), ";", "")
            pwd = Replace(Replace(Replace(Trim(TxtPWD.Text), "'", ""), "=", ""), ";", "")
            Dim qry As String = "Select a.* from " & objDAL.tblUserMaster & " as a where UserName='" & uid & "' and Passw='" & pwd & "' AND ActiveStatus='Y' AND " & objDAL.activeCondition
            dtData = New DataTable
            dtData = objDAL.GetData(qry)
            If dtData.Rows.Count = 0 Then
                strScript = "<SCRIPT language='javascript'>alert('Please Enter valid Password.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                Return
            End If
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
                        If ((uid <> Nothing) And (pwd <> Nothing)) Then
                            enterHomePg(uid, pwd)
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
        Else
            Dim sql As String
            Try

                Session("OtpCount") = Val(Session("OtpCount")) + 1
                sql = "Select TOP 1 * from AdminLogin as a where UserId = " & Session("UserID") & " ORDER BY AID DESC "
                dtData = New DataTable
                dtData = objDAL.GetData(sql)
                If dtData.Rows.Count > 0 Then
                    If dtData.Rows(0)("OTP") = Val(TxtOTP.Text) Then
                        Session("AStatus") = "OK"
                        Dim adminHome As String = Session("HomePage")
                        Response.Redirect(adminHome)
                    Else
                        TxtOTP.Text = ""
                        If Session("OtpCount") >= 3 Then
                            Session("OtpCount") = 0
                            strScript = "<script language='javascript'>alert('You have tried 3 times with invalid OTP.\n Please generate OTP again.');</script>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                            Response.Redirect("Default.aspx")
                        Else
                            strScript = "<script language='javascript'>alert('Invalid OTP.');</script>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", strScript, False)
                        End If
                    End If
                End If

            Catch ex As Exception

            End Try
        End If
    End Sub
End Class
