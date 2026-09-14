Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_IssuedEPinReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = " Epin Report / Issued Epin Report"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            '  objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                GvData.Visible = False
                gvContainer.Visible = False
                Session("IssuedPinReport") = Nothing
                Fillkit()
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

    Public Sub BindData(ByVal pageIndex As Integer)
        lblErr.Text = ""
        lblCount.Text = ""
        Try
            Dim Condition As String = ""
            Dim kitId As String = "0"

            'If txtStartDate.Text <> "" Then
            '    Condition = Condition & " and CAST(Transactiondate AS DATE)>='" & txtStartDate.Text & "'"

            'End If
            'If txtEndDate.Text <> "" Then
            '    Condition = Condition & " and CAST(Transactiondate AS DATE)<='" & txtEndDate.Text & "'"
            'End If
            'If ChkKit.Checked = True Then
            '    Condition = Condition & " And KitID='" & CmbKit.SelectedValue & "'"
            'End If

            If ChkMember.Checked Then
                If TxtMemId.Text <> "" Then
                    Condition = Trim(TxtMemId.Text)
                Else
                    Condition = "'0'"

                End If
            Else
                Condition = "0"

            End If
            If ChkKit.Checked Then
                KitId = CmbKit.SelectedValue
            Else
                KitId = "0"
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
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Condition).ToLower())
            prms(1) = New SqlParameter("@KitId", kitId)
            prms(2) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(3) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(4) = New SqlParameter("@PageIndex", pageIndex)
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(6) = New SqlParameter("@IsExport", "N")
            prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetIssuedEpinDetail", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()

            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("IssuedPinReport") = Ds.Tables(0)
            ViewState("IdNo") = "IdNo"
            ViewState("Sort_Order") = "ASC"

            If Ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                    Dim img As New Image()
                    img.ImageUrl = "~/Images/Uparrow.png"
                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                    tableCell.Controls.Add(img)
                Next
                lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
                GvData.Visible = True
                gvContainer.Visible = True
                'btnExport.Enabled = True
                'btnPrintAll.Enabled = False
                'btnPrintCurrent.Enabled = False
            Else
                lblErr.Text = "No Record Found!!"
                GvData.Visible = False
                gvContainer.Visible = False
                'btnExport.Enabled = False
                'btnPrintAll.Enabled = False
                'btnPrintCurrent.Enabled = False
            End If
            Me.PopulatePager(recordCount, pageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try



        'Dim sql As String = "select * from(" & _
        '" select Cast(a.ReqNo as Varchar) as ReqNo,a.IdNo,(b.MemFirstName+' '+b.MemLastName)" & _
        '                 " as MemName,a.KitId,a.KitName,a.KitAmount, c.Bv,a.Qty as RequestQuantity,a.DispQty as DispatchQty ," & _
        '             " s.TotalAmount,a.DepositAmount,a.Remarks,Case when u.UserName is NULL  then ' ' else u.UserName " & _
        '            " end as Username,Replace(Convert(Varchar,a.DispatchDate,106),' ','-') as TransactionDate, " & _
        '               " CONVERT(varchar(15), CAST(a.DispatchDate AS TIME),100)as TransactionTime from TrnPinRequest " & _
        '             " as a ,TrnPinDispatch as d Left join M_UserMaster as u On d.UserId=u.UserId  " & _
        '            " and u.RowStatus='Y' ,M_MemberMaster as b,M_Kitmaster as c,TrnPinReqMain as s " & _
        '            "  where a.ReqNo=s.ReqNo and  a.Formno=b.Formno and a.KitId=c.KitId and c.RowStatus='Y' " & _
        '            " and a.DispQty<>0  and d.ReqNo=a.ReqNo  and d.Rid in(select Max(Rid) from TrnPinDispatch Group by ReqNo) " & _
        '            " and a.Idno=d.Idno and a.KitId=d.KitId" & _
        '            " Union All " & _
        '    " select 'Direct Issued by Admin' as ReqNo,temp.Idno,(c.MemFirstName+' '+c.MemLastName) as MemName,Temp.KitId,d.KitName,d.KitAmount," & _
        '  " d.Bv, 0 as RequestQuantity,Temp.RequestQuantity as DispatchQty,(d.KitAmount*Temp.RequestQuantity) as TotalAmount ,(d.KitAmount*Temp.RequestQuantity) as DispatchAmount," & _
        '   " Temp.Narration as Remark,Case when b.UserName is NULL  then ' ' else b.UserName end as Username " & _
        '   " ,Replace(Convert(Varchar,Temp.IssuedDate,106),' ','-')  as TransactionDate, CONVERT(varchar(15), " & _
        '  " CAST(Temp.IssuedDate AS TIME),100)as TransactionTime from (" & _
        '" select Count(ProdId) as RequestQuantity,ProdId as KitId,IssuedIdno as Idno,IssuedUserId, IssuedDate,Narration from M_FormGeneration " & _
        ' " where ReqNo=0 group by ProdId,IssuedIdno,IssuedUserId,IssuedDate,Narration) as Temp Left Join M_UserMaster as b on Temp.IssuedUserId=b.UserId  and b.RowStatus='Y'  ,M_MemberMaster as c," & _
        '  " M_KitMaster as d where temp.Idno=c.Idno and temp.KitId=d.KitId  and d.RowStatus='Y' ) as Temp1 where 1=1 " & Condition_ & " order by Year(TransactionDate) Desc,Month(TransactionDate)Desc,Day(TransactionDate)Desc,TransactionTime Desc "


        'dtData = New DataTable
        'dtData = objDAL.GetData(sql)
        ' GvData.DataSource = dtData
        ' GvData.DataBind()
        'Session("IssuedPinReport") = dtData
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
            Dim dt As DataTable = CType(Session("IssuedPinReport"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()

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
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        Try
            GvData.PageIndex = e.NewPageIndex
            GvData.DataSource = Session("IssuedPinReport")
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim Condition As String = ""
            Dim kitId As String = "0"
            If ChkMember.Checked Then
                If TxtMemId.Text <> "" Then
                    Condition = Trim(TxtMemId.Text)
                Else
                    Condition = "'0'"

                End If
            Else
                Condition = "0"

            End If
            If ChkKit.Checked Then
                kitId = CmbKit.SelectedValue
            Else
                kitId = "0"
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
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Condition).ToLower())
            prms(1) = New SqlParameter("@KitId", kitId)
            prms(2) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(3) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(4) = New SqlParameter("@PageIndex", 1)
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(6) = New SqlParameter("@IsExport", "Y")
            prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetIssuedEpinDetail", prms)
            Session("IssuedPinReport1") = Ds.Tables(0)
            ExportExcel()
            ''btnPrintAll.Enabled = True
            'btnPrintCurrent.Enabled = True
        Catch ex As Exception
        End Try

    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("IssuedPinReport1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "IssuedEpin")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=IssuedEpinReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
    'Private Sub ExportIssuePin()
    '    Try
    '        Dim dtTemp As New DataTable



    '        Dim dg As New DataGrid
    '        Dim Condition As String = ""
    '        If txtStartDate.Text <> "" Then
    '            Condition = Condition & " and CAST(Transactiondate AS DATE)>='" & txtStartDate.Text & "'"
    '            ' condition2 = condition2
    '        End If
    '        If txtEndDate.Text <> "" Then
    '            ' Condition = Condition & "and REPLACE(CONVERT(VARCHAR,CAST(TransactionDate AS DATE), 106), ' ', '-')<='" & txtEndDate.Text & "'"
    '            Condition = Condition & " And Cast(TransactionDate as Date)<='" & txtEndDate.Text & "'"
    '        End If
    '        If ChkKit.Checked = True Then
    '            Condition = Condition & " And KitID='" & CmbKit.SelectedValue & "'"
    '        End If

    '        If ChkMember.Checked Then
    '            If TxtMemId.Text <> "" Then
    '                Condition = Condition & " And Idno='" & Trim(TxtMemId.Text) & "' "

    '            End If
    '        End If



    '        '         Dim sql As String = "select IdNo,MemName as MemberName,kitName as PackageName,Bv as PackageBv,KitAmount as PackageAmount,DispatchQty as AcceptQuantity," & _
    '        '        " DepositAmount,Remarks,UserName,Replace(Convert(Varchar,TransactionDate,106),' ','-')as TransactionDate,CONVERT(varchar(15), CAST(TransactionDate AS TIME),100) as TransactionTime from(" & _
    '        '   " select Cast(a.ReqNo as Varchar) as ReqNo,a .IdNo,(b.MemFirstName+' '+b.MemLastName)" & _
    '        '                     " as MemName,a.KitId,a.KitName,a.KitAmount, c.Bv,a.Qty as RequestQuantity,a.DispQty as DispatchQty ," & _
    '        '                 " s.TotalAmount,a.DepositAmount,a.Remarks,Case when u.UserName is NULL  then ' ' else u.UserName " & _
    '        '                " end as Username,a.DispatchDate as TransactionDate from TrnPinRequest " & _
    '        '                 " as a ,TrnPinDispatch as d Left join M_UserMaster as u On d.UserId=u.UserId " & _
    '        '                " and u.RowStatus='Y' ,M_MemberMaster as b,M_Kitmaster as c,TrnPinReqMain as s " & _
    '        '                "  where a.ReqNo=s.ReqNo and  a.Formno=b.Formno and a.KitId=c.KitId and c.RowStatus='Y' " & _
    '        '                " and a.DispQty<>0  and d.ReqNo=a.ReqNo  and d.Rid in(select Max(Rid) from TrnPinDispatch Group by ReqNo) " & _
    '        '                " and a.Idno=d.Idno and a.KitId=d.KitId" & _
    '        '    " Union All " & _
    '        '    " select 'Direct Issued by Admin' as ReqNo,temp.Idno,(c.MemFirstName+' '+c.MemLastName) as MemName,Temp.KitId,d.KitName,d.KitAmount," & _
    '        '  " d.Bv,0 as RequestQuantity,Temp.RequestQuantity as DispatchQty,(d.KitAmount*Temp.RequestQuantity) as TotalAmount ,(d.KitAmount*Temp.RequestQuantity) as DispatchAmount," & _
    '        '   " temp.Narration as Remark,Case when b.UserName is NULL  then ' ' else b.UserName end as Username " & _
    '        '   " ,Temp.IssuedDate as TransactionDate from (" & _
    '        '" select Count(ProdId) as RequestQuantity,ProdId as KitId,IssuedIdno as Idno,IssuedUserId, IssuedDate,Narration from M_FormGeneration " & _
    '        ' " where ReqNo=0 group by ProdId,IssuedIdno,IssuedUserId,IssuedDate,Narration) as Temp Left Join M_UserMaster as b on Temp.IssuedUserId=b.UserId  and b.RowStatus='Y'  ,M_MemberMaster as c," & _
    '        '  " M_KitMaster as d where temp.Idno=c.Idno and temp.KitId=d.KitId  and d.RowStatus='Y' ) as Temp1 where 1=1 " & Condition & " Order by  Year(TransactionDate) Desc,Month(TransactionDate)Desc,Day(TransactionDate)Desc,TransactionTime Desc"


    '        'Adp.Fill(ds, "ExportToExcel")
    '        'dtTemp = New DataTable
    '        'dtTemp = objDAL.GetData(sql)

    '        'dg.DataSource = dtTemp
    '        'dg.DataBind()

    '        'ExportToExcel("IssuedPin.xls", dg)

    '    Catch ex As Exception
    '        Response.Write(ex.Message & "Error In Exporting File")
    '    End Try
    '    'Comm.Cancel()
    '    'ds.Dispose()

    'End Sub


    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try


            lblErr.Text = ""
            lblCount.Text = ""

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
            dtData = Session("IssuedPinReport1")
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
            dtData = Session("IssuedPinReport")
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
            dtData = Session("IssuedPinReport1")
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
            dtData = Session("IssuedPinReport1")
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
