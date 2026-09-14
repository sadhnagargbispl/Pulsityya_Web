Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class ApprovePaymentReqsEP
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

            Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"

        End If
        If txtEndDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"

        End If

        Dim url As String = String.Empty

        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri

        url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")


        url = url.ToLower

        Dim sql As String = " select a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername,Mobl as Mobileno,ReqNo," & _
        " Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+  " & _
                       " CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate,a.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate," & _
            " b.BankName,a.Branchname,Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected'" & _
          " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,Amount,(Amount*85) As EPValue,a.Remarks,Case when ScannedFile='' then '' else '" & url & "/images/UploadImage/'+ ScannedFile end as ScannedFile " & _
          " ,Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark from WalletReq " & _
          " as a  with(nolock),M_BankMaster as b  with(nolock), M_MemberMaster as c  with(nolock) where a.Formno=c.Formno and  a.BankId=b.BankCode and b.RowStatus='Y' " & Condition & "order by ReqNo "
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
            'Else
            '    btnApproove.Visible = False
            '    BtnRejects.Visible = False
            'End If
        Else
            'BtnReject.Visible = False
            btnApproove.Visible = False
            BtnRejects.Visible = False
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
    Private Sub AprvAction(ByVal AprvType As String, ByVal ApprvStatus As String)
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        Dim sql1 As String = "Select Balance from M_GenerateEP"
        Dim dt1 As New DataTable
        dt1 = objDAL.GetData(sql1)
        If (dt1.Rows.Count > 0) Then
            Session("EPBalance") = dt1.Rows(0)("Balance")
        End If




        Dim sql As String = ""
        Dim Chk As CheckBox
        Dim lbl As Label
        Dim lblReqno As Label
        Dim lblIdno As Label
        Dim LblAmount As Label
        Dim lblPaymode As Label
        Dim Cnt As Integer = 0
        Dim Remark As String = ""
        Dim voucherno As String = ""
        Dim dt As DataTable
        Dim Amount As Decimal = 0
        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
            lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
            lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
            LblAmount = DirectCast(Gvr.FindControl("lblAmount"), Label)
            lblPaymode = DirectCast(Gvr.FindControl("LblPaymode"), Label)
            If Chk.Checked = True And Chk.Enabled = True Then


                Dim strSql As String = "Select Count(*) As Cnt  from WalletReq Where ReqNo = '" & lblReqno.Text & "' and IsApprove <> 'N'"
                dt = New DataTable
                dt = objDAL.GetData(strSql)
                If (Val(dt.Rows(0)("Cnt")) = 0) Then
                    If AprvType = "Y" Then
                        Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                        sql = sql & "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) SELECT ISNULL(Max(VoucherNo)+1,1001),'" & Format(Now, "dd-MMM-yyyy") & "'," & _
                    "'0','" & lbl.Text & "'," & Val(LblAmount.Text) & ",'Amount Added by Payment  Req.No.:" & lblReqno.Text & ".','Req/" & lblReqno.Text & "','R','C',Convert(Varchar,Getdate(),112),'" & Session("CurrentSessn") & "' FROM TrnVoucher Where Narration Not in ('Amount Added by Payment  Req.No.:" & lblReqno.Text & ".');"


                        sql &= "  Exec Sp_GenerateEP 'Debit', '" & Val(LblAmount.Text) & "'"
                        sql &= "Insert into M_GenerateEPHistory (Formno,EP,VcType,Remark,Balance) Values"
                        sql &= "('" & lbl.Text & "','" & Val(LblAmount.Text) & "','D','" & Remark & "', '" & (Val(Session("EPBalance")) - Val(LblAmount.Text)) & "' )"
                    Else

                        Remark = " Reject Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                    End If
                    sql = sql & ";Update WalletReq SET  IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApproveBy='" & Val(Session("UserID")) & "',ApproveRemark='" & TxtARemark.Text & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
                    sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp)Values" & _
             "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Approve Payment Request ','Approve Payment Request','" & Remark & "',Getdate())"


                

                    Amount += Val(LblAmount.Text)

                    Cnt = Cnt + 1
                End If

            End If
        Next

        If (Amount > Val(Session("EPBalance"))) Then
            Dim scrName As String
            scrName = "<SCRIPT language='javascript'>alert('Insufficient balance Please Try Again.!! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
            Exit Sub
        End If
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
End Class
