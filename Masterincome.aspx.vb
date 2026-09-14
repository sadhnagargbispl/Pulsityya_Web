Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Masterincome
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try


            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim searchtext As String = Session("Search")
            If Not Page.IsPostBack Then
                GvData.Visible = False
                gvContainer.Visible = False
                Session("MtMincome") = Nothing

            End If
        Catch ex As Exception

        End Try
    End Sub




    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MtMincome")
        GvData.DataBind()
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            '  If ChkMember.Checked Then
            If txtMember.Text <> "" Then
                formno = GetFormNo()
                Condition = Condition & " and a.Formno='" & Val(formno) & "'"

            End If
            If txtFromDate.Text <> "" Then
                Condition = Condition & " AND Cast(Convert(varchar,a.RecTimeStamp,106) as Datetime) >='" & txtFromDate.Text & "' "
            End If
            If TxtToDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(varchar,a.RecTimeStamp,106) as Datetime)<='" & TxtToDate.Text & "' "

            End If
            ' End If

            Dim qry1 As String = ""
            'qry1 = " select b.Idno,B.Memfirstname+' '+b.Memlastname as MemberName,a.[Total EMI]," & _
            '        " a.[Paid EMI],a.[Due EMI] from(select sum(TotalEMI) as [Total EMI],sum(PaidAmount) " & _
            '        " as [Paid EMI],Case when sum(TotalEmi)< Sum(PaidAmount) then 0 else " & _
            '        " Sum(TotalEMI)-sum(paidamount)end as [Due EMI] from( " & _
            '        " select Sum(Amount) as TotalEMi,0 as PaidAmount,Crto as Formno from TrnEmi where Crto<>0 and ActiveStatus='Y' Group by Crto " & _
            '        " Union all " & _
            '        " select 0 as TotalEMI,Sum(Amount) as PaidAmount,Drto as Formno from TrnEmi  where Drto<>0 and ActiveStatus='Y' " & _
            '        " Group by Drto)as a Group by formno)as a ,M_memberMaster as b where a.Formno=b.Formno " & Condition
            qry1 = "select b.Idno,B.Memfirstname+' '+b.Memlastname as MemberName,a.Amount,REPLACE(CONVERT(varchar,A.RecTimeStamp,106),' ','-') As Date from M_masterincome as a,M_memberMaster as b where A.formno=b.formno" & Condition
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("MasterincomeReport.xls", dg)

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
        Dim formno As String = ""
        Dim scrName As String = ""


        If txtMember.Text <> "" Then
            formno = GetFormNo()
            Condition = Condition & " and a.Formno='" & Val(formno) & "'"

        End If
        If txtFromDate.Text <> "" Then
            Condition = Condition & " AND Cast(Convert(varchar,a.RecTimeStamp,106) as Datetime) >='" & txtFromDate.Text & "' "
        End If
        If TxtToDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(varchar,a.RecTimeStamp,106) as Datetime)<='" & TxtToDate.Text & "' "

        End If


        Dim qry1 As String = ""

        'qry1 = " select b.Idno,B.Memfirstname+' '+b.Memlastname as MemberName,a.[Total EMI]," & _
        '      " a.[Paid EMI],a.[Due EMI] ,a.reqno from(select sum(TotalEMI) as [Total EMI],sum(PaidAmount) " & _
        '      " as [Paid EMI],Case when sum(TotalEmi)< Sum(PaidAmount) then 0 else " & _
        '      " Sum(TotalEMI)-sum(paidamount)end as [Due EMI],Formno,Reqno from( " & _
        '      " select Reqno, Sum(Amount) as TotalEMi,0 as PaidAmount,Crto as Formno from TrnEmi where Crto<>0  ANd ActiveStatus='Y' Group by Crto,Reqno " & _
        '      " Union all " & _
        '      " select Replace(RefNo,'EMI/','0')as ReqNo,0 as TotalEMI,Sum(Amount) as PaidAmount,Drto as Formno from TrnEmi  where Drto<>0 and ActiveStatus='Y'" & _
        '      " Group by Drto,RefNo)as a Group by formno,ReqNo)as a ,M_memberMaster as b where a.Formno=b.Formno " & Condition

        qry1 = "select b.Idno,B.Memfirstname+' '+b.Memlastname as MemberName,a.Amount,REPLACE(CONVERT(varchar,A.RecTimeStamp,106),' ','-') As Date from M_masterincome as a,M_memberMaster as b where A.formno=b.formno" & Condition

        dtData = New DataTable
        dtData = objDAL.GetData(qry1)
        If dtData.Rows.Count > 0 Then
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("MtMincome") = dtData
            GvData.Visible = True
            gvContainer.Visible = True

        Else
            GvData.Visible = False
            gvContainer.Visible = False
            lblErr.Text = "No Record Found!!"
        End If

    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMember.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            'lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            'lblErr.Visible = True
            lblmessage.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblmessage.Visible = True
            txtMember.Text = ""
        End If
        Return formno
    End Function


    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("MtMincome")
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.PagerSettings.Visible = False
        GvData.BorderStyle = BorderStyle.Solid
        ' gridview.BorderWidth = 
        GvData.BorderColor = Drawing.Color.Black



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
        dtData = Session("MtMincome")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub


    'Protected Sub BtADd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtADd.Click
    '    Response.Redirect("Addmasterincome.aspx")
    'End Sub

    Protected Sub txtMember_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMember.TextChanged
        GetFormNo()
    End Sub
End Class
