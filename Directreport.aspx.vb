Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_Directreport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Ad As SqlDataAdapter
    Dim dt As DataTable
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
    Private Function get_FormNo(ByVal IDNo As String) As String
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        Dim FormNo As String = ""

        Dim dr As SqlDataReader
        Comm = New SqlCommand("Select FormNo From M_MemberMaster Where IDNo='" & IDNo & "'", Conn)
        dr = Comm.ExecuteReader
        If dr.Read = True Then
            IDNo = dr("FormNo")
        End If
        dr.Close()
        Comm.Cancel()
        Conn.Close()
        Return IDNo
    End Function



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                GvData.Visible = True
                gvContainer.Visible = True
                Session("IssuedPinValue") = Nothing

                Filldate()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Filldate()
        Try
            objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
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


    Public Sub BindData(ByVal PageIndex As Integer)
        Try


            Dim userid As String = "0"
            Dim Directmember As String = ""

            If txtMemberId.Text <> "" Then
                userid = txtMemberId.Text
            Else
                userid = 0
            End If
            If Txtnumber.Text <> "" Then
                Directmember = Txtnumber.Text
            Else
                Directmember = 0
            End If




            Dim startDate As Date
            Dim endDate As Date
            Dim startDate1 As Date
            Dim endDate1 As Date
            If txtStartDate.Text = "" Then
                startDate = "12-Feb-2010 "
                startDate1 = "12-Feb-2010 "
            Else
                startDate = txtStartDate.Text
                startDate1 = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
                endDate1 = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
                endDate1 = txtEndDate.Text
            End If
            'startDate = Session("StartDate")
            'endDate = Session("endDate")
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(7) {}
            prms(0) = New SqlParameter("@UserId", Convert.ToString(userid).ToLower())
            prms(1) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(2) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(3) = New SqlParameter("@Directmember", Convert.ToString(Directmember).ToLower())
            prms(4) = New SqlParameter("@PageIndex", PageIndex)
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(6) = New SqlParameter("@IsExport", "N")
            prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetDirectReport", prms)
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
            startDate1 = Request("Startdate")
            endDate1 = Request("endDate")
            Me.PopulatePager(recordCount, PageIndex)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click

        Try

            BindData(1)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("IssuedPinValue")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim userid As String = "0"
            Dim Directmember As String = ""

            If txtMemberId.Text <> "" Then
                userid = txtMemberId.Text

            Else
                userid = "0"
            End If
            If Txtnumber.Text <> "" Then
                Directmember = Txtnumber.Text
            Else
                Directmember = 0
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
            prms(0) = New SqlParameter("@UserId", Convert.ToString(userid).ToLower())
            prms(1) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(2) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(3) = New SqlParameter("@Directmember", Convert.ToString(Directmember).ToLower())
            prms(4) = New SqlParameter("@PageIndex", 1)
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(6) = New SqlParameter("@IsExport", "Y")
            prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetDirectReport", prms)
            GvData.DataSource = Ds.Tables(0)
            Session("IssuedPinValue1") = Ds.Tables(0)


            ExportExcel()
        Catch ex As Exception

        End Try
    End Sub




    Private Sub ExportExcel()
        Dim dt As DataTable = Session("IssuedPinValue1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "IssuedEpinValue")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=DirectMemberReport.xlsx")
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



End Class
