Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Partial Class BusinessReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim Dt As New DataTable
    Dim objDAL As DAL
    'Dim objModuleFun As New ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim str As String = ""
    Dim Condition As String = ""
    Dim Condition2 As String = ""
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Report / Business Report"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                txtMemId.Text = ""
                GrdTotal.Visible = False

                If Session("AStatus") = "OK" Then
                    BindFromDate()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub BindFromDate()
        '    Dim sql As String = "select Sessid,Replace(Convert(Varchar,FrmDate,106),' ','-') as FromDate,Replace(Convert(Varchar,ToDate,106),' ','-') as Todate from D_SessnMaster order by Sessid Desc"
        '    objModuleFun.FillCombo(sql, DDlFromDate, "FromDate", "Sessid")
        '    objModuleFun.FillCombo(sql, DDltodate, "ToDate", "Sessid")
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "Sp_FillmonthSession")
            DDlFromDate.DataSource = Ds.Tables(0)
            DDlFromDate.DataValueField = "SessID"
            DDlFromDate.DataTextField = "Year"
            DDlFromDate.DataBind()
             Catch ex As Exception
        End Try
    End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        
        'sql = "select PayoutDate,IdNo,MemName,Mobl,BankName,AccountNo,IFSCode,Panno,BinaryIncome,NetIncome,TDSPer,TdsAmount,Deduction,Prevbal as Previous,chqAmt as ChequeAmount,ClsBal As Closing from V#DailyPayoutDetail where 1=1 And (NetIncome>0 Or PrevBal>0 Or ClsBal>0) " & Condition
        dtData = New DataTable

        Dim prms As SqlParameter() = New SqlParameter(1) {}
        prms(0) = New SqlParameter("@Sessid", Integer.Parse(DDlFromDate.SelectedValue))
        prms(1) = New SqlParameter("@Idno", Convert.ToString(txtMemId.Text.Trim))
        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "Sp_GroupBusinessAdmin", prms).Tables(0)

        ' dtData = objDAL.GetData(sql)
        GrdTotal.DataSource = dtData
        GrdTotal.DataBind()
        Session("GData") = dtData
        GrdTotal.Visible = True



        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub
    Protected Sub GrdTotal_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdTotal.PageIndexChanging
        GrdTotal.PageIndex = e.NewPageIndex
        GrdTotal.DataSource = Session("GData")
        GrdTotal.DataBind()
    End Sub
    
    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData1")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "BusinessReport")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=BusinessReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            GrdTotal.DataSource = Nothing
            GrdTotal.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(1) {}
            prms(0) = New SqlParameter("@Sessid", Integer.Parse(DDlFromDate.SelectedValue))
            prms(1) = New SqlParameter("@Idno", Convert.ToString(txtMemId.Text.Trim))
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "Sp_GroupBusinessAdmin", prms)
            Session("GData1") = Ds.Tables(0)
            ExportExcel()
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
