Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class MemberDetails

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
                If Request.QueryString("Auid") <> "" Then
                    Load_MemberDetail(Convert.ToInt32(Request.QueryString("Auid")))
                End If

                If Request.QueryString("Duid") <> "" Then
                    Load_MemberDetail1(Convert.ToInt32(Request.QueryString("Duid")))
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Load_MemberDetail(ByVal Id As Integer)
        Try

            Dim sql As String = String.Empty
            sql = " Select IDno,MemFirstName, replace(Convert(Varchar,Doj,106),' ','-') As Doj, "
            sql &= " replace(Convert(Varchar,UpgradeDate,106),' ','-') As UpgradeDate"
            sql &= " from  M_MemberMaster Where  stateCode  = '" & Id & "' And Activestatus = 'Y'  "
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            GvData.DataSource = dt
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Load_MemberDetail1(ByVal Id As Integer)
        Try

            Dim sql As String = String.Empty
            ''sql = " Select IDno,MemFirstName, replace(Convert(Varchar,Doj,106),' ','-') As Doj   from  M_MemberMaster Where  stateCode  = '" & Id & "'  And Activestatus = 'Y'   "


            sql = " Select IDno,MemFirstName, replace(Convert(Varchar,Doj,106),' ','-') As Doj, "
            sql &= " '' As UpgradeDate"
            sql &= " from  M_MemberMaster Where  stateCode  = '" & Id & "' And Activestatus = 'N'  "

            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(sql)
            GvData.DataSource = dt
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub


End Class
