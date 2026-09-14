Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Configuration
Imports ClosedXML.Excel
Imports System.Globalization

Partial Class App_UI_Application_Pages_LevelDetailReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Report  / Level Incentive Report"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Not Page.IsPostBack Then
                txtMemId.Text = ""
                GvData.Visible = False

                If Session("AStatus") = "OK" Then
                    'BindSession()
                    'Filldate()
                    If Request.QueryString.HasKeys Then
                        If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                            txtMemId.Text = Request.QueryString("key")
                            If txtMemId.Text <> "" Then
                                ' CheckBox1.Checked = True
                                'IncentiveDetail()
                            End If


                            '  ChkRadioMember.SelectedValue = "N" : IncentiveDetail()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
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
    'Public Sub BindSession()
    '    Try
    '        Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelSession")
    '        ddlSession.DataSource = Ds.Tables(0)
    '        ddlSession.DataValueField = "SessID"
    '        ddlSession.DataTextField = "SessnName"
    '        ddlSession.DataBind()
    '    Catch ex As Exception
    '    End Try
    'End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        'Try

        'If CheckBox2.Checked = True Then
        '    If ddlSession.SelectedValue <> 5000000 Then
        '        Condition = Condition & " And SessID=" & ddlSession.SelectedValue
        '    End If
        'End If
        Dim FromSessid As String = "0"
        Dim ToSessid As String = "0"
        If txtStartDate.Text <> "" Then
            Dim inputDate As DateTime = DateTime.ParseExact(txtStartDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
            Dim outputDateString As String = inputDate.ToString("yyyyMMdd")

            FromSessid = outputDateString
        Else
            FromSessid = "0"
        End If
        If txtEndDate.Text <> "" Then
            Dim inputDate As DateTime = DateTime.ParseExact(txtEndDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
            Dim outputDateString As String = inputDate.ToString("yyyyMMdd")
            ToSessid = outputDateString
        Else
            ToSessid = "0"
        End If

        If txtMemId.Text <> "" Then
            Condition = " And IDNo='" & txtMemId.Text & "'"
            'IncentiveDetail(0, 1)
            IncentiveDetail(ToSessid, 1)
        Else
            'IncentiveDetail(ddlSession.SelectedValue, 1)
            IncentiveDetail(ToSessid, 1)
        End If
        'Catch ex As Exception

        'End Try
    End Sub
    Protected Sub ViewDetail(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)

            LblSessionNo.Text = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
            IncentiveDetail(Val(LblSessionNo.Text), 1)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub IncentiveDetail(ByVal Sessid As Integer, ByVal PageIndex As Integer)
        Try




            Dim Idno As String = "0"

            If txtMemId.Text <> "" Then
                Idno = txtMemId.Text
            Else
                Idno = "0"
            End If
            
            Dim FromSessid As String = "0"
            Dim ToSessid As String = "0"
            If txtStartDate.Text <> "" Then
                Dim inputDate As DateTime = DateTime.ParseExact(txtStartDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                Dim outputDateString As String = inputDate.ToString("yyyyMMdd")

                FromSessid = outputDateString
            Else
                FromSessid = "0"
            End If
            If txtEndDate.Text <> "" Then
                Dim inputDate As DateTime = DateTime.ParseExact(txtEndDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                Dim outputDateString As String = inputDate.ToString("yyyyMMdd")
                ToSessid = outputDateString
            Else
                ToSessid = "0"
            End If
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@IdNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@FromSessid", FromSessid)
            prms(2) = New SqlParameter("@ToSessid", ToSessid)
            prms(3) = New SqlParameter("@PageIndex", PageIndex)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "N")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            If Session("compid") = "1101" Or Session("compid") = "1103" Then
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetIncentiveDetailReportR", prms)
            Else
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetIncentiveDetailReport", prms)
            End If

            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()

            'Dim recordCount As Integer = Ds.Tables(0).Rows.Count
           Dim recordCount As Integer = Ds.Tables(1).Rows(0)("Recordcount")
            Session("GData1") = Ds.Tables(0)
            ViewState("PayoutDate") = "SNo"
            ViewState("Sort_Order") = "ASC"


            If Ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 1 To GvData.Columns.Count - 1
                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                    Dim img As New Image()
                    img.ImageUrl = "~/Images/Uparrow.png"
                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                    tableCell.Controls.Add(img)
                Next
                GvData.Focus()


            Else


            End If
            GvData.Visible = True
            Me.PopulatePager(recordCount, PageIndex)

            Session("IssuedPinValue1") = Ds.Tables(0)
            'ExportExcel()
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
        Try
            Dim dt As DataTable = CType(Session("GData1"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()
            ViewState("PayoutDate") = sColimnName
            ViewState("Sort_Order") = sSortOrder
            GvData.Focus()
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
            Dim sessid As String = "0"
            
            Dim FromSessid As String = "0"
            Dim ToSessid As String = "0"
            If txtStartDate.Text <> "" Then
                Dim inputDate As DateTime = DateTime.ParseExact(txtStartDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                Dim outputDateString As String = inputDate.ToString("yyyyMMdd")

                FromSessid = outputDateString
            Else
                FromSessid = "0"
            End If
            If txtEndDate.Text <> "" Then
                Dim inputDate As DateTime = DateTime.ParseExact(txtEndDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                Dim outputDateString As String = inputDate.ToString("yyyyMMdd")
                ToSessid = outputDateString
            Else
                ToSessid = "0"
            End If

            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.IncentiveDetail(ToSessid, pageIndex)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim sessid As String = "0"
           
            Dim FromSessid As String = "0"
            Dim ToSessid As String = "0"
            If txtStartDate.Text <> "" Then
                Dim inputDate As DateTime = DateTime.ParseExact(txtStartDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                Dim outputDateString As String = inputDate.ToString("yyyyMMdd")

                FromSessid = outputDateString
            Else
                FromSessid = "0"
            End If
            If txtEndDate.Text <> "" Then
                Dim inputDate As DateTime = DateTime.ParseExact(txtEndDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                Dim outputDateString As String = inputDate.ToString("yyyyMMdd")
                ToSessid = outputDateString
            Else
                ToSessid = "0"
            End If

            Me.IncentiveDetail(sessid, 1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData1")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try


            Dim Idno As String = "0"
            Dim sessid As String = "0"
            If txtMemId.Text <> "" Then
                Idno = txtMemId.Text
            Else
                Idno = "0"
            End If
            Dim FromSessid As String = "0"
            Dim ToSessid As String = "0"
            If txtStartDate.Text <> "" Then
                Dim inputDate As DateTime = DateTime.ParseExact(txtStartDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                Dim outputDateString As String = inputDate.ToString("yyyyMMdd")

                FromSessid = outputDateString
            Else
                FromSessid = "0"
            End If
            If txtEndDate.Text <> "" Then
                Dim inputDate As DateTime = DateTime.ParseExact(txtEndDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                Dim outputDateString As String = inputDate.ToString("yyyyMMdd")
                ToSessid = outputDateString
            Else
                ToSessid = "0"
            End If

            

            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@IdNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@FromSessid", FromSessid)
            prms(2) = New SqlParameter("@ToSessid", ToSessid)
            prms(3) = New SqlParameter("@PageIndex", 1)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "Y")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)

            If Session("compid") = "1101" Or Session("compid") = "1103" Then
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetIncentiveDetailReportR", prms)
            Else
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetIncentiveDetailReport", prms)
            End If
            
            Session("GData1") = Ds.Tables(0)
            ExportExcel()
            
        Catch ex As Exception

        End Try
        
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Incentive")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=IncentiveDetail.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

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
   

    Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    End Sub

   
End Class
