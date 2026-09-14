Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_RptCancelPins
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Epin Report / Cancel EPin Detail"
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
                Session("CancelPins") = Nothing
                FillKit()
                Filldate()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Filldate()
        Try
            objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = objDAL.GetData(Str)
            If dtData.Rows.Count > 0 Then
                txtStartDate.Text = dtData.Rows(0)("CurrentDate")
                txtEndDate.Text = dtData.Rows(0)("CurrentDate")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Fillkit()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetKitMaster")
            CmbKit.DataSource = Ds.Tables(0)
            CmbKit.DataValueField = "KitID"
            CmbKit.DataTextField = "Kitname"
            CmbKit.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Public Sub BindData(ByVal PageIndex As Integer)
        Try

            lblErr.Text = ""
            lblCount.Text = ""
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim Kitid As String = "0"
            If ChkKit.Checked Then
                Kitid = CmbKit.SelectedValue
            Else
                Kitid = "0"

            End If
            Dim startDate As Date
            Dim endDate As Date
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
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@KitId", Convert.ToString(Kitid).ToLower())
            prms(1) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(2) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(3) = New SqlParameter("@PageIndex", PageIndex)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "N")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetCancelEpinValueDetail", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("CancelPinValue") = Ds.Tables(0)
            ViewState("Formno") = "Formno"
            ViewState("Sort_Order") = "ASC"

            If Ds.Tables(1).Rows(0)("RecordCount") > 0 Then
                'btnExport.Enabled = True
                'btnPrintAll.Enabled = True
                'btnPrintCurrent.Enabled = True
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
        Catch ex As Exception

        End Try


        'Dim Sql As String = "Select * from V#CancelInformation Where 1=1 " & Condition_ & " Order by Cast(Convert(varchar,CancelDate,106) as DateTime) desc"
        'dtData = New DataTable
        'dtData = objDAL.GetData(Sql)
        'GvData.DataSource = dtData
        'GvData.DataBind()
        'Session("CancelPins") = dtData
        'GvData.Visible = True
        'gvContainer.Visible = True
        'If dtData.Rows.Count > 0 Then
        '    lblCount.Text = "Total Record: " & dtData.Rows.Count
        '    btnExport.Enabled = True
        '    btnPrintAll.Enabled = True
        '    btnPrintCurrent.Enabled = True
        'Else
        '    lblErr.Text = "No Record Found!!"
        '    btnExport.Enabled = False
        '    btnPrintAll.Enabled = False
        '    btnPrintCurrent.Enabled = False
        'End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("CancelPinValue")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim Kitid As String = "0"
            If ChkKit.Checked Then
                Kitid = CmbKit.SelectedValue
            Else
                Kitid = "0"

            End If
            Dim startDate As Date
            Dim endDate As Date
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
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@KitId", Convert.ToString(Kitid).ToLower())
            prms(1) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(2) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(3) = New SqlParameter("@PageIndex", 1)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "Y")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetCancelEpinValueDetail", prms)
            Session("CancelPinValue1") = Ds.Tables(0)
            ExportExcel()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("CancelPinValue1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "CancelEpinValue")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=CancelEpinValueReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.BindData(1)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try


            ' If e.SortExpression = ViewState("FromIdno").ToString() Then
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
        Try
            Dim dt As DataTable = CType(Session("CancelPinValue"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()
            ViewState("Formno") = sColimnName
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
            Me.BindData(pageIndex)
        Catch ex As Exception

        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            lblErr.Text = ""
            lblCount.Text = ""
            'Dim subcon As String = ""
            'Dim Condition As String = ""
            'If txtStartDate.Text <> "" Then
            '    Condition = Condition & " And Cast(Convert(varchar,CancelDate,106) as DateTime)>='" & txtStartDate.Text & "'"
            'End If
            'If txtEndDate.Text <> "" Then
            '    Condition = Condition & " And Cast(Convert(varchar,CancelDate,106) as DateTime)<='" & txtEndDate.Text & "'"
            'End If
            'If ChkKit.Checked = True Then
            '    Condition = Condition & " And KitID='" & CmbKit.SelectedValue & "'"
            'End If



            BindData(1)
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        Try

            GvData.AllowPaging = False
            GvData.GridLines = GridLines.Both
            dtData = New DataTable
            dtData = Session("CancelPinValue")
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
            dtData = Session("CancelPins")
            GvData.DataSource = dtData
            GvData.DataBind()
        Catch ex As Exception
        End Try


    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        Try
            GvData.AllowPaging = False
            GvData.GridLines = GridLines.Both
            dtData = New DataTable
            dtData = Session("CancelPinValue")
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
            dtData = Session("CancelPins")
            GvData.DataSource = dtData
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub CancelIssue(ByVal sender As Object, ByVal e As System.EventArgs)
        Try


            Dim GrpID, scrname As String
            Dim GVRw As GridViewRow
            'Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            'Conn.Open()
            GVRw = CType(sender.Parent.Parent, GridViewRow)
            GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
            If DirectCast(GVRw.FindControl("LblIsCancel"), Label).Text = "Y" Then
                Dim Sql As String = "Exec Sp_RollbackIssueEpins " & GrpID.ToString
                Dim updateEffect As Integer = objDAL.UpdateData(Sql)
                If updateEffect <> 0 Then
                    scrname = "<SCRIPT language='javascript'>alert('Pin Deleted Successfully!');" & "</SCRIPT>"
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected pin! ');" & "</SCRIPT>"
                End If
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "E-Pin Deletion", scrname, False)
                BindData(1)
            End If
        Catch ex As Exception

        End Try
    End Sub


End Class
