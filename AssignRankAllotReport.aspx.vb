Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class AssignRankAllotReport
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

                FillData()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillReport()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String = String.Empty
            sql = " Sp_IdWiseRankReport '" & txtMemberID.Text & "'," & ddlRank.SelectedValue & ""
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            If (dt.Rows.Count > 0) Then
                GvData.DataSource = dt
                GvData.DataBind()
                Session("GData") = dt

            Else
                GvData.DataSource = dt
                GvData.DataBind()
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('No record found.!!');", True)
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub FillData()
        Try
            Dim str As String = ""
            Dim dt As DataTable = New DataTable()
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            str = " Select 0 As RankId , '--Select Rank---' As Rank union All "
            str &= " Select Rankid,RankName as Rank from M_RankMaster Where  ActiveStatus = 'Y'"
            dt = objDAL.GetData(str)
            If (dt.Rows.Count > 0) Then
                ddlRank.DataSource = dt
                ddlRank.DataTextField = "Rank"
                ddlRank.DataValueField = "RankId"
                ddlRank.DataBind()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow.Click
        Try
            FillReport()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        Try


            GvData.PageIndex = e.NewPageIndex
            GvData.DataSource = Session("GData")
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub
End Class
