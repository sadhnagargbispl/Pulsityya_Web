Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class App_UI_Application_Pages_UserMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "User / New User"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            txtSearch.Text = ""
            Label1.Visible = False

            'Fill Group drop down list
            Dim qry1 As String = "Select * from " & objDAL.tblUserGrpMaster & " Where ActiveStatus='Y' AND RowStatus='Y'"
            objModuleFun.FillCombo(qry1, ddlGroup, "GroupName", "GroupId")

            If String.IsNullOrEmpty(Session("GroupId")) = False Then
                ddlGroup.SelectedValue = Session("GroupId")
            End If

            If Convert.ToInt32(Session("grdIndex")) = 0 Then
            Else
                GvData.PageIndex = Convert.ToInt32(Session("grdIndex"))
            End If
            'Fill grid with data
            BindData()
            Session("GroupId") = ddlGroup.SelectedValue.ToString()
            Session("GroupName") = ddlGroup.SelectedItem.Text
            Session("grpID") = ddlGroup.SelectedValue.ToString()
        End If
    End Sub

    Public Sub BindData()

        Dim qry2 As String = "Select Cast(UserId as varchar) as VUserId,*,'********' as Password,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status from " & objDAL.tblUserMaster & " where GroupId=" & Val(ddlGroup.SelectedValue.ToString()) & " AND rowstatus='Y' Order by UserName"
        dtData = New DataTable
        dtData = objDAL.GetData(qry2)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then

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
        Session("grdIndex") = GvData.PageIndex
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            If String.Equals(e.Row.Cells(5).Text.ToLower(), "deactive") = True Then
                'e.Row.BackColor = Drawing.Color.Red
                ' e.Row.Style("background-image") = "images/redback2.jpg"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "red"
                Next
            End If
        End If
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
                    sql = "Select Cast(UserId as varchar) as VUserId,*,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status from " & objDAL.tblUserMaster & " where GroupId=" & Val(ddlGroup.SelectedValue.ToString()) & " AND rowStatus='Y' AND ActiveStatus like '%" + status.ToString() + "%' Order by UserName"
                End If
            Else
                sql = "Select Cast(UserId as varchar) as VUserId,*,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status from " & objDAL.tblUserMaster & " where GroupId=" & Val(ddlGroup.SelectedValue.ToString()) & " AND rowStatus='Y' AND " & ddlSearchFields.SelectedValue.ToString() & " like '%" & txtSearch.Text & "%'  Order by UserName"
            End If
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
        End If
    End Sub

    Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
        Session("grpID") = ddlGroup.SelectedValue.ToString()
        Session("GroupId") = ddlGroup.SelectedValue.ToString()
        Session("GroupName") = ddlGroup.SelectedItem.Text
        BindData()
    End Sub

    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim userID, scrname As String
        Dim GVRw As GridViewRow
        'Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Conn.Open()
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        userID = DirectCast(GVRw.FindControl("LblUserID"), Label).Text
        Dim Sql As String = "Update " + objDAL.tblUserMaster + " SET ActiveStatus='N',LastModified='De-Activated by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' WHERE UserId='" & userID & "' AND RowStatus='Y'"
        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected User! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "User Deletion", scrname, False)
        BindData()
    End Sub

    Protected Sub BtnShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShowAll.Click
        Dim qry2 As String = "Select Cast(UserId as varchar) as VUserId,*,CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status from " & objDAL.tblUserMaster & " where rowstatus='Y' Order by UserName"
        dtData = New DataTable
        dtData = objDAL.GetData(qry2)
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

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "UserDetails.xls"))
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
End Class
