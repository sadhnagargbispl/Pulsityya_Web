Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net.Mail
Imports System.Data.DataTable
Partial Class ApprovePaymentReqsEvent
    Inherits System.Web.UI.Page
    Dim dtData As DataTable = New DataTable()
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Me.btnApprove.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.btnApprove))
        Me.BtnReject.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnReject))
        If Not Page.IsPostBack Then
            If Session("Status") = "OK" Then
                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        TxtMemID.Text = Request.QueryString("key")
                        ChkMem.Checked = True
                        BindData(" AND b.IDNo='" & Request.QueryString("key") & "'")
                    End If
                Else
                    BindData()
                End If
            End If
        End If
    End Sub
    Private Function DisableTheButton(ByVal pge As Control, ByVal btn As Control) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ")
        sb.Append("this.value = 'Please Wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(pge.Page.GetPostBackEventReference(btn))
        sb.Append(";")
        Return sb.ToString()
    End Function
    Public Sub BindData(Optional ByVal Condition As String = "")
        If ChkMem.Checked = True And Trim(TxtMemID.Text) <> "" Then
            Condition = Condition & " AND c.IdNo='" & Trim(TxtMemID.Text) & "'"
        End If
        If RbReqStatus.SelectedValue <> "A" Then
            Condition = Condition & " And a.IsApprove='" & RbReqStatus.SelectedValue & "'"
        End If
        DivRemark.Visible = False
        If txtStartDate.Text <> "" Then
            If CmbType.SelectedValue = "N" Then
                Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
            ElseIf CmbType.SelectedValue = "A" Then
                Condition = Condition & " and Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
            Else
                Condition = Condition & " and Isapprove='" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
            End If
        End If
        If txtEndDate.Text <> "" Then
            If CmbType.SelectedValue = "Y" Then
                Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
            ElseIf CmbType.SelectedValue = "A" Then
                Condition = Condition & " and Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
            Else
                Condition = Condition & " and Isapprove='" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
            End If
        End If
        Dim url As String = String.Empty
        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri
        url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
        url = url.ToLower
        Dim sql As String
        sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype,"
        sql &= " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+ CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,"
        sql &= "a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate,b.BankName,a.Branchname,"
        sql &= "Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected' end as status,"
        sql &= "Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,"
        sql &= "Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile "
        sql &= " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq "
        sql &= " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) "
        sql &= "where ForType = 'V' AND a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo "
        dtData = New DataTable
        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).Tables(0)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then
            btnApproove.Visible = True
            BtnRejects.Visible = True
            btnExport.Visible = True
            lblReqs.Text = dtData.Rows.Count
            lblTotalAmont.Text = dtData.Compute("Sum(Amount)", "")
            LblEmailno.Text = dtData.Rows(0)("Email")
        Else
            btnApproove.Visible = False
            BtnRejects.Visible = False
            btnExport.Visible = False
            lblReqs.Text = "0"
            lblTotalAmont.Text = "0.00"
        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""
        BindData(Condition)
    End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub
    Private Sub CreateBlankTable()
        Session("SmsList") = Nothing
        Dim BlankDt As New DataTable
        BlankDt.Columns.Add("SMS")
        BlankDt.Columns.Add("Mobileno")
        BlankDt.Columns.Add("Idno")
        BlankDt.Columns.Add("ReqNo")
        BlankDt.Columns.Add("Amount")
        Session("SmsList") = BlankDt
        Dim Dt As DataTable = New DataTable()
        Dt.Columns.Add("SMS")
        Dt.Columns.Add("Mobileno")
        Dt.Columns.Add("Idno")
        Dt.Columns.Add("ReqNo")
        Dt.Columns.Add("Amount")
        Dim Dr As DataRow
        Dr = Dt.NewRow
        Dr("SMS") = ""
        Dr("Mobileno") = "0"
        Dr("Idno") = ""
        Dr("ReqNo") = "0"
        Dr("Amount") = "0"
        Dt.Rows.Add(Dr)
    End Sub
    Private Sub AprvAction(ByVal AprvType As String, ByVal ApprvStatus As String)
        Dim sql As String = ""
        Dim Chk As CheckBox
        Dim lbl As Label
        Dim lblReqno As Label
        Dim lblIdno As Label
        Dim LblAmount As Label
        Dim lblPaymode As Label
        Dim LblMobileno As Label
        Dim Cnt As Integer = 0
        Dim Remark As String = ""
        Dim voucherno As String = ""
        Dim dt As DataTable = New DataTable()
        Dim Dt1 As DataTable = New DataTable()
        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
            lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
            lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
            LblAmount = DirectCast(Gvr.FindControl("lblAmount"), Label)
            lblPaymode = DirectCast(Gvr.FindControl("LblPaymode"), Label)
            LblMobileno = DirectCast(Gvr.FindControl("LblMobileno"), Label)
            If Chk.Checked = True And Chk.Enabled = True Then
                Dim strSql As String = "Select Count(*) As Cnt  from WalletReq Where ReqNo = '" & lblReqno.Text & "' and IsApprove <> 'N'"
                Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strSql).Tables(0)
                If (Val(Dt1.Rows(0)("Cnt")) = 0) Then
                    Remark = " Reject Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                    If AprvType = "Y" Then
                        Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                        sql &= "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) "
                        sql &= "SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "','0','" & lbl.Text & "'," & Val(LblAmount.Text) & ","
                        sql &= "'Amount Added by Payment  Req.No.:" & lblReqno.Text & ".','Req/" & lblReqno.Text & "','E','C',Convert(Varchar,Getdate(),112),"
                        sql &= "'" & Session("CurrentSessn") & "' FROM TrnVoucher Where Narration Not in ('Amount Added by Payment  Req.No.:" & lblReqno.Text & ".');"
                    End If
                    sql &= ";Update WalletReq SET  IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApproveBy='" & Val(Session("UserID")) & "',ApproveRemark='" & TxtARemark.Text & "' "
                    sql &= " where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "' AND ForType = 'V' "
                    sql &= "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,Memberid)Values('" & Val(Session("UserID")) & "',"
                    sql &= "'" & Session("UserName") & "','Approve Payment Request ','Approve Payment Request','" & Remark & "',Getdate(),'" & lbl.Text & "')"
                    Cnt = Cnt + 1
                End If
            End If
        Next
        Dim a As Integer
        If sql <> "" Then
            a = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql))
        End If
        Dim MsgTxt As String = ""
        If AprvType = "Y" Then
            MsgTxt = "Approved"
        Else : MsgTxt = "Rejected"
        End If
        If a <> 0 And Cnt > 0 Then
            Session("remark") = TxtARemark.Text
            Session("GETDATE") = DateTime.Now
            lblMsg.Text = "" & Cnt & " Requests " & MsgTxt & " Successfully."
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Green
            TxtARemark.Text = ""
            BindData()
        Else
            lblMsg.Text = " Request Already  " & MsgTxt
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Red
        End If
    End Sub
    Public Function SendToMemberMail() As Boolean
        Try
            Dim sql As String = ""
            Dim userEmail As String = ""
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(LblEmailno.Text)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%""> " & _
                     "<tr>" & _
                     "<td>" & _
