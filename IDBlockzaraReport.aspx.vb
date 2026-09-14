Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_IDBlockzaraReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" And Session("UserPermission") = 1 Then
            Session("PageName") = "Home"

        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'BindData()
            'BindDataSummary()

            BindData()
        End If
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")

        Dim url As String = String.Empty

        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri



        url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")


        url = url.ToLower

        Dim sql As String = ""
        If Session("CompId") = "1055" Then
            sql = "Select top 500  A.IDNo,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
       " case when Cast(a.Doj as Date)>='01-Jan-2022' then IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') else '01-Jan-2022' end As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
       "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
        ElseIf Session("CompID") = "1057" Or Session("CompID") = "1084" Then
            sql = "Select top 500  A.IDNo,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
       " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
       "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case when a.IsBlock='Y' then 'Blocked' Else 'Unblocked' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode, IsNull(REPLACE(CONVERT(VARCHAR(11), a.BlockDate , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.BlockDate ,100 ) ,7), 6, 0, ' '),'') as [Block Date]" & _
"  ,a.blockremark From tempMemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID and a.Ctype='B' " & Condition & " Order by A.BlockDate Desc"
        ElseIf Session("CompID") = "1081" Then
            sql = "Select top 500  A.IDNo,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
       " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
       "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case when a.IsBlock='Y' then 'Blocked' Else 'Unblocked' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode, IsNull(REPLACE(CONVERT(VARCHAR(11), a.BlockDate , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.BlockDate ,100 ) ,7), 6, 0, ' '),'') as [Block Date]" & _
"  ,a.blockremark From tempMemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID and a.Ctype='B' " & Condition & " Order by A.BlockDate Desc"

        ElseIf Session("CompID") <> "1025" Then
            sql = "Select top 500  A.IDNo,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
       " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
       "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"

        Else
            sql = "Select top 500  A.IDNo,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
     " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj" & _
     " ,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
     "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,cm.CountryName,'true' as CountryVisible,cm.stdcode From M_MemberMaster As A with(nolock) Inner Join M_StateDivMaster As B with(nolock) " & _
