Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class App_UI_Application_Pages_SeminarMasterAll
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then

            Session("PageName") = "Master / Seminar Master"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
        lblView.Visible = False
        ddlSearchFields.SelectedIndex = 0
        txtSearch.Text = ""
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

    Public Sub BindData(Optional ByVal Condition As String = "")
        Dim sql As String = "Select ROW_NUMBER() Over(Order by AID DESC) As SNo,CASE WHEN a.UserType='A' THEN 'Admin' Else M.IDNO+ ' ('+M.MemFirstName+')'  END as UserID,MeetingID,Cast(MeetingID as varchar) as VMeetingID,dbo.FormatDate(MeetingTime,'dd-MM-yyyy') As Date,CONVERT(varchar(15),CAST(MeetingTime AS TIME),100) As Time,CityName,DistrictName,B.StateName,A.Address as Venue,Program,Speaker,A.ContactPerson ,A.ContactNo ,A.Remarks As Description," & _
        " CASE WHEN A.ActiveStatus='Y' THEN 'Active' ELSE 'DeActive' END AS Status," & _
        " Case when A.ActiveStatus='N' then 'label label-danger' else 'label label-success' end as StatusClass From M_StateDivMaster As B,M_MeetingMaster As A LEFT JOIN M_MemberMaster as M ON A.UserID=M.FormNo Where A.StateCode=B.StateCode " & Condition & " ORDER BY MeetingTime DESC"
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
        Dim MCode, scrname As String
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        MCode = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        Dim Sql As String = "Update M_MeetingMaster SET ActiveStatus='N',LastModified='De-Activated by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' WHERE MeetingID='" & Val(MCode.ToString()) & "'"
        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Meeting Deletion", scrname, False)
        BindData()
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Dim Condition As String = ""
        If String.IsNullOrEmpty(txtSearch.Text) Or ddlSearchFields.SelectedValue = "0" Then
            Exit Sub
        ElseIf String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "showall") = True Then
            Condition = ""
        ElseIf String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "status") = True Then
            If String.IsNullOrEmpty(txtSearch.Text) = False Then
                If txtSearch.Text.ToLower().Contains("deactive") = True Then
                    Condition = " AND a.ActiveStatus='N'"
                Else
                    Condition = " AND a.ActiveStatus='Y'"
                End If
            End If
        ElseIf String.Equals(ddlSearchFields.SelectedValue.ToLower(), "remarks") = True Then
            Condition = " AND a.Remarks like '%" & Trim(txtSearch.Text) & "%'"
        Else
            Condition = " AND " & ddlSearchFields.SelectedValue & " like '%" & Trim(txtSearch.Text) & "%'"
        End If
        BindData(Condition)
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            If String.Equals(e.Row.Cells(14).Text.ToLower(), "deactive") = True Then
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
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "SeminarDetails.xls"))
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
