Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class rptCallingSummary
    Inherits System.Web.UI.Page
    Dim Ds As DataSet
    Dim DsDetail As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        allCallDetail()
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Calling option"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub allCallDetail()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetCallReport")
            If Ds.Tables(0).Rows.Count > 0 Then
                GridView1.DataSource = Ds.Tables(0)
                GridView1.DataBind()
                btnExport.Enabled = True
                Session("CallSummary") = Ds.Tables(0)
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
        Dim dt As DataTable = Session("CallSummary")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "CallSummary")
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
        Dim dt As DataTable = Session("CallDetail")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "CallDetail")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=CallDetails-" & DateTime.Now.ToString("dd-MMM-yyyy") & ".xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub
    Protected Sub grdDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Try
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim rowIndex As Integer = gvr.RowIndex
            If e.CommandName = "Total" Then
                Dim prms As SqlParameter() = New SqlParameter(0) {}
                prms(0) = New SqlParameter("@ID", 1)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetCallDtail", prms)
                If Ds.Tables(0).Rows.Count > 0 Then
                    GvData.DataSource = Ds.Tables(0)
                    GvData.DataBind()
                    Session("CallDetail") = Ds.Tables(0)
                    BtnExportSkipped.Enabled = True
                    mp1.Show()
                End If
            ElseIf e.CommandName = "BookedProduct" Then
                Dim prms As SqlParameter() = New SqlParameter(0) {}
                prms(0) = New SqlParameter("@ID", 2)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetCallDtail", prms)
                If Ds.Tables(0).Rows.Count > 0 Then
                    GvData.DataSource = Ds.Tables(0)
                    GvData.DataBind()
                    Session("CallDetail") = Ds.Tables(0)
                    BtnExportSkipped.Enabled = True
                    mp1.Show()
                End If
            ElseIf e.CommandName = "CallYesterday" Then
                Dim prms As SqlParameter() = New SqlParameter(0) {}
                prms(0) = New SqlParameter("@ID", 3)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetCallDtail", prms)
                If Ds.Tables(0).Rows.Count > 0 Then
                    GvData.DataSource = Ds.Tables(0)
                    GvData.DataBind()
                    Session("CallDetail") = Ds.Tables(0)
                    BtnExportSkipped.Enabled = True
                    mp1.Show()
                End If
            ElseIf e.CommandName = "BookedYesterday" Then
                Dim prms As SqlParameter() = New SqlParameter(0) {}
                prms(0) = New SqlParameter("@ID", 4)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetCallDtail", prms)
                If Ds.Tables(0).Rows.Count > 0 Then
                    GvData.DataSource = Ds.Tables(0)
                    GvData.DataBind()
                    Session("CallDetail") = Ds.Tables(0)
                    BtnExportSkipped.Enabled = True
                    mp1.Show()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Product Request", "error occured", False)
        End Try
    End Sub
    
End Class
