Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports System.Configuration

Partial Class WeeklyIncentive
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Wallet / Daily Incentive"
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
            FillBank()
        End If
    End Sub
    Private Sub FillKit()
        Dim sql As String = "Select KitName,KitID From " + objDAL.tblKitMaster + " Where " + objDAL.activeCondition + "  and OldKit<>'OLD' Order by KitName"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            CmbKit.DataSource = dtData
            CmbKit.DataTextField = "KitName"
            CmbKit.DataValueField = "KitID"
            CmbKit.DataBind()
        End If
    End Sub
    Private Sub FillBank()
        Dim sql As String = "Select BankName,BankCode From M_Bankmaster Where ActiveStatus='Y' and RowStatus='Y' Order by BankName"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        If dtData.Rows.Count > 0 Then
            DdlBank.DataSource = dtData
            DdlBank.DataTextField = "BankName"
            DdlBank.DataValueField = "BankCode"
            DdlBank.DataBind()
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
        Dim sql As String = "select SessId,KitName,IDNumber,ReferrelId as SponsorId,ReferalName as SponsorName,FirstName as MemberName,MemberDob as DateOfBirth,Case when Gender='M' then 'Male' else  'Female' end as Gender,Address," & _
"City,State,PinCode,MobileNo,Password,Doj,NomineeName,PanNo,BankName,AcNo as AccountNo,IfsCode as IFSCCode,BV,UpgradeSessId,UpgradeDate,Status from V#MemberProfile " & condition & " order by IdNumber"
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
    'exportto csvCode
    '    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
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
    '        If ChkPin.Checked = True Then
    '            Condition = Condition & " And PinPoint='" & RbtPinPoint.SelectedItem.Text & "'"
    '        End If
    '        If ChkBank.Checked Then
    '            Condition = Condition & " And BankName='" & DdlBank.SelectedItem.Text & "'"
    '        End If

    '        If ChkMem.Checked Then
    '            Condition = Condition & " And IdNumber='" & Trim(txtMember.Text) & "'"
    '        End If
    '        Dim spec As String = "&" & ""


    '        Dim strQuery As String = "Select Sessid,Replace(Replace(Replace(KitName,'""',''),',',''),';','') as PackageName,Replace(Replace(Replace(IDNumber,'""',''),',',''),';','') as IDNumber,Replace(Replace(Replace((Firstname+' '+LastName),'""',''),',',''),';','')  as MemberName," & _
    '                                  " Replace(Replace(Replace(UplinerId,'""',''),',',''),';','') as UplinerId,Replace(Replace(Replace(UplinerName,'""',''),',',''),';','')  as UplinerName" & _
    ' " ,Replace(Replace(Replace(ReferrelId,'""',''),',',''),';','') as SponsorId,Replace(Replace(Replace(ReferalName,'""',''),',',''),';','')  as SponsorName" & _
    ' ",LegName,REPLACE(CONVERT(VARCHAR,MemberDOB , 106), ' ', '-') as DOB,Case when Gender='M' then 'Male' else  'Female' end as Gender,Replace(Replace(Replace(Replace(Address,'""',''),',',''),';',''),'  ','') as Address," & _
    ' " Replace(Replace(Replace(City,'""',''),',',''),';','') as City,Replace(Replace(Replace(State,'""',''),',',''),';','') as State," & _
    ' " Replace(Replace(Replace(Country,'""',''),',',''),';','') as Country,Replace(Replace(Replace(PinCode,'""',''),',',''),';','') as PinCode," & _
    ' " Replace(Replace(Replace(STDCode,'""',''),',',''),';','') as STDCode,Replace(Replace(MobileNo,',',''),'+','')as MobileNo,REPLACE(CONVERT(VARCHAR, Doj , 106), ' ', '-') as JoiningDate, " & _
    ' "Replace(Replace(Replace(NomineeName,'""',''),',',''),';','') as NomineeName,REPLACE(CONVERT(VARCHAR, NomineeDOB, 106), ' ', '-') as NomineeDOB,Replace(Replace(Replace(NomineeAge,'""',''),',',''),';','') as Nomineeage," & _
    ' "Replace(Replace(PanNo ,' ',''),',','')as PanCardNo,Replace(Replace(Replace(BankName,'""',''),',',''),';','')as BankName,Replace(Replace(AcNo ,'',''),',','')as AccountNo," & _
    '" Replace(Replace(IFSCode,',',''),';','')  as IFSCCode,Replace(Replace(ChDDNo,',',''),';','')  as ChDDNo,REPLACE(CONVERT(VARCHAR,ChDDDate, 106), ' ', '-') as ChDDDate," & _
    '" Replace(Replace(Replace(ChDDBank,'""',''),',',''),';','')as ChDDBank,Replace(Replace(Bv,',',''),';','')  as Bv," & _
    '"Replace(Replace(UpgradeSessId,',',''),';','')  as UpgradeSessId,REPLACE(CONVERT(VARCHAR, UpgradeDate , 106), ' ', '-') as UpgradeDate " & _
    '" ,Replace(Replace(IsPanCard,',',''),';','')  as IsPanCard,Replace(Replace(Status,',',''),';','')  as Status,Replace(Replace(Remarks,',',''),';','')  as Remarks, " & _
    '"Replace(Replace(TopUpStatus,',',''),';','')  as TopUpStatus,Replace(Replace(RP,',',''),';','')  as RP,Replace(Replace(SP,',',''),';','')  as Sp,Replace(Replace(Replace(Password,'""',''),',',''),';','') as Password from V#MemberProfile  where 1=1 " & Condition & " order by JoiningDate"
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

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim Condition As String = ""
        Try
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
            'If ChkPin.Checked = True Then
            '    Condition = Condition & " And PinPoint='" & RbtPinPoint.SelectedItem.Text & "'"
            'End If
            If ChkBank.Checked Then
                Condition = Condition & " And BankName='" & DdlBank.SelectedItem.Text & "'"
            End If

            If ChkMem.Checked Then
                Condition = Condition & " And IdNumber='" & Trim(txtMember.Text) & "'"
            End If
            Dim spec As String = "&" & ""
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim strQuery As String = "select   SessId,KitName as PackageName,IDNumber,ReferrelId as SponsorId,ReferalName as SponsorName,FirstName as MemberName,MemberDob as DateOfBirth,Case when Gender='M' then 'Male' else  'Female' end as Gender,Address," & _
    "City,State,PinCode,MobileNo,Password,Doj,NomineeName,BankName,PanNo,AcNo as AccountNo,Ifscode as IFSCCode,BV,UpgradeSessId,UpgradeDate,EPassword,Status,Password from V#MemberProfile  where 1=1" & Condition & " order by IdNumber"
            'Dim cmd As New SqlCommand(strQuery)
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(strQuery)
            'Dim wb As xmlW
            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("MemberDetail.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try

    End Sub


    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    'Using con As New SqlConnection(constr)
    '    Using cmd As New SqlCommand("SELECT * FROM Customers")
    '        Using sda As New SqlDataAdapter()
    '            cmd.Connection = con
    '            sda.SelectCommand = cmd
    '            Using dt As New DataTable()
    '                sda.Fill(dt)
    '                Using wb As New XLWorkbook()
    '                    wb.Worksheets.Add(dt, "Customers")

    '                    Response.Clear()
    '                    Response.Buffer = True
    '                    Response.Charset = ""
    '                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    '                    Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx")
    '                    Using MyMemoryStream As New MemoryStream()
    '                        wb.SaveAs(MyMemoryStream)
    '                        MyMemoryStream.WriteTo(Response.OutputStream)
    '                        Response.Flush()
    '                        Response.End()
    '                    End Using
    '                End Using
    '            End Using
    '        End Using
    '    End Using
    'End Using
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
        If ChkBank.Checked Then
            Condition = Condition & " And BankName='" & DdlBank.SelectedItem.Text & "'"
        End If

        If ChkMem.Checked Then
            Condition = Condition & " And IdNumber='" & Trim(txtMember.Text) & "'"
        End If
        If txtStartDate.Text <> "" Then
            If CmbType.SelectedValue = "J" Then
                Condition = Condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)<='" & txtStartDate.Text & "'"
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
        'If ChkPin.Checked = True Then
        '    Condition = Condition & " And PinPoint='" & RbtPinPoint.SelectedItem.Text & "'"
        'End If
        If Condition <> "" Then
            Dim qry1 As String = ""
            qry1 = "select   SessId,KitName,IDNumber,ReferrelId as SponsorId,ReferalName as SponsorName,FirstName as MemberName,MemberDob as DateOfBirth,Case when Gender='M' then 'Male' else  'Female' end as Gender,Address," & _
