Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_BusinessSummaryReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Report / Business Summary Report"
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

            BindFromDate()
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
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "BusinessSummaryReport.xls"))
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
        ' End If
        GvData.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.[End]()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub

    Public Sub BindFromDate()
        Dim sql As String = "Select * From(Select 0 as sessid,'-- ALL --' As ToDate Union ALL select Sessid, Replace(Convert(varchar,ToDate,106),' ','-')  As Todate from D_SessnMaster where EndTime is Not Null ) As Temp order by Sessid"

        objModuleFun.FillCombo(sql, DDlFromDate, "ToDate", "Sessid")


    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblErr.Text = ""
        lblCount.Text = ""
        Dim Condition As String = ""
        Dim formno As String = ""
        Dim scrName As String = ""
        Dim q As String = ""

        'If RbtSearch.SelectedValue = "M" Then
        '    If txtMember.Text = "" Then
        '        scrName = "<SCRIPT language='javascript'>alert('Enter MemberId.');" & "</SCRIPT>"
        '        Me.RegisterStartupScript("MyAlert", scrName)
        '        Exit Sub
        '    End If

        'End If

        Dim condition1 As String = ""
        If DDlFromDate.SelectedValue <> 0 Then
            condition1 = " And  Cast(Convert(Varchar,d.ToDate,106) as DateTime)='" & DDlFromDate.SelectedItem.Text & "'"
        End If

        'If RbtSearch.SelectedValue = "M" Then
        '    formno = GetFormNo()
        '    Condition = Condition & " And Temp2.Idno='" & Val(formno) & "'"
        '    If DDlFromDate.SelectedValue <> 0 Then
        '        condition1 = "And  Cast(Convert(Varchar,VoucherDate,106) as DateTime)='" & DDlFromDate.SelectedItem.Text & "'"
        '        Condition = Condition & " And  Cast(Convert(Varchar,ms.ToDate,106) as DateTime)='" & DDlFromDate.SelectedItem.Text & "'"

        '    End If

        '    q = " Union " & _
        '        " select Case when a.CrTo=0 then Drto else CrTo end as Formno,0 AS LeftJoining, 0 AS RightJoining, " & _
        '        " Convert(Varchar,a.VoucherDate,112) as SessId,0 as Bv,IsNull(a.Balance,0.00) as WalletAmount from TrnVoucher as a,D_SessnMaster " & _
        '         " as b,( select Convert(varchar,VoucherDate,106) as VoucherDate,Max(VoucherId) as VoucherId from " & _
        '          " TrnVoucher where DrTo='" & formno & "' or CrTo ='" & formno & "' " & condition1 & "  group by Convert(varchar,VoucherDate,106) )as t" & _
        '         " where  a.VoucherId=t.VoucherId   group by CrTo,a.VoucherDate,DrTo,Balance"
        'End If
        'If RbtSearch.SelectedValue = "D" Then

        '    If DDlFromDate.SelectedValue <> 0 Then
        '        q = " Union " & _
        '        " Select Formno,0 AS LeftJoining, 0 AS RightJoining, Convert(Varchar,a.VoucherDate,112) as Sessid,0 as Bv,(SUM(CrAmt)-SUM(DrAmt)) as WalletAmount FROM (" & _
        '            " Select CRto as Formno,SUM(Amount) as CrAmt ,0 As DrAmt,VoucherDate FROm TrnVoucher WHERE Drto=0 AND Cast(Convert(Varchar,VoucherDate,106) as DateTime)<='" & DDlFromDate.SelectedItem.Text & "' GROUP BY Crto,VoucherDate " & _
        '        " UNION ALL " & _
        '     " Select DRto as Formno,0 as CrAmt ,SUM(Amount) As DrAmt,VoucherDate FROm TrnVoucher WHERE Crto=0 AND Cast(Convert(Varchar,VoucherDate,106) as DateTime)<='" & DDlFromDate.SelectedItem.Text & "' GROUP BY Drto,VoucherDate) as a where 1=1 GROUP BY formno,a.VoucherDate"
        '        Condition = Condition & " And  Cast(Convert(Varchar,ms.ToDate,106) as DateTime)='" & DDlFromDate.SelectedItem.Text & "'"
        '    End If

        'End If
        'If txtStartDate.Text <> "" Then

        '    Condition = Condition & " And  Cast(Convert(Varchar,c.FrmDate,106) as DateTime)>='" & txtStartDate.Text & "'"

        'End If
        'If txtEndDate.Text <> "" Then
        '    Condition = Condition & " And Cast(Convert(Varchar,c.Todate,106)as DateTime)<='" & txtEndDate.Text & "'"
        'End If


        Dim qry1 As String = ""

        qry1 = "  select Replace(Convert(Varchar,d.ToDate,106),' ','-') + ' '+  CONVERT(varchar(15),CAST(d.ToDate AS TIME),100) as ReportDate," & _
             "  d.TotalJoin as [Today Joining], Temp3.EpinUsedAmount as [Epin Used Amount Value],Temp3.IssuedEpinAmount as [Total Issued Epin Value]," & _
          "d.PaidAmount as [Net Matching Incentive],Temp3.RepurchIncome as [Net Repurchase Incentive],Temp3.PayableIncentive as [Total Payable Incentive]," & _
         " Temp3.UnusedAmount as [Epin UnUsed Amount Value],Temp3.MemberWallet as [Member Wallet Total Amt] from " & _
       "(select Temp.Sessid as sessid,sum(EpinAmount) as EpinUsedAmount, Sum(Issuedamount) as IssuedEpinAmount," & _
         " Sum(Repurchincome) as RepurchIncome,Sum(PayableIncentive) as   PayableIncentive,Sum(MemberWallet) as MemberWallet ,Sum(UnusedAmount) as UnUsedAmount " & _
       " from(  select b.Sessid,Count(*) as PinNo,(Count(*)*c.KitAmount) as EpinAmount,Prodid,0 as IssuedPin," & _
        " 0 as IssuedAmount , 0 as RepurchIncome,0 as PayableIncentive ,0 as MemberWallet,0 as UnUsedAmount  from" & _
     " M_FormGeneration as a, D_SessnmASTER AS b ,M_KitMaster as c  where  a.Prodid=c.KitId and  " & _
    " Cast(Replace(Convert(Varchar,a.UsedDate,106),' ','-') as DateTime) " & _
       " =Cast(Replace(Convert(Varchar,b.Todate,106),' ','-') as DateTime) and b.EnDTime  is Not Null and c.RowStatus='Y'" & _
   " Group By b.Sessid,Prodid,KitAmount  Union All  select  temp1.Sessid,0 as PinNo,0 as EpinAmount,0 as Prodid," & _
   "  0 as IssuedPin,Sum(IssuedAmount) as Issuedamount, 0 as RepurchIncome,0 as PayableIncentive,0 as MemberWallet," & _
    " 0 as UnUsedAmount  from ( select b.Sessid,0 as PinNo,0 as EpinAmount,0 as Prodid,Count(*) as IssuedPin," & _
   "  (Count(*)*c.KitAmount)as IssuedAmount  from M_FormGeneration as a, D_SessnmASTER AS b ,M_KitMaster as c" & _
     " where  a.Prodid=c.KitId and Cast(Replace(Convert(Varchar,a.IssuedDate,106),' ','-') as DateTime) =Cast(Replace(Convert(Varchar,b.Todate,106),' ','-') as DateTime) " & _
      " and b.EnDTime  is Not Null and c.RowStatus='Y' Group By b.Sessid,Prodid,KitAmount,IssuedUserId ) as temp1 " & _
      " Group By Sessid " & _
       " Union All " & _
       " select  temp1.Sessid,0 as PinNo,0 as EpinAmount,0 as Prodid,0 as IssuedPin,0 as Issuedamount," & _
       " 0 as RepurchIncome,0 as PayableIncentive,0 as MemberWallet,Sum(UnUsedAmount) as UnUsedAmount  from ( " & _
       " select b.Sessid,0 as PinNo,0 as EpinAmount,0 as Prodid,Count(*) as UnusedPin,(Count(*)*c.KitAmount)as  UnUsedAmount" & _
       " from M_FormGeneration as a, D_SessnmASTER AS b ,M_KitMaster as c  where  a.Prodid=c.KitId and " & _
        "( UsedBy=0  Or Cast(Replace(Convert(Varchar,a.UsedDate,106),' ','-') as DateTime)  >Cast(Replace(Convert(Varchar,b.Todate,106),' ','-') as DateTime) ) " & _
         "And Cast(Replace(Convert(Varchar,IssueDdate,106),' ','-') as DateTime)<=Cast(Replace(Convert(Varchar,b.Todate,106),' ','-') as DateTime)  and b.EnDTime " & _
     " is Not Null  and C.RowStatus='Y' Group By b.Sessid,Prodid,KitAmount ) as temp1 Group By Sessid  " & _
      "Union All" & _
    " select  Convert(Varchar,a.BillDate,112) as Sessid,0 as PinNo,0 as EpinAmount,0 as Prodid,0 as IssuedPin," & _
    " 0 as Issuedamount, Sum(RepurchIncome)as RepurchIncome,0 as PayableIncentive,0 as MemberWallet,0 as UnUsedAmount  from RepurchIncome as a,D_SessnMaster as b" & _
     " where a.BillType='R' and  Convert(Varchar,a.BillDate,112) =b.sessid group by Convert(Varchar,a.BillDate,112) " & _
      "Union All " & _
       " Select a.Sessid,0 as PinNo,0 as EpinAmount,0 as Prodid,0 as IssuedPin," & _
    " 0 as Issuedamount, 0 as RepurchIncome,0 as PayableIncentive,(SUM(CrAmt)-SUM(DrAmt)) as MemberWallet,0 as UnUsedAmount FROM ( " & _
      " Select SUM(Amount) as CrAmt ,0 As DrAmt,b.Sessid FROm TrnVoucher as a,D_sessnmaster as b WHERE Drto=0 and b.EnDTime  is Not Null AND Cast(Convert(Varchar,VoucherDate,106) as DateTime)" & _
      " <=Cast(Convert(Varchar,ToDate,106) as DateTime) Group By b.Sessid  " & _
        " UNION ALL " & _
       " Select 0 as CrAmt ,SUM(Amount) As DrAmt,b.Sessid FROm TrnVoucher as a,D_SessnMaster as b WHERE Crto=0 and b.EnDTime  is Not Null AND Cast(Convert(Varchar,VoucherDate,106) as DateTime)" & _
       " <=Cast(Convert(Varchar,ToDate,106) as DateTime) Group By b.Sessid ) as a  Group by Sessid" & _
       " Union All " & _
      " select  Sessid as Sessid,0 as PinNo,0 as EpinAmount,0 as Prodid,0 as IssuedPin,0 as Issuedamount  ," & _
     "0 as RepurchIncome, Sum(ChqAmt) as PayableIncentive,0 as MemberWallet,0 as UnUsedAmount from D_MonthlyPayDetail  " & _
     "group by Sessid " & _
           " ) as Temp  Group By Temp.Sessid) as tEmp3 ,D_SessnMaster as d where Temp3.Sessid =d.SessId and d.EnDTime  is Not Null" & condition1 & "   Order by Temp3.Sessid "
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
    'Private Function GetFormNo() As String
    '     objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '    Dim idNo As String
    '    Dim formno As String
    '    idNo = txtMember.Text
    '    idNo = idNo.Trim
    '    Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
    '    Dim dt As New DataTable
    '    dt = objDAL.GetData(qry)
    '    If (dt.Rows.Count > 0) Then
    '        formno = dt.Rows(0)("FormNo")
    '    Else
    '        lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
    '        lblErr.Visible = True
    '        txtMember.Text = ""
    '    End If
    '    Return formno
    'End Function
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
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        'For i As Integer = 0 To GvData.Rows.Count - 1
        '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 1).Visible = False
        '    GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        'Next

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

    'Protected Sub RbtSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtSearch.SelectedIndexChanged
    '    If RbtSearch.SelectedValue = "D" Then
    '        txtMember.Text = ""
    '    Else
    '        DDlFromDate.SelectedIndex = 0

    '    End If
    'End Sub
End Class
