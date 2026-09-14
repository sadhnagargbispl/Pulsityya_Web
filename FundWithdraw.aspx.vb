Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Partial Class App_UI_Application_Pages_FundWithdraw
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
            Session("PageName") = " Wallet / Wallet Authentication "
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        Me.btnRejectAll.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.btnRejectAll))
        Me.BtnApproveAll.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnApproveAll))
        Dim str = "exec('Create table Trnrejectbyadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
"ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnfundtransferbyadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
        Dim i As Integer = 0
        i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
        If Not Page.IsPostBack Then
            HdnCheckTrnns.Value = GenerateRandomString(6)
            txtMemId.Text = ""
            GvData.Visible = False
            btnExport.Enabled = False
            If Session("AStatus") = "OK" Then
                BindSession()
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
    Private Function DisableTheButton(pge As Control, btn As Control) As String
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
        Dim sql As String = "Select * From(Select 100000 As SessID,'-- ALL --' As SessnName Union ALL select SessID,'Withdrawal Week '+Cast(SessID As Varchar)+' Date : '+ Replace(Convert(Varchar, frmDate,106),' ','-') +' to '+ Replace(Convert(Varchar, ToDate,106),' ','-') As SessnName from M_sessnmaster Where ToDate Is Not Null) As Temp order by SessID Desc"
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
            If ddlSession.SelectedValue <> 100000 Then
                Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
            End If
        End If
        If RbtStatus.SelectedValue = "N" Then
            Condition = Condition
        Else
            Condition = Condition & " And a.Status='" & RbtStatus.SelectedValue & "'"
        End If

        'sql = "Select Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, A.ReqID,A.WSessID as WeekNo,A.FormNo,C.IdNo As BBAID,C.Mobl,ReqAmount As Amount,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,A.PayeeName,B.BankName,A.AcNo,A.IFSCode,A.PanNo,A.BranchName,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status,Case When Status='A' Or Status='R' Then 'False' Else 'True' end As IsVisible,A.Remark+' On '+dbo.formatDate(A.IssueDate,'dd-MMM-yyyy') As Remark From Fundwithdrawls As A,M_BankMaster As B,M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.BankID=B.BankCode And B.RowStatus='Y' And B.ActiveStatus='Y' And A.FormNo=C.FormNo " & Condition & " Order by A.WSessID"
        If Session("CompId") = 1078 Or Session("CompId") = 1093 Then
            sql = " Select A.FormNo,A.ReqID,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount,AdminCharge,NetAmount" & _
            " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status" & _
           " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-')  " & _
           " + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
           " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
            " c.AcNo, c.IFSCode, B.BankName, c.BranchName, A.PanNo,Case When Status='A' Or Status='R' Then 'False' Else 'True' end As IsVisible,C.Mobl " & _
            " ,Case when ChequeNo<> '' then dbo.FormatDate(ChequeDate,'dd-MMM-yyyy') else '' end  As ChequeDate,ChequeNo,PaymentMode From Monthwithdrawls As A Left join M_BankMaster   As B On  A.BankID=B.BankCode And " & _
         "  B.RowStatus='Y' And B.ActiveStatus='Y' ,M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & " Order by a.UserId, A.Formno"

        ElseIf (Session("CompID") = "1106") Then
            sql = " Select A.FormNo,A.ReqID,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount,AdminCharge,NetAmount " & _
" ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status" & _
" ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-')  " & _
" + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
" ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
" c.AcNo, c.IFSCode, B.BankName, c.BranchName, A.PanNo,Case When Status='A' Or Status='R' Then 'False' Else 'True' end As IsVisible,C.Mobl " & _
" ,Case when ChequeNo<> '' then dbo.FormatDate(ChequeDate,'dd-MMM-yyyy') else '' end  As ChequeDate,ChequeNo,PaymentMode From Monthwithdrawls As A Left join M_BankMaster   As B On  A.BankID=B.BankCode And " & _
"  B.RowStatus='Y' And B.ActiveStatus='Y' ,M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & " Order by a.UserId, A.Formno"
        Else
            sql = " Select A.FormNo,A.ReqID,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount,AdminCharge,NetAmount " & _
            " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status" & _
           " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-')  " & _
           " + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
           " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
            " c.AcNo, c.IFSCode, B.BankName, c.BranchName, A.PanNo,Case When Status='A' Or Status='R' Then 'False' Else 'True' end As IsVisible,C.Mobl " & _
            " ,Case when ChequeNo<> '' then dbo.FormatDate(ChequeDate,'dd-MMM-yyyy') else '' end  As ChequeDate,ChequeNo,PaymentMode From Fundwithdrawls As A Left join M_BankMaster   As B On  A.BankID=B.BankCode And " & _
         "  B.RowStatus='Y' And B.ActiveStatus='Y' ,M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & " Order by a.UserId, A.Formno"
        End If
     

     
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()

        If Session("CompID") = "1106" Then
            GvData.Columns(8).Visible = False
            GvData.Columns(9).Visible = False
        End If
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
                If ddlSession.SelectedValue <> 100000 Then
                    Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
                End If
            End If
            If RbtStatus.SelectedValue = "N" Then
                Condition = Condition
            Else
                Condition = Condition & " And a.Status='" & RbtStatus.SelectedValue & "'"
            End If
            If (Session("CompID") = 1038) Then
                sql = " Select ''''+Convert(Varchar,GetDate(),112) +'09790001' As Transaction_Ref_No,NetAmount As Amount,AdminCharge,NetAmount,"
                sql &= " Convert(Varchar, Getdate(),103) As Value_Date,	''''+ '0350' As Branch_Code,'11' As Sender_Account_Type,"
                sql &= " ''''+'0005020100004757' As Remitter_Account_No,'UCM Ventures Private Limited' As Remitter_Name,a.IFSCode As IFSC_Code,"
                sql &= " ''''+ '0005020100004757' As Debit_Account, '10' As Beneficiary_Account_type, ''''+ Convert(Varchar,a.AcNo) As Bank_Account_Number,"
                sql &= " PayeeName  As Beneficiary_Name,'SALARY' As Remittance_Details, '11' As Debit_Account_System,"
                sql &= " 'UCM Ventures Private Limited' As Originator_Of_Remmittance"
                sql &= " from Fundwithdrawls As a, M_MemberMaster As b   Where a.Formno = b.Formno " & Condition & " Order by ReqId "
            ElseIf (Session("CompID") = 1007) Then
                sql = "Select dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount,AdminCharge,NetAmount,C.IdNo +'-'+ A.PayeeName As [Payee Name With Id Number] " & _
                " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status " & _
                " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
               " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
               " ''''+A.AcNo as AcNo, A.IFSCode, B.BankName, A.BranchName, A.PanNo From Fundwithdrawls As A Left join M_BankMaster " & _
              "  As B On  A.BankID=B.BankCode And    B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
               " M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & "  Order by a.UserId, A.Formno"
            ElseIf (Session("CompID") = 1049) Then
                sql = "Select Row_Number() Over(Order by A.ReqID Desc) As SNo, memfirstname As Name, ''''+A.AcNo as [Account No], A.IFSCode As [IFSC Code], NetAmount As Amount,AdminCharge,NetAmount From Fundwithdrawls As A Left join M_BankMaster " & _
              "  As B On  A.BankID=B.BankCode And    B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
               " M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & "  Order by a.Reqid desc"
            ElseIf (Session("CompID") = 1078) Then
                sql = "Select dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount,AdminCharge,NetAmount" & _
                 " ,Case when a.Status='P' Then '' else A.Remark end As Remark," & _
                " B.BankName, A.BranchName,''''+A.AcNo as AcNo, A.IFSCode, A.PanNo,Case when ChequeNo<> '' then dbo.FormatDate(ChequeDate,'dd-MMM-yyyy') else '' end  As ChequeDate,ChequeNo,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status, " & _
                " UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate From Monthwithdrawls As A Left join M_BankMaster " & _
               "  As B On  A.BankID=B.BankCode And    B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
                " M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & "  Order by a.UserId, A.Formno"
            ElseIf (Session("CompID") = 1093) Then
                sql = "Select dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount,AdminCharge,NetAmount" & _
                 " ,Case when a.Status='P' Then '' else A.Remark end As Remark," & _
                " B.BankName, A.BranchName,''''+A.AcNo as AcNo, A.IFSCode, A.PanNo,Case when ChequeNo<> '' then dbo.FormatDate(ChequeDate,'dd-MMM-yyyy') else '' end  As ChequeDate,ChequeNo,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status, " & _
                " UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate From Monthwithdrawls As A Left join M_BankMaster " & _
               "  As B On  A.BankID=B.BankCode And    B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
                " M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & "  Order by a.UserId, A.Formno"
            Else
                sql = "Select dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount,AdminCharge,NetAmount " & _
                 " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status " & _
                 " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
                " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
                " ''''+A.AcNo as AcNo, A.IFSCode, B.BankName, A.BranchName, A.PanNo From Fundwithdrawls As A Left join M_BankMaster " & _
               "  As B On  A.BankID=B.BankCode And    B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
                " M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & "  Order by a.UserId, A.Formno"
            End If
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
        'Dim Sql As String = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & TxtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',
        'UserName='" & Session("UserName") & "' Where ReqID=" & lblID.Text & ";" & _
        Dim sql As String = ""
        If Session("CompId") = 1078 Or Session("CompId") = 1093 Then
            sql = "Update Monthwithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & TxtRemark.Text & "',PaymentMode='" & RbtPayment.SelectedItem.Text & "', " & _
        " BankId='" & DDlBank.SelectedValue & "',AcNo='" & txtAccount.Text & "',BranchName='" & TxtBranchName.Text & "',IFSCode='" & TxtIFSCode.Text & "',ChequeDate='" & txtChequeDate.Text & "'," & _
        " ChequeNo='" & TxtCheque.Text & "' ,UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & lblID.Text & "  ;" & _
         "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & LblForm1.Text & "' And vtype='W' And VoucherDate='" & LabelDate1.Text & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & LabelDate1.Text & " with req. no." & lblID.Text & "';" & _
        " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & LblForm1.Text & "')"
        Else
            sql = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & TxtRemark.Text & "',PaymentMode='" & RbtPayment.SelectedItem.Text & "', " & _
        " BankId='" & DDlBank.SelectedValue & "',AcNo='" & txtAccount.Text & "',BranchName='" & TxtBranchName.Text & "',IFSCode='" & TxtIFSCode.Text & "',ChequeDate='" & txtChequeDate.Text & "'," & _
        " ChequeNo='" & TxtCheque.Text & "' ,UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & lblID.Text & "  ;" & _
         "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & LblForm1.Text & "' And vtype='W' And VoucherDate='" & LabelDate1.Text & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & LabelDate1.Text & " with req. no." & lblID.Text & "';" & _
        " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & LblForm1.Text & "')"
        End If
        
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
                Dim Sql As String = ""
                If Session("CompID") = 1078 Or Session("CompId") = 1093 Then
                    Sql = "Update Monthwithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
               "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
               " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

                Else
                    Sql = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
               "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
               " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

                End If
                
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
        
        Dim Chk As CheckBox
        Dim cnt As Integer
        Dim updateeffect As Integer
        Dim StrSql As String = "Insert into Trnrejectbyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
        updateeffect = objDAL.SaveData(StrSql)
        If updateeffect > 0 Then
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
                    Dim Sql As String = ""
                    If Session("CompID") = 1078 Or Session("CompId") = 1093 Then
                        Sql = "Update Monthwithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
                                       "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
                                       " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

                    Else
                        Sql = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" & txtRemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblId.Text & ";" & _
                   "Update TrnVoucher Set Userid='" & Session("UserId") & "' Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';" & _
                   " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
             "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Approve','" & Remark & "',Getdate(),'" & FormNo & "')"

                    End If

                    updateeffect = objDAL.SaveData(Sql)
                    'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                    cnt = cnt + 1
                End If
            Next

            If updateeffect <> 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('" & cnt & "Withdrawal Approved Successfully.');location.replace('FundWithdraw.aspx');", True)
            Else
                scrname = "<SCRIPT language='javascript'>alert('Try After Some Time.');" & "</SCRIPT>"
            End If
            Me.RegisterStartupScript("MyAlert", scrname)

            ClearALL()
            FillDetail()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Try After Some Time.');location.replace('FundWithdraw.aspx');", True)
        End If
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

        'sql = "Select Row_Number() Over(Order by A.ReqID Desc) As SNo,A.WSessID as WeekNo,C.IdNo As BBAID,C.Mobl,ReqAmount As WithDrawAmount,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,A.PayeeName,B.BankName,A.AcNo As AccountNo,A.IFSCode,A.PanNo,A.BranchName,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status,A.Remark+' On '+dbo.formatDate(A.IssueDate,'dd-MMM-yyyy') As Remark From Fundwithdrawls As A,M_BankMaster As B,M_MemberMaster As C Where A.BankID=B.BankCode And B.RowStatus='Y' And B.ActiveStatus='Y'  And A.FormNo=C.FormNo " & Condition & " Order by A.WSessID"


        If CheckBox1.Checked = True Then
            Condition = Condition & " And b.IDNo='" & txtMemId.Text & "'"
        End If
        If CheckBox2.Checked = True Then
            If ddlSession.SelectedValue <> 100000 Then
                Condition = Condition & " And A.WSessID=" & ddlSession.SelectedValue
            End If
        End If
        If RbtStatus.SelectedValue = "N" Then
            Condition = Condition
        Else
            Condition = Condition & " And a.Status='" & RbtStatus.SelectedValue & "'"
        End If



        'sql = "Select Row_Number() Over(Order by A.ReqID Desc) As SNo,A.WSessID as WeekNo,C.IdNo As BBAID,C.Mobl,ReqAmount As WithDrawAmount,dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,A.PayeeName,B.BankName,''''+A.AcNo As AccountNo,A.IFSCode,A.PanNo,A.BranchName,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status,A.Remark+' On '+dbo.formatDate(A.IssueDate,'dd-MMM-yyyy') As Remark From Fundwithdrawls As A,M_BankMaster As B,M_MemberMaster As C Where A.BankID=B.BankCode And B.RowStatus='Y' And B.ActiveStatus='Y'  And A.FormNo=C.FormNo " & Condition & " Order by A.WSessID"
        'sql = "Select dbo.FormatDate(ReqDate,'dd-MMM-yyyy') As WithDrawDate,C.IdNo As MemberID,A.PayeeName,ReqAmount As Amount" & _
        '     " ,Case When A.Status='P' Then 'PENDING' Else Case When A.Status='A' Then 'APPROVED' else 'REJECTED' end end As Status " & _
        '     " ,Case when a.Status='P' Then '' else A.Remark end As Remark,UserName as ProcessedBy,Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as ProcessedDate,A.WSessID as WeekNo" & _
        '    " ,Replace(Convert(Varchar,d.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,d.ToDate,106),' ','-') as ToDate, " & _
        '    " ''''+A.AcNo as AcNo, A.IFSCode, B.BankName, A.BranchName, A.PanNo From Fundwithdrawls As A Left Join M_BankMaster As B On  A.BankID=B.BankCode And " & _
        '   " B.RowStatus='Y' And B.ActiveStatus='Y' ," & _
        '    " M_MemberMaster As C,m_sESSNmASTER AS D Where A.wsESSId=D.sESSID And A.FormNo=C.FormNo " & Condition & " Order by a.UserId, A.Formno"


        If (Session("CompID") = 1007) Then

            sql = "select cast(''''+' 023305004548' as Nvarchar)  as [Debit Ac No],cast(''' '+ a.AcNo as Nvarchar) as [Beneficiary Ac No],"
            sql &= " a.PayeeName as [Beneficiary Name],a.NetAmount as Amt,'N' as [Pay Mod],"
            sql &= " ' '+ Replace(Convert(Varchar,Getdate(),106),' ','-') as [Date],a.IFSCode as [IFSC],"
            sql &= " '' as [Payable Location],'' as [Print Location],' '+ Cast(b.mobl as Nvarchar) as [Bene Mobile No.] ,b.Email as [Bene Email ID],"
            sql &= " b.Address1 as [Bene add1], '' as [Bene add2],'' as [Bene add3],'' as [Bene add4],"
            sql &= " '' as [Add Details 1],'' as [Add Details 2],'' as [Add Details 3],'' as [Add Details 4],'' as [Add Details 5],'' as Remarks"
            sql &= " from Fundwithdrawls as a,M_memberMaster as b   where a.Formno = b.Formno " & Condition & " Order by ReqId"
            'ElseIf (Session("CompID") = 1038) Then
            '    sql = " Select ''''+Convert(Varchar,GetDate(),112) +'09790001' As Transaction_Ref_No,NetAmount As Amount,"
            '    sql &= " Convert(Varchar, Getdate(),103) As Value_Date,	''''+ '0350' As Branch_Code,'11' As Sender_Account_Type,"
            '    sql &= " ''''+'50200049505144' As Remitter_Account_No,'UCM Ventures Private Limited' As Remitter_Name,a.IFSCode As IFSC_Code,"
            '    sql &= " ''''+ '50200049505144' As Debit_Account, '10' As Beneficiary_Account_type, ''''+ Convert(Varchar,a.AcNo) As Bank_Account_Number,"
            '    sql &= " PayeeName  As Beneficiary_Name,'SALARY' As Remittance_Details, '11' As Debit_Account_System,"
            '    sql &= " 'UCM Ventures Private Limited' As Originator_Of_Remmittance"
            '    sql &= " from Fundwithdrawls As a, M_MemberMaster As b   Where a.Formno = b.Formno " & Condition & " Order by ReqId "

        ElseIf (Session("CompID") = 1038) Then
            sql = " Select ''''+Convert(Varchar,GetDate(),112) +'09790001' As Transaction_Ref_No,NetAmount As Amount,"
            sql &= " Convert(Varchar, Getdate(),103) As Value_Date,	''''+ '0350' As Branch_Code,'11' As Sender_Account_Type,"
            sql &= " ''''+'0005020100004757' As Remitter_Account_No,'UCM Ventures Private Limited' As Remitter_Name,a.IFSCode As IFSC_Code,"
            sql &= " ''''+ '0005020100004757' As Debit_Account, '10' As Beneficiary_Account_type, ''''+ Convert(Varchar,a.AcNo) As Bank_Account_Number,"
            sql &= " PayeeName  As Beneficiary_Name,'SALARY' As Remittance_Details, '11' As Debit_Account_System,"
            sql &= " 'UCM Ventures Private Limited' As Originator_Of_Remmittance"
            sql &= " from Fundwithdrawls As a, M_MemberMaster As b   Where a.Formno = b.Formno " & Condition & " Order by ReqId "

        ElseIf (Session("CompID") = 1067) Then
            sql = "select a.PayeeName as [Beneficiary Name],cast(''' '+ a.AcNo as Nvarchar) as [Beneficiary Ac No],a.IFSCode as [IFSC],'NEFT' as [Transaction Type],''''+'10094200289' as [Debit Ac No], "
            sql &= " Replace(Convert(varchar,IssueDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssueDate AS TIME),100) as [Transaction Date],"
            sql &= " a.NetAmount as Amount,'INR' as [Currency],'' as [Beneficiary Email ID],"
            sql &= " b.Idno as [Remarks], '' as [Custom Header 1],'' as [Custom Header 2],'' as [Custom Header 3],"
            sql &= " '' as [Custom Header 4],'' as [Custom Header 5]"
            sql &= " from Fundwithdrawls as a,M_memberMaster as b   where a.Formno = b.Formno " & Condition & " Order by ReqId"
        ElseIf (Session("CompID") = 1078) Then
            sql = "select b.Idno,WSessid as [Payout Week],''as [Bank A/C No.],'' as [Company Name],'' as [Column1],'' as [Column2],'' as [Column3]," & _
      " a.IFSCode as [IFSC Code],'#'+cast(a.AcNo as Varchar) as [Distributor A/C No],PayeeName as Name,' ' as [Column4],'' as [Column5],'' as [Column6]," & _
      " '' as [Column7],ROW_NUMBER() OVER (ORDER BY ReqId) AS SequenceNo,Replace(Convert(Varchar,Reqdate,106),' ','-')As date,NetAmount" & _
      " from Monthwithdrawls as a,M_memberMaster as b   where a.Formno = b.Formno " & Condition & " Order by ReqId "
        ElseIf (Session("CompID") = 1093) Then
            sql = "select b.Idno,WSessid as [Payout Week],''as [Bank A/C No.],'' as [Company Name],'' as [Column1],'' as [Column2],'' as [Column3]," & _
      " a.IFSCode as [IFSC Code],'#'+cast(a.AcNo as Varchar) as [Distributor A/C No],PayeeName as Name,' ' as [Column4],'' as [Column5],'' as [Column6]," & _
      " '' as [Column7],ROW_NUMBER() OVER (ORDER BY ReqId) AS SequenceNo,Replace(Convert(Varchar,Reqdate,106),' ','-')As date,NetAmount" & _
      " from Monthwithdrawls as a,M_memberMaster as b   where a.Formno = b.Formno " & Condition & " Order by ReqId "

        Else

            sql = "select b.Idno,WSessid as [Payout Week],''as [Bank A/C No.],'' as [Company Name],'' as [Column1],'' as [Column2],'' as [Column3]," & _
       " a.IFSCode as [IFSC Code],'#'+cast(a.AcNo as Varchar) as [Distributor A/C No],PayeeName as Name,' ' as [Column4],'' as [Column5],'' as [Column6]," & _
       " '' as [Column7],ROW_NUMBER() OVER (ORDER BY ReqId) AS SequenceNo,Replace(Convert(Varchar,Reqdate,106),' ','-')As date,NetAmount" & _
       " from Fundwithdrawls as a,M_memberMaster as b   where a.Formno = b.Formno " & Condition & " Order by ReqId "

        End If



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

        Dim Chk As CheckBox
        Dim cnt As Integer
        Dim updateeffect As Integer
        Dim StrSql As String = "Insert into Trnrejectbyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
        updateeffect = objDAL.SaveData(StrSql)

        If updateeffect > 0 Then


            Dim LblId As New Label
            Dim sql As String = ""
            Dim LblMobl As New Label
            Dim LblFromDate As New Label
            Dim LblTodate As New Label
            Dim Remark As String = ""

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

                    '' Comment By Rakesh Soni 
                    'sql = sql & "Update FundWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtRemark & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & Id & ";" & _
                    '" Delete From TrnVoucher Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';"
                    'sql = sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                    '"('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Reject','" & Remark & "',Getdate(),'" & FormNo & "')"
                    '' Add New Code New  Credit Entry Insert in TrnVoucher Table
                    If Session("CompID") = 1078 Or Session("CompID") = 1093 Then
                        sql = sql & "Update Monthwithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtRemark & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & Id & ";"
                    Else
                        sql = sql & "Update FundWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtRemark & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & Id & ";"
                    End If
                    sql = sql & " INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) "
                    sql = sql & "  SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "',0, '" & FormNo & "'," & Val(LblAmount) & ","
                    sql = sql & "  'Withdrawal Rejected  Req. No. " & Id & "','Req/" & Id & "','M','C',convert(varchar,getdate(),112),"
                    sql = sql & "  (Select Max(SessID) from M_SessnMaster) FROM TrnVoucher;"

                    sql = sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                           "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Reject','" & Remark & "',Getdate(),'" & FormNo & "')"

                    updateeffect = objDAL.SaveData(sql)
                    'SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                    cnt = cnt + 1
                End If
            Next
            If updateeffect <> 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('" & cnt & " Withdrawal Rejected Successfully.');location.replace('FundWithdraw.aspx');", True)
            Else
                scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
            End If
            Me.RegisterStartupScript("MyAlert", scrname)
            ClearALL()
            FillDetail()
            DivRemark.Visible = False
        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Try After Some Time.');location.replace('FundWithdraw.aspx');", True)
        End If

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
                Dim LblAmount As String = DirectCast(Gvr.FindControl("LblAmount"), Label).Text
                Remark = "Withdrawal Rejected Of Idno " & IdNo & " for WeekNo:" & WeekNo & " By " & Session("UserName") & ""

                '' Comment By Rakesh Soni
                'sql = sql & "Update FundWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & Id & ";" & _
                '" Delete From TrnVoucher Where DrTo='" & FormNo & "' And vtype='W' And VoucherDate='" & DateOn & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & DateOn & " with req. no." & Id & "';"
                'sql = sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                '"('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Fund WithDrawals','Fund Withdrawls Reject','" & Remark & "',Getdate(),'" & FormNo & "')"

                '' Add New Code New  Credit Entry Insert in TrnVoucher Table
                If Session("CompID") = 1078 Or Session("CompID") = 1093 Then
                    sql = sql & "Update Monthwithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & Id & ";"
                Else
                    sql = sql & "Update FundWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & Id & ";"

                End If

                sql = sql & " INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) "
                sql = sql & "  SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "',0, '" & FormNo & "'," & Val(LblAmount) & ","
                sql = sql & "  'Withdrawal Rejected  Req. No. " & Id & "','Req/" & Id & "','M','C',convert(varchar,getdate(),112),"
                sql = sql & "  (Select Max(SessID) from M_SessnMaster) FROM TrnVoucher;"

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
        Dim Sql As String = ""
        If Session("CompID") = 1078 Or Session("CompID") = 1093 Then
            Sql = "Update Monthwithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblRejReqNo.Text & ";Delete From TrnVoucher Where DrTo='" & lblRejFormno.Text & "' And vtype='W' And VoucherDate='" & LblRejDateOn.Text & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & LblRejDateOn.Text & " with req. no." & LblRejReqNo.Text & "';"

        Else
            Sql = "Update FundWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" & TxtARemark.Text & "',UserId='" & Val(Session("UserID")) & "',UserName='" & Session("UserName") & "' Where ReqID=" & LblRejReqNo.Text & ";Delete From TrnVoucher Where DrTo='" & lblRejFormno.Text & "' And vtype='W' And VoucherDate='" & LblRejDateOn.Text & "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " & LblRejDateOn.Text & " with req. no." & LblRejReqNo.Text & "';"

        End If
        
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
