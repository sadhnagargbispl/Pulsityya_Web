Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Partial Class App_UI_Application_Pages_RoyaltyWithdrawls
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
            Session("PageName") = " Wallet / Royalty Withdrawls List "
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If Not Page.IsPostBack Then
            txtMemId.Text = ""
            GvData.Visible = False
            btnExport.Enabled = False
            If Session("AStatus") = "OK" Then
                BindSession()
            End If
        End If
    End Sub

    Public Sub BindSession()
        Dim sql As String = "Select * From(Select 0 As SessID,'-- ALL --' As SessnName Union ALL select SessID,'Withdrawal Week '+Cast(SessID As Varchar)+' Date : '+ Replace(Convert(Varchar, frmDate,106),' ','-') +' to '+ Replace(Convert(Varchar, ToDate,106),' ','-') As SessnName from M_MonthSessnMAster Where ToDate Is Not Null) As Temp order by SessID"
        objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
    End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub
    Protected Sub RbtPayment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtPayment.SelectedIndexChanged
        If RbtPayment.SelectedValue = "C" Then
            BankDetail.Visible = False
        Else
            BankDetail.Visible = True

        End If
    End Sub
    Private Sub FillDetail()
        If CheckBox1.Checked = True Then
            Condition = Condition & " And C.IDNo='" & txtMemId.Text & "'"
        End If
        If CheckBox2.Checked = True Then
            If ddlSession.SelectedValue > 0 Then
                Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
            End If
        End If
        If RbtStatus.SelectedValue = "N" Then
            Condition = Condition
        Else
            Condition = Condition & " And a.Status='" & RbtStatus.SelectedValue & "'"
        End If

        'sql = "Select Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, A.Sno,A.WSessID as WeekNo,A.FormNo,C.IdNo As BBAID,C.Mobl,ReqAmount As Amount,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,A.PayeeName,B.BankName,A.AcNo,A.IFSCode,A.PanNo,A.BranchName,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status,Case When Status='A' Or Status='R' Then 'False' Else 'True' end As IsVisible,A.Remark+' On '+dbo.formatDate(A.IssueDate,'dd-MMM-yyyy') As Remark From RoyaltyWithdrawls As A,M_BankMaster As B,M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.BankID=B.BankCode And B.RowStatus='Y' And B.ActiveStatus='Y' And A.FormNo=C.FormNo " & Condition & " Order by A.WSessID"
        sql = " Select A.FormNo,A.Sno,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount" & _
            " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status" & _
           " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-')  " & _
           " + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
           " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
            " c.AcNo, c.IFSCode, B.BankName, c.BranchName, A.PanNo,Case When Status='A' Or Status='R' Then 'False' Else 'True' end As IsVisible,C.Mobl " & _
            " ,Case when ChequeNo<> '' then dbo.FormatDate(ChequeDate,'dd-MMM-yyyy') else '' end  As ChequeDate,ChequeNo,PaymentMode From RoyaltyWithdrawls As A Left join M_BankMaster   As B On  A.BankID=B.BankCode And " & _
         "  B.RowStatus='Y' And B.ActiveStatus='Y' ,M_MemberMaster As C,M_MonthSessnMAster AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & " Order by a.UserId, A.Formno"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        GvData.Visible = True

        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub
    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click


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

            If CheckBox1.Checked = True Then
                Condition = Condition & " And C.IDNo='" & txtMemId.Text & "'"
            End If
            If CheckBox2.Checked = True Then
                If ddlSession.SelectedValue > 0 Then
                    Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
                End If
            End If
            If RbtStatus.SelectedValue = "N" Then
                Condition = Condition
            Else
                Condition = Condition & " And a.Status='" & RbtStatus.SelectedValue & "'"
            End If



            'sql = "Select Row_Number() Over(Order by A.Sno Desc) As SNo,A.WSessID as WeekNo,C.IdNo As BBAID,C.Mobl,ReqAmount As WithDrawAmount,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,A.PayeeName,B.BankName,''''+A.AcNo As AccountNo,A.IFSCode,A.PanNo,A.BranchName,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status,A.Remark+' On '+dbo.formatDate(A.IssueDate,'dd-MMM-yyyy') As Remark From RoyaltyWithdrawls As A,M_BankMaster As B,M_MemberMaster As C Where A.BankID=B.BankCode And B.RowStatus='Y' And B.ActiveStatus='Y'  And A.FormNo=C.FormNo " & Condition & " Order by A.WSessID"
            sql = "Select dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount" & _
                 " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status " & _
                 " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
                " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
                " ''''+A.AcNo as AcNo, A.IFSCode, B.BankName, A.BranchName, A.PanNo From RoyaltyWithdrawls As A Left join M_BankMaster " & _
               "  As B On  A.BankID=B.BankCode And    B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
                " M_MemberMaster As C,M_MonthSessnMAster AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & "  Order by a.UserId, A.Formno"
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("WalletAuthentication.xls", dg)

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
    Protected Sub RejectData(ByVal sender As Object, ByVal e As System.EventArgs)
        DivRemark.Visible = True
        btnReject.Visible = False
        btnApprove.Visible = False
        btnRejectSingle.Visible = True
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        LblRejReqNo.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        lblRejFormno.Text = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
        LblRejDateOn.Text = DirectCast(GVRw.FindControl("LblDate"), Label).Text
        LblRejIdNo.Text = DirectCast(GVRw.FindControl("LblIDNo"), Label).Text
        LblRejWeek.Text = DirectCast(GVRw.FindControl("LblWeek"), Label).Text
    End Sub
    Protected Sub ApproveData(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        Dim LblWeekNo As New Label
        'lblID.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        'TxtIDNo.Text = DirectCast(GVRw.FindControl("LblIDNo"), Label).Text
        'TxtName.Text = DirectCast(GVRw.FindControl("LblPayeeName"), Label).Text
        'TxtAmount.Text = DirectCast(GVRw.FindControl("LblAmount"), Label).Text
        LblFDate.Text = DirectCast(GVRw.FindControl("LblFromDate"), Label).Text
        LblTdate.Text = DirectCast(GVRw.FindControl("LblToDate"), Label).Text
        LblMobil.Text = DirectCast(GVRw.FindControl("Lblmobl"), Label).Text
        LnblWeek.Text = DirectCast(GVRw.FindControl("LblWeek"), Label).Text

        LblForm1.Text = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
        LabelDate1.Text = DirectCast(GVRw.FindControl("LblDate"), Label).Text
        lblID.Text = DirectCast(GVRw.FindControl("LblID"), Label).Text
        TxtIDNo.Text = DirectCast(GVRw.FindControl("LblIDNo"), Label).Text
        TxtName.Text = DirectCast(GVRw.FindControl("LblPayeeName"), Label).Text
        TxtAmount.Text = DirectCast(GVRw.FindControl("LblAmount"), Label).Text
        FillBankMaster()
        txtAccount.Text = DirectCast(GVRw.FindControl("LblAccountNo"), Label).Text
        TxtBranchName.Text = DirectCast(GVRw.FindControl("LblBranch"), Label).Text
        TxtIFSCode.Text = DirectCast(GVRw.FindControl("LblIFsCode"), Label).Text
        DDlBank.SelectedItem.Text = DirectCast(GVRw.FindControl("LblBankname"), Label).Text

        DivTopup.Visible = True
    End Sub
    Private Sub FillBankMaster()
        sql = "SELECT BankCode as Bid,BANKNAME as Bank FROM M_BankMaster WHERE ACTIVESTATUS='Y' ORDER BY BANKCode"
        objModuleFun.FillCombo(sql, DDlBank, "Bank", "Bid")
    End Sub
    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim Remark As String = ""
        Remark = "Withdrawal Approved Of Idno " & TxtIDNo.Text & " For WeekNo:" & LnblWeek.Text & " By " & Session("UserName") & ""
        'Dim Sql As String = "Update RoyaltyWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & TxtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',
        'UserName='" & Session("UserName") & "' Where Sno=" & lblID.Text & ";" & _
        Dim sql As String = "Update RoyaltyWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & TxtRemark.Text & "',PaymentMode='" & RbtPayment.SelectedItem.Text & "', " & _
        " BankId='" & DDlBank.SelectedValue & "',AcNo='" & txtAccount.Text & "',BranchName='" & TxtBranchName.Text & "',IFSCode='" & TxtIFSCode.Text & "',ChequeDate='" & txtChequeDate.Text & "'," & _
        " ChequeNo='" & TxtCheque.Text & "' ,UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where Sno=" & lblID.Text & "  ;" & _
         "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & LblForm1.Text & "' And vtype='W' And VoucherDate='" & LabelDate1.Text & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & LabelDate1.Text & " with req. no." & lblID.Text & "';" & _
        " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & LblForm1.Text & "')"
        If objDAL.SaveData(sql) <> 0 Then
            'SendSMS(TxtName.Text, LblTdate.Text, LblFDate.Text, TxtAmount.Text, LblMobil.Text)
            scrname = "<SCRIPT language='javascript'>alert('Withdrawal Approved Successfully.');" & "</SCRIPT>"
            ClearALL()
            FillDetail()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)
    End Sub
    Private Sub ClearALL()
        lblError.Text = ""
        lblID.Text = ""
        TxtIDNo.Text = ""
        TxtName.Text = ""
        TxtAmount.Text = ""
        TxtRemark.Text = ""
        DivTopup.Visible = False
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ClearALL()
    End Sub
    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        Dim Chk As CheckBox
        Dim cnt As Integer
        Dim updateeffect As Integer
        Dim LblId As New Label
        Dim LblMobl As New Label
        Dim LblFromDate As New Label
        Dim LblTodate As New Label
        Dim txtRemark As New TextBox
        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            If Chk.Checked Then
                LblId.Text = DirectCast(Gvr.FindControl("LblID"), Label).Text
                TxtIDNo.Text = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
                TxtName.Text = DirectCast(Gvr.FindControl("LblPayeeName"), Label).Text
                TxtAmount.Text = DirectCast(Gvr.FindControl("LblAmount"), Label).Text
                LblMobl.Text = DirectCast(Gvr.FindControl("Lblmobl"), Label).Text
                LblFromDate.Text = DirectCast(Gvr.FindControl("LblFromDate"), Label).Text
                LblTodate.Text = DirectCast(Gvr.FindControl("LblToDate"), Label).Text
                LnblWeek.Text = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
                txtRemark.Text = DirectCast(Gvr.FindControl("TxtRemarks"), TextBox).Text
                Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
                Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
                Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
                ' LblPayoutno.Text = DirectCast(Gvr.FindControl("LblWeekNo"), Label).Text
                'LblPayDate.Text = DirectCast(Gvr.FindControl("LblPayoutDate"), Label).Text
                Dim Remark As String = ""
                Remark = "Withdrawal Approved Of Idno " & TxtIDNo.Text & " For WeekNo:" & LnblWeek.Text & " By " & Session("UserName") & ""
                Dim Sql As String = "Update RoyaltyWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where Sno=" & LblId.Text & ";" & _
               "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
               " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

                updateeffect = objDAL.SaveData(Sql)
                'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                cnt = cnt + 1
            End If
        Next

        If updateeffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('" & cnt & "''Withdrawal Approved Successfully.');" & "</SCRIPT>"

        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)

        ClearALL()
        FillDetail()
        DivRemark.Visible = False


    End Sub

    Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    End Sub

    Protected Sub BtnApproveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApproveAll.Click
        'Dim chk As CheckBox
        'For Each Gvr As GridViewRow In GvData.Rows
        '    'Dim sms As String = TxtSMS.Text
        '    chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
        '    'chk = DirectCast(GVRw.FindControl("chkSelect"), CheckBox)
        '    If chk.Checked Then
        '        DivRemark.Visible = True
        '        btnReject.Visible = False
        '        btnApprove.Visible = True
        '        btnRejectSingle.Visible = False
        '        ' Else
        '        '  DivRemark.Visible = False
        '    End If
        'Next
        Dim Chk As CheckBox
        Dim cnt As Integer
        Dim updateeffect As Integer
        Dim LblId As New Label
        Dim LblMobl As New Label
        Dim LblFromDate As New Label
        Dim LblTodate As New Label
        Dim txtRemark As New TextBox
        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            If Chk.Checked Then
                LblId.Text = DirectCast(Gvr.FindControl("LblID"), Label).Text
                TxtIDNo.Text = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
                TxtName.Text = DirectCast(Gvr.FindControl("LblPayeeName"), Label).Text
                TxtAmount.Text = DirectCast(Gvr.FindControl("LblAmount"), Label).Text
                LblMobl.Text = DirectCast(Gvr.FindControl("Lblmobl"), Label).Text
                LblFromDate.Text = DirectCast(Gvr.FindControl("LblFromDate"), Label).Text
                LblTodate.Text = DirectCast(Gvr.FindControl("LblToDate"), Label).Text
                LnblWeek.Text = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
                txtRemark.Text = DirectCast(Gvr.FindControl("TxtRemarks"), TextBox).Text
                Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
                Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
                Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
                ' LblPayoutno.Text = DirectCast(Gvr.FindControl("LblWeekNo"), Label).Text
                'LblPayDate.Text = DirectCast(Gvr.FindControl("LblPayoutDate"), Label).Text
                Dim Remark As String = ""
                Remark = "Withdrawal Approved Of Idno " & TxtIDNo.Text & " For WeekNo:" & LnblWeek.Text & " By " & Session("UserName") & ""
                Dim Sql As String = "Update RoyaltyWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where Sno=" & LblId.Text & ";" & _
               "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & "' ;" & _
               " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

                updateeffect = objDAL.SaveData(Sql)
                'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                cnt = cnt + 1
            End If
        Next

        If updateeffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('" & cnt & "Withdrawal Approved Successfully.');" & "</SCRIPT>"

        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)

        ClearALL()
        FillDetail()
    End Sub

    Protected Sub BtnExportToCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToCsv.Click
        'If CheckBox1.Checked = True Then
        '    Condition = Condition & " And C.IDNo='" & txtMemId.Text & "'"
        'End If
        'If CheckBox2.Checked = True Then
        '    If ddlSession.SelectedValue > 0 Then
        '        Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
        '    End If
        'End If

        'sql = "Select Row_Number() Over(Order by A.Sno Desc) As SNo,A.WSessID as WeekNo,C.IdNo As BBAID,C.Mobl,ReqAmount As WithDrawAmount,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,A.PayeeName,B.BankName,A.AcNo As AccountNo,A.IFSCode,A.PanNo,A.BranchName,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status,A.Remark+' On '+dbo.formatDate(A.IssueDate,'dd-MMM-yyyy') As Remark From RoyaltyWithdrawls As A,M_BankMaster As B,M_MemberMaster As C Where A.BankID=B.BankCode And B.RowStatus='Y' And B.ActiveStatus='Y'  And A.FormNo=C.FormNo " & Condition & " Order by A.WSessID"


        If CheckBox1.Checked = True Then
            Condition = Condition & " And b.IDNo='" & txtMemId.Text & "'"
        End If
        If CheckBox2.Checked = True Then
            If ddlSession.SelectedValue > 0 Then
                Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
            End If
        End If
        If RbtStatus.SelectedValue = "N" Then
            Condition = Condition
        Else
            Condition = Condition & " And a.Status='" & RbtStatus.SelectedValue & "'"
        End If



        'sql = "Select Row_Number() Over(Order by A.Sno Desc) As SNo,A.WSessID as WeekNo,C.IdNo As BBAID,C.Mobl,ReqAmount As WithDrawAmount,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,A.PayeeName,B.BankName,''''+A.AcNo As AccountNo,A.IFSCode,A.PanNo,A.BranchName,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status,A.Remark+' On '+dbo.formatDate(A.IssueDate,'dd-MMM-yyyy') As Remark From RoyaltyWithdrawls As A,M_BankMaster As B,M_MemberMaster As C Where A.BankID=B.BankCode And B.RowStatus='Y' And B.ActiveStatus='Y'  And A.FormNo=C.FormNo " & Condition & " Order by A.WSessID"
        'sql = "Select dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount" & _
        '     " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status " & _
        '     " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
        '    " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
        '    " ''''+A.AcNo as AcNo, A.IFSCode, B.BankName, A.BranchName, A.PanNo From RoyaltyWithdrawls As A Left Join M_BankMaster As B On  A.BankID=B.BankCode And " & _
        '   " B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
        '    " M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & " Order by a.UserId, A.Formno"

        sql = "select b.Idno,WSessid as [Payout Week],''as [Bank A/C No.],'' as [Company Name],'' as [Column1],'' as [Column2],'' as [Column3]," & _
        " a.IFSCode as [IFSC Code],'#'+cast(a.AcNo as Varchar) as [Distributor A/C No],PayeeName as Name,' ' as [Column4],'' as [Column5],'' as [Column6]," & _
        " '' as [Column7],ROW_NUMBER() OVER (ORDER BY Sno) AS SequenceNo,Replace(Convert(Varchar,Reqdate,106),' ','-')As date,NetAmount" & _
        " from RoyaltyWithdrawls as a,M_memberMaster as b   where a.Formno = b.Formno " & Condition & " Order by Sno "

        Dim dt As DataTable = objDAL.GetData(sql)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", _
                "attachment;filename=WalletAuthentication.csv")
        Response.Charset = ""
        Response.ContentType = "application/text"

        Dim sb As New StringBuilder()
        For k As Integer = 0 To dt.Columns.Count - 1
            'add separator
            sb.Append(dt.Columns(k).ColumnName + ","c)
        Next
        'append new line
        sb.Append(vbCr & vbLf)
        For i As Integer = 0 To dt.Rows.Count - 1
            For k As Integer = 0 To dt.Columns.Count - 1
                'add separator
                sb.Append(dt.Rows(i)(k).ToString().Replace(",", ";") + ","c)
            Next
            'append new line
            sb.Append(vbCr & vbLf)
        Next
        Response.Output.Write(sb.ToString())
        Response.Flush()
        Response.End()


    End Sub

    Protected Sub btnRejectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRejectAll.Click
        'Dim chk As CheckBox
        'For Each Gvr As GridViewRow In GvData.Rows
        '    'Dim sms As String = TxtSMS.Text
        '    chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
        '    'chk = DirectCast(GVRw.FindControl("chkSelect"), CheckBox)
        '    If chk.Checked Then
        '        DivRemark.Visible = True
        '        btnReject.Visible = True
        '        btnRejectSingle.Visible = False
        '        btnApprove.Visible = False
        '        ' Else
        '        '  DivRemark.Visible = False
        '    End If
        'Next
        Dim Chk As CheckBox
        Dim cnt As Integer
        Dim updateeffect As Integer
        Dim LblId As New Label
        Dim sql As String = ""
        Dim LblMobl As New Label
        Dim LblFromDate As New Label
        Dim LblTodate As New Label
        Dim Remark As String = ""

        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            If Chk.Checked Then
                Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
                Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
                Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
                Dim IdNo As String = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
                Dim WeekNo As String = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
                Dim TxtRemark As String = DirectCast(Gvr.FindControl("TxtRemarks"), TextBox).Text
                Remark = "Withdrawal Rejected Of Idno " & IdNo & " for WeekNo:" & WeekNo & " By " & Session("UserName") & ""
                sql = sql & "Update RoyaltyWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtRemark & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where Sno=" & Id & ";" & _
  " Delete From TrnVoucher Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & "';"
                sql = sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                        "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Reject','" & Remark & "',Getdate(),'" & FormNo & "')"

                updateeffect = objDAL.SaveData(sql)
                'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                cnt = cnt + 1
            End If
        Next
        If updateeffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('" & cnt & " Withdrawal Rejected Successfully.');" & "</SCRIPT>"
            FillDetail()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)




        ClearALL()
        FillDetail()
        DivRemark.Visible = False


    End Sub

    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReject.Click
        Dim Chk As CheckBox
        Dim cnt As Integer
        Dim updateeffect As Integer
        Dim LblId As New Label
        Dim sql As String = ""
        Dim LblMobl As New Label
        Dim LblFromDate As New Label
        Dim LblTodate As New Label
        Dim Remark As String = ""

        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            If Chk.Checked Then
                Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
                Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
                Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
                Dim IdNo As String = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
                Dim WeekNo As String = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
                Remark = "Withdrawal Rejected Of Idno " & IdNo & " for WeekNo:" & WeekNo & " By " & Session("UserName") & ""
                sql = sql & "Update RoyaltyWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where Sno=" & Id & ";" & _
  " Delete From TrnVoucher Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';"
                sql = sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                        "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Reject','" & Remark & "',Getdate(),'" & FormNo & "')"

                updateeffect = objDAL.SaveData(sql)
                'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                cnt = cnt + 1
            End If
        Next
        If updateeffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('" & cnt & " Withdrawal Rejected Successfully.');" & "</SCRIPT>"
            FillDetail()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)




        ClearALL()
        FillDetail()
        DivRemark.Visible = False

    End Sub

    Protected Sub btnRejectSingle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRejectSingle.Click
        'Dim GVRw As GridViewRow
        'GVRw = CType(sender.Parent.Parent, GridViewRow)
        'Dim Id As String = DirectCast(GVRw.FindControl("LblID"), Label).Text
        'Dim FormNo As String = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
        'Dim DateOn As String = DirectCast(GVRw.FindControl("LblDate"), Label).Text
        'Dim IdNo As String = DirectCast(GVRw.FindControl("LblIDNo"), Label).Text
        'Dim WeekNo As String = DirectCast(GVRw.FindControl("LblWeek"), Label).Text
        Dim Remark As String = ""
        Remark = "Withdrawal Rejected Of Idno " & LblRejIdNo.Text & " for WeekNo:" & LblRejWeek.Text & " By " & Session("UserName") & ""
        Dim Sql As String = "Update RoyaltyWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where Sno=" & LblRejReqNo.Text & ";Delete From TrnVoucher Where DrTo='" & lblRejFormno.Text & "' And vtype='W' And VoucherDate='" & LblRejDateOn.Text & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & LblRejDateOn.Text & " with req. no." & LblRejReqNo.Text & "';"

        Sql = Sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Reject','" & Remark & "',Getdate(),'" & lblRejFormno.Text & "')"
        If objDAL.SaveData(Sql) <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Withdrawal Rejected Successfully.');" & "</SCRIPT>"
            FillDetail()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)

    End Sub
End Class