" on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo " & _
" Left Join M_MemberMaster As e with(nolock) On A.UpLnFormNo=e.Formno " & _
" Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID " & _
"left join M_countrymaster as cm with(nolock) on cm.cid=a.CountryId and cm.Rowstatus='Y'" & Condition & " Order by A.Doj Desc"

        End If
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        For Each Dr As DataRow In dtData.Rows
            Dr("LgnID") = Crypto.Encrypt("uid=" & Dr("IDNo") & "&pwd=" & Dr("Passw"))
        Next
        GvData.DataSource = dtData
        GvData.DataBind()
        If Session("compid") = "1025" Then
            GvData.Columns(14).Visible = True
            GvData.Columns(15).Visible = True
        Else
            'GvData.Columns(14).Visible = False
            'GvData.Columns(15).Visible = False
        End If
        Session("MemberData") = dtData
        ViewState("Idno") = "Idno"
        ViewState("Sort_Order") = "ASC"
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MemberData")
        GvData.DataBind()
    End Sub
    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        If e.SortExpression = ViewState("Idno").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        'Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/Upaarrow.png", "~/Images/DownArrow.png")
                        'tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        'tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = e.SortExpression Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        'Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/Upaarrow.png", "~/Images/DownArrow.png")
                        'tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        'tableCell.Controls.Add(img)
                    End If
                Next
            End If

        Else
            RebindData(e.SortExpression, "ASC")
        End If
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("MemberData"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("Idno") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""
        If ddlSearch.SelectedValue = "0" And ddlstatus.SelectedValue = "A" Then
            Condition = ""
            'Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
            'Exit Sub
        ElseIf ddlSearch.SelectedValue = "0" Or ddlstatus.SelectedValue <> "A" Then
            Condition = " Where (a.IDNO like '%" & txtSrchText.Text & "%' OR a.Mobl like '%" & txtSrchText.Text & "%' OR a.email like '%" & txtSrchText.Text & "%') and a.Isblock='" & ddlstatus.SelectedValue & "' "
        ElseIf ddlSearch.SelectedValue = "StateName" Then
            Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "DOJ" Then
            If Session("CompId") = "1055" Then
                Condition = " Where Replace(Convert(varchar,Case when Cast(a.DOJ as Date)>='01-Jan-2022' then a.DOj else '01-Jan-2022' end ,106),' ','-') like '%" & txtSrchText.Text & "%'"

            Else
                Condition = " Where Replace(Convert(varchar, a.Doj,106),' ','-') like '%" & txtSrchText.Text & "%'"

            End If
        ElseIf ddlSearch.SelectedValue = "MemName" Then
            Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "RMemName" Then
            Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "KitName" Then
            Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "RMemID" Then
            Condition = " Where e.IDNO = '" & txtSrchText.Text & "' "
        ElseIf ddlstatus.SelectedValue = "A" Then
            Condition = " Where (a.IDNO like '%" & txtSrchText.Text & "%' OR a.Mobl like '%" & txtSrchText.Text & "%' OR a.email like '%" & txtSrchText.Text & "%')  "
        ElseIf ddlSearch.SelectedValue = "IDNo" Or ddlstatus.SelectedValue <> "A" Then
            Condition = " Where a.IDNO = '" & txtSrchText.Text & "' and a.Isblock='" & ddlstatus.SelectedValue & "' "
        ElseIf ddlSearch.SelectedValue = "Mobl" Or ddlstatus.SelectedValue <> "A" Then
            Condition = " Where a.Mobl = '" & txtSrchText.Text & "' and a.Isblock='" & ddlstatus.SelectedValue & "' "
        ElseIf ddlSearch.SelectedValue = "EMail" Or ddlstatus.SelectedValue <> "A" Then
            Condition = " Where a.EMail = '" & txtSrchText.Text & "' and a.Isblock='" & ddlstatus.SelectedValue & "' "
        Else
            Condition = " Where a." & ddlSearch.SelectedValue & " like '%" & txtSrchText.Text & "%'"
        End If
        BindData(Condition)
    End Sub

    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Me.BindData(1)
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Exportdata()
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

    Public Sub Exportdata()
        Dim Condition As String = ""
        If ddlSearch.SelectedValue = "0" And ddlstatus.SelectedValue = "A" Then
            Condition = ""
            'Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
            'Exit Sub
        ElseIf ddlSearch.SelectedValue = "0" Or ddlstatus.SelectedValue <> "A" Then
            Condition = " Where (a.IDNO like '%" & txtSrchText.Text & "%' OR a.Mobl like '%" & txtSrchText.Text & "%' OR a.email like '%" & txtSrchText.Text & "%') and a.Isblock='" & ddlstatus.SelectedValue & "' "
        ElseIf ddlSearch.SelectedValue = "StateName" Then
            Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "DOJ" Then
            If Session("CompId") = "1055" Then
                Condition = " Where Replace(Convert(varchar,Case when Cast(a.DOJ as date) >='01-Jan-2022' then a.Doj else '01-Jan-2022' end,106),' ','-') like '%" & txtSrchText.Text & "%'"

            Else
                Condition = " Where Replace(Convert(varchar, a.Doj,106),' ','-') like '%" & txtSrchText.Text & "%'"

            End If
        ElseIf ddlSearch.SelectedValue = "MemName" Then
            Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "RMemName" Then
            Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "KitName" Then
            Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "RMemID" Then
            Condition = " Where e.IDNO = '" & txtSrchText.Text & "' "
        ElseIf ddlstatus.SelectedValue = "A" Then
            'Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
            Condition = " Where (a.IDNO like '%" & txtSrchText.Text & "%' OR a.Mobl like '%" & txtSrchText.Text & "%' OR a.email like '%" & txtSrchText.Text & "%')  "
        ElseIf ddlSearch.SelectedValue = "IDNo" Or ddlstatus.SelectedValue <> "A" Then
            Condition = " Where a.IDNO = '" & txtSrchText.Text & "' and a.Isblock='" & ddlstatus.SelectedValue & "' "
        Else
            Condition = " Where a." & ddlSearch.SelectedValue & " like '%" & txtSrchText.Text & "%'"
        End If
        Dim dtTemp As New DataTable
        Dim dg As New DataGrid
        Try
            Dim sql As String = ""
            If Session("CompID") = "1057" Or Session("CompID") = "1084" Then
                sql = "Select top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName," & _
          " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.Mobl As MobileNo,A.Email," & _
   "  Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Unblocked' End As Status," & _
   " IsNull(REPLACE(CONVERT(VARCHAR(11), a.BlockDate , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.BlockDate ,100 ) ,7), 6, 0, ' '),'') as [Block Date]" & _
   "  ,a.blockremark From tempMemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
   " Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID and A.ctype='B' " & Condition & " Order by A.BlockDate Desc"
            ElseIf Session("CompID") = "1081" Then
                sql = "Select top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName," & _
          " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.Mobl As MobileNo,A.Email," & _
   "  Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Unblocked' End As Status," & _
   " IsNull(REPLACE(CONVERT(VARCHAR(11), a.BlockDate , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.BlockDate ,100 ) ,7), 6, 0, ' '),'') as [Block Date]" & _
   "  ,a.blockremark From tempMemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
   " Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID and A.ctype='B' " & Condition & " Order by A.BlockDate Desc"

            ElseIf Session("compid") <> "1025" Then
                sql = "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName," & _
 " A.Email,A.Mobl As MobileNo,D.KitName as packageName,D.KitAmount as PackageMRP,D.Bv as PackageBv,IsNull(REPLACE(CONVERT(VARCHAR, a.Doj , 106), ' ', '-'),' ') as DojDate," & _
 " STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime,A.City,B.StateName,A.Passw, " & _
 " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as UpgrdDate," & _
 " CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' " & _
 " and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status,'Rs 0.00/-' As Balance,a.ActiveStatus From M_MemberMaster As A with(nolock) " & _
 " Inner Join M_StateDivMaster As B with(nolock) on A.StateCode=B.StateCode and b.RowStatus='Y' Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e " & _
 " On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"

            Else

                sql = "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName," & _
                          " A.Email,A.Mobl As MobileNo,D.KitName as packageName,D.KitAmount as PackageMRP,D.Bv as PackageBv," & _
                          " Case when Cast(a.Doj as Date)>='01-Jan-2022' then IsNull(REPLACE(CONVERT(VARCHAR, a.Doj , 106), ' ', '-'),' ')else '01-Jan-2022' end as DojDate," & _
               " STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime,A.City,B.StateName,cm.countryName,cm.stdCode,A.Passw, " & _
               " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as UpgrdDate," & _
               " CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' " & _
               " and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status,'Rs 0.00/-' As Balance,a.ActiveStatus From M_MemberMaster As A with(nolock) " & _
               " Inner Join M_StateDivMaster As B with(nolock) on A.StateCode=B.StateCode and b.RowStatus='Y' Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo " & _
               " Left Join M_MemberMaster As e with(nolock)" & _
               " On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID " & _
               "left join M_countrymaster as cm with(nolock) on cm.cid=a.CountryId and cm.Rowstatus='Y'" & Condition & " Order by A.Doj Desc"

            End If

            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)
            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("BlockunblockMemberDetail.xls", dg)
        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try

    End Sub


    Protected Sub btnExportCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportCsv.Click
        Dim strQuery As String = ""
        Dim Condition As String = ""
        If ddlSearch.SelectedValue = "0" Then
            Condition = ""

        ElseIf ddlSearch.SelectedValue = "StateName" Then
            Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "DOJ" Then
            Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "MemName" Then
            Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "RMemName" Then
            Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "KitName" Then
            Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "RMemID" Then
            Condition = " Where c.IDNO = '" & txtSrchText.Text & "' "
        ElseIf ddlSearch.SelectedValue = "IDNo" Then
            Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
        Else
            Condition = " Where a." & ddlSearch.SelectedValue & " like '%" & txtSrchText.Text & "%'"
        End If
        strQuery = "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName,A.Email,A.Mobl As MobileNo,D.KitName as packageName,D.KitAmount as PackageMRP,D.Bv as PackageBv,IsNull(REPLACE(CONVERT(VARCHAR, a.Doj , 106), ' ', '-'),' ') as DojDate,STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime,A.City,B.StateName,A.Passw, " & _
            " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as UpgrdDate,CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' then 'Active' Else 'Pending' End As Status,'Rs 0.00/-' As Balance,a.ActiveStatus From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
        Dim dt As DataTable = objDAL.GetData(strQuery)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", _
                "attachment;filename=MemberDetail.csv")
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
End Class
