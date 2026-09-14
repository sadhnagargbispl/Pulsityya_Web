Imports System.Data
Partial Class App_UI_Application_Pages_UnBlockzara
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim dtIDList As New DataTable
    Dim dbConnect As cls_DataAccess
    Dim objGen As clsGeneral = New clsGeneral

#Region "Page Events"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        lblrecordcount.Text = ""
        If Not Page.IsPostBack Then
            txtMemberId.Text = ""
            If Request.QueryString.HasKeys Then
                If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                    If Request.QueryString("Tp") = "S" Then
                        rdblistChoice.SelectedValue = "single"
                    Else
                        rdblistChoice.SelectedValue = "multiple"
                    End If
                    txtMemberId.Text = Request.QueryString("key").ToString
                    ShowDetail()
                End If
            End If
            If Session("CompID") = "1057" Then
                divremark.Visible = True
            End If
        End If
    End Sub
#End Region

    Private Sub ShowDetail()
        Dim idNo As String
        If txtMemberId.Text <> "" Then
            idNo = objDAL.ClearInject(txtMemberId.Text)
            lblError.Text = ""
            Dim qry As String = "Select FormNo FROM M_MemberMaster WHERE IDNO='" & idNo & "'"
            dtData = New DataTable
            dtData = objDAL.GetData(qry)
            If dtData.Rows.Count > 0 Then
                TxtFormNo.Text = dtData.Rows(0)(0)
            Else
                lblError.Text = "Member ID not exist. Please provide correct member ID."
                lblError.Visible = True
                Exit Sub
            End If
            If rdblistChoice.SelectedValue = "single" Then
                If Session("CompID") = "1057" Then
                    qry = "Select IDNO as [Member ID],MemName as [Member Name],Replace(Convert(varchar,DOJ,106),' ','-') as [Join Date],KitName as [Kit Name],Replace(Convert(varchar,a.UpgradeDate,106),' ','-') as [Activ.Date],IsNull(a.ReferalIDNo,' ') as [Sponsor],IsNull(a.ReferralName,'') as [Sponsor Name],BlockStatus as [Block Status],CASE WHEN ISBlock='Y' THEN Replace(Convert(varchar,BlockDate,106),' ','-') ELSE '' END as [Block Date],a.BlockRemark from V#MemberDetail as a Where IDNO like '" & idNo & "'"
                Else
                    qry = "Select IDNO as [Member ID],MemName as [Member Name],Replace(Convert(varchar,DOJ,106),' ','-') as [Join Date],KitName as [Kit Name],Replace(Convert(varchar,a.UpgradeDate,106),' ','-') as [Activ.Date],IsNull(a.ReferalIDNo,' ') as [Sponsor],IsNull(a.ReferralName,'') as [Sponsor Name],BlockStatus as [Block Status],CASE WHEN ISBlock='Y' THEN Replace(Convert(varchar,BlockDate,106),' ','-') ELSE '' END as [Block Date] from V#MemberDetail as a Where IDNO like '" & idNo & "'"
                End If

            Else
                If Session("CompID") = "1057" Then
                    qry = "Select a.IDNO as [Member ID],a.MemName as [Member Name],Replace(Convert(varchar,a.DOJ,106),' ','-') as [Join Date],a.KitName as [Kit Name],Replace(Convert(varchar,a.UpgradeDate,106),' ','-') as [Activ.Date],IsNull(a.ReferalIDNo,' ') as [Sponsor],IsNull(a.ReferralName,'') as [Sponsor Name],BlockStatus as [Block Status],CASE WHEN ISBlock='Y' THEN Replace(Convert(varchar,BlockDate,106),' ','-') ELSE '' END as [Block Date],a.BlockRemark from V#MemberDetail as a,M_MemTreeRelation as b Where b.FormNoDwn=a.FormNo AND b.FormNo='" & Trim(TxtFormNo.Text) & "'"
                Else
                    qry = "Select a.IDNO as [Member ID],a.MemName as [Member Name],Replace(Convert(varchar,a.DOJ,106),' ','-') as [Join Date],a.KitName as [Kit Name],Replace(Convert(varchar,a.UpgradeDate,106),' ','-') as [Activ.Date],IsNull(a.ReferalIDNo,' ') as [Sponsor],IsNull(a.ReferralName,'') as [Sponsor Name],BlockStatus as [Block Status],CASE WHEN ISBlock='Y' THEN Replace(Convert(varchar,BlockDate,106),' ','-') ELSE '' END as [Block Date] from V#MemberDetail as a,M_MemTreeRelation as b Where b.FormNoDwn=a.FormNo AND b.FormNo='" & Trim(TxtFormNo.Text) & "'"
                End If
            End If

            dtData = New DataTable
            dtData = objDAL.GetData(qry)
            If dtData.Rows.Count = 0 Then
                lblError.Text = "Detail Not Found."
                lblError.Visible = True
            Else
                GvData.DataSource = dtData
                Session("UnBlockIDs") = dtData
                GvData.DataBind()
                GvData.Visible = True
                lblrecordcount.Text = "Record Count : " & dtData.Rows.Count
                BtnBlock.Visible = True
            End If
        Else
            lblError.Text = "Member Id can not be blank. Please provide member ID to proceed."
            lblError.Visible = True
        End If
    End Sub

    Protected Sub rdblistChoice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdblistChoice.SelectedIndexChanged
        If Trim(txtMemberId.Text) <> "" Then
            ShowDetail()
        End If
    End Sub

    Protected Sub btnShowSingleDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowSingleDetail.Click
        ShowDetail()
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("UnBlockIDs")
        GvData.DataBind()
    End Sub

    Protected Sub BtnBlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBlock.Click
        Dim Sql, scrname As String
        Dim Remark As String = ""
        If rdblistChoice.SelectedValue = "single" Then
            Remark = " UnBlock Id " & txtMemberId.Text & " By " & Session("UserName") & ""
            If Session("CompId") = 1057 Then
                Sql = "UPDATE M_MemberMaster SET IsBlock='N',BlockDate=GETDATE(),BlockRemark='" & txremarks.Text & "' WHERE FormNo='" & Trim(TxtFormNo.Text) & "' AND IsBlock='Y' "
                Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
       "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','UnBlock ','UnBlock Id','" & Remark & "',Getdate(),'" & TxtFormNo.Text & "')"

                Sql = Sql & "; Insert Into TempMemberMaster Select *,'UnBlock Id',GetDate(),'B' From M_MemberMaster Where FormNo='" & Trim(TxtFormNo.Text) & "'"
            Else
                Sql = "UPDATE M_MemberMaster SET IsBlock='N',BlockDate=GETDATE() WHERE FormNo='" & Trim(TxtFormNo.Text) & "' AND IsBlock='Y' "
                Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
       "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','UnBlock ','UnBlock Id','" & Remark & "',Getdate(),'" & TxtFormNo.Text & "')"

            End If

            scrname = "ID"
        Else
            Remark = " UnBlock Tree " & txtMemberId.Text & " By " & Session("UserName") & ""
            If Session("") = 1057 Then

                Sql = "UPDATE M_MemberMaster SET IsBlock='N',BlockDate=GETDATE(),BlockRemark='" & Remark & "' FROM M_MemberMaster as a,M_MemTreeRelation as b WHERE b.FormNodwn=a.FormNo AND b.FormNo='" & Trim(TxtFormNo.Text) & "'  AND a.IsBlock='Y'"
                Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
        "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','UnBlock ','UnBlock Tree','" & txremarks.Text & "',Getdate(),'" & TxtFormNo.Text & "')"

                Sql = Sql & "; Insert Into TempMemberMaster Select *,'UnBlock Tree',GetDate(),'B' From M_MemberMaster Where FormNo='" & Trim(TxtFormNo.Text) & "'"
            Else
                Sql = "UPDATE M_MemberMaster SET IsBlock='N',BlockDate=GETDATE(),BlockRemark='" & Remark & "' FROM M_MemberMaster as a,M_MemTreeRelation as b WHERE b.FormNodwn=a.FormNo AND b.FormNo='" & Trim(TxtFormNo.Text) & "'  AND a.IsBlock='Y'"
                Sql = Sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
        "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','UnBlock ','UnBlock Tree','" & Remark & "',Getdate(),'" & TxtFormNo.Text & "')"


            End If
            scrname = "Tree"
        End If
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('" & scrname & " unblocked Successfully!! ');" & "</SCRIPT>"
            TxtFormNo.Text = "" : txtMemberId.Text = "" : BtnBlock.Visible = False
            GvData.Visible = False : lblrecordcount.Text = ""
            txremarks.Text = ""
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not unblocked!! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

End Class
