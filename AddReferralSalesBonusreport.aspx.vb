Imports System.Data
Imports System.IO

Partial Class AddReferralSalesBonusreport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Master / Gallery Master"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindData()
        'btnShowRecord.Visible = False
        'lblView.Visible = False
        'txtSearch.Text = ""
        'ddlSearchFields.SelectedIndex = 0
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'txtSearch.Text = ""
            'btnShowRecord.Visible = False
            ' lblView.Visible = False
            If Session("AStatus") = "OK" Then
                'BindData()
            End If
        End If
    End Sub

    Protected Sub BindData()
        Dim LblFileType As New Label
        Dim LblVideo As New Label
        Dim i As Integer = 0
        Dim ImgImage As New Image
        Dim formno As String = ""

        Dim condition1 As String = ""
        Dim condition2 As String = ""
        Dim condition As String = ""
        If ddlSearch.SelectedValue = "0" Then
            condition = ""
        ElseIf ddlSearch.SelectedValue = "IDNo" Then
            condition = " AND b.IDNO = '" & txtSrchText.Text & "' "
        ElseIf ddlSearch.SelectedValue = "CustomerName" Then
            condition = " AND a.cname like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "CustomerMobileNo" Then
            condition = " AND a.mobile like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "CallerName" Then
            condition = " AND a.callername like '%" & txtSrchText.Text & "%'"
        End If
        If ddllist.SelectedValue <> "All" Then
            condition = condition & " And a.Status='" & ddllist.SelectedValue & "'"
        End If
        If txtFromDate.Text <> "" Then
            condition = " And Cast(Convert(varchar,a.rectimestamp,106) as DateTime)>='" & txtFromDate.Text & "'"
        End If
        If TxtToDate.Text <> "" Then
            condition = " And Cast(Convert(varchar,a.rectimestamp,106) as DateTime)<='" & TxtToDate.Text & "'"
        End If

        Dim sql As String = " select a.m_id,b.memfirstname as membername,b.idno as idno,  " & _
             " replace(convert(varchar,a.rectimestamp,106),' ','-') as Date1," & _
      " c_name as cname, c_mobileno as mobile ,healthissue as hlissue," & _
    " Case when Status='A' then 'Close' else 'Pending' end as Status,callername,remark " & _
    " from M_ReferralSalesBonus as a ,m_membermaster as b Where  a.formno = b.formno   " & condition & ""
        dtData = objDAL.GetData(sql)
        'lbl.Text = dtData.Rows(0)("Status")
        GvData.DataSource = dtData
        GvData.DataBind()
        If dtData.Rows.Count > 0 Then
            lbl.Text = dtData.Rows(0)("Status")
            If lbl.Text = "Close" Then
                For Each rowItem As GridViewRow In GvData.Rows
                    rowItem.Cells(11).Enabled = False
                Next
            End If
        End If
        Session("GData") = dtData

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim stts As String = DirectCast(e.Row.FindControl("lblStatus"), Label).Text

            If String.Equals(stts.ToLower(), "deactive") = True Then
                'e.Row.BackColor = Drawing.Color.Red
                e.Row.Style("background-image") = "images/redback2.jpg"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "red"
                Next
            End If
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
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            '     Dim condition As String = ""
            Dim condition1 As String = ""
            Dim condition2 As String = ""
            
            If ddlSearch.SelectedValue = "0" Then
                condition = ""
            ElseIf ddlSearch.SelectedValue = "IDNo" Then
                condition = " AND b.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch.SelectedValue = "CustomerName" Then
                condition = " AND a.cname like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch.SelectedValue = "CustomerMobileNo" Then
                condition = " AND a.mobile like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch.SelectedValue = "CallerName" Then
                condition = " AND a.callername like '%" & txtSrchText.Text & "%'"
            End If
            If ddllist.SelectedValue <> "All" Then
                condition = condition & " And a.Status='" & ddllist.SelectedValue & "'"
            End If
            If txtFromDate.Text <> "" Then
                condition = " And Cast(Convert(varchar,a.rectimestamp,106) as DateTime)>='" & txtFromDate.Text & "'"
            End If
            If TxtToDate.Text <> "" Then
                condition = " And Cast(Convert(varchar,a.rectimestamp,106) as DateTime)<='" & TxtToDate.Text & "'"
            End If

            Dim sql As String = " select a.m_id as Sno,b.idno as [Member ID],b.memfirstname as [Member Name],  " & _
                 " c_name as [Customer Name],c_mobileno as [Customer Mobile No.] ,healthissue as [Health Issue]," & _
          "  replace(convert(varchar,a.rectimestamp,106),' ','-') as Date,callername as [Caller Name]," & _
        " remark as Remark,Case when Status='A' then 'Close' else 'Pending' end as Status" & _
        " from M_ReferralSalesBonus as a ,m_membermaster as b Where  a.formno = b.formno   " & Condition & ""
           dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()
            ExportToExcel("ReferralSalesReport.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")

        End Try
    End Sub
End Class
