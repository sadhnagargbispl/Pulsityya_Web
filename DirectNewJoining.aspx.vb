Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class DirectNewJoining
    Inherits System.Web.UI.Page

    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = " Epin Report / Epin Issued Value Report"
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
                GvData.Visible = False
                gvContainer.Visible = False
                Session("IssuedPinValue") = Nothing
                Filldate()
                '   FillFranchisee()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Filldate()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as Date,Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = objDAL.GetData(Str)
            If dtData.Rows.Count > 0 Then
                txtStartDate.Text = dtData.Rows(0)("CurrentDate")
                txtEndDate.Text = dtData.Rows(0)("CurrentDate")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Function GetFormNo() As String
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim idNo As String
            Dim formno As String
            idNo = txtAmount.Text
            idNo = idNo.Trim
            Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
            Dim dt As New DataTable
            dt = objDAL.GetData(qry)
            If (dt.Rows.Count > 0) Then
                formno = dt.Rows(0)("FormNo")
            Else
                lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
                lblErr.Visible = True
                txtAmount.Text = ""
            End If
            Return formno

        Catch ex As Exception

        End Try

    End Function

    Public Sub BindData(ByVal PageIndex As Integer)
        Try
            lblErr.Text = ""
            lblCount.Text = ""
            Dim userid As String = "0"
            Dim Amount As String = "0"
            Dim startDate As Date
            Dim endDate As Date

            'If userid = DDlFranchisee.SelectedItem.Text Then
            'Else
            If txtAmount.Text <> "" Then
                Amount = txtAmount.Text.Trim
            Else
                Amount = "0"

            End If


            ' End If

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
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@Amount", Convert.ToString(Amount).ToLower())
            prms(1) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(2) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(3) = New SqlParameter("@PageIndex", PageIndex)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "N")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)

            'prms(7) = New SqlParameter("@Type", Convert.ToString(CmbKit.SelectedValue))
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "Sp_DirectNewJoining", prms)
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
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("IssuedPinValue")
        GvData.DataBind()
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.BindData(1)
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim userid As String = "0"
            Dim startDate As Date
            Dim endDate As Date
            Dim Amount As String = "0"
            If txtAmount.Text <> "" Then
                Amount = txtAmount.Text.Trim
            Else
                Amount = "0"

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
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@Amount", Convert.ToString(Amount).ToLower())
            prms(1) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(2) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(3) = New SqlParameter("@PageIndex", 1)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "Y")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)

            'prms(7) = New SqlParameter("@Type", Convert.ToString(CmbKit.SelectedValue))
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "Sp_DirectNewJoining", prms)
            Session("IssuedPinValue1") = Ds.Tables(0)
            ExportExcel()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("IssuedPinValue1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "DirectNewJoiningReport")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=DirectNewJoiningReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

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
            Dim dt As DataTable = CType(Session("IssuedPinValue"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()
            ViewState("IssueDate") = sColimnName
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

    'Private Sub ExportIssuePin()
    '    Try
    '        Dim dtTemp As New DataTable



    '        Dim dg As New DataGrid
    '        Dim Condition As String = ""

    '        If txtStartDate.Text <> "" Then
    '            Condition = Condition & " And Cast(Convert(varchar,IssueDate,106) as DateTime)>='" & txtStartDate.Text & "'"
    '        End If
    '        If txtEndDate.Text <> "" Then
    '            Condition = Condition & " And Cast(Convert(varchar,IssueDate,106) as DateTime)<='" & txtEndDate.Text & "'"
    '        End If
    '        If ChkKit.Checked Then
    '            Condition = Condition & " and Issueduserid='" & CmbKit.SelectedValue & "'"
    '        End If

    '        Dim Sql As String = " select IssueDate,UserName,Sum(DepositAmount) as EpinValue from  (select a.IssuedUserId,IsNull(b.UserName,' ') as UserName," & _
    '" Count(ProdId) as Qty,REPLACE(CONVERT(VARCHAR,CAST(a.IssuedDate AS DATE), 106), ' ', '-')as Issuedate,Prodid ," & _
    '" 0 as WalletAmount,(Count(Prodid)* k.KitAmount) as DepositAmount,(Count(Prodid)* k.KitAmount) as TotalAmount  from M_FormGeneration as a Left Join M_UserMaster as b " & _
    '" On a.IssuedUserId=b.UserId and b.RowStatus='Y',M_KitMaster as k where a.ProdId=k.KitId and k.RowStatus='Y'  and  IssuedIdno Is Not Null " & _
    '" Group By CAST(a.IssuedDate AS DATE),a.IssuedUserId,UserName,Prodid ,KitName,KitAmount " & _
    '  ") as temp where 1=1" & Condition & "  Group by UserName,Issuedate  Order by Year(IssueDate) Desc,Month(Issuedate) Desc,Day(IssueDate) Desc "
    '        '      " Union all" & _
    '        '     "  select a.UserId,Isnull(b.UserName,' ')as UserName,DispQty as Qty,REPLACE(CONVERT(VARCHAR,CAST(a.IssuedDate AS DATE), 106), ' ', '-')as Issuedate, " & _
    '        ' "Prodid ,k.KitName,k.KitAmount,p.WalletAmt,(p.BankAmt+p.OtherAmt) as DepositAmount,(DispQty*k.KitAmount) as TotalAmount from M_FormGeneration as a Left Join M_UserMaster as b On a.UserId=b.UserId" & _
    '        '" and b.RowStatus='Y',M_KitMaster as k ,TrnPinReqMain as p,TrnPinRequest as r where a.ProdId=k.KitId " & _
    '        '" and k.RowStatus='Y' and a.ReqNo=p.ReqNo and p.ReqNo=r.ReqNo and r.Kitid=k.KitId and k.RowStatus='Y' and " & _
    '        ' " r.Idno=a.IssuedIdno and r.dispqty<>0 and p.SentQty<>0   Group By CAST(a.IssuedDate AS DATE),a.UserId,UserName,Prodid ,k.KitName,WalletAmt,BankAmt,OtherAmt,dispQty,k.KitAmount" & _
    '        '") as temp where 1=1" & Condition & " Order by IssueDate"
    '        dtTemp = New DataTable
    '        dtTemp = objDAL.GetData(Sql)

    '        dg.DataSource = dtTemp
    '        dg.DataBind()

    '        ExportToExcel("IssuedEPinValue.xls", dg)

    '    Catch ex As Exception
    '        Response.Write(ex.Message & "Error In Exporting File")
    '    End Try
    '    'Comm.Cancel()
    '    'ds.Dispose()

    'End Sub
    'Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
    '    Dim sw As New System.IO.StringWriter
    '    Dim htw As System.Web.UI.HtmlTextWriter
    '    Response.Clear()
    '    Response.Buffer = True
    '    Response.ContentType = "application/vnd.xls"
    '    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
    '    Response.Charset = ""
    '    dg.EnableViewState = False
    '    htw = New HtmlTextWriter(sw)
    '    dg.RenderControl(htw)
    '    Response.Write(sw.ToString())
    '    Response.End()
    'End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'lblErr.Text = ""
        'lblCount.Text = ""
        'Dim Condition As String = ""

        'If txtStartDate.Text <> "" Then
        '    Condition = Condition & " And Cast(Convert(varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
        'End If
        'If txtEndDate.Text <> "" Then
        '    Condition = Condition & " And Cast(Convert(varchar,a.RectimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
        'End If

        Try

            BindData(1)
        Catch ex As Exception

        End Try
    End Sub

    'Protected Sub ddlSearchFields_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSearchFields.SelectedIndexChanged
    '    If String.Equals(ddlSearchFields.SelectedValue.ToLower(), "dateofjoining") = True Then
    '        lblStart.Text = "Enter Start Date : "
    '        lblStart.Visible = True
    '        lblEnd.Visible = True
    '        txtStart.Visible = True
    '        txtEnd.Visible = True
    '    Else
    '        lblStart.Text = "Enter value : "
    '        lblStart.Visible = False
    '        txtStart.Visible = True
    '    End If
    'End Sub

    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        Try
            GvData.AllowPaging = False
            GvData.GridLines = GridLines.Both
            dtData = New DataTable
            dtData = Session("IssuedPinValue")
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
            dtData = Session("IssuedPinValue")
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
            dtData = Session("IssuedPinValue")
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
            dtData = Session("IssuedPinValue")
            GvData.DataSource = dtData
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub






    'Protected Sub CmbKit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbKit.SelectedIndexChanged

    '    If DDlFranchisee.Visible = True Then
    '        txtMemberId.Visible = False
    '    Else
    '        DDlFranchisee.Visible = False
    '        txtMemberId.Visible = True
    '    End If
    'End Sub


End Class


