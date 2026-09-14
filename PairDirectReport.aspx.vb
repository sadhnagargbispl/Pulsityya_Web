Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_PairDirectReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Member / Paid Direct Report"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim searchtext As String = Session("Search")
        If Not Page.IsPostBack Then
            GvData.Visible = False
            gvContainer.Visible = False
            Session("PairData") = Nothing

            FillKit()
        End If
    End Sub
    Private Sub FillKit()
        Dim sql As String = "Select KitName,KitID From " + objDAL.tblKitMaster + " Where " + objDAL.activeCondition + " Order by KitName"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            CmbKit.DataSource = dtData
            CmbKit.DataTextField = "KitName"
            CmbKit.DataValueField = "KitID"
            CmbKit.DataBind()
        End If
    End Sub



    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = Trim(TxtSearch.Text)
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formNo = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            TxtSearch.Text = ""
        End If
        Return formNo
    End Function

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("PairData")
        GvData.DataBind()
    End Sub

    'Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
    '    Dim i As Integer
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        If String.Equals(e.Row.Cells(30).Text.ToLower(), "deactive") = True Then
    '            'e.Row.BackColor = Drawing.Color.Red
    '            e.Row.Style("background-image") = "../Resources/Images/redback2.jpg"
    '            For i = 0 To e.Row.Cells.Count - 1
    '                e.Row.Cells(i).Style("color") = "whitesmoke"
    '            Next
    '        End If
    '    End If
    'End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "PairDataDetails.xls"))
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("PairData")
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
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        'For i As Integer = 0 To GvData.Rows.Count - 1
        '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        'Next

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

    

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblErr.Text = ""
        lblCount.Text = ""
        Dim Condition As String = ""
        Dim formno As String
        If TxtSearch.Text <> "" Then
            formno = GetFormNo()
            
            Condition = Condition & " and a.RefFormno='" & formno & "'"

        End If
        If ChkKit.Checked Then
            Condition = Condition & " And a.kitId='" & CmbKit.SelectedValue & "'"
        End If
        If DDlDate.SelectedValue = "J" Then
            If txtStartDate.Text <> "" Then
                Condition = Condition & " and cast(a.Doj as Date)>='" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " and cast(a.Doj as Date)<='" & txtEndDate.Text & "'"
            End If
        End If
        If DDlDate.SelectedValue = "A" Then
            Condition = Condition & "and a.ActiveStatus='Y'"
            If txtStartDate.Text <> "" Then
                Condition = Condition & "  and cast(a.UpgradeDate as Date)>='" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & "and cast(a.UpgradeDate as Date)<='" & txtEndDate.Text & "'"
            End If
        End If
        If Condition <> "" Then
            Dim qry1 As String = ""
           
            qry1 = " select a.IdNo as [Login Id],(a.MemFirstName+' '+a.MemLastName) as [Login Name],Replace(convert(Varchar,a.Doj,106),' ','-')as [Joining Date]," & _
                   " Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') else  '' end as [Activation Date], " & _
                   " Case when a.Legno=1 then 'Left' else 'Right' end as Position,c.KitName as [Package Name]," & _
                   " c.KitAmount as [Package Amount] from M_Kitmaster as c ,M_Membermaster as a " & _
                   " where a.KitId=c.KitId    and c.RowStatus='Y' " & Condition & " "

            dtData = New DataTable
            dtData = objDAL.GetData(qry1)
            If dtData.Rows.Count > 0 Then
                GvData.DataSource = dtData
                GvData.DataBind()
                Session("PairData") = dtData
                GvData.Visible = True
                gvContainer.Visible = True
                lblCount.Text = "Total : " & dtData.Rows.Count
            Else
                GvData.Visible = False
                gvContainer.Visible = False
                lblErr.Text = "No Record Found!!"
            End If
        Else
            lblErr.Text = "No Search Criteria!"
        End If
    End Sub

    'Protected Sub ddlSearchFields_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSearchFields.SelectedIndexChanged
    '    If String.Equals(ddlSearchFields.SelectedValue.ToLower(), "dateofjoining") = True Then
    '        lblStart.Text = "Enter Start Date : "
    '        lblStart.Visible = True
    '        lblEnd.Visible = True
    '        txtStart.Visible = True
    '        txtEnd.Visible = True
    '    Else
    '        lblStart.Text = "Enter value : "
    '        lblStart.Visible = False
    '        txtStart.Visible = True
    '    End If
    'End Sub

    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("PairData")
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
        dtData = Session("PairData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("PairData")
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
        dtData = Session("PairData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    

End Class
