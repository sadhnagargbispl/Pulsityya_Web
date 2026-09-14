Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports System.Net
Imports System.Globalization
Partial Class ApproveSeminarReqs
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
            If Session("AStatus") = "OK" Then
                FillSeminar()
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

    Private Sub FillSeminar()
        Try
            Dim strQuery As String = ""
            strQuery = " Select 0 As MeetingID,'---Select Seminar---' As Program Union All "
            strQuery &= "Select MeetingID,Program from  M_MeetingMaster "
            strQuery &= " Where   ActiveStatus = 'Y'"

            Dim tmpTable As DataTable = New DataTable
            tmpTable = objDAL.GetData(strQuery)
            With ddlSeminar
                .DataSource = tmpTable
                .DataValueField = "MeetingID"
                .DataTextField = "Program"
                .DataBind()
            End With
            Session("ProgramDetail") = tmpTable

        Catch ex As Exception
         
        End Try
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")

        Dim Condition1 As String = ""
        Dim Condition2 As String = ""

        If ChkMem.Checked = True And Trim(TxtMemID.Text) <> "" Then
            Condition = Condition & " AND c.IdNo='" & Trim(TxtMemID.Text) & "'"
            Condition1 = Condition1 & " AND a.Formno='" & Trim(TxtMemID.Text).ToString().ToUpper().Replace("VI", "") & "'"
        End If
        If RbReqStatus.SelectedValue <> "A" Then
            Condition = Condition & " And a.IsApprove='" & RbReqStatus.SelectedValue & "'"
            Condition1 = Condition1 & " And a.IsApprove='" & RbReqStatus.SelectedValue & "'"

        End If
        DivRemark.Visible = False

        If txtStartDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
            Condition1 = Condition1 & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"

        End If
        If txtEndDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
            Condition1 = Condition1 & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"

        End If

        If Val(ddlSeminar.SelectedValue) > 0 Then

            Condition = Condition & " And  M.MeetingID='" & Val(ddlSeminar.SelectedValue) & "'"
            Condition1 = Condition1 & " And  a.SeminarID='" & Val(ddlSeminar.SelectedValue) & "'"
            Condition2 = Condition2 & " And  MeetingID='" & Val(ddlSeminar.SelectedValue) & "'"
        End If

        Dim url As String = String.Empty

        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri

        If (Session("CompID") = "1017") Then
            url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "")
        Else
            url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
        End If
        url = url.ToLower
        ' Below commit 21 March 2022
        'Dim sql As String = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Mobl as Mobileno,ReqNo," & _
        '" Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
        '               " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
        '    " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
        '  " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile " & _
        '  " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark,a.Person,M.program from SeminarReq as a  with(nolock),M_MeetingMaster As M with(nolock)," & _
        '  " M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.SeminarID=M.MeetingID  And a.Formno=c.Formno and  a.BankId=b.BankCode and M.ActiveStatus='Y'  and b.RowStatus='Y' " & Condition & "order by ReqNo ;"

        'sql &= " Select Sum(Person) As TotalPerson  from M_MeetingMaster Where ActiveStatus = 'Y' " & Condition2 & ";"
        'sql &= " Select Isnull(Sum(Person),0) As ApprovePerson From SeminarReq As a Where IsApprove = 'Y' " & Condition1 & ";"
        'sql &= " Select Isnull(Sum(Person),0) As PendingPerson From SeminarReq As a Where IsApprove = 'N' " & Condition1 & ";"
        Dim sql As String
        If Session("CompID") = 1007 Then
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Mobl as Mobileno,a.ReqNo,"
            sql &= " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  "
            sql &= " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate,"
            sql &= " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'"
            sql &= " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,"
            sql &= "Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile "
            sql &= " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark,a.Person,M.program,d.ReqNo,"
            sql &= "isnull(e.groupname,'') as groupname "
            sql &= " from SeminarReq as a  with(nolock)"
            sql &= "left join M_SeminarGroupMaster as e on a.groupid=e.GroupId,M_MeetingMaster As M with(nolock),"
            sql &= " M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock),TrnSemimarMember as d "
            sql &= "where a.SeminarID=M.MeetingID  And a.Formno=c.Formno and  a.BankId=b.BankCode and M.ActiveStatus='Y' "
            sql &= " and b.RowStatus='Y' and a.Reqno=d.Reqno " & Condition & " "
            sql &= "group by  a.Formno,c.Idno ,a.ReqNo,c.MemFirstName,c.MemlastName,Mobl,a.RectimeStamp,a.PayMode,a.Chqno,a.ChqDate,b.BankName,a.Branchname,IsApprove"
            sql &= ",Amount,a.Remarks,ScannedFile,ApproveRemark,a.Person,M.program,d.ReqNo,e.groupname order by a.ReqNo ;"

            sql &= " Select Sum(Person) As TotalPerson  from M_MeetingMaster Where ActiveStatus = 'Y' " & Condition2 & ";"
            sql &= " Select Isnull(Sum(Person),0) As ApprovePerson From SeminarReq As a Where IsApprove = 'Y' " & Condition1 & ";"
            sql &= " Select Isnull(Sum(Person),0) As PendingPerson From SeminarReq As a Where IsApprove = 'N' " & Condition1 & ";"

            '24 apr 2024
            'sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Mobl as Mobileno,a.ReqNo,"
            'sql &= " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  "
            'sql &= " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate,"
            'sql &= " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'"
            'sql &= " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,"
            'sql &= "Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile "
            'sql &= " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark,a.Person,M.program,d.ReqNo,d.name,"
            'sql &= "d.mobile,d.email,d.city,isnull(e.groupname,'') as groupname "
            'sql &= " from SeminarReq as a  with(nolock)"
            'sql &= "left join M_SeminarGroupMaster as e on a.groupid=e.GroupId,M_MeetingMaster As M with(nolock),"
            'sql &= " M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock),TrnSemimarMember as d "
            'sql &= "where a.SeminarID=M.MeetingID  And a.Formno=c.Formno and  a.BankId=b.BankCode and M.ActiveStatus='Y' "
            'sql &= " and b.RowStatus='Y' and a.Reqno=d.Reqno " & Condition & " "
            'sql &= "group by  a.Formno,c.Idno ,a.ReqNo,c.MemFirstName,c.MemlastName,Mobl,a.RectimeStamp,a.PayMode,a.Chqno,a.ChqDate,b.BankName,a.Branchname,IsApprove"
            'sql &= ",Amount,a.Remarks,ScannedFile,ApproveRemark,a.Person,M.program,d.ReqNo,e.groupname order by a.ReqNo ;"

            'sql &= " Select Sum(Person) As TotalPerson  from M_MeetingMaster Where ActiveStatus = 'Y' " & Condition2 & ";"
            'sql &= " Select Isnull(Sum(Person),0) As ApprovePerson From SeminarReq As a Where IsApprove = 'Y' " & Condition1 & ";"
            'sql &= " Select Isnull(Sum(Person),0) As PendingPerson From SeminarReq As a Where IsApprove = 'N' " & Condition1 & ";"

        Else
            sql = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Mobl as Mobileno,a.ReqNo," & _
                " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                               " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
                    " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
                  " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,a.Remarks,Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile " & _
                  " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark,a.Person,M.program,d.ReqNo,d.name,d.mobile,d.email,d.city from SeminarReq as a  with(nolock),M_MeetingMaster As M with(nolock)," & _
                  " M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock),TrnSemimarMember as d with(nolock) where a.SeminarID=M.MeetingID  And a.Formno=c.Formno and  a.BankId=b.BankCode and M.ActiveStatus='Y' and b.RowStatus='Y' and a.Reqno=d.Reqno " & Condition & "order by a.ReqNo ;"

            sql &= " Select Sum(Person) As TotalPerson  from M_MeetingMaster Where ActiveStatus = 'Y' " & Condition2 & ";"
            sql &= " Select Isnull(Sum(Person),0) As ApprovePerson From SeminarReq As a Where IsApprove = 'Y' " & Condition1 & ";"
            sql &= " Select Isnull(Sum(Person),0) As PendingPerson From SeminarReq As a Where IsApprove = 'N' " & Condition1 & ";"

        End If
        
        Dim ds As DataSet = New DataSet()
        dtData = New DataTable
        ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql)
        dtData = ds.Tables(0)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then

            btnApproove.Visible = True
            BtnRejects.Visible = True


            lblReqs.Text = dtData.Rows.Count
            lblTotalAmont.Text = dtData.Compute("Sum(Amount)", "")

            lblTotalPerson.Text = ds.Tables(1).Rows(0)("TotalPerson")
            lblTotalApPerson.Text = ds.Tables(2).Rows(0)("ApprovePerson")
            lblTotalPePerson.Text = ds.Tables(3).Rows(0)("PendingPerson")


        Else

            btnApproove.Visible = False
            BtnRejects.Visible = False
            lblReqs.Text = "0"
            lblTotalAmont.Text = "0.00"
            lblTotalPerson.Text = "0"
            lblTotalApPerson.Text = "0"
            lblTotalPePerson.Text = "0"
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        'GvData.DataSource = Session("GData")
        'GvData.DataBind()
        BindData()
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
        Dim Cnt As Integer = 0
        Dim Remark As String = ""
        Dim voucherno As String = ""
        Dim dt As New DataTable
        Dim Dt1 As New DataTable


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
                Dim strSql As String = "Select Count(*) As Cnt  from SeminarReq Where ReqNo = '" & lblReqno.Text & "' and IsApprove <> 'N'"
                Dt1 = New DataTable
                Dt1 = objDAL.GetData(strSql)
                If (Val(Dt1.Rows(0)("Cnt")) = 0) Then
                    If AprvType = "Y" Then
                        Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                        sql = sql & " Update SeminarReq SET  IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApproveBy='" & Val(Session("UserID")) & "',"
                        sql = sql & " ApproveRemark='" & TxtARemark.Text & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
                        sql = sql & " Exec IssueCode '" & lblReqno.Text & "'"
                    Else
                        Remark = " Reject Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                        sql = sql & "Update SeminarReq SET  IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApproveBy='" & Val(Session("UserID")) & "',ApproveRemark='" & TxtARemark.Text & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
                    End If
                    sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp)Values" & _
                    "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Approve Payment Request ','Approve Payment Request','" & Remark & "',Getdate())"

                    Cnt = Cnt + 1

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
        AprvAction("Y", "A")
    End Sub
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click
        AprvAction("R", "R")
    End Sub
    Protected Sub Sendsms(ByVal sms As String, ByVal Mobl As String)
        If Mobl.Length > 9 Then


            Dim client As New WebClient
            Dim baseurl As String
            Dim data As Stream


            Try
                baseurl = "http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & Mobl & "&SenderId=" & Session("ClientId") & ""

                data = client.OpenRead(baseurl)
                Dim reader As New StreamReader(data)
                Dim s As String
                s = reader.ReadToEnd()
                data.Close()
                reader.Close()
            Catch ex As Exception

            End Try
        End If
    End Sub
End Class
