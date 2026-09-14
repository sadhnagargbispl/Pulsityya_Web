
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_OldPayoutdetail
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim Dt As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim str As String = ""
    Dim Condition As String = ""
    Dim Condition2 As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Report / Daily Incentive Report"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If Not Page.IsPostBack Then
            txtMemId.Text = ""
            GvData.Visible = False
            btnExport.Enabled = False
            If Session("AStatus") = "OK" Then
                BindFromDate()
                'BindSession()
            End If
        End If
    End Sub

    ''Public Sub BindSession()
    ''    'Dim sql As String = "Select * From(Select 0 As SessID,'-- ALL --' As SessnName Union ALL select SessID,Cast(SessID as varchar) + ' [' + Replace(Convert(varchar,FrmDate,106),' ','-') + ' to ' + ISNULL(Replace(Convert(varchar,ToDate,106),' ','-'),'') + ']' As SessnName from D_SessnMaster Where ToDate Is Not Null) As Temp order by SessID"
    ''    Dim sql As String = "Select * From(Select 0 As SessID,'-- ALL --' As SessnName Union ALL select SessID,' [' + Replace(Convert(varchar,FrmDate,106),' ','-') + ' to ' + ISNULL(Replace(Convert(varchar,ToDate,106),' ','-'),'') + ']' As SessnName from D_SessnMaster Where ToDate Is Not Null) As Temp order by SessID"

    ''    objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
    ''End Sub
    Public Sub BindFromDate()
        Dim sql As String = "select Distinct Closing_Date from vom_income_binary"
        objModuleFun.FillCombo(sql, DDlFromDate, "Closing_Date ", "Closing_Date")

    End Sub
    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click

        Dim condition As String = ""
        If CheckBox1.Checked = True Then
            Condition = Condition & " And IDNo='" & txtMemId.Text & "'"
        End If
        If CheckBox2.Checked = True Then
            condition = condition & "And Closing_date='" & DDlFromDate.SelectedValue & "' "
        End If

        'If DdlSearch.SelectedValue = "D" Then
        '    Condition = Condition & " Order By Sessid, ChqAmt Desc"
        'Else
        '    Condition = Condition & " Order By Sessid,ChqAmt Asc"
        'End If

        'sql = "select PayoutDate,IdNo,MemName,Mobl,BankName,AccountNo,IFSCode,Panno,BinaryIncome,NetIncome,TDSPer,TdsAmount,Deduction,Prevbal as Previous,chqAmt as ChequeAmount,ClsBal As Closing from V#DailyPayoutDetail where 1=1 And (NetIncome>0 Or PrevBal>0 Or ClsBal>0) " & Condition
        sql = "select * from vom_income_binary where 1=1 And (Amount>0) " & Condition & ""
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        GvData.Visible = True



        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    'Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
    '    Try
    '        Dim dtTemp As New DataTable
    '        Dim dg As New DataGrid

    '        Dim condition1 As String = ""
    '        If CheckBox1.Checked = True Then
    '            Condition = Condition & " And IDNo='" & txtMemId.Text & "'"
    '        End If
    '        If CheckBox2.Checked = True Then
    '            Condition = Condition & "And Sessid >='" & DDlFromDate.SelectedValue & "' And   Sessid <='" & DDltodate.SelectedValue & "'"
    '        End If

    '        If DdlSearch.SelectedValue = "D" Then
    '            Condition = Condition & " Order By Sessid, ChqAmt Desc"
    '        Else
    '            Condition = Condition & " Order By Sessid,ChqAmt Asc"
    '        End If

    '        'sql = "select PayoutDate,IdNo,MemName,Mobl,BankName,AccountNo,IFSCode,Panno,BinaryIncome,NetIncome,TDSPer,TdsAmount,Deduction,Prevbal as Previous,chqAmt as ChequeAmount,ClsBal As Closing from V#DailyPayoutDetail where 1=1 And (NetIncome>0 Or PrevBal>0 Or ClsBal>0) " & Condition
    '        sql = "select * from vom_income_binary where 1=1 And (NetIncome>0 Or PrevBal>0 Or ClsBal>0) " & Condition & ""

    '        dtTemp = New DataTable
    '        dtTemp = objDAL.GetData(sql)

    '        dg.DataSource = dtTemp
    '        dg.DataBind()

    '        ExportToExcel("DailyIncentiveDetail.xls", dg)

    '    Catch ex As Exception
    '        Response.Write(ex.Message & "Error In Exporting File")
    '    End Try
    'End Sub

    'Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
    '    Dim sw As New System.IO.StringWriter
    '    Dim htw As System.Web.UI.HtmlTextWriter
    '    Response.Clear()
    '    Response.Buffer = True
    '    Response.ContentType = "application/vnd.xls"
    '    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
    '    Response.Charset = ""
    '    dg.EnableViewState = False
    '    htw = New HtmlTextWriter(sw)
    '    dg.RenderControl(htw)
    '    Response.Write(sw.ToString())
    '    Response.End()
    'End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub



End Class

