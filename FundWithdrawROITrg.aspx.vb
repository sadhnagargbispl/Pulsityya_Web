Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Partial Class App_UI_Application_Pages_FundWithdrawROITrg
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Wallet / Fund Withdrawal ROI "
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Me.btnRejectAll.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.btnRejectAll))
        Me.BtnApproveAll.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnApproveAll))
        If Not Page.IsPostBack Then
            HdnCheckTrnns.Value = GenerateRandomString(6)
            txtMemId.Text = ""
            GvData.Visible = False
            btnExport.Enabled = False
            If Session("AStatus") = "OK" Then
                If Session("CompID") = "1081" Then
                    SelectTypeDiv.Visible = True
                    If ddlsearchtype.SelectedValue = "1" Then
                        LegWiyDiv.Visible = False
                    Else
                        LegWiyDiv.Visible = True
                    End If
                    BindSessionTrueway()
                Else
                    SelectTypeDiv.Visible = False
                    BindSession()
                End If

            End If
        End If
    End Sub
    Public Function GenerateRandomString(ByRef iLength As Integer) As String
        Dim sResult As String = ""
        Dim current_datetime As String = Format(Now(), "yyyyMMddHHmmssfff")
        Dim random_number As Integer = New Random().Next(0, 999)
        Dim formatted_datetime As String = current_datetime & random_number.ToString().PadLeft(3, "0"c)
        sResult = formatted_datetime
        Return sResult
    End Function
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
    Public Sub BindSession()
        Dim sql As String = "Select * From(Select 100000 As SessID,'-- ALL --' As SessnName " & _
        "Union ALL " & _
        "select SessID,'Withdrawal Week '+Cast(SessID As Varchar)+' Date : '+ Replace(Convert(Varchar, frmDate,106),' ','-') +' to '+ Replace(Convert(Varchar, ToDate,106),' ','-') As SessnName " & _
        "from M_sessnmaster Where ToDate Is Not Null and Sessid>84) As Temp order by SessID Desc"
        objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
    End Sub
    Public Sub BindSessionTrueway()
        Dim sql As String = "Select * From(Select 100000 As SessID,'-- ALL --' As SessnName " & _
        "Union ALL " & _
        "select SessID,'Withdrawal Week '+Cast(SessID As Varchar)+' Date : '+ Replace(Convert(Varchar, frmDate,106),' ','-') +' to '+ Replace(Convert(Varchar, ToDate,106),' ','-') As SessnName " & _
        "from M_sessnmaster Where ToDate Is Not Null and Onwebsite='Y' ) As Temp order by SessID Desc"
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
        Dim idno As String = "0"
        Dim Wsessid As String = "0"
        Dim status As String = "Z"
        Dim Legno As String = "0"
        Dim SelectType As String = "0"
        If Session("compid") = "1081" Then
            If txtMemId.Text <> "" Then
                idno = txtMemId.Text
            Else
                idno = "0"
            End If
            If ddlSession.SelectedValue <> 100000 Then
                Wsessid = ddlSession.SelectedValue
            End If
            If RbtStatus.SelectedValue <> "Z" Then
                status = RbtStatus.SelectedValue
            End If
            If ddlsearchtype.SelectedValue <> "0" Then
                SelectType = ddlsearchtype.SelectedValue
            End If
            If DDlLagno.SelectedValue <> "0" Then
                Legno = DDlLagno.SelectedValue
            End If
            sql = "exec sp_GetROIwithdrawalDetail_Update '" & idno & "','" & Wsessid & "','" & status & "'," & SelectType & "," & Legno & ",'N'"
        Else
            If txtMemId.Text <> "" Then
                Condition = Condition & " And C.IDNo='" & txtMemId.Text & "'"
            End If
            If ddlSession.SelectedValue <> 100000 Then
                Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
            End If
            If RbtStatus.SelectedValue = "N" Then
                Condition = Condition
            Else
                Condition = Condition & " And a.Status='" & RbtStatus.SelectedValue & "'"
            End If
            sql = " Select A.FormNo,A.ReqID,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount"
            sql &= " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status"
            sql &= " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-')  "
            sql &= " + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo"
            sql &= " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, "
            sql &= " c.AcNo, c.IFSCode, B.BankName, c.BranchName, A.PanNo,Case When Status='A' Or Status='R' Then 'False' Else 'True' end As IsVisible,C.Mobl "
            sql &= " ,Case when ChequeNo<> '' then dbo.FormatDate(ChequeDate,'dd-MMM-yyyy') else '' end  As ChequeDate,ChequeNo,PaymentMode,"
            sql &= "Case When IsPanVerified = 'N' Then 'PENDING' When IsPanVerified = 'R' Then 'REJECT' When IsPanVerified = 'Y' Then 'APPROVE' End As PanStatus,  "
            sql &= "Case When IsBankVerified = 'N' Then 'PENDING' When IsBankVerified = 'R' Then 'REJECT' When IsBankVerified = 'Y' Then 'APPROVE' End As BankStatus "
            sql &= "From ROIwithdrawls As A Left join M_BankMaster   As B On  A.BankID=B.BankCode And "
            sql &= "  B.RowStatus='Y' And B.ActiveStatus='Y' ,M_MemberMaster As C,Kycverify As KS,m_sESSNmASTER AS D "
            sql &= "Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo and KS.formno  = C.formno " & Condition & " Order by a.UserId, A.Formno"
        End If
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
        If Session("compid") = "1081" Then
            If ddlsearchtype.SelectedValue = 2 Then
                If txtMemId.Text = "" Then
                    Dim scrname As String = ""
                    scrname = "<SCRIPT language='javascript'>alert('Please Enter Member ID.!');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                    Exit Sub
                End If
                FillDetail()
            Else
                FillDetail()
            End If
        Else
            FillDetail()
        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    Protected Sub ddlsearchtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlsearchtype.SelectedIndexChanged
        Try
            If Session("CompId") = "1081" Then
                If ddlsearchtype.SelectedValue = "1" Then
                    LegWiyDiv.Visible = False
                Else
                    LegWiyDiv.Visible = True
                End If
            End If
            GvData.DataSource = Nothing
            GvData.DataBind()
        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim idno As String = "0"
            Dim Wsessid As String = "0"
            Dim status As String = "Z"
            Dim Legno As String = "0"
            Dim SelectType As String = "0"
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            If Session("compid") = "1081" Then
                If txtMemId.Text <> "" Then
                    idno = txtMemId.Text
                Else
                    idno = "0"
                End If
                If ddlSession.SelectedValue <> 100000 Then
                    Wsessid = ddlSession.SelectedValue
                End If
                If RbtStatus.SelectedValue <> "Z" Then
                    status = RbtStatus.SelectedValue
                End If
                If ddlsearchtype.SelectedValue <> "0" Then
                    SelectType = ddlsearchtype.SelectedValue
                End If
                If DDlLagno.SelectedValue <> "0" Then
                    Legno = DDlLagno.SelectedValue
                End If
                sql = "exec sp_GetROIwithdrawalDetail_Update '" & idno & "','" & Wsessid & "','" & status & "'," & SelectType & "," & Legno & ",'Y'"
            else
            If txtMemId.Text <> "" Then
                Condition = Condition & " And C.IDNo='" & txtMemId.Text & "'"
            End If
            If ddlSession.SelectedValue <> 100000 Then
                Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
            End If
            If RbtStatus.SelectedValue = "N" Then
                Condition = Condition
            Else
                Condition = Condition & " And a.Status='" & RbtStatus.SelectedValue & "'"
            End If
                sql = "Select dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount" & _
                     " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status " & _
                     " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15)," & _
                     "CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
                    " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
                    " ''''+A.AcNo as AcNo, A.IFSCode, B.BankName, A.BranchName, A.PanNo,C.Mobl as [Mobile No.]," & _
                    "Case When IsPanVerified = 'N' Then 'PENDING' When IsPanVerified = 'R' Then 'REJECT' When IsPanVerified = 'Y' Then 'APPROVE' End As PanStatus,  " & _
                    "Case When IsBankVerified = 'N' Then 'PENDING' When IsBankVerified = 'R' Then 'REJECT' When IsBankVerified = 'Y' Then 'APPROVE' End As BankStatus " & _
                    "From ROIwithdrawls As A Left join M_BankMaster " & _
                   "  As B On  A.BankID=B.BankCode And    B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
                    " M_MemberMaster As C, Kycverify As KS, m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & _
                     "and KS.formno  = C.formno " & Condition & "  Order by a.UserId, A.Formno"
            End If
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)
            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("FundWithdrawalROI.xls", dg)
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
        Dim sql As String = "Update ROIwithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & TxtRemark.Text & "',PaymentMode='" & RbtPayment.SelectedItem.Text & "', " & _
        " BankId='" & DDlBank.SelectedValue & "',AcNo='" & txtAccount.Text & "',BranchName='" & TxtBranchName.Text & "',IFSCode='" & TxtIFSCode.Text & "',ChequeDate='" & txtChequeDate.Text & "'," & _
        " ChequeNo='" & TxtCheque.Text & "' ,UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & lblID.Text & "  ;" & _
         "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & LblForm1.Text & "' And vtype='W' And VoucherDate='" & LabelDate1.Text & "' And Narration Like 'Fund ROI Debited Against Bank Withdrawal on " & LabelDate1.Text & " with req. no." & lblID.Text & "';" & _
        " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','ROI WithDrawals','ROI Withdrawls Approve','" & Remark & "',Getdate(),'" & LblForm1.Text & "')"
        If objDAL.SaveData(sql) <> 0 Then
            'SendSMS(TxtName.Text, LblTdate.Text, LblFDate.Text, TxtAmount.Text, LblMobil.Text)
            scrname = "<SCRIPT language='javascript'>alert('ROI Withdrawal Approved Successfully.');" & "</SCRIPT>"
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
        Try





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
                    Dim Sql As String = "Update ROIwithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
                   "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'ROI Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
                   " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
             "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','ROI WithDrawals','ROI Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

                    updateeffect = objDAL.SaveData(Sql)
                    'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                    cnt = cnt + 1
                End If
            Next

            If updateeffect > 0 Then
                scrname = "<SCRIPT language='javascript'>alert('" & cnt & "'' ROI Withdrawal Approved Successfully.');" & "</SCRIPT>"

            Else
                scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
            End If
            Me.RegisterStartupScript("MyAlert", scrname)

            ClearALL()
            FillDetail()
            DivRemark.Visible = False

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    End Sub
    Protected Sub BtnApproveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApproveAll.Click
        Try

            Dim dt1 As DataTable = New DataTable()
            Dim Chk As CheckBox
            Dim cnt As Integer
            Dim updateeffect As Integer
            Dim LblId As New Label
            Dim LblMobl As New Label
            Dim LblFromDate As New Label
            Dim LblTodate As New Label
            Dim txtRemark As New TextBox
            Dim Str = "Insert into Trnroitransferbyadmin (Transid) values(" & HdnCheckTrnns.Value & ")"
            'updateeffect = objDAL.SaveData(Str)
            If objDAL.SaveData(Str) > 0 Then


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
                        Dim Sql As String = "Update ROIwithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
                       "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Against Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
                       " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','ROI WithDrawals','ROI Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

                        updateeffect = objDAL.SaveData(Sql)

                        'sql = " Exec sp_RoiApprovepayment '" & FormNo & "','" & Format(Now, "dd-MMM-yyyy") & "','" & FormNo & "',"
                        'sql &= "" & Val(TxtAmount.Text) & ",'Amount Added by Payment  Req.No.:" & LblId.Text & ".',"
                        'sql &= "'Req/" & LblId.Text & "','" & LblId.Text & "','A','" & Session("CurrentSessn") & "',"
                        'sql &= "'" & txtRemark.Text & "','" & Val(Session("UserID")) & "','" & Session("UserName") & "',"
                        'sql &= "'" & Remark & "','" & DateOn & "'"


                        'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                        cnt = cnt + 1
                    End If
                Next
                'dt1 = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings("constr").ConnectionString, CommandType.Text, sql).Tables(0)
                If updateeffect > 0 And cnt > 0 Then
                    scrname = "<SCRIPT language='javascript'>alert('" & cnt & " ROI Withdrawal Approved Successfully.');" & "</SCRIPT>"

                Else
                    scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
                End If

            Else
                scrname = "<SCRIPT language='javascript'>alert('Try Later.!');" & "</SCRIPT>"
            End If
            Me.RegisterStartupScript("MyAlert", scrname)
            ClearALL()
            FillDetail()
        Catch ex As Exception
            Dim message As String = "Try Again.!"
            Dim url As String = "FundWithdrawROITrg.aspx"
            Dim script As String = "window.onload = function(){ alert('"
            script += message
            script += "');"
            script += "window.location = '"
            script += url
            script += "'; }"
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "Redirect", script, True)
        End Try

    End Sub
    Protected Sub btnRejectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRejectAll.Click
        Try


            Dim Chk As CheckBox
            Dim cnt As Integer
            Dim updateeffect As Integer
            Dim LblId As New Label
            Dim sql As String = ""
            Dim LblMobl As New Label
            Dim LblFromDate As New Label
            Dim LblTodate As New Label
            Dim Remark As String = ""
            Dim Str = "Insert into Trnroitransferbyadmin (Transid) values(" & HdnCheckTrnns.Value & ")"
            'updateeffect = objDAL.SaveData(Str)
            If objDAL.SaveData(Str) > 0 Then
                For Each Gvr As GridViewRow In GvData.Rows
                    Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                    If Chk.Checked Then

                        sql = ""
                        sql = String.Empty
                        Dim Id As String = DirectCast(Gvr.FindControl("LblID"), Label).Text
                        Dim FormNo As String = DirectCast(Gvr.FindControl("LblFormNo"), Label).Text
                        Dim DateOn As String = DirectCast(Gvr.FindControl("LblDate"), Label).Text
                        Dim IdNo As String = DirectCast(Gvr.FindControl("LblIDNo"), Label).Text
                        Dim WeekNo As String = DirectCast(Gvr.FindControl("LblWeek"), Label).Text
                        Dim TxtRemark As String = DirectCast(Gvr.FindControl("TxtRemarks"), TextBox).Text
                        Dim LblAmount As String = DirectCast(Gvr.FindControl("LblAmount"), Label).Text
                        Remark = "Withdrawal Rejected Of Idno " & IdNo & " for WeekNo:" & WeekNo & " By " & Session("UserName") & ""


                        sql = sql & "Update ROIwithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtRemark & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & Id & ";"
                        sql = sql & " INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) "
                        sql = sql & "  SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "',0, '" & FormNo & "'," & Val(LblAmount) & ","
                        sql = sql & "  'Withdrawal Rejected  Req. No. " & Id & "','Req/" & Id & "','S','C',convert(varchar,getdate(),112),"
                        sql = sql & "  (Select Max(SessID) from M_SessnMaster) FROM TrnVoucher;"

                        sql = sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                               "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','ROI WithDrawals','ROI Withdrawls Reject','" & Remark & "',Getdate(),'" & FormNo & "')"

                        updateeffect = objDAL.SaveData(sql)

                        'sql = " Exec sp_RoiApprovepayment '" & FormNo & "','" & Format(Now, "dd-MMM-yyyy") & "','" & FormNo & "',"
                        'sql &= "" & Val(TxtAmount.Text) & ",'Withdrawal Rejected  Req. No. " & LblId.Text & "',"
                        'sql &= "'Req/" & LblId.Text & "','" & LblId.Text & "','R','" & Session("CurrentSessn") & "',"
                        'sql &= "'" & txtRemark.Text & "','" & Val(Session("UserID")) & "','" & Session("UserName") & "',"
                        'sql &= "'" & Remark & "',''"


                        'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                        cnt = cnt + 1
                    End If
                Next
                'dt1 = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings("constr").ConnectionString, CommandType.Text, sql).Tables(0)
                If updateeffect > 0 And cnt > 0 Then
                    scrname = "<SCRIPT language='javascript'>alert('" & cnt & " ROI Withdrawal Rejected Successfully.');" & "</SCRIPT>"
                    FillDetail()
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
                End If
            Else
                scrname = "<SCRIPT language='javascript'>alert('Try Later.!');" & "</SCRIPT>"
            End If

            Me.RegisterStartupScript("MyAlert", scrname)
            ClearALL()
            FillDetail()
            DivRemark.Visible = False
        Catch ex As Exception
            Dim message As String = "Try Again.!"
            Dim url As String = "FundWithdrawROITrg.aspx"
            Dim script As String = "window.onload = function(){ alert('"
            script += message
            script += "');"
            script += "window.location = '"
            script += url
            script += "'; }"
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "Redirect", script, True)
        End Try
    End Sub
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReject.Click
        Try


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
                    Dim LblAmount As String = DirectCast(Gvr.FindControl("LblAmount"), Label).Text
                    Remark = "Withdrawal Rejected Of Idno " & IdNo & " for WeekNo:" & WeekNo & " By " & Session("UserName") & ""


                    '' Add New Code New  Credit Entry Insert in TrnVoucher Table
                    sql = sql & "Update ROIwithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & Id & ";"
                    sql = sql & " INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) "
                    sql = sql & "  SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "',0, '" & FormNo & "'," & Val(LblAmount) & ","
                    sql = sql & "  'Withdrawal Rejected  Req. No. " & Id & "','Req/" & Id & "','S','C',convert(varchar,getdate(),112),"
                    sql = sql & "  (Select Max(SessID) from M_SessnMaster) FROM TrnVoucher;"

                    sql = sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                           "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','ROI WithDrawals','ROI Withdrawls Reject','" & Remark & "',Getdate(),'" & FormNo & "')"

                    updateeffect = objDAL.SaveData(sql)
                    'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                    cnt = cnt + 1
                End If
            Next
            If updateeffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('" & cnt & " ROI Withdrawal Rejected Successfully.');" & "</SCRIPT>"
                FillDetail()
            Else
                scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
            End If
            Me.RegisterStartupScript("MyAlert", scrname)




            ClearALL()
            FillDetail()
            DivRemark.Visible = False
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnRejectSingle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRejectSingle.Click

        Dim Remark As String = ""
        Remark = "Withdrawal Rejected Of Idno " & LblRejIdNo.Text & " for WeekNo:" & LblRejWeek.Text & " By " & Session("UserName") & ""
        Dim Sql As String = "Update ROIwithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblRejReqNo.Text & ";Delete From TrnVoucher Where DrTo='" & lblRejFormno.Text & "' And vtype='W' And VoucherDate='" & LblRejDateOn.Text & "' And Narration Like 'Fund Debited Against Bank Withdrawal on " & LblRejDateOn.Text & " with req. no." & LblRejReqNo.Text & "';"

        Sql = Sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Reject','" & Remark & "',Getdate(),'" & lblRejFormno.Text & "')"
        If objDAL.SaveData(Sql) <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('ROI Withdrawal Rejected Successfully.');" & "</SCRIPT>"
            FillDetail()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
        End If
        Me.RegisterStartupScript("MyAlert", scrname)


    End Sub
End Class
