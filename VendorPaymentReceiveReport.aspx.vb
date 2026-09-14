Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net.Mail

Partial Class VendorPaymentReceiveReport
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
        Dim str = "exec('Create table Trnbillrejectbyadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
 "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnbillrejectbyadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
        Dim i As Integer = 0
        i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)

        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                HdnCheckTrnns.Value = GenerateRandomString(6)
                BindData()
            End If
        End If
    End Sub
    Public Function GenerateRandomString(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function
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
    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As EventArgs)
        Dim sqlStr As String = ""
        Dim sqlRes As String = ""
        Dim HdnBillNo As String, HdnReqNo As String, HdnAmount As String, Lblidno As String, Lblformno As String, LblShopname As String, scrName As String
        Try
            Dim gvRw As GridViewRow = CType(CType(sender, System.Web.UI.Control).NamingContainer, GridViewRow)
            HdnBillNo = CType(gvRw.FindControl("HdnBillNo"), Label).Text
            HdnReqNo = CType(gvRw.FindControl("HdnReqNo"), Label).Text
            HdnAmount = CType(gvRw.FindControl("HdnAmount"), Label).Text
            Lblidno = CType(gvRw.FindControl("Lblidno"), Label).Text
            Lblformno = CType(gvRw.FindControl("Lblformno"), Label).Text
            LblShopname = CType(gvRw.FindControl("LblShopname"), Label).Text
            Dim Sql_ = "Select * from BillUploadReq where 1 = 1 AND IsApprove = 'Y' AND Chqno = '" & HdnBillNo & "'"
            Dim dt As New DataTable()
            dt = objDAL.GetData(Sql_)
            If dt.Rows.Count > 0 Then
                Dim updateeffect_ As Integer = 0
                Dim StrSql As String = "Insert into Trnbillrejectbyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
                updateeffect_ = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, StrSql)
                If updateeffect_ > 0 Then
                    Dim sql As String = "EXEC SavebillPaymentRecevieved '" & Lblidno.Trim() & "','" & HdnBillNo & "','" & HdnAmount & "',"
                    sql &= "'" & Lblformno & "','" & LblShopname & "'"
                    Dim i As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql))
                    If i > 0 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Vendor Bill Approve Successfully.!');location.replace('VendorPaymentReceiveReport.aspx');", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Approve Failed.!');location.replace('VendorPaymentReceiveReport.aspx');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Something Went Wrong.');location.replace('VendorPaymentReceiveReport.aspx');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Please Bill Status Update');location.replace('VendorPaymentReceiveReport.aspx');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('" & ex.Message & "');location.replace('VendorPaymentReceiveReport.aspx');", True)
        End Try
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        Try
            Dim startDate As String
            Dim endDate As String
            Dim id As String
            Dim BillNo As String
            Dim currentDate As DateTime = DateTime.Now
            Dim formattedDate As String = currentDate.ToString("dd-MMM-yyyy")

            If String.IsNullOrEmpty(txtStartDate.Text) Then
                startDate = "12-oct-2017"
            Else
                startDate = txtStartDate.Text
            End If

            If String.IsNullOrEmpty(txtEndDate.Text) Then
                endDate = formattedDate
            Else
                endDate = txtEndDate.Text
            End If

            If String.IsNullOrEmpty(txtBillNo.Text) Then
                BillNo = ""
            Else
                BillNo = txtBillNo.Text
            End If
            If String.IsNullOrEmpty(TxtMemID.Text) Then
                id = ""
            Else
                id = TxtMemID.Text
            End If

            Dim dtData As New DataTable()
            Dim strquery = " Exec Sp_GetVendorBillAdminReport '" & id & "','" & BillNo & "','" & startDate & "', '" & endDate & "','" & RbtStatus.SelectedValue & "'"
            dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strquery).Tables(0)
            Session("ReceivedPin") = dtData
            GvData.DataSource = dtData
            GvData.DataBind()

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") & Environment.NewLine
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("ReceivedPin")
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
            If Trim(TxtMemID.Text) <> "" Then
                Condition = Condition & " AND idno = '" & Trim(TxtMemID.Text) & "'"
            End If
            If txtStartDate.Text <> "" Then
                Condition = Condition & " and Cast(Convert(Varchar,RedistartionDate,106) as Date) >= '" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " and Cast(Convert(Varchar,RedistartionDate,106) as Date) <= '" & txtEndDate.Text & "'"
            End If
            Dim url As String = String.Empty
            Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri
            url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
            url = url.ToLower
            Dim sql As String
            sql = "Select Sno as [Sr.No],idno as [Member ID],memfirstname as [Member Name],RedistartionDate as [Date],ShopName as [Vendor Name], "
            sql &= "MobileNo as [Mobile No],CatName as [Category Name],SubCatName as [Sub Category Category],City,Statename as [State],PinCode as [Pin Code],"
            sql &= "Commission,Cashback,ActiveStatus as [Status] from V#VendorReport where 1 = 1 " & Condition & " order by RedistartionDate desc  "
            Dim dtTemp As New DataTable
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)
            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("VendorReport.xls", dg)
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
