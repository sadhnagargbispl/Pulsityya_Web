Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewPinDetail
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
        If String.IsNullOrEmpty(Request("IdNo")) = False Then
            ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("IdNo")))

        End If
        If Not Page.IsPostBack Then

            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("IdNo")) = False Then
                    'LblNo.Text = " Request No :" & ReqNo
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
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim cond As String = ""
            'If Request("Type") = 2 Then
            '    cond = "and a.Fcode='" & Request("Idno") & "'"
            'Else

            '    cond = "and a.IssuedIdno='" & Request("Idno") & "'"
            'End If
            If Not Request("Type") Is Nothing Then
                cond = " and a.ChallanNo='" & Request("Type") & "'"

            End If
            ' Dim sql As String = "select Formno,ReqNo,Case when Status='R' then RejectRemark else Remarks end as Remarks from TrnPinReqMain where ReqNo='" & ReqNo & "'" & SrchCond
            Dim sql As String = "Select c.IdNo as UsedId,A.UsedBy,Replace(Convert(Varchar,a.UsedDate,106),' ','-') + ' '+  CONVERT(varchar(15),CAST(a.UsedDate AS TIME),100)as UsedDate1, " & _
                   " A.ChallanNo,A.IssuedIDNo as FCode,RTRIM(LTRIM(M.Prefix))+' '+ M.MemFirstName+ ' '+ M.MemLastName as MemName,A.FormNo," & _
                   " A.ScratchNo,CASE WHEN A.UsedBy<='0' THEN 'Y' ELSE 'N' END AS IsCancel,CASE WHEN A.UsedBy<='0' THEN 'Cancel' ELSE '' END AS PinCancel," & _
                   " Replace(Convert(varchar,A.IssuedDate,106),' ','-')+ ' '+  CONVERT(varchar(15),CAST(A.IssuedDate AS TIME),100) As IssueDt,B.KitName,CASE WHEN A.UsedBy='0' THEN 'Cancel' ELSE '' END as CancelOption," & _
                   " Case when a.IsIssued='Y' Then  'Used' else 'UnUsed' end as Status From " & _
                               " M_KitMaster As B,M_MemberMaster as M with(nolock),M_FormGeneration As A with(nolock) Left Join M_MemberMaster as C with(nolock)  On A.UsedBy=c.FormNo " & _
                               " Where A.ProdID=B.KitID AND A.IssuedIDNo =M.IDNo AND A.GeneratedBY='Y'  AND A.LastModified='Y' And a.ProdId='" & Request("ProdId") & "' " & cond & " and " & _
                               "Replace(Convert(varchar,a.IssuedDate,106),' ','-') + ' '+ CONVERT(varchar(15),CAST(a.IssuedDate AS TIME),100)='" & Request("IssuedDate") & "' " & SrchCond & "  Order by A.IssuedDate Desc,A.FormNo"

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
    Protected Sub CancelIssue(ByVal sender As Object, ByVal e As System.EventArgs)
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim GrpID, scrname As String
        Dim FormNo, ScratchNo As String
        Dim GVRw As GridViewRow
        'Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Conn.Open()
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        FormNo = DirectCast(GVRw.FindControl("LblFormNo"), Label).Text
        ScratchNo = DirectCast(GVRw.FindControl("LblScratchNo"), Label).Text
        If DirectCast(GVRw.FindControl("LblIsCancel"), Label).Text = "Y" Then
            Dim Sql As String = "Exec [Sp_CancelEpins] '" & FormNo & "','" & ScratchNo & "','" & Session("UserID") & "'"
            Dim updateEffect As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql))
            If updateEffect > 0 Then
                BindData()
                scrname = "<SCRIPT language='javascript'>alert('Pin Deleted Successfully!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "E-Pin Deletion", scrname, False)
            Else
                scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected pin! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "E-Pin Deletion", scrname, False)
            End If
            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "E-Pin Deletion", scrname, False)

        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub


    Protected Sub rbtnStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnStatus.SelectedIndexChanged
        Dim Condition As String = ""
        If rbtnStatus.SelectedValue = "Used" Then
            Condition = " And A.isIssued='Y'"
        ElseIf rbtnStatus.SelectedValue = "UnUsed" Then
            Condition = " And A.isIssued='N'"
        End If
        BindData(Condition)
    End Sub
End Class