"City,State,Country,PinCode,STDCode,MobileNo,Password,Doj,NomineeName,NomineeDOB,NomineeAge,BankName,PanNo,AcNo as AccountNo,Ifscode as IFSCCode,ChDDNo,ChDDDate,ChDDBank,BV,UpgradeSessId,UpgradeDate,IsPanCard,EPassword,Status,Remarks,TopUpStatus,RP,SP,Password from V#MemberProfile  where 1=1" & Condition & " order by IdNumber"
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

    Protected Sub BtnExportCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportCsv.Click
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
        'If ChkPin.Checked = True Then
        '    Condition = Condition & " And PinPoint='" & RbtPinPoint.SelectedItem.Text & "'"
        'End If
        If ChkBank.Checked Then
            Condition = Condition & " And BankName='" & DdlBank.SelectedItem.Text & "'"
        End If

        If ChkMem.Checked Then
            Condition = Condition & " And IdNumber='" & Trim(txtMember.Text) & "'"
        End If
        Dim spec As String = "&" & ""
        Dim strQuery As String = "Select Replace(Replace(Replace((Firstname+' '+LastName),'""',''),',',''),';','') " & _
                                 " as MemberName,Replace(Replace(Replace(IDNumber,'""',''),',',''),';','') as IDNumber," & _
                                 " Replace(Replace(MobileNo,',',''),'+','')as MobileNo,REPLACE(CONVERT(VARCHAR, Doj , 106), ' ', '-') as JoiningDate, " & _
                                 " Replace(Replace(Replace(KitName,'""',''),',',''),';','') as PackageName," & _
                            " BV as JoiningBV, ReferrelId as SponsorId,Replace(Replace(PanNo ,' ',''),',','')as PanCardNo," & _
                            "BankName,Replace(Replace(AcNo ,'',''),',','')as AccountNo," & _
