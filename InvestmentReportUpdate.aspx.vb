Imports ClosedXML.Excel
Imports DocumentFormat.OpenXml.Drawing.Diagrams
Imports DocumentFormat.OpenXml.Spreadsheet
Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls

Partial Class InvestmentReportUpdate
    Inherits System.Web.UI.Page
    Dim Ds As New DataSet()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Session("AStatus") IsNot Nothing Then
                    Fill_DDlPackage()
                    Fillkit()
                    Filltype()
                    Filldate()
                Else
                    Response.Redirect("Default.aspx")
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Filldate()
        Dim dtData As New DataTable()
        Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "

        dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Str).Tables(0)

        If dtData.Rows.Count > 0 Then
            txtStartDate.Text = dtData.Rows(0)("CurrentDate").ToString()
            txtEndDate.Text = dtData.Rows(0)("CurrentDate").ToString()
        End If

    End Sub

    Private Sub Fillkit()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_ddlPageSize")

            ddlPageSize.DataSource = Ds.Tables(0)
            ddlPageSize.DataValueField = "ddlPageSize"
            ddlPageSize.DataTextField = "ddlPageSize"
            ddlPageSize.DataBind()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Sub Filltype()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_ddltype")

            ddltype.DataSource = Ds.Tables(0)
            ddltype.DataValueField = "id"
            ddltype.DataTextField = "type"
            ddltype.DataBind()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Private Sub Fill_DDlPackage()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "Sp_Fill_DDlPackage")

            DDlPackage.DataSource = Ds.Tables(0)
            DDlPackage.DataValueField = "kitid"
            DDlPackage.DataTextField = "kitname"
            DDlPackage.DataBind()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub
    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnShow.Click
        Try
            BindData(1)

        Catch ex As Exception

        End Try
    End Sub
    Public Sub BindData(ByVal PageIndex As Integer)
        lblError.Text = ""

        Dim FromSessid As String = ""
        Dim ToSessid As String = ""
        Dim Idno As String = "0"
        FromSessid = If(txtStartDate.Text <> "", txtStartDate.Text, Session("CompDate").ToString())
        ToSessid = If(txtEndDate.Text <> "", txtEndDate.Text, DateTime.Now.ToString("dd-MMM-yyyy"))
        Idno = If(txtMemId.Text <> "", txtMemId.Text, "0")
        GvData1.DataSource = Nothing
        GvData1.DataBind()
        Dim prms(8) As SqlParameter
        prms(0) = New SqlParameter("@IDNo", Idno.ToLower())
        prms(1) = New SqlParameter("@FromSessid", FromSessid)
        prms(2) = New SqlParameter("@ToSessid", ToSessid)
        prms(3) = New SqlParameter("@PageIndex", PageIndex)
        prms(4) = New SqlParameter("@PageSize", 100000000)
        prms(5) = New SqlParameter("@IsExport", "N")
        prms(6) = New SqlParameter("@Type", ddltype.SelectedValue.ToString())
        prms(7) = New SqlParameter("@RecordCount", SqlDbType.Int)
        prms(7).Direction = ParameterDirection.Output
        prms(8) = New SqlParameter("@kitid", DDlPackage.SelectedValue.ToString())
        Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")),"sp_GetInvestmentDetailNewKitwisse1", prms)
        If Ds.Tables.Count > 0 AndAlso Ds.Tables(0).Rows.Count > 0 Then

            GvData1.DataSource = Ds.Tables(0)
            GvData1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue)
            GvData1.DataBind()

            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("Recordcount").ToString()

            Session("InvestmentReport") = Ds.Tables(0)

            ViewState("IdNo") = Idno
            ViewState("Sort_Order") = "ASC"

            lblCount.Text = "Total Record: " & recordCount.ToString()

            GvData1.Visible = True

        Else
            lblError.Text = "No Record Found!!"
            GvData1.Visible = False
        End If

 
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click

        Try
            Dim FromSessid As String = ""
            Dim ToSessid As String = ""
            Dim Idno As String = "0"
            FromSessid = If(txtStartDate.Text <> "", txtStartDate.Text, Session("CompDate").ToString())
            ToSessid = If(txtEndDate.Text <> "", txtEndDate.Text, DateTime.Now.ToString("dd-MMM-yyyy"))
            Idno = If(txtMemId.Text <> "", txtMemId.Text, "0")
            Dim prms(8) As SqlParameter

            prms(0) = New SqlParameter("@IDNo", Idno.ToLower())
            prms(1) = New SqlParameter("@FromSessid", FromSessid)
            prms(2) = New SqlParameter("@ToSessid", ToSessid)
            prms(3) = New SqlParameter("@PageIndex", 1)
        prms(4) = New SqlParameter("@PageSize",Convert.ToInt32(ddlPageSize.SelectedValue))
            prms(5) = New SqlParameter("@IsExport", "Y")
            prms(6) = New SqlParameter("@Type", ddltype.SelectedValue.ToString())

            prms(7) = New SqlParameter("@RecordCount", SqlDbType.Int)
            prms(7).Direction = ParameterDirection.Output

        prms(8) = New SqlParameter("@kitid", DDlPackage.SelectedValue.ToString())

        Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")),"sp_GetInvestmentDetailNewKitwisse1",prms)

            Session("InvestmentReportExcel") = Ds.Tables(0)

            ExportExcel()

        Catch ex As Exception

        End Try

    End Sub
    Private Sub ExportExcel()

        Try
        Dim dt As DataTable = CType(Session("InvestmentReportExcel"), DataTable)

            Using wb As New XLWorkbook()

                wb.Worksheets.Add(dt, "InvestmentReport")

                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

            Response.AddHeader("content-disposition","attachment;filename=InvestmentReport.xlsx")

                Using MyMemoryStream As New MemoryStream()

                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)

                    Response.Flush()
                    Response.End()

                End Using
            End Using

        Catch ex As Exception



        End Try

    End Sub
    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged

        Try
            BindData(1)

        Catch ex As Exception


        End Try

    End Sub
    Protected Sub GrdTotal1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GvData1.PageIndexChanging

        Try
            GvData1.PageIndex = e.NewPageIndex

            GvData1.DataSource = Session("InvestmentReport")
            GvData1.DataBind()

        Catch ex As Exception



        End Try

    End Sub
    Protected Sub DDlPackage_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDlPackage.SelectedIndexChanged

        Try
            BindData(1)

        Catch ex As Exception



        End Try

    End Sub
    Protected Sub ddltype_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddltype.SelectedIndexChanged

        Try
            BindData(1)

        Catch ex As Exception


        End Try

    End Sub
End Class