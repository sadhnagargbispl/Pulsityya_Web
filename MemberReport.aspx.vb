Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_MemberReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Member / Member Report"
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
            Session("MemberList") = Nothing
            If searchtext <> "" Then
                BindData()
            End If
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


    Public Sub BindData()
        lblErr.Text = ""
        lblCount.Text = ""
        Dim search As String = ""
        Dim condition As String = ""
        search = Session("Search")

        If search = "" Then
            condition = ""

        Else
            'trtype.Visible = False
            'tdPassw.Visible = False
            'trDate.Visible = False
            condition = " where IdNo='" + search + "' Or FormNo='" + search + "' Or FirstName like '%" + search + "%'  Or MobileNo like '%" + search + "%' Or City like '%" + search + "%' "
        End If
        Dim sql As String = "select IdNumber,(FirstName+' '+ LastName) as MemberName,UplinerId,UplinerName,ReferrelID as SponsorId,ReferalName as SponsorName,MobileNo,Email,KitName as PackageName,KitAmount as PackageAmount,KitBv as PackageBv,LegName as Node,Doj as JoiningDate,UpgradeDate from V#memberProfile where 1=1 order by IdNumber"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("MemberList") = dtData
        GvData.Visible = True
        gvContainer.Visible = True
        If dtData.Rows.Count > 0 Then
            lblCount.Text = "Total : " & dtData.Rows.Count
            btnExport.Enabled = True
            btnPrintAll.Enabled = True
            btnPrintCurrent.Enabled = True
        Else
            lblErr.Text = "No Record Found!!"
            btnExport.Enabled = False
            btnPrintAll.Enabled = False
            btnPrintCurrent.Enabled = False
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MemberList")
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
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "MemberDetails.xls"))
        Response.ContentType = "application/ms-excel"
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("MemberList")
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

    Protected Sub btnshowall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnshowall.Click
        BindData()
        'GvData.Visible = True
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblErr.Text = ""
        lblCount.Text = ""
        Dim Condition As String = ""
        If txtStartDate.Text <> "" Then
            If CmbType.SelectedValue = "J" Then
                Condition = Condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)>='" & txtStartDate.Text & "'"
            Else
                Condition = Condition & " And Status='Active' And Cast(Convert(Varchar,DateOfUpgrade,106) as DateTime)>='" & txtStartDate.Text & "'"
            End If
        End If
        If txtEndDate.Text <> "" Then
            If CmbType.SelectedValue = "J" Then
                Condition = Condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)<='" & txtEndDate.Text & "'"
            Else
                Condition = Condition & " And Status='Active' And Cast(Convert(Varchar,DateOfUpgrade,106) as DateTime)<='" & txtEndDate.Text & "'"
            End If
        End If
        If ChkKit.Checked = True Then
            Condition = Condition & " And KitName='" & CmbKit.SelectedItem.Text & "'"
        End If
       
        If ChkSearch.Checked = True Then
            If DDlSerchBy.SelectedValue = "I" Then
                Condition = Condition & " And  IdNumber='" & Trim(TxtSearch.Text) & "'"
            ElseIf DDlSerchBy.SelectedValue = "M" Then
                Condition = Condition & " And  MobileNo='" & Val(Trim(TxtSearch.Text)) & "'"
            ElseIf DDlSerchBy.SelectedValue = "B" Then
                Condition = Condition & " And KitBv='" & Val(TxtSearch.Text) & "'"
            End If
        End If
        If Condition <> "" Then
            Dim qry1 As String = ""
            qry1 = "select IdNumber,(FirstName+' '+ LastName) as MemberName,UplinerId,UplinerName,ReferrelID as SponsorId,ReferalName as SponsorName,MobileNo,Email,KitName as PackageName,KitAmount as PackageAmount,KitBv as PackageBv,LegName as Node,Doj as JoiningDate,UpgradeDate from V#memberProfile where 1=1" & Condition & " order by IdNumber"
            dtData = New DataTable
            dtData = objDAL.GetData(qry1)
            If dtData.Rows.Count > 0 Then
                GvData.DataSource = dtData
                GvData.DataBind()
                Session("MemberList") = dtData
                GvData.Visible = True
                gvContainer.Visible = True
                lblCount.Text = "Total : " & dtData.Rows.Count
            Else
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
        dtData = Session("MemberList")
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
        dtData = Session("MemberList")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("MemberList")
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
        dtData = Session("MemberList")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    '    Protected Sub BtnExportCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportCsv.Click
    '        Dim Condition As String = ""
    '        If txtStartDate.Text <> "" Then
    '            If CmbType.SelectedValue = "J" Then
    '                Condition = Condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)>='" & txtStartDate.Text & "'"
    '            Else
    '                Condition = Condition & " And Status='Active' And Cast(Convert(Varchar,DateOfUpgrade,106) as DateTime)>='" & txtStartDate.Text & "'"
    '            End If
    '        End If
    '        If txtEndDate.Text <> "" Then
    '            If CmbType.SelectedValue = "J" Then
    '                Condition = Condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)<='" & txtEndDate.Text & "'"
    '            Else
    '                Condition = Condition & " And Status='Active' And Cast(Convert(Varchar,DateOfUpgrade,106) as DateTime)<='" & txtEndDate.Text & "'"
    '            End If
    '        End If
    '        If ChkKit.Checked = True Then
    '            Condition = Condition & " And KitName='" & CmbKit.SelectedItem.Text & "'"
    '        End If

    '        Dim spec As String = "&" & ""
    '        Dim strQuery As String = "Select Replace(Replace(Replace((Firstname+' '+LastName),'""',''),',',''),';','') " & _
    '                                 " as MemberName,Replace(Replace(Replace(IDNumber,'""',''),',',''),';','') as IDNumber," & _
    '                                 " Replace(Replace(MobileNo,',',''),'+','')as MobileNo,REPLACE(CONVERT(VARCHAR, Doj , 106), ' ', '-') as JoiningDate, " & _
    '                                 " Replace(Replace(Replace(KitName,'""',''),',',''),';','') as PackageName," & _
    '                            " BV as JoiningBV,UplinerId, ReferrelId as SponsorId,LegName,Replace(Replace(PanNo ,' ',''),',','')as PanCardNo," & _
    '                            "BankName,Replace(Replace(AcNo ,'',''),',','')as AccountNo," & _
    '" Replace(Replace(IFSCode,',',''),';','')  as IFSCCode,REPLACE(CONVERT(VARCHAR, UpgradeDate , 106), ' ', '-') as UpgradeDate  from V#MemberProfile  where 1=1 " & Condition & " order by JoiningDate"
    '        'Dim cmd As New SqlCommand(strQuery)
    '        Dim dt As DataTable = objDAL.GetData(strQuery)
    '        Response.Clear()
    '        Response.Buffer = True
    '        Response.AddHeader("content-disposition", _
    '                "attachment;filename=MemberDetail.csv")
    '        Response.Charset = ""
    '        Response.ContentType = "application/text"

    '        Dim sb As New StringBuilder()
    '        For k As Integer = 0 To dt.Columns.Count - 1
    '            'add separator
    '            sb.Append(dt.Columns(k).ColumnName + ","c)
    '        Next
    '        'append new line
    '        sb.Append(vbCr & vbLf)
    '        For i As Integer = 0 To dt.Rows.Count - 1
    '            For k As Integer = 0 To dt.Columns.Count - 1
    '                'add separator
    '                sb.Append(dt.Rows(i)(k).ToString().Replace(",", ";") + ","c)
    '            Next
    '            'append new line
    '            sb.Append(vbCr & vbLf)
    '        Next
    '        Response.Output.Write(sb.ToString())
    '        Response.Flush()
    '        Response.End()
    '    End Sub

End Class
