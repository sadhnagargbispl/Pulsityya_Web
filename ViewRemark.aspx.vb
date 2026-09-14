Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewRemark
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

    'Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
    '    BindData()
    '    btnShowRecord.Visible = False
    '    lblView.Visible = False
    '    ddlSearchFields.SelectedIndex = 0
    '    txtSearch.Text = ""
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim scrname As String
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Dim ReqNo As String
        If String.IsNullOrEmpty(Request("ReqNo")) = False Then
            'ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("ReqNo")))
            ReqNo = Request("ReqNo")
        End If
        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("ReqNo")) = False Then
                    LblNo.Text = " Request No :" & ReqNo
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

            Dim sql As String = "select Formno,ReqNo,Case when Status='R' then RejectRemark else Remarks end as Remarks from TrnPinReqMain where ReqNo='" & ReqNo & "'" & SrchCond
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            Dim str As String = "Select a.ReqNo,a.FormNo,b.IdNo,a.KitID,a.KitName,b.MemFirstName+' '+ b.MemLastName as MemName,a.Qty,a.RemainQty ,a.DispQty,CASE WHEN a.Status='P' THEN 'Pending' ELSE 'Clear' END AS Status,a.KitAmount FROM TrnPinRequest a,M_MemberMaster b Where  a.FormNo=b.FormNo  and a.ReqNo='" & ReqNo & "'" & SrchCond
            dtData = New DataTable
            dtData = objDAL.GetData(str)
            GrdData.DataSource = dtData
            GrdData.DataBind()
            Session("GvData") = dtData

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub GrdData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdData.PageIndexChanging
        GrdData.PageIndex = e.NewPageIndex
        GrdData.DataSource = Session("GvData")
        GrdData.DataBind()
    End Sub


End Class
