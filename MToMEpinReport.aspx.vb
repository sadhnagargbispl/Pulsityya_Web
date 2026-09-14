Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class MToMEpinReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = " Epin Report / M To M Epin Transfer Report"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If Not Page.IsPostBack Then
            GvData.Visible = False
            gvContainer.Visible = False
            Session("MtMPin") = Nothing
            FillKit()
            Filldate()
        End If
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
    Private Sub Filldate()
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as Date,Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
        dtData = New DataTable
        dtData = objDAL.GetData(Str)
        If dtData.Rows.Count > 0 Then
            txtStartDate.Text = dtData.Rows(0)("CurrentDate")
            txtEndDate.Text = dtData.Rows(0)("CurrentDate")
        End If

    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MtMPin")
        GvData.DataBind()
    End Sub


    'Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
    '    Try
    '        Dim dtTemp As New DataTable
    '        Dim dg As New DataGrid
    '        Dim Condition As String = ""
    '        Dim formno As String = ""
    '        Dim scrName As String = ""

    '        If ChkMem.Checked Then
    '            If DDlSearchName.SelectedValue = "F" Then

    '                Condition = Condition & " and Temp.FromIdNo='" & Trim(txtMember.Text) & "'"
    '            Else

    '                Condition = Condition & " And  Temp.ToIdNo='" & Trim(txtMember.Text) & "'"
    '            End If

    '        End If
    '        If txtStartDate.Text <> "" Then
    '            Condition = Condition & " And  Cast(Convert(Varchar,Temp.RecTimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
    '        End If
    '        If txtEndDate.Text <> "" Then
    '            Condition = Condition & " And Cast(Convert(Varchar,Temp.RecTimeStamp,106)as DateTime)<='" & txtEndDate.Text & "'"
    '        End If
    '        If ChkKit.Checked Then
    '            Condition = Condition & " And Temp.KitId='" & CmbKit.SelectedValue & "'"
    '        End If
    '        Dim qry1 As String = ""
    '        qry1 = "select Temp.FromIdNo,(b.MemFirstname+' '+b.MemLastname) as FromUserName,ToIdNo,(a.MemFirstName+' '+ a.Memlastname) ToUsername,c.KitName as [Package Name]," & _
    '         " c.KitAmount as [Package MRP],c.Bv as [Package Bv],Temp.Qty as TransferQty,(c.KitAmount*Temp.Qty) as [EpinValue]," & _
    '    " Replace(Convert(Varchar,Temp.RecTimestamp,106),' ','-') + ' '+  CONVERT(varchar(15),CAST(Temp.RectimeStamp AS TIME),100)as [Transaction Date and Time]" & _
    '    " from(" & _
    '  " select FromIdNo,ToIdNo,Count(KitId) as qty,KitId,RecTimestamp from TrnTransferPinDetail Group By FromIdNo,ToIdNo,RecTimeStamp,KitId " & _
    '" ) as Temp,M_Membermaster as a,M_Membermaster as b,M_Kitmaster as c where Temp.FromIdNo=b.IdNo and Temp.ToIdNo = a.IdNo " & _
    ' " and Temp.KitId=c.KitId and c.RowStatus='Y'" & Condition & " "
    '        dtTemp = New DataTable
    '        dtTemp = objDAL.GetData(qry1)

    '        dg.DataSource = dtTemp
    '        dg.DataBind()

    '        ExportToExcel("MToMEPinTransferReport.xls", dg)

    '    Catch ex As Exception
    '        Response.Write(ex.Message & "Error In Exporting File")
    '    End Try
    'End Sub

    Protected Sub BindData(ByVal PageIndex As Integer)
        Try
            lblErr.Text = ""
            lblCount.Text = ""
            ' Dim Condition As String = ""
            ' Dim formno As String = ""
            Dim scrName As String = ""
            Dim FromIdno As String = "0"
            Dim ToIdNo As String = "0"
            Dim kitid As String = "0"
            If ChkMem.Checked Then
                If DDlSearchName.SelectedValue = "F" Then
                    FromIdno = Trim(txtMember.Text)
                    ToIdNo = "0"
                    'Condition = Condition & " and Temp.FromIdNo='" & Trim(txtMember.Text) & "'"
                Else
                    ToIdNo = Trim(txtMember.Text)
                    FromIdno = "0"
                    ' Condition = Condition & " And  Temp.ToIdNo='" & Trim(txtMember.Text) & "'"
                End If

            End If

            If ChkKit.Checked Then
                kitid = CmbKit.SelectedValue
            Else
                kitid = "0"
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
            '        Dim qry1 As String = ""
            '        qry1 = "select Temp.FromIdNo,(b.MemFirstname+' '+b.MemLastname) as FromUserName,ToIdNo,(a.MemFirstName+' '+ a.Memlastname) ToUsername,c.KitName as [Package Name]," & _
            '         " c.KitAmount as [Package MRP],c.Bv as [Package Bv],Temp.Qty as TransferQty,(c.KitAmount*Temp.Qty) as [EpinValue]," & _
            '    " Replace(Convert(Varchar,Temp.RecTimestamp,106),' ','-') + ' '+  CONVERT(varchar(15),CAST(Temp.RectimeStamp AS TIME),100)as [Transaction Date and Time]" & _
            '    " from(" & _
            '  " select FromIdNo,ToIdNo,Count(KitId) as qty,KitId,RecTimestamp from TrnTransferPinDetail Group By FromIdNo,ToIdNo,RecTimeStamp,KitId " & _
            '" ) as Temp,M_Membermaster as a,M_Membermaster as b,M_Kitmaster as c where Temp.FromIdNo=b.IdNo and Temp.ToIdNo = a.IdNo " & _
            ' " and Temp.KitId=c.KitId and c.RowStatus='Y'" & Condition & " "
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(8) {}
            prms(0) = New SqlParameter("@FromIDNo", Convert.ToString(FromIdno).ToLower())
            prms(1) = New SqlParameter("@ToIdNo", Convert.ToString(ToIdNo).ToLower)
            prms(2) = New SqlParameter("@KitId", kitid)
            prms(3) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(4) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(5) = New SqlParameter("@PageIndex", PageIndex)
            prms(6) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(7) = New SqlParameter("@IsExport", "N")
            prms(8) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetMToMEpinDetail", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()


            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("MtMPin") = Ds.Tables(0)
            ViewState("FromIdno") = "FromIdNo"
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

                btnPrintAll.Enabled = False
                btnPrintCurrent.Enabled = False

                GvData.Visible = False
                gvContainer.Visible = False
                lblErr.Text = "No Record Found!!"
            End If
            Me.PopulatePager(recordCount, PageIndex)
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


            Dim dt As DataTable = CType(Session("MtMPin"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()

            ViewState("FromIdno") = sColimnName
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindData(1)

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
        Try


            GvData.AllowPaging = False
            GvData.GridLines = GridLines.Both
            dtData = New DataTable
            dtData = Session("MtMPin1")
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
            dtData = Session("MtMPin1")
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
            dtData = Session("MtMPin1")
            GvData.DataSource = dtData
            GvData.DataBind()
            GvData.PagerSettings.Visible = False
            GvData.BorderStyle = BorderStyle.Solid
            ' gridview.BorderWidth = 
            GvData.BorderColor = Drawing.Color.Black



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
            dtData = Session("MtMPin1")
            GvData.DataSource = dtData
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.BindData(1)
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim FromIdno As String = "0"
            Dim ToIdNo As String = "0"
            Dim kitid As String = "0"
            If ChkMem.Checked Then
                If DDlSearchName.SelectedValue = "F" Then
                    FromIdno = Trim(txtMember.Text)
                    ToIdNo = "0"
                    'Condition = Condition & " and Temp.FromIdNo='" & Trim(txtMember.Text) & "'"
                Else
                    ToIdNo = Trim(txtMember.Text)
                    FromIdno = "0"
                    ' Condition = Condition & " And  Temp.ToIdNo='" & Trim(txtMember.Text) & "'"
                End If

            End If

            If ChkKit.Checked Then
                kitid = CmbKit.SelectedValue
            Else
                kitid = "0"
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
            Dim prms As SqlParameter() = New SqlParameter(8) {}
            prms(0) = New SqlParameter("@FromIDNo", Convert.ToString(FromIdno).ToLower())
            prms(1) = New SqlParameter("@ToIdNo", Convert.ToString(ToIdNo).ToLower)
            prms(2) = New SqlParameter("@KitId", kitid)
            prms(3) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(4) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(5) = New SqlParameter("@PageIndex", 1)
            prms(6) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(7) = New SqlParameter("@IsExport", "Y")
            prms(8) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetMToMEpinDetail", prms)
            Session("MtMPin1") = Ds.Tables(0)
            If Ds.Tables(0).Rows.Count > 0 Then
                ' btnPrintCurrent.Enabled = True
                ' btnPrintAll.Enabled = True
            Else
                ' btnPrintCurrent.Enabled = False
                ' btnPrintAll.Enabled = False
            End If
            ExportExcel()
        Catch ex As Exception

        End Try
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("MtMPin1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "MToMEpinTransfer")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=MToMEpinTransfer.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
End Class
