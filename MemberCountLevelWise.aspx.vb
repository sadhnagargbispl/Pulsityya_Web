Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class MemberCountLevelWise
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
                'Session("PageName") = "Member / Update Member Profile"
                '' UserStatus.InnerHtml = "<p>Welcome " & Session("MemName") & "(" & Session("FormNo") & ") To" & Session("CompName") & " </p>"

            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then


                ''FillReport()
            End If


        Catch ex As Exception

        End Try
    End Sub

    
    Private Sub FillReport()
        Try

            Dim sql As String = String.Empty
            Dim scrname As String = String.Empty

            If txtMemberID.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Please Enter Member ID.');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                Exit Sub
            End If
            sql = "Select    Idno,MemFirstName,MLevel ,Sum(ActCnt) as ActCnt ,Sum(DActCnt) as DActCnt, (Sum(ActCnt) + Sum(DActCnt) ) As TotalMember   From "
            sql &= " ( "
            sql &= " Select a.MLevel, c.Idno,c.MemFirstName, Count(*) as  ActCnt, 0 as DActCnt  from R_MemTreeRelation a "
            sql &= " inner  Join  M_memberMaster b on  a.FormNoDwn  = b.formno"
            sql &= " inner  Join  M_memberMaster c on a.formno = c.formno "
            sql &= "  Where C.idno = '" & txtMemberID.Text & "' "
            sql &= "  And b.Activestatus = 'Y'"
            sql &= " group by a.MLevel,c.Idno,c.MemFirstName"
            sql &= "  Union All "

            sql &= " Select a.MLevel,c.Idno,c.MemFirstName,0 as  ActCnt, Count(*) as DActCnt  from R_MemTreeRelation a "
            sql &= " inner  Join  M_memberMaster b on  a.FormNoDwn  = b.formno"
            sql &= " inner  Join  M_memberMaster c on a.formno = c.formno "
            sql &= " Where C.idno = '" & txtMemberID.Text & "' "
            sql &= " And b.Activestatus = 'N'"
            sql &= " group by a.MLevel,c.Idno,c.MemFirstName"
            sql &= " )  as temp"
            sql &= " Group by Idno,MemFirstName,MLevel"




            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            GvData.DataSource = dt
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillReport()
    End Sub
End Class
