Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class SearchRepurchase
    Inherits System.Web.UI.Page

    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Session("AStatus") = "OK" Then
            Session("PageName") = " Epin Report / Epin Issued Value Report"
        Else
            Response.Redirect("Default.aspx")
        End If

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            GvData.Visible = False
            gvContainer.Visible = False
            Session("IssuedPinValue") = Nothing
            'Filldate()

        End If

    End Sub


    Public Sub BindData(ByVal PageIndex As Integer)

        lblErr.Text = ""
        lblCount.Text = ""
        Dim IDNo As String = "0"
        Dim Userformnno As String = "0"
        Dim startDate As Date
        Dim endDate As Date

        If txtIDNo.Text <> "" Then
            IDNo = txtIDNo.Text.Trim
        Else
            IDNo = "0"

        End If

        If TextUserformnno.Text <> "" Then
            Userformnno = TextUserformnno.Text.Trim
        Else
            Userformnno = "0"

        End If


        If txtStartDate.Text = "" Then
            startDate = Session("CompDate")
        Else
            startDate = txtStartDate.Text
        End If
        If txtEndDate.Text = "" Then
            endDate = Format(Date.Now, "dd-MMM-yyyy")
        Else
            endDate = txtEndDate.Text
        End If
        GvData.DataSource = Nothing

        GvData.DataBind()
        Dim prms As SqlParameter() = New SqlParameter(7) {}
        prms(0) = New SqlParameter("@IDNo", Convert.ToString(IDNo).ToLower())
        prms(1) = New SqlParameter("@formdate", Convert.ToDateTime(startDate))
        prms(2) = New SqlParameter("@Todate", Convert.ToDateTime(endDate))
        prms(3) = New SqlParameter("@PageIndex", PageIndex)
        prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
        prms(5) = New SqlParameter("@IsExport", "N")
        prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
        prms(7) = New SqlParameter("@Userformnno", Userformnno)

        Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetSearchRepurchase", prms)
        GvData.DataSource = Ds.Tables(0)
        GvData.DataBind()
        Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
        Session("IssuedPinValue") = Ds.Tables(0)
        ViewState("IssueDate") = "IssueDate"
        ViewState("Sort_Order") = "ASC"

        If Ds.Tables(1).Rows(0)("RecordCount") > 0 Then
            For i As Integer = 0 To GvData.Columns.Count - 1
                Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                Dim img As New Image()
                img.ImageUrl = "~/Images/Uparrow.png"
                tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                tableCell.Controls.Add(img)
            Next

            GvData.Visible = True
            gvContainer.Visible = True
            lblCount.Text = "Total : " & recordCount
        Else

            GvData.Visible = False
            gvContainer.Visible = False
            lblErr.Text = "No Record Found!!"
        End If
        Me.PopulatePager(recordCount, PageIndex)

    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("IssuedPinValue")
        GvData.DataBind()
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)

        Me.BindData(1)

    End Sub



    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim userid As String = "0"
        Dim startDate As Date
        Dim endDate As Date
        Dim IDNo As String = "0"
        Dim Userformnno As String = "0"
        If txtIDNo.Text <> "" Then
            IDNo = txtIDNo.Text.Trim
        Else
            IDNo = "0"

        End If
        If TextUserformnno.Text <> "" Then
            Userformnno = TextUserformnno.Text.Trim
        Else
            Userformnno = "0"

        End If
        If txtStartDate.Text = "" Then
            startDate = Session("CompDate")
        Else
            startDate = txtStartDate.Text
        End If
        If txtEndDate.Text = "" Then
            endDate = Format(Date.Now, "dd-MMM-yyyy")
        Else
            endDate = txtEndDate.Text
        End If
        GvData.DataSource = Nothing
        GvData.DataBind()


        Dim prms As SqlParameter() = New SqlParameter(7) {}
        prms(0) = New SqlParameter("@IDNo", Convert.ToString(IDNo).ToLower())
        prms(1) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
        prms(2) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
        prms(3) = New SqlParameter("@PageIndex", 1)
        prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
        prms(5) = New SqlParameter("@IsExport", "Y")
        prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
        prms(7) = New SqlParameter("@Userformnno", Userformnno)

        Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetSearchRepurchase", prms)
        Session("IssuedPinValue1") = Ds.Tables(0)
        ExportExcel()

    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("IssuedPinValue1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "RepurchaseBusiness")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=RepurchaseBusiness.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub

    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

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


    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)

        Dim dt As DataTable = CType(Session("IssuedPinValue"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()
        ViewState("IssueDate") = sColimnName
        ViewState("Sort_Order") = sSortOrder

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

        Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
        Me.BindData(pageIndex)

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        BindData(1)

    End Sub
End Class