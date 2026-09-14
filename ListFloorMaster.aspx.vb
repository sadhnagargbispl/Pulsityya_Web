Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class ListFloorMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try


            If Session("AStatus") = "OK" Then
                Session("PageName") = "Player Master"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            '  objModuleFun = New ModuleFunction()
            If Not Page.IsPostBack Then
                'txtSearch.Text = ""
                BindData()
                Label1.Visible = False
                If Convert.ToInt32(Session("grdIndex")) = 0 Then
                Else
                    GvData.PageIndex = Convert.ToInt32(Session("grdIndex"))
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindData()
        Try
            Dim _Condition As String = ""
            Dim Qry As String = "Select id,Floortype,Status From V#FloortypeMaster Where 1=1" & _Condition & " Order by ID Desc"
            dtData = New DataTable
            dtData = objDAL.GetData(Qry)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            If dtData.Rows.Count > 0 Then
                btnExport.Enabled = True
                BtnPrint.Enabled = True
                ' BtnExportPDF.Enabled = True
                'btnPrintAll.Enabled = True
                'btnPrintCurrent.Enabled = True
            Else
                btnExport.Enabled = False
                BtnPrint.Enabled = False
                'BtnExportPDF.Enabled = False
                'btnPrintCurrent.Enabled = False
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
        Session("grdIndex") = GvData.PageIndex
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            If String.Equals(e.Row.Cells(3).Text.ToLower(), "deactive") = True Then
                'e.Row.BackColor = Drawing.Color.Red
                ' e.Row.Style("background-image") = "images/redback2.jpg"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "red"
                Next
            End If
        End If
    End Sub

    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim userID, scrname As String
        Dim GVRw As GridViewRow
        'Conn = New SqlConnection(Application("Connect"))
        'Conn.Open()
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        userID = DirectCast(GVRw.FindControl("LblID"), Label).Text
        Dim Sql As String = "Update M_FloorMaster SET ActiveStatus='N' WHERE Id='" & userID & "' "
        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deactivate Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected record! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "User Deletion", scrname, False)
        BindData()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try


            Dim Qry As String = "Select Floortype,Status From V#FloortypeMaster Where 1=1 Order by ID Desc"
            dtData = New DataTable
            dtData = objDAL.GetData(Qry)
            ExportToExcel(dtData, "FloorType")
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub ExportToExcel(ByVal dt As DataTable, ByVal filename As String)
        '  Dim dt As DataTable = Session("MemberList1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Report")
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.Buffer = True
            HttpContext.Current.Response.Charset = ""
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" & filename & ".xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream)
                HttpContext.Current.Response.Flush()
                HttpContext.Current.Response.End()
            End Using
        End Using
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub




    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        GvData.BorderStyle = BorderStyle.Solid
        ' gridview.BorderWidth = 
        GvData.BorderColor = Drawing.Color.Black

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            '   GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
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
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    'Protected Sub BtnExportPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportPDF.Click
    '    Dim Dt As New DataTable
    '    Dt = Session("GData")
    '    export.ExportTopdf(Dt, "DepoList")
    'End Sub
End Class
