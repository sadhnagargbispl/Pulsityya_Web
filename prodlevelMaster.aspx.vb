Imports System.Data
Imports System.IO

Partial Class App_UI_Application_Pages_prodlevelMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then

            Session("PageName") = "Master / Kit Master"
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
            txtSearch.Text = ""
            btnShowRecord.Visible = False
            lblView.Visible = False
            If Session("AStatus") = "OK" Then
                BindData()
            End If
        End If
    End Sub

    Public Sub BindData()
        Dim sql As String = ""

        sql = "select a.prodid,ProductName,level_1,level_2,level_3,level_4,level_5,level_6,level_7,level_8,level_9,level_10,b.ActiveStatus " & _
 " from M_prodLevelMaster as b left join decebalinv..M_productMaster as a on a.prodid=b.prodid"

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

    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim KitId, scrname As String
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        KitId = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        Dim Sql As String = "Update M_prodLevelMaster SET ActiveStatus='N' WHERE prodId='" & Val(KitId.ToString()) & "' "
        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
        BindData()
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Dim sql As String
        Dim status As String
        If String.IsNullOrEmpty(txtSearch.Text) Then
        ElseIf ddlSearchFields.SelectedValue = "showall" Then
            BindData()
        ElseIf String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "showall") = True Then
            BindData()
        Else
            If String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "status") = True Then
                If String.IsNullOrEmpty(txtSearch.Text) = False Then
                    If txtSearch.Text.ToLower().Contains("deactive") = True Then
                        status = "N"
                    Else
                        status = "Y"
                    End If
                    If Session("Compid") <> "1033" Then
                        sql = "Select *,'False' as PlanStatus,'' as Plan1 ,Cast(KitId as varchar) as VKitId,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status From " + objDAL.tblKitMaster + " Where " + objDAL.activeCondition + " AND ActiveStatus like '%" + status.ToString() + "%' and OldKit<>'OLD' Order by KitName"
                    Else
                        sql = "Select *,'True' as PlanStatus,Case when Plantype=0 then 'Choose Plan' when Plantype=1 then 'Plan A' when Plantype=2 then 'Plan B' else '' end as Plan1 ,Cast(KitId as varchar) as VKitId,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status From " + objDAL.tblKitMaster + " Where " + objDAL.activeCondition + " AND ActiveStatus like '%" + status.ToString() + "%' and OldKit<>'OLD' Order by KitName"

                    End If
                End If
            Else
                If Session("Compid") <> "1033" Then
                    sql = "Select *,'False' as PlanStatus,'' as Plan1,Cast(KitId as varchar) as VKitId,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status From " + objDAL.tblKitMaster + " Where " + objDAL.activeCondition + " AND " + ddlSearchFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%' and OldKit<>'OLD' Order by KitName"

                Else

                    sql = "Select *,'True' as PlanStatus,Case when Plantype=0 then 'Choose Plan' when Plantype=1 then 'Plan A' when Plantype=2 then 'Plan B' else '' end as Plan1,Cast(KitId as varchar) as VKitId,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status From " + objDAL.tblKitMaster + " Where " + objDAL.activeCondition + " AND " + ddlSearchFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%' and OldKit<>'OLD' Order by KitName"
                End If
            End If
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            If Session("compid") = "1033" Then
                GvData.Columns(2).Visible = True
            Else
                GvData.Columns(2).Visible = False
            End If
            Session("GData") = dtData
            btnShowRecord.Visible = True
            lblView.Visible = True
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

    

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "KitDetails.xls"))
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

    Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    End Sub
End Class
