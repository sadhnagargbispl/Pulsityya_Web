Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewAward
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
        'If String.IsNullOrEmpty(Request("IdNo")) = False Then
        '    ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("IdNo")))

        'End If
        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                '  If String.IsNullOrEmpty(Request("IdNo")) = False Then
                'LblNo.Text = " Request No :" & ReqNo
                BindData()
                'End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
    End Sub

    Public Sub BindData(Optional ByVal SrchCond As String = "")
        Try
            Dim cond As String = ""
          
            ' Dim sql As String = "select Formno,ReqNo,Case when Status='R' then RejectRemark else Remarks end as Remarks from TrnPinReqMain where ReqNo='" & ReqNo & "'" & SrchCond
            Dim sql As String = " select c.Idno,(C.MemFirstName+' '+c.MemLastName) as MemberName, b.FormnoDwn,RewardId,a.Sessid,PwrLeg,WkrLeg from M_AwardFinal as a,M_MemTreeRelation as b,M_Membermaster as c where " & _
                                 " a.Formno=b.FormnoDwn and b.FormNo='" & Request("Formno") & "' and b.LegNo='" & Request("LegNo") & "' and RewardId='" & (Val(Request("RewardId")) - 1) & "' and a.Sessid<='" & Request("Sessid") & "' and b.FormNoDwn=c.Formno"

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
