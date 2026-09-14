Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class ApprovePinReqs1
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("Status") = "OK" Then
                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        txtMemId.Text = Request.QueryString("key")
                        ChkMem.Checked = True
                        BindData(" AND b.IDNo='" & Request.QueryString("key") & "'")
                    End If
                Else
                    BindData()
                End If
            End If
        End If
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")

        If RbtPymode.SelectedValue = "B" Then
            Condition = Condition & " AND a.BankAmt>0 AND a.OtherAmt=0"
        ElseIf RbtPymode.SelectedValue = "O" Then
            Condition = Condition & " AND a.BankAmt=0 AND a.OtherAmt>0"
        ElseIf RbtPymode.SelectedValue = "W" Then
            Condition = Condition & " AND a.WalletAmt>0"
        ElseIf RbtPymode.SelectedValue = "A" Then
            'Condition = Condition & " AND a.BankAmt>0 AND a.OtherAmt>0"
        End If
        If ChkMem.Checked = True And Trim(TxtMemID.Text) <> "" Then
            Condition = Condition & " AND b.IdNo='" & RTrim(TxtMemID.Text) & "'"
        End If
        If RbReqStatus.SelectedValue <> "N" Then
            Condition = Condition & " And a.Status='" & RbReqStatus.SelectedValue & "'"

        End If
        'If RbReqStatus.SelectedValue = "P" Then
        '    btnApproove.Visible = True : BtnReject.Visible = True
        'ElseIf RbReqStatus.SelectedValue = "A" Then
        '    btnApproove.Visible = False : BtnReject.Visible = True
        'ElseIf RbReqStatus.SelectedValue = "R" Then
        '    btnApproove.Visible = True : BtnReject.Visible = False
        'End If

        If txtStartDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,a.RectimeStamp,106) as DateTime)>='" & txtStartDate.Text & "'"

        End If
        If txtEndDate.Text <> "" Then

            Condition = Condition & " And  Cast(Convert(Varchar,a.RecTimeStamp,106) as DateTime)<='" & txtEndDate.Text & "'"

        End If

        Dim sql As String = " Select Cast(a.FormNo as varchar) as FormNo,a.ReqNo,b.IDNo,RTRIM(b.MemFirstName +' ' + b.MemLastName)" & _
                                 " as MemName, a.TotalQty,a.WalletAmt,a.BankAmt,a.OtherAmt,a.TotalAmount, a.RejectRemark  " & _
                                "  as Remarks,Case when a.Status='R' then 'False' else 'True'" & _
                                " end as Reject,Case when a.Status='P' then 'True' else 'False' end as Approve, Case When a.Status='C' then 'Approve'  when a.Status='P' " & _
                                " then 'Pending' when a.Status='R' then  'Rejected'  end as status ," & _
                                " b.Mobl as MobileNo, Replace(convert(Varchar,a.RecTimeStamp,106),' ','-')as ReqDate," & _
                                " a.paymentmode,a.BankName,a.BranchName,a.transno,'" & Session("CompWeb") & "/images/UploadImage/'+ImgPath as ImagePath,Replace(convert(Varchar,a.Dddate,106),' ','-') as DDDate  From TrnPinReqMain a," & _
                                " M_MemberMaster b Where a.FormNo=b.FormNo " & Condition & " Order by a.RectimeStamp Desc;"
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GPinData") = dtData
        If dtData.Rows.Count > 0 Then
            'BtnReject.Visible = True
            'btnApproove.Visible = True
        Else
            'BtnReject.Visible = False
            'btnApproove.Visible = False
        End If
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GPinData")
        GvData.DataBind()
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""

        'If ChkMem.Checked = True Then
        '    Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
        'End If
        BindData(Condition)
    End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
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
    'Protected Sub SendPin()
    '    Try
    '        Dim msg As String = ""
    '        Dim updateEffect As Integer
    '        Dim scrname, IDNo As String
    '        Dim ReqNo, KitID, FormNo, Qty, stkQty As Integer
    '        Dim Chk As CheckBox
    '        Dim lbl As Label
    '        Dim lblReqno As Label
    '        Dim lblIdno As Label
    '        Dim i As Integer = 0
    '        'Dim GVRw As GridViewRow
    '        Dim dt1 As DataTable
    '        dt1 = New DataTable

    '        For Each Gvrw As GridViewRow In GvData.Rows
    '            'GVRw = CType(sender.Parent.Parent, GridViewRow)
    '            'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
    '            Chk = DirectCast(Gvrw.FindControl("chkSelect"), CheckBox)
    '            FormNo = DirectCast(Gvrw.FindControl("LblGrpID"), Label).Text
    '            lblReqno = DirectCast(Gvrw.FindControl("LblReqNo"), Label)
    '            IDNo = DirectCast(Gvrw.FindControl("LblIdNo"), Label).Text
    '            ReqNo = DirectCast(Gvrw.FindControl("LblReqNo"), Label).Text
    '            'KitID = DirectCast(Gvrw.FindControl("LblKitID"), Label).Text
    '            ' Qty = DirectCast(Gvrw.FindControl("TxtPinQty"), TextBox).Text
    '            ' FormNo = DirectCast(Gvrw.FindControl("LblFormNo"), Label).Text
    '            'IDNo = DirectCast(Gvrw.FindControl("LblIdNo"), Label).Text
    '            If Chk.Checked Then
    '                Dim sqlqry As String = "Select a.ReqNo,a.FormNo,b.IdNo,a.KitID,a.KitName,b.MemFirstName+' '+ b.MemLastName as MemName,a.Qty,a.RemainQty ,a.DispQty,CASE WHEN a.Status='P' THEN 'Pending' ELSE 'Clear' END AS Status,a.KitAmount,a.MobileNo FROM TrnPinRequest a,M_MemberMaster b Where  a.FormNo=b.FormNo  and a.ReqNo='" & ReqNo & "'"
    '                dtData = New DataTable
    '                dtData = objDAL.GetData(sqlqry)
    '                If dtData.Rows.Count > 0 Then


    '                    For j = 0 To dtData.Rows.Count - 1
    '                        KitID = dtData.Rows(j)("KitId")
    '                        Qty = dtData.Rows(j)("Qty")

    '                        Dim str As String = "Select Count(*) as StkQty FROM M_FormGeneration WHERE GeneratedBy<>'Y' AND LastModified<>'Y' AND ProdId='" & Val(KitID) & "'"
    '                        objDAL = New DAL
    '                        dt1 = objDAL.GetData(str)
    '                        If dt1.Rows.Count > 0 Then
    '                            stkQty = dt1.Rows(0)("StkQty")

    '                        End If
    '                        If stkQty >= Qty Then
    '                            msg = CheckProdStockAvail(Val(KitID), Val(Qty))
    '                            If msg <> "" Then
    '                                If msg.Contains("Please attach Products with Kit.") = True Then
    '                                    'lblError.Text = msg
    '                                    scrname = "<SCRIPT language='javascript'>alert('" & msg & "');" & "</SCRIPT>"
    '                                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Availability!!", scrname, False)
    '                                    Exit For
    '                                Else
    '                                    'blError.Text = "Available Stock for Product: " & msg & " is not Sufficient."
    '                                    scrname = "<SCRIPT language='javascript'>alert(' Available Stock for Product: " & msg & " is not Sufficient.');" & "</SCRIPT>"
    '                                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Availability!!", scrname, False)
    '                                    Exit For
    '                                End If
    '                            End If

    '                            Dim Remark As String = ""
    '                            Remark = " Pin Issued To ReqNo=" & ReqNo & " On Id=" & IDNo & ""
    '                            ' "Exec Generate_EPins " & KitID & "," & Qty & "," & Session("UserID") & ";" & _
    '                            Dim Sql As String = " Update TrnPinReqMain SET Status='A', IsApprove = 'Y',ApproveDate=GEtdate(),ApprovedBy='" & Val(Session("UserID")) & "' where FormNo='" & FormNo & "' And ReqNo='" & ReqNo & "' " & _
    '                             "Exec  Sp_IssueEpins '" & IDNo & "'," & KitID & "," & Qty & ",' Against Req No." & ReqNo & "'," & Session("UserID") & ";" & _
    '                            "UPDATE TrnPinRequest SET DispQty=DispQty+" & Val(Qty) & ", DepositAmount=KitAmount * '" & Qty & "' WHERE ReqNO='" & ReqNo & "' and KitId='" & KitID & "' ;" & _
    '                            "UPDATE TrnPinRequest SET Status=CASE WHEN DispQty>=Qty THEN 'C' ELSE 'P' END WHERE ReqNO='" & ReqNo & "' and kitId='" & KitID & "' ;" & _
    '                                        "UPDATE TrnPinReqMain Set Status='S',SentQty=SentQty+" & Val(Qty) & " WHERE ReqNO='" & ReqNo & "' ;" & _
    '                             "UPDATE TrnPinReqMain Set Status=Case when SentQty=TotalQty then 'C' Else 'S' End   WHERE ReqNO='" & ReqNo & "' ;" & _
    '                                        "INSERT INTO TrnPinDispatch (ReqNo,FormNo,Idno,KitId,KitName,Qty,DispQty," & _
    '                    " Status,PayMode,TransNo,Amount,AccountNo,BankName," & _
    '                    " BranchName,RDate,RecTimeStamp,KitAmount,DepositAmount," & _
    '                    " DispatchDate,UserId,Remarks)" & _
    '                    " SELECT ReqNo,FormNo,Idno,KitId,KitName,'" & Qty & "',DispQty," & _
    '                    " Status,'','0',KitAmount * " & Qty & ",'0',''," & _
    '                    " '',RDate,Getdate(),KitAmount,DepositAmount,Getdate(),'" & Session("UserID") & "','Pin Issued on Req.' FROM TrnPinRequest WHERE ReqNo='" & ReqNo & "' and KitId='" & KitID & "'; " & _
    '                            " Update M_FormGeneration Set ReqNo=" & ReqNo & " where TransNo In (Select Top " & Qty & " TransNo From M_FormGeneration where IssuedIdno='" & IDNo & "' and ProdId='" & KitID & "' order by TransNo Desc  )" & _
    '                         " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
    '                         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Sent Pin ','Pin Issued','" & Remark & "',Getdate(),'" & FormNo & "')"


    '                            updateEffect = objDAL.UpdateData(Sql)
    '                            i = i + 1



    '                        Else
    '                            scrname = "<SCRIPT language='javascript'>alert('Stock Not Availble  !');" & "</SCRIPT>"
    '                            Exit For
    '                        End If
    '                    Next
    '                End If
    '                If updateEffect <> 0 Then
    '                    scrname = "<SCRIPT language='javascript'>alert('" & i & " Request to Pin Sent Successfully!');" & "</SCRIPT>"
    '                Else
    '                    scrname = "<SCRIPT language='javascript'>alert('Sorry! Pin couldn't be sent! ');" & "</SCRIPT>"
    '                End If
    '                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Pin Send", scrname, False)
    '                BindData()
    '                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
    '                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    '            End If
    '        Next
    '    Catch ex As Exception

    '    End Try
    'End Sub
    'Protected Sub Forward(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim FormNo, scrname As String
    '    Dim IDNo As String
    '    Dim GVRw As GridViewRow
    '    'Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '    'Conn.Open()
    '    GVRw = CType(sender.Parent.Parent, GridViewRow)
    '    IDNo = DirectCast(GVRw.FindControl("hdnReqNo"), HiddenField).Value

    '    If IDNo <> "0" Then
    '        Dim Sql As String = "Insert into TrnApprovePinRequest(ReqNo,CreatedBy) Values('" & IDNo & "','" & Session("UserID") & "')"
    '        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
    '        If updateEffect <> 0 Then
    '            scrname = "<SCRIPT language='javascript'>alert('Forward successfuly!');" & "</SCRIPT>"
    '        Else
    '            scrname = "<SCRIPT language='javascript'>alert('not able to send successfuly! ');" & "</SCRIPT>"
    '        End If
    '        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "ForwardEpin", scrname, False)
    '        BindData()
    '    End If
    'End Sub

    'Private Sub ForwardAction(ByVal AprvType As String, ByVal ApprvStatus As String)
    '    Dim sql As String = ""
    '    Dim Chk As CheckBox
    '    Dim lbl As Label
    '    Dim lblReqno As Label
    '    Dim lblIdno As Label
    '    Dim Cnt As Integer = 0
    '    Dim Remark As String = ""
    '    For Each Gvr As GridViewRow In GvData.Rows
    '        Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
    '        lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
    '        lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
    '        lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
    '        Remark = " Forward ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
    '        If Chk.Checked = True Then

    '            'sql = sql & ";Update TrnPinReqMain SET Status='" & ApprvStatus & "', IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApprovedBy='" & Val(Session("UserID")) & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
    '            sql = sql & "; insert into TrnApprovePinRequest(ReqNo,CreatedBy)Values " & _
    '     "('" & lblReqno.Text & "','" & Session("UserName") & "')"
    '            sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,memberId)Values" & _
    '     "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Forward Request ','Forward Request','" & Remark & "',Getdate(),'" & Val(lbl.Text) & "')"

    '            Cnt = Cnt + 1
    '        End If
    '    Next
    '    Dim a As Integer = objDAL.UpdateData(sql)
    '    Dim MsgTxt As String = ""
    '    If AprvType = "Y" Then
    '        MsgTxt = "Forwarded"
    '    Else : MsgTxt = "Rejected"
    '    End If
    '    If a <> 0 And Cnt > 0 Then
    '        lblMsg.Text = "" & Cnt & " Requests " & MsgTxt & " Successfully."
    '        lblMsg.Visible = True
    '        lblMsg.ForeColor = Drawing.Color.Green
    '        BindData()
    '    Else
    '        lblMsg.Text = "Not " & MsgTxt
    '        lblMsg.Visible = True
    '        lblMsg.ForeColor = Drawing.Color.Red
    '    End If
    'End Sub
    Private Sub AprvAction(ByVal AprvType As String, ByVal ApprvStatus As String)
        Dim sql As String = ""
        Dim Chk As CheckBox
        Dim lbl As Label
        Dim lblReqno As Label
        Dim lblIdno As Label
        Dim Cnt As Integer = 0
        Dim Remark As String = ""
        For Each Gvr As GridViewRow In GvData.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            lbl = DirectCast(Gvr.FindControl("LblGrpID"), Label)
            lblReqno = DirectCast(Gvr.FindControl("LblReqNo"), Label)
            lblIdno = DirectCast(Gvr.FindControl("LblIdNo"), Label)
            Remark = " Approve Pin On ReqNo:" & lblReqno.Text & " for Idno:" & lblIdno.Text & ""
            If Chk.Checked = True Then

                sql = sql & ";Update TrnPinReqMain SET Status='" & ApprvStatus & "', IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApprovedBy='" & Val(Session("UserID")) & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
                sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,memberId)Values" & _
         "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Approve Pin ','Approve Pin','" & Remark & "',Getdate(),'" & Val(lbl.Text) & "')"

                Cnt = Cnt + 1
            End If
        Next
        Dim a As Integer = objDAL.UpdateData(sql)
        Dim MsgTxt As String = ""
        If AprvType = "Y" Then
            MsgTxt = "Approved"
        Else : MsgTxt = "Rejected"
        End If
        If a <> 0 And Cnt > 0 Then
            lblMsg.Text = "" & Cnt & " Requests " & MsgTxt & " Successfully."
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Green
            BindData()
        Else
            lblMsg.Text = "Not " & MsgTxt
            lblMsg.Visible = True
            lblMsg.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Protected Sub ChkMem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkMem.CheckedChanged
        TxtMemID.Enabled = ChkMem.Checked
    End Sub

    Protected Sub btnApproove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApproove.Click
        AprvAction("Y", "A")
        'SendPin()
    End Sub

    Protected Sub BtnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click
        AprvAction("R", "R")
    End Sub

End Class
