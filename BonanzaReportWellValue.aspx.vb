Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class BonanzaReportWellValue
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim Dt As New DataTable
    Dim objDAL As DAL
    'Dim objModuleFun As New ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim str As String = ""
    Dim Condition As String = ""
    Dim Condition2 As String = ""
    Dim Ds As DataSet
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Report / Daily Incentive Report"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                If Session("compid") = "1010" Or Session("compid") = "1091"  Then
                    Div3.Visible = True
                Else
                    Div3.Visible = False
                End If
                txtMemId.Text = ""
                GrdTotal.Visible = False
                FillBonanza()
                If Session("AStatus") = "OK" Then
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        BindDataWellValue(1)
    End Sub
    Private Sub FillBonanza()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetBonanza")
            CmbKit.DataSource = Ds.Tables(0)
            CmbKit.DataValueField = "offerid"
            CmbKit.DataTextField = "offername"
            CmbKit.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    Public Sub BindDataWellValue(ByVal PageIndex As Integer)
        lblError.Text = ""
        Try
            Dim FromSessid As String = "0"
            Dim ToSessid As String = "0"
            Dim Idno As String = "0"

            Dim cmdkit As String = ""
            Dim status As String = ""
            If txtMemId.Text <> "" Then
                Idno = txtMemId.Text
            Else
                Idno = ""
            End If
            If CmbKit.SelectedItem.Text <> "" Then
                cmdkit = CmbKit.SelectedValue
            End If
            GrdTotal.DataSource = Nothing
            GrdTotal.DataBind()
            GridView1.DataSource = Nothing
            GridView1.DataBind()
            GridView2.DataSource = Nothing
            GridView2.DataBind()
            GridView3.DataSource = Nothing
            GridView3.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@Bonanza", Integer.Parse(CmbKit.SelectedValue))
            prms(2) = New SqlParameter("@PaidStatus", Convert.ToString(DDlPaidStatus.SelectedValue))
            prms(3) = New SqlParameter("@PageIndex", PageIndex)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "N")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_BonanzaReportUpdate1", prms)
            Dim Dt As DataTable = New DataTable()
            Dt = Ds.Tables(0)
            Dim OfferType As String
            If Dt.Rows.Count > 0 Then
                OfferType = Dt.Rows(0)("Offertype").ToString()
                If OfferType = "T" Then
                    If Session("compid") = "1103" Or Session("compid") = "1108" Then
                        GrdTotal.Visible = False
                        GridView1.Visible = False
                        GridView3.Visible = False
                        GridView2.Visible = True
                        GridView2.DataSource = Dt
                        GridView2.DataBind()
                    ElseIf Session("compid") = "1091" Then
                        GrdTotal.Visible = False
                        GridView3.Visible = True
                        GridView1.Visible = False
                        GridView2.Visible = False
                        GridView3.DataSource = Dt
                        GridView3.DataBind()
                    Else
                        GrdTotal.Visible = True
                        GridView3.Visible = False
                        GridView1.Visible = False
                        GridView2.Visible = False
                        GrdTotal.DataSource = Dt
                        GrdTotal.DataBind()
                    End If
                Else
                    GrdTotal.Visible = False
                    GridView3.Visible = False
                    GridView2.Visible = False
                    GridView1.DataSource = Dt
                    GridView1.DataBind()
                End If
            End If
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
            Session("GData1") = Ds.Tables(0)
            ViewState("IdNo") = "IdNo"
            ViewState("Sort_Order") = "ASC"
            If Ds.Tables(0).Rows.Count > 0 Then

                If OfferType = "T" Then
                    If Session("compid") = "1103" Or Session("compid") = "1108" Then
                        GridView1.Visible = False
                        GridView3.Visible = False
                        GrdTotal.Visible = False
                        GridView2.Visible = True
                    ElseIf Session("compid") = "1091" Then
                        GridView1.Visible = False
                        GridView3.Visible = True
                        GrdTotal.Visible = False
                        GridView2.Visible = False
                    Else
                        GridView1.Visible = False
                        GrdTotal.Visible = True
                        GridView3.Visible = False
                        GridView2.Visible = False
                    End If
                Else
                    GrdTotal.Visible = False
                    GridView3.Visible = False
                    GridView1.Visible = True
                    GridView2.Visible = False
                End If
            Else
                lblError.Text = "No Record Found!!"
                If OfferType = "T" Then
                    GridView2.Visible = False
                    GrdTotal.Visible = False
                    GridView1.Visible = False
                    GridView3.Visible = False
                Else
                    GridView2.Visible = False
                    GrdTotal.Visible = False
                    GridView1.Visible = False
                    GridView3.Visible = False
                End If
            End If
            Me.PopulatePager(recordCount, PageIndex)
            ' End If
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try
    End Sub
    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData1(e.SortExpression, "DESC")
                For i As Integer = 0 To GridView1.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView1.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData1(e.SortExpression, "ASC")
                For i As Integer = 0 To GridView1.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView1.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub RebindData1(ByVal sColimnName As String, ByVal sSortOrder As String)
        Try
            Dim dt As DataTable = CType(Session("GData"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GridView1.DataSource = dt
            GridView1.DataBind()
            ViewState("IdNo") = sColimnName
            ViewState("Sort_Order") = sSortOrder
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = Session("GData")
        GridView1.DataBind()
    End Sub
    Protected Sub GridView2_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData2(e.SortExpression, "DESC")
                For i As Integer = 0 To GridView2.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView2.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData2(e.SortExpression, "ASC")
                For i As Integer = 0 To GridView2.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView2.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub RebindData2(ByVal sColimnName As String, ByVal sSortOrder As String)
        Try
            Dim dt As DataTable = CType(Session("GData"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GridView2.DataSource = dt
            GridView2.DataBind()
            ViewState("IdNo") = sColimnName
            ViewState("Sort_Order") = sSortOrder
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub GridView2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView2.PageIndexChanging
        GridView2.PageIndex = e.NewPageIndex
        GridView2.DataSource = Session("GData")
        GridView2.DataBind()
    End Sub

    Protected Sub GridView3_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData3(e.SortExpression, "DESC")
                For i As Integer = 0 To GridView3.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView3.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData3(e.SortExpression, "ASC")
                For i As Integer = 0 To GridView3.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView3.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub RebindData3(ByVal sColimnName As String, ByVal sSortOrder As String)
        Try
            Dim dt As DataTable = CType(Session("GData"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GridView3.DataSource = dt
            GridView3.DataBind()
            ViewState("IdNo") = sColimnName
            ViewState("Sort_Order") = sSortOrder
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub GridView3_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView3.PageIndexChanging
        GridView3.PageIndex = e.NewPageIndex
        GridView3.DataSource = Session("GData")
        GridView3.DataBind()
    End Sub

    Protected Sub GrdTotal_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GrdTotal.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GrdTotal.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GrdTotal.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GrdTotal.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)

                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Try
            Dim dt As DataTable = CType(Session("GData"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GrdTotal.DataSource = dt
            GrdTotal.DataBind()
            ViewState("IdNo") = sColimnName
            ViewState("Sort_Order") = sSortOrder
        Catch ex As Exception
        End Try
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
            Me.BindDataWellValue(pageIndex)
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub GrdTotal_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdTotal.PageIndexChanging
        GrdTotal.PageIndex = e.NewPageIndex
        GrdTotal.DataSource = Session("GData")
        GrdTotal.DataBind()
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.BindDataWellValue(1)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "BonanzaReport")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=BonanzaReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim Idno As String = ""
            If txtMemId.Text.Trim <> "" Then
                Idno = txtMemId.Text
            Else
                Idno = ""
            End If
            GrdTotal.DataSource = Nothing
            GrdTotal.DataBind()
            GridView1.DataSource = Nothing
            GridView1.DataBind()
            GridView2.DataSource = Nothing
            GridView2.DataBind()
            GridView3.DataSource = Nothing
            GridView3.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@Bonanza", Integer.Parse(CmbKit.SelectedValue))
            prms(2) = New SqlParameter("@PaidStatus", Convert.ToString(DDlPaidStatus.SelectedValue))
            prms(3) = New SqlParameter("@PageIndex", 1)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "Y")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_BonanzaReportUpdate1", prms)
            Session("GData1") = Ds.Tables(0)
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
    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated
        If Session("compid") = "1103" Then
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(5).Text = "Required Direct"
                e.Row.Cells(6).Text = "Achieve Direct"
            End If
        Else
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(5).Text = "Required Direct BV"
                e.Row.Cells(6).Text = "Achieve BV"
            End If
        End If
  
    End Sub
End Class
