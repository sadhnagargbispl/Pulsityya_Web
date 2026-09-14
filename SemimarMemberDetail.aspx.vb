Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class SemimarMemberDetail
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL

    Dim ReqNo As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim scrname As String


        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("reqno")) = False Then
                    'LblNo.Text = " IdNo :" & Request("IdNo")
                    BindData()

                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
    End Sub

    Public Sub BindData(Optional ByVal SrchCond As String = "")
        Try
            Dim cond As String = ""
            Dim formno As String = ""


           
            ''Dim sql1 As String = "Select idno,NAme,Mobile,Email,City,PassCode from TrnSemimarMember Where  Reqno = '" & Request("reqno") & "'"
            Dim sql1 As String = "Exec Sp_SeminarMemberDetail '" & Request("reqno") & "'"

            dtData = New DataTable
            dtData = objDAL.GetData(sql1)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub



End Class