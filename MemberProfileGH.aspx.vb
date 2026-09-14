Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Imports System.Configuration

Partial Class App_UI_Application_Pages_MemberProfileGH
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Member / Member Detail"
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
                Filldate()
                GvData.Visible = False
                gvContainer.Visible = False
                Session("MemberList") = Nothing
                Fillkit()
                FillBank()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Fillkit()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetKitMaster")
            CmbKit.DataSource = Ds.Tables(0)
            CmbKit.DataValueField = "KitID"
            CmbKit.DataTextField = "Kitname"
            CmbKit.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillBank()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetBankMaster")
            DdlBank.DataSource = Ds.Tables(0)
            DdlBank.DataTextField = "BankName"
            DdlBank.DataValueField = "BankCode"
            DdlBank.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Filldate()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = objDAL.GetData(Str)
            If dtData.Rows.Count > 0 Then
                txtStartDate.Text = dtData.Rows(0)("CurrentDate")
                txtEndDate.Text = dtData.Rows(0)("CurrentDate")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub BindData(ByVal PageIndex As Integer)
        lblErr.Text = ""
        lblCount.Text = ""
        'Dim search As String = ""
        ' Dim condition As String = ""

        '        Dim sql As String = "select SessId,KitName,IDNumber,ReferrelId as SponsorId,ReferalName as SponsorName,FirstName as MemberName,MemberDob as DateOfBirth,Case when Gender='M' then 'Male' else  'Female' end as Gender,Address," & _
        '"City,State,PinCode,MobileNo,Password,Doj,NomineeName,PanNo,BankName,AcNo as AccountNo,IfsCode as IFSCCode,BV,UpgradeSessId,UpgradeDate,Status from V#MemberProfile " & condition & " order by IdNumber"
        '        dtData = New DataTable
        '        dtData = objDAL.GetData(sql)
        '        GvData.DataSource = dtData
        '        GvData.DataBind()
        '        Session("MemberList") = dtData
        '        ViewState("WithDrawDate") = "Sessid"
        '        ViewState("Sort_Order") = "ASC"
        '        GvData.Visible = True
        '        gvContainer.Visible = True
        '        If dtData.Rows.Count > 0 Then
        '            lblCount.Text = "Total : " & dtData.Rows.Count
        '            btnExport.Enabled = True
        '            btnPrintAll.Enabled = True
        '            btnPrintCurrent.Enabled = True
        '        Else
        '            lblErr.Text = "No Record Found!!"
        '            btnExport.Enabled = False
        '            btnPrintAll.Enabled = False
        '            btnPrintCurrent.Enabled = False
        '        End If
        Try
            Dim KitName As String = "0"
            Dim Idno As String = ""
            Dim BankName As String = ""
            If ChkKit.Checked = True Then
                KitName = CmbKit.SelectedValue
            Else
                KitName = ""
            End If
            If ChkMem.Checked = True Then
                Idno = txtMember.Text
            Else
                Idno = ""
            End If
            If ChkBank.Checked Then
                BankName = DdlBank.SelectedItem.Text
            Else
                BankName = ""

            End If
            Dim startDate As Date
            Dim endDate As Date
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
            End If
            'If txtStartDate.Text <> "" Then
            '    If CmbType.SelectedValue = "J" Then
            '        Condition = Condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)>='" & txtStartDate.Text & "'"
            '    Else
            '        Condition = Condition & " And Status='Active' And Cast(Convert(Varchar,DateOfUpgrade,106) as DateTime)>='" & txtStartDate.Text & "'"
            '    End If
            'End If
            'If txtEndDate.Text <> "" Then
            '    If CmbType.SelectedValue = "J" Then
            '        Condition = Condition & " And Cast(Convert(Varchar,DateOfJoin,106)as DateTime)<='" & txtEndDate.Text & "'"
            '    Else
            '        Condition = Condition & " And Status='Active' And Cast(Convert(Varchar,DateOfUpgrade,106) as DateTime)<='" & txtEndDate.Text & "'"
            '    End If
            'End If
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(10) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@KitName", KitName)
            prms(2) = New SqlParameter("@BankName", BankName)
            prms(3) = New SqlParameter("@SearchType", CmbType.SelectedValue)
            prms(4) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(5) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(6) = New SqlParameter("@JoinType", Convert.ToString(ddljointype.SelectedValue))
            prms(7) = New SqlParameter("@PageIndex", PageIndex)
            prms(8) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(9) = New SqlParameter("@IsExport", "N")
            prms(10) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetmemberProfileDetail", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("MemberList") = Ds.Tables(0)
            ViewState("IdNo") = "IdNo"
            ViewState("Sort_Order") = "ASC"

            If Ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                    Dim img As New Image()
                    img.ImageUrl = "~/Images/Uparrow.png"
                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                    tableCell.Controls.Add(img)
                Next
                lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
                GvData.Visible = True
                gvContainer.Visible = True
            Else
                lblErr.Text = "No Record Found!!"
                GvData.Visible = False
                gvContainer.Visible = True
            End If
            Me.PopulatePager(recordCount, PageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try

    End Sub
    Private Sub PopulatePager(ByVal recordCount As Integer, ByVal currentPage As Integer)
        Dim pages As New List(Of ListItem)()
        Dim startIndex As Integer, endIndex As Integer
        Dim pagerSpan As Integer = 5

        'Calculate the Start and End Index of pages to be displayed.
        Dim dblPageCount As Double = CDbl(CDec(recordCount) / Convert.ToDecimal(ddlPageSize.SelectedValue))
        Dim pageCount As Integer = CInt(Math.Ceiling(dblPageCount))
        startIndex = If(currentPage > 1 AndAlso currentPage + pagerSpan - 1 < pagerSpan, currentPage, 1)
        endIndex = If(pageCount > pagerSpan, pagerSpan, pageCount)
        If currentPage > pagerSpan Mod 2 Then
            If currentPage = 2 Then
                endIndex = 5
            Else
                endIndex = currentPage + 2
            End If
        Else
            endIndex = (pagerSpan - currentPage) + 1
        End If

        If endIndex - (pagerSpan - 1) > startIndex Then
            startIndex = endIndex - (pagerSpan - 1)
        End If

        If endIndex > pageCount Then
            endIndex = pageCount
            startIndex = If(((endIndex - pagerSpan) + 1) > 0, (endIndex - pagerSpan) + 1, 1)
        End If

        'Add the First Page Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("First", "1"))
        End If

        'Add the Previous Button.
        If currentPage > 1 Then
            pages.Add(New ListItem("<<", (currentPage - 1).ToString()))
        End If

        For i As Integer = startIndex To endIndex
            pages.Add(New ListItem(i.ToString(), i.ToString(), i <> currentPage))
        Next

        'Add the Next Button.
        If currentPage < pageCount Then
            pages.Add(New ListItem(">>", (currentPage + 1).ToString()))
        End If

        'Add the Last Button.
        If currentPage <> pageCount Then
            pages.Add(New ListItem("Last", pageCount.ToString()))
        End If
        rptPager.DataSource = pages
        rptPager.DataBind()
    End Sub
    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.BindData(pageIndex)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Me.BindData(1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            ' If e.SortExpression = ViewState("PayoutDate").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next

            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)

                    End If
                Next
            End If
            'Else
            'RebindData(e.SortExpression, "ASC")
            'End If


        Catch ex As Exception

        End Try
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Try

            Dim dt As DataTable = CType(Session("MemberList"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()
            ViewState("IdNo") = sColimnName
            ViewState("Sort_Order") = sSortOrder
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ExportExcel()
        Dim dt As DataTable = Session("MemberList1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "MemberProfile")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=MemberProfileReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MemberList")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Try

            lblErr.Text = ""
            lblCount.Text = ""


            Dim KitName As String = "0"
            Dim Idno As String = ""
            Dim BankName As String = ""
            If ChkKit.Checked = True Then
                KitName = CmbKit.SelectedValue
            Else
                KitName = ""
            End If
            If ChkMem.Checked = True Then
                Idno = txtMember.Text
            Else
                Idno = ""
            End If
            If ChkBank.Checked Then
                BankName = DdlBank.SelectedItem.Text
            Else
                BankName = ""

            End If
            Dim startDate As Date
            Dim endDate As Date
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
            End If
            Dim prms As SqlParameter() = New SqlParameter(10) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@KitName", KitName)
            prms(2) = New SqlParameter("@BankName", BankName)
            prms(3) = New SqlParameter("@SearchType", CmbType.SelectedValue)
            prms(4) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(5) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(6) = New SqlParameter("@JoinType", Convert.ToString(ddljointype.SelectedValue))
            prms(7) = New SqlParameter("@PageIndex", 1)
            prms(8) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(9) = New SqlParameter("@IsExport", "Y")
            prms(10) = New SqlParameter("@RecordCount", ParameterDirection.Output)

            'Dim prms As SqlParameter() = New SqlParameter(9) {}
            'prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            'prms(1) = New SqlParameter("@KitName", KitName)
            'prms(2) = New SqlParameter("@BankName", BankName)
            'prms(3) = New SqlParameter("@SearchType", CmbType.SelectedValue)
            'prms(4) = New SqlParameter("@StartDate", txtStartDate.Text)
            'prms(5) = New SqlParameter("@EndDate", txtEndDate.Text)
            'prms(6) = New SqlParameter("@PageIndex", 1)
            'prms(7) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            'prms(8) = New SqlParameter("@IsExport", "Y")
            'prms(9) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetmemberProfileDetail", prms)
            Session("MemberList1") = Ds.Tables(0)
            ExportExcel()
        Catch ex As Exception



        End Try
        '    Dim Condition As String = ""
        '    Try
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
        '        'If ChkPin.Checked = True Then
        '        '    Condition = Condition & " And PinPoint='" & RbtPinPoint.SelectedItem.Text & "'"
        '        'End If
        '        If ChkBank.Checked Then
        '            Condition = Condition & " And BankName='" & DdlBank.SelectedItem.Text & "'"
        '        End If

        '        If ChkMem.Checked Then
        '            Condition = Condition & " And IdNumber='" & Trim(txtMember.Text) & "'"
        '        End If
        '        Dim spec As String = "&" & ""
        '        Dim dtTemp As New DataTable
        '        Dim dg As New DataGrid
        '        Dim strQuery As String = "select   SessId,KitName as PackageName,IDNumber,ReferrelId as SponsorId,ReferalName as SponsorName,FirstName as MemberName,MemberDob as DateOfBirth,Case when Gender='M' then 'Male' else  'Female' end as Gender,Address," & _
        '"City,State,PinCode,MobileNo,Password,Doj,NomineeName,BankName,PanNo,AcNo as AccountNo,Ifscode as IFSCCode,BV,UpgradeSessId,UpgradeDate,EPassword,Status,Password from V#MemberProfile  where 1=1" & Condition & " order by IdNumber"
        '        'Dim cmd As New SqlCommand(strQuery)
        '        dtTemp = New DataTable
        '        dtTemp = objDAL.GetData(strQuery)
        '        'Dim wb As xmlW
        '        dg.DataSource = dtTemp
        '        dg.DataBind()

        '        ExportToExcel("MemberDetail.xls", dg)

        '    Catch ex As Exception
        '        Response.Write(ex.Message & "Error In Exporting File")
        '    End Try

    End Sub


    'Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
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

    'Protected Sub btnshowall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnshowall.Click
    '    BindData()
    '    'GvData.Visible = True
    'End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim Qry5 As String = "Exec Sp_V#Admin '" & ddljointype.SelectedValue & "' "
        dtData = New DataTable
        dtData = objDAL.GetData(Qry5)
        If dtData.Rows.Count > 0 Then
            lblactive.Text = " Total Activation: " & dtData.Rows(0)("TotalActivate")
            lbldeactive.Text = " Total Deactivation: " & dtData.Rows(0)("TotalDeactivate")
        End If




        BindData(1)
        '        lblErr.Text = ""
        '        lblCount.Text = ""
        '        Dim Condition As String = ""
        '        If ChkBank.Checked Then
        '            Condition = Condition & " And BankName='" & DdlBank.SelectedItem.Text & "'"
        '        End If

        '        If ChkMem.Checked Then
        '            Condition = Condition & " And IdNumber='" & Trim(txtMember.Text) & "'"
        '        End If
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
        '        'If ChkPin.Checked = True Then
        '        '    Condition = Condition & " And PinPoint='" & RbtPinPoint.SelectedItem.Text & "'"
        '        'End If
        '        If Condition <> "" Then
        '            Dim qry1 As String = ""
        '            qry1 = "select   SessId,KitName,IDNumber,ReferrelId as SponsorId,ReferalName as SponsorName,FirstName as MemberName,MemberDob as DateOfBirth,Case when Gender='M' then 'Male' else  'Female' end as Gender,Address," & _
        '"City,State,Country,PinCode,STDCode,MobileNo,Password,Doj,NomineeName,NomineeDOB,NomineeAge,BankName,PanNo,AcNo as AccountNo,Ifscode as IFSCCode,ChDDNo,ChDDDate,ChDDBank,BV,UpgradeSessId,UpgradeDate,IsPanCard,EPassword,Status,Remarks,TopUpStatus,RP,SP,Password from V#MemberProfile  where 1=1" & Condition & " order by IdNumber"
        '            dtData = New DataTable
        '            dtData = objDAL.GetData(qry1)
        '            If dtData.Rows.Count > 0 Then
        '                GvData.DataSource = dtData
        '                GvData.DataBind()
        '                Session("MemberList") = dtData
        '                GvData.Visible = True
        '                gvContainer.Visible = True
        '                lblCount.Text = "Total : " & dtData.Rows.Count
        '            Else
        '                lblErr.Text = "No Record Found!!"
        '            End If
        '        Else
        '            lblErr.Text = "No Search Criteria!"
        '        End If
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
