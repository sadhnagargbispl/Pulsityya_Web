Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class FlatDetail
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try


            If Session("AStatus") = "OK" Then

            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            '   objModuleFun = New ModuleFunction()
            Dim dt As New DataTable
            If Not Page.IsPostBack Then
                'txtSearch.Text = ""
                If Request.QueryString.HasKeys And Not Request.QueryString("Id") Is Nothing Then
                    ID = Val(Crypto.Decrypt(Replace(Request.QueryString("Id"), " ", "+")))
                    Dim sql As String
                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    sql = "SELECT * from V#ProjectDetail where ProjectId='" & Val(ID) & "' Order by FloorType"
                    dt = objDAL.GetData(sql)
                    If dt.Rows.Count > 0 Then
                        GvData.DataSource = dt
                        GvData.DataBind()
                    End If
                End If

                If Convert.ToInt32(Session("grdIndex")) = 0 Then
                Else
                    GvData.PageIndex = Convert.ToInt32(Session("grdIndex"))
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub BindData()
        Try
            Dim _Condition As String = ""
            Dim Qry As String = "Select * From V#ProjectMaster Where 1=1" & _Condition & " Order by ID Desc"
            dtData = New DataTable
            dtData = objDAL.GetData(Qry)
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
        Session("grdIndex") = GvData.PageIndex
    End Sub

    'Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
    '    Dim i As Integer
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        If String.Equals(e.Row.Cells(4).Text.ToLower(), "deactive") = True Then
    '            'e.Row.BackColor = Drawing.Color.Red
    '            ' e.Row.Style("background-image") = "images/redback2.jpg"
    '            For i = 0 To e.Row.Cells.Count - 1
    '                e.Row.Cells(i).Style("color") = "red"
    '            Next
    '        End If
    '    End If
    'End Sub

   


End Class
