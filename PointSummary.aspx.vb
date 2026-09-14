Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_PointSummary
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Report / Point Summary Report"
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
            Session("PointList") = Nothing
            If searchtext <> "" Then

            End If

        End If
    End Sub





    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("PointList")
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
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "PointSummary.xls"))
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("PointList")
        GvData.DataSource = dtData
        GvData.DataBind()
        'BindGridview()
        'Change the Header Row back to white color
        'If Session("PointList") <> "" Then
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
        ' End If
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
        Dim formno As String = ""
        Dim scrName As String = ""

        If RbtType.SelectedValue = "D" Then
            'If txtStartDate.Text = "" Then
            '    scrName = "<SCRIPT language='javascript'>alert('Enter Start Date.');" & "</SCRIPT>"
            '    Me.RegisterStartupScript("MyAlert", scrname)
            '    Exit Sub
            'End If
            'If txtEndDate.Text = "" Then
            '    scrName = "<SCRIPT language='javascript'>alert('Enter End Date.');" & "</SCRIPT>"
            '    Me.RegisterStartupScript("MyAlert", scrName)
            '    Exit Sub
            'End If
            If ChkMem.Checked = False Then
                If txtMember.Text = "" Then
                    scrName = "<SCRIPT language='javascript'>alert('Enter MemberId.');" & "</SCRIPT>"
                    Me.RegisterStartupScript("MyAlert", scrName)
                    Exit Sub
                End If

            End If
        End If

        If ChkMem.Checked Then
            formno = GetFormNo()
            Condition = Condition & " And a.Formno='" & Val(formno) & "'"
        End If
        If txtStartDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,c.FrmDate,106) as DateTime)>='" & txtStartDate.Text & "'"

        End If
        If txtEndDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(Varchar,c.Todate,106)as DateTime)<='" & txtEndDate.Text & "'"
        End If


        Dim qry1 As String = ""

        If RbtType.SelectedValue = "D" Then


            qry1 = "select Replace(Convert(Varchar,c.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,c.Todate,106),' ','-') as Todate,b.IdNo,(B.memFirstname+' '+ b.MemLastname) as Membername,d.KitName as PackageName,d.KitAmount as PackageMRP,d.Bv as PackageBv,LegXBvTotal as TotalLeftBv,LegYBvTotal as TotalRightBv," & _
                        " LegXBvPaid as MatchedBv,LegxBvCf as UnMatchedLeftBv,LegYBvCf as UnMatchedRightBv  from D_sesswiseBvCF as a, M_Membermaster as b,D_SessnMaster as c,M_kitMaster as d " & _
                     " where b.KitId=d.KitId  and a.Formno=b.FormNo and a.Sessid=c.Sessid " & Condition & ""
        Else
            qry1 = "select Replace(Convert(Varchar,c.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,c.Todate,106),' ','-') as Todate,b.IdNo,(B.memFirstname+' '+ b.MemLastname) as Membername,k.KitName as PackageName,k.KitAmount as PackageMRP,k.Bv as PackageBv,LegXBvTotal as TotalLeftBv,LegYBvTotal as TotalRightBv," & _
                     " LegXBvPaid as MatchedBv,LegxBvCf as UnMatchedLeftBv,LegYBvCf as UnMatchedRightBv  from D_sesswiseBvCF as a, M_Membermaster as b,D_SessnMaster as c,M_kitmaster as k, " & _
                  " (select Formno, Max(SessId) as Sessid from D_SessWiseBvCf Group by Formno) as d" & _
                 " where b.KitId=k.KitId  and a.Formno=b.FormNo and a.Sessid=c.Sessid And  a.formno=d.Formno and a.Sessid=d.Sessid " & Condition & ""
        End If
        dtData = New DataTable
        dtData = objDAL.GetData(qry1)
        If dtData.Rows.Count > 0 Then
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("PointList") = dtData
            btnExport.Enabled = True
            GvData.Visible = True
            gvContainer.Visible = True
            lblCount.Text = "Total : " & dtData.Rows.Count
        Else
            btnExport.Enabled = False
            lblErr.Text = "No Record Found!!"
        End If

    End Sub
    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMember.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formNo = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMember.Text = ""
        End If
        Return formNo
    End Function
    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("PointList")
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
        dtData = Session("PointList")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub
    Protected Sub BtnExportCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportCsv.Click
        Dim Condition As String = ""
        Dim formno As String = ""
        Dim scrname As String = ""
        If RbtType.SelectedValue = "D" Then
            If ChkMem.Checked = False Then
                If txtMember.Text = "" Then
                    scrName = "<SCRIPT language='javascript'>alert('Enter MemberId.');" & "</SCRIPT>"
                    Me.RegisterStartupScript("MyAlert", scrName)
                    Exit Sub
                End If

            End If
        End If
        If ChkMem.Checked Then
            formno = GetFormNo()
            Condition = Condition & " And a.Formno='" & Val(formno) & "'"
        End If
        If txtStartDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,c.FrmDate,106) as DateTime)>='" & txtStartDate.Text & "'"

        End If
        If txtEndDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(Varchar,c.Todate,106)as DateTime)<='" & txtEndDate.Text & "'"
        End If


        Dim qry1 As String = ""

        If RbtType.SelectedValue = "D" Then


            qry1 = "select Replace(Convert(Varchar,c.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,c.Todate,106),' ','-') as Todate,Replace(Replace(Replace(b.Idno,'""',''),',',''),';','') as Idno,Replace(Replace(Replace((b.MemFirstName+' '+b.MemLastName),'""',''),',',''),';','') as Membername,Replace(Replace(Replace((d.KitName),'""',''),',',''),';','') as Packagename,d.KitAmount as PackageMRP,d.Bv as PackageBv,LegXBvTotal as TotalLeftBv,LegYBvTotal as TotalRightBv," & _
                        " LegXBvPaid as MatchedBv,LegxBvCf as UnMatchedLeftBv,LegYBvCf as UnMatchedRightBv  from D_sesswiseBvCF as a, M_Membermaster as b,D_SessnMaster as c ,M_Kitmaster as d" & _
                     " where b.KitId=d.KitId  and a.Formno=b.FormNo and a.Sessid=c.Sessid " & Condition & ""
        Else
            qry1 = "select Replace(Convert(Varchar,c.FrmDate,106),' ','-') as FromDate,Replace(Convert(varchar,c.Todate,106),' ','-') as Todate,Replace(Replace(Replace(b.Idno,'""',''),',',''),';','') as Idno,Replace(Replace(Replace((b.MemFirstName+' '+b.MemLastName),'""',''),',',''),';','') as Membername,Replace(Replace(Replace((k.KitName),'""',''),',',''),';','') as Packagename,k.KitAmount as PackageMRP,k.Bv as PackageBv,LegXBvTotal as TotalLeftBv,LegYBvTotal as TotalRightBv," & _
                     " LegXBvPaid as MatchedBv,LegxBvCf as UnMatchedLeftBv,LegYBvCf as UnMatchedRightBv  from D_sesswiseBvCF as a, M_Membermaster as b,D_SessnMaster as c,M_kitmaster as k, " & _
                  " (select Formno, Max(SessId) as Sessid from D_SessWiseBvCf Group by Formno) as d" & _
                 " where b.KitId=k.KitId  and  a.Formno=b.FormNo and a.Sessid=c.Sessid And  a.formno=d.Formno and a.Sessid=d.Sessid " & Condition & ""
        End If

        Dim dt As DataTable = objDAL.GetData(qry1)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", _
                "attachment;filename=PointSummary.csv")
        Response.Charset = ""
        Response.ContentType = "application/text"

        Dim sb As New StringBuilder()
        For k As Integer = 0 To dt.Columns.Count - 1
            'add separator
            sb.Append(dt.Columns(k).ColumnName + ","c)
        Next
        'append new line
        sb.Append(vbCr & vbLf)
        For i As Integer = 0 To dt.Rows.Count - 1
            For k As Integer = 0 To dt.Columns.Count - 1
                'add separator
                sb.Append(dt.Rows(i)(k).ToString().Replace(",", ";") + ","c)
            Next
            'append new line
            sb.Append(vbCr & vbLf)
        Next
        Response.Output.Write(sb.ToString())
        Response.Flush()
        Response.End()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("PointList")
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
        dtData = Session("PointList")
        GvData.DataSource = dtData
        GvData.DataBind()
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
End Class
