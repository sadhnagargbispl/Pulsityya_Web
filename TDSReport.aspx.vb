Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization.DateTimeFormatInfo

Partial Class TDSReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("~\Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If Not Page.IsPostBack Then
            txtMemId.Text = ""
            GvData.Visible = False
            btnExport.Enabled = False
            If Session("AStatus") = "OK" Then
                BindSession()
            End If
        End If
    End Sub

    Public Sub BindSession()
        Dim sql As String = "select * from (Select distinct DateName( Year,frmDate )as Year from M_SessnMaster) as a Order By Year"
        objModuleFun.FillCombo(sql, DDlYear, "Year", "Year")
        'For i As Int16 = 1 To 12
        '    ddlMonth.Items.Add(Strings.Left(MonthName(i), 3).Trim.ToUpper)
        'Next
        Dim Month() As String = System.Globalization.DateTimeFormatInfo.InvariantInfo.MonthNames
        ddlMonth.DataSource = Month
        ddlMonth.DataBind()

    End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        Try

      
            If CheckBox1.Checked = True Then
                Condition = Condition & " And IDNo='" & txtMemId.Text.Trim & "'"
            End If


            'sql = "select PayoutDate,IdNo,MemName,Mobl,BankName,AccountNo,IFSCode,Panno,BinaryIncome,NetIncome,TDSPer,TdsAmount,Deduction,Prevbal as Previous,chqAmt as ChequeAmount,ClsBal As Closing from V#DailyPayoutDetail where 1=1 And (NetIncome>0 Or PrevBal>0 Or ClsBal>0) " & Condition

            sql = "Select * from(select  Sessid,payoutDate,Idno,Mem_name as MemberName, Address1 ,PanNo,(NetIncome) as NetIncome,TdsAmount from  V#PayoutDetail With(Nolock) where 1=1 And (TdsAmount>0) and DateName( Year,fromDate )='" & DDlYear.SelectedValue & "' and DateName(MOnth,FromDate)='" & ddlMonth.SelectedItem.Text & "' and DateName(MOnth,ToDate)='" & ddlMonth.SelectedItem.Text & "' " & Condition & "" & _
            " Union all " & _
           " select  Sessid,payoutDate,Idno,Mem_Name as MemberName, Address1 ,PanNo,(NetIncome) as NetIncome,(TdsAmount)As TdsAmount from  V#DailyPayoutDetail With(Nolock) where 1=1 And (TdSamount>0) and DateName( Year,fromDate )='" & DDlYear.SelectedValue & "' and DateName(MOnth,FromDate)='" & ddlMonth.SelectedItem.Text & "' and DateName(MOnth,ToDate)='" & ddlMonth.SelectedItem.Text & "'  " & Condition & " )as Temp Order by Sessid"
            dtData = New DataTable
            dtData = objDAL.GetData(sql)

            If dtData.Rows.Count > 0 Then
                LblPayPeriod.Text = ddlMonth.SelectedItem.Text & " " & DDlYear.SelectedItem.Text
                btnExport.Enabled = True
                divHeader.Visible = True
                Dim aDr1 As DataRow = dtData.NewRow
                aDr1("PanNo") = "Grand Total"
                aDr1("NetIncome") = dtData.Compute("Sum(NetIncome)", "")
                aDr1("TdsAmount") = dtData.Compute("Sum(TdSAmount)", "")
                dtData.Rows.Add(aDr1)
                dtData.AcceptChanges()
                GvData.DataSource = dtData
                GvData.DataBind()
                Session("GData") = dtData
                GvData.Visible = True
            Else
                
                LblPayPeriod.Text = ""
                btnExport.Enabled = False
                divHeader.Visible = False
            End If



          
        Catch ex As Exception

        End Try

    End Sub
    

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.ClearContent()
        Response.Buffer = True
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "TDSReport.xls"))
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



        Dim sw As New StringWriter()

        Dim hw As New HtmlTextWriter(sw)

        GvData.RenderControl(hw)
        Dim s As New StringWriter()
        Dim H As New HtmlTextWriter(s)
        divHeader.RenderControl(H)

        Dim HeaderHtml As String = s.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")


        Dim gridHTML As String = sw.ToString().Replace("""", "'").Replace(System.Environment.NewLine, "")

        Dim sb As New StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload = new function(){")

        sb.Append("var printWin = window.open('', '', 'left=0")

        sb.Append(",top=0,width=1000,height=1000,status=0');")

        sb.Append("printWin.document.write(""")

        sb.Append(HeaderHtml & gridHTML)

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
