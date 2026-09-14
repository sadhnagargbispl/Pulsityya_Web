Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_RptIssuedPins
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    '' Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = " Epin / Deactive E-Pin Report"
                Filldate()
            Else
                Response.Redirect("Default.aspx")
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                GvData.Visible = False
                gvContainer.Visible = False
                Session("IssuedPins") = Nothing
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

    Public Sub BindData(ByVal pageIndex As Integer)
        Try

            Dim KitId As String = "0"
            If ChkKit.Checked = True Then
                KitId = CmbKit.SelectedValue
            Else
                KitId = "0"
            End If
            Dim IssuedIdno As String = "0"
            Dim FCODE As String = "0"

            If ChkMember.Checked = True Then
                If DDlMember.SelectedValue = "A" Then
                    If TxtMemId.Text <> "" Then
                        IssuedIdno = TxtMemId.Text.Trim
                        FCODE = "0"
                    Else
                        IssuedIdno = "0"
                        FCODE = "0"
                    End If
                    'Condition = Condition & " And IssuedIdno='" & Trim(TxtMemId.Text) & "'"
                    'subcon = ",'1' as ReqType"
                Else
                    If TxtMemId.Text <> "" Then
                        FCODE = TxtMemId.Text.Trim
                        IssuedIdno = "0"
                    Else
                        IssuedIdno = "0"
                        FCODE = "0"
                    End If
                End If


            End If
            'lblErr.Text = ""
            'lblCount.Text = ""
            'If Condition2 = "" Then
            '    Condition2 = ", Case when IssuedIdno<> Fcode then '2' when IssuedIdno=Fcode then '1' else '0' end as ReqType"
            'End If
            'Dim Sql As String = "select FCode,ChallanNo,ProdId,Count(ProdId) as KitId,b.KitName,a.IssuedIdno,(C.MemFirstName+' '+c.MemLastName) as MemName,Replace(Convert(varchar,IssuedDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(IssuedDate AS TIME),100)as IssuedDate, IssuedDate as IssuedDate1" & Condition2 & "  from M_FormGeneration as a," & _
            '                  " M_Kitmaster as b,m_membermaster as c where a.ProdId=b.KiTid and a.fcode=c.idno" & _
            '               " and b.ActiveStatus='Y' and b.RowStatus='Y' And a.LastModified='Y' and a.GeneratedBy='Y'" & Condition_ & "" & _
            '                 " Group by Kitname ,ProdId,ChallanNo,IssuedIdno,C.MemFirstname,c.MemLastName,IssuedDate,FCode"

            Dim startDate As Date
            Dim EndDate As Date
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                EndDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                EndDate = txtEndDate.Text
            End If
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(8) {}
            prms(0) = New SqlParameter("@IssuedIDNo", Convert.ToString(IssuedIdno).ToLower())
            prms(1) = New SqlParameter("@FCode", Convert.ToString(FCODE).ToLower())
            prms(2) = New SqlParameter("@KitId", KitId)
            prms(3) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(4) = New SqlParameter("@EndDate", Convert.ToDateTime(EndDate))
            prms(5) = New SqlParameter("@PageIndex", pageIndex)
            prms(6) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(7) = New SqlParameter("@IsExport", "N")
            prms(8) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetDeactiveEpinDetail", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("IssuedPinReport") = Ds.Tables(0)
            ViewState("IdNo") = "IssuedIDNo"
            ViewState("Sort_Order") = "ASC"
            GvData.Visible = True
            gvContainer.Visible = True
            If Ds.Tables(1).Rows.Count > 0 Then
                For i As Integer = 0 To GvData.Columns.Count - 2
                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                    Dim img As New Image()
                    img.ImageUrl = "~/Images/Uparrow.png"
                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                    tableCell.Controls.Add(img)
                Next
                lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
                btnExport.Enabled = True
                btnPrintAll.Enabled = False
                btnPrintCurrent.Enabled = False
            Else
                lblErr.Text = "No Record Found!!"
                btnExport.Enabled = False
                btnPrintAll.Enabled = False
                btnPrintCurrent.Enabled = False
            End If
            Me.PopulatePager(recordCount, pageIndex)



            'GvData.DataSource = Ds.Tables(0)
            'GvData.DataBind()




            'dtData = New DataTable
            'dtData = objDAL.GetData(Sql)
            'GvData.DataSource = dtData
            'GvData.DataBind()
            'Session("IssuedPins") = dtData
            'ViewState("WithDrawDate") = "FCode"
            'ViewState("Sort_Order") = "ASC"
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
        Catch ex As Exception
            'Response.Write(ex.Message & "SideB")
        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("IssuedPinReport")
        GvData.DataBind()
    End Sub



    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Try

            Dim KitId As String = "0"
            If ChkKit.Checked = True Then
                KitId = CmbKit.SelectedValue
            Else
                KitId = "0"
            End If
            Dim IssuedIdno As String = "0"
            Dim FCODE As String = "0"

            If ChkMember.Checked = True Then
                If DDlMember.SelectedValue = "A" Then
                    If TxtMemId.Text <> "" Then
                        IssuedIdno = TxtMemId.Text.Trim
                        FCODE = "0"
                    Else
                        IssuedIdno = "0"
                        FCODE = "0"
                    End If
                    'Condition = Condition & " And IssuedIdno='" & Trim(TxtMemId.Text) & "'"
                    'subcon = ",'1' as ReqType"
                Else
                    If TxtMemId.Text <> "" Then
                        FCODE = TxtMemId.Text.Trim
                        IssuedIdno = "0"
                    Else
                        IssuedIdno = "0"
                        FCODE = "0"
                    End If
                End If


            End If
            Dim startDate As Date
            Dim EndDate As Date
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                EndDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                EndDate = txtEndDate.Text
            End If

            Dim prms As SqlParameter() = New SqlParameter(8) {}
            prms(0) = New SqlParameter("@IssuedIDNo", Convert.ToString(IssuedIdno).ToLower())
            prms(1) = New SqlParameter("@FCode", Convert.ToString(FCODE).ToLower())
            prms(2) = New SqlParameter("@KitId", KitId)
            prms(3) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(4) = New SqlParameter("@EndDate", Convert.ToDateTime(EndDate))
            prms(5) = New SqlParameter("@PageIndex", 1)
            prms(6) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(7) = New SqlParameter("@IsExport", "Y")
            prms(8) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetDeactiveEpinDetail", prms)
            Session("IssuedPinReport1") = Ds.Tables(0)
            If Ds.Tables(0).Rows.Count > 0 Then
                btnPrintCurrent.Enabled = True
                btnPrintAll.Enabled = True
            Else
                btnPrintCurrent.Enabled = False
                btnPrintAll.Enabled = False
            End If
            ExportExcel()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("IssuedPinReport1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "DeactivePinReport")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=DeactiveEpinReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try


            lblErr.Text = ""
            lblCount.Text = ""
            BindData(1)
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
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
        dtData = Session("IssuedPins")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
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
        dtData = Session("IssuedPins")
        GvData.DataSource = dtData
        GvData.DataBind()
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


    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try


            ' If e.SortExpression = ViewState("FromIdno").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 2
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
                For i As Integer = 0 To GvData.Columns.Count - 2
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
        Dim dt As DataTable = CType(Session("IssuedPinReport"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("IdNo") = sColimnName
        ViewState("Sort_Order") = sSortOrder
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
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try


            Me.BindData(1)
        Catch ex As Exception

        End Try
    End Sub
  
  
End Class
