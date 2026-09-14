Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class PinRejectRemark
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
            ' ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("ReqNo")))
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


            'Dim sql As String = "select Formno,ReqNo,Remarks from TrnPinReqMain where ReqNo='" & ReqNo & "'" & SrchCond
            Dim sql As String = "Select Cast(a.FormNo as varchar) as FormNo, a.ReqNo,b.IdNo,a.KitID,a.KitName,b.MemFirstName+' '+ b.MemLastName as MemName,a.Qty,a.RemainQty ,a.DispQty,CASE WHEN a.Status='P' THEN 'Pending' ELSE 'Clear' END AS Status,a.KitAmount,b.Mobl as MobileNo, a.Remarks  From TrnPinRequest as  a,M_MemberMaster b Where a.FormNo=b.FormNo and ReqNo='" & ReqNo & "' "

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


    Protected Sub BtnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click
        Dim sql As String = ""
        Dim a As Integer
        Dim scrname As String = ""
        Dim LblFormno As New Label

        Dim LblAmount As New Label
        For Each Gvr As GridViewRow In GvData.Rows
            LblFormno = DirectCast(Gvr.FindControl("lblFormno"), Label)
            LblAmount = DirectCast(Gvr.FindControl("LblTotalAmt"), Label)
        Next
        Dim Remark As String = ""
        Remark = " ReqNo:" & ReqNo & " Rejected by " & Session("UserName") & ""
        sql = " Update TrnPinReqMain SET Status='R', IsApprove = 'N',ApproveDate=GEtdate(),ApprovedBy='" & Val(Session("UserID")) & "',RejectRemark='" & txtRemark.Text & "' where ReqNo='" & ReqNo & "'"
        sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
       "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Reject Pin ','Reject Pin','" & Remark & "',Getdate(),'" & LblFormno.Text & "')"

       
        a = objDAL.UpdateData(sql)
        If a > 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Pin Rejected Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Sorry! Pin couldn't be rejected! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Pin Rejected", scrname, False)
        BindData()
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)


    End Sub
End Class
