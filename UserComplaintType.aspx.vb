Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class UserComplaintType
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" And Session("UserPermission") = 1 Then
        Else
            Response.Redirect("Default.aspx")
        End If
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
        Dim sql As String = "Select a.CTypeId,Cast(a.CTypeId as varchar) as VCTypeId,CType,a.Remarks,CASE WHEN a.ActiveStatus='Y' Then  "
        sql &= " 'Active' ELSE 'DeActive' END AS Status,a.ToUserEmail as UserEmail From " + objDAL.tblCTypeMaster + " as a"
        sql &= " Where    a." + objDAL.activeCondition + " Order by CType"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GrpID, scrname As String
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        Dim Sql As String = "Update " + objDAL.tblCTypeMaster + " SET ActiveStatus='N',LastModified='De-Activated by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' WHERE CTypeId='" & GrpID & "' AND RowStatus='Y'"
        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Complaint Type! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Complaint Type Deletion", scrname, False)
        BindData()
    End Sub
    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Dim sql As String = ""
        Dim status As String = ""
        If ddlGroupFields.SelectedValue = "None" Then
            Exit Sub
        End If
        If String.IsNullOrEmpty(txtSearch.Text) Then
        ElseIf String.Equals(ddlGroupFields.SelectedItem.Text.ToLower(), "showall") = True Then
            BindData()
        Else
            If String.Equals(ddlGroupFields.SelectedItem.Text.ToLower(), "status") = True Then
                If String.IsNullOrEmpty(txtSearch.Text) = False Then
                    If txtSearch.Text.ToLower().Contains("deactive") = True Then
                        status = "N"
                    Else
                        status = "Y"
                    End If
                    sql = "Select CTypeId,Cast(CTypeId as varchar) as VCTypeId,CType,a.Remarks,CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,"
                    sql &= "b.Username,a.ToUserEmail as UserEmail From " + objDAL.tblCTypeMaster + " as a,M_UserMaster as b Where a.ToUserId=b.UserId and b.RowStatus='Y' "
                    sql &= "and b.ActiveStatus='Y' and  a.RowStatus='Y' AND a.ActiveStatus like '%" + status.ToString() + "%' Order by CType"
                End If
            Else
                sql = "Select CTypeId,Cast(CTypeId as varchar) as VCTypeId,CType,a.Remarks,CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status,"
                sql &= "b.Username,a.ToUserEmail as UserEmail From " + objDAL.tblCTypeMaster + " as a,M_UserMaster as b Where a.ToUserId=b.UserId and b.RowStatus='Y' "
                sql &= "and b.ActiveStatus='Y' and  a. " + objDAL.activeCondition + " AND a." + ddlGroupFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%'"
                sql &= " Order by CType"
            End If
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
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
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "ComplaintTypeDetails.xls"))
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
    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
        lblView.Visible = False
        ddlGroupFields.SelectedIndex = 0
        txtSearch.Text = ""
    End Sub
End Class
