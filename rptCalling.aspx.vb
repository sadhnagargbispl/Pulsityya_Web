Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel


Partial Class rptCalling
    Inherits System.Web.UI.Page
    Dim Ds As DataSet
    Dim DsDetail As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        searchDetail()
        allCallDetail()
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Calling option"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub searchDetail()
        Try
            Dim prms As SqlParameter() = New SqlParameter(0) {}
            prms(0) = New SqlParameter("@UserId", Session("UserID"))
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetAllSkippedCall", prms)
            If Ds.Tables(0).Rows.Count > 0 Then
                GvData.DataSource = Ds.Tables(0)
                GvData.DataBind()
                Session("SkippedCall") = Ds.Tables(0)
                BtnExportSkipped.Enabled = True
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub allCallDetail()
        Try
            Dim prms As SqlParameter() = New SqlParameter(0) {}
            prms(0) = New SqlParameter("@UserId", Session("UserID"))
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetAllCall", prms)
            If Ds.Tables(0).Rows.Count > 0 Then
                GridView1.DataSource = Ds.Tables(0)
                GridView1.DataBind()
                Session("GetAllCall") = Ds.Tables(0)
                btnExport.Enabled = True
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        ExportExcelAll()
    End Sub
    Protected Sub BtnExportSkipped_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportSkipped.Click
        ExportExcelAllskipped()
    End Sub
    Private Sub ExportExcelAll()
        Dim dt As DataTable = Session("GetAllCall")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Customers")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=CallingReport-" & DateTime.Now.ToString("dd-MMM-yyyy") & ".xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub
    Private Sub ExportExcelAllskipped()
        Dim dt As DataTable = Session("SkippedCall")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "SkippedCall")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=AllSkippedReport-" & DateTime.Now.ToString("dd-MMM-yyyy") & ".xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub
End Class
