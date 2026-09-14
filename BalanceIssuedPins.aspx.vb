Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_BalanceIssuedPins
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Epin Report / Epin Balance Detail"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                GvData.Visible = False
                gvContainer.Visible = False
                Session("BalancedPinReport") = Nothing
                Fillkit()
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


    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("BalancedPinReport"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("IdNo") = sColimnName
        ViewState("Sort_Order") = sSortOrder
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
    Public Sub BindData(ByVal PageIndex As Integer)
        LblError.Text = ""
        lblCount.Text = ""
        Try


            'Dim Sql As String = "select temp1.Idno,(a.MemFirstName+' '+a.MemLastName) as MemName,b.KitName,b.KitAmount,b.Bv,Temp1.ReceivedFromId,Temp1.ReceivedFromAdmin," & _
            '                   " Temp1.TransferPin,Temp1.UsedPin,Temp1.BalancePin,(Temp1.BalancePin* b.kitAmount) as BalanceEpinValue  from ( " & _
            '                " select sum(ReceivedFromId) as ReceivedFromId,Sum(ReceivedFromAdmin) as ReceivedFromAdmin," & _
            '                " Sum(TransferPin) as TransferPin,Sum(UsedPin) as UsedPin,Sum(BalancePin) as BalancePin,KitId,Idno from( " & _
            '                " select Count(b.KitId) as ReceivedFromId,0 as ReceivedFromAdmin,0  as TransferPin,0 as UsedPin, " & _
            '              " 0 as BalancePin,KitId ,b.ToIdNo as Idno from trnTransferPinDetail as b group by ToIdno,KitId " & _
            '              " Union All " & _
            '              " select 0 as ReceivedfromId,Count(ProdId) as ReceivedFromAdmin,0  as TransferPin,0 as UsedPin," & _
            '              " 0 as BalancePin,ProdId as KitId,IssuedIdno as Idno from M_FormGeneration " & _
            '              " where 1=1  " & Condition1 & " group by ProdId,IssuedIdno" & _
            '              " Union All " & _
            '              " select 0 as ReceivedfromId,0 as ReceivedFromAdmin,Count(KitId) as TransferPin,0 as UsedPin, " & _
            '              " 0 as BalancePin,Kitid as KitId,FromIdno as Idno from trnTransferPinDetail as b" & _
            '              " group by KitId,FromIdno " & _
            '              "  Union All " & _
            '             " select 0 as ReceivedfromId,0 as ReceivedFromAdmin,0 as TransferPin,Count(Prodid) as UsedPin, " & _
            '             " 0 as BalancePin,ProdId as KitId,Fcode as Idno from M_FormGeneration " & _
            '              " where  IsIssued='Y' " & Condition2 & " group by ProdId,Fcode " & _
            '            " Union All " & _
            '             " select 0 as ReceivedfromId,0 as ReceivedFromAdmin,0 as TransferPin,0 as UsedPin," & _
            '            " Count(Prodid) as BalancePin,ProdId as KitId,Fcode as Idno from M_FormGeneration " & _
            '             " where  IsIssued='N' " & Condition2 & " group by ProdId,Fcode)" & _
            '             " as temp  where 1=1 " & Condition3 & " Group by KitId,Idno) as Temp1, M_Membermaster as a," & _
            '             "  M_kitMaster as b where a.idNo=Temp1.Idno and b.KitId=temp1.KitId and b.RowStatus='Y' " & Condition_ & ""


            '        dtData = New DataTable
            'dtData = objDAL.GetData(Sql)
            'GvData.DataSource = dtData
            'GvData.DataBind()
            'Session("BalancedPinReport") = dtData
            'GvData.Visible = True
            'gvContainer.Visible = True
            'If dtData.Rows.Count > 0 Then
            '    lblCount.Text = "Total Record: " & dtData.Rows.Count
            '    btnExport.Enabled = True
            '    btnPrintAll.Enabled = True
            '    btnPrintCurrent.Enabled = True
            'Else
            '    LblError.Text = "No Record Found!!"
            '    btnExport.Enabled = False
            '    btnPrintAll.Enabled = False
            '    btnPrintCurrent.Enabled = False
            'End If
            Dim KitId As String = "0"
            Dim Idno As String = "0"
            If ChkKit.Checked = True Then
                KitId = CmbKit.SelectedValue
            Else
                KitId = "0"
            End If
            If ChkMem.Checked = True Then
                Idno = TxtMemId.Text
            Else
                Idno = "0"

            End If
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(5) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@KitId", KitId)
            prms(2) = New SqlParameter("@PageIndex", PageIndex)
            prms(3) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(4) = New SqlParameter("@IsExport", "N")
            prms(5) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetBalanceEpinDetail", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("BalancedPinReport") = Ds.Tables(0)
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
            Else
                LblError.Text = "No Record Found!!"
                GvData.Visible = False
                gvContainer.Visible = False
            End If
            Me.PopulatePager(recordCount, PageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("BalancedPinReport")
        GvData.DataBind()
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("BalancedPinReport1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "BalancedEpin")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=BalancedEpinReport.xlsx")
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

            Dim KitId As String = "0"
            Dim Idno As String = "0"
            If ChkKit.Checked = True Then
                KitId = CmbKit.SelectedValue
            Else
                KitId = "0"
            End If
            If ChkMem.Checked = True Then
                Idno = TxtMemId.Text
            Else
                Idno = "0"

            End If
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(5) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@KitId", KitId)
            prms(2) = New SqlParameter("@PageIndex", 1)
            prms(3) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(4) = New SqlParameter("@IsExport", "Y")
            prms(5) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetBalanceEpinDetail", prms)
            Session("BalancedPinReport1") = Ds.Tables(0)
            ExportExcel()
        Catch ex As Exception

        End Try
    End Sub
    'Private Sub ExportIssuePin()
    '    Try
    '        Dim dtTemp As New DataTable



    '        Dim dg As New DataGrid
    '        Dim Condition As String = ""
    '        Dim Condition1 As String = ""
    '        Dim condition2 As String = ""
    '        Dim Condition3 As String = ""
    '        If ChkKit.Checked = True Then
    '            Condition = Condition & " And Temp1.KitID='" & CmbKit.SelectedValue & "'"
    '        End If
    '        If ChkMem.Checked = True Then
    '            Condition3 = "And Idno='" & Trim(TxtMemId.Text) & "' "
    '            Condition1 = "and IssuedIdno='" & Trim(TxtMemId.Text) & "'"
    '            condition2 = "and Fcode='" & Trim(TxtMemId.Text) & "'"
    '        End If


    '        Dim Sql As String = "select temp1.Idno as IdNo,(a.MemFirstName+' '+a.MemLastName) as [Member Name],b.KitName as [Package Name],b.KitAmount as [Package Amount],b.Bv as [Package Bv],Temp1.ReceivedFromId as [Received from Id] ,Temp1.ReceivedFromAdmin as [Received From Admin]," & _
    '                        " Temp1.TransferPin as [Transfer Pin],Temp1.UsedPin as [Used Pin],Temp1.BalancePin as [Balance Pin],(Temp1.BalancePin* b.kitAmount) as [Balance Epin Value] from ( " & _
    '                        " select sum(ReceivedFromId) as ReceivedFromId,Sum(ReceivedFromAdmin) as ReceivedFromAdmin," & _
    '                        " Sum(TransferPin) as TransferPin,Sum(UsedPin) as UsedPin,Sum(BalancePin) as BalancePin,KitId,Idno from( " & _
    '                        " select Count(b.KitId) as ReceivedFromId,0 as ReceivedFromAdmin,0  as TransferPin,0 as UsedPin, " & _
    '                      " 0 as BalancePin,KitId ,b.ToIdNo as Idno from trnTransferPinDetail as b group by ToIdno,KitId " & _
    '                      " Union All " & _
    '                      " select 0 as ReceivedfromId,Count(ProdId) as ReceivedFromAdmin,0  as TransferPin,0 as UsedPin," & _
    '                      " 0 as BalancePin,ProdId as KitId,IssuedIdno as Idno from M_FormGeneration " & _
    '                      " where 1=1  " & Condition1 & " group by ProdId,IssuedIdno" & _
    '                      " Union All " & _
    '                      " select 0 as ReceivedfromId,0 as ReceivedFromAdmin,Count(KitId) as TransferPin,0 as UsedPin, " & _
    '                      " 0 as BalancePin,Kitid as KitId,FromIdno as Idno from trnTransferPinDetail as b" & _
    '                      " group by KitId,FromIdno " & _
    '                      "  Union All " & _
    '                     " select 0 as ReceivedfromId,0 as ReceivedFromAdmin,0 as TransferPin,Count(Prodid) as UsedPin, " & _
    '                     " 0 as BalancePin,ProdId as KitId,Fcode as Idno from M_FormGeneration " & _
    '                      " where  IsIssued='Y' " & condition2 & " group by ProdId,Fcode " & _
    '                    " Union All " & _
    '                     " select 0 as ReceivedfromId,0 as ReceivedFromAdmin,0 as TransferPin,0 as UsedPin," & _
    '                    " Count(Prodid) as BalancePin,ProdId as KitId,Fcode as Idno from M_FormGeneration " & _
    '                     " where  IsIssued='N' " & condition2 & " group by ProdId,Fcode)" & _
    '                     " as temp  where 1=1 " & Condition3 & " Group by KitId,Idno) as Temp1, M_Membermaster as a," & _
    '                     "  M_kitMaster as b where a.idNo=Temp1.Idno and b.KitId=temp1.KitId and b.RowStatus='Y' " & Condition & ""
    '        'Adp.Fill(ds, "ExportToExcel")
    '        dtTemp = New DataTable
    '        dtTemp = objDAL.GetData(Sql)

    '        dg.DataSource = dtTemp
    '        dg.DataBind()

    '        ExportToExcel("BalancePins.xls", dg)

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
        LblError.Text = ""
        lblCount.Text = ""
        Dim Condition As String = ""
        
    
        Dim Condition1 As String = ""
        Dim condition2 As String = ""
        Dim Condition3 As String = ""
        If ChkKit.Checked = True Then
            Condition = Condition & " And Temp1.KitID='" & CmbKit.SelectedValue & "'"
        End If
        If ChkMem.Checked = True Then
            Condition3 = " and Idno='" & Trim(TxtMemId.Text) & "' "
            Condition1 = "and IssuedIdno='" & Trim(TxtMemId.Text) & "'"
            condition2 = "and Fcode='" & Trim(TxtMemId.Text) & "'"
        End If

        BindData(1)
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
            dtData = Session("BalancedPinReport")
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
            dtData = Session("BalancedPinReport")
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
            dtData = Session("BalancedPinReport")
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
            dtData = Session("BalancedPinReport")
            GvData.DataSource = dtData
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    


    Protected Sub TxtMemId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtMemId.TextChanged
        Check_IdNo()
    End Sub
    Private Function Check_IdNo() As Boolean
        Dim sql As String = ""
        sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From " & objDAL.tblMemberMaster & " WHERE IDNO='" & Trim(TxtMemId.Text) & "'"
        Dim Dt_ As New DataTable
        Dt_ = objDAL.GetData(Sql)
        If Dt_.Rows.Count = 0 Then
            LblError.Text = " Please enter correct Member ID."
            LblError.ForeColor = Drawing.Color.Red
            LblError.Visible = True
            TxtMemId.Text = ""
            btnSearch.Enabled = False
            btnExport.Enabled = False
            btnPrintAll.Enabled = False
            btnPrintCurrent.Enabled = False
            Return False
        Else
            LblError.Text = Dt_.Rows(0)("MemName")
            LblError.ForeColor = Drawing.Color.Black
            LblError.Visible = True
            btnSearch.Enabled = True
            btnExport.Enabled = True
            btnPrintAll.Enabled = True
            btnPrintCurrent.Enabled = True
            Return True
        End If
    End Function

    'Protected Sub btnshowall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnshowall.Click
    '    BindData(1)
    'End Sub
End Class
