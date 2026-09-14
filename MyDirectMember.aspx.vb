Imports System.Data.SqlClient
Imports System.Data
Partial Class MyDirectMember
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
            If Not Page.IsPostBack Then
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
            formNo = dt.Rows(0)("FormNo")
        Else
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again."
            lblError.Visible = True
            txtMemberId.Text = ""
        End If
        Return formNo
    End Function

    Protected Sub FillLevel()
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        Dim str As String
        Dt = New DataTable
        str = "select distinct MLevel,LevelName from(Select 0 as MLevel,'--All--' As LevelName Union ALL select Mlevel,'Level:'+ Cast(MLevel as varchar(20)) as MLevelName from R_MemTreeRelation) as Temp order by MLevel "
        Comm = New SqlCommand(str, Conn)
        Ad = New SqlDataAdapter(Comm)
        Dt = New DataTable
        Ad.Fill(Dt)
        DDLLevel.DataSource = Dt
        DDLLevel.DataTextField = "LevelName"
        DDLLevel.DataValueField = "MLevel"
        DDLLevel.DataBind()
        Conn.Close()
    End Sub
    Protected Sub LevelDetail()
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        Dim formNo As String
        Dim condition As String = ""
        Dim condition1 As String = ""
        If ChkDate.Checked Then
            If txtFromDate.Text <> "" And TxtToDate.Text <> "" Then

                If DDlDate.SelectedValue = "A" Then
                    condition1 = "AND a.ActiveStatus='Y' and  Cast(Replace(Convert(Varchar,a.UpgradeDate,106),' ','-')as Date) >='" & txtFromDate.Text & "' And Cast(Replace(Convert(Varchar,a.UpgradeDate,106),' ','-')as Date)<='" & TxtToDate.Text & "'"
                ElseIf DDlDate.SelectedValue = "J" Then
                    condition1 = "AND Cast(Replace(Convert(Varchar,a.Doj,106),' ','-')as Date) >='" & txtFromDate.Text & "' And Cast(Replace(Convert(Varchar,a.DOj,106),' ','-')as Date)<='" & TxtToDate.Text & "'"
                End If

            End If
        Else
            condition1 = ""

        End If

        If txtMemberId.Text = "" Then
            condition = ""
        Else
            formNo = GetFormNo()
            condition = "B.formNo='" & formNo & "'"
        End If
        Dim WhereCond As String = ""
        If DDLLevel.SelectedValue <> "0" Then
            WhereCond = " AND B.MLevel='" & DDLLevel.SelectedValue & "' "
        End If
        If condition = "" Then
            Comm = New SqlCommand("select A.SessId as SessId,a.IDNo as IdNo,A.MemFirstName + ' ' + A.MemLastName as MemName,a.Mobl as MobileNo,a.City as city," & _
"A.Bv as Unit,CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as ActiveStatus," & _
" Ref.IDNo as RefIdNo,Ref.MemFirstName as ReferalName,B.MLevel,dbo.FormatDate(A.Doj,'dd-MMM-yyyy') As Doj," & _
"CASE WHEN A.ActiveStatus='Y' Then dbo.FormatDate(A.UpgradeDate,'dd-MMM-yyyy') else '' End As UpgradeDate " & _
 "From M_MemberMaster as A Inner join  R_MemTreeRelation as B On A.FormNo=B.FormNoDwn " & _
  " Inner Join m_membermaster As Ref On A.RefFormNo=Ref.FormNo ")
        Else
            Comm = New SqlCommand("select A.SessId as SessId,a.IDNo as IdNo,A.MemFirstName + ' ' + A.MemLastName as MemName,a.Mobl as MobileNo,a.City as city," & _
"A.Bv as Unit,CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as ActiveStatus," & _
" Ref.IDNo as RefIdNo,Ref.MemFirstName as ReferalName,B.MLevel,dbo.FormatDate(A.Doj,'dd-MMM-yyyy') As Doj," & _
"CASE WHEN A.ActiveStatus='Y' Then dbo.FormatDate(A.UpgradeDate,'dd-MMM-yyyy') else '' End As UpgradeDate " & _
 "From M_MemberMaster as A Inner join  R_MemTreeRelation as B On A.FormNo=B.FormNoDwn " & _
  " Inner Join m_membermaster As Ref On " & _
" A.RefFormNo=Ref.FormNo Where" & _
" " & condition & " " & WhereCond & "" & condition1 & "")
        End If
        
        Comm.Connection = Conn
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
            If txtMemberId.Text = "" Then
                condition = ""
            Else
                formNo = GetFormNo()
                condition = "B.formNo='" & formNo & "'"
            End If
            Dim WhereCond As String = ""
            If DDLLevel.SelectedValue <> "0" Then
                WhereCond = " AND B.MLevel='" & DDLLevel.SelectedValue & "' "
            End If
            If ChkDate.Checked Then
                If txtFromDate.Text <> "" And TxtToDate.Text <> "" Then
                    If DDlDate.SelectedValue = "A" Then
                        condition1 = "AND a.ActiveStatus='Y' and  Cast(Replace(Convert(Varchar,a.UpgradeDate,106),' ','-')as Date) >='" & txtFromDate.Text & "' And Cast(Replace(Convert(Varchar,a.UpgradeDate,106),' ','-')as Date)<='" & TxtToDate.Text & "'"
                    ElseIf DDlDate.SelectedValue = "J" Then
                        condition1 = "AND Cast(Replace(Convert(Varchar,a.Doj,106),' ','-')as Date) >='" & txtFromDate.Text & "' And Cast(Replace(Convert(Varchar,a.DOj,106),' ','-')as Date)<='" & TxtToDate.Text & "'"
                    End If
                End If
             
            Else
                condition1 = ""

            End If
            sql = "select a.IDNo as IdNo,A.MemFirstName + ' ' + A.MemLastName as MemName,a.Mobl as MobileNo,a.City as city," & _
"A.Bv as BV,CASE WHEN A.ActiveStatus='Y' Then 'Active' else 'Deactive' End as ActiveStatus," & _
" Ref.IDNo as SponsorId,Ref.MemFirstName as SponsorName,B.MLevel as Level,dbo.FormatDate(A.Doj,'dd-MMM-yyyy') As Doj," & _
"CASE WHEN A.ActiveStatus='Y' Then dbo.FormatDate(A.UpgradeDate,'dd-MMM-yyyy') else '' End As UpgradeDate " & _
 "From M_MemberMaster as A Inner join  R_MemTreeRelation as B On A.FormNo=B.FormNoDwn " & _
  " Inner Join m_membermaster As Ref On " & _
" A.RefFormNo=Ref.FormNo Where" & _
" " & condition & " " & WhereCond & "" & condition1 & ""
          
            dtTemp = New DataTable
            dtTemp = Obj.GetData(sql)

            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("DirectMember.xls", dg)

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
End Class
