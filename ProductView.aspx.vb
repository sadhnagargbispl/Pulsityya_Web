Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class ProductView
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim ReqNo As String
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim scrname As String
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("OrderID")) = False Then
            ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("OrderID")))
        End If
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("OrderID")) = False Then
                    LblOrderNo.Text = Request("OrderID")
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
            Dim sql As String = ""
            sql = " Exec Sp_getProductView '" & Request("OrderID") & "'"
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            If dtData.Rows.Count > 0 Then
                LblMemberID.Text = dtData.Rows(0)("Member ID")
                LblMemberName.Text = dtData.Rows(0)("Member Name")
            End If
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
