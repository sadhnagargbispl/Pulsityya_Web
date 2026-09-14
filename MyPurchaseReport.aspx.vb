Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Xml
Imports System.Web.Script.Serialization
Partial Class MyPurchaseReport
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim dtData As New DataTable
    Dim Ds As DataSet
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.BtnShow.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnShow))
        Try
            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("logout.aspx")
            End If
            If Not Page.IsPostBack Then
                FillReport()

            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub FillReport()
        Dim TransactionID As String = "0"
        Dim WalletAddress As String = ""
        Dim startDate As String
        Dim endDate As String
        Dim currentDate As DateTime = DateTime.Now
        Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")
        If txtMemId.Text <> "" Then
            TransactionID = txtMemId.Text.Trim
        Else
            TransactionID = "0"
        End If

        If txtStartDate.Text = "" Then
            startDate = "12-oct-2017"
        Else
            startDate = txtStartDate.Text
        End If
        If txtEndDate.Text = "" Then
            endDate = formattedDate
        Else
            endDate = txtEndDate.Text
        End If
        Dim sql As String = "exec sp_GetPurchaseReport '" & TransactionID & "','" & startDate & "','" & endDate & "','" & DDlBYSelectUser.SelectedValue & "'"
        dtData = New DataTable
        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).tables(0)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        ViewState("WithDrawDate") = "BankCode"
        ViewState("Sort_Order") = "ASC"
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub

    Private Function DisableTheButton(ByVal pge As Control, ByVal btn As Control) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ")
        sb.Append("this.value = 'Please wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(pge.Page.GetPostBackEventReference(btn))
        sb.Append(";")
        Return sb.ToString()
    End Function
    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        FillReport()
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        FillReport()
    End Sub
    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        If e.SortExpression = ViewState("WithDrawDate").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/up.png", "~/Images/down.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = e.SortExpression Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/up.png", "~/Images/down.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next
            End If

        Else
            RebindData(e.SortExpression, "ASC")
        End If
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("GDataNew"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("WithDrawDate") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim TransactionID As String = "0"
        Dim WalletAddress As String = ""
        Dim startDate As String
        Dim endDate As String
        Dim currentDate As DateTime = DateTime.Now
        Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")
        If txtMemId.Text <> "" Then
            TransactionID = txtMemId.Text.Trim
        Else
            TransactionID = "0"
        End If

        If txtStartDate.Text = "" Then
            startDate = "12-oct-2017"
        Else
            startDate = txtStartDate.Text
        End If
        If txtEndDate.Text = "" Then
            endDate = formattedDate
        Else
            endDate = txtEndDate.Text
        End If
        Dim sql As String = "exec sp_GetPurchaseReport '" & TransactionID & "','" & startDate & "','" & endDate & "','" & DDlBYSelectUser.SelectedValue & "'"
        dtData = New DataTable
        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).tables(0)

        Session("GData") = dtData
        ExportExcel()
        ViewState("WithDrawDate") = "BankCode"
        ViewState("Sort_Order") = "ASC"
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Incentive")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=MyPurchaseReport.xlsx")
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

    Protected Sub DDlBYSelectUser_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlBYSelectUser.TextChanged
        FillReport()
    End Sub
End Class