" Replace(Replace(IFSCode,',',''),';','')  as IFSCCode,REPLACE(CONVERT(VARCHAR, UpgradeDate , 106), ' ', '-') as UpgradeDate  from V#MemberProfile  where 1=1 " & Condition & " order by JoiningDate"
        'Dim cmd As New SqlCommand(strQuery)
        Dim dt As DataTable = objDAL.GetData(strQuery)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", _
                "attachment;filename=MemberDetail.csv")
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

    Protected Sub BtnBankDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBankDetail.Click
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
        'If ChkPin.Checked = True Then
        '    Condition = Condition & " And PinPoint='" & RbtPinPoint.SelectedItem.Text & "'"
        'End If
        If ChkBank.Checked Then
            Condition = Condition & " And BankName='" & DdlBank.SelectedItem.Text & "'"
        End If

        If ChkMem.Checked Then
            Condition = Condition & " And IdNumber='" & Trim(txtMember.Text) & "'"
        End If
        Dim qry1 As String = ""
        qry1 = "select  IDNumber,FirstName as MemberName, MobileNo, AcNo as AccountNo,Ifscode as IFSCCode,BankName,Branchname,PanNo from V#MemberProfile  where 1=1" & Condition & " order by IdNumber"
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


    End Sub

    Protected Sub BtnExportBank_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportBank.Click

        Dim condition As String = ""
        If txtStartDate.Text <> "" Then
            If CmbType.SelectedValue = "J" Then
                condition = condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)>='" & txtStartDate.Text & "'"
            Else
                condition = condition & " And Status='Active' And Cast(Convert(Varchar,DateOfUpgrade,106) as DateTime)>='" & txtStartDate.Text & "'"
            End If
        End If
        If txtEndDate.Text <> "" Then
            If CmbType.SelectedValue = "J" Then
                condition = condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)<='" & txtEndDate.Text & "'"
            Else
                condition = condition & " And Status='Active' And Cast(Convert(Varchar,DateOfUpgrade,106) as DateTime)<='" & txtEndDate.Text & "'"
            End If
        End If
        If ChkKit.Checked = True Then
            condition = condition & " And KitName='" & CmbKit.SelectedItem.Text & "'"
        End If
        'If ChkPin.Checked = True Then
        '    Condition = Condition & " And PinPoint='" & RbtPinPoint.SelectedItem.Text & "'"
        'End If
        If ChkBank.Checked Then
            condition = condition & " And BankName='" & DdlBank.SelectedItem.Text & "'"
        End If

        If ChkMem.Checked Then
            condition = condition & " And IdNumber='" & Trim(txtMember.Text) & "'"
        End If


        Dim dtTemp As New DataTable
        Dim dg As New DataGrid
        Try
            Dim strQuery As String = "Select  IDNumber,(Firstname+' '+LastName) " & _
                                     " as MemberName,MobileNo as MobileNo,'&nbsp;' + Cast(AcNo as Varchar) + '&nbsp;' as AccountNo,IFSCode  as IFSCCode, " & _
                                     " BankName as BankName,BranchName as BranchName,PanNo as PanCardNo" & _
                                    "   from V#MemberProfile  where 1=1 " & condition & "  order by IdNumber"
            'Dim cmd As New SqlCommand(strQuery)
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(strQuery)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("BankDetail.xls", dg)

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
End Class
