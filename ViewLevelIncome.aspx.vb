

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewLevelIncome
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
        If String.IsNullOrEmpty(Request("IdNo")) = False Then
            ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("IdNo")))
        End If
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("IdNo")) = False Then
                    BindData()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
    End Sub
    Private Function GetFormNo() As String
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
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
            Dim sql1 As String = ""
            formno = GetFormNo()
            If Session("compid") = "1090" Then
                sql1 = " Select A.SessID,replace(CONVERT(VARCHAR(11), CONVERT(DATE, CONVERT(VARCHAR(8), a.sessid), 112), 106),' ','-') As FrmDate,"
                sql1 &= "B.IDNo, B.MemFirstName,A.Slab,A.Comm,a.MLevel,pairincome as MatchingIncome  From MstLevelIncome As A "
                sql1 &= "Inner join M_MemberMaster As B On A.FormNoDwn = B.FormNo "
                sql1 &= " Where A.FormNo = '" & Val(formno) & "' And A.SessID = '" & Request("Sessid") & "' and Comm <> 0 Order by a.MLevel "
            ElseIf Session("compid") = "1091" Then
                sql1 = " Select A.SessID,replace(CONVERT(VARCHAR(11), CONVERT(DATE, CONVERT(VARCHAR(8), a.sessid), 112), 106),' ','-') As FrmDate,"
                sql1 &= "B.IDNo, B.MemFirstName,A.Slab,A.Comm,A.BV  From MstRefIncome As A "
                sql1 &= "Inner join M_MemberMaster As B On A.FormNoDwn = B.FormNo "
                sql1 &= " Where A.FormNo = '" & Val(formno) & "' And A.SessID = '" & Request("Sessid") & "' and Comm > 0 "
            ElseIf Session("compid") = "1095" Then
                sql1 = " Exec SP_LevelIncomeReport '" & Val(formno) & "','" & Request("Sessid") & "' "
            Else
                sql1 = " Select A.SessID,replace(CONVERT(VARCHAR(11), CONVERT(DATE, CONVERT(VARCHAR(8), a.sessid), 112), 106),' ','-') As FrmDate,"
                sql1 &= "B.IDNo, B.MemFirstName,A.Slab,A.Comm,a.MLevel,pairincome as MatchingIncome  From MstLevelIncome As A "
                sql1 &= "Inner join M_MemberMaster As B On A.FormNoDwn = B.FormNo "
                sql1 &= " Where A.FormNo = '" & Val(formno) & "' And A.DSessID = '" & Request("Sessid") & "' and Comm <> 0 Order by a.MLevel "
            End If  
            dtData = New DataTable
            dtData = objDAL.GetData(sql1)
            If Session("CompId") = 1091 Then
                Gvdatasolfit.DataSource = dtData
                Gvdatasolfit.DataBind()
                Gvdatasolfit.Visible = True
                GvData.Visible = False
            Else
                GvData.DataSource = dtData
                GvData.DataBind()
                Gvdatasolfit.Visible = False
                GvData.Visible = True
            End If
          
            Session("GData") = dtData
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        If Session("CompId") = 1091 Then
            Gvdatasolfit.PageIndex = e.NewPageIndex
            Gvdatasolfit.DataSource = Session("GData")
            Gvdatasolfit.DataBind()
        Else
            GvData.PageIndex = e.NewPageIndex
            GvData.DataSource = Session("GData")
            GvData.DataBind()
        End If
        
    End Sub
End Class
