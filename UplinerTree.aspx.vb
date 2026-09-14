Imports System.Data.SqlClient
Imports System.Data
Partial Class UplinerTree
    Inherits System.Web.UI.Page
    'Dim Conn As SqlConnection
    'Dim Comm As SqlCommand
    Dim Dt As DataTable
    Dim Ad As SqlDataAdapter
    Dim Obj As DAL
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try

            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Session("AStatus") = "OK" Then
                If Not Page.IsPostBack Then
                    Session("PageName") = "Member / Upliner Report"
                    FillLevel()
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
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GrdDirects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdDirects.PageIndexChanging
        GrdDirects.PageIndex = e.NewPageIndex
        GrdDirects.DataSource = Session("DirectData1")
        GrdDirects.DataBind()
    End Sub


    Private Function GetFormNo() As String
        Try

      
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
                LblError.Text = "Member Id does not exist. Please check it once and then enter it again."
                LblError.Visible = True
                txtMemberId.Text = ""
            End If
            Return formno
        Catch ex As Exception

        End Try
    End Function

    Protected Sub FillLevel()
        Try

       
            Dim Formno As String = ""
            Dim condition As String = ""
            'Conn = New SqlConnection(Application("Connect"))
            'Conn.Open()
            Dim str As String
            Dt = New DataTable
            If txtMemberId.Text <> "" Then
                Formno = GetFormNo()
                If Formno <> 0 Then
                    condition = " And formnoDwn=" & Formno & ""
                Else
                    condition = ""
                End If
            End If
            If RbtType.SelectedValue = "B" Then
                str = "select distinct MLevel,LevelName from(Select 0 as MLevel,'--All--' As LevelName Union ALL select Mlevel,'Level:'+ Cast(MLevel as varchar(20)) as MLevelName from M_MemTreeRelation where 1=1 " & condition & " ) as Temp order by MLevel "

            Else
                str = "select distinct MLevel,LevelName from(Select 0 as MLevel,'--All--' As LevelName Union ALL select Mlevel,'Level:'+ Cast(MLevel as varchar(20)) as MLevelName from R_MemTreeRelation where 1=1 " & condition & " ) as Temp order by MLevel "

            End If
            'Comm = New SqlCommand(str, Conn)
            'Ad = New SqlDataAdapter(Comm)
            Dt = New DataTable
            Dt = Obj.GetData(str)
            ' Ad.Fill(Dt)
            DDLLevel.DataSource = Dt
            DDLLevel.DataTextField = "LevelName"
            DDLLevel.DataValueField = "MLevel"
            DDLLevel.DataBind()
            ' Conn.Close()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub LevelDetail()
        Try

       
            'Conn = New SqlConnection(Application("Connect"))
            'Conn.Open()
            Dim formNo As String
            Dim condition As String = ""
            Dim condition1 As String = ""
            Dim str As String = ""
            'If ChkDate.Checked Then
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
                    condition1 = condition1 & "AND Cast(a.Doj as Date)>='" & txtFromDate.Text & "'"
                End If
                If TxtToDate.Text <> "" Then
                    condition1 = condition1 & " And Cast(a.DOj as Date)<='" & TxtToDate.Text & "'"
                End If
            End If
            'Else
            'condition1 = ""

            'End If

            If txtMemberId.Text = "" Then
                condition = ""
            Else
                formNo = GetFormNo()
                condition1 = condition1 & " and  B.formNoDwn='" & formNo & "'"
            End If

            If DDLLevel.SelectedValue <> "0" Then
                condition1 = condition1 & " AND B.MLevel='" & DDLLevel.SelectedValue & "' "
            End If
            If RbtType.SelectedValue = "B" Then
                str = " select a.Idno as [ID No],a.MemFirstName+' '+a.MemLastName as [Member Name] ,Replace(Convert(varchar,Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(Doj AS TIME),100) as [Date Of Joining]," & _
                                  " CASE WHEN A.ActiveStatus='Y' Then Replace(Convert(varchar,A.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.UpgradeDate AS TIME),100) else '' End As [Date Of Activation]," & _
                                  " CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as Status,c.KitName as [Package Name],b.mLevel as Level from  M_MemberMaster as a with(Nolock) Inner Join M_MemTreeRelation as b with(Nolock)" & _
                                  " On a.FormNo=b.FormNo Inner Join M_Kitmaster as c with(Nolock) On a.KitId=c.KitId  " & _
                                  " where  (c.RowStatus='Y')" & condition1 & " Order by MLevel"
            Else
                str = " select a.Idno as [ID No],a.MemFirstName+' '+a.MemLastName as [Member Name] ,Replace(Convert(varchar,Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(Doj AS TIME),100) as [Date Of Joining]," & _
                                 " CASE WHEN A.ActiveStatus='Y' Then Replace(Convert(varchar,A.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.UpgradeDate AS TIME),100) else '' End As [Date Of Activation]," & _
                                 " CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as Status,c.KitName as [Package Name],b.mLevel as Level,isnull(t.Rank,'')As Cadre  from " & _
                                 " M_MemberMaster as a  with(Nolock)Inner Join R_MemTreeRelation as b with(Nolock) " & _
                                 " On a.FormNo=b.FormNo Inner Join M_Kitmaster as c with(Nolock) On a.KitId=c.KitId  Left Join (select a.Formno,b.Rank from (select Formno,Max(RankId)as RankId from  MstRankAchievers with(Nolock) " & _
                                 " Group by Formno)as a,MstRanks as b  with(Nolock)    where a.RankId=b.RankId and b.ActiveStatus='Y')as t On t.Formno=A.Formno Left Join " & _
                                 " (select formno,sum(selfPV)+sum(LevelPv1) as TotalPV from  ViewDirecBvPV Group By Formno)as x On x.Formno=a.Formno    " & _
                                 " where  (c.RowStatus='Y')" & condition1 & " Order by MLevel"

            End If

            '        Comm.Connection = Conn
            'Ad = New SqlDataAdapter(Comm)
            Dt = New DataTable
            ' Ad.Fill(Dt)
            Dt = Obj.GetData(str)
            Session("DirectData1") = Dt
            GrdDirects.DataSource = Dt
            GrdDirects.DataBind()
            'Conn.Close()
        Catch ex As Exception

        End Try
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
            '    If ChkDate.Checked Then
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
                    condition1 = condition1 & "AND cast(a.Doj as Date)>='" & txtFromDate.Text & "'"
                End If
                If TxtToDate.Text <> "" Then
                    condition1 = condition1 & " And cast(a.DOj as Date)<='" & TxtToDate.Text & "'"
                End If
            End If
            'Else
            'condition1 = ""

            'End If

            If txtMemberId.Text = "" Then
                condition = ""
            Else
                formNo = GetFormNo()
                condition1 = condition1 & " and  B.formNoDwn='" & formNo & "'"
            End If

            If DDLLevel.SelectedValue <> "0" Then
                condition1 = condition1 & " AND B.MLevel='" & DDLLevel.SelectedValue & "' "
            End If
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If RbtType.SelectedValue = "B" Then
                sql = " select a.Idno as [ID No],a.MemFirstName+' '+a.MemLastName as [Member Name] ,Replace(Convert(varchar,Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(Doj AS TIME),100) as [Date Of Joining]," & _
                                  " CASE WHEN A.ActiveStatus='Y' Then Replace(Convert(varchar,A.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.UpgradeDate AS TIME),100) else '' End As [Date Of Activation]," & _
                                  " CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as Status,c.KitName as [Package Name],b.mLevel as Level from  M_MemberMaster as a with(Nolock) Inner Join M_MemTreeRelation as b with(Nolock)" & _
                                  " On a.FormNo=b.FormNo Inner Join M_Kitmaster as c with(Nolock) On a.KitId=c.KitId  " & _
                                  " where  (c.RowStatus='Y')" & condition1 & " Order by MLevel"
            Else
                sql = " select a.Idno as [ID No],a.MemFirstName+' '+a.MemLastName as [Member Name] ,Replace(Convert(varchar,Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(Doj AS TIME),100) as [Date Of Joining]," & _
                                 " CASE WHEN A.ActiveStatus='Y' Then Replace(Convert(varchar,A.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.UpgradeDate AS TIME),100) else '' End As [Date Of Activation]," & _
                                 " CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as Status,c.KitName as [Package Name],b.mLevel as Level,isnull(t.Rank,'')As Cadre from " & _
                                 " M_MemberMaster as a  with(Nolock)Inner Join R_MemTreeRelation as b with(Nolock) " & _
                                 " On a.FormNo=b.FormNo Inner Join M_Kitmaster as c with(Nolock) On a.KitId=c.KitId  Left Join (select a.Formno,b.Rank from (select Formno,Max(RankId)as RankId from  MstRankAchievers with(Nolock) " & _
                                 " Group by Formno)as a,MstRanks as b  with(Nolock)    where a.RankId=b.RankId and b.ActiveStatus='Y')as t On t.Formno=A.Formno Left Join " & _
                                 " (select formno,sum(selfPV)+sum(LevelPv1) as TotalPV from  ViewDirecBvPV Group By Formno)as x On x.Formno=a.Formno    " & _
                                 " where  (c.RowStatus='Y')" & condition1 & " Order by MLevel"

            End If

            'If RbtType.SelectedValue = "B" Then


            '    sql = " select a.Idno,a.MemFirstName+' '+a.MemLastName as MemberName ,Replace(Convert(varchar,Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(Doj AS TIME),100) as DateOfJoining," & _
            '                      " CASE WHEN A.ActiveStatus='Y' Then Replace(Convert(varchar,A.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.UpgradeDate AS TIME),100) else '' End As UpgradeDate," & _
            '                      " CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as Status,c.KitName as PackageName,b.mLevel as Level from  M_MemberMaster as a inner Join " & _
            '                      " M_MemTreeRelation as b on with(nolock) On a.FormNo=b.FormNo  inner Join M_Kitmaster as c  on with(nolock) on a.KitId=c.KitId" & _
            '                      " where  (c.RowStatus='Y' or c.KitId=2)" & condition1 & "Order by MLevel"

            'Else
            '    sql = " select a.Idno,a.MemFirstName+' '+a.MemLastName as MemberName ,Replace(Convert(varchar,Doj,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(Doj AS TIME),100) as DateOfJoining," & _
            '           " CASE WHEN A.ActiveStatus='Y' Then Replace(Convert(varchar,A.UpgradeDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(a.UpgradeDate AS TIME),100) else '' End As UpgradeDate," & _
            '          " CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as Status,c.KitName as PackageName,b.mLevel as Level ,t.Rank as Cadre from  M_MemberMaster as a with(nolock) inner join " & _
            '       " R_MemTreeRelation as b with(nolock) on a.FormNo=b.FormNo inner Join M_Kitmaster as c with(nolock) on a.KitId=c.KitId " & _
            '          " Left Join (select a.Formno,b.Rank from (select Formno,Max(RankId)as RankId from  MstRankAchievers " & _
            '         " Group by Formno)as a,MstRanks as b      where a.RankId=b.RankId and b.ActiveStatus='Y')as t On t.Formno=A.Formno" & _
            '         " where  (c.RowStatus='Y' or c.KitId=2)" & condition1 & "Order by MLevel"

            'End If
            dtTemp = New DataTable
            dtTemp = Obj.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("UplinerReport.xls", dg)

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

    Protected Sub DDLLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLLevel.SelectedIndexChanged

    End Sub

    Protected Sub txtMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberId.TextChanged
        FillLevel()
    End Sub

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

    Protected Sub RbtType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtType.SelectedIndexChanged
        FillLevel()
    End Sub
End Class
