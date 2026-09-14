Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_fullpayoutMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim searchtext As String = Session("Search")
        If Not Page.IsPostBack Then
            GvData.Visible = False
            gvContainer.Visible = False

            Session("fullpayoutData") = Nothing

        End If
    End Sub




    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMemberId.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemberId.Text = ""
        End If
        Return formno
    End Function


    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        'GvData.DataSource = Session("AccountData")
        GvData.DataSource = Session("fullpayoutData")
        GvData.DataBind()
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound


        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    If Session("CompID") = 1066 Then
        '        If DirectCast(e.Row.FindControl("LblRefNo"), Label).Text = "0" Then
        '            ' DirectCast(e.Row.FindControl("LBModify"), Label).Visible = False
        '            DirectCast(e.Row.FindControl("PanlModify"), Panel).Visible = False
        '        Else
        '            'DirectCast (e.Row.Page.FindControl("DivModfy").Visible) = True
        '            DirectCast(e.Row.FindControl("PanlModify"), Panel).Visible = False


        '        End If
        '    Else
        '        If DirectCast(e.Row.FindControl("LblRefNo"), Label).Text = "0" Then
        '            ' DirectCast(e.Row.FindControl("LBModify"), Label).Visible = False
        '            DirectCast(e.Row.FindControl("PanlModify"), Panel).Visible = False
        '        Else
        '            'DirectCast (e.Row.Page.FindControl("DivModfy").Visible) = True
        '            DirectCast(e.Row.FindControl("PanlModify"), Panel).Visible = True


        '        End If

        '    End If

        'End If
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            Dim condition1 As String = ""
            Dim Condition2 As String = ""
            Dim condition3 As String = ""

            If ChkMember.Checked Then
                If txtMemberId.Text <> "" Then
                    'formno = GetFormNo()
                    Condition = Condition & " and a.Idno='" & txtMemberId.Text & "'"
                End If
            End If

            If txtStartDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(varchar,a.RecTimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(varchar,a.RecTimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
            End If

            Dim qry1 As String = ""
            qry1 = "select a.Idno, (b.MemFirstName+''+b.MemLastName) as [MemberName]," & _
       " Replace(Convert(varchar,a.RecTimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(a.RecTimeStamp AS TIME),100) as [Date] " & _
       " from M_fullpayment as a" & _
       ",M_MemberMaster as b where a.Idno=b.Idno " & Condition & " "

            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("fullpayoutData.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblErr.Text = ""
        lblCount.Text = ""
        Dim Condition As String = ""
        Dim condition1 As String = ""

        Dim Condition2 As String = ""
        Dim condition3 As String = ""
        Dim formno As String = ""
        Dim qry1 As String = ""
        If ChkMember.Checked Then
            If txtMemberId.Text <> "" Then
                formno = GetFormNo()
                Condition = Condition & " and a.Idno='" & txtMemberId.Text & "'"

            End If
        End If

        If txtStartDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(varchar,a.RecTimestamp,106) as DateTime)>='" & txtStartDate.Text & "'"
        End If
        If txtEndDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(varchar,a.RecTimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
        End If

        Dim str As String = ""
        Dim Dt As DataTable
        Dt = New DataTable

        qry1 = "select a.TId,a.Idno, (b.MemFirstName+''+b.MemLastName) as [MemberName]," & _
        " Replace(Convert(varchar,a.RecTimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(a.RecTimeStamp AS TIME),100) as [Date], " & _
        " Case when a.activestatus='Y' then 'Active' else 'Deactive' end as Status from M_fullpayment as a" & _
        ",M_MemberMaster as b where a.Idno=b.Idno " & Condition & " And a.Activestatus='Y' "
        dtData = New DataTable
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dtData = objDAL.GetData(qry1)
        If dtData.Rows.Count > 0 Then
            GvData.DataSource = dtData
            GvData.DataBind()
            'Session("AccountData") = dtData
            Session("fullpayoutData") = dtData
            GvData.Visible = True
            gvContainer.Visible = True
            btnExport.Enabled = True
            ' lblCount.Text = "Total : " & dtData.Rows.Count
        Else
            lblErr.Text = "No Record Found!!"
            gvContainer.Visible = False
            btnExport.Enabled = False
        End If

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


    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        'dtData = Session("AccountData")
        dtData = Session("fullpayoutData")
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
        'dtData = Session("AccountData")
        dtData = Session("fullpayoutData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        'dtData = Session("AccountData")
        dtData = Session("fullpayoutData")
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
        'dtData = Session("AccountData")
        dtData = Session("fullpayoutData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub




End Class
