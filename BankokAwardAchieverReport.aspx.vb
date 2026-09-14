Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_BankokAwardAchieverReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Report / Bangkok Award Achiever Report"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim searchtext As String = Session("Search")
        If Not Page.IsPostBack Then
            GvData.Visible = False
            gvContainer.Visible = False
            Session("AwardList") = Nothing
            If searchtext <> "" Then

            End If

        End If
    End Sub





    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("AwardList")
        GvData.DataBind()
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""

            If ChkMem.Checked Then
                formno = GetFormNo()
                Condition = Condition & " And Temp.Formno='" & Val(formno) & "'"
            End If

            Dim qry1 As String = ""
            qry1 = "select Temp.Idno as IdNo ,Temp.Memname as [Member Name],Temp.PreviousMatchedBv as [Previous Matched Bv]," & _
  " Temp.LeftDirect as [Left Direct],Temp.RightDirect as [Right Direct],Temp.RequireBv as [Require BV] ,Temp.AchievedLeftBv as [Achieved Left Bv],Temp.AchivedRightBv as [Achieved Right Bv],Temp.AchieveMatchBv as [Achieved Match Bv],Temp.Status as Status from " & _
  " (select a.Sessid,a.Formno, b.Idno,(b.MemFirstName+' '+b.MemLastName) as MemName," & _
  " LeftBv as PreviousLeftBv,RightBv as PreviousRightBv,MatchBv as PreviousMatchedBv,LeftDirect,RightDirect,RequireBv,LeftBvNew " & _
  " as AchievedLeftBv,RightBvNew as AchivedRightBv,MatchBvNew as AchieveMatchbv,Case when a.IsAchieve='Y' then 'Achieved' else 'Not Achieved' end as status from BangkokReward as a,M_Membermaster as b " & _
  " where a.Formno=b.Formno And  a.Sessid=(Select Max(Sessid) from D_sessnMaster where EndTime Is Not Null) )" & _
  " as Temp where 1=1" & Condition & "  "
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("BangkokReward.xls", dg)

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
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblErr.Text = ""
        lblCount.Text = ""
        Dim Condition As String = ""
        Dim formno As String = ""
        Dim scrName As String = ""

        If ChkMem.Checked Then
            formno = GetFormNo()
            Condition = Condition & " And Temp.Formno='" & Val(formno) & "'"
        End If

        Dim qry1 As String = ""
        qry1 = "select Temp.Idno as IdNo ,Temp.Memname as [Member Name],Temp.PreviousMatchedBv as [Previous Matched Bv]," & _
" Temp.LeftDirect as [Left Direct],Temp.RightDirect as [Right Direct],Temp.RequireBv as [Require BV] ,Temp.AchievedLeftBv as [Achieved Left Bv],Temp.AchivedRightBv as [Achieved Right Bv],Temp.AchieveMatchBv as [Achieved Match Bv],Temp.Status as Status from " & _
" (select a.Sessid,a.Formno, b.Idno,(b.MemFirstName+' '+b.MemLastName) as MemName," & _
" LeftBv as PreviousLeftBv,RightBv as PreviousRightBv,MatchBv as PreviousMatchedBv,LeftDirect,RightDirect,RequireBv,LeftBvNew " & _
" as AchievedLeftBv,RightBvNew as AchivedRightBv,MatchBvNew as AchieveMatchbv,Case when a.IsAchieve='Y' then 'Achieved' else 'Not Achieved' end as status from BangkokReward as a,M_Membermaster as b " & _
" where a.Formno=b.Formno And  a.Sessid=(Select Max(Sessid) from D_sessnMaster where EndTime Is Not Null) )" & _
" as Temp where 1=1" & Condition & "  "


        dtData = New DataTable
        dtData = objDAL.GetData(qry1)
        If dtData.Rows.Count > 0 Then
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("AwardList") = dtData
            GvData.Visible = True
            gvContainer.Visible = True
            lblCount.Text = "Total : " & dtData.Rows.Count
        Else
            GvData.Visible = False
            gvContainer.Visible = False
            lblErr.Text = "No Record Found!!"
        End If

    End Sub
    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMember.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMember.Text = ""
        End If
        Return formno
    End Function
    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("AwardList")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        'gridview.BorderWidth = "2px"
        GvData.BorderStyle = BorderStyle.Solid
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        'For i As Integer = 0 To GvData.Rows.Count - 1
        '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        'Next

        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GvData.RenderControl(hw)

        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=600,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(gridHTML)

        sb.Append(""");")

        sb.Append("printWin.document.close();")

        sb.Append("printWin.focus();")

        sb.Append("printWin.print();")

        sb.Append("printWin.close();};")

        sb.Append("</script>")

        ClientScript.RegisterStartupScript(Me.GetType(), "GridPrint", sb.ToString())

        GvData.AllowPaging = True
        GvData.PagerSettings.Visible = True
        dtData = New DataTable
        dtData = Session("AwardList")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("AwardList")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        GvData.BorderStyle = BorderStyle.Solid
        ' gridview.BorderWidth = 
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GvData.RenderControl(hw)

        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=1000,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(gridHTML)

        sb.Append(""");")

        sb.Append("printWin.document.close();")

        sb.Append("printWin.focus();")

        sb.Append("printWin.print();")

        sb.Append("printWin.close();};")

        sb.Append("</script>")

        ClientScript.RegisterStartupScript(Me.[GetType](), "GridPrint", sb.ToString())

        GvData.AllowPaging = True
        GvData.PagerSettings.Visible = True
        dtData = New DataTable
        dtData = Session("AwardList")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub


End Class
