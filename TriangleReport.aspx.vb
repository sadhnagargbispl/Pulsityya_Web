Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_TriangleReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Report / Triangle Report"
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
            Session("TriangleData") = Nothing


        End If
    End Sub




    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = Trim(TxtSearch.Text)
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            TxtSearch.Text = ""
        End If
        Return formno
    End Function

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("TriangleData")
        GvData.DataBind()
    End Sub

    ' select c.Idno as SponsorId,(c.MemFirstName+ ''+c.MemLastName) as SponsorName,a.Idno as DirectId,(a.MemFirstname+' '+a.MemLastname) as DirectName,b.Kitname,b.Kitamount,b.Bv,Case when a.LegNo='1' then 'Left' else 'Right' end As LegNo
    'from M_Membermaster as a,m_kitmaster as b,M_MemberMaster as c where a.KitId=b.Kitid and  a.RefFormno=13300 and
    '  a.RefFormno=c.Formno and c.ActiveStatus='Y'
    ' and Upper( RTrim(LTrim(a.PanNo)))=Upper('AMKPD7596C  ') and b.RowStatus='Y' and a.ActiveStatus='Y' 


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable



            Dim dg As New DataGrid
            Dim Condition As String = ""
            If ChkSearch.Checked Then
                Condition = Condition & " And Idno='" & TxtSearch.Text & "'"
            End If
            If txtStartDate.Text <> "" Then

                Condition = Condition & " And  Cast(Convert(Varchar,Doj,106) as DateTime)>='" & txtStartDate.Text & "'"

            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(Varchar,Doj,106)as DateTime)<='" & txtEndDate.Text & "'"
            End If
            Dim str As String = "select Idno as SponsorId,MemName as SponsorName,PanNo,Mobl as Mobileno,Replace(Convert(varchar,Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(Doj AS TIME),100) as DateOfJoining from V#TriangularIDs  where 1=1 " & Condition & "order by Doj"

            'Adp.Fill(ds, "ExportToExcel")
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(str)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("TriangleReport.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
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

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblErr.Text = ""
        lblCount.Text = ""
        Dim Condition As String = ""
        Dim formno As String
        If ChkSearch.Checked Then
            Condition = Condition & " And Idno='" & TxtSearch.Text & "'"
        End If
        If txtStartDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,Doj,106) as DateTime)>='" & txtStartDate.Text & "'"

        End If
        If txtEndDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(Varchar,Doj,106)as DateTime)<='" & txtEndDate.Text & "'"
        End If

        Dim qry1 As String = ""
        qry1 = " select Idno as SponsorId,MemName as SponsorName ,PanNo,Mobl as MobileNo,Replace(Convert(varchar,Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(Doj AS TIME),100) as DateOfJoining  from V#TriangularIDs where 1=1 " & Condition & " order by Doj"
        dtData = New DataTable
        dtData = objDAL.GetData(qry1)
        If dtData.Rows.Count > 0 Then
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("TriangleData") = dtData
            GvData.Visible = True
            gvContainer.Visible = True
            lblCount.Text = "Total : " & dtData.Rows.Count
        Else
            lblErr.Text = "No Record Found!!"
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
        dtData = Session("TriangleData")
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
        dtData = Session("TriangleData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("TriangleData")
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
        dtData = Session("TriangleData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub



End Class
