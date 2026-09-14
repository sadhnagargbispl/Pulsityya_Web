Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class LevelChangeReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = " Member / Level Change Report"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                GvData.Visible = False
                gvContainer.Visible = False
                Session("LevelChange") = Nothing
                BindClub()

            End If
        Catch ex As Exception

        End Try
    End Sub
    'Private Sub FillLevel()
    '    Dim sql As String = "Select Club,ClubID from M_ClubMaster where ActiveStatus='Y'"

    '    dtData = New DataTable
    '    dtData = objDAL.GetData(sql)
    '    If dtData.Rows.Count > 0 Then
    '        DDLLevel.DataSource = dtData
    '        DDLLevel.DataTextField = "Club"
    '        DDLLevel.DataValueField = "ClubID"
    '        DDLLevel.DataBind()
    '    End If
    'End Sub
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

    Public Sub BindData(ByVal PageIndex As Integer)
        LblError.Text = ""
        lblCount.Text = ""
        Dim Recordcount As Integer = 0

        'Dim Sql As String = ""
        'If Val(TxtFromAmount.Text) <> 0 And Val(TxtToAmount.Text) <> 0 Then
        '    Condition = "Having Sum(Comm)>='" & Val(TxtFromAmount.Text) & "' and Sum(Comm)<='" & Val(TxtToAmount.Text) & "'"
        'Else

        '    If Val(TxtFromAmount.Text) <> 0 Then
        '        Condition = "Having Sum(Comm)>='" & Val(TxtFromAmount.Text) & "'"
        '    End If
        '    If Val(TxtToAmount.Text) <> 0 Then
        '        Condition = " Having Sum(Comm)<='" & Val(TxtToAmount.Text) & "'"
        '    End If
        'End If
        'If DDLLevel.SelectedValue = "1" Then
        '    Sql = " select a.Idno,(a.MemFirstName+' '+a.MemLastName)As MemberName,a.Mobl as Mobileno," & _
        '          "  b.TotalCommission from M_MemberMaster as a,(select Formno,Sum(Comm)As TotalCommission " & _
        '          " from M_ExecutiveClub Group By Formno " & Condition & ")As B where a.Formno=b.Formno Order by a.Idno  "
        'ElseIf DDLLevel.SelectedValue = "2" Then
        '    Sql = " select a.Idno,(a.MemFirstName+' '+a.MemLastName)As MemberName,a.Mobl as Mobileno," & _
        '          "  b.TotalCommission from M_MemberMaster as a,(select Formno,Sum(Comm)As TotalCommission " & _
        '          " from M_DiamondClub Group By Formno " & Condition & ")As B where a.Formno=b.Formno Order by a.Idno "
        'ElseIf DDLLevel.SelectedValue = "3" Then
        '    Sql = " select a.Idno,(a.MemFirstName+' '+a.MemLastName)As MemberName,a.Mobl as Mobileno," & _
        '          "  b.TotalCommission from M_MemberMaster as a,(select Formno,Sum(Comm)As TotalCommission " & _
        '          " from M_BlueDiamondClub Group By Formno " & Condition & ")As B where a.Formno=b.Formno Order by a.Idno "
        'ElseIf DDLLevel.SelectedValue = "4" Then
        '    Sql = " select a.Idno,(a.MemFirstName+' '+a.MemLastName)As MemberName,a.Mobl as Mobileno," & _
        '           "  b.TotalCommission from M_MemberMaster as a,(select Formno,Sum(Comm)As TotalCommission " & _
        '           " from M_RoyalClub Group By Formno " & Condition & ")As B where a.Formno=b.Formno Order by a.Idno "

        'End If


        'dtData = New DataTable
        'dtData = objDAL.GetData(Sql)
        'GvData.DataSource = dtData
        'GvData.DataBind()

        GvData.DataSource = Nothing
        GvData.DataBind()
        Dim prms As SqlParameter() = New SqlParameter(6) {}
        prms(0) = New SqlParameter("@ClubId", Convert.ToString(DDLLevel.SelectedValue))
        prms(1) = New SqlParameter("@FromAmount", Convert.ToString(TxtFromAmount.Text.Trim))
        prms(2) = New SqlParameter("@ToAmount", Convert.ToString(TxtToAmount.Text.Trim))
        prms(3) = New SqlParameter("@PageIndex", PageIndex)
        prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
        prms(5) = New SqlParameter("@IsExport", "N")
        prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
        Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_getLevelChangeDetail", prms)
        GvData.DataSource = Ds.Tables(0)
        GvData.DataBind()
        Session("LevelChange") = Ds.Tables(0)
        ViewState("Idno") = "Idno"
        ViewState("Sort_Order") = "DESC"
        GvData.Visible = True
        gvContainer.Visible = True
        Recordcount = Ds.Tables(1).Rows(0)("RecordCount")
        If Ds.Tables(0).Rows.Count > 0 Then
            lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
            btnExport.Enabled = True
            For i As Integer = 0 To GvData.Columns.Count - 1
                Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                Dim img As New Image()
                img.ImageUrl = "~/Images/Uparrow.png"
                tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                tableCell.Controls.Add(img)
            Next
        Else
            LblError.Text = "No Record Found!!"
            btnExport.Enabled = False
        End If
        Me.PopulatePager(Recordcount, PageIndex)
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
        Dim dt As DataTable = CType(Session("LevelChange"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("Idno") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub

    Private Sub ExportExcel()
        Dim dt As DataTable = Session("LevelChange1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Level")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=LevelChangeReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("LevelChange")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@ClubId", Convert.ToString(DDLLevel.SelectedValue))
            prms(1) = New SqlParameter("@FromAmount", Convert.ToString(TxtFromAmount.Text.Trim))
            prms(2) = New SqlParameter("@ToAmount", Convert.ToString(TxtToAmount.Text.Trim))
            prms(3) = New SqlParameter("@PageIndex", 1)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "Y")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_getLevelChangeDetail", prms)
            Session("LevelChange1") = Ds.Tables(0)

            ExportExcel()

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
        LblError.Text = ""
        lblCount.Text = ""
        BindData(1)
    End Sub
End Class
