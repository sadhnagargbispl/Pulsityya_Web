Imports System.Data.SqlClient
Imports System.Data
Imports ClosedXML.Excel
Imports System.IO

Partial Class GroupzaraDirects
    Inherits System.Web.UI.Page

    Dim Ds As DataSet
    Dim dt As DataTable
    Dim conn As New SqlConnection
    Dim Comm As New SqlCommand
    Dim Adp As SqlDataAdapter
    ''Dim Obj As New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    Dim Obj As DAL
    Dim dtData As New DataTable
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Session("AStatus") = "OK" Then
                If Not Page.IsPostBack Then


                    FillLevel()
                    DdlLevel.SelectedValue = "0"
                    LevelDetail(1)
                    txtMemberId.Text = ""
                    If Session("CompID") = 1057 Or Session("CompID") = 1081 Then
                        FillData()

                    End If

                    If Session("CompID") = 1057 Or Session("CompID") = 1081 Then
                        'datewisepanel.Visible = True

                    Else
                        'datewisepanel.Visible = False

                    End If

                End If
            Else

                Response.Redirect("logout.aspx")
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub
    Protected Sub RptDirects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles RptDirects.PageIndexChanging
        RptDirects.PageIndex = e.NewPageIndex
        RptDirects.DataSource = Session("GData3")
        RptDirects.DataBind()
    End Sub
    Private Sub Filldate()
        Try
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = Obj.GetData(Str)
            If dtData.Rows.Count > 0 Then
                txtStartDate.Text = dtData.Rows(0)("CurrentDate")
                txtEndDate.Text = dtData.Rows(0)("CurrentDate")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub FillLevel()
        ' Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Conn.Open()
        Try
            Dim Formno As String = ""
            If txtMemberId.Text = "" Then
                formno = 0
            Else
                Formno = GetFormNo()
            End If
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
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
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Function GetFormNo() As String
        Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMemberId.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from M_MemberMaster  where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = Obj.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            errMsg.Text = "Member Id does not exist. Please check it once and then enter it again."
            errMsg.Visible = True
            txtMemberId.Text = ""
        End If
        Return formno
    End Function
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.LevelDetail(1)
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Public Sub LevelDetail(ByVal pageIndex As Integer)
        Try
            Dim legno As String = ""
            Dim level As String = ""
            Dim startDate As Date
            Dim endDate As Date
            lbltotal.Text = ""
            Dim Formno As String = ""
            If Session("CompID") = 1057 Or Session("CompID") = 1081 Then
                If txtMemberId.Text = "" Then
                    Formno = 0
                Else
                    Formno = GetFormNo()
                End If
                If rbtnsearch.SelectedValue = "L" Then
                    legno = 0
                    level = DdlLevel.SelectedValue
                Else
                    legno = rbtnsearch.SelectedValue
                    'level = 1
                    level = 0
                End If
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
              
                Dim prms As SqlParameter() = New SqlParameter(9) {}
                prms(0) = New SqlParameter("@MLevel", level)
                prms(1) = New SqlParameter("@Legno", legno)
                prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
                prms(3) = New SqlParameter("@FormNo", Formno)
                prms(4) = New SqlParameter("@PageIndex", pageIndex)
                prms(5) = New SqlParameter("@PageSize", Integer.Parse(150000000))
                prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
                prms(7) = New SqlParameter("@Startdate", startDate)
                prms(8) = New SqlParameter("@Enddate", endDate)
                prms(9) = New SqlParameter("@SearchType", DDlDate.SelectedValue)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetailZaracpanelAdmin", prms)


                RptDirects.DataSource = Ds.Tables(0)
                RptDirects.DataBind()
                Session("GData3") = Ds.Tables(0)
                If Ds.Tables(0).Rows.Count > 0 Then
                    Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
                    lbltotal.Text = recordCount
                    lbltotal.Visible = True
                    Label1.Visible = True
                    If recordCount > 1 Then
                        BtnExportA.Visible = True
                    Else
                        BtnExportA.Visible = False

                    End If
                End If
                
            End If


        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub


    Private Sub FillData()

        Try
            Dim Formno As String = ""
            If txtMemberId.Text = "" Then
                Formno = 0
            Else
                Formno = GetFormNo()
            End If

            Dim qry As String = ""
            If Session("CompID") = 1057 Then
                qry = "Select * from V#ReferalDownlineinfo where Formno=" & Formno & " "
            ElseIf Session("CompID") = 1081 Then
                qry = "Select * from V#ReferalDownlineinfo where Formno=" & Formno & " "
            Else

                qry = "Select * from V#ReferalDownlineinfonew where Formno=" & Formno & " "
            End If
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
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub


    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
            Me.LevelDetail(pageIndex)

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub


    Protected Sub DdlLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlLevel.SelectedIndexChanged

    End Sub

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        Try
            LevelDetail(1)

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub





    Protected Sub rbtnsearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.SelectedIndexChanged
        Try
            If rbtnsearch.SelectedValue = "L" Then
                lbllevel.Visible = True
                'lbltotal.Visible = False
            Else
                lbllevel.Visible = False
                'lbltotal.Visible = False

            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub ExportExcel()

        Dim dt As DataTable = Session("GData3")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Directreport")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=Directreport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub

    Protected Sub DDlDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlDate.SelectedIndexChanged
        If DDlDate.SelectedValue = "A" Then
            'lbltotal.Visible = False
        Else
            'lbltotal.Visible = False

        End If
    End Sub

    Protected Sub txtMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberId.TextChanged
        Dim Formno As String = ""
        Formno = GetFormNo()
        If Formno = 0 Then

        Else

            FillLevel()
            FillData()
            'LevelDetail(1)
            'GvData.Visible = False
            Label1.Visible = False
            lbltotal.Visible = False
            errMsg.Visible = False
        End If
    End Sub

    Protected Sub BtnExportA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportA.Click
        Try
            Dim Condition2 As String = ""
            Dim condition3 As String = ""
            Dim startDate As Date
            Dim endDate As Date

            Dim legno As String = ""
            Dim level As String = ""
            If rbtnsearch.SelectedValue = "L" Then
                legno = 0
                level = DdlLevel.SelectedValue
            Else
                legno = rbtnsearch.SelectedValue
                'level = 1
                level = DdlLevel.SelectedValue
            End If
            Dim Formno As String = ""
            If txtMemberId.Text = "" Then
                Formno = 0
            Else
                Formno = GetFormNo()
            End If
            If Session("CompID") = 1057 Or Session("CompID") = 1081 Then
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

                RptDirects.DataSource = Nothing
                RptDirects.DataBind()

                Dim prms As SqlParameter() = New SqlParameter(9) {}
                prms(0) = New SqlParameter("@MLevel", level)
                prms(1) = New SqlParameter("@Legno", legno)
                prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
                prms(3) = New SqlParameter("@FormNo", Formno)
                prms(4) = New SqlParameter("@PageIndex", 1)
                prms(5) = New SqlParameter("@PageSize", Integer.Parse(150000000))
                prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
                prms(7) = New SqlParameter("@Startdate", startDate)
                prms(8) = New SqlParameter("@Enddate", endDate)
                prms(9) = New SqlParameter("@SearchType", DDlDate.SelectedValue)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetailZaracpanelAdmin", prms)

                Session("GData3") = Ds.Tables(0)

                ExportExcel()
            End If

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
End Class
