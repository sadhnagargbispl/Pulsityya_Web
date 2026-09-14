Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports System.Net.Mail

Partial Class WithoutApproveProductReport
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
            Else
                BindData()
            End If

        End If
    End Sub
    Private Shared Function Base64Encode(ByVal plainText As String) As String
        Dim plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText)
        Return System.Convert.ToBase64String(plainTextBytes)
    End Function

    Private Shared Function Base64Decode(ByVal base64EncodedData As String) As String
        Dim base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData)
        Return System.Text.Encoding.UTF8.GetString(base64EncodedBytes)
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
    Public Sub BindData(Optional ByVal Condition As String = "")
        If Trim(TxtMemID.Text) <> "" Then
            Condition = Condition & " AND MemberID = '" & Trim(TxtMemID.Text) & "'"
        End If
        If Trim(TxtTransactionNo.Text) <> "" Then
            If DDlByWalletSelect.SelectedValue = "N" Then
                Condition = Condition & " AND OrderNo = '" & Trim(TxtTransactionNo.Text) & "'"
            Else
                Condition = Condition & " AND TransactionNo = '" & Trim(TxtTransactionNo.Text) & "'"
            End If

        End If
        'If DDlSelectTypeFordis.SelectedValue = "C" Then
        '    Condition = Condition & " And IsApprove = 'Y'"
        'Else
        '    Condition = Condition & " And DispStatus not in ('C','R') And IsApprove <> 'Y' "
        'End If
        If RbReqStatus.SelectedValue <> "A" Then
            Condition = Condition & " And DispStatus = '" & RbReqStatus.SelectedValue & "'"
        End If

        'End If
        DivRemark.Visible = False
        If txtStartDate.Text <> "" Then
            If CmbType.SelectedValue = "N" Then
                Condition = Condition & " And  Cast(Convert(Varchar,RecTimeStamp,106) as Date) >= '" & txtStartDate.Text & "'"
            ElseIf CmbType.SelectedValue = "A" Then
                Condition = Condition & " and Cast(Convert(Varchar,RecTimeStamp,106) as Date) >= '" & txtStartDate.Text & "'"
            Else
                Condition = Condition & " and Isapprove = '" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,RecTimeStamp,106) as Date) >= '" & txtStartDate.Text & "'"
            End If
        End If
        If txtEndDate.Text <> "" Then
            If CmbType.SelectedValue = "Y" Then
                Condition = Condition & " And  Cast(Convert(Varchar,RecTimeStamp,106) as Date) <= '" & txtEndDate.Text & "'"
            ElseIf CmbType.SelectedValue = "A" Then
                Condition = Condition & " and Cast(Convert(Varchar,RecTimeStamp,106) as Date) <= '" & txtEndDate.Text & "'"
            Else
                Condition = Condition & " and Isapprove = '" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,RecTimeStamp,106) as Date) <= '" & txtEndDate.Text & "'"
            End If
        End If
        Dim url As String = String.Empty
        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri
        url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
        url = url.ToLower
        Dim sql As String
        If DDlByWalletSelect.SelectedValue = "N" Then
            sql = " select * from V#ProductRequestByWallet where 1 = 1 AND RectimeStamp NOT BETWEEN '2024-11-01' and '2025-04-30' " & Condition & " order by RectimeStamp desc  "
        Else
            sql = " select * from V#ProductRequest where 1 = 1 AND RectimeStamp NOT BETWEEN '2024-11-01' and '2025-04-30' " & Condition & " order by RectimeStamp desc  "
        End If
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        If DDlByWalletSelect.SelectedValue <> "N" Then
            GvData.Columns(6).Visible = True
            GvData.Columns(7).Visible = True
            GvData.Columns(8).Visible = True
            'GvData.Columns(16).Visible = True
            GvData.Columns(17).Visible = True
        Else
            GvData.Columns(6).Visible = False
            GvData.Columns(7).Visible = False
            GvData.Columns(8).Visible = False
            'GvData.Columns(16).Visible = False
            GvData.Columns(17).Visible = False
        End If
        Session("GData") = dtData
        If dtData.Rows.Count > 0 Then
            lblMsg.Visible = False
            'If DDlByWalletSelect.SelectedValue = "N" Then
            '    If DDlSelectTypeFordis.SelectedValue = "C" Then
            '        btnApproove.Visible = False
            '        BtnRejects.Visible = False
            '        btnExport.Visible = True
            '    Else
            '        btnApproove.Visible = False
            '        BtnRejects.Visible = False
            btnExport.Visible = True
            '    End If
            'Else
            '    btnApproove.Visible = True
            '    BtnRejects.Visible = True
            '    btnExport.Visible = True
            'End If
            lblReqs.Text = dtData.Rows.Count
            lblTotalAmont.Text = dtData.Compute("Sum(TotalAmount)", "")
            LblTotalBV.Text = dtData.Compute("Sum(TotalBV)", "")
            LblEmailno.Text = dtData.Rows(0)("Email")
        Else
            lblMsg.Text = "No data to display.!"
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Red
            btnApproove.Visible = False
            BtnRejects.Visible = False
            btnExport.Visible = False
            lblReqs.Text = "0"
            lblTotalAmont.Text = "0.00"
        End If
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim Condition As String = ""
            DivRemark.Visible = False
            Dim dg As DataGrid = New DataGrid()
            If Trim(TxtMemID.Text) <> "" Then
                Condition = Condition & " AND MemberID = '" & Trim(TxtMemID.Text) & "'"
            End If
            If Trim(TxtTransactionNo.Text) <> "" Then
                If DDlByWalletSelect.SelectedValue = "N" Then
                    Condition = Condition & " AND OrderNo = '" & Trim(TxtTransactionNo.Text) & "'"
                Else
                    Condition = Condition & " AND TransactionNo = '" & Trim(TxtTransactionNo.Text) & "'"
                End If
            End If
            If RbReqStatus.SelectedValue <> "A" Then
                If DDlByWalletSelect.SelectedValue = "N" Then
                    Condition = Condition & " And DispStatus = '" & RbReqStatus.SelectedValue & "'"
                Else
                    Condition = Condition & " And IsApprove = '" & RbReqStatus.SelectedValue & "'"
                End If
            End If
            DivRemark.Visible = False
            If txtStartDate.Text <> "" Then
                If CmbType.SelectedValue = "N" Then
                    Condition = Condition & " And  Cast(Convert(Varchar,RecTimeStamp,106) as Date) >= '" & txtStartDate.Text & "'"
                ElseIf CmbType.SelectedValue = "A" Then
                    Condition = Condition & " and Cast(Convert(Varchar,RecTimeStamp,106) as Date) >= '" & txtStartDate.Text & "'"
                Else
                    Condition = Condition & " and Isapprove = '" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,RecTimeStamp,106) as Date) >= '" & txtStartDate.Text & "'"
                End If
            End If
            If txtEndDate.Text <> "" Then
                If CmbType.SelectedValue = "Y" Then
                    Condition = Condition & " And  Cast(Convert(Varchar,RecTimeStamp,106) as Date) <= '" & txtEndDate.Text & "'"
                ElseIf CmbType.SelectedValue = "A" Then
                    Condition = Condition & " and Cast(Convert(Varchar,RecTimeStamp,106) as Date) <= '" & txtEndDate.Text & "'"
                Else
                    Condition = Condition & " and Isapprove = '" & CmbType.SelectedValue & "' And Cast(Convert(Varchar,RecTimeStamp,106) as Date) <= '" & txtEndDate.Text & "'"
                End If
            End If
            Dim url As String = String.Empty
            Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri
            url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")
            url = url.ToLower
            Dim sql As String
            If DDlByWalletSelect.SelectedValue = "N" Then
                sql = " select MemberID,MemberName,''''+convert(varchar,orderno) as orderno,RequestDate,TotalAmount,TotalAmount,UserAddress,City,District,PinCode,UserState"
                sql &= " from V#ProductRequestByWallet where 1 = 1 AND RectimeStamp NOT BETWEEN '2024-11-01' and '2025-04-30' " & Condition & " order by RecTimeStamp desc  "
            Else
                sql = " select MemberID,MemberName,''''+convert(varchar,orderno) as orderno,RequestDate,PaymentMode,''''+convert(varchar,TransactionNo) as TransactionNo,ChequeDate as Transactiondate,TotalAmount,TotalAmount,UserAddress,City,District,PinCode,UserState"
                sql &= " from V#ProductRequest where 1 = 1 AND RectimeStamp NOT BETWEEN '2024-11-01' and '2025-04-30' " & Condition & " order by RecTimeStamp desc  "
            End If
            Dim dtTemp As New DataTable
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)
            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("ApproveProduct.xls", dg)
        Catch ex As Exception
        End Try

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
        Dim lblReqno As Label
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
            lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
            lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
            LblAmount = DirectCast(Gvr.FindControl("lblAmount"), Label)
            lblPaymode = DirectCast(Gvr.FindControl("LblPaymode"), Label)
            LblActype = DirectCast(Gvr.FindControl("lblactype"), Label)
            LblOrderNo = DirectCast(Gvr.FindControl("LblOrderNo"), Label)
            If Chk.Checked = True And Chk.Enabled = True Then
                Dt1 = New DataTable
                If Dt1.Rows.Count > 0 Then
                    'Query = "; Exec [DispatchOrder] '" & LblOrderNo.Text & "','" & PartyCode & "','0','','','','" & txtDispatchDate.Text & "','" & DDlDispatchStatus.SelectedValue & "','" & TxtDispatchRemark.Text & "','" & DDlDispatchStatus.SelectedValue & "' "
                    'sqlstr = " Begin Try   Begin Transaction " & Query & " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH"
                Else
                    Dim strSql As String = "Select Count(*) As Cnt  from TrnProductorderDetail Where orderno = '" & LblOrderNo.Text & "' and IsApprove <> 'N'"
                    Dt1 = New DataTable
                    Dt1 = objDAL.GetData(strSql)
                    If (Val(Dt1.Rows(0)("Cnt")) = 0) Then
                        Dim KitName1 As String = ""
                        Dim KitID1 As Integer = 0
                        Dim dtKit As DataTable = New DataTable
                        If LblActype.Text.ToString().ToUpper() = "A" Then
                            Dim sqlStr1 As String = " select KitId,KitName from m_Kitmaster where  TopUpSeq> 0"
                            sqlStr1 &= " and BV<='" & LblAmount.Text & "'  And BV > 0 and Activestatus='Y' Order by KitAmount Desc"
                            dtKit = objDAL.GetData(sqlStr1)
                            If (dtKit.Rows.Count > 0) Then
                                KitName1 = dtKit.Rows(0)("KitName")
                                KitID1 = dtKit.Rows(0)("KitId")
                            Else
                                KitName1 = ""
                                KitID1 = 0
                            End If
                        End If
                        Query = "select sum(RepurchaseWallet)As RepurchaseWallet,sum(Sessid)as Sessid "
                        Query &= "from(Select Balance as RepurchaseWallet,0 as Sessid From dbo.ufnGetBalance(" & lbl.Text & ",'R')"
                        Query &= "Union all "
                        Query &= " Select 0 as RepurchaseWallet,Max(Sessid)as Sessid FROM M_SessnMaster) as Temp"
                        dt = objDAL.GetData(Query)
                        If dt.Rows.Count > 0 Then
                            sessid = dt.Rows(0)("Sessid")
                        Else
                            sessid = 0
                        End If
                        dt = New DataTable
                        sql = "Select Address1,ActiveStatus,Kitid FROM M_MemberMaster where Formno = '" & lbl.Text & "'"
                        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                        dt = New DataTable
                        dt = objDAL.GetData(sql)
                        If dt.Rows.Count > 0 Then
                            Session("MemAddress") = dt.Rows(0)("Address1")
                            Session("ProductStatus") = dt.Rows(0)("ActiveStatus")
                            Session("ProductKitid") = dt.Rows(0)("Kitid")
                        End If
                        Remark = " Approve Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                        Dim TotalPV As Decimal = 0
                        Dim strSql_ As String = "Select * from TrnProductorderDetail Where orderno = '" & LblOrderNo.Text & "' and IsApprove = 'N'"
                        Dim ds1 As New DataSet
                        ds1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strSql_)
                        PartyCode = Session("WR")
                        If Session("ProductStatus") = "Y" Then
                            OrderType = "O"
                        Else
                            OrderType = "T"
                        End If
                        If AprvType = "Y" Then
                            If lblPaymode.Text = "Cash" Then
                                Dim Sqlstr1 As String = " Select Isnull(Sum(PVValue),0)  as  PVValue from Repurchincome Where FormNo = '" & lbl.Text & "'"
                                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                                Dim Dt__ As DataTable = New DataTable
                                Dt__ = objDAL.GetData(Sqlstr1)
                                If (Session("ProductStatus") = "N") Then
                                    Query &= "; Declare @BV numeric(18,2);Declare @PV numeric(18,2); Select @BV=isnull(sum(BV),0),@PV=isnull(sum(PV),0) FROM TrnProductorderDetail WHERE OrderNo = '" & LblOrderNo.Text & "' and IsApprove = 'N' "
                                    Query &= ";Insert into Repurchincome(Sessid,Formno,BillNo,Billdate,Repurchincome,Imported,BillType,SoldBy,MSessid,"
                                    Query &= "KitId,Dsessid,Remarks,PVvalue,BType,FromID)"
                                    Query &= " Select '" & sessid & "','" & lbl.Text & "','Order " & LblOrderNo.Text & "',Getdate(),@BV,'N','A','" & PartyCode & "',"
                                    Query &= "isnull(Max(Sessid),1) ,0,Convert(varchar,Getdate(),112),'',@PV,'A','" & lbl.Text & "' from M_MonthSessnMaster"
                                    If (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) >= 4950) Then
                                        Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,4;"
                                    ElseIf (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) >= 1975) And (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) < 4950) Then
                                        Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,5;"
                                    Else
                                        Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,4;"
                                    End If
                                ElseIf Session("ProductStatus") = "Y" Then
                                    If Val(Session("ProductKitid")) = "5" Then
                                        Query &= "; Declare @BV numeric(18,2);Declare @PV numeric(18,2); Select @BV=isnull(sum(BV),0),@PV=isnull(sum(PV),0) FROM TrnProductorderDetail WHERE OrderNo = '" & LblOrderNo.Text & "' and IsApprove = 'N' "
                                        Query &= ";Insert into Repurchincome(Sessid,Formno,BillNo,Billdate,Repurchincome,Imported,BillType,SoldBy,MSessid,"
                                        Query &= "KitId,Dsessid,Remarks,PVvalue,BType)"
                                        Query &= " Select '" & sessid & "','" & lbl.Text & "','Order " & LblOrderNo.Text & "',Getdate(),0,'N','A','" & PartyCode & "',"
                                        Query &= "isnull(Max(Sessid),1) ,0,Convert(varchar,Getdate(),112),'',@PV,'A' from M_MonthSessnMaster"
                                        If (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) >= 4950) Then
                                            Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,4;"
                                        ElseIf (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) >= 1975) And (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) < 4950) Then
                                            Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,5;"
                                        End If
                                    Else
                                        Query &= "; Declare @BV numeric(18,2);Declare @PV numeric(18,2); Select @BV=isnull(sum(BV),0),@PV=isnull(sum(PV),0) FROM TrnProductorderDetail WHERE OrderNo = '" & LblOrderNo.Text & "' and IsApprove = 'N' "
                                        Query &= ";Insert into Repurchincome(Sessid,Formno,BillNo,Billdate,Repurchincome,Imported,BillType,SoldBy,MSessid,"
                                        Query &= "KitId,Dsessid,Remarks,PVvalue,FromID)"
                                        Query &= " Select '" & sessid & "','" & lbl.Text & "','Order " & LblOrderNo.Text & "',Getdate(),@BV,'N','R','" & PartyCode & "',"
                                        Query &= "isnull(Max(Sessid),1) ,0,Convert(varchar,Getdate(),112),'',@PV,'" & lbl.Text & "' from M_MonthSessnMaster"
                                    End If
                                End If
                                Query &= " Update TrnProductorderDetail Set IsApprove = 'Y',Remark = '" & Remark & "',ApproveRemark = '" & TxtARemark.Text & "',"
                                Query &= "Approvedate = Getdate() where OrderNo = '" & LblOrderNo.Text & "'; "
                                'Query &= "; Exec [DispatchOrder] '" & LblOrderNo.Text & "','" & PartyCode & "','0','','','','" & txtDispatchDate.Text & "','" & DDlDispatchStatus.SelectedValue & "','" & TxtDispatchRemark.Text & "','C' "
                                sqlstr = " Begin Try   Begin Transaction " & Query & " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH"
                            Else

                                If ds1.Tables(0).Rows.Count > 0 Then
                                    For i As Integer = 0 To ds1.Tables(0).Rows.Count - 1
                                        Query &= " Insert Into TrnorderDetail(OrderNo,FormNo,ProductID,Qty,Rate,NetAmount,RecTimeStamp,DispDate,DispStatus,DispQty,"
                                        Query &= " RemQty,DispAmt,MRP,DP,ProductName,ImgPath,RP,BV,FSEssId,Prodtype,PV,UserTypeAD)"
                                        Query &= " Select '" & LblOrderNo.Text & "','" & lbl.Text & "',prodid,'" & Val(ds1.Tables(0).Rows(i)("qty")) & "',DP,"
                                        Query &= "DP*" & Val(ds1.Tables(0).Rows(i)("qty")) & ",getDate(),'','N',0,'" & Val(ds1.Tables(0).Rows(i)("qty")) & "',"
                                        Query &= "0,MRP,Dp,ProductName,'',0,BV,(Select ISNULL(Max(FsessID),1) "
                                        Query &= "FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_FiscalMaster ),'P',PV,'W'"
                                        Query &= " From " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster where"
                                        Query &= " ActiveStatus='Y' and OnWebsite='Y' and Prodid = '" & ds1.Tables(0).Rows(i)("ProductID") & "';"
                                        c = c + 1
                                    Next
                                End If
                                Query &= "Insert INTO TrnOrder(OrderNo,OrderDate,MemFirstName,MemLastName,Address1,Address2,CountryID,CountryName,StateCode,City,PinCode,"
                                Query &= " Mobl,EMail,FormNo,UserType,Passw,PayMode,ChDDNo,ChDate,ChAmt,BankName,BranchName,Remark,OrderAmt,OrderItem,OrderQty,ActiveStatus,"
                                Query &= "HostIp, RecTimeStamp, IsTransfer, DispatchDate, DispatchStatus, DispatchQty, RemainQty,DispatchAmount,Shipping,SessID,RewardPoint,"
                                Query &= "CourierName, DocketNo, OrderFor, IsConfirm, OrderType, Discount, OldShipping, ShippingStatus,IdNo,FSessId,BankAmt,OtherAmt,WalletAmt,"
                                Query &= "TravelPoint,KitName,ForVadicGurukul)"
                                Query &= " select '" & LblOrderNo.Text & "',Cast(Convert(varchar,GETDATE(),106) as Datetime),MemFirstName , MemLastName ,address1 , Address2 , "
                                Query &= "CountryID , CountryName , StateCode , City , Case when PinCode='' then 0 else Pincode  end as Pincode ,Mobl, EMail ,'" & lbl.Text & "','', "
                                Query &= "Passw ,'',0,'',0,'','','" & TxtARemark.Text & "','0','0','0','Y','H',Getdate(),'Y','','N',0,'0',0,0,"
                                Query &= "'" & Val(sessid) & "',0,'',0,'','Y','" & OrderType & "',0,'" & lbl.Text & "','Y','" & lblIdno.Text & "','1',"
                                Query &= "'0','0','0',0,'" & KitName1 & "','N' from M_memberMaster where formno = '" & lbl.Text & "'"
                                Query &= ";UPDATE TrnOrder SET OrderAmt=OrderAmount,OrderItem=b.OrderItem,OrderQty=b.OrderQty,RemainQty=b.OrderQty,"
                                Query &= "BV=BVV_,WalletAmt=OrderAmount,PV = PVV_,Shipping=b.ShippingAmt FROM trnOrder a, (Select Count(*) OrderItem, SUM(Qty) as OrderQty, "
                                Query &= "SUM(NetAmount) as OrderAmount,SUM(b.BV *a.Qty) as BVV_,SUM(b.PV *a.Qty) as PVV_ ,sum(a.Qty) as ShippingAmt FROM TrnOrderDetail a,"
                                Query &= "" & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster b WHERE a.ProductID=b.ProdID "
                                Query &= "AND a.OrderNo='" & LblOrderNo.Text & "' And a.Formno = '" & lbl.Text & "' ) b "
                                Query &= "WHERE a.OrderNo = '" & LblOrderNo.Text & "' And a.Formno = '" & lbl.Text & "';"
                                Query &= "Declare @BV numeric(18,2);Declare @PV numeric(18,2); Select @BV=BV,@PV= PV FROM TrnOrder WHERE OrderNo = '" & LblOrderNo.Text & "'; "
                                Query &= " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,Memberid)"
                                Query &= "Values('" & lbl.Text & "','" & lblIdno.Text & "','Product Request','Product Request',"
                                Query &= "' Product Request For Order No " & LblOrderNo.Text & " ',Getdate()," & lbl.Text & ");"
                                Query &= " Insert into " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnPaymentConfirmation(SNo,ConfirmBy,OrderNo,FormNo,"
                                Query &= "OrderAmt, IsConfirm, RecTimeStamp, UserID, OrderFor,IDNO,ActiveStatus,OrdType,FSessId)"
                                Query &= "select Case When Max(SNo) Is Null Then '1001' Else Max(SNo)+1 END as SNo,'','" & LblOrderNo.Text & "','" & lbl.Text & "',"
                                Query &= "'" & LblAmount.Text & "','Y',Getdate(),0,'','" & lblIdno.Text & "','Y','D',1 from "
                                Query &= " " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnPaymentConfirmation"
                                Dim Sqlstr1 As String = " Select Isnull(Sum(PVValue),0)  as  PVValue from Repurchincome Where FormNo = '" & lbl.Text & "'"
                                objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                                Dim Dt__ As DataTable = New DataTable
                                Dt__ = objDAL.GetData(Sqlstr1)
                                If (Session("ProductStatus") = "N") Then
                                    Query &= ";Insert into Repurchincome(Sessid,Formno,BillNo,Billdate,Repurchincome,Imported,BillType,SoldBy,MSessid,"
                                    Query &= "KitId,Dsessid,Remarks,PVvalue,BType,FromID)"
                                    Query &= " Select '" & sessid & "','" & lbl.Text & "','Order " & LblOrderNo.Text & "',Getdate(),@BV,'N','A','" & PartyCode & "',"
                                    Query &= "isnull(Max(Sessid),1) ,0,Convert(varchar,Getdate(),112),'',@PV,'A','" & lbl.Text & "' from M_MonthSessnMaster"
                                    If (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) >= 4950) Then
                                        Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,4;"
                                    ElseIf (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) >= 1975) And (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) < 4950) Then
                                        Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,5;"
                                    Else
                                        Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,4;"
                                    End If
                                ElseIf Session("ProductStatus") = "Y" Then
                                    If Val(Session("ProductKitid")) = "5" Then
                                        Query &= ";Insert into Repurchincome(Sessid,Formno,BillNo,Billdate,Repurchincome,Imported,BillType,SoldBy,MSessid,"
                                        Query &= "KitId,Dsessid,Remarks,PVvalue,BType)"
                                        Query &= " Select '" & sessid & "','" & lbl.Text & "','Order " & LblOrderNo.Text & "',Getdate(),0,'N','A','" & PartyCode & "',"
                                        Query &= "isnull(Max(Sessid),1) ,0,Convert(varchar,Getdate(),112),'',@PV,'A' from M_MonthSessnMaster"
                                        If (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) >= 4950) Then
                                            Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,4;"
                                        ElseIf (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) >= 1975) And (Val(Val(Dt__.Rows(0)("Pvvalue")) + Val(TotalPV)) < 4950) Then
                                            Query &= " Exec Sp_ActivateMember_New '" & lblIdno.Text & "','" & LblOrderNo.Text & "'," & Val(Dt__.Rows(0)("Pvvalue")) & ",@BV,5;"
                                        End If
                                    Else
                                        Query &= ";Insert into Repurchincome(Sessid,Formno,BillNo,Billdate,Repurchincome,Imported,BillType,SoldBy,MSessid,"
                                        Query &= "KitId,Dsessid,Remarks,PVvalue,FromID)"
                                        Query &= " Select '" & sessid & "','" & lbl.Text & "','Order " & LblOrderNo.Text & "',Getdate(),@BV,'N','R','" & PartyCode & "',"
                                        Query &= "isnull(Max(Sessid),1) ,0,Convert(varchar,Getdate(),112),'',@PV,'" & lbl.Text & "' from M_MonthSessnMaster"
                                    End If
                                End If
                                Query &= " Update TrnProductorderDetail Set IsApprove = 'Y',Remark = '" & Remark & "',ApproveRemark = '" & TxtARemark.Text & "',"
                                Query &= " Approvedate = Getdate() where OrderNo = '" & LblOrderNo.Text & "'; "
                                ' Query &= "; Exec [DispatchOrder] '" & LblOrderNo.Text & "','" & PartyCode & "','0','','','','" & txtDispatchDate.Text & "','" & DDlDispatchStatus.SelectedValue & "','" & TxtDispatchRemark.Text & "','C' "
                                sqlstr = " Begin Try   Begin Transaction " & Query & " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH"
                            End If

                        Else
                            Remark = " Reject Payment Request On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
                            Query &= " Update TrnProductorderDetail Set IsApprove = 'R',Remark = '" & Remark & "',ApproveRemark = '" & TxtARemark.Text & "',"
                            Query &= "Rejectdate = getdate() where OrderNo = '" & LblOrderNo.Text & "'; "
                            'Query &= "; Exec [DispatchOrder] '" & LblOrderNo.Text & "','" & PartyCode & "','0','','','','" & txtDispatchDate.Text & "','" & DDlDispatchStatus.SelectedValue & "','" & TxtDispatchRemark.Text & "','R' "
                            sqlstr = " Begin Try   Begin Transaction " & Query & " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH"
                        End If
                        Cnt = Cnt + 1
                    End If
                End If
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
    Protected Sub DDlByWalletSelect_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlByWalletSelect.SelectedIndexChanged
        If DDlByWalletSelect.SelectedValue <> "N" Then
            Response.Redirect("WithoutApproveProductReport.aspx")
        Else
            BindData()
        End If

    End Sub
    'Protected Sub DDlSelectTypeFordis_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlSelectTypeFordis.SelectedIndexChanged
    '    BindData()
    'End Sub
End Class
