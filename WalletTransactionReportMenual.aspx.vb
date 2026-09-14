Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_WalletTransactionReportMenual
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Wallet / Wallet Transaction Report"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim searchtext As String = Session("Search")
        If Not Page.IsPostBack Then
            GvData.Visible = False
            gvContainer.Visible = False
            Session("AccountData") = Nothing
            Filldate()
            'FillKit()
        End If
    End Sub
    Private Sub Filldate()
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
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
    Private Function GetFormNo() As String
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim idNo As String
            Dim formno As String
            idNo = txtMemberId.Text
            idNo = idNo.Trim
            Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
            Dim dt As New DataTable
            dt = objDAL.GetData(qry)
            If (dt.Rows.Count > 0) Then
                formno = dt.Rows(0)("FormNo")
            Else
                lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
                lblErr.Visible = True
                txtMemberId.Text = ""
            End If
            Return formno

        Catch ex As Exception

        End Try

    End Function

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("AccountData")
        GvData.DataBind()
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("AccountData1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "WalletTransaction")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=WalletTransactionReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        ' ExportIssuePin()
        Try

            Dim AcType As String = RbtWalletType.SelectedValue
            Dim Idno As String = "0"

            If ChkMember.Checked = True Then
                Idno = GetFormNo()
            Else
                Idno = "0"

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
            Dim prms As SqlParameter() = New SqlParameter(7) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@WalletType", AcType)
            prms(2) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(3) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(4) = New SqlParameter("@PageIndex", 1)
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(6) = New SqlParameter("@IsExport", "Y")
            prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetWalletTransactionDetail_Menual", prms)
            Session("AccountData1") = Ds.Tables(0)
            ExportExcel()
        Catch ex As Exception

        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblErr.Text = ""
        lblCount.Text = ""
        'Dim Condition As String = ""
        'Dim condition1 As String = ""

        'Dim Condition2 As String = ""
        'Dim condition3 As String = ""
        'Dim formno As String = ""
        'Dim qry1 As String = ""
        'If ChkMember.Checked Then
        '    If txtMemberId.Text <> "" Then
        '        formno = GetFormNo()
        '        Condition = Condition & " and temp.FormNo='" & formno & "'"
        '    End If
        'End If

        'If txtStartDate.Text <> "" Then
        '    Condition = Condition & " And Cast(Convert(varchar,Temp.RecTimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
        'End If
        'If txtEndDate.Text <> "" Then
        '    Condition = Condition & " And Cast(Convert(varchar,Temp.RecTimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
        'End If
        ''If DDlSearchBy.SelectedValue = "D" Then
        ''    condition1 = " Order By Temp.RectimeStamp Desc"
        ''Else
        ''    condition1 = " Order By Temp.RectimeStamp Asc"
        ''End If

        'If RbtWalletType.SelectedValue = "S" Then
        '    Condition2 = " and Actype='S' and VType='C'"
        '    condition3 = " and Actype='S' and VType='D'"
        'Else
        '    Condition2 = " and Actype='M' and VType='C'"
        '    condition3 = " and Actype='M' and (VType='D' Or VType='W')"

        'End If
        'Dim str As String = ""
        'Dim Dt As DataTable
        'Dt = New DataTable
        'qry1 = "select b.Idno,(b.MemFirstName+''+b.MemLastName) as [Member Name]" & _
        '     " ,Replace(Convert(varchar,Temp.RectimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(Temp.RectimeStamp AS TIME),100) as [Transaction Date],Narration as [Transaction Description],Sum(Credit) as [" & RbtWalletType.SelectedItem.Text & " Credit],sum(Debit) as [" & RbtWalletType.SelectedItem.Text & " Debit],Balance ,Temp.UserName from ( " & _
        '     " select VoucherNo,DrTo as Formno,a.RecTimeStamp,0 as Credit ,Amount as Debit,Balance,Narration,B.UserName from TrnVoucher as a Left Join M_UserMaster as b on  a.UserId=b.Userid and b.RowStatus='Y' where 1=1" & condition3 & " " & _
        '      " Union all" & _
        '    " select VoucherNo,CrTo as Formno,a.RectimeStamp,Amount as Credit ,0 as Debit,Balance,Narration,b.UserName from TrnVoucher as a Left Join M_UserMaster as b on  a.UserId=b.Userid and b.RowStatus='Y' where 1=1 " & Condition2 & " " & _
        '   ") as temp ,M_MemberMaster as b where Temp.Formno=b.Formno " & Condition & " Group By IdNo,MemFirstName,MemLastname,Temp.RectimeStamp,Narration,Balance,VoucherNo,Temp.UserName " & condition1 & " "

        'If formno <> "" Then
        '    str = "Select * From dbo.ufnGetBalance(" & Val(formno) & ",'" & RbtWalletType.SelectedValue & "')"
        '    Dt = New DataTable
        '    Dt = objDAL.GetData(str)
        '    If Dt.Rows.Count > 0 Then
        '        LblCredit.Text = " " & RbtWalletType.SelectedItem.Text & " Credit:" & Dt.Rows(0)("Credit") & "   "
        '        lblDebit.Text = " " & RbtWalletType.SelectedItem.Text & " Debit:" & Dt.Rows(0)("Debit") & "   "
        '        LblBalance.Text = " " & RbtWalletType.SelectedItem.Text & " Balance:" & Dt.Rows(0)("Balance")
        '    End If
        'Else
        '    LblCredit.Text = ""
        '    lblDebit.Text = ""
        '    LblBalance.Text = ""
        'End If
        'dtData = New DataTable
        'objDAL = New DAL
        'dtData = objDAL.GetData(qry1)
        'If dtData.Rows.Count > 0 Then
        '    GvData.DataSource = dtData
        '    GvData.DataBind()
        '    Session("AccountData") = dtData
        '    GvData.Visible = True
        '    gvContainer.Visible = True
        '    btnExport.Enabled = True
        '    ' lblCount.Text = "Total : " & dtData.Rows.Count
        'Else
        '    lblErr.Text = "No Record Found!!"
        '    btnExport.Enabled = False
        'End If
        BindData(1)

    End Sub

    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
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

        Catch ex As Exception

        End Try
    End Sub
    Private Sub PopulatePager(ByVal recordCount As Integer, ByVal currentPage As Integer)
        Try

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
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.BindData(pageIndex)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub BindData(ByVal PageIndex As Integer)
        lblErr.Text = ""
        lblCount.Text = ""
        Try
            Dim AcType As String = RbtWalletType.SelectedValue
            Dim Idno As String = "0"

            If ChkMember.Checked = True Then
                Idno = GetFormNo()
            Else
                Idno = "0"
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
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(7) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@WalletType", AcType)
            prms(2) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(3) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(4) = New SqlParameter("@PageIndex", PageIndex)
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(6) = New SqlParameter("@IsExport", "N")
            prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetWalletTransactionDetail_Menual", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("AccountData") = Ds.Tables(0)
            ViewState("IdNo") = "IdNo"
            ViewState("Sort_Order") = "ASC"
            If Ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To 9
                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                    Dim img As New Image()
                    img.ImageUrl = "~/Images/Uparrow.png"
                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                    tableCell.Controls.Add(img)
                Next
                lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
                GvData.Visible = True
                gvContainer.Visible = True
            Else
                lblErr.Text = "No Record Found!!"
                GvData.Visible = False
                gvContainer.Visible = False
            End If
            Me.PopulatePager(recordCount, PageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Try
            Dim dt As DataTable = CType(Session("AccountData"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()
            ViewState("Idno") = sColimnName
            ViewState("Sort_Order") = sSortOrder
            GvData.Focus()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.BindData(1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("AccountData")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        'gridview.BorderWidth = "2px"
        GvData.BorderStyle = BorderStyle.Solid
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
        dtData = Session("AccountData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("AccountData")
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
        dtData = Session("AccountData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub
End Class
