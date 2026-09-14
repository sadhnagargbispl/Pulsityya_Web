Imports System.Data
Imports System.Data.SqlClient
Partial Class Reward
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim conn As New SqlConnection
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                btnExport.Enabled = False
                If Session("CompId") = "1010" Or Session("CompId") = "1103" Or Session("CompId") = "1108" Then
                    If ddlsearchtype.SelectedValue = 1 Then
                        Div1.Visible = True
                        Div3.Visible = True
                    Else
                        Div2.Visible = True
                        Div1.Visible = True
                        Div3.Visible = True
                    End If
                End If
                If Session("CompId") = "1103" Or Session("CompId") = "1108" Then
                    lblReward.Text = "Rank / Reward"
                Else
                    lblReward.Text = "Reward"
                End If
                If Session("CompId") = "1103" Or Session("CompId") = "1108" Then
                    GrdViewWellValue.Columns(6).Visible = False
                End If
                If Session("CompID").ToString() = "1103" Or Session("CompId") = "1108" Then
                    GrdViewWellValue.Columns(4).Visible = True
                Else
                    GrdViewWellValue.Columns(4).Visible = False
                End If

                FillReward()
            Else
                Response.Redirect("logout.aspx")
            End If
        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("RewardList")
        GvData.DataBind()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Session("CompId") = "1010" Or Session("CompId") = "1103" Or Session("CompId") = "1108" Then
            If ddlsearchtype.SelectedValue = 1 Then
                FillData()
            Else
                If Chkmemid.Checked Then
                    FillData()
                Else
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Enter Member ID.!');", True)
                    Exit Sub
                End If
            End If
        Else
            If ddlsearchtype.SelectedValue = 1 Then
                FillData()
            Else
                If Chkmemid.Checked Then
                    FillData()
                Else
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Enter Member ID.!');", True)
                    Exit Sub
                End If
            End If
        End If
    End Sub
    Protected Sub FillData()
        Try
            Dim formno As String = ""
            Dim condition1 As String = ""
            Dim condition2 As String = ""
            Dim condition As String = ""
            Dim qry1 As String = ""
            If Session("CompID") = 1075 Then
                If Chkmemid.Checked Then
                    formno = GetFormNo()
                    condition = condition & " And c.Formno='" & Val(formno) & "'"
                End If
                If ddllist.SelectedValue <> 0 Then
                    condition = condition & " And b.Rewardid='" & ddllist.SelectedValue & "'"
                End If
                If txtFromDate.Text <> "" Then
                    condition1 = " And Cast(Convert(varchar,b.frmDate,106) as DateTime)>='" & txtFromDate.Text & "'"
                End If
                If TxtToDate.Text <> "" Then
                    condition2 = " And Cast(Convert(varchar,b.frmDate,106) as DateTime)<='" & TxtToDate.Text & "'"
                End If
                qry1 = "select a.Idno,c.Formno,a.MemFirstname as MemberName,b.RewardNew as Rewardname,b.Rewardid,CONVERT(varchar,d.frmDate,106) as AchieveDate "
                qry1 &= "from M_Membermaster as a,MstReward as b ,MstRewardAchievers as c,D_Sessnmaster as d where(c.Rewardid = b.Rewardid And c.SEssid = d.SEssid)"
                qry1 &= "and a.Formno=c.Formno  " & condition & "  " & condition1 & " " & condition2 & " "
                dtData = New DataTable
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dtData = objDAL.GetData(qry1)
                GWdataproho.DataSource = dtData
                GWdataproho.DataBind()
                Session("RewardList") = dtData
                If dtData.Rows.Count > 0 Then
                    btnExport.Enabled = True
                Else
                    btnExport.Enabled = False
                End If
            ElseIf Session("CompId") = "1010" Or Session("CompId") = "1103" Or Session("CompId") = "1108" Then
                If ddlsearchtype.SelectedValue = 1 Then
                    If Chkmemid.Checked Then
                        formno = GetFormNo()
                    End If
                    qry1 = "Exec Sp_GetRewardPointReport '" & Val(formno) & "','" & ddllist.SelectedValue & "','" & txtFromDate.Text & "',"
                    qry1 &= "'" & TxtToDate.Text & "','" & ddltype.SelectedValue & "','" & ddlsearchtype.SelectedValue & "','" & DDlPaidStatus.SelectedValue & "','N'"
                    dtData = New DataTable
                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    dtData = objDAL.GetData(qry1)
                    GrdViewWellValue.DataSource = dtData
                    GrdViewWellValue.DataBind()
                    Session("RewardList") = dtData
                    If dtData.Rows.Count > 0 Then
                        SelfDiv.Visible = True
                        btnExport.Enabled = True
                        TeamDiv.Visible = False
                    Else
                        btnExport.Enabled = False
                        SelfDiv.Visible = False
                        TeamDiv.Visible = False
                    End If
                Else
                    If Chkmemid.Checked Then
                        formno = GetFormNo()
                    End If
                    qry1 = "Exec Sp_GetRewardPointReport '" & Val(formno) & "','" & ddllist.SelectedValue & "','" & txtFromDate.Text & "','" & TxtToDate.Text & "',"
                    qry1 &= "'" & ddltype.SelectedValue & "','" & ddlsearchtype.SelectedValue & "','" & DDlPaidStatus.SelectedValue & "','N'"
                    Dim dtData1 As DataTable = New DataTable()
                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    dtData1 = objDAL.GetData(qry1)
                    GrdViewWellValue1.DataSource = dtData1
                    GrdViewWellValue1.DataBind()

                    Session("RewardList") = dtData1
                    If dtData1.Rows.Count > 0 Then
                        SelfDiv.Visible = False
                        btnExport.Enabled = True
                        TeamDiv.Visible = True
                    Else
                        btnExport.Enabled = False
                        SelfDiv.Visible = False
                        TeamDiv.Visible = False
                    End If
                End If
            Else
                If Chkmemid.Checked Then
                    formno = GetFormNo()
                    condition = condition & " And b.Formno='" & Val(formno) & "'"
                End If
                If ddllist.SelectedValue <> 0 Then
                    condition = condition & " And b.Rewardid='" & ddllist.SelectedValue & "'"
                End If
                If txtFromDate.Text <> "" Then
                    condition1 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)>='" & txtFromDate.Text & "'"
                End If
                If TxtToDate.Text <> "" Then
                    condition2 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)<='" & TxtToDate.Text & "'"
                End If
                qry1 = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,a.Idno,a.MemFirstname as [Member Name],b.Reward as [Reward Name],"
                qry1 &= " Replace(Convert(Varchar,b.RectimeStamp,106),' ','-') as  [Achive Date], "
                qry1 &= " Case when c.Rdays>=b.Daycnt then 'Time Limit' Else 'No Time Limit' End as [Reward Type]"
                qry1 &= " from M_Membermaster as a,M_RewardFinal as b ,M_RewardMaster as c where c.Rewardid=b.Rewardid and "
                qry1 &= "a.Formno=b.Formno " & condition & " " & condition1 & " " & condition2 & " "
                dtData = New DataTable
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dtData = objDAL.GetData(qry1)
                GvData.DataSource = dtData
                GvData.DataBind()
                Session("RewardList") = dtData
                If dtData.Rows.Count > 0 Then
                    btnExport.Enabled = True
                Else
                    btnExport.Enabled = False
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String = ""
        Dim formno As String = ""
        idNo = txtMemId.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
            lblErr.Visible = False
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemId.Text = ""
        End If
        Return formno
    End Function
    Private Sub FillReward()
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim S As String = ""
        If (Session("CompID") = "1004") Then
            S = "Exec Sp_FillRewardDropwown"
        ElseIf Session("CompID") = 1075 Then
            S = "Select * From (Select 0 As Rewardid, '-- ALL --' As Reward Union ALL " & _
            "select Rewardid,Rewardnew As Reward from MstReward Where ActiveStatus='Y' ) as Temp "
        ElseIf Session("CompID") = 1010 Then

            S = "Select * From (Select 0 As Rewardid, '-- ALL --' As Reward Union ALL" & _
        " select Rewardid,Rank as Reward from M_RewardMaster Where ActiveStatus='Y' ) as Temp "
        ElseIf Session("CompID") = 1103 Then
            S = "Select * From (Select 0 As Rewardid, '-- ALL --' As Reward Union ALL" & _
         " select Rewardid,rank + ' - ' + Reward from MstRewards Where ActiveStatus='Y' ) as Temp "

      
        Else
            S = "Select * From (Select 0 As Rewardid, '-- ALL --' As Reward Union ALL" & _
         " select Rewardid,Reward from M_RewardMaster Where ActiveStatus='Y' ) as Temp "
        End If
        Dim dt As New DataTable
        dt = objDAL.GetData(S)

        ddllist.DataSource = dt
        ddllist.DataTextField = "Reward"
        ddllist.DataValueField = "rewardid"
        ddllist.DataBind()
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            '     Dim condition As String = ""
            Dim condition1 As String = ""
            Dim condition2 As String = ""
            Dim qry1 As String = ""
            If Session("CompId") = 1075 Then
                If Chkmemid.Checked Then
                    formno = GetFormNo()
                    Condition = Condition & " And c.Formno='" & Val(formno) & "'"
                End If

                If ddllist.SelectedValue <> 0 Then
                    Condition = Condition & " And b.Rewardid='" & ddllist.SelectedValue & "'"
                End If
                If txtFromDate.Text <> "" Then
                    condition1 = " And Cast(Convert(varchar,b.frmDate,106) as DateTime)>='" & txtFromDate.Text & "'"
                End If
                If TxtToDate.Text <> "" Then
                    condition2 = " And Cast(Convert(varchar,b.frmDate,106) as DateTime)<='" & TxtToDate.Text & "'"
                End If
                qry1 = "select a.Idno,c.Formno,a.MemFirstname as MemberName,b.RewardNew as Rewardname, " & _
            "CONVERT(varchar,d.frmDate,106) as AchieveDate " & _
            "from M_Membermaster as a,MstReward as b ,MstRewardAchievers as c,D_Sessnmaster as d " & _
                "  where(c.Rewardid = b.Rewardid And c.SEssid = d.SEssid)" & _
               "and    a.Formno=c.Formno  " & Condition & " " & _
                " " & condition1 & " " & condition2 & " "

            ElseIf Session("CompId") = "1010" Or Session("CompId") = "1103" Or Session("CompId") = "1108" Then
                If ddlsearchtype.SelectedValue = 1 Then
                    If Chkmemid.Checked Then
                        formno = GetFormNo()
                    End If
                    qry1 = "Exec Sp_GetRewardPointReport '" & Val(formno) & "','" & ddllist.SelectedValue & "','" & txtFromDate.Text & "',"
                    qry1 &= "'" & TxtToDate.Text & "','" & ddltype.SelectedValue & "','" & ddlsearchtype.SelectedValue & "','" & DDlPaidStatus.SelectedValue & "','Y'"

                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    dtTemp = New DataTable
                    dtTemp = objDAL.GetData(qry1)
                    dg.DataSource = dtTemp
                    dg.DataBind()
                Else
                    If Chkmemid.Checked Then
                        formno = GetFormNo()
                    End If
                    qry1 = "Exec Sp_GetRewardPointReport '" & Val(formno) & "','" & ddllist.SelectedValue & "','" & txtFromDate.Text & "','" & TxtToDate.Text & "',"
                    qry1 &= "'" & ddltype.SelectedValue & "','" & ddlsearchtype.SelectedValue & "','" & DDlPaidStatus.SelectedValue & "','Y'"
                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    dtTemp = New DataTable
                    dtTemp = objDAL.GetData(qry1)
                    dg.DataSource = dtTemp
                    dg.DataBind()
                    ExportToExcel("Reward.xls", dg)
                End If
                'ElseIf Session("CompId") = "1010" Then
                '    If ddlsearchtype.SelectedValue = 1 Then
                '        If Chkmemid.Checked Then
                '            formno = GetFormNo()
                '            Condition = Condition & " And b.Formno='" & Val(formno) & "'"
                '        End If

                '        If ddllist.SelectedValue <> 0 Then
                '            Condition = Condition & " And b.Rewardid='" & ddllist.SelectedValue & "'"
                '        End If
                '        If txtFromDate.Text <> "" Then
                '            condition1 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)>='" & txtFromDate.Text & "'"
                '        End If
                '        If TxtToDate.Text <> "" Then
                '            condition2 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)<='" & TxtToDate.Text & "'"
                '        End If

                '        If ddltype.SelectedValue <> 0 Then
                '            Condition = Condition & " And a.Legno='" & ddltype.SelectedValue & "'"
                '        End If
                '        qry1 = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,a.Idno,a.MemFirstname as [Member Name],b.Reward as [Reward Name]," & _
                '                 " Replace(Convert(Varchar,b.RectimeStamp,106),' ','-') as  [Achive Date], " & _
                '                " Case when c.Rdays>=b.Daycnt then 'Time Limit' Else 'No Time Limit' End as [Reward Type]" & _
                '                " from M_Membermaster as a,M_RewardFinal as b ,M_RewardMaster as c where c.Rewardid=b.Rewardid and    a.Formno=b.Formno " & Condition & " " & _
                '                " " & condition1 & " " & condition2 & "  "
                '        dtData = New DataTable
                '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                '        dtData = objDAL.GetData(qry1)

                '        GvData.DataSource = dtData
                '        GvData.DataBind()
                '        Session("RewardList") = dtData
                '    Else
                '        If Chkmemid.Checked Then
                '            formno = GetFormNo()
                '            Condition = Condition & " And Mt.Formno='" & Val(formno) & "'"
                '        End If

                '        If ddllist.SelectedValue <> 0 Then
                '            Condition = Condition & " And b.Rewardid='" & ddllist.SelectedValue & "'"
                '        End If
                '        If txtFromDate.Text <> "" Then
                '            condition1 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)>='" & txtFromDate.Text & "'"
                '        End If
                '        If TxtToDate.Text <> "" Then
                '            condition2 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)<='" & TxtToDate.Text & "'"
                '        End If

                '        If ddltype.SelectedValue <> 0 Then
                '            Condition = Condition & " And MT.Legno='" & ddltype.SelectedValue & "'"
                '        End If
                '        qry1 = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo, a.Idno,a.MemFirstname as [Member Name],b.Reward as [Reward Name]," & _
                '                     " Replace(Convert(Varchar,b.RectimeStamp,106),' ','-') as  [Achive Date]," & _
                '                    " Case when c.Rdays>=b.Daycnt then 'Time Limit' Else 'No Time Limit' End as [Reward Type]" & _
                '                    ",Case When Mt.LegNo= 1  then 'Left'  When Mt.LegNo= 2  then 'Right'  End As Side  " & _
                '                    " from M_Membermaster as a,M_MemtreeRelation  As Mt,M_RewardFinal as b ,M_RewardMaster as c where c.Rewardid=b.Rewardid and a.Formno=b.Formno And a.Formno=Mt.FormnoDwn " & Condition & " " & _
                '                    " " & condition1 & " " & condition2 & " "
                '        dtData = New DataTable
                '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                '        dtData = objDAL.GetData(qry1)

                '        GvData.DataSource = dtData
                '        GvData.DataBind()
                '        Session("RewardList") = dtData
                '    End If
            Else
                If Chkmemid.Checked Then
                    formno = GetFormNo()
                    Condition = Condition & " And b.Formno='" & Val(formno) & "'"
                End If
                If ddllist.SelectedValue <> 0 Then
                    Condition = Condition & " And b.Rewardid='" & ddllist.SelectedValue & "'"
                End If
                If txtFromDate.Text <> "" Then
                    condition1 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)>='" & txtFromDate.Text & "'"
                End If
                If TxtToDate.Text <> "" Then
                    condition2 = " And Cast(Convert(varchar,b.RectimeStamp,106) as DateTime)<='" & TxtToDate.Text & "'"
                End If

                qry1 = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,a.Idno,a.MemFirstname as [Member Name],b.Reward as [Reward Name]," & _
                             " Replace(Convert(Varchar,b.RectimeStamp,106),' ','-') as  [Achive Date], " & _
                            " Case when c.Rdays>=b.Daycnt then 'Time Limit' Else 'No Time Limit' End as [Reward Type]" & _
                            " from M_Membermaster as a,M_RewardFinal as b ,M_RewardMaster as c where c.Rewardid=b.Rewardid and    a.Formno=b.Formno " & Condition & " " & _
                            " " & condition1 & " " & condition2 & " "
            End If

            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("Reward.xls", dg)

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
    Protected Sub ddlsearchtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlsearchtype.TextChanged
        If Session("CompId") = "1010" Or Session("CompId") = "1103" Or Session("CompId") = "1108" Then
            If ddlsearchtype.SelectedValue = 1 Then
                Div1.Visible = True
                Div2.Visible = False
                SelfDiv.Visible = True
                TeamDiv.Visible = False
            Else
                Div2.Visible = True
                Div1.Visible = True
                Dim dtData As DataTable = New DataTable()
                GrdViewWellValue.DataSource = dtData
                GrdViewWellValue.DataBind()
                GrdViewWellValue1.DataSource = dtData
                GrdViewWellValue1.DataBind()
                SelfDiv.Visible = False
                TeamDiv.Visible = True
            End If
        End If
    End Sub

    Protected Sub GrdViewWellValue1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdViewWellValue1.PageIndexChanging
        GrdViewWellValue1.PageIndex = e.NewPageIndex
        GrdViewWellValue1.DataSource = Session("RewardList")
        GrdViewWellValue1.DataBind()
    End Sub

    Protected Sub GrdViewWellValue_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdViewWellValue.PageIndexChanging
        GrdViewWellValue.PageIndex = e.NewPageIndex
        GrdViewWellValue.DataSource = Session("RewardList")
        GrdViewWellValue.DataBind()
    End Sub
End Class
