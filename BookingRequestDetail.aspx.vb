Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports System.Net
Imports System.Globalization
Partial Class BookingRequestDetail
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
        Try

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

            If (Session("CompID") = "1017") Then
                url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "")
            Else
                url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
            End If





            url = url.ToLower

            Dim sql As String = "select a.id,a.flatid,a.Formno,c.Idno,(c.MemFirstName+ ' '+c.MemlastName) as Membername," & _
            " Mobl as Mobileno,ReqNo,Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')+ ' '+CONVERT(varchar(15), CAST(a.RectimeStamp AS TIME),100) as ReqDate" & _
            ",d.PayMode,a.Chqno,Replace(Convert(Varchar,a.ChqDate,106),' ','-') as ChequeDate,b.BankName,a.Branchname," & _
            " Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approve' else 'Rejected' " & _
            " end as status,Case when IsApprove='N' then 'True' else 'False' end as EnableStatus,a.Amount,a.Remark," & _
            " Case when ScanneFile='' then '' else 'https://cpanel.dlb.estate/images/UploadImage/'+ ScanneFile end as ScannedFile " & _
             ",Case when ScanneFile='' then 'False' else 'True' end as ScannedFileStatus,ApproveRemark,a.CustomerName," & _
             " a.MobileNo as Cmobileno,a.City,e.projectname,F.FloorType,g.Flatno,g.amount as BookingAmount from BookingRequest as a  with(nolock)," & _
             " M_BankMaster as b  with(nolock), M_MemberMaster as c with(nolock) ,M_Paymodemaster as d with(nolock)," & _
             " M_Projectmaster as e with(nolock),M_FloorMaster as f with(nolock),M_Flatimagemaster as g where a.Formno=c.Formno " & _
             " and  a.BankId=b.BankCode and b.RowStatus='Y' and a.pid=d.pid and d.activeStatus='Y' and a.projectid=e.id and " & _
             " e.activeStatus='Y' and f.id=a.floorid and f.activeStatus='Y' and g.id=a.flatid and g.activeStatus='Y'" & Condition & "      order by ReqNo "
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
        Catch ex As Exception

        End Try

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
        Try

        
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
            Dim lblflatid As Label

            For Each Gvr As GridViewRow In GvData.Rows
                Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
                lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
                lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
                lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
                LblAmount = DirectCast(Gvr.FindControl("lblAmount"), Label)
                lblPaymode = DirectCast(Gvr.FindControl("LblPaymode"), Label)
                LblMobileno = DirectCast(Gvr.FindControl("LblMobileno"), Label)

                lblflatid = DirectCast(Gvr.FindControl("LblFlatid"), Label)
                If Chk.Checked = True And Chk.Enabled = True Then

                    Dim Dr_ As DataRow
                    Dim strSql As String = "Select Count(*) As Cnt  from BookingRequest Where ReqNo = '" & lblReqno.Text & "' and IsApprove <> 'N'"
                    Dt1 = New DataTable
                    Dt1 = objDAL.GetData(strSql)
                    If (Val(Dt1.Rows(0)("Cnt")) = 0) Then
                        If AprvType = "Y" Then
                            Remark = " Approve Booking Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                            sql = sql & "Update M_Flatimagemaster Set SoldStatus='Y',SoldDate=GetDate() where id='" & Val(lblflatid.Text) & "';"
                        Else
                            sql = sql & "Update M_Flatimagemaster Set SoldStatus='N',SoldDate=GetDate() where id='" & Val(lblflatid.Text) & "';"

                        End If
                        sql = sql & ";Update BookingRequest SET  IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApproveBy='" & Val(Session("UserID")) & "',ApproveRemark='" & TxtARemark.Text & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
                        sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp)Values" & _
                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Approve Booking Request ','Approve Booking Request','" & Remark & "',Getdate())"

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
        Catch ex As Exception

        End Try
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
        Try
            AprvAction("Y", "A")
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click
        Try

            AprvAction("R", "R")
        Catch ex As Exception

        End Try

    End Sub
    End Class
