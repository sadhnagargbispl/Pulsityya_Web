Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports ClosedXML.Excel
Partial Class App_UI_Application_Pages_MemberProfile
    Inherits System.Web.UI.Page
    Dim Ds As DataSet
    Dim dt As DataTable
    Dim conn As New SqlConnection
    Dim Comm As New SqlCommand
    Dim Adp As SqlDataAdapter
    Dim Obj As DAL
    Dim dtData As New DataTable
    Dim objDAL As DAL
    ''Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    Dim objGen As clsGeneral = New clsGeneral
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Session("AStatus") = "OK" Then
                If Not Page.IsPostBack Then
                    FillLevel()
                    Filldate()
                    BtnExportA.Visible = True
                    DdlLevel.SelectedValue = "0"
                    LevelDetail(1)
                    If Session("CompID") = 1057 Then
                        datewisepanel.Visible = True
                    Else
                        datewisepanel.Visible = False

                    End If

                End If
            Else
                Response.Redirect("logout.aspx")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Filldate()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = objDAL.GetData(Str)
            If dtData.Rows.Count > 0 Then
                txtStartDate.Text = dtData.Rows(0)("CurrentDate")
                txtEndDate.Text = dtData.Rows(0)("CurrentDate")
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
            formno = 0
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
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Me.LevelDetail(1)
    End Sub
    Public Sub LevelDetail(ByVal pageIndex As Integer)
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
            If txtMember.Text = "" Then
                Formno = 0
            Else
                Formno = GetFormNo()
            End If
            If Session("CompID") = 1057 Then
                BtnExportA.Visible = True
                'If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
                '    If DDlDate.SelectedValue = "J" Then
                '        Condition2 = "and Cast(Doj as date)>=Cast('" & txtStartDate.Text & "' as Date) and cast(Doj as Date)<=Cast('" & txtEndDate.Text & "' as date) "

                '    Else
                '        Condition2 = "and Cast(Upgradedate as date)>=Cast('" & txtStartDate.Text & "' as Date) and Cast(UpgradeDate as Date)<=Cast('" & txtEndDate.Text & "' as date) And ActiveStatus='Y'"
                '    End If
                '    'Else
                '    '    txtStartDate.Text = "" And txtEndDate.Text = ""
                'End If
                ''If DDlDate.SelectedValue = "A" Then
                ''    Condition2 = Condition2 & " And Activestatus='Y'"
                ''End If

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
                'If DDlSearchby.SelectedValue = "" Then
                '    DDlSearchby.SelectedValue = "N"
                'Else
                '    DDlSearchby.SelectedValue = "Y"
                'End If

                'GrdDirects.DataSource = Nothing
                'GrdDirects.DataBind()
                'below commit 02 Jan 2023
                'Dim prms As SqlParameter() = New SqlParameter(6) {}
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
                'Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetailNew", prms)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetailZara", prms)
                GvData.DataSource = Ds.Tables(0)
                GvData.DataBind()
                'BtnExportA.Visible = True
                Session("IssuedPinValue") = Ds.Tables(0)
                Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
                lbltotal.Text = recordCount
                Label1.Visible = True
                lbltotal.Visible = True
                If recordCount > 1 Then
                    BtnExportA.Visible = True
                    BtnExportA.Enabled = True
                Else
                    BtnExportA.Visible = True
                    ' BtnExportA.Enabled = False
                End If

                'ExportExcel()
                'BtnExportA.Visible = True
                'GvData.Visible = True
                GvData.Visible = True
                'BtnExportA.Visible = True
            ElseIf Session("CompId") = "1081" Then
                Dim prms As SqlParameter() = New SqlParameter(6) {}
                prms(0) = New SqlParameter("@MLevel", level)
                prms(1) = New SqlParameter("@Legno", legno)
                prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
                prms(3) = New SqlParameter("@FormNo", Formno)
                prms(4) = New SqlParameter("@PageIndex", pageIndex)
                prms(5) = New SqlParameter("@PageSize", Integer.Parse(150000000))
                prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetail", prms)
                GvData.DataSource = Ds.Tables(0)
                GvData.DataBind()
                Session("IssuedPinValue") = Ds.Tables(0)
                Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
                lbltotal.Text = recordCount
                If recordCount > 1 Then
                    BtnExportA.Visible = True
                    BtnExportA.Enabled = True
                    Label1.Visible = True
                    lbltotal.Visible = True
                    GvData.Visible = True
                Else
                    BtnExportA.Visible = True
                    ' BtnExportA.Enabled = False
                End If
                
            Else
                Dim prms As SqlParameter() = New SqlParameter(6) {}
                prms(0) = New SqlParameter("@MLevel", level)
                prms(1) = New SqlParameter("@Legno", legno)
                prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
                prms(3) = New SqlParameter("@FormNo", Formno)
                prms(4) = New SqlParameter("@PageIndex", pageIndex)
                prms(5) = New SqlParameter("@PageSize", Integer.Parse(150000000))
                prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetail", prms)
                GvData.DataSource = Ds.Tables(0)
                GvData.DataBind()
                Session("IssuedPinValue") = Ds.Tables(0)
                Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
                lbltotal.Text = recordCount
                Label1.Visible = True
                lbltotal.Visible = True
                BtnExportA.Visible = False
                GvData.Visible = True

            End If
            'GvData.Visible = True
            'BtnExportA.Visible = True
            ' Me.PopulatePager(recordCount, pageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try

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
            If txtMember.Text = "" Then
                Formno = 0
            Else
                Formno = GetFormNo()
            End If
            If Session("CompID") = 1057 Then
                

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
                
                GvData.DataSource = Nothing
                GvData.DataBind()
                
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
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetailZara", prms)
                
                Session("GData3") = Ds.Tables(0)
               
                ExportExcel()
            ElseIf Session("CompID") = 1081 Then
                Dim prms As SqlParameter() = New SqlParameter(6) {}
                prms(0) = New SqlParameter("@MLevel", level)
                prms(1) = New SqlParameter("@Legno", legno)
                prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
                prms(3) = New SqlParameter("@FormNo", Formno)
                prms(4) = New SqlParameter("@PageIndex", 1)
                prms(5) = New SqlParameter("@PageSize", Integer.Parse(150000000))
                prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetail", prms)
                GvData.DataSource = Ds.Tables(0)
                GvData.DataBind()
                Session("GData3") = Ds.Tables(0)
                ExportExcel()
            Else
                Dim prms As SqlParameter() = New SqlParameter(6) {}
                prms(0) = New SqlParameter("@MLevel", level)
                prms(1) = New SqlParameter("@Legno", legno)
                prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
                prms(3) = New SqlParameter("@FormNo", Formno)
                prms(4) = New SqlParameter("@PageIndex", 1)
                prms(5) = New SqlParameter("@PageSize", Integer.Parse(150000000))
                prms(6) = New SqlParameter("@RecordCount", ParameterDirection.Output)
                Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetail", prms)
                GvData.DataSource = Ds.Tables(0)
                GvData.DataBind()
                Session("GData3") = Ds.Tables(0)
               
            End If
            

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
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
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
    Private Sub FillData()
        Try




            ' Dim Str As String = ""
            '      Dim Str As String = " Select Sum(LeftDirect) as LeftDirect,Sum(RightDirect) as RightDirect," & _
            '      " Sum(TotalDirect) as TotalDirect from " & _
            '    " (Select Count(FormnoDwn) as leftdirect,0 as RightDirect,0 as TotalDirect from " & _
            '    " M_MembeRMaster as a,R_Memtreerelation as b where b.formnodwn=a.Formno and b.MLevel=1 " & _
            '    " and a.legno=1 and b.Formno=" & Session("FormNo") & " " & _
            '      " Union All" & _
            ' " Select 0 as LeftDirect,Count(FormnoDwn) as RightDirect,0 as TotalDirect from " & _
            ' " M_MembeRMaster as a,R_Memtreerelation as b where b.formnodwn=a.Formno and b.MLevel=1 " & _
            ' " and a.legno=2 and b.Formno=" & Session("FormNo") & " " & _
            '      " union All" & _
            '" Select 0 as leftdirect,0 as RightDirect,Count(FormnoDwn) as TotalDirect from " & _
            '" M_MembeRMaster as a,R_Memtreerelation as b where b.formnodwn=a.Formno " & _
            ' " and b.MLevel=1  and b.Formno=" & Session("FormNo") & ")as Temp "

            '      Dim Dt As New DataTable
            '      Dt = Obj.GetData(Str)
            '      If Dt.Rows.Count > 0 Then


            '      End If
           
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

                tdDirectleft1.InnerText = dt.Rows(0)("RegisterLeft1")
                tdDirectright1.InnerText = dt.Rows(0)("RegisterRight1")
                TotalDirect1.InnerText = Val(dt.Rows(0)("RegisterLeft1")) + Val(dt.Rows(0)("RegisterRight1"))
                tddirectActive1.InnerText = dt.Rows(0)("ConfirmLeft1")
                tdindirectActive1.InnerText = dt.Rows(0)("ConfirmRight1")
                TotalActive1.InnerText = Val(dt.Rows(0)("ConfirmLeft1")) + Val(dt.Rows(0)("confirmRight1"))
                'Directunit.InnerText = Val(dt.Rows(0)("LeftBv"))
                'indirectunit.InnerText = Val(dt.Rows(0)("RightBv"))
                'totalunit.InnerText = Val(dt.Rows(0)("LeftBv")) + Val(dt.Rows(0)("rightBv"))
            Else
                tdDirectleft.InnerText = 0
                tdDirectright.InnerText = 0
                TotalDirect.InnerText = 0
                tddirectActive.InnerText = 0
                tdindirectActive.InnerText = 0
                TotalActive.InnerText = 0

                tdDirectleft1.InnerText = 0
                tdDirectright1.InnerText = 0
                TotalDirect1.InnerText = 0
                tddirectActive1.InnerText = 0
                tdindirectActive1.InnerText = 0
                TotalActive1.InnerText = 0
                'Directunit.InnerText = 0
                'indirectunit.InnerText = 0
                'totalunit.InnerText = 0
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetFormNo() As String
        Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMember.Text
        idNo = idNo.Trim
        Dim qry As String = "Select FormNo from M_MemberMaster  where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = Obj.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblErr.Visible = True
            txtMember.Text = ""
        End If
        Return formno
    End Function
    Protected Sub txtMember_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMember.TextChanged

        Dim Formno As String = ""
        Formno = GetFormNo()
        If Formno = 0 Then

        Else

            FillLevel()
            FillData()
            GvData.Visible = False
            Label1.Visible = False
            lbltotal.Visible = False
            lblErr.Visible = False
        End If
    End Sub
    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
        Me.LevelDetail(pageIndex)
    End Sub
    
    Protected Sub DdlLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlLevel.SelectedIndexChanged

    End Sub

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click

        LevelDetail(1)


    End Sub

    Protected Sub rbtnsearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.SelectedIndexChanged
        If rbtnsearch.SelectedValue = "L" Then
            lbllevel.Visible = True

        Else
            lbllevel.Visible = False


        End If
    End Sub
End Class
