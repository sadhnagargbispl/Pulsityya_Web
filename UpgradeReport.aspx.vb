Imports System.Data.SqlClient
Imports System.Data
Partial Class UpgradeReport
    Inherits System.Web.UI.Page
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Dt As DataTable
    Dim Ad As SqlDataAdapter
    Dim Obj As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Member / Upgrade Report"
            If Not Page.IsPostBack Then
                ' FillLevel()
                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        txtMemberId.Text = Request.QueryString("key").ToString
                        LevelDetail()
                    End If
                End If
            End If

        Else
            Response.Redirect("logout.aspx")
        End If

    End Sub

    Protected Sub GrdDirects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdDirects.PageIndexChanging
        GrdDirects.PageIndex = e.NewPageIndex
        GrdDirects.DataSource = Session("DirectData1")
        GrdDirects.DataBind()
    End Sub


    Private Function GetFormNo() As String
        Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim idNo As String
        Dim formno As String
        idNo = txtMemberId.Text
        Dim qry As String = "Select FormNo from " & Obj.tblMemberMaster & " where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = Obj.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblError.Visible = True
            txtMemberId.Text = ""
        End If
        Return formno
    End Function

    'Protected Sub FillLevel()
    '    Dim Formno As String = ""
    '    Dim condition As String = ""
    '    Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '    Conn.Open()
    '    Dim str As String
    '    Dt = New DataTable
    '    If txtMemberId.Text <> "" Then
    '        Formno = GetFormNo()
    '        If Formno <> 0 Then
    '            condition = " And formnoDwn=" & Formno & ""
    '        Else
    '            condition = ""
    '        End If
    '    End If
    '    str = "select distinct MLevel,LevelName from(Select 0 as MLevel,'--All--' As LevelName Union ALL select Mlevel,'Level:'+ Cast(MLevel as varchar(20)) as MLevelName from M_MemTreeRelation where 1=1 " & condition & " ) as Temp order by MLevel "
    '    Comm = New SqlCommand(str, Conn)
    '    Ad = New SqlDataAdapter(Comm)
    '    Dt = New DataTable
    '    Ad.Fill(Dt)
    '    DDLLevel.DataSource = Dt
    '    DDLLevel.DataTextField = "LevelName"
    '    DDLLevel.DataValueField = "MLevel"
    '    DDLLevel.DataBind()
    '    Conn.Close()
    'End Sub
    Protected Sub LevelDetail()
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        Dim formNo As String
        Dim condition As String = ""
        Dim condition1 As String = ""
        If ChkDate.Checked Then
            'condition1 = condition1 & "AND Cast(Convert(varchar,a.UpgradeDate,106) as Datetime) >='" & txtFromDate.Text & "' And Cast(Convert(varchar,a.UpgradeDate,106) as Datetime)<='" & TxtToDate.Text & "' AND A.ActiveStatus='Y' "
            If DDlDate.SelectedValue = "A" Then
                If txtFromDate.Text <> "" Then
                    condition1 = condition1 & "AND Cast(Convert(varchar,a.UpgradeDate,106) as Datetime) >='" & txtFromDate.Text & "' AND A.ActiveStatus='Y' "
                End If
                If TxtToDate.Text <> "" Then
                    condition1 = condition1 & " And Cast(Convert(varchar,a.UpgradeDate,106) as Datetime)<='" & TxtToDate.Text & "' AND A.ActiveStatus='Y' "

                End If
            ElseIf DDlDate.SelectedValue = "J" Then
                If txtFromDate.Text <> "" Then
                    condition1 = condition1 & "AND a.Doj>='" & txtFromDate.Text & "'"
                End If
                If TxtToDate.Text <> "" Then
                    condition1 = condition1 & " And a.DOj<='" & TxtToDate.Text & "'"
                End If
            End If
        Else
            condition1 = ""

        End If

        If txtMemberId.Text = "" Then
            condition = ""
        Else
            formNo = GetFormNo()
            condition1 = condition1 & " and  Formno='" & formNo & "'"
        End If

        'If DDLLevel.SelectedValue <> "0" Then
        '    condition1 = condition1 & " AND B.MLevel='" & DDLLevel.SelectedValue & "' "
        'End If
        Comm = New SqlCommand(" select * from(select a.Formno,a.Idno,(b.MemFirstName+' '+b.MemLastName) as MemberName," & _
                              " c.Kitname,c.Bv,Replace(Convert(varchar,A.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.UpgradeDate AS TIME),100) as UpgradeDate  from PreTopUp_MemDtl as a,M_MemberMaster as b," & _
                              " M_KitMaster as c  where a.Formno=b.Formno and a.KitId=c.KitId and (c.RowStatus='Y' Or c.KitId=2) " & _
                              " Union all " & _
                             " select b.Formno,b.Idno,(b.MemFirstName+' '+b.MemLastName) as MemberName,c.Kitname,c.Bv,Replace(Convert(varchar,b.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(b.UpgradeDate AS TIME),100) as UpgradeDate from " & _
                             " M_MemberMaster as b,M_KitMaster as c  where  b.KitId=c.KitId and (c.RowStatus='Y' Or c.KitId=2)) as Temp " & _
                             "where 1=1 " & condition1 & " order by IdNo,Year(UpgradeDate),Month(UpgradeDate),Day(UpgradeDate)", Conn)


        '        Comm.Connection = Conn
        Ad = New SqlDataAdapter(Comm)
        Dt = New DataTable
        Ad.Fill(Dt)
        Session("DirectData1") = Dt
        GrdDirects.DataSource = Dt
        GrdDirects.DataBind()
        Conn.Close()
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        LevelDetail()
    End Sub

    Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid

            Dim sql As String
            Dim formNo As String
            Dim condition As String = ""
            Dim condition1 As String = ""
            If ChkDate.Checked Then
                'condition1 = condition1 & "AND Cast(Convert(varchar,a.UpgradeDate,106) as Datetime) >='" & txtFromDate.Text & "' And Cast(Convert(varchar,a.UpgradeDate,106) as Datetime)<='" & TxtToDate.Text & "' AND A.ActiveStatus='Y' "
                If DDlDate.SelectedValue = "A" Then
                    If txtFromDate.Text <> "" Then
                        condition1 = condition1 & "AND Cast(Convert(varchar,a.UpgradeDate,106) as Datetime) >='" & txtFromDate.Text & "' AND A.ActiveStatus='Y' "
                    End If
                    If TxtToDate.Text <> "" Then
                        condition1 = condition1 & " And Cast(Convert(varchar,a.UpgradeDate,106) as Datetime)<='" & TxtToDate.Text & "' AND A.ActiveStatus='Y' "

                    End If
                ElseIf DDlDate.SelectedValue = "J" Then
                    If txtFromDate.Text <> "" Then
                        condition1 = condition1 & "AND a.Doj>='" & txtFromDate.Text & "'"
                    End If
                    If TxtToDate.Text <> "" Then
                        condition1 = condition1 & " And a.DOj<='" & TxtToDate.Text & "'"
                    End If
                End If
            Else
                condition1 = ""

            End If

            If txtMemberId.Text = "" Then
                condition = ""
            Else
                formNo = GetFormNo()
                condition1 = condition1 & " and  formno='" & formNo & "'"
            End If

            'If DDLLevel.SelectedValue <> "0" Then
            '    condition1 = condition1 & " AND B.MLevel='" & DDLLevel.SelectedValue & "' "
            'End If
            Obj = New DAL((HttpContext.Current.Session("MlmDatabase" & Session("CompID"))))
            sql = " select * from(select a.Formno,a.Idno,(b.MemFirstName+' '+b.MemLastName) as MemberName," & _
                              " c.Kitname,c.Bv,Replace(Convert(varchar,A.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.UpgradeDate AS TIME),100) as UpgradeDate  from PreTopUp_MemDtl as a,M_MemberMaster as b," & _
                              " M_KitMaster as c  where a.Formno=b.Formno and a.KitId=c.KitId and (c.RowStatus='Y' Or c.KitId=2) " & _
                              " Union all " & _
                             " select b.Formno,b.Idno,(b.MemFirstName+' '+b.MemLastName) as MemberName,c.Kitname,c.Bv,Replace(Convert(varchar,b.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(b.UpgradeDate AS TIME),100) as UpgradeDate from " & _
                             " M_MemberMaster as b,M_KitMaster as c  where  b.KitId=c.KitId and (c.RowStatus='Y' Or c.KitId=2)) as Temp " & _
                             "where 1=1 " & condition1 & " order by IdNo,Year(UpgradeDate),Month(UpgradeDate),Day(UpgradeDate)"


            dtTemp = New DataTable
            dtTemp = Obj.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("UpgradeReport.xls", dg)

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

    Protected Sub GrdDirects_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdDirects.SelectedIndexChanged

    End Sub

 

    'Protected Sub txtMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberId.TextChanged
    '    FillLevel()
    'End Sub

    Protected Sub ChkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkDate.CheckedChanged
        If ChkDate.Checked Then
            txtFromDate.Enabled = True
            TxtToDate.Enabled = True
            DDlDate.Enabled = True
        Else
            TxtToDate.Text = ""
            txtFromDate.Text = ""
            txtFromDate.Enabled = False
            TxtToDate.Enabled = False
            DDlDate.Enabled = False
        End If

    End Sub

    Protected Sub TxtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtToDate.TextChanged
        Dim startDate, EndDate As DateTime
        EndDate = Convert.ToDateTime(TxtToDate.Text)
        startDate = Convert.ToDateTime(txtFromDate.Text)
        If startDate.Date > EndDate.Date Then
            LblError.Text = "Enter Greater Date from Start Date"
            LblError.ForeColor = Drawing.Color.Red
            LblError.Visible = True
        Else
            LblError.Visible = False

        End If
    End Sub
End Class
