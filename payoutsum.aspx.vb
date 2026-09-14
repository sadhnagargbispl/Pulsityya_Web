Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Configuration
Imports ClosedXML.Excel

Partial Class App_UI_Application_Pages_payoutsum
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Payout Summary"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Not Page.IsPostBack Then
                txtMemId.Text = ""
                GvData.Visible = False

                If Session("AStatus") = "OK" Then
                    BindSession()
                    ' BindSession1()
                    If Request.QueryString.HasKeys Then
                        If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                            txtMemId.Text = Request.QueryString("key")
                            If txtMemId.Text <> "" Then
                                CheckBox1.Checked = True
                                'IncentiveDetail()
                            End If


                            '  ChkRadioMember.SelectedValue = "N" : IncentiveDetail()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindSession()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetWeeklySession")
            ddlSession.DataSource = Ds.Tables(0)
            ddlSession.DataValueField = "SessID"
            ddlSession.DataTextField = "SessnName"
            ddlSession.DataBind()
            ddlSession1.DataSource = Ds.Tables(0)
            ddlSession1.DataValueField = "SessID"
            ddlSession1.DataTextField = "SessnName"
            ddlSession1.DataBind()

        Catch ex As Exception
        End Try
    End Sub
    

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        Try


            IncentiveDetail(0, 1)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub IncentiveDetail(ByVal Sessid As Integer, ByVal PageIndex As Integer)
        Try



            Dim Idno As String = "0"

            If CheckBox1.Checked Then
                If txtMemId.Text <> "" Then
                    Idno = txtMemId.Text
                Else
                    Idno = "0"
                End If
            Else
                Idno = "0"
            End If
            Dim FromSessid As Integer
            Dim ToSessid As Integer
            If CheckBox2.Checked Then
                If ddlSession.SelectedValue <> "5000000" Then
                    FromSessid = ddlSession.SelectedValue
                Else
                    FromSessid = 0
                End If
                If ddlSession1.SelectedValue <> "5000000" Then
                    ToSessid = ddlSession1.SelectedValue
                Else
                    ToSessid = 0

                End If
            End If
              
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@IdNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@FromSessid", Convert.ToInt32(FromSessid))
            prms(2) = New SqlParameter("@ToSessid", Convert.ToInt32(ToSessid))
            prms(3) = New SqlParameter("@PageIndex", PageIndex)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "N")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetPayoutSummery", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()


            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            Session("GData1") = Ds.Tables(0)
            ViewState("PayoutDate") = "SNo"
            ViewState("Sort_Order") = "ASC"
            If Session("Compid") = "1030" Then

                If GvData.Rows.Count > 0 Then
                    GvData.Columns(0).Visible = True
                Else
                    GvData.Columns(0).Visible = False
                End If
            End If

            If Ds.Tables(1).Rows(0)("RecordCount") > 0 Then
                For i As Integer = 1 To GvData.Columns.Count - 1
                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                    Dim img As New Image()
                    img.ImageUrl = "~/Images/Uparrow.png"
                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                    tableCell.Controls.Add(img)
                Next
                GvData.Focus()
                divDetail.Visible = True
                GvData.Visible = True

            Else

                divDetail.Visible = False
                GvData.Visible = False
            End If
            Me.PopulatePager(recordCount, PageIndex)
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
            Dim dt As DataTable = CType(Session("GData1"), DataTable)
            dt.DefaultView.Sort = sColimnName + " " + sSortOrder
            GvData.DataSource = dt
            GvData.DataBind()
            ViewState("PayoutDate") = sColimnName
            ViewState("Sort_Order") = sSortOrder
            GvData.Focus()
        Catch ex As Exception

        End Try
    End Sub


    'Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
    '    '  Dim imgAsc As String = " <img src='UpArrow.png' border='0' title='Ascending' />"
    '    '  Dim imgDes As String = " <img src='DownArrow.png' border='0' title='Descendng' />"

    '    If e.Row.RowType = DataControlRowType.Header Then
    '        DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
    '        'For Each cell As TableCell In e.Row.Cells
    '        '    Dim lnkbtn As LinkButton = CType(cell.Controls(0), LinkButton)
    '        '    If lnkbtn.Text = GvData.SortExpression Then
    '        '        If GvData.SortDirection = SortDirection.Ascending Then
    '        '            lnkbtn.Text += imgAsc
    '        '        Else
    '        '            lnkbtn.Text += imgDes
    '        '        End If
    '        '    End If
    '        'Next
    '    End If

    'End Sub

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
            Dim sessid As String = "0"

            If ddlSession.SelectedValue = "5000000" Then
                If Val(LblSessionNo.Text) <> 0 Then
                    sessid = Val(LblSessionNo.Text)
                Else
                    sessid = 0

                End If
            Else
                sessid = ddlSession.SelectedValue
            End If
            If ddlSession1.SelectedValue = "5000000" Then
                If Val(LblSessionNo.Text) <> 0 Then
                    sessid = Val(LblSessionNo.Text)
                Else
                    sessid = 0

                End If
            Else
                sessid = ddlSession1.SelectedValue
            End If
            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.IncentiveDetail(sessid, pageIndex)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim sessid As String = "0"

            If ddlSession.SelectedValue = "5000000" Then
                If Val(LblSessionNo.Text) <> 0 Then
                    sessid = Val(LblSessionNo.Text)
                Else
                    sessid = 0

                End If
            Else
                sessid = ddlSession.SelectedValue
            End If

            If ddlSession1.SelectedValue = "5000000" Then
                If Val(LblSessionNo.Text) <> 0 Then
                    sessid = Val(LblSessionNo.Text)
                Else
                    sessid = 0

                End If
            Else
                sessid = ddlSession1.SelectedValue
            End If

            Me.IncentiveDetail(sessid, 1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try


            Dim Idno As String = "0"
            If CheckBox1.Checked Then
                If txtMemId.Text <> "" Then
                    Idno = txtMemId.Text
                Else
                    Idno = "0"
                End If
            Else
                Idno = "0"
            End If
            Dim FromSessid As Integer
            Dim ToSessid As Integer
            If CheckBox2.Checked Then
                If ddlSession.SelectedValue <> "5000000" Then
                    FromSessid = ddlSession.SelectedValue
                Else
                    FromSessid = 0
                End If
                If ddlSession1.SelectedValue <> "5000000" Then
                    ToSessid = ddlSession1.SelectedValue
                Else
                    ToSessid = 0

                End If
            End If

            Dim prms As SqlParameter() = New SqlParameter(6) {}
            prms(0) = New SqlParameter("@IdNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@FromSessid", Convert.ToInt32(FromSessid))
            prms(2) = New SqlParameter("@ToSessid", Convert.ToInt32(ToSessid))
            prms(3) = New SqlParameter("@PageIndex", 1)
            prms(4) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "Y")
            prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetPayoutSummery", prms)
            ' GvData.DataSource = Ds.Tables(0)
            ' GvData.DataBind()

            Session("GData") = Ds.Tables(0)
            ExportExcel()
            'dtData = Session("GData")
            'GvData.DataSource = dtData
            'GvData.DataBind()

            'GvData.HeaderRow.Style.Add("background-color", "#FFFFFF")

            'For i As Integer = 0 To GvData.HeaderRow.Cells.Count - 1
            '    GvData.HeaderRow.Cells(i).Style.Add("background-color", "#D8A38D")
            'Next



            'For i As Integer = 0 To GvData.Rows.Count - 1
            '    For j As Integer = 0 To GvData.Rows(i).Cells.Count - 1
            '        GvData.Rows(i).Cells(j).Style.Add("background-color", "#FFFFFF")
            '        GvData.Rows(i).Cells(j).Style.Add("color", "#000000")
            '    Next
            'Next
            'GvData.RenderControl(htw)
            'Response.Write(sw.ToString())
            'Response.[End]()
        Catch ex As Exception

        End Try
        'Try


        '    Response.ClearContent()
        '    Response.Buffer = True
        '    Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", "IncentiveDetailReport.xls"))
        '    Response.ContentType = "application/ms-excel"
        '    Dim sw As New StringWriter()
        '    Dim htw As New HtmlTextWriter(sw)
        '    GvData.AllowPaging = False
        '    GvData.GridLines = GridLines.Both
        '    dtData = New DataTable
        '    dtData = Session("GData")
        '    GvData.DataSource = dtData
        '    GvData.DataBind()
        '    'BindGridview()
        '    'Change the Header Row back to white color
        '    GvData.HeaderRow.Style.Add("background-color", "#FFFFFF")
        '    'Applying stlye to gridview header cells
        '    For i As Integer = 0 To GvData.HeaderRow.Cells.Count - 1
        '        GvData.HeaderRow.Cells(i).Style.Add("background-color", "#D8A38D")
        '    Next

        '    'Remove modify and Delete columns from grid
        '    GvData.HeaderRow.Cells(0).Visible = False
        '    'GvData.HeaderRow.Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        '    For i As Integer = 0 To GvData.Rows.Count - 1
        '        GvData.Rows(i).Cells(0).Visible = False
        '        '  GvData.Rows(i).Cells(GvData.HeaderRow.Cells.Count - 2).Visible = False
        '    Next

        '    For i As Integer = 0 To GvData.Rows.Count - 1
        '        For j As Integer = 0 To GvData.Rows(i).Cells.Count - 1
        '            GvData.Rows(i).Cells(j).Style.Add("background-color", "#FFFFFF")
        '            GvData.Rows(i).Cells(j).Style.Add("color", "#000000")
        '        Next
        '    Next
        '    GvData.RenderControl(htw)
        '    Response.Write(sw.ToString())
        '    Response.[End]()
        'Catch ex As Exception

        'End Try

        'Try
        '    Dim dtTemp As New DataTable
        '    Dim dg As New DataGrid

        '    If CheckBox1.Checked = True Then
        '        Condition = Condition & " And IDNo='" & txtMemId.Text & "'"
        '    End If
        '    If CheckBox2.Checked = True Then
        '        If ddlSession.SelectedValue > 0 Then
        '            Condition = Condition & " And SessID=" & ddlSession.SelectedValue
        '        End If
        '    End If


        '    'sql = "select PayoutDate,IdNo,MemName As MemberName,Mobl As MobileNo,SpillIncome as [Direct Income],BinaryIncome as [Bonus Income],[UpgrdCut] as [Upgrade Cut],NetIncome As GrossIncome,TdsAmount,Deduction,PrevBal as PreviousBalance,chqAmt as NetIncome,ClsBal as ClosingBalance " & _
        '    '" from V#PayoutDetail where 1=1 " & Condition & " order by Sessid Desc"
        '    dtTemp = New DataTable
        '    dtTemp = objDAL.GetData(sql)

        '    dg.DataSource = dtTemp
        '    dg.DataBind()

        '    ExportToExcel("IncentiveDetail.xls", dg)

        'Catch ex As Exception
        '    Response.Write(ex.Message & "Error In Exporting File")
        'End Try
    End Sub
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Incentive")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=MonthlyIncentiveDetail.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

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
    'Private Sub SendSMS()
    '    Dim Chk As CheckBox
    '    Dim Lbl As Label
    '    Dim LblName As Label
    '    Dim LblFormNo As Label
    '    Dim LblIdNo As Label
    '    Dim LblNetIncome As Label
    '    Dim LblSess As Label
    '    Dim LblFromDate As New Label
    '    Dim LbltoDate As New Label
    '    Dim client As New WebClient
    '    Dim baseurl As String
    '    Dim data As Stream
    '    Dim FormNo As String = Session("MIDNo")
    '    Dim issms As Integer
    '    Dim Fromdate, ToDate, Amount As String

    '    For Each Gvr As GridViewRow In GvData.Rows
    '        Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
    '        Lbl = DirectCast(Gvr.FindControl("LblMobl"), Label)
    '        LblIdNo = DirectCast(Gvr.FindControl("LblMemId"), Label)
    '        LblFormNo = DirectCast(Gvr.FindControl("LblFormNo"), Label)
    '        LblName = DirectCast(Gvr.FindControl("LblFullName"), Label)
    '        LblNetIncome = DirectCast(Gvr.FindControl("LblNetIncome1"), Label)
    '        Amount = DirectCast(Gvr.FindControl("LblNetAmount"), Label).Text
    '        LblSess = DirectCast(Gvr.FindControl("LblSess"), Label)
    '        LblFromDate = DirectCast(Gvr.FindControl("lblfromDate"), Label)
    '        LbltoDate = DirectCast(Gvr.FindControl("lblTodate"), Label)

    '        If Chk.Checked = True And Chk.Enabled = True Then

    '            ' Dim sms As String = "Congrats " & LblName.Text & "," & LblIdNo.Text & ",Your Payout Of Rupees " & LblNetIncome.Text & " is Generated. For More Details Website " & Session("CompWeb")
    '            Dim sms As String = "Dear " & LblName.Text & "-" & LblIdNo.Text & ", your payout for period " & LblFromDate.Text & " to " & LbltoDate.Text & " is Rs." & Amount & ". Pls visit " & Session("CompWeb1") & " for more details."
    '            Try
    '                Dim Mobl As String = Lbl.Text
    '                ' baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Mobl & "&msg=" & sms & ""

    '                ' baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Lbl.Text & "&msg=" & sms & ""
    '                baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & Lbl.Text & "&SenderId=" & Session("ClientId") & ""
    '                'baseurl = Session("SmsAPI") & "username=" & Session("SmsId") & "&password=" & Session("SmsPass") & "&Sender=" & Session("ClientId") & "&to=" & Lbl.Text & "&message=" & sms & "&format=text&unique=1"

    '                data = client.OpenRead(baseurl)
    '                Dim reader As New StreamReader(data)
    '                Dim s As String
    '                s = reader.ReadToEnd()

    '                data.Close()
    '                reader.Close()
    '                issms += 1
    '                sql = "Update M_MonthlyPayDetail Set IsSentSms='Y' where Formno='" & Val(LblFormNo.Text) & "' and Sessid='" & Val(LblSess.Text) & "';"

    '                sql = sql & "Insert into M_SmsMaster(FormNo,SmsStatus,smsDate,RecTimeStamp,ActiveStatus)values('" & Val(LblFormNo.Text) & "','Payout',GetDate(),GetDate(),'Y')"

    '                'sql = "Update M_MonthlyPayDetail Set IsSentSms='Y' where FormNo= '" & LblFormNo.Text & "'  AND SessId= '" & LblSess.Text & "' "
    '                objDAL.UpdateData(sql)

    '            Catch ex As Exception
    '                'MsgBox(ex.Message)
    '            End Try
    '        End If
    '    Next
    '    lblError.Visible = True
    '    lblError.ForeColor = Drawing.Color.Green
    '    lblError.Text = issms & " SMS Sent Successfully!!"

    'End Sub

    'Protected Sub btnSendSms_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendSms.Click
    '    SendSMS()
    'End Sub

    Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    End Sub

    'Protected Sub BtnSendSmsToAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSendSmsToAll.Click
    '    If CheckBox1.Checked = True Then
    '        Condition = Condition & " And IDNo='" & txtMemId.Text & "'"
    '    End If
    '    If CheckBox2.Checked = True Then
    '        If ddlSession.SelectedValue > 0 Then
    '            Condition = Condition & " And SessID=" & ddlSession.SelectedValue
    '        End If
    '    End If
    '    Dim client As New WebClient
    '    Dim baseurl As String
    '    Dim data As Stream
    '    Dim issms As Integer = 0

    '    'sql = "select PayoutDate,IdNo,MemName,Mobl,BankName,AccountNo,IFSCode,Panno,BinaryIncome,NetIncome,TDSPer,TdsAmount,Deduction,Prevbal as Previous,chqAmt as ChequeAmount,ClsBal As Closing from V#DailyPayoutDetail where 1=1 And (NetIncome>0 Or PrevBal>0 Or ClsBal>0) " & Condition
    '    sql = "select 'true' as status,*,Cast(NetIncome as Numeric) as Netincome1  from V#PayoutDetail where 1=1 And (NetIncome>0 ) And IsSentSms='N' " & Condition & " order by SessId Desc"
    '    dtData = New DataTable
    '    dtData = objDAL.GetData(sql)
    '    Dim k As Integer = dtData.Rows.Count
    '    If dtData.Rows.Count > 0 Then
    '        For i As Integer = 0 To k - 1
    '            'Dim sms As String = "Dear " & dtData.Rows(i)("MemName") & " (" & dtData.Rows(i)("IdNo") & " ) your payout for period " & dtData.Rows(i)("FromDate") & " to " & dtData.Rows(i)("ToDate") & " is Rs." & dtData.Rows(i)("NetIncome1") & " .Pls visit " & Session("CompWeb1") & " for more details."
    '            Dim sms As String = "Dear " & dtData.Rows(i)("MemName") & "-" & dtData.Rows(i)("IdNo") & ", your payout for period " & dtData.Rows(i)("FromDate") & " to " & dtData.Rows(i)("ToDate") & " is Rs." & dtData.Rows(i)("NetIncome1") & ". Pls visit " & Session("CompWeb1") & " for more details."

    '            Try
    '                Dim Mobl As String = dtData.Rows(i)("Mobl")
    '                ' baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Mobl & "&msg=" & sms & ""

    '                ' baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Mobl & "&msg=" & sms & ""
    '                'baseurl = Session("SmsAPI") & "username=" & Session("SmsId") & "&password=" & Session("SmsPass") & "&Sender=" & Session("ClientId") & "&to=" & Mobl & "&message=" & sms & "&format=text&unique=1"
    '                baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & dtData.Rows(i)("Mobl") & "&SenderId=" & Session("ClientId") & ""
    '                Data = client.OpenRead(baseurl)
    '                Dim reader As New StreamReader(Data)
    '                Dim s As String
    '                s = reader.ReadToEnd()

    '                Data.Close()
    '                reader.Close()
    '                issms += 1
    '                sql = "Update M_MonthlyPayDetail Set IsSentSms='Y' where Formno='" & dtData.Rows(i)("Formno") & "' and Sessid='" & dtData.Rows(i)("Sessid") & "';"

    '                sql = sql & "Insert into M_SmsMaster(FormNo,SmsStatus,smsDate,RecTimeStamp,ActiveStatus)values('" & dtData.Rows(i)("FormNo") & "','Payout',GetDate(),GetDate(),'Y')"

    '                ' sql = "Update M_MonthlyPayDetail Set IsSentSms='Y' where FormNo= '" & dtData.Rows(i)("Formno") & "'  AND SessId= '" & dtData.Rows(i)("Sessid") & "' "
    '                objDAL.UpdateData(sql)

    '            Catch ex As Exception
    '                'MsgBox(ex.Message)
    '            End Try
    '        Next





    '        Dim sms1 As String
    '        sms1 = dtData.Rows(0)("IsSentSms")
    '        If sms1 = "Sms Sent Successfully" Then
    '        End If
    '        btnExport.Enabled = True
    '    Else
    '        btnExport.Enabled = False
    '    End If

    'End Sub
End Class
