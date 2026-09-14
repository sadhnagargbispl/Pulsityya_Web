Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class MakeFranchiseReport
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

                FillReport()
            End If


        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillReport()
        Try

            Dim sql As String = String.Empty
            If Session("CompId") = "1049" Then
                sql = " Select a.IDno,a.Name as MemFirstName,c.stateName,replace(Convert(varchar,a.FranchiseDate,106),' ','-') as Date,a.Remark   "
                sql &= " from M_Makefranchise a With(nolock)"
                sql &= " , M_MemberMaster  b With(nolock) Left Join M_StateDivmaster as c On b.StateCode=c.StateCode and c.RowStatus='Y'" & _
                " where a.formno = b.formno"

            Else
                sql = " Select a.IDno,b.MemFirstName,a.stateName,replace(Convert(varchar,ractimestamp,106),' ','-') as Date ,  "
                sql &= " '' as Remark from M_Makefranchise a With(nolock)"
                sql &= " Left Join M_MemberMaster  b With(nolock) on a.formno = b.formno"

            End If
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            GvData.DataSource = dt
            GvData.DataBind()
            If Session("CompId") = "1049" Then
                GvData.Columns(5).Visible = True
            Else
                GvData.Columns(5).Visible = False

            End If
        Catch ex As Exception

        End Try
    End Sub











End Class
