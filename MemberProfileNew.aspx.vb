Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Imports System.Configuration

Partial Class App_UI_Application_Pages_MemberProfileNew
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
                GridView2.Visible = False
                GridView1.Visible = False
                gvContainer.Visible = False
                Session("MemberList") = Nothing
                Fillkit()
                FillBank()
                'CreateOrAlter_sp_getmemberProfileDetail()
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
    Private Sub BindKitPaln()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetKitMasterPaln")
            CmbKit.DataSource = Ds.Tables(0)
            CmbKit.DataValueField = "KitID"
            CmbKit.DataTextField = "Kitname"
            CmbKit.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub CmbPaln_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        BindKitPaln()
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
        Try
            Dim compid = Session("compID")
            Dim KitName As String = "0"
            Dim Idno As String = ""
            Dim BankName As String = ""


            If ChkKit.Checked = True Then
                KitName = CmbKit.SelectedValue
            Else
                KitName = ""
            End If

            If ChkMem.Checked = True Then
                'Idno = txtMember.Text
                Idno = txtMember.Text.Trim
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
            GridView1.DataSource = Nothing
            GridView1.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(9) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@KitName", KitName)
            prms(2) = New SqlParameter("@BankName", BankName)
            prms(3) = New SqlParameter("@SearchType", CmbType.SelectedValue)
            prms(4) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(5) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(6) = New SqlParameter("@PageIndex", PageIndex)
            prms(7) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(8) = New SqlParameter("@IsExport", "N")
            prms(9) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            If CmbPaln.SelectedValue = 1 Then
                GridView2.Visible = False
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_getmemberProfileDetail_update_1", prms)
                GridView1.DataSource = Ds.Tables(0)
                GridView1.DataBind()
                If Ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To GridView1.Columns.Count - 1
                        Dim tableCell As TableCell = GridView1.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = "~/Images/Uparrow.png"
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    Next
                    If compid = "1095" Then
                        GridView1.Columns(11).Visible = True
                        GridView1.Columns(8).Visible = True
                        GridView1.Columns(25).Visible = True
                        GridView1.Columns(13).Visible = False
                        GridView1.Columns(13).Visible = False
                        'GridView1.Columns(27).Visible = False
                    End If
                    lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
                    GridView1.Visible = True
                    gvContainer.Visible = True
                Else
                    lblErr.Text = "No Record Found!!"
                    GridView1.Visible = False
                    gvContainer.Visible = True
                End If
            Else
                GridView1.Visible = False
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_getmemberProfileDetail_Udateplanwise", prms)
                GridView2.DataSource = Ds.Tables(0)
                GridView2.DataBind()

                If Ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To GridView2.Columns.Count - 1
                        Dim tableCell As TableCell = GridView2.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = "~/Images/Uparrow.png"
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    Next

                    If compid = "1095" Then
                        GridView2.Columns(11).Visible = True
                        GridView2.Columns(8).Visible = True
                        GridView2.Columns(25).Visible = True
                        GridView2.Columns(13).Visible = False
                        GridView2.Columns(13).Visible = False
                        'GridView2.Columns(27).Visible = False
                    End If
                    lblCount.Text = "Total Record: " & Ds.Tables(1).Rows(0)("RecordCount")
                    GridView2.Visible = True
                    gvContainer.Visible = True
                Else
                    lblErr.Text = "No Record Found!!"
                    GridView2.Visible = False
                    gvContainer.Visible = True
                End If
            End If
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("MemberList") = Ds.Tables(0)
            ViewState("IdNo") = "IdNo"
            ViewState("Sort_Order") = "ASC"
            Me.PopulatePager(recordCount, PageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try

    End Sub
    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        ' Hide column for compid = 1106 (Header + Data rows)
        If Session("compid") IsNot Nothing AndAlso Session("compid").ToString() = "1106" Then
            e.Row.Cells(3).Visible = False   ' <-- change index if needed
            Exit Sub
        End If

        ' Header text change logic
        If e.Row.RowType = DataControlRowType.Header Then
            Dim lbl As Label = CType(e.Row.FindControl("lblEVBV"), Label)

            If lbl IsNot Nothing Then
                If Session("compid").ToString() = "1107" Then
                    lbl.Text = "EV"
                Else
                    lbl.Text = "BV"
                End If
            End If
        End If

    End Sub
    Protected Sub GridView2_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        ' Hide column for compid = 1106 (Header + Data rows)
        If Session("compid") IsNot Nothing AndAlso Session("compid").ToString() = "1106" Then
            e.Row.Cells(3).Visible = False   ' <-- change index if needed
            Exit Sub
        End If

        ' Header text change logic
        If e.Row.RowType = DataControlRowType.Header Then
            Dim lbl As Label = CType(e.Row.FindControl("lblEVBV"), Label)

            If lbl IsNot Nothing Then
                If Session("compid").ToString() = "1107" Then
                    lbl.Text = "EV"
                Else
                    lbl.Text = "BV"
                End If
            End If
        End If

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
    Protected Sub Gridview1_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            ' If e.SortExpression = ViewState("PayoutDate").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GridView1.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView1.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next

            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GridView1.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView1.HeaderRow.Cells(i)
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
    Protected Sub Gridview2_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Try
            ' If e.SortExpression = ViewState("PayoutDate").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GridView2.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView2.HeaderRow.Cells(i)
                        Dim img As New Image()
                        img.ImageUrl = If((ViewState("Sort_Order").ToString() = "ASC"), "~/Images/Uparrow.png", "~/Images/DownArrow.png")
                        tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        tableCell.Controls.Add(img)
                    End If
                Next

            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GridView2.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GridView2.HeaderRow.Cells(i)
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
            If Session("CompId") = 1068 Or Session("compID") = 1010 Or Session("compID") = 1072 Or Session("compID") = 1073 Or Session("compID") = 1074 Or Session("compID") = 1078 Or Session("compID") = 1079 Or Session("compID") = 1080 Or Session("CompID") = 1093 Then
                GridView1.DataSource = dt
                GridView1.DataBind()
            End If

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
    Protected Sub Gridview1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = Session("MemberList")
        GridView1.DataBind()
    End Sub
    Protected Sub Gridview2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView2.PageIndexChanging
        GridView2.PageIndex = e.NewPageIndex
        GridView2.DataSource = Session("MemberList")
        GridView2.DataBind()
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Try

            lblErr.Text = ""
            lblCount.Text = ""

            Dim compID = Session("compID")
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
            Dim prms As SqlParameter() = New SqlParameter(9) {}
            prms(0) = New SqlParameter("@IDNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@KitName", KitName)
            prms(2) = New SqlParameter("@BankName", BankName)
            prms(3) = New SqlParameter("@SearchType", CmbType.SelectedValue)
            prms(4) = New SqlParameter("@StartDate", Convert.ToDateTime(startDate))
            prms(5) = New SqlParameter("@EndDate", Convert.ToDateTime(endDate))
            prms(6) = New SqlParameter("@PageIndex", 1)
            prms(7) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(8) = New SqlParameter("@IsExport", "Y")
            prms(9) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            If CmbPaln.SelectedValue = 1 Then
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_getmemberProfileDetail_update_1", prms)
            Else
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_getmemberProfileDetail_Udateplanwise", prms)
            End If
            Session("MemberList1") = Ds.Tables(0)
            ExportExcel()
        Catch ex As Exception



        End Try

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Session("compid") = "1090" Or Session("compid") = "1095" Then
        Else
            Dim Qry5 As String = "Select * from V#Admin"
            dtData = New DataTable
            dtData = objDAL.GetData(Qry5)
            If dtData.Rows.Count > 0 Then
                lblactive.Text = " Total Activation: " & dtData.Rows(0)("TotalActivate")
                lbldeactive.Text = " Total Deactivation: " & dtData.Rows(0)("TotalDeactivate")
            End If
        End If
        BindData(1)
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
