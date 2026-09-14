Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class MisReportBS
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
                'Session("PageName") = "Member / Update Member Profile"
                '' UserStatus.InnerHtml = "<p>Welcome " & Session("MemName") & "(" & Session("FormNo") & ") To" & Session("CompName") & " </p>"
            Else
                Response.Redirect("logout.aspx")
            End If
            If Not Page.IsPostBack Then

                Filldate()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Filldate()
        Try

            Dim dtData As DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = objDAL.GetData(Str)
            If dtData.Rows.Count > 0 Then
                TxtToDate.Text = dtData.Rows(0)("CurrentDate")

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillReport()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String = String.Empty
            sql = " exec Sp_KitMisDateWise '" & TxtToDate.Text.ToString().Trim() & "'"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            If (dt.Rows.Count > 0) Then
                GvData.DataSource = dt
                GvData.DataBind()
            Else
                GvData.DataSource = dt
                GvData.DataBind()

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub PayOutMis()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String = String.Empty
            sql = " exec Sp_ShowPayoutMis '" & TxtToDate.Text.ToString().Trim() & "'"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            If (dt.Rows.Count > 0) Then
                gvpay.DataSource = dt
                gvpay.DataBind()
            Else
                gvpay.DataSource = dt
                gvpay.DataBind()

            End If
        Catch ex As Exception

        End Try
    End Sub











    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            FillReport()
            PayOutMis()
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'Try
        '    Dim dtTemp As New DataTable
        '    Dim dg As New DataGrid
        '    Dim Condition As String = ""
        '    Dim formno As String = ""
        '    Dim scrName As String = ""
        '    '     Dim condition As String = ""


        '    Dim qry1 As String = ""
        '    qry1 = "exec Sp_KitMisDateWise '" & TxtToDate.Text.ToString().Trim() & "'"
        '    dtTemp = New DataTable
        '    dtTemp = objDAL.GetData(qry1)

        '    dg.DataSource = dtTemp
        '    dg.DataBind()
        '    ExportToExcel("Mis.xls", dg)

        'Catch ex As Exception
        '    Response.Write(ex.Message & "Error In Exporting File")

        'End Try


        Dim constr As String = HttpContext.Current.Session("MlmDatabase" & Session("CompID"))
        Dim query As String = "  exec Sp_KitMisDateWise '" & TxtToDate.Text.ToString().Trim() & "'"
        query &= " exec Sp_ShowPayoutMis '" & TxtToDate.Text.ToString().Trim() & "'"
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(query)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using ds As New DataSet()
                        sda.Fill(ds)

                        'Set Name of DataTables.
                        ds.Tables(0).TableName = "KitMis"
                        ds.Tables(1).TableName = "PayoutMis"

                        Using wb As New XLWorkbook()
                            For Each dt As DataTable In ds.Tables
                                'Add DataTable as Worksheet.
                                wb.Worksheets.Add(dt)
                            Next

                            'Export the Excel file.
                            Response.Clear()
                            Response.Buffer = True
                            Response.Charset = ""
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            Response.AddHeader("content-disposition", "attachment;filename=Mis.xlsx")
                            Using MyMemoryStream As New MemoryStream()
                                wb.SaveAs(MyMemoryStream)
                                MyMemoryStream.WriteTo(Response.OutputStream)
                                Response.Flush()
                                Response.End()
                            End Using
                        End Using
                    End Using
                End Using
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


    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        Try
            GvData.PageIndex = e.NewPageIndex
            FillReport()
        Catch ex As Exception

        End Try
    End Sub


End Class
