Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports OfficeOpenXml
Public Class UploadExcel
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim Dt As New DataTable
    Dim objDAL As DAL
    'Dim objModuleFun As New ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim str As String = ""
    Dim Condition As String = ""
    Dim Condition2 As String = ""
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Private uploadFolder As String = ""
    Private ConnectionString As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Try
                uploadFolder = Server.MapPath("~/Uploads/1010")
                If Not Directory.Exists(uploadFolder) Then
                    Directory.CreateDirectory(uploadFolder)
                End If
                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Not Page.IsPostBack Then
                    If Session("AStatus") = "OK" Then
                        ConnectionString = HttpContext.Current.Session("MlmDatabase" & Session("CompID"))
                    Else
                        Response.Redirect("Default.aspx")
                    End If
                End If
            Catch ex As Exception
            End Try
        Catch ex As Exception
        End Try
        ' Get DB connection string from session



    End Sub

    Private Function ReadExcelStream(ByVal fileUpload As FileUpload) As DataTable
        Dim dt As New DataTable()

        Using package As New ExcelPackage(fileUpload.PostedFile.InputStream)
            Dim worksheet = package.Workbook.Worksheets.First()
            Dim hasHeader As Boolean = True ' True agar first row header hai

            ' Create columns
            For col = 1 To worksheet.Dimension.End.Column
                dt.Columns.Add(If(hasHeader, worksheet.Cells(1, col).Text, "Column" & col))
            Next

            ' Fill rows
            Dim startRow = If(hasHeader, 2, 1)
            For row = startRow To worksheet.Dimension.End.Row
                Dim dr = dt.NewRow()
                For col = 1 To worksheet.Dimension.End.Column
                    dr(col - 1) = worksheet.Cells(row, col).Text
                Next
                dt.Rows.Add(dr)
            Next
        End Using

        Return dt
    End Function


    Private Function CheckRowsExistence(ByVal dt As DataTable) As DataTable
        ' Add validation column
        If Not dt.Columns.Contains("IsValid") Then
            dt.Columns.Add("IsValid", GetType(Boolean))
        End If

        Using con As New SqlConnection(ConnectionString)
            con.Open()
            For Each row As DataRow In dt.Rows
                Dim idValue As String = row("ID").ToString().Trim()
                Dim query As String = "SELECT COUNT(1) FROM YourTable WHERE ID = @ID" ' Change table/column

                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@ID", idValue)
                    Dim exists As Boolean = Convert.ToInt32(cmd.ExecuteScalar()) > 0
                    row("IsValid") = exists
                End Using
            Next
        End Using

        Return dt
    End Function
    Protected Function IsLastDateOfMonth(ByVal selectedDate As Date) As Boolean
        Dim year As Integer = selectedDate.Year
        Dim month As Integer = selectedDate.Month
        Dim lastDate As Integer = Date.DaysInMonth(year, month)
        Return selectedDate.Day = lastDate
    End Function
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs)
        lblMessage.Text = ""

        If Not FileUpload1.HasFile Then
            lblMessage.Text = "Please select an Excel file."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If
        If txtStartDate.Text = "" Then
            lblMessage.Text = "Please Select Payout Date."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If
        Dim userInput As String = txtStartDate.Text.Trim()
        Dim selectedDate As Date
        If Date.TryParseExact(userInput, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, selectedDate) Then
            If IsLastDateOfMonth(selectedDate) Then
            Else
                lblMessage.Text = "Please select only the last date of the month."
                lblMessage.ForeColor = System.Drawing.Color.Red
                txtStartDate.Text = ""
                Return
            End If
        Else
            lblMessage.Text = "Invalid date format."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If

        Dim ext As String = IO.Path.GetExtension(FileUpload1.FileName).ToLower()
        Dim randomFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") & "_" & New Random().Next(1000, 9999) & ext
        Dim savedPath As String = Path.Combine(uploadFolder, randomFileName)

        Try
            ' --- Save file to server ---
            FileUpload1.SaveAs(savedPath)

            Dim dt As New DataTable()

            If ext = ".xlsx" Then
                ' --- EPPlus for .xlsx ---
                Using package As New ExcelPackage(New FileInfo(savedPath))
                    'Dim ws = package.Workbook.Worksheets(0)
                    If package.Workbook.Worksheets.Count = 0 Then
                        lblMessage.Text = "No worksheet found in Excel file."
                        Exit Sub
                    End If

                    Dim ws As ExcelWorksheet = package.Workbook.Worksheets(1) ' First worksheet
                    ' Header
                    For col = 1 To ws.Dimension.End.Column
                        dt.Columns.Add(ws.Cells(1, col).Text)
                    Next

                    ' Rows
                    For row = 2 To ws.Dimension.End.Row
                        Dim dr = dt.NewRow()
                        For col = 1 To ws.Dimension.End.Column
                            dr(col - 1) = ws.Cells(row, col).Text
                        Next
                        dt.Rows.Add(dr)
                    Next
                End Using
                'ElseIf ext = ".xls" Then
                '    ' --- OLEDB for .xls ---
                '    'Dim connStr As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & savedPath & ";Extended Properties=""Excel 8.0;HDR=YES;IMEX=1"""
                '    Dim connStr As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & savedPath & ";Extended Properties=""Excel 8.0;HDR=YES;IMEX=1"""
                '    Using conn As New OleDbConnection(connStr)
                '        conn.Open()
                '        Dim dtSheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                '        Dim sheetName As String = dtSheets.Rows(0)("TABLE_NAME").ToString()

                '        Dim da As New OleDbDataAdapter("SELECT * FROM [" & sheetName & "]", conn)
                '        da.Fill(dt)
                '    End Using
            Else
                lblMessage.Text = "Only .xlsx files are supported."
                lblMessage.ForeColor = System.Drawing.Color.Red
                Return
            End If

            ' --- Check existence in DB ---
            If Not dt.Columns.Contains("IsValid") Then
                dt.Columns.Add("IsValid", GetType(Boolean))
            End If

            Using con As New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                con.Open()
                For Each row As DataRow In dt.Rows
                    Dim idValue As String = row("ID NO.").ToString().Trim()
                    Dim query As String = "SELECT COUNT(1) FROM m_membermaster WHERE formno = @ID"

                    Using cmd As New SqlCommand(query, con)
                        cmd.Parameters.AddWithValue("@ID", idValue)
                        Dim exists As Boolean = Convert.ToInt32(cmd.ExecuteScalar()) > 0
                        row("IsValid") = exists
                    End Using
                Next
            End Using

            ' --- Bind and highlight invalid rows ---
            gvPreview.DataSource = dt
            gvPreview.DataBind()
            btnSubmit.Visible = True
            For i As Integer = 0 To gvPreview.Rows.Count - 1
                Dim isValid As Boolean = Convert.ToBoolean(dt.Rows(i)("IsValid"))
                If Not isValid Then
                    gvPreview.Rows(i).BackColor = System.Drawing.Color.Red
                End If
            Next

            ' --- Save in ViewState for submit ---
            ViewState("PreviewData") = dt
            lblMessage.Text = "File uploaded, checked, and preview ready."
            btnUpload.Enabled = False
            FileUpload1.Enabled = False
            txtStartDate.Enabled = False
        Catch ex As Exception
            lblMessage.Text = "Error: " & ex.Message
        End Try
    End Sub

    'Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    lblMessage.Text = ""

    '    If Not FileUpload1.HasFile Then
    '        lblMessage.Text = "Please select an Excel file."
    '        Return
    '    End If

    '    Dim ext As String = IO.Path.GetExtension(FileUpload1.FileName).ToLower()

    '    Try
    '        If ext = ".xlsx" Then
    '            ' --- EPPlus for .xlsx ---
    '            Using package As New ExcelPackage(FileUpload1.PostedFile.InputStream)
    '                Dim ws = package.Workbook.Worksheets(0)
    '                Dim dt As New DataTable()

    '                ' Header
    '                For col = 1 To ws.Dimension.End.Column
    '                    dt.Columns.Add(ws.Cells(1, col).Text)
    '                Next

    '                ' Rows
    '                For row = 2 To ws.Dimension.End.Row
    '                    Dim dr = dt.NewRow()
    '                    For col = 1 To ws.Dimension.End.Column
    '                        dr(col - 1) = ws.Cells(row, col).Text
    '                    Next
    '                    dt.Rows.Add(dr)
    '                Next

    '                gvPreview.DataSource = dt
    '                gvPreview.DataBind()
    '                lblMessage.Text = "Excel data loaded successfully (.xlsx)."
    '            End Using

    '        ElseIf ext = ".xls" Then
    '            ' --- OLEDB for .xls ---
    '            Dim filePath As String = Server.MapPath("~/Uploads/1010/") & FileUpload1.FileName
    '            FileUpload1.SaveAs(filePath)

    '            Dim connStr As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
    '                "Data Source=" & filePath & ";" & _
    '                "Extended Properties=""Excel 8.0;HDR=YES;IMEX=1"""

    '            Using conn As New OleDbConnection(connStr)
    '                conn.Open()
    '                Dim dtSheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
    '                Dim sheetName As String = dtSheets.Rows(0)("TABLE_NAME").ToString()

    '                Dim da As New OleDbDataAdapter("SELECT * FROM [" & sheetName & "]", conn)
    '                Dim dt As New DataTable()
    '                da.Fill(dt)
    '                gvPreview.DataSource = dt
    '                gvPreview.DataBind()
    '                lblMessage.Text = "Excel data loaded successfully (.xls)."
    '            End Using
    '        Else
    '            lblMessage.Text = "Only .xls or .xlsx files are supported."
    '        End If

    '    Catch ex As Exception
    '        lblMessage.Text = "Error: " & ex.Message
    '    End Try
    'End Sub

    Protected Sub gvPreview_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim isValid As Boolean = Convert.ToBoolean(drv("IsValid"))

            If Not isValid Then
                e.Row.CssClass = "invalid-row"
                e.Row.ToolTip = "This row does not exist in database"
                lblMessage.ForeColor = System.Drawing.Color.Red
            End If
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        lblMessage.Text = ""
        Dim dtChecked As DataTable = TryCast(ViewState("PreviewData"), DataTable)
        If dtChecked Is Nothing Then
            lblMessage.Text = "Please upload and validate Excel data first."
            lblMessage.ForeColor = System.Drawing.Color.Red
            Return
        End If
        Try
            Using con As New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                con.Open()
                Dim rowsInserted As Integer = 0
                For Each row As DataRow In dtChecked.Rows
                    If Convert.ToBoolean(row("IsValid")) Then
                        Dim query As String = "INSERT INTO RepurchaseData(SrNo, Name, IDNo, Amount, BV, BVValue,payoutdate,billtype,Msessid,Sessid)"
                        query &= "VALUES(@SrNo, @Name, @IDNo, @Amount, @BV, @BVValue,@payoutdate,@billtype,"
                        query &= "(SELECT TOP 1 SessID FROM M_MonthSessnMaster  WHERE todate = @payoutdate  ORDER BY SessID DESC)"
                        query &= ",(select top 1 SessID from m_sessnmaster where frmdate <= @payoutdate AND todate >= @payoutdate order by sessid desc))"
                        Using cmd As New SqlCommand(query, con)
                            cmd.Parameters.AddWithValue("@SrNo", row("SR. NO."))
                            cmd.Parameters.AddWithValue("@Name", row("Name"))
                            cmd.Parameters.AddWithValue("@IDNo", row("ID No."))
                            cmd.Parameters.AddWithValue("@Amount", row("Amount"))
                            cmd.Parameters.AddWithValue("@BV", row("BV").Replace("%", ""))
                            cmd.Parameters.AddWithValue("@BVValue", row("BV Value"))
                            cmd.Parameters.AddWithValue("@payoutdate", txtStartDate.Text)
                            cmd.Parameters.AddWithValue("@billtype", "R")
                            cmd.ExecuteNonQuery()
                        End Using
                        rowsInserted += 1
                    End If
                Next
                lblMessage.Text = rowsInserted & " valid rows Payout successfully."
                If rowsInserted > 0 Then
                    Dim query As String = "Exec Sp_ExPayout"
                    Using cmd As New SqlCommand(query, con)
                        cmd.ExecuteNonQuery()
                    End Using
                End If
                Dim scrname = "<SCRIPT language='javascript'>alert('" & lblMessage.Text & "');;location.replace('UploadExcel.aspx');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                gvPreview.DataSource = Nothing
                gvPreview.DataBind()
                btnSubmit.Visible = False
                txtStartDate.Text = ""
                ViewState("PreviewData") = Nothing
            End Using

        Catch ex As Exception
            lblMessage.Text = "Error during insert: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim filePath As String = Server.MapPath("~/Uploads/1010/SampleFileDownLoad.xlsx") ' Yahan file ka real path den
        If System.IO.File.Exists(filePath) Then
            Response.Clear()
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("Content-Disposition", "attachment; filename=SampleFileDownLoad.xlsx")
            Response.WriteFile(filePath)
            Response.Flush()
            Response.End()
        Else
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Sample file not found!');", True)
        End If
    End Sub
End Class
