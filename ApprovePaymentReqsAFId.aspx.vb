Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net.Mail

Partial Class ApprovePaymentReqsAFId
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
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
        'If txtStartDate.Text <> "" Then
        '    If CmbType.SelectedValue = "R" Then
        '        Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
        '    Else
        '        Condition = Condition & " and Isapprove='" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
        '    End If

        'End If
        'If txtEndDate.Text <> "" Then
        '    If CmbType.SelectedValue = "R" Then
        '        Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
        '    Else
        '        Condition = Condition & " and Isapprove='" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
        '    End If

        'End If
        Dim url As String = String.Empty

        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri

        If (Session("CompID") = "1017") Then
            url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "")
        Else
            'url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.").Replace("ADMIN.", "LOGIN.")
            url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
        End If





        url = url.ToLower
        Dim sql As String
        If Session("CompId") = 1066 Then
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype," & _
        " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                       " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
            " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
          " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,Case when ScannedFile='' then '' else 'https://login.alkamediindia.in/images/UploadImage/'+ ScannedFile end as ScannedFile " & _
          " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
          " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo "
        ElseIf Session("CompId") = 1074 Then
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype," & _
                    " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                                   " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                        " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                      " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else 'https://login.cashlessbazar.in/images/UploadImage/'+ScannedFile END AS ScannedFile " & _
                      " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
                      " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo desc "

        ElseIf Session("CompId") = 1075 Then
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype," & _
                    " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                                   " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                        " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                      " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else 'https://login.cashlessbazar.in/images/UploadImage/'+ScannedFile END AS ScannedFile " & _
                      " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
                      " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo desc "

        ElseIf Session("CompId") = 1078 Then
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype," & _
                    " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                                   " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                        " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                      " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else 'https://consultant.manjar.in/images/UploadImage/'+ScannedFile END AS ScannedFile " & _
                      " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
                      " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo desc "

        ElseIf Session("CompId") = 1093 Then
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype," & _
                    " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                                   " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                        " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                      " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else 'https://network.swastikemall.com/images/UploadImage/'+ScannedFile END AS ScannedFile " & _
                      " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
                      " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo desc "
        
        ElseIf Session("CompId") = 1057 Then
            '    sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo," & _
            '" Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
            '               " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
            '    " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
            '  " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN replace(ScannedFile, 'https://cpanel.zaradobit.com/images/UploadImage/', '/images/UploadImage/') ELSE '/images/UploadImage/' + ScannedFile END AS ScannedFile " & _
            '  " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
            '  " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo "
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,A.actype," & _
        " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
        " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
        " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
        " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks, CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else 'https://cpanel.zaradobit.com/images/UploadImage/'+ScannedFile END AS ScannedFile " & _
        " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
        " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo "

        ElseIf Session("CompId") = 1072 Then
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype," & _
                               " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                                              " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                                   " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                                 " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile " & _
                                 " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
                                 " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo desc"

        ElseIf Session("CompId") = 1079 Then
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype," & _
                    " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                                   " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                        " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                      " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else 'https://cpanel.afuniverse.in/images/UploadImage/'+ScannedFile END AS ScannedFile " & _
                      " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReqId " & _
                      " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo desc "

        Else
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Email as Email ,Mobl as Mobileno,ReqNo,'' as Actype," & _
                    " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                                   " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                        " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                      " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile " & _
                      " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
                      " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo "

        End If
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then
            'BtnReject.Visible = True
            'If Session("IsView") = "Y" Then
            btnApproove.Visible = True
            BtnRejects.Visible = True
            btnExport.Visible = True
            'Else
            '    btnApproove.Visible = False
            '    BtnRejects.Visible = False
            'End If

            lblReqs.Text = dtData.Rows.Count
            lblTotalAmont.Text = dtData.Compute("Sum(Amount)", "")
            LblEmailno.Text = dtData.Rows(0)("Email")

        Else
            'BtnReject.Visible = False
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

        'If ChkMem.Checked = True Then
        '    Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
        'End If
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

        Dim Dt As New DataTable

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



        Dim sms As String = ""
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim sql As String = ""
        Dim Chk As CheckBox
        Dim lbl As Label
        Dim lblReqno As Label
        Dim lblIdno As Label
        Dim LblAmount As Label
        Dim lblPaymode As Label
        Dim LblMobileno As Label
        Dim LblActype As Label
        Dim Cnt As Integer = 0
        Dim Remark As String = ""
        Dim voucherno As String = ""
        Dim dt As New DataTable
        Dim Dt1 As New DataTable
        If Session("CompId") = "1026" Then
            CreateBlankTable()

            dt = DirectCast(Session("SmsList"), DataTable)
        End If

        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
            lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
            lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
            LblAmount = DirectCast(Gvr.FindControl("lblAmount"), Label)
            lblPaymode = DirectCast(Gvr.FindControl("LblPaymode"), Label)
            LblMobileno = DirectCast(Gvr.FindControl("LblMobileno"), Label)

            If Chk.Checked = True And Chk.Enabled = True Then

                Dim Dr_ As DataRow
                Dim strSql As String = ""
                If Session("CompID") = 1079 Then
                    strSql = "Select Count(*) As Cnt  from WalletReqId Where ReqNo = '" & lblReqno.Text & "' and IsApprove <> 'N'"
                Else
                    strSql = "Select Count(*) As Cnt  from WalletReq Where ReqNo = '" & lblReqno.Text & "' and IsApprove <> 'N'"
                End If
                'Dim strSql As String = "Select Count(*) As Cnt  from WalletReq Where ReqNo = '" & lblReqno.Text & "' and IsApprove <> 'N'"
                Dt1 = New DataTable
                Dt1 = objDAL.GetData(strSql)
                If (Val(Dt1.Rows(0)("Cnt")) = 0) Then
                    If AprvType = "Y" Then
                        If Session("CompId") = 1066 Or Session("CompId") = 1068 Or Session("CompId") = 1078 Or Session("CompId") = 1093 Then
                            Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                            sql = sql & "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "'," & _
                        "'0','" & lbl.Text & "'," & Val(LblAmount.Text) & ",'Amount Added by Payment  Req.No.:" & lblReqno.Text & ".','Req/" & lblReqno.Text & "','S','C',Convert(Varchar,Getdate(),112),'" & Session("CurrentSessn") & "' FROM TrnVoucher Where Narration Not in ('Amount Added by Payment  Req.No.:" & lblReqno.Text & ".');"
                        ElseIf Session("CompId") = 1057 Then
                            LblActype = DirectCast(Gvr.FindControl("lblactype"), Label)
                            If LblActype.Text = "R" Then
                                Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                                sql = sql & "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "'," & _
                            "'0','" & lbl.Text & "'," & Val(LblAmount.Text) & ",'Amount Added by Payment  Req.No.:" & lblReqno.Text & ".','Req/" & lblReqno.Text & "','R','C',Convert(Varchar,Getdate(),112),'" & Session("CurrentSessn") & "' FROM TrnVoucher Where Narration Not in ('Amount Added by Payment  Req.No.:" & lblReqno.Text & ".');"
                            Else
                                Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                                sql = sql & "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "'," & _
                            "'0','" & lbl.Text & "'," & Val(LblAmount.Text) & ",'Amount Added by Payment  Req.No.:" & lblReqno.Text & ".','Req/" & lblReqno.Text & "','X','C',Convert(Varchar,Getdate(),112),'" & Session("CurrentSessn") & "' FROM TrnVoucher Where Narration Not in ('Amount Added by Payment  Req.No.:" & lblReqno.Text & ".');"
                            End If
                        Else
                            If Session("CompId") = 1072 Or Session("CompId") = 1073 Or Session("CompId") = 1077 Or Session("CompId") = 1078 Or Session("CompId") = 1079 Or Session("CompId") = 1093 Then
                                Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                                sql = sql & "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "'," & _
                            "'0','" & lbl.Text & "'," & Val(LblAmount.Text) & ",'Amount Added by Payment  Req.No.:" & lblReqno.Text & ".','Req/" & lblReqno.Text & "','S','C',Convert(Varchar,Getdate(),112),'" & Session("CurrentSessn") & "' FROM TrnVoucher Where Narration Not in ('Amount Added by Payment  Req.No.:" & lblReqno.Text & ".');"
                            Else
                                Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                                sql = sql & "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "'," & _
                            "'0','" & lbl.Text & "'," & Val(LblAmount.Text) & ",'Amount Added by Payment  Req.No.:" & lblReqno.Text & ".','Req/" & lblReqno.Text & "','R','C',Convert(Varchar,Getdate(),112),'" & Session("CurrentSessn") & "' FROM TrnVoucher Where Narration Not in ('Amount Added by Payment  Req.No.:" & lblReqno.Text & ".');"

                            End If
                        End If

                        If Session("CompId") = "1026" Then
                            sms = "  Dear " & lblIdno.Text & " your payment Request No." & lblReqno.Text & " is Approved for Rs. " & LblAmount.Text & ". Pls visit " & Session("CompWeb") & " for more details. "
                        End If
                    Else
                        If Session("CompId") = "1026" Then
                            sms = " Dear " & lblIdno.Text & " your payment Request No." & lblReqno.Text & " is Rejected for Rs. " & LblAmount.Text & " due to " & TxtARemark.Text & ". Pls visit " & Session("CompWeb") & " for more details.  "
                        End If
                        Remark = " Reject Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                    End If
                    If Session("CompId") = 1079 Then
                        sql = sql & ";exec Sp_ActivateMember '" & lblIdno.Text & "','2','" & lbl.Text & "'"
                        sql = sql & ";Update WalletReqId SET  IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApproveBy='" & Val(Session("UserID")) & "',ApproveRemark='" & TxtARemark.Text & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
                    Else
                        sql = sql & ";Update WalletReq SET  IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApproveBy='" & Val(Session("UserID")) & "',ApproveRemark='" & TxtARemark.Text & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
                    End If

                    sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,Memberid)Values" & _
             "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Approve Payment Request ','Approve Payment Request','" & Remark & "',Getdate(),'" & lbl.Text & "')"

                    '       sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp)Values" & _
                    '"('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Approve Payment Request ','Approve Payment Request','" & Remark & "',Getdate())"

                    Cnt = Cnt + 1
                    If Session("CompId") = "1026" Then


                        Dr_ = dt.NewRow
                        Dr_("SMS") = sms
                        Dr_("Mobileno") = LblMobileno.Text
                        Dr_("Idno") = lblIdno.Text
                        Dr_("ReqNo") = lblReqno.Text
                        Dr_("Amount") = LblAmount.Text
                        dt.Rows.Add(Dr_)
                    End If
                End If
            End If
        Next
        Dim a As Integer
        If sql <> "" Then
            a = objDAL.UpdateData(sql)
        End If

        Dim MsgTxt As String = ""
        If AprvType = "Y" Then
            MsgTxt = "Approved"
        Else : MsgTxt = "Rejected"
        End If
        If a <> 0 And Cnt > 0 Then
            If Session("CompId") = "1026" Then


                For i = 0 To dt.Rows.Count - 1
                    Sendsms(dt.Rows(i)("SMS"), dt.Rows(i)("MobileNo"))
                Next
            End If
            Session("remark") = TxtARemark.Text
            'Session("IssueDate") = IssueDate
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
"Your Wallet Request has been approved on  " & Session("GETDATE") & ". Please check your wallet." & Session("remark") & " <br />" & _
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
            MyMessage.Subject = "Wallet approved!!"
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
        If Session("compid") = 1066 Or Session("compid") = 1072 Or Session("compid") = 1073 Or Session("compid") = 1074 Or Session("compid") = 1075 Or Session("compid") = 1077 Or Session("compid") = 1078 Or Session("compid") = 1079 Or Session("CompId") = 1093 Then

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
        If Session("compid") = 1066 Or Session("compid") = 1072 Or Session("compid") = 1073 Or Session("compid") = 1074 Or Session("compid") = 1075 Then
        ElseIf Session("compid") <> 1001 Then
        Else
            SendToMemberMail()
        End If

    End Sub
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click
        If Session("compid") = 1066 Or Session("compid") = 1072 Or Session("compid") = 1073 Or Session("compid") = 1074 Or Session("compid") = 1075 Or Session("compid") = 1078 Or Session("CompId") = 1093 Then

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
        If Session("compid") = 1066 Or Session("compid") = 1072 Or Session("compid") = 1073 Or Session("compid") = 1074 Or Session("compid") = 1075 Then

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

            If (Session("CompID") = "1017") Then
                url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "")
            Else
                'url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.").Replace("ADMIN.", "LOGIN.")
                url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
            End If

            url = url.ToLower
            Dim sql As String = ""
            If Session("CompId") = 1079 Then
                sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Mobl as Mobileno,Email,ReqNo,'' as Actype," & _
            " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                           " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
              " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile " & _
              " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReqId " & _
              " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & " order by ReqNo "

            Else
                sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Mobl as Mobileno,Email,ReqNo,'' as Actype," & _
                            " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                                           " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                                " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                              " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile " & _
                              " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
                              " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & " order by ReqNo "

            End If
            

            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()

            dg.DataSource = dtData
            dg.DataBind()
            Session("Email") = dtData.Rows(0)("Email")
            ExportToExcel("Approvepayment.xls", dg)


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
    'Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
    '    Try
    '        ' If e.SortExpression = ViewState("PayoutDate").ToString() Then
    '        If ViewState("Sort_Order").ToString() = "ASC" Then
    '            RebindData(e.SortExpression, "DESC")
    '            For i As Integer = 0 To GvData.Columns.Count - 1
    '                Dim lbText As String = "DESC"
    '                ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
    '                If lbText = ViewState("Sort_Order").ToString() Then
    '                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
    '                    Dim img As New Image()
    '                    img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
    '                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
    '                    tableCell.Controls.Add(img)
    '                End If
    '            Next

    '        Else
    '            RebindData(e.SortExpression, "ASC")
    '            For i As Integer = 0 To GvData.Columns.Count - 1
    '                Dim lbText As String = "ASC"
    '                If lbText = ViewState("Sort_Order").ToString() Then
    '                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
    '                    Dim img As New Image()
    '                    img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
    '                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
    '                    tableCell.Controls.Add(img)

    '                End If
    '            Next
    '        End If
    '        'Else
    '        'RebindData(e.SortExpression, "ASC")
    '        'End If


    '    Catch ex As Exception

    '    End Try
    'End Sub
    'Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
    '    Try

    '        Dim dt As DataTable = CType(Session("GvData1"), DataTable)
    '        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
    '        GvData.DataSource = dt
    '        GvData.DataBind()
    '        ViewState("IdNo") = sColimnName
    '        ViewState("Sort_Order") = sSortOrder
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class
