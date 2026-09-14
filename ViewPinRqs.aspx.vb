Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class ViewPinRqs
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
       

            Dim sql As String = "Select a.ReqNo,a.FormNo,b.IdNo,a.KitID,a.KitName,b.MemFirstName+' '+ b.MemLastName as MemName,a.Qty,a.RemainQty ,Case when a.RemainQty<=0 then 'False' else 'True' end As SentStatus,a.DispQty,CASE WHEN a.Status='P' THEN 'Pending' ELSE 'Clear' END AS Status,a.KitAmount,b.Mobl as MobileNo FROM TrnPinRequest a,M_MemberMaster b Where  a.FormNo=b.FormNo  and a.ReqNo='" & ReqNo & "'" & SrchCond
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
    

    Private Function CheckProdStockAvail(ByVal KitID As Integer, ByVal Qty As Integer) As String
        Dim Msg As String = ""
        Dim q As String = ""
        q = "Select a.*,b.ProductName,CASE WHEN a.AvailQty<ReqQty THEN b.ProductName ELSE '' END as NAProd FROM" & _
" (Select ISNULL(FCode,'" & Session("WRPartyCode") & "') as FCode,b.Barcode,b.ProdID,ISNULL(SUM(a.Qty),0) as AvailQty," & Qty & "*  b.Qty as ReqQty " & _
" FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..Im_CurrentStock a RIGHT JOIN M_KitProductDetail b ON a.ProdId=b.ProdId AND a.Barcode=b.Barcode AND FCode ='" & Session("WRPartyCode") & "'" & _
" WHERE   b.KitId='" & KitID & "' And b.ActiveStatus='Y' and b.RowStatus='Y'" & _
" GROUP BY ISNULL(FCode,'" & Session("WRPartyCode") & "') ,b.Barcode,b.ProdID, b.Qty) as a," & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as b" & _
" WHERE a.ProdID=b.ProdID"
        Dim Dt_ As New DataTable
        Dt_ = objDAL.GetData(q)
        If Dt_.Rows.Count = 0 And KitID <> 1 Then
            Msg = "Please attach Products with Kit."
        Else
            For i As Integer = 0 To Dt_.Rows.Count - 1
                If Dt_.Rows(i)("NAProd").ToString <> "" Then
                    Msg = Msg & Dt_.Rows(i)("NAProd").ToString & ", "
                End If
            Next
            If Msg <> "" Then
                Msg = Msg.Substring(0, Len(Msg) - 2)
            End If
        End If
        Return Msg
    End Function
    Protected Sub SendPin(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim Sql As String = ""
            Dim msg As String = ""
            Dim updateEffect As Integer
            Dim scrname, IDNo As String
            Dim ReqNo, KitID, FormNo, Qty, stkQty As Integer
            'Dim GVRw As GridViewRow
            Dim dt1 As DataTable
            dt1 = New DataTable
            For Each Gvrw As GridViewRow In GvData.Rows
                'GVRw = CType(sender.Parent.Parent, GridViewRow)
                'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
                ReqNo = DirectCast(Gvrw.FindControl("LblReqNo"), Label).Text
                KitID = DirectCast(Gvrw.FindControl("LblKitID"), Label).Text
                Qty = DirectCast(Gvrw.FindControl("TxtPinQty"), TextBox).Text
                FormNo = DirectCast(Gvrw.FindControl("LblFormNo"), Label).Text
                IDNo = DirectCast(Gvrw.FindControl("LblIdNo"), Label).Text

                Sql = ""

                Dim Remark As String = ""
                Remark = " Pin Issued To ReqNo=" & ReqNo & " On Id=" & IDNo & ""
                Sql = " Exec Generate_EPins " & KitID & "," & Qty & "," & Session("UserID") & ";"
                Sql = Sql & " Update TrnPinReqMain SET Status='A', IsApprove = 'Y',ApproveDate=GEtdate(),ApprovedBy='" & Val(Session("UserID")) & "' where FormNo='" & FormNo & "' And ReqNo='" & ReqNo & "'; "
                Sql = Sql & "Exec  Sp_IssueEpins '" & IDNo & "'," & KitID & "," & Val(Qty) & ",' Against Req No." & ReqNo & "','" & Val(Session("UserID")) & "';"
                Sql = Sql & "UPDATE TrnPinRequest SET DispQty=DispQty+" & Val(Qty) & ", DepositAmount=KitAmount * '" & Qty & "' WHERE ReqNO='" & ReqNo & "' and KitId='" & KitID & "' ;"
                Sql = Sql & "UPDATE TrnPinRequest SET Status=CASE WHEN DispQty>=Qty THEN 'C' ELSE 'P' END WHERE ReqNO='" & ReqNo & "' and kitId='" & KitID & "' ;"
                Sql = Sql & "UPDATE TrnPinReqMain Set Status='S',SentQty=SentQty+" & Val(Qty) & " WHERE ReqNO='" & ReqNo & "' ;"
                Sql = Sql & "UPDATE TrnPinReqMain Set Status=Case when SentQty=TotalQty then 'C' Else 'S' End   WHERE ReqNO='" & ReqNo & "' ;"
                Sql = Sql & "INSERT INTO TrnPinDispatch (ReqNo,FormNo,Idno,KitId,KitName,Qty,DispQty," & _
         " Status,PayMode,TransNo,Amount,AccountNo,BankName," & _
         " BranchName,RDate,RecTimeStamp,KitAmount,DepositAmount," & _
         " DispatchDate,UserId,Remarks)" & _
         " SELECT ReqNo,FormNo,Idno,KitId,KitName,'" & Qty & "',DispQty," & _
         " Status,'','0',KitAmount * " & Qty & ",'0',''," & _
         " '',RDate,Getdate(),KitAmount,DepositAmount,Getdate(),'" & Session("UserID") & "','Pin Issued on Req.' FROM TrnPinRequest WHERE ReqNo='" & ReqNo & "' and KitId='" & KitID & "'; "
                Sql = Sql & " Update M_FormGeneration Set ReqNo=" & ReqNo & " where TransNo In (Select Top " & Qty & " TransNo From M_FormGeneration where IssuedIdno='" & IDNo & "' and ProdId='" & KitID & "' order by TransNo Desc  )"
                Sql = Sql & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                   "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Sent Pin ','Pin Issued','" & Remark & "',Getdate(),'" & FormNo & "');"
                '"Exec  " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & " ..GenBill " & KitID & ", " & Val(Qty) & ", '" & Session("WRPartyCode") & "','" & IDNo & "','Issue Epin'"


                updateEffect = objDAL.UpdateData(Sql)


                If updateEffect <> 0 Then
                    scrname = "<SCRIPT language='javascript'>alert('Pin Sent Successfully!');" & "</SCRIPT>"
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Sorry! Pin couldn't be sent! ');" & "</SCRIPT>"
                End If



                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Pin Send", scrname, False)
                BindData()
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)


            Next
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub TxtPinQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ReqQty As String
        Dim SentQty As String
        Dim qty As String
        Dim aqty As String
        Dim scrname As String
        For Each Gvr As GridViewRow In GvData.Rows
            ReqQty = DirectCast(Gvr.FindControl("LblQty"), Label).Text
            SentQty = DirectCast(Gvr.FindControl("LblDispQty"), Label).Text
            aqty = DirectCast(Gvr.FindControl("TxtPinQty"), TextBox).Text
            qty = ReqQty - SentQty
            If aqty > qty Then
                DirectCast(Gvr.FindControl("TxtPinQty"), TextBox).Text = qty

                scrname = "<SCRIPT language='javascript'>alert('Sent Pin can not be greater then Requested Pin !');" & "</SCRIPT>"
            
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Pin Send", scrname, False)
            End If



        Next

    End Sub

 
End Class
