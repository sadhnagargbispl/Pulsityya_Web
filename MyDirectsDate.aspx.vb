Imports System.Data.SqlClient
Imports System.Data
Imports ClosedXML.Excel
Imports System.IO

Partial Class App_UI_Application_Pages_MyDirectsDate
    Inherits System.Web.UI.Page
    Dim Ds As DataSet
    Dim dt As DataTable
    Dim conn As New SqlConnection
    Dim Comm As New SqlCommand
    Dim Adp As SqlDataAdapter
    Dim Obj As DAL
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Session("AStatus") = "OK" Then
                If Not Page.IsPostBack Then
                    FillLevel()
                    Fillkit()
                    DdlLevel.SelectedValue = "0"
                    ''LevelDetail(1)

                End If
            Else
                Response.Redirect("logout.aspx")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("IssuedPinValue")
        GvData.DataBind()
    End Sub


    Protected Sub FillLevel()
        ' Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Conn.Open()
        Dim Formno As String = ""
        If txtMember.Text = "" Then
            Formno = 0
        Else
            Formno = GetFormNo()
        End If
        Try
            Dim prms As SqlParameter() = New SqlParameter(1) {}
            prms(0) = New SqlParameter("@FormNo", Formno)
            prms(1) = New SqlParameter("@type", "N")

            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevel", prms)
            DdlLevel.DataSource = Ds.Tables(0)
            DdlLevel.DataTextField = "LevelName"
            DdlLevel.DataValueField = "MLevel"
            DdlLevel.DataBind()
            ' Conn.Close()
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Fillkit()
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetKitMaster")
            ddlKitName.DataSource = Ds.Tables(0)
            ddlKitName.DataValueField = "KitID"
            ddlKitName.DataTextField = "Kitname"
            ddlKitName.DataBind()
        Catch ex As Exception
        End Try
    End Sub



    Public Sub LevelDetail(ByVal pageIndex As Integer)
        Try
            Dim legno As String = ""
            Dim level As String = ""
            If rbtnsearch.SelectedValue = "L" Then
                legno = 0
                level = DdlLevel.SelectedValue
            Else
                legno = rbtnsearch.SelectedValue
                level = 1
            End If
            Dim Formno As String = ""
            If txtMember.Text = "" Then
                Formno = 0
            Else
                Formno = GetFormNo()
            End If

            Dim startDate As Date
            Dim endDate As Date
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
            End If

            'GrdDirects.DataSource = Nothing
            'GrdDirects.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(10) {}
            prms(0) = New SqlParameter("@MLevel", level)
            prms(1) = New SqlParameter("@Legno", legno)
            prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
            prms(3) = New SqlParameter("@FormNo", Formno)
            prms(4) = New SqlParameter("@PageIndex", pageIndex)
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))

            prms(6) = New SqlParameter("@FromDate", Convert.ToDateTime(startDate))
            prms(7) = New SqlParameter("@Todate", Convert.ToDateTime(endDate))
            prms(8) = New SqlParameter("@KitID", Convert.ToInt32(ddlKitName.SelectedValue))
            prms(9) = New SqlParameter("@IsExport", Convert.ToString("N"))

            prms(10) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetailDate", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue)
            GvData.DataBind()
            Session("IssuedPinValue") = Ds.Tables(0)
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            lbltotal.Text = recordCount
            ''Me.PopulatePager(recordCount, pageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try

    End Sub





    Private Sub FillData()
        Try
            Dim Formno As String = ""
            Formno = GetFormNo()

            Dim qry As String = "Select * from V#ReferalDownlineinfo where Formno=" & Formno & " "
            Dim Dt1 As New DataTable
            dt = Obj.GetData(qry)
            If dt.Rows.Count > 0 Then
                tdDirectleft.InnerText = dt.Rows(0)("RegisterLeft")
                tdDirectright.InnerText = dt.Rows(0)("RegisterRight")
                TotalDirect.InnerText = Val(dt.Rows(0)("RegisterLeft")) + Val(dt.Rows(0)("RegisterRight"))
                tddirectActive.InnerText = dt.Rows(0)("ConfirmLeft")
                tdindirectActive.InnerText = dt.Rows(0)("ConfirmRight")
                TotalActive.InnerText = Val(dt.Rows(0)("ConfirmLeft")) + Val(dt.Rows(0)("confirmRight"))
                Directunit.InnerText = Val(dt.Rows(0)("LeftBv"))
                indirectunit.InnerText = Val(dt.Rows(0)("RightBv"))
                totalunit.InnerText = Val(dt.Rows(0)("LeftBv")) + Val(dt.Rows(0)("rightBv"))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetFormNo() As String
        Try


            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim idNo As String
            Dim formno As String
            idNo = txtMember.Text
            idNo = idNo.Trim
            Dim qry As String = "Select FormNo,MemFirstName As Name from M_MemberMaster  where IdNo='" & idNo & "'"
            Dim dt As New DataTable
            dt = Obj.GetData(qry)
            If (dt.Rows.Count > 0) Then
                formno = dt.Rows(0)("FormNo")
                lblMemberNAme.Text = dt.Rows(0)("Name")
                lblMemberNAme.Visible = True
                lblErr.Text = ""
            Else
                lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
                lblErr.Visible = True
                txtMember.Text = ""
                lblMemberNAme.Text = ""
                lblMemberNAme.Visible = False
            End If
            Return formno
        Catch ex As Exception

        End Try
    End Function
    Protected Sub txtMember_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMember.TextChanged
        Try
            Dim Formno As String = ""
            Formno = GetFormNo()
            If Formno = 0 Then

            Else

                FillLevel()
                FillData()
            End If
        Catch ex As Exception

        End Try
    End Sub





    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        Try
            LevelDetail(1)
        Catch ex As Exception

        End Try



    End Sub

    Protected Sub rbtnsearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.SelectedIndexChanged
        Try


            If rbtnsearch.SelectedValue = "L" Then
                lbllevel.Visible = True

            Else
                lbllevel.Visible = False


            End If
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try


            Dim legno As String = ""
            Dim level As String = ""
            If rbtnsearch.SelectedValue = "L" Then
                legno = 0
                level = DdlLevel.SelectedValue
            Else
                legno = rbtnsearch.SelectedValue
                level = 1
            End If
            Dim Formno As String = ""
            If txtMember.Text = "" Then
                Formno = 0
            Else
                Formno = GetFormNo()
            End If

            Dim startDate As Date
            Dim endDate As Date
            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
            End If
            'GvData.DataSource = Nothing
            'GvData.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(10) {}
            prms(0) = New SqlParameter("@MLevel", level)
            prms(1) = New SqlParameter("@Legno", legno)
            prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
            prms(3) = New SqlParameter("@FormNo", Formno)
            prms(4) = New SqlParameter("@PageIndex", Convert.ToInt32(1))
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(ddlPageSize.SelectedValue))

            prms(6) = New SqlParameter("@FromDate", Convert.ToDateTime(startDate))
            prms(7) = New SqlParameter("@Todate", Convert.ToDateTime(endDate))
            prms(8) = New SqlParameter("@KitID", Convert.ToInt32(ddlKitName.SelectedValue))
            prms(9) = New SqlParameter("@IsExport", Convert.ToString("Y"))
            prms(10) = New SqlParameter("@RecordCount", ParameterDirection.Output)

            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetailDate", prms)
            
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
            Response.AddHeader("content-disposition", "attachment;filename=MyDirect.xlsx")
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


End Class
