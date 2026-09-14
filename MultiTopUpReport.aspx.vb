Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class MultiTopupReport 
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
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim searchtext As String = Session("Search")
        If Not Page.IsPostBack Then
            GvData.Visible = False
            gvContainer.Visible = False
            Session("MtMPin") = Nothing
            FillKit()
            If searchtext <> "" Then

            End If

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



    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MtMPin")
        GvData.DataBind()
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            If ChkMember.Checked Then
                If txtMember.Text <> "" Then
                    formno = GetFormNo()
                    Condition = Condition & " and a.Formno='" & Val(formno) & "'"

                End If
            End If
            If ChkKit.Checked Then

                Condition = Condition & " And (Kit1='" & Val(CmbKit.SelectedValue) & "' or Kit2 ='" & Val(CmbKit.SelectedValue) & "' " & _
                " Or Kit3 = '" & Val(CmbKit.SelectedValue) & "' or " & _
                " Kit4 = '" & Val(CmbKit.SelectedValue) & "' Or Kit5 ='" & Val(CmbKit.SelectedValue) & "')"
            End If
            Dim qry1 As String = ""
            qry1 = "select M.Idno,(M.MemFirstname+' '+M.MemLastName) as MemberName,M.mobl as MobileNo,M.Address1 as Address,M.Tehsil,M.City,M.District,Case when R.statecode=0 then '' Else R.Statename End As StateName," & _
" b.Kitname as Package1, Replace(convert(varchar,a.upgradedate1,106),' ','-')+ ' '+ CONVERT(varchar(15)," & _
" CAST(a.upgradeDate1 AS TIME),100) as UpgradeDate1 " & _
            " ,isnull(c.KitName,'') as Package2,Case when c.KitName is Null then '' " & _
            " else Replace(Convert(varchar,a.UpgradeDate2,106),' ','-')+ ' '+ CONVERT(varchar(15),CAST(a.upgradeDate2 AS TIME),100) end as UpgradeDate2 " & _
            " ,isnull(d.KitName,'') as Package3,Case when d.KitName is Null then '' else Replace(Convert(varchar,a.UpgradeDate3,106),' ','-')+ ' '+ " & _
            " CONVERT(varchar(15),CAST(a.upgradeDate3 AS TIME),100) end as UpgradeDate3,isnull(e.KitName,'') as Package4,Case when e.KitName is Null then ''" & _
            " else Replace(Convert(varchar,a.UpgradeDate4,106),' ','-')+ ' '+ CONVERT(varchar(15),CAST(a.upgradeDate4 AS TIME),100) end as UpgradeDate4," & _
            " isnull(f.KitName,'') as package5,Case when f.KitName is Null then '' else Replace(Convert(varchar,a.UpgradeDate5,106),' ','-')+ ' '+ " & _
            " CONVERT(varchar(15),CAST(a.upgradeDate5 AS TIME),100) end as UpgradeDate5 from MultiTopUp as a Left Join M_KitMaster as c On a.Kit2=c.KitId and c.RowStatus='Y'" & _
            " Left Join M_KitMaster as d On a.Kit3=d.KitId and d.RowStatus='Y' Left Join M_KitMaster as e On a.Kit4=e.KitId and e.RowStatus='Y'" & _
            " Left Join M_KitMaster as f On a.Kit5=f.KitId and f.RowStatus='Y' ,M_KitMaster as b ,M_MemberMaster as M,M_StatedivMaster as R where a.Kit1=b.KitId and  R.statecode=M.statecode and R.Rowstatus='Y'  and a.Formno=M.Formno and b.RowStatus='Y' " & Condition & " "

            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("MultiTopupReport.xls", dg)

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
        If ChkMember.Checked Then
            If txtMember.Text <> "" Then
                formno = GetFormNo()
                Condition = Condition & " and a.Formno='" & Val(formno) & "'"

            End If
        End If

        If ChkKit.Checked Then

            Condition = Condition & " And (Kit1='" & Val(CmbKit.SelectedValue) & "' or Kit2 ='" & Val(CmbKit.SelectedValue) & "' " & _
            " Or Kit3 = '" & Val(CmbKit.SelectedValue) & "' or " & _
            " Kit4 = '" & Val(CmbKit.SelectedValue) & "' Or Kit5 ='" & Val(CmbKit.SelectedValue) & "')"
        End If
        Dim qry1 As String = ""
        qry1 = "select M.Idno,(M.MemFirstname+' '+M.MemLastName) as MemberName,M.Address1 as Address,Case when R.statecode=0 then '' Else R.Statename End As StateName,b.Kitname as Package1," & _
            " Replace(convert(varchar,a.upgradedate1,106),' ','-')+ ' '+ CONVERT(varchar(15),CAST(a.upgradeDate1 AS TIME),100) as UpgradeDate1 " & _
            " ,isnull(c.KitName,'') as Package2,Case when c.KitName is Null then '' " & _
            " else Replace(Convert(varchar,a.UpgradeDate2,106),' ','-')+ ' '+ CONVERT(varchar(15),CAST(a.upgradeDate2 AS TIME),100) end as UpgradeDate2 " & _
            " ,isnull(d.KitName,'') as Package3,Case when d.KitName is Null then '' else Replace(Convert(varchar,a.UpgradeDate3,106),' ','-')+ ' '+ " & _
            " CONVERT(varchar(15),CAST(a.upgradeDate3 AS TIME),100) end as UpgradeDate3,isnull(e.KitName,'') as Package4,Case when e.KitName is Null then ''" & _
            " else Replace(Convert(varchar,a.UpgradeDate4,106),' ','-')+ ' '+ CONVERT(varchar(15),CAST(a.upgradeDate4 AS TIME),100) end as UpgradeDate4," & _
            " isnull(f.KitName,'') as package5,Case when f.KitName is Null then '' else Replace(Convert(varchar,a.UpgradeDate5,106),' ','-')+ ' '+ " & _
            " CONVERT(varchar(15),CAST(a.upgradeDate5 AS TIME),100) end as UpgradeDate5 from MultiTopUp as a Left Join M_KitMaster as c On a.Kit2=c.KitId and c.RowStatus='Y'" & _
            " Left Join M_KitMaster as d On a.Kit3=d.KitId and d.RowStatus='Y' Left Join M_KitMaster as e On a.Kit4=e.KitId and e.RowStatus='Y'" & _
            " Left Join M_KitMaster as f On a.Kit5=f.KitId and f.RowStatus='Y' ,M_KitMaster as b ,M_MemberMaster as M,M_StatedivMaster as R where a.Kit1=b.KitId and  R.statecode=M.statecode and R.Rowstatus='Y'  and a.Formno=M.Formno and b.RowStatus='Y' " & Condition & " "
        dtData = New DataTable
        dtData = objDAL.GetData(qry1)
        If dtData.Rows.Count > 0 Then
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("MtMPin") = dtData
            GvData.Visible = True
            gvContainer.Visible = True
            lblCount.Text = "Total : " & dtData.Rows.Count
        Else
            GvData.Visible = False
            gvContainer.Visible = False
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
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMember.Text = ""
        End If
        Return formno
    End Function
    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("MtMPin")
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
        dtData = Session("MtMPin")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("MtMPin")
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
        dtData = Session("MtMPin")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub


End Class
