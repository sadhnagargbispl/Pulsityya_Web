
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net.Mail

Partial Class MemberShipReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Me.BtnSearch.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnSearch))
        If Not Page.IsPostBack Then
            If Session("Status") = "OK" Then
                BindData()
            End If
        End If
    End Sub
    Public Function GetCompID() As String
        Dim url As String = String.Empty
        Dim conn As SqlConnection

        Try
            url = HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("CPANEL.", "").Replace("LOGIN.", "")
            Dim str As String = String.Empty
            ''str = " Select ID,Logo from M_CompanyMasterNew Where IsActive = 1 And (Upper(URL) = '" & url.ToUpper().Trim() & "' OR  Upper(URL) = 'LOCALHOST') "

            If url = "LOCALHOST" Then
                str = " Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID from M_CompanyMasterNew Where IsActive = 1 And ID='" & ConfigurationManager.AppSettings("CompanyID") & "'"
            Else
                str = " Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID from M_CompanyMasterNew Where IsActive = 1 And (Upper(URL) = '" & url.ToUpper().Trim() & "') "

            End If
            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand
            conn = New SqlConnection(Application("sConnect"))
            conn.Open()
            cmd = New SqlCommand(str, conn)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("CompID") = dRead("ID")
                Session("Logo") = dRead("Logo")
                Session("WRPartyCode") = dRead("PartyCode")
                Session("CompName") = dRead("Name")
                Session("Title") = "Welcome To " & dRead("Name")
            Else
                Response.Redirect("UnderCons.aspx", False)
            End If
            dRead.Close()
            conn.Close()

        Catch ex As Exception
            If Not conn Is Nothing Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
        End Try
        GetCompID = url
    End Function
    Private Function DisableTheButton(ByVal pge As Control, ByVal btn As Control) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ")
        sb.Append("this.value = 'Please Wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(pge.Page.GetPostBackEventReference(btn))
        sb.Append(";")
        Return sb.ToString()
    End Function
    Public Sub BindData(Optional ByVal Condition As String = "")
        Dim TransactionID As String = "0"
        Dim WalletAddress As String = ""
        Dim startDate As String
        Dim endDate As String
        Dim currentDate As DateTime = DateTime.Now
        Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")
        If TxtMemID.Text <> "" Then
            TransactionID = TxtMemID.Text.Trim
        Else
            TransactionID = "0"
        End If

        If txtStartDate.Text = "" Then
            startDate = "12-oct-2017"
        Else
            startDate = txtStartDate.Text
        End If
        If txtEndDate.Text = "" Then
            endDate = formattedDate
        Else
            endDate = txtEndDate.Text
        End If
        Dim sql As String = "exec Sp_GetClubMemberShipDetail '" & TransactionID & "','" & startDate & "','" & endDate & "'"
        dtData = New DataTable
        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).tables(0)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        ViewState("WithDrawDate") = "BankCode"
        ViewState("Sort_Order") = "ASC"
        If dtData.Rows.Count > 0 Then
            btnExport.Visible = True
        Else
            btnExport.Visible = False
        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub
    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""
        BindData(Condition)
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim Condition As String = ""
            Dim dg As DataGrid = New DataGrid()
            Dim TransactionID As String = "0"
            Dim WalletAddress As String = ""
            Dim startDate As String
            Dim endDate As String
            Dim currentDate As DateTime = DateTime.Now
            Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")
            If TxtMemID.Text <> "" Then
                TransactionID = TxtMemID.Text.Trim
            Else
                TransactionID = "0"
            End If

            If txtStartDate.Text = "" Then
                startDate = "12-oct-2017"
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                endDate = formattedDate
            Else
                endDate = txtEndDate.Text
            End If
            Dim sql As String = "exec Sp_GetClubMemberShipDetail '" & TransactionID & "','" & startDate & "','" & endDate & "'"
            dtData = New DataTable
            dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).tables(0)
            dg.DataSource = dtData
            dg.DataBind()
            ExportToExcel("ClubMebershipReport.xls", dg)
        Catch ex As Exception
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
