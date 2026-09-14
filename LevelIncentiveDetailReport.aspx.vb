Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Configuration
Imports ClosedXML.Excel

Partial Class App_UI_Application_Pages_LevelIncentiveDetailReport
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Public formNo As String
    Dim sql As String = ""
    Dim Condition As String = ""
    Dim Ds As DataSet
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Report  / Weekly Incentive Report"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Not Page.IsPostBack Then
                txtMemId.Text = ""
                GvData.Visible = False

                If Session("AStatus") = "OK" Then
                    BindSession()
                    If Request.QueryString.HasKeys Then
                        If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                            txtMemId.Text = Request.QueryString("key")
                            If txtMemId.Text <> "" Then
                                CheckBox1.Checked = True
                                'IncentiveDetail()
                            End If


                            '  ChkRadioMember.SelectedValue = "N" : IncentiveDetail()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindSession()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelSession")
            ddlSession.DataSource = Ds.Tables(0)
            ddlSession.DataValueField = "SessID"
            ddlSession.DataTextField = "SessnName"
            ddlSession.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        'Try

        If CheckBox2.Checked = True Then
            If ddlSession.SelectedValue <> 5000000 Then
                Condition = Condition & " And SessID=" & ddlSession.SelectedValue
            End If
        End If
        If CheckBox1.Checked = True Then
            Condition = " And IDNo='" & txtMemId.Text & "'"
            IncentiveDetail(0, 1)

        Else
            IncentiveDetail(ddlSession.SelectedValue, 1)
        End If
        'Catch ex As Exception

        'End Try
    End Sub
    Protected Sub ViewDetail(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim GVRw As GridViewRow
            GVRw = CType(sender.Parent.Parent, GridViewRow)

            LblSessionNo.Text = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
            IncentiveDetail(Val(LblSessionNo.Text), 1)
        Catch ex As Exception

        End Try

    End Sub
    
    Private Sub IncentiveDetail(ByVal Sessid As Integer, ByVal PageIndex As Integer)
        Try




            Dim Idno As String = "0"

            If CheckBox1.Checked Then
                If txtMemId.Text <> "" Then
                    Idno = txtMemId.Text
                Else
                    Idno = "0"
                End If
            Else
                Idno = "0"
            End If
            If Sessid = 0 Then
                If CheckBox2.Checked Then
                    If ddlSession.SelectedValue <> "5000000" Then
                        Sessid = ddlSession.SelectedValue
                    Else
                        Sessid = 0
                    End If
                End If
            End If
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(5) {}
            prms(0) = New SqlParameter("@IdNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@Sessid", Convert.ToInt32(Sessid))
            prms(2) = New SqlParameter("@PageIndex", PageIndex)
            prms(3) = New SqlParameter("@PageSize", 100000000)
            prms(4) = New SqlParameter("@IsExport", "N")
            prms(5) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetIncentiveDetailReport", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()

            Dim recordCount As Integer = Ds.Tables(0).Rows.Count
            Session("GData1") = Ds.Tables(0)
            ViewState("PayoutDate") = "SNo"
            ViewState("Sort_Order") = "ASC"


            If Ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 1 To GvData.Columns.Count - 1
                    Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                    Dim img As New Image()
                    img.ImageUrl = "~/Images/Uparrow.png"
                    tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                    tableCell.Controls.Add(img)
                Next
                GvData.Focus()


            Else


            End If
            GvData.Visible = True
            ' Me.PopulatePager(recordCount, PageIndex)

            Session("IssuedPinValue1") = Ds.Tables(0)
            'ExportExcel()
        Catch ex As Exception
           
        End Try
    End Sub
    

    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim sessid As String = "0"
            If ddlSession.SelectedValue = "5000000" Then
                If Val(LblSessionNo.Text) <> 0 Then
                    sessid = Val(LblSessionNo.Text)
                Else
                    sessid = 0

                End If
            Else
                sessid = ddlSession.SelectedValue
            End If

            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.IncentiveDetail(sessid, pageIndex)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim sessid As String = "0"
            If ddlSession.SelectedValue = "5000000" Then
                If Val(LblSessionNo.Text) <> 0 Then
                    sessid = Val(LblSessionNo.Text)
                Else
                    sessid = 0

                End If
            Else
                sessid = ddlSession.SelectedValue
            End If


            Me.IncentiveDetail(sessid, 1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData1")
        GvData.DataBind()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try


            Dim Idno As String = "0"
            Dim sessid As String = "0"
            If CheckBox1.Checked Then
                If txtMemId.Text <> "" Then
                    Idno = txtMemId.Text
                Else
                    Idno = "0"
                End If
            Else
                Idno = "0"
            End If
            If ddlSession.SelectedValue = "5000000" Then
                If Val(LblSessionNo.Text) <> 0 Then
                    sessid = Val(LblSessionNo.Text)
                Else
                    sessid = 0

                End If
            Else
                sessid = ddlSession.SelectedValue
            End If
            GvData.DataSource = Nothing
            GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(5) {}
            prms(0) = New SqlParameter("@IdNo", Convert.ToString(Idno).ToLower())
            prms(1) = New SqlParameter("@Sessid", Convert.ToInt32(sessid))
            prms(2) = New SqlParameter("@PageIndex", 1)
            prms(3) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))
            prms(4) = New SqlParameter("@IsExport", "Y")
            prms(5) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            If Session("CompID") = 1068 Or Session("CompID") = 1093 Then
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetIncentiveDetailReport", prms)
            Else
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelPayoutDetail", prms)
            End If

            ' GvData.DataSource = Ds.Tables(0)
            ' GvData.DataBind()

            Session("GData") = Ds.Tables(0)
            ExportExcel()
            
        Catch ex As Exception

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
            Response.AddHeader("content-disposition", "attachment;filename=IncentiveDetail.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

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
    

    Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    End Sub

    
End Class
