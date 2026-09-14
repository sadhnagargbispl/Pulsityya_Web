Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class App_UI_Application_Pages_DispatchdetailMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then

            Session("PageName") = "Member / Dispatch Detail Master"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
        lblView.Visible = False
        txtSearch.Text = ""
        ddlSearchFields.SelectedIndex = 0
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'GvData.PageIndex = Session("index")
            txtSearch.Text = ""
            btnShowRecord.Visible = False
            lblView.Visible = False
            If Session("AStatus") = "OK" Then
                BindData()
                'DeactivateRecord()
            End If
        End If
    End Sub
    Protected Sub DeactivateRecord()
        'Dim NewsId As String = ""
        ' Dim sql As String = "select * from M_NewsSeminarmaster where convert(datetime,dbo.formatdate(TODATE,'dd-MMM-yyyy'),1)<=Cast(Convert(varchar,GETDATE(),106) as Datetime) "
        ' dtData = New DataTable
        ' dtData = objDAL.GetData(sql)
        ' If dtData.Rows.Count > 0 Then
        'NewsId = dtData.Rows(0)("NewsId")
        Dim str As String = ""
        str = "Update " & objDAL.tbldispatchMaster & " Set ActiveStatus='N' where convert(datetime,dbo.formatdate(DATE,'dd-MMM-yyyy'),1)<=Cast(Convert(varchar,GETDATE(),106) as Datetime) "
        dtData = New DataTable
        objDAL.UpdateData(str)

        '  End If
    End Sub



    Public Sub BindData()
        'Dim sql As String = "Select a.Type,Cast(b.NewsId as varchar) as VNewsId,b.NewsHdr,b.NewsDtl,dbo.FormatDate(b.FrmDate,'dd-MM-yyyy') as FrmDate,dbo.FormatDate(ToDate,'dd-MM-yyyy') as ToDate,b.Remarks,b.NewsId  ,CASE WHEN b.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status From " + objDAL.tblNewsTypeMaster + " as a," + objDAL.tblNewsMaster + " as b Where a.RowStatus='Y' AND b.RowStatus='Y' AND a.Prefix=b.NType   Order by NewsHdr "
        Dim sql As String = "Select a.DId as VDId,a.Did,a.Id,a.Idno,b.kitname,dbo.FormatDate(date,'dd-MM-yyyy') as Date,a.Remarks,CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,Case when a.ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass From " + objDAL.tbldispatchMaster + " as a inner join M_kitmaster as b on a.kitid=b.kitid Where a.RowStatus='Y' Order by Did "
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
            btnPrintAll.Enabled = True
            btnPrintCurrent.Enabled = True
        Else
            btnExport.Enabled = False
            btnPrintAll.Enabled = False
            btnPrintCurrent.Enabled = False
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub DeleteNews(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim NewsID, scrname As String
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        NewsID = DirectCast(GVRw.FindControl("LblNewsID"), Label).Text
        Dim Sql As String = "Update " + objDAL.tbldispatchMaster + " SET ActiveStatus='N' WHERE Id='" & NewsID & "' AND RowStatus='Y'"
        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Dispatch Deletion", scrname, False)
        BindData()
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Dim sql As String
        Dim status As String
        If String.IsNullOrEmpty(txtSearch.Text) Then
        ElseIf String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "showall") = True Then
            BindData()
        ElseIf ddlSearchFields.SelectedValue = "showall" Then
            BindData()
        Else
            If String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "status") = True Then
                If String.IsNullOrEmpty(txtSearch.Text) = False Then
                    If txtSearch.Text.ToLower().Contains("deactive") = True Then
                        status = "N"
                    Else
                        status = "Y"
                    End If
                    'sql = " Select DId as VDId,Id,Did,Idno,dbo.FormatDate(Date,'dd-MM-yyyy') as Date,Remarks,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,Case when ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass From " + objDAL.tbldispatchMaster + " Where RowStatus='Y' AND ActiveStatus like '%" + status.ToString() + "%' Order by Did"
                    sql = "Select  a.DId as VDId,a.Did,a.Id,a.Idno,b.kitname,dbo.FormatDate(date,'dd-MM-yyyy') as Date,a.Remarks,CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,Case when a.ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass From " + objDAL.tbldispatchMaster + " as a inner join M_kitmaster as b on a.kitid=b.kitid Where A.RowStatus='Y' AND a.ActiveStatus like '%" + status.ToString() + "%' Order by a.Did"
                End If
            Else
                'sql = "Select DId as VDId,Id,Did,Idno,dbo.FormatDate(Date,'dd-MM-yyyy') as Date,Remarks,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,Case when ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass From " + objDAL.tbldispatchMaster + " Where RowStatus='Y' AND " + ddlSearchFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%' Order by Did"
                sql = "Select a.DId as VDId,a.Did,a.Id,a.Idno,b.kitname,dbo.FormatDate(date,'dd-MM-yyyy') as Date,a.Remarks,CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,Case when a.ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass From " + objDAL.tbldispatchMaster + " as a inner join M_kitmaster as b on a.kitid=b.kitid Where A.RowStatus='Y' AND a." + ddlSearchFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%' Order by a.Did"
            End If
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            btnShowRecord.Visible = True
            'lblView.Visible = True

            If dtData.Rows.Count > 0 Then
                btnExport.Enabled = True
                btnPrintAll.Enabled = True
                btnPrintCurrent.Enabled = True
            Else
                btnExport.Enabled = False
                btnPrintAll.Enabled = False
                btnPrintCurrent.Enabled = False
            End If
        End If
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' If String.Equals(e.Row.Cells(6).Text.ToLower(), "deactive") = True Then
            If String.Equals(e.Row.Cells(5).Text.ToLower(), "deactive") = True Then
                'e.Row.BackColor = Drawing.Color.Red
                e.Row.Style("background-image") = "../Resources/Images/redback2.jpg"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "red"
                Next
            End If
        End If
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "DispatchDetails.xls"))
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
        'BindGridview()
        'Change the Header Row back to white color
        GvData.HeaderRow.Style.Add("background-color", "#FFFFFF")
        GvData.RowStyle.ForeColor = Drawing.Color.Black
        'Applying stlye to gridview header cells
        For i As Integer = 0 To GvData.HeaderRow.Cells.Count - 1
            GvData.HeaderRow.Cells(i).Style.Add("background-color", "#D8A38D")
        Next

        'Remove modify and Delete columns from grid
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        For i As Integer = 0 To GvData.Rows.Count - 1
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
            GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        Next

        For i As Integer = 0 To GvData.Rows.Count - 1
            For j As Integer = 0 To GvData.Rows(i).Cells.Count - 1
                GvData.Rows(i).Cells(j).Style.Add("background-color", "#FFFFFF")
                GvData.Rows(i).Cells(j).Style.Add("color", "#000000")
            Next
        Next

        GvData.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.[End]()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub

    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
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
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
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
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

End Class
