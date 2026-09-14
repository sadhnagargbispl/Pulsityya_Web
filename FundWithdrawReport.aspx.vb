Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Partial Class App_UI_Application_Pages_FundWithdrawReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim scrname As String = ""
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Wallet / Wallet Authentication Report "
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If Not Page.IsPostBack Then
            txtMemId.Text = ""
            GvData.Visible = False
            If Session("AStatus") = "OK" Then
                'BindSession()
            End If
        End If
    End Sub
   
    Private Sub FillDetail()

        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        sql = " Sp_FunwithdrawalReport '" & txtMemId.Text & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "','" & RbtStatus.SelectedValue & "' "
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        If (dtData.Rows.Count > 0) Then
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            GvData.Visible = True
        Else
            GvData.DataSource = dtData
            GvData.DataBind()
            scrname = "<SCRIPT language='javascript'>alert('No record Found.!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('No record Found.!');", True)
        End If
       


    End Sub
    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        FillDetail()
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub



    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            Dim dg As New DataGrid
            Dim dtTemp As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            sql = " Sp_FunwithdrawalReport '" & txtMemId.Text & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "','" & RbtStatus.SelectedValue & "' "
            dtTemp = New DataTable

            dtTemp = objDAL.GetData(sql)
            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("WalletAuthentication.xls", dg)

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
End Class
