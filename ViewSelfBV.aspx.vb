Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewSelfBV
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim ReqNo As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    'Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
    '    BindData()
    '    btnShowRecord.Visible = False
    '    lblView.Visible = False
    '    ddlSearchFields.SelectedIndex = 0
    '    txtSearch.Text = ""
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim scrname As String
        'objModuleFun = New ModuleFunction()
        'Dim ReqNo As String
        If String.IsNullOrEmpty(Request("IdNo")) = False Then
            'ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("IdNo")))

        End If
        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("IdNo")) = False Then
                    'LblNo.Text = " Request No :" & ReqNo
                    BindData()
                End If
                'GetFormNo()
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
    End Sub
    Private Function GetFormNo() As String
        'objDAL = New DAL()
        Dim idNo As String
        Dim formno As String
        idNo = Request("Idno")
        Dim qry As String = "Select FormNo from M_MemberMaster where IdNo='" & idNo & "'"
        Dim dt As New DataTable
        dt = objDAL.GetData(qry)
        If (dt.Rows.Count > 0) Then
            formno = dt.Rows(0)("FormNo")
        Else
            formno = 0
        End If
        Return formno
    End Function
    Public Sub BindData(Optional ByVal SrchCond As String = "")
        Try

            Dim cond As String = ""
            Dim formno As String = ""
            'formno = GetFormNo()

            Dim sql As String = "EXEC sp_GetSelfBv '" & Request("Idno") & "','" & Request("amount") & "','" & Request("startdate") & "','" & Request("enddate") & "' "

            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub



End Class
