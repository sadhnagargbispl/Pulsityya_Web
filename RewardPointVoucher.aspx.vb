Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Partial Class RewardPointVoucher
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim scrname As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Wallet / Reward Point Voucher "
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))



        If Not Page.IsPostBack Then
            txtMemId.Text = ""
            btnExport.Enabled = False
            If Session("AStatus") = "OK" Then
                'BindSession()
            End If
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
    Protected Sub ApproveData(ByVal sender As Object, ByVal e As System.EventArgs)
        Try


            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            Dim LblWeekNo As New Label
            Dim formno As String
            Dim VoucherNo As String
            Dim Remark As String = ""
            Remark = DirectCast(GVRw.FindControl("TxtRemarks"), TextBox).Text

            formno = DirectCast(GVRw.FindControl("LBlFormno"), Label).Text
            VoucherNo = DirectCast(GVRw.FindControl("LblVoucherNo"), Label).Text
            sql = "Update RewardVoucher Set Isused='Y' ,UsedDate=Getdate(),Remark='" & Remark & "',Userid='" & Session("UserId") & "' where formno='" & formno & "' and VoucherNo='" & VoucherNo & "' and Isused='N'"
            Dim i As Integer = 0
            i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql)
            If i > 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Voucher Used Successfully.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                FillDetail()
            Else
                scrname = "<SCRIPT language='javascript'>alert('Try again later.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                '          

            End If
        Catch ex As Exception

        End Try
    End Sub

    'Public Sub BindSession()
    '    Dim sql As String = "Select * From(Select 0 As SessID,'-- ALL --' As SessnName Union ALL select SessID,'Withdrawal Week '+Cast(SessID As Varchar)+' Date : '+ Replace(Convert(Varchar, frmDate,106),' ','-') +' to '+ Replace(Convert(Varchar, ToDate,106),' ','-') As SessnName from D_sessnmaster Where ToDate Is Not Null) As Temp order by SessID"
    '    objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
    'End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub
    Private Sub FillDetail()
        Dim Idno As String = "0"
        Dim VoucherNo As String = "0"
        If CheckBox1.Checked Then
            If txtMemId.Text <> "" Then
                Idno = Trim(txtMemId.Text)
            Else
                Idno = "0"

            End If
        Else
            Idno = "0"

        End If
        If ChkVoucher.Checked Then
            VoucherNo = TxtVoucher.Text.Trim
        Else
            VoucherNo = "0"
        End If
        Dim startDate As Date
        Dim endDate As Date
        If txtStartDate.Text = "" Then
            startDate = Session("CompDate")
        Else
            startDate = txtStartDate.Text
        End If
        If txtEndDate.Text = "" Then
            endDate = Format(Date.Now, "dd-MMM-yyyy")
        Else
            endDate = txtEndDate.Text
        End If
        Dim prms As SqlParameter() = New SqlParameter(8) {}
        prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
        prms(1) = New SqlParameter("@VoucherNo", Convert.ToString(VoucherNo))
        prms(2) = New SqlParameter("@Status", Convert.ToString(RbtStatus.SelectedValue))
        prms(3) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
        prms(4) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
        prms(5) = New SqlParameter("@PageIndex", 1)
        prms(6) = New SqlParameter("@PageSize", 10000000)
        prms(7) = New SqlParameter("@IsExport", "N")
        prms(8) = New SqlParameter("@RecordCount", ParameterDirection.Output)
        'sql = " exec sp_GetRewardVoucherAdmin '" & Idno & "','" & VoucherNo & "','" & RbtStatus.SelectedValue & "','" & Convert.ToDateTime(startDate) & "','" & Convert.ToDateTime(endDate) & "','1',10,'N',1"
        dtData = New DataTable

        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetRewardVoucherAdmin", prms).Tables(0)
        ' dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()

        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If

    End Sub
    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click


        'GvData.LblTdsAmoun.Visible = False

        'LblTdsAmoun.text = False

        FillDetail()
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Private Sub SendSMS(ByVal Name As String, ByVal ToDate As String, ByVal Fromdate As String, ByVal Amount As String, ByVal Mobile As String)
        Dim client As New WebClient
        Dim baseurl As String
        Dim data As Stream
        'Dear XXXXXXXXXXXX, UR Sales Incentive (Month+Year eg. Sep.2016) Rs.&&&& TRF/NEFT in UR Ac on XXXXXXXX (Date). Thanks Credence Herbal
        ' Dim sms As String = "Dear " & Trim(TxtName.Text) & ", UR Sales Incentive  " & LblPayDate.Text & " Rs." & TxtAmount.Text & " " & TxtRemark.Text & " in UR Ac on " & Format(Date.Today, "dd-MMM-yyyy") & ". Thanks " & Session("CompName") & ""


        Dim sms As String = "Dear " & Name & " your payout generated for period " & Fromdate & " to " & ToDate & " is Rs." & Amount & "  Visit " & Session("CompWeb") & " for more details."
        Try
            'baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Trim(Mobile) & "&msg=" & sms & ""
            baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Trim(Mobile) & "&msg=" & sms & ""
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
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid

            Dim Idno As String = "0"
            Dim VoucherNo As String = "0"
            If CheckBox1.Checked Then
                If txtMemId.Text <> "" Then
                    Idno = Trim(txtMemId.Text)
                Else
                    Idno = "0"

                End If
            Else
                Idno = "0"

            End If
            If ChkVoucher.Checked Then
                VoucherNo = TxtVoucher.Text.Trim
            Else
                VoucherNo = "0"
            End If
            Dim startDate As Date
            Dim endDate As Date
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
            End If
            Dim prms As SqlParameter() = New SqlParameter(8) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@VoucherNo", Convert.ToString(VoucherNo))
            prms(2) = New SqlParameter("@Status", Convert.ToString(RbtStatus.SelectedValue))
            prms(3) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(4) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(5) = New SqlParameter("@PageIndex", 1)
            prms(6) = New SqlParameter("@PageSize", 10000000)
            prms(7) = New SqlParameter("@IsExport", "Y")
            prms(8) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            'sql = " exec sp_GetRewardVoucherAdmin '" & Idno & "','" & VoucherNo & "','" & RbtStatus.SelectedValue & "','" & Convert.ToDateTime(startDate) & "','" & Convert.ToDateTime(endDate) & "','1',10,'N',1"
            dtData = New DataTable

            dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetRewardVoucherAdmin", prms).Tables(0)

            dg.DataSource = dtData
            dg.DataBind()

            ExportToExcel("RewardVoucher.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
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
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub


    Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    End Sub

    'Protected Sub BtnApproveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApproveAll.Click
    '    Try

    '        Dim Chk As CheckBox
    '        Dim cnt As Integer
    '        Dim updateeffect As Integer
    '        Dim StrSql As String = "Insert into Trnrejectbyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
    '        updateeffect = objDAL.SaveData(StrSql)

    '        If updateeffect > 0 Then




    '            Dim LblId As New Label
    '            Dim LblMobl As New Label
    '            Dim LblFromDate As New Label
    '            Dim LblTodate As New Label
    '            Dim txtRemark As New TextBox
    '            If Session("CompID") = 1073 Then
    '                For Each Gvr As GridViewRow In GridView1.Rows
    '                    Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
    '                    If Chk.Checked Then
    '                        LblId.Text = DirectCast(Gvr.FindControl("LblID"), Label).Text
    '                        TxtIDNo.Text = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
    '                        TxtName.Text = DirectCast(Gvr.FindControl("LblPayeeName"), Label).Text
    '                        If Session("CompID") = 1073 Then
    '                            TxtAmount.Text = DirectCast(Gvr.FindControl("LblRequestAmount"), Label).Text
    '                        Else
    '                            TxtAmount.Text = DirectCast(Gvr.FindControl("LblAmount"), Label).Text
    '                        End If
    '                        LblMobl.Text = DirectCast(Gvr.FindControl("Lblmobl"), Label).Text
    '                        LnblWeek.Text = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
    '                        txtRemark.Text = DirectCast(Gvr.FindControl("TxtRemarks"), TextBox).Text
    '                        Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
    '                        Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
    '                        Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
    '                        Dim Remark As String = ""
    '                        Remark = "Withdrawal Approved Of Idno " & TxtIDNo.Text & " For WeekNo:" & LnblWeek.Text & " By " & Session("UserName") & ""
    '                        Dim Sql As String = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
    '                       "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
    '                       " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    '                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

    '                        updateeffect = objDAL.SaveData(Sql)
    '                        'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
    '                        cnt = cnt + 1
    '                    End If
    '                Next
    '            ElseIf Session("CompID") = 1082 Then
    '                For Each Gvr As GridViewRow In GridView1.Rows
    '                    Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
    '                    If Chk.Checked Then
    '                        LblId.Text = DirectCast(Gvr.FindControl("LblID"), Label).Text
    '                        TxtIDNo.Text = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
    '                        TxtName.Text = DirectCast(Gvr.FindControl("LblPayeeName"), Label).Text

    '                        TxtAmount.Text = DirectCast(Gvr.FindControl("LblRequestAmount"), Label).Text


    '                        LblMobl.Text = DirectCast(Gvr.FindControl("Lblmobl"), Label).Text
    '                        LnblWeek.Text = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
    '                        txtRemark.Text = DirectCast(Gvr.FindControl("TxtRemarks"), TextBox).Text
    '                        Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
    '                        Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
    '                        Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
    '                        Dim Remark As String = ""
    '                        Remark = "Withdrawal Approved Of Idno " & TxtIDNo.Text & " For WeekNo:" & LnblWeek.Text & " By " & Session("UserName") & ""
    '                        Dim Sql As String = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
    '                       "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
    '                       " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    '                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

    '                        updateeffect = objDAL.SaveData(Sql)
    '                        'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
    '                        cnt = cnt + 1
    '                    End If
    '                Next
    '            ElseIf Session("CompID") = 1083 Then
    '                For Each Gvr As GridViewRow In GridView1.Rows
    '                    Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
    '                    If Chk.Checked Then
    '                        LblId.Text = DirectCast(Gvr.FindControl("LblID"), Label).Text
    '                        TxtIDNo.Text = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
    '                        TxtName.Text = DirectCast(Gvr.FindControl("LblPayeeName"), Label).Text

    '                        TxtAmount.Text = DirectCast(Gvr.FindControl("LblRequestAmount"), Label).Text

    '                        LblMobl.Text = DirectCast(Gvr.FindControl("Lblmobl"), Label).Text
    '                        LnblWeek.Text = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
    '                        txtRemark.Text = DirectCast(Gvr.FindControl("TxtRemarks"), TextBox).Text
    '                        Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
    '                        Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
    '                        Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
    '                        Dim Remark As String = ""
    '                        Remark = "Withdrawal Approved Of Idno " & TxtIDNo.Text & " For WeekNo:" & LnblWeek.Text & " By " & Session("UserName") & ""
    '                        Dim Sql As String = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
    '                       "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
    '                       " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    '                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

    '                        updateeffect = objDAL.SaveData(Sql)
    '                        'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
    '                        cnt = cnt + 1
    '                    End If
    '                Next
    '            Else
    '                For Each Gvr As GridViewRow In GvData.Rows
    '                    Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
    '                    If Chk.Checked Then
    '                        LblId.Text = DirectCast(Gvr.FindControl("LblID"), Label).Text
    '                        TxtIDNo.Text = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
    '                        TxtName.Text = DirectCast(Gvr.FindControl("LblPayeeName"), Label).Text
    '                        TxtAmount.Text = DirectCast(Gvr.FindControl("LblAmount"), Label).Text
    '                        LblMobl.Text = DirectCast(Gvr.FindControl("Lblmobl"), Label).Text
    '                        LnblWeek.Text = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
    '                        txtRemark.Text = DirectCast(Gvr.FindControl("TxtRemarks"), TextBox).Text
    '                        Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
    '                        Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
    '                        Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
    '                        Dim Remark As String = ""
    '                        Remark = "Withdrawal Approved Of Idno " & TxtIDNo.Text & " For WeekNo:" & LnblWeek.Text & " By " & Session("UserName") & ""
    '                        Dim Sql As String = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
    '                       "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
    '                       " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    '                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

    '                        updateeffect = objDAL.SaveData(Sql)
    '                        'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
    '                        cnt = cnt + 1
    '                    End If
    '                Next

    '            End If


    '            If updateeffect <> 0 Then
    '                scrname = "<SCRIPT language='javascript'>alert('" & cnt & "Withdrawal Approved Successfully.');" & "</SCRIPT>"

    '            Else
    '                scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
    '            End If
    '            Me.RegisterStartupScript("MyAlert", scrname)

    '            ClearALL()
    '            FillDetail()
    '        Else
    '            Response.Redirect("FundWithdrawDateWise.aspx")
    '            'scrname = "<SCRIPT language='javascript'>alert('Try again later.');" & "</SCRIPT>"
    '            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
    '            'Exit Sub
    '        End If
    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Sub








End Class
