Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class DownlineRankReport
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then
                FillCityPinDetail()


            End If


        Catch ex As Exception

        End Try
    End Sub

    'Private Sub FillCityPinDetail()
    '    Try

    '        Dim sql As String = String.Empty
    '        sql = " Select SessID,     "
    '        sql &= " (Convert(Varchar,SessID) +'-- ' + Replace(Convert(Varchar,FrmDate,106),' ','-') +' - '+ Replace(Convert(Varchar,ToDate,106),' ','-')) as Session"
    '        sql &= " From M_SessnMaster Where todate is not null"
    '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        dt = objDAL.GetData(sql)
    '        ddlstate.DataSource = dt
    '        ddlstate.DataTextField = "Session"
    '        ddlstate.DataValueField = "SessID"
    '        ddlstate.DataBind()
    '        ddlstate.Items.Insert(0, "--Select Session--")
    '    Catch ex As Exception

    '    End Try
    'End Sub


    Private Sub FillCityPinDetail()
        Try

            Dim sql As String = String.Empty

            sql = " Exec Sp_FillRankDrop"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            ddlstate.DataSource = dt
            ddlstate.DataTextField = "Rank"
            ddlstate.DataValueField = "RankId"
            ddlstate.DataBind()
        Catch ex As Exception

        End Try
    End Sub


    Private Function GetFormNo() As String
        Dim scrname As String = ""
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim formno As String
        Dim qry As String = "Exec Sp_GetFormno '" & txtMemId.Text & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            scrname = "<SCRIPT language='javascript'>alert('invaild Idno.!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
            txtMemId.Text = ""
        End If
        Return formno
    End Function


    Private Sub FillReport()
        Try
            Dim scrname As String = ""
            If (txtMemId.Text = "") Then
                scrname = "<SCRIPT language='javascript'>alert('Please Enter Idno.!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                txtMemId.Text = ""
            End If
            Dim formno As String = "0"
            formno = GetFormNo()
            Dim sql As String = String.Empty


            sql = " Exec Sp_GetDownlineRankReport " & formno & "," & ddlstate.SelectedValue & ",'',''"
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            If (dt.Rows.Count > 0) Then
                GvData.DataSource = dt
                GvData.DataBind()
                Session("GData") = dt
            Else
                scrname = "<SCRIPT language='javascript'>alert('Please Enter Idno.!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                txtMemId.Text = ""
            End If



        Catch ex As Exception

        End Try
    End Sub









    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillReport()

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        FillReport()
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            FillReport()
            ExportExcel()

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")

        End Try
    End Sub

    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Incentive")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=DownlineRankReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


End Class
