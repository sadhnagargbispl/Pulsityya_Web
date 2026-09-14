Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class AchieverReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Report / Top 30 Achiever Report"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim searchtext As String = Session("Search")
            If Not Page.IsPostBack Then
                GvData.Visible = False
                gvContainer.Visible = False
                Session("AchieverList") = Nothing
                BindSession()
                BindClub()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim sessid As String = "0"
            If ddlSession.SelectedValue = "0" Then
                sessid = "0"
            Else
                sessid = ddlSession.SelectedValue
            End If
            Dim prms As SqlParameter() = New SqlParameter(5) {}
            prms(0) = New SqlParameter("@SessId", Convert.ToString(sessid).ToLower())
            prms(1) = New SqlParameter("@ClubId", Convert.ToString(DDlLevel.SelectedValue))
            prms(2) = New SqlParameter("@PageIndex", 1)
            prms(3) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(4) = New SqlParameter("@IsExport", "Y")
            prms(5) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            '  prms(5) = New 
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetClubWiseDetail", prms)
            Session("AchieverList1") = Ds.Tables(0)
            btnPrintAll.Enabled = True
            btnPrintCurrent.Enabled = True
            ExportExcel()

        Catch ex As Exception

        End Try
    End Sub


    Public Sub BindSession()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetDailySession")
            ddlSession.DataSource = Ds.Tables(0)
            ddlSession.DataValueField = "SessID"
            ddlSession.DataTextField = "Sessnname"
            ddlSession.DataBind()
        Catch ex As Exception
        End Try
        'Dim sql As String = "Select * From(Select 0 As SessID,'-- ALL --' As SessnName Union ALL select " & _
        '                    " SessID, ISNULL(Replace(Convert(varchar,ToDate,106),' ','-'),'')  As SessnName" & _
        '                  " from D_SessnMaster Where EndTime Is Not Null) As Temp order by SessId"
        'objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
    End Sub
    Public Sub BindClub()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetClubDetails")
            DDlLevel.DataSource = Ds.Tables(0)
            DDlLevel.DataValueField = "ClubID"
            DDlLevel.DataTextField = "Club"
            DDlLevel.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("AchieverList")
        GvData.DataBind()
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
    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("AchieverList1")
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
        dtData = Session("AchieverList")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("AchieverList1")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        GvData.BorderStyle = BorderStyle.Solid
        ' gridview.BorderWidth = 
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
        dtData = Session("AchieverList")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub BindData(ByVal PageIndex As Integer)
        Try
            Dim sessid As String = "0"
            If ddlSession.SelectedValue = "0" Then
                sessid = "0"
            Else
                sessid = ddlSession.SelectedValue
            End If
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(5) {}
            prms(0) = New SqlParameter("@SessId", Convert.ToString(sessid).ToLower())
            prms(1) = New SqlParameter("@ClubId", Convert.ToString(DDlLevel.SelectedValue))
            prms(2) = New SqlParameter("@PageIndex", PageIndex)
            prms(3) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(4) = New SqlParameter("@IsExport", "N")
            prms(5) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            '  prms(5) = New 
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetClubWiseDetail", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("AchieverList") = Ds.Tables(0)
            ViewState("WithDrawDate") = "Date"
            ViewState("Sort_Order") = "ASC"
            GvData.Visible = True
            gvContainer.Visible = True
            If Ds.Tables(0).Rows.Count > 0 Then
                lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
                btnExport.Enabled = True
                ' btnPrintAll.Enabled = True
                '  btnPrintCurrent.Enabled = True
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                    Dim img As New Image()
                    img.ImageUrl = "~/Images/Uparrow.png"
                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                    tableCell.Controls.Add(img)
                Next
                btnExport.Enabled = True



            Else
                lblCount.Text = "No Record Found!!"
                btnExport.Enabled = False
                btnPrintAll.Enabled = False
                btnPrintCurrent.Enabled = False

            End If
            Me.PopulatePager(recordCount, PageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try


    End Sub
   
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindData(1)
        'Try
        '    Dim sql As String = ""
        '    Dim dt As DataTable
        '    '  Dim obj As DAL
        '    Dim condition As String = ""

        '    dt = New DataTable
        '    objDAL = New DAL
        '    If ddlSession.SelectedValue <> "0" Then
        '        condition = "and a.Sessid='" & ddlSession.SelectedValue & "'"
        '    End If
        '    If DDlLevel.SelectedValue = "1" Then
        '        sql = " select Replace(Convert(Varchar,b.Todate,106),' ','-') as Date,Level1Fund,Level1Ach,Level1Rate,Level2Fund,Level2Ach,Level2Rate,Level3Fund,Level3Ach,Level3Rate,Level4Fund,Level4Ach,Level4Rate,Level5Fund,Level5Ach,Level5Rate,TotalFund,Distribution,RemainFund,b.TotalActive from ExecutiveClubStatus as a,D_Sessnmaster as b where a.Sessid=b.Sessid   " & condition & " Order by a.Sessid Desc "
        '    ElseIf DDlLevel.SelectedValue = "2" Then
        '        sql = " select Replace(Convert(Varchar,b.Todate,106),' ','-') as Date,Level1Fund,Level1Ach,Level1Rate,Level2Fund,Level2Ach,Level2Rate,Level3Fund,Level3Ach,Level3Rate,Level4Fund,Level4Ach,Level4Rate,Level5Fund,Level5Ach,Level5Rate,TotalFund,Distribution,RemainFund,b.TotalActive from DiamondClubStatus as a,D_Sessnmaster as b where a.Sessid=b.Sessid  " & condition & "Order by a.Sessid Desc "
        '    ElseIf DDlLevel.SelectedValue = "3" Then
        '        sql = " select Replace(Convert(Varchar,b.Todate,106),' ','-') as Date,Level1Fund,Level1Ach,Level1Rate,Level2Fund,Level2Ach,Level2Rate,Level3Fund,Level3Ach,Level3Rate,Level4Fund,Level4Ach,Level4Rate,Level5Fund,Level5Ach,Level5Rate,TotalFund,Distribution,RemainFund,b.TotalActive from  BlueDiamondClubStatus as a,D_Sessnmaster as b where a.Sessid=b.Sessid " & condition & "Order by a.Sessid Desc "
        '    ElseIf DDlLevel.SelectedValue = "4" Then
        '        sql = " select Replace(Convert(Varchar,b.Todate,106),' ','-') as Date,Level1Fund,Level1Ach,Level1Rate,Level2Fund,Level2Ach,Level2Rate,Level3Fund,Level3Ach,Level3Rate,Level4Fund,Level4Ach,Level4Rate,Level5Fund,Level5Ach,Level5Rate,TotalFund,Distribution,RemainFund,b.TotalActive from RoyalClubStatus as a,D_Sessnmaster as b where a.Sessid=b.Sessid  " & condition & "Order by a.Sessid Desc "
        '    End If
        '    dt = objDAL.GetData(sql)
        '    If dt.Rows.Count > 0 Then
        '        Session("AchieverList") = dt
        '        GvData.DataSource = dt
        '        GvData.DataBind()
        '        ViewState("WithDrawDate") = "Date"
        '        ViewState("Sort_Order") = "ASC"
        '        lblCount.ForeColor = Drawing.Color.Black
        '        lblCount.Text = "Total Records" & dt.Rows.Count
        '        GvData.Visible = True
        '        gvContainer.Visible = True
        '        btnExport.Enabled = True
        '        btnPrintAll.Enabled = True
        '        btnPrintCurrent.Enabled = True
        '    Else
        '        GvData.Visible = False
        '        gvContainer.Visible = False
        '        lblCount.ForeColor = Drawing.Color.Red
        '        lblCount.Text = "No Record Found"

        '        btnExport.Enabled = False
        '        btnPrintAll.Enabled = False
        '        btnPrintCurrent.Enabled = False
        '    End If


        'Catch ex As Exception
        'End Try
    End Sub
    Private Sub PopulatePager(ByVal recordCount As Integer, ByVal currentPage As Integer)
        Dim pages As New List(Of ListItem)()
        Dim startIndex As Integer, endIndex As Integer
        Dim pagerSpan As Integer = 5

        'Calculate the Start and End Index of pages to be displayed.
        Dim dblPageCount As Double = CDbl(CDec(recordCount) / Convert.ToDecimal(ddlPageSize.SelectedValue))
        Dim pageCount As Integer = CInt(Math.Ceiling(dblPageCount))
        startIndex = If(currentPage > 1 AndAlso currentPage + pagerSpan - 1 < pagerSpan, currentPage, 1)
        endIndex = If(pageCount > pagerSpan, pagerSpan, pageCount)
        If currentPage > pagerSpan Mod 2 Then
            If currentPage = 2 Then
                endIndex = 5
            Else
                endIndex = currentPage + 2
            End If
        Else
            endIndex = (pagerSpan - currentPage) + 1
        End If

        If endIndex - (pagerSpan - 1) > startIndex Then
            startIndex = endIndex - (pagerSpan - 1)
        End If

        If endIndex > pageCount Then
            endIndex = pageCount
            startIndex = If(((endIndex - pagerSpan) + 1) > 0, (endIndex - pagerSpan) + 1, 1)
        End If

        'Add the First Page Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("First", "1"))
        End If

        'Add the Previous Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("<<", (currentPage - 1).ToString()))
        End If

        For i As Integer = startIndex To endIndex
            pages.Add(New ListItem(i.ToString(), i.ToString(), i <> currentPage))
        Next

        'Add the Next Button.
        If currentPage < pageCount Then
            pages.Add(New ListItem(">>", (currentPage + 1).ToString()))
        End If

        'Add the Last Button.
        If currentPage <> pageCount Then
            pages.Add(New ListItem("Last", pageCount.ToString()))
        End If
        rptPager.DataSource = pages
        rptPager.DataBind()
    End Sub
    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.BindData(pageIndex)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Me.BindData(1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            ' If e.SortExpression = ViewState("PayoutDate").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next

            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)

                    End If
                Next
            End If
            'Else
            'RebindData(e.SortExpression, "ASC")
            'End If


        Catch ex As Exception

        End Try
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("AchieverList"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("WithDrawDate") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub

    Private Sub ExportExcel()
        Dim dt As DataTable = Session("AchieverList1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Level")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=LevelReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


End Class