"Your Wallet Request has been approved on  " & Session("GETDATE") & ". Please check your wallet." & Session("remark") & " <br />" & _
"<span style=""color: #0099FF; font-weight: bold;"">Regards,</span><br />" & _
                     "<a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompName") & "</a><br />" & _
                     "<br />" & _
                     "<br />" & _
                     "</td>" & _
                     "</tr>" & _
                    "</table>"

            MyMessage.Subject = "Wallet approved!!"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True
            Dim smtp As New Net.Mail.SmtpClient("smtp.gmail.com")
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True
        Catch ex As Exception
            Response.Write("Try later.")
        End Try

    End Function
    Public Function SendToMemberMailRejected() As Boolean
        Try
            'Dim dt As DataTable
            Dim sql As String = ""
            Dim userEmail As String = ""


            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(LblEmailno.Text)
            'Session("EMail")
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%""> " & _
                     "<tr>" & _
                     "<td>" & _
"Your Wallet Request has been Rejected on  " & Session("GETDATE") & " " & Session("remark") & " <br />" & _
"<span style=""color: #0099FF; font-weight: bold;"">Regards,</span><br />" & _
                     "<a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompName") & "</a><br />" & _
                     "<br />" & _
                     "<br />" & _
                     "</td>" & _
                     "</tr>" & _
                    "</table>"
            '"<strong>Sponsor Name: " & MemberName & "</strong><br />" & _
            '        "<strong>Joining Date: " & Passw & "</strong><br />" & _
            '        "<strong>Joining Amount: " & Passw & "</strong><br />" & _
            '"Congratulations, you have successfully registered with  " & Session("CompName") & " !<br />" & _
            '"<br />" & _
            '"Your username and password are given below : <br />" & _
            '"<strong>Login ID: " & IdNo & "</strong><br />" & _
            '"<strong>Password: " & Password & "</strong><br />" & _
            '"Thank You For Registration!!""<br />" & _
            '"You may login to the Member Center at: <a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompWeb") & "</a><br />" & _
            MyMessage.Subject = "Wallet Rejected!!"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True

            'Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            Dim smtp As New Net.Mail.SmtpClient("smtp.gmail.com")
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            'smtp.EnableSsl = False
            smtp.EnableSsl = True
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network

            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True

        Catch ex As Exception
            ' Response.Write(ex.Message)
            Response.Write("Try later.")
        End Try

    End Function
    Protected Sub ChkMem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkMem.CheckedChanged
        TxtMemID.Enabled = ChkMem.Checked
    End Sub
    Protected Sub btnApproove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApproove.Click
        'AprvAction("Y", "A")
        DivRemark.Visible = True
        btnApprove.Visible = True
        BtnReject.Visible = False
    End Sub
    Protected Sub BtnRejects_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRejects.Click
        DivRemark.Visible = True
        btnApprove.Visible = False
        BtnReject.Visible = True
        'AprvAction("R", "R")
    End Sub
    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        If Session("compid") = 1066 Or Session("compid") = 1072 Or Session("compid") = 1073 Or Session("compid") = 1074 Or Session("compid") = 1075 Or Session("compid") = 1077 Or Session("compid") = 1078 Or Session("compid") = 1079 Or Session("compid") = 1082 Or Session("CompId") = 1093 Then

            If TxtARemark.Text = "" Then
                LblARemark.Visible = True
                LblARemark.Text = "Please Enter Remark"
                Exit Sub
            End If
            LblARemark.Visible = False
        Else
            If TxtARemark.Text = "" Then
                LblARemark.Visible = True
                LblARemark.Text = "Please Enter Remark"
                Exit Sub
            End If
            LblARemark.Visible = False
        End If
        AprvAction("Y", "A")
        If Session("compid") = 1066 Or Session("compid") = 1072 Or Session("compid") = 1073 Or Session("compid") = 1074 Or Session("compid") = 1075 Or Session("compid") = 1082 Then
        ElseIf Session("compid") <> 1001 Then
        Else
            SendToMemberMail()
        End If

    End Sub
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click
        If Session("compid") = 1066 Or Session("compid") = 1072 Or Session("compid") = 1073 Or Session("compid") = 1074 Or Session("compid") = 1075 Or Session("compid") = 1078 Or Session("compid") = 1082 Or Session("CompId") = 1093 Then

            If TxtARemark.Text = "" Then
                LblARemark.Visible = True
                LblARemark.Text = "Please Enter Remark"
                Exit Sub
            End If
            LblARemark.Visible = False

        Else
            If TxtARemark.Text = "" Then
                LblARemark.Visible = True
                LblARemark.Text = "Please Enter Remark"
                Exit Sub
            End If
            LblARemark.Visible = False
        End If
        AprvAction("R", "R")
        If Session("compid") = 1066 Or Session("compid") = 1072 Or Session("compid") = 1073 Or Session("compid") = 1074 Or Session("compid") = 1075 Or Session("compid") = 1082 Then

        ElseIf Session("compid") <> 1001 Then

        Else
            SendToMemberMailRejected()
        End If


    End Sub
    Protected Sub Sendsms(ByVal sms As String, ByVal Mobl As String)
        If Mobl.Length > 9 Then


            Dim client As New WebClient
            Dim baseurl As String
            Dim data As Stream
            'Dim Sms As String = " Welcome to " & Session("CompName") & ". Your login details are ID: " & Session("SMSIDNo") & "/Login Pwd:" & MemberPass & "/Trans Pwd:" & MemberTransPassw & " Visit " & Session("CompWeb1") & " for more details."
            '   Welcome To *, Thank You For Registration.Your ID Is * and Password is *. Visit .* Best of luck.
            'sms = "Welcome To " & Session("CompName") & ", Thank You For Registration.Your ID Is " & Session("SMSIDNo") & " and Password is " & Session("SMSIDPass") & ". Visit " & Session("CompWeb1") & "  Best of luck."
            Try
                baseurl = "http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & Mobl & "&SenderId=" & Session("ClientId") & ""
                ' baseurl = "http://103.233.79.246//submitsms.jsp?user=" & Session("SmsId") & "&key=5d0f10e5faXX&mobile=" & txtMobileNo.Text.Trim & "&message=" & sms & "&senderid=" & Session("ClientId") & "&accusage=1"
                data = client.OpenRead(baseurl)
                Dim reader As New StreamReader(data)
                Dim s As String
                s = reader.ReadToEnd()
                data.Close()
                reader.Close()
            Catch ex As Exception
                'MsgBox(ex.Message)
            End Try
        End If
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim Condition As String = ""
            If ChkMem.Checked = True And Trim(TxtMemID.Text) <> "" Then
                Condition = Condition & " AND c.IdNo='" & Trim(TxtMemID.Text) & "'"
            End If
            If RbReqStatus.SelectedValue <> "A" Then
                Condition = Condition & " And a.IsApprove='" & RbReqStatus.SelectedValue & "'"
            End If
            DivRemark.Visible = False
            If txtStartDate.Text <> "" Then
                If CmbType.SelectedValue = "N" Then
                    Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
                ElseIf CmbType.SelectedValue = "A" Then
                    Condition = Condition & " and Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
                Else
                    Condition = Condition & " and Isapprove='" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
                End If
            End If
            If txtEndDate.Text <> "" Then
                If CmbType.SelectedValue = "Y" Then
                    Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
                ElseIf CmbType.SelectedValue = "A" Then
                    Condition = Condition & " and Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
                Else
                    Condition = Condition & " and Isapprove='" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
                End If
            End If
            Dim dg As New DataGrid
            Dim url As String = String.Empty
            Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri
            url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
            url = url.ToLower
            Dim sql As String = " select c.Idno as [ID No.],(c.MemFirstName+ ' '+c.MemlastName) as [Member Name], "
            sql &= " Mobl as [Mobile No], Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+   CONVERT(varchar(15),"
            sql &= " CAST(a.RectimeStamp AS TIME),100) as [Request Date],a.PayMode as [Payment Mode],  "
            sql &= " ''''+a.Chqno as [Cheque/ TransactionNo],Replace(Convert(Varchar,a.ChqDate,106),' ','-') as [Cheque/ Transaction Date],"
            sql &= "    b.BankName as [Bank Name],a.Branchname as [Branch Name],Amount,a.Remarks as [Member Remark], "
            sql &= " Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected' end as status, "
            sql &= " ApproveRemark as [User Remark] from WalletReq "
            sql &= " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where ForType = 'V' AND  a.Formno=c.Formno and "
            sql &= " a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & " order by ReqNo "
            Dim dtTemp As DataTable = New DataTable()
            dtTemp = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).Tables(0)
            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("VadicGurukulEvent.xls", dg)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
        Dim sw As New System.IO.StringWriter
        Dim htw As System.Web.UI.HtmlTextWriter
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
        Response.Charset = ""
        dg.EnableViewState = False
        htw = New HtmlTextWriter(sw)
        dg.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub
End Class
