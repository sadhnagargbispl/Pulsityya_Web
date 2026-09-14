Imports System.Data.SqlClient
Imports System.Data
Partial Class GroupRefDirects
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
                    If Session("CompId") = "1041" Then
                        FillPinLevel()
                    End If
                    FillLevel()
                    DdlLevel.SelectedValue = "0"
                    LevelDetail(1)

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
    Protected Sub FillPinLevel()

        Try
            Dim dt As DataTable = New DataTable()
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = "Sp_FillRank"
            dt = Obj.GetData(str)
            ddlpinLevel.DataSource = dt
            ddlpinLevel.DataTextField = "Rank"
            ddlpinLevel.DataValueField = "RankID"
            ddlpinLevel.DataBind()

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
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
    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Me.LevelDetail(1)
    End Sub
    Public Sub LevelDetail(ByVal pageIndex As Integer)
        Try
            Dim pinLevel As String = ""

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
            If (Session("CompID") = "1041") Then
                If (ddlpinLevel.SelectedValue > 0) Then
                    pinLevel = ddlpinLevel.SelectedItem.Text
                End If
            End If
            'GrdDirects.DataSource = Nothing
            'GrdDirects.DataBind()
            Dim prms As SqlParameter() = New SqlParameter(7) {}
            prms(0) = New SqlParameter("@MLevel", level)
            prms(1) = New SqlParameter("@Legno", legno)
            prms(2) = New SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue)
            prms(3) = New SqlParameter("@FormNo", Formno)
            prms(4) = New SqlParameter("@PageIndex", pageIndex)
            prms(5) = New SqlParameter("@PageSize", Integer.Parse(150000000))
            prms(6) = New SqlParameter("@pinlevel", pinLevel)
            prms(7) = New SqlParameter("@RecordCount", ParameterDirection.Output)
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevelDetail", prms)
            GvData.DataSource = Ds.Tables(0)
            GvData.DataBind()
            Session("IssuedPinValue") = Ds.Tables(0)
            Dim recordCount As Integer = Ds.Tables(1).Rows(0)("RecordCount")
            lbltotal.Text = recordCount
            ' Me.PopulatePager(recordCount, pageIndex)
        Catch ex As Exception
            Response.Write(ex.Message & "SideB")
        End Try

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
                Directunit.InnerText = Val(dt.Rows(0)("LeftBv"))
                indirectunit.InnerText = Val(dt.Rows(0)("RightBv"))
                totalunit.InnerText = Val(dt.Rows(0)("LeftBv")) + Val(dt.Rows(0)("rightBv"))
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
        End If
    End Sub
    Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
        Me.LevelDetail(pageIndex)
    End Sub
    'Private Sub PopulatePager(ByVal recordCount As Integer, ByVal currentPage As Integer)
    '    Dim dblPageCount As Double = CType((CType(recordCount, Decimal) / Decimal.Parse(ddlPageSize.SelectedValue)), Double)
    '    Dim pageCount As Integer = CType(Math.Ceiling(dblPageCount), Integer)
    '    Dim pages As New List(Of ListItem)
    '    If (pageCount > 0) Then
    '        pages.Add(New ListItem("First", "1", (currentPage > 1)))
    '        Dim i As Integer = 1
    '        Do While (i <= pageCount)
    '            pages.Add(New ListItem(i.ToString, i.ToString, (i <> currentPage)))
    '            i = (i + 1)
    '        Loop
    '        pages.Add(New ListItem("Last", pageCount.ToString, (currentPage < pageCount)))
    '    End If
    '    rptPager.DataSource = pages
    '    rptPager.DataBind()
    'End Sub
    'Protected Sub GrdDirects_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles GrdDirects.PageIndexChanged
    '    Try
    '        GrdDirects.CurrentPageIndex = 0
    '        GrdDirects.CurrentPageIndex = e.NewPageIndex
    '        GrdDirects.DataSource = Session("DirectData1")
    '        GrdDirects.DataBind()
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Protected Sub DdlLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlLevel.SelectedIndexChanged

    End Sub

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click

        LevelDetail(1)


    End Sub

    'Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    '    Try
    '        If Conn.State = ConnectionState.Open Then
    '            Conn.Close()
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
    '    Try
    '        If Conn.State = ConnectionState.Open Then
    '            Conn.Close()
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Protected Sub rbtnsearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.SelectedIndexChanged

    'End Sub



    Protected Sub rbtnsearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnsearch.SelectedIndexChanged
        If rbtnsearch.SelectedValue = "L" Then
            lbllevel.Visible = True

        Else
            lbllevel.Visible = False


        End If
    End Sub
End Class
