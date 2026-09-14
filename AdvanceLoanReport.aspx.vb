Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_WalletTransactionReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
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
            Session("AccountData") = Nothing

            'FillKit()
        End If
        If Session("Compid") = "1066" Then
            BtnAddNew.Visible = False
        End If
    End Sub




    Private Function GetFormNo() As String
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMemberId.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from " & objDAL.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMemberId.Text = ""
        End If
        Return formno
    End Function

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("AccountData")
        GvData.DataBind()
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("CompID") = 1066 Then
                If DirectCast(e.Row.FindControl("LblRefNo"), Label).Text = "0" Then
                    ' DirectCast(e.Row.FindControl("LBModify"), Label).Visible = False
                    DirectCast(e.Row.FindControl("PanlModify"), Panel).Visible = False
                Else
                    'DirectCast (e.Row.Page.FindControl("DivModfy").Visible) = True
                    DirectCast(e.Row.FindControl("PanlModify"), Panel).Visible = False


                End If
            Else
                If DirectCast(e.Row.FindControl("LblRefNo"), Label).Text = "0" Then
                    ' DirectCast(e.Row.FindControl("LBModify"), Label).Visible = False
                    DirectCast(e.Row.FindControl("PanlModify"), Panel).Visible = False
                Else
                    'DirectCast (e.Row.Page.FindControl("DivModfy").Visible) = True
                    DirectCast(e.Row.FindControl("PanlModify"), Panel).Visible = True


                End If

            End If
            
        End If
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            Dim condition1 As String = ""
            Dim Condition2 As String = ""
            Dim condition3 As String = ""

            If ChkMember.Checked Then
                If txtMemberId.Text <> "" Then
                    formno = GetFormNo()
                    Condition = Condition & " and Temp.FormNo='" & formno & "'"
                End If
            End If

            If txtStartDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(varchar,Temp.RecTimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text <> "" Then
                Condition = Condition & " And Cast(Convert(varchar,Temp.RecTimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
            End If

            Dim qry1 As String = ""
            qry1 = "select  b.Idno,(b.MemFirstName+''+b.MemLastName) as [MemberName],b.City, d.StateName as State" & _
              " ,Replace(Convert(varchar,Temp.RecTimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(Temp.RecTimeStamp AS TIME),100) as [LoanDate],Narration as [LoanDescription],Sum(Credit) as [LoanAmount],sum(Debit) as [RecoverAmount],Balance as [DueAmount],Temp.UserName from ( " & _
              " select VoucherNo,DrTo as Formno,a.RecTimeStamp,0 as Credit ,Amount as Debit,Balance,Narration,b.UserName , 0 as RefNo from TrnVoucher as a Left Join M_UserMaster as b on  a.UserId=b.Userid and b.RowStatus='Y' where 1=1 and a.Actype='L' and a.DrTo<>0 " & condition3 & " " & _
               " Union all" & _
             " select VoucherNo,CrTo as Formno,a.RecTimeStamp,Amount as Credit ,0 as Debit,Balance,Narration,b.UserName ,a.RefNo from TrnVoucher as a Left Join M_UserMaster as b on  a.UserId=b.Userid and b.RowStatus='Y' where 1=1 and a.Actype='L' and a.Crto<>0" & Condition2 & " " & _
            ") as temp ,M_MemberMaster as b Left Join M_StateDivMaster as d on b.StateCode=d.StateCode and d.RowStatus='Y'  where Temp.Formno=b.Formno " & Condition & " Group By IdNo,MemFirstName,MemLastname,Temp.RecTimeStamp,Narration,Balance,VoucherNo,Temp.UserName,StateName,City,Temp.Formno,Temp.RefNo " & condition1 & "" & _
            " order by Idno,CONVERT(DateTime,Temp.RectimeStamp) Desc"

            dtTemp = New DataTable
            dtTemp = objDAL.GetData(qry1)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("AdvanceLoanReport.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblErr.Text = ""
        lblCount.Text = ""
        Dim Condition As String = ""
        Dim condition1 As String = ""

        Dim Condition2 As String = ""
        Dim condition3 As String = ""
        Dim formno As String = ""
        Dim qry1 As String = ""
        If ChkMember.Checked Then
            If txtMemberId.Text <> "" Then
                formno = GetFormNo()
                Condition = Condition & " and Temp.FormNo='" & formno & "'"
            End If
        End If

        If txtStartDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(varchar,Temp.RecTimestamp,106) as DateTime)>='" & txtStartDate.Text & "'"
        End If
        If txtEndDate.Text <> "" Then
            Condition = Condition & " And Cast(Convert(varchar,Temp.RecTimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"
        End If

        Dim str As String = ""
        Dim Dt As DataTable
        Dt = New DataTable
        'If Session("CompID") = 1066 Then
        '    qry1 = "select Temp.Formno, b.Idno,(b.MemFirstName+''+b.MemLastName) as [MemberName],b.City, d.StateName as State" & _
        '    " ,Replace(Convert(varchar,Temp.RecTimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(Temp.RecTimeStamp AS TIME),100) as [LoanDate],Narration as [LoanDescription],Sum(Credit) as [LoanAmount],sum(Debit) as [RecoverAmount],Balance as [DueAmount],Temp.UserName,RefNo,Case when RefNo=0 then 'False' else 'True' end as Status from ( " & _
        '    " select VoucherNo,DrTo as Formno,a.RecTimeStamp,0 as Credit ,Amount as Debit,Balance,Narration,b.UserName , 0 as RefNo from TrnVoucher as a Left Join M_UserMaster as b on  a.UserId=b.Userid and b.RowStatus='Y' where 1=1 and a.Actype='S' and a.DrTo<>0 " & condition3 & " " & _
        '     " Union all" & _
        '   " select VoucherNo,CrTo as Formno,a.RecTimeStamp,Amount as Credit ,0 as Debit,Balance,Narration,b.UserName ,a.RefNo from TrnVoucher as a Left Join M_UserMaster as b on  a.UserId=b.Userid and b.RowStatus='Y' where 1=1 and a.Actype='L' and a.Crto<>0" & Condition2 & " " & _
        '  ") as temp ,M_MemberMaster as b Left Join M_StateDivMaster as d on b.StateCode=d.StateCode and d.RowStatus='Y'  where Temp.Formno=b.Formno " & Condition & " Group By IdNo,MemFirstName,MemLastname,Temp.RecTimeStamp,Narration,Balance,VoucherNo,Temp.UserName,StateName,City,Temp.Formno,Temp.RefNo " & condition1 & " " & _
        '  " order by Idno,CONVERT(DateTime,Temp.RectimeStamp) Desc "
        'Else
        qry1 = "select Temp.Formno, b.Idno,(b.MemFirstName+''+b.MemLastName) as [MemberName],b.City, d.StateName as State" & _
        " ,Replace(Convert(varchar,Temp.RecTimeStamp,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(Temp.RecTimeStamp AS TIME),100) as [LoanDate],Narration as [LoanDescription],Sum(Credit) as [LoanAmount],sum(Debit) as [RecoverAmount],Balance as [DueAmount],Temp.UserName,RefNo,Case when RefNo='0' then 'False' else 'True' end as Status from ( " & _
        " select VoucherNo,DrTo as Formno,a.RecTimeStamp,0 as Credit ,Amount as Debit,Balance,Narration,b.UserName , '0' as RefNo from TrnVoucher as a Left Join M_UserMaster as b on  a.UserId=b.Userid and b.RowStatus='Y' where 1=1 and a.Actype='L' and a.DrTo<>0 " & condition3 & " " & _
         " Union all" & _
       " select VoucherNo,CrTo as Formno,a.RecTimeStamp,Amount as Credit ,0 as Debit,Balance,Narration,b.UserName ,a.RefNo from TrnVoucher as a Left Join M_UserMaster as b on  a.UserId=b.Userid and b.RowStatus='Y' where 1=1 and a.Actype='L' and a.Crto<>0" & Condition2 & " " & _
      ") as temp ,M_MemberMaster as b Left Join M_StateDivMaster as d on b.StateCode=d.StateCode and d.RowStatus='Y'  where Temp.Formno=b.Formno " & Condition & " Group By IdNo,MemFirstName,MemLastname,Temp.RecTimeStamp,Narration,Balance,VoucherNo,Temp.UserName,StateName,City,Temp.Formno,Temp.RefNo " & condition1 & " " & _
      " order by Idno,CONVERT(DateTime,Temp.RectimeStamp) Desc "

        'End If

        dtData = New DataTable
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dtData = objDAL.GetData(qry1)
        If dtData.Rows.Count > 0 Then
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("AccountData") = dtData
            GvData.Visible = True
            gvContainer.Visible = True
            btnExport.Enabled = True
            ' lblCount.Text = "Total : " & dtData.Rows.Count
        Else
            lblErr.Text = "No Record Found!!"
            gvContainer.Visible = False
            btnExport.Enabled = False
        End If

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
        dtData = Session("AccountData")
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
        dtData = Session("AccountData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub

    Protected Sub btnPrintAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        GvData.AllowPaging = False
        GvData.GridLines = GridLines.Both
        dtData = New DataTable
        dtData = Session("AccountData")
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
        dtData = Session("AccountData")
        GvData.DataSource = dtData
        GvData.DataBind()
    End Sub




End Class
