Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Imports ClosedXML.Excel

Imports System.Configuration

Partial Class UnitRateReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                BindSession()


                '                IncentiveDetail()
                '  ChkRadioMember.SelectedValue = "N" : IncentiveDetail()

            End If
        End If

    End Sub

    Public Sub BindSession()
        Dim sql As String = "Exec Sp_UnitSession 'FillSesion',0"
        objModuleFun.FillCombo(sql, ddlSession, "SessnName", "SessID")
    End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click



        'sql = "Select b.KitName,b.KitAmount,b.BFund as [Binary Fund],Count(*) as TotalSale, b.PV*Count(*) as [TotalBV], b.KitAmount*Count(*) as [FundCollection],b.BFund*Count(*) as [Binary Fund Collection],b.CFund*Count(*) as [C FundCollection] FROM RepurchIncome a, M_KitMAster b" & _
        '"  WHERE a.KitID = b.KitID And a.SessID=" & ddlSession.SelectedValue & " GROUP BY b.KitName,b.KitAmount,b.BFund,b.PV,b.CFund"
        sql = "Exec Sp_UnitSession 'FillData', " & ddlSession.SelectedValue & ""
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvActivation.DataSource = dtData
        GvActivation.DataBind()
        Session("ActivationData") = dtData
        GvActivation.Visible = True
        If dtData.Rows.Count > 0 Then

            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If


    End Sub

    Protected Sub GvActivation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvActivation.PageIndexChanging
        GvActivation.PageIndex = e.NewPageIndex
        GvActivation.DataSource = Session("ActivationData")
        GvActivation.DataBind()
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim dtTemp As New DataTable
        Dim dg As New DataGrid
        Try
            Dim strQuery As String = "Exec Sp_UnitSession 'FillData', " & ddlSession.SelectedValue & ""
            'Dim cmd As New SqlCommand(strQuery)
            dtTemp = New DataTable
            dtTemp = objDAL.GetData(strQuery)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("UnitRateReport.xls", dg)

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

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub

    Protected Sub GvActivation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvActivation.SelectedIndexChanged

    End Sub




End Class
