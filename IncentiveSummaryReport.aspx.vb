Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Partial Class App_UI_Application_Pages_IncentiveSummaryReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Report  / Weekly Incentive Report"
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
                BindSession()
                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        txtMemId.Text = Request.QueryString("key")
                        If txtMemId.Text <> "" Then
                            CheckBox1.Checked = True
                            IncentiveSummry()
                        End If


                        '  ChkRadioMember.SelectedValue = "N" : IncentiveDetail()
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub BindSession()
        Dim sql As String = "Select * From(Select 0 As SessID,'-- ALL --' As SessnName Union ALL select SessID,Cast(SessID as varchar) + ' [' + Replace(Convert(varchar,FrmDate,106),' ','-') + ' to ' + ISNULL(Replace(Convert(varchar,ToDate,106),' ','-'),'') + ']' As SessnName from M_SessnMaster Where ToDate Is Not Null) As Temp order by SessID"
        objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
    End Sub
    Private Sub IncentiveSummry()
       
        If CheckBox2.Checked = True Then
            If ddlSession.SelectedValue > 0 Then
                Condition = Condition & " And SessID=" & ddlSession.SelectedValue
            End If
        End If


        sql = "select Sessid,Cast(Fromdate as Varchar)+' To '+Cast(Todate as Varchar) as PayoutDate,Count(*) as TotalCount,0 as MatchedBv,Sum(ChqAmt) as NetAmount" & _
             " from V#PayoutDetail as a where  ChqAmt>0 Group By Sessid,Fromdate,ToDate"

           dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GData") = dtData
        GvData.Visible = True
        If dtData.Rows.Count > 0 Then
            Dim sms As String
            sms = dtData.Rows(0)("IsSentSms")
            If sms = "Sms Sent Successfully" Then
            End If
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If


    End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid

            If CheckBox1.Checked = True Then
                Condition = Condition & " And IDNo='" & txtMemId.Text & "'"
            End If
            If CheckBox2.Checked = True Then
                If ddlSession.SelectedValue > 0 Then
                    Condition = Condition & " And SessID=" & ddlSession.SelectedValue
                End If
            End If


            sql = "select PayoutDate,IdNo,MemName As MemberName,Mobl As MobileNo,LegXPaid as MatchedBv,BinaryIncome as [Sales Matching Income],SpillIncome as [Magic Spill],NetIncome As GrossIncome,TdsAmount,AdminCharge,CouponsAmt as [Repurchase Deduction],LoanRecovery,Deduction,chqAmt as NetIncome,PrevBal as PreviousBalance,ClsBal as ClosingBalance " & _
            " from V#PayoutDetail where 1=1 And (NetIncome>0 ) " & Condition & " order by Sessid Desc"
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("IncentiveDetail.xls", dg)

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
