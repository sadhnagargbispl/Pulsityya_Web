Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class StockWalletTransactionReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Wallet / Wallet Transaction Report"
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
            Session("AccountData") = Nothing
            Fill_DeliveryCenter()
            'FillKit()
        End If
    End Sub


    Private Sub Fill_DeliveryCenter()
        Dim Dt As New DataTable
        Dim str As String = ""
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Str = "Select PartyCode,PartyName FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_Ledgermaster WHERE GroupId Not In(5,21) and OnWebSite='Y' "
        Dt = objDAL.GetData(str)
        DDlMember.DataSource = Dt
        DDlMember.DataTextField = "PartyName"
        DDlMember.DataValueField = "PartyCode"
        DDlMember.DataBind()
    End Sub



    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("AccountData")
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
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            Dim condition1 As String = ""
            Dim Condition2 As String = ""
            Dim condition3 As String = ""



            If txtStartDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(varchar,b.VoucherDate,106) as DateTime)>='" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(varchar,b.VoucherDate,106) as DateTime)<='" & txtEndDate.Text & "'"
            End If
            Dim qry1 As String = ""
            If RbtWalletType.SelectedValue = "T" Then

                qry1 = " Select Replace(Convert(varchar,b.VoucherDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(b.VoucherDate AS TIME),100)  as [Date],b.Crto as [Party Code],l.PartyName as [Party Name],b.CrAmt as [Credit],b.DrAmt as [Debit],Narration FROM (" & _
    " Select VoucherID,recTimeStamp as VoucherDate,Crto,Amount as CrAmt,0 as DrAmt,Narration FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnVoucher WHERE Vtype='R' AND Crto='" & Trim(DDlMember.SelectedValue) & "' UNION ALL " & _
    " Select VoucherID,recTimeStamp as VoucherDate, Drto,0,Amount as DrAmt ,Narration FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnVoucher WHERE Vtype='R' AND Drto='" & Trim(DDlMember.SelectedValue) & "'" & _
    " ) b," & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_LedgerMAster as l WHERE l.PartyCode=b.CrTo " & Condition & " ORDER BY VoucherID Desc"
            Else
                qry1 = "Select b.CRTO as [Party Code],l.PartyName as [Party Name], SUM(CrAmt)-SUM(DrAmt) as Balance FROM ( " & _
                 " Select Crto,ISNULL(SUM(Amount),0) as CrAmt,0 as DrAmt FROM  " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnVoucher WHERE Vtype='R' AND Crto='" & Trim(DDlMember.SelectedValue) & "' GROUP BY Crto " & _
                "  UNION ALL " & _
                 " Select Drto,0,ISNULL(SUM(Amount),0) as CrAmt FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnVoucher WHERE Vtype='R' AND Drto='" & Trim(DDlMember.SelectedValue) & "' GROUP BY Drto) b," & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_LedgerMaster l " & _
                 " WHERE l.PartyCode=b.CrTo  GROUP BY b.CrTo,l.PartyName"


            End If
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("StockWalletTransactionReport.xls", dg)

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
        If txtStartDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(varchar,b.VoucherDate,106) as DateTime)>='" & txtStartDate.Text & "'"
        End If
        If txtEndDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(varchar,b.VoucherDate,106) as DateTime)<='" & txtEndDate.Text & "'"
        End If
        'Dim qry1 As String = ""
        If RbtWalletType.SelectedValue = "T" Then

            qry1 = " Select Replace(Convert(varchar,b.VoucherDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(b.VoucherDate AS TIME),100) as [Date],b.Crto as [Party Code],l.PartyName as [Party Name],b.CrAmt as [Credit],b.DrAmt as [Debit],Narration FROM (" & _
" Select VoucherID,recTimeStamp as VoucherDate,Crto,Amount as CrAmt,0 as DrAmt,Narration FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnVoucher WHERE Vtype='R' AND Crto='" & Trim(DDlMember.SelectedValue) & "'" & _
"  UNION ALL " & _
" Select VoucherID,recTimeStamp as VoucherDate, Drto,0,Amount as DrAmt ,Narration FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnVoucher WHERE Vtype='R' AND Drto='" & Trim(DDlMember.SelectedValue) & "'" & _
" ) b," & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_LedgerMAster as l WHERE l.PartyCode=b.CrTo " & Condition & " ORDER BY VoucherID Desc"
        Else
            qry1 = "Select b.CRTO as [Party Code],l.PartyName as [Party Name], SUM(CrAmt)-SUM(DrAmt) as Balance FROM ( " & _
            " Select Crto,ISNULL(SUM(Amount),0) as CrAmt,0 as DrAmt FROM  " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnVoucher WHERE Vtype='R' AND Crto='" & Trim(DDlMember.SelectedValue) & "' GROUP BY Crto " & _
           "  UNION ALL " & _
            " Select Drto,0,ISNULL(SUM(Amount),0) as CrAmt FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnVoucher WHERE Vtype='R' AND Drto='" & Trim(DDlMember.SelectedValue) & "' GROUP BY Drto) b," & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_LedgerMaster l " & _
            " WHERE l.PartyCode=b.CrTo  GROUP BY b.CrTo,l.PartyName"

        End If
        dtData = New DataTable
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dtData = objDAL.GetData(qry1)
        If dtData.Rows.Count > 0 Then
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("AccountData") = dtData
            GvData.Visible = True
            gvContainer.Visible = True
            btnExport.Enabled = True
            ' lblCount.Text = "Total : " & dtData.Rows.Count
        Else
            GvData.Visible = False
            gvContainer.Visible = False
            lblErr.Text = "No Record Found!!"
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
        dtData = Session("AccountData")
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
        dtData = Session("AccountData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("AccountData")
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
        dtData = Session("AccountData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub




    Protected Sub RbtWalletType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtWalletType.SelectedIndexChanged
        If RbtWalletType.SelectedValue = "B" Then
            txtEndDate.Enabled = False
            txtStartDate.Enabled = False
            txtEndDate.Text = ""
            txtStartDate.Text = ""
        Else
            txtEndDate.Enabled = True
            txtStartDate.Enabled = True
        End If
    End Sub
End Class
