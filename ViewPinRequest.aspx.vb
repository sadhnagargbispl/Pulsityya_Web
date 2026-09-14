Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class ViewPinRequest
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
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
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

    Public Sub BindData(Optional ByVal Condition As String = "")

       
        If ChkMem.Checked = True And Trim(TxtMemID.Text) <> "" Then
            Condition = Condition & " AND b.IdNo='" & Trim(TxtMemID.Text) & "'"
        End If
        If txtStartDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"

        End If
        If txtEndDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,a.RecTimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"

        End If
        'If RbReqStatus.SelectedValue = "P" Then
        '    btnApproove.Visible = True : BtnReject.Visible = True
        'ElseIf RbReqStatus.SelectedValue = "A" Then
        '    btnApproove.Visible = False : BtnReject.Visible = True
        'ElseIf RbReqStatus.SelectedValue = "R" Then
        '    btnApproove.Visible = True : BtnReject.Visible = False
        'End If

        Dim sql As String = "Select Cast(a.FormNo as varchar) as FormNo,a.ReqNo,b.IDNo,RTRIM(b.MemFirstName +' ' + b.MemLastName) as MemName,b.Mobl as MobileNo,a.TotalQty,a.WalletAmt,a.BankAmt,a.OtherAmt,a.TotalAmount,a.Remarks From TrnPinReqMain a,M_MemberMaster b Where a.FormNo=b.FormNo AND a.IsApprove='Y' " & Condition & ";"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""

        'If ChkMem.Checked = True Then
        '    Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
        'End If
        BindData(Condition)
    End Sub

    Private Sub AprvAction(ByVal AprvType As String)
        Dim sql As String = ""
        Dim Chk As CheckBox
        Dim lbl As Label
        Dim Cnt As Integer = 0

        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
            If Chk.Checked = True Then
                sql = sql & ";Update TrnPinReqMain SET IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApprovedBy='" & Val(Session("UserID")) & "' where FormNo='" & lbl.Text & "'"
                Cnt = Cnt + 1
            End If
        Next
        Dim a As Integer = objDAL.UpdateData(sql)
        Dim MsgTxt As String = ""
        If AprvType = "Y" Then
            MsgTxt = "Approved"
        Else : MsgTxt = "Rejected"
        End If
        If a <> 0 And Cnt > 0 Then
            lblMsg.Text = "" & Cnt & " Requests " & MsgTxt & " Successfully."
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Green
            BindData()
        Else
            lblMsg.Text = "Not " & MsgTxt
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Protected Sub ChkMem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkMem.CheckedChanged
        TxtMemID.Enabled = ChkMem.Checked
    End Sub

   
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim condition As String = ""
        Dim dtTemp As New DataTable
        Dim dg As New DataGrid
        Try
            If ChkMem.Checked = True And Trim(TxtMemID.Text) <> "" Then
                condition = condition & " AND b.IdNo='" & Trim(TxtMemID.Text) & "'"
            End If
            If txtStartDate.Text <> "" Then

                condition = condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"

            End If
            If txtEndDate.Text <> "" Then

                condition = condition & " And  Cast(Convert(Varchar,a.RecTimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"

            End If
           

            Dim sql As String = "Select b.IDNo,RTRIM(b.MemFirstName +' ' + b.MemLastName) as MemName,b.Mobl as MobileNo,a.ReqNo,a.TotalAmount,a.WalletAmt,a.BankAmt,a.OtherAmt,a.Remarks From TrnPinReqMain a,M_MemberMaster b Where a.FormNo=b.FormNo AND a.IsApprove='Y' " & condition & ";"

            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("SentPinDetail.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
        'Comm.Cancel()
        'ds.Dispose()

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
     
    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
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
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintCurrent.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("GData")
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
        dtData = Session("GData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

End Class
