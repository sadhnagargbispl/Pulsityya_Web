Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewEpinRqs
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


            Dim sql As String = "Select a.ReqNo,a.FormNo,a.KitID,a.KitName, a.Qty,a.RemainQty ,a.DispQty,CASE WHEN a.Status='P' and b.Status='R' THEN 'Rejected' when (a.Status='P' Or a.Status='A') then 'Pending'  ELSE 'Sent' END AS Status,a.KitAmount,ISNULL(c.AvailableQty,0) AvailableQty,Case When ISNULL(c.AvailableQty,0)>=a.Qty and (a.Status!='C') Then 'Y' else 'N' END IsAvailable,d.IdNo FROM  TrnPinReqMain as b,TrnPinRequest a left join (Select ProductName,Count(KitId) AvailableQty,KitId from V#EpinStatus Where KitId<>1 and  ReqFormNo='" & Session("IdNo") & "' And [Status]='UnUsed' Group by ProductName,KitId) c on a.kitid=c.KitId,M_MemberMaster d Where  a.ReqNo=b.ReqNo  and a.ReqNo='" & ReqNo & "' and d.Formno=b.FormNo " & SrchCond
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
