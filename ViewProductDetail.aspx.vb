Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewProductDetail
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
            ReqNo = Request("ReqNo")
        End If
        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("ReqNo")) = False Then
                    LblNo.Text = " Order No :" & ReqNo
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


            'Dim sql As String = "Select a.ReqNo,a.FormNo,a.KitID,a.KitName," & _
            '" a.Qty,a.RemainQty ,a.DispQty,CASE WHEN a.Status='P' and b.Status='R' THEN 'Rejected' when a.Status='P'  and (b.Status='P' Or b.Status='A') then 'Pending'  ELSE 'Sent' END AS Status,a.KitAmount FROM " & _
            '" TrnPinRequest a,TrnPinReqMain as b Where  a.ReqNo=b.ReqNo  and a.ReqNo='" & ReqNo & "' and a.Formno='" & Session("Formno") & "'" & SrchCond

            Dim sql As String = "select *,Case when DispStatus='C' then 'Dispatch' else 'Pending' end as status  from TrnOrderDetail where Orderno='" & Request("ReqNo") & "'"
            dtData = New DataTable
            dtData = objDAL.GetData(sql)
            GvData.DataSource = dtData
            GvData.DataBind()
            Session("GData") = dtData
            'If dtData.Rows.Count > 0 Then
            '    btnExport.Enabled = True
            '    btnPrintAll.Enabled = True
            '    btnPrintCurrent.Enabled = True
            'Else
            '    btnExport.Enabled = False
            '    btnPrintAll.Enabled = False
            '    btnPrintCurrent.Enabled = False
            'End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub



End Class
