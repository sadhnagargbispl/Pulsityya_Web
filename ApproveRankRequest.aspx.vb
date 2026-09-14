Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net.Mail

Partial Class ApproveRankRequest
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
        Me.btnApproove.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.btnApproove))
        Me.BtnReject.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnReject))
        Me.BtnSearch.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnSearch))
        Me.btnApproove.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.btnApproove))
        Me.BtnRejects.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnRejects))

        If Not Page.IsPostBack Then
            If Session("Status") = "OK" Then

                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        TxtMemID.Text = Request.QueryString("key")
                        ChkMem.Checked = True
                        BindData(" AND b.IDNo='" & Request.QueryString("key") & "'")
                    End If
                Else
                    BindData()
                End If
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
        If Trim(TxtMemID.Text) <> "" Then
            Condition = Condition & " AND MemberID = '" & Trim(TxtMemID.Text) & "'"
        End If
        
        If RbReqStatus.SelectedValue <> "A" Then
            Condition = Condition & " And Activestatus = '" & RbReqStatus.SelectedValue & "'"
        End If
        DivRemark.Visible = False
        If txtStartDate.Text <> "" Then
            If CmbType.SelectedValue = "N" Then
                Condition = Condition & " And  Cast(Convert(Varchar,RequestDate,106) as Date) >= '" & txtStartDate.Text & "'"
            ElseIf CmbType.SelectedValue = "A" Then
                Condition = Condition & " and Cast(Convert(Varchar,RequestDate,106) as Date) >= '" & txtStartDate.Text & "'"
            Else
                Condition = Condition & " and Activestatus = '" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,RecTimeStamp,106) as Date) >= '" & txtStartDate.Text & "'"
            End If
        End If
        If txtEndDate.Text <> "" Then
            If CmbType.SelectedValue = "Y" Then
                Condition = Condition & " And  Cast(Convert(Varchar,RequestDate,106) as Date) <= '" & txtEndDate.Text & "'"
            ElseIf CmbType.SelectedValue = "A" Then
                Condition = Condition & " and Cast(Convert(Varchar,RequestDate,106) as Date) <= '" & txtEndDate.Text & "'"
            Else
                Condition = Condition & " and Activestatus = '" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,RequestDate,106) as DateTime) <= '" & txtEndDate.Text & "'"
            End If
        End If
        Dim url As String = String.Empty
        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri
        url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
        url = url.ToLower
        Dim sql As String
        sql = " select * from V#Rank where 1 = 1 " & Condition & " order by RequestDate desc  "
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then
            lblMsg.Visible = False
            btnApproove.Visible = True
            'BtnRejects.Visible = True
            btnExport.Visible = True
            lblReqs.Text = dtData.Rows.Count
            'lblTotalAmont.Text = dtData.Compute("Sum(TotalAmount)", "")
            'LblTotalBV.Text = dtData.Compute("Sum(TotalBV)", "")
            'LblEmailno.Text = dtData.Rows(0)("Email")
        Else
            lblMsg.Text = "No data to display.!"
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Red
            btnApproove.Visible = False
            BtnRejects.Visible = False
            btnExport.Visible = False
            'lblReqs.Text = "0"
            'lblTotalAmont.Text = "0.00"
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
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub
    Private Sub CreateBlankTable()
        Session("SmsList") = Nothing
        Dim BlankDt As New DataTable
        BlankDt.Columns.Add("SMS")
        BlankDt.Columns.Add("Mobileno")
        BlankDt.Columns.Add("Idno")
        BlankDt.Columns.Add("ReqNo")
        BlankDt.Columns.Add("Amount")

        Session("SmsList") = BlankDt

        Dim Dt As New DataTable

        Dt.Columns.Add("SMS")
        Dt.Columns.Add("Mobileno")
        Dt.Columns.Add("Idno")
        Dt.Columns.Add("ReqNo")
        Dt.Columns.Add("Amount")

        Dim Dr As DataRow
        Dr = Dt.NewRow
        Dr("SMS") = ""
        Dr("Mobileno") = "0"
        Dr("Idno") = ""
        Dr("ReqNo") = "0"
        Dr("Amount") = "0"
        Dt.Rows.Add(Dr)


    End Sub
    Protected Sub ChkMem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkMem.CheckedChanged
        TxtMemID.Enabled = ChkMem.Checked
    End Sub
    Protected Sub btnApproove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApproove.Click
        'AprvAction("Y", "A")
        DivRemark.Visible = True
        btnApprove.Visible = True
        BtnReject.Visible = False
    End Sub
    Protected Sub BtnRejects_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRejects.Click
        DivRemark.Visible = True
        btnApprove.Visible = False
        BtnReject.Visible = True
        'AprvAction("R", "R")
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim Condition As String = ""
            DivRemark.Visible = False
            Dim dg As DataGrid = New DataGrid()
            If Trim(TxtMemID.Text) <> "" Then
                Condition = Condition & " AND MemberID = '" & Trim(TxtMemID.Text) & "'"
            End If
           
            If RbReqStatus.SelectedValue <> "A" Then
                Condition = Condition & " And Activestatus = '" & RbReqStatus.SelectedValue & "'"
            End If
            DivRemark.Visible = False
            If txtStartDate.Text <> "" Then
                If CmbType.SelectedValue = "N" Then
                    Condition = Condition & " And  Cast(Convert(Varchar,RequestDate,106) as Date) >= '" & txtStartDate.Text & "'"
                ElseIf CmbType.SelectedValue = "A" Then
                    Condition = Condition & " and Cast(Convert(Varchar,RequestDate,106) as Date) >= '" & txtStartDate.Text & "'"
                Else
                    Condition = Condition & " and Activestatus = '" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,RequestDate,106) as Date) >= '" & txtStartDate.Text & "'"
                End If
            End If
            If txtEndDate.Text <> "" Then
                If CmbType.SelectedValue = "Y" Then
                    Condition = Condition & " And  Cast(Convert(Varchar,RequestDate,106) as DateTime) <= '" & txtEndDate.Text & "'"
                ElseIf CmbType.SelectedValue = "A" Then
                    Condition = Condition & " and Cast(Convert(Varchar,RequestDate,106) as DateTime) <= '" & txtEndDate.Text & "'"
                Else
                    Condition = Condition & " and Activestatus = '" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,RecTimeStamp,106) as Date) <= '" & txtEndDate.Text & "'"
                End If
            End If
            Dim url As String = String.Empty
            Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri
            url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
            url = url.ToLower
            Dim sql As String
            sql = " select * from V#Rank where 1 = 1 " & Condition & " order by RequestDate desc  "
            Dim dtTemp As New DataTable
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)
            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("ApproveRank.xls", dg)
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
    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        If TxtARemark.Text = "" Then
            LblARemark.Visible = True
            LblARemark.Text = "Please Enter Remark"
            Exit Sub
        Else
            LblARemark.Visible = False
            AprvAction("Y", "A")
        End If
    End Sub
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click
        If TxtARemark.Text = "" Then
            LblARemark.Visible = True
            LblARemark.Text = "Please Enter Remark"
            Exit Sub
        Else
            LblARemark.Visible = False
            AprvAction("R", "R")
        End If
    End Sub
    Private Sub AprvAction(ByVal AprvType As String, ByVal ApprvStatus As String)
        Dim Query As String = ""
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim PartyCode As String = ""
        Dim OrderType As String = ""
        Dim sql As String = ""
        Dim Chk As CheckBox
        Dim sqlstr As String = ""
        Dim lbl As Label
        Dim lblformno As Label
        Dim lblIdno As Label
        Dim LblAmount As Label
        Dim lblPaymode As Label
        Dim LblOrderNo As Label
        Dim LblActype As Label
        Dim Cnt As Integer = 0
        Dim Remark As String = ""
        Dim c As Integer = 0
        Dim sessid As Integer = 0
        Dim voucherno As String = ""
        Dim dt As New DataTable
        Dim Dt1 As New DataTable
        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
            'lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
            lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
            lblformno = DirectCast(Gvr.FindControl("lblformno"), Label)
            'LblAmount = DirectCast(Gvr.FindControl("lblAmount"), Label)
            'lblPaymode = DirectCast(Gvr.FindControl("LblPaymode"), Label)
            'LblActype = DirectCast(Gvr.FindControl("lblactype"), Label)
            'LblOrderNo = DirectCast(Gvr.FindControl("LblOrderNo"), Label)
            If Chk.Checked = True And Chk.Enabled = True Then
               

                If AprvType = "Y" Then
                    'Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                    Dim TotalPV As Decimal = 0
                    Dim strSql_ As String = "Select * from IdwiserankEmall Where RankId = '" & lbl.Text & "' and formno='" & lblformno.Text & "' and Activestatus='N'"
                    Dim ds1 As New DataSet
                    ds1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strSql_)
                    If ds1.Tables(0).Rows.Count > 0 Then
                        For i As Integer = 0 To ds1.Tables(0).Rows.Count - 1
                            Query &= "Insert Into Idwiserank(Sessid,formno,Rankid,RectimeStamp,RefFormNo,Reflegno,ACtiveStatus) "
                            Query &= "Select Sessid,formno,Rankid,RectimeStamp,RefFormNo,0,'Y' from IdwiserankEmall where rankid=5 and formno='" & lblformno.Text & "' and ACtiveStatus='N';"
                            Query &= " Update IdwiserankEmall set ActiveStatus='" & AprvType & "',Date=getdate(), remark='" & TxtARemark.Text & "' where Rankid='" & lbl.Text & "' and formno='" & lblformno.Text & "' "
                            Query &= "Update M_MemberMaster Set Planid='" & lbl.Text & "'  where formno='" & lblformno.Text & "'; "

                            c = c + 1
                        Next
                    End If
                    sqlstr = " Begin Try   Begin Transaction " & Query & " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH"
                End If
                Cnt = Cnt + 1
            End If
        Next
        Dim a As Integer
        If (sqlstr > "") Then
            a = objDAL.UpdateData(sqlstr)
        End If
        Dim MsgTxt As String = ""
        If AprvType = "Y" Then
            MsgTxt = "Approved"
        Else : MsgTxt = "Rejected"
        End If
        If (a > 0) Then
            Session("remark") = TxtARemark.Text
            Session("GETDATE") = DateTime.Now
            lblMsg.Text = "" & Cnt & " Requests " & MsgTxt & " Successfully."
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Green
            TxtARemark.Text = ""
            BindData()
        Else
            lblMsg.Text = " Request Already  " & MsgTxt
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Red
        End If
    End Sub
End Class
