Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class ApprovePinReqs
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim dbconnect As cls_DataAccess
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Epin / Approve Epin Request"
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dbconnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                If Session("AStatus") = "OK" Then
                    txtStartDate.Text = Format(dbconnect.Get_ServerDate(), "dd-MMM-yyyy")
                    txtEndDate.Text = Format(dbconnect.Get_ServerDate(), "dd-MMM-yyyy")
                    If Request.QueryString.HasKeys Then
                        If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                            TxtMemID.Text = Request.QueryString("key")
                            ChkMem.Checked = True
                            BindData(" AND b.IDNo='" & Request.QueryString("key") & "'")
                        End If
                    Else
                        BindData()
                    End If
                End If
            End If

        Catch ex As Exception
            dbconnect.closeConnection()
        End Try
    End Sub

    Public Sub BindData(Optional ByVal Condition As String = "")
        Try


            If ChkMem.Checked = True And Trim(TxtMemID.Text) <> "" Then
                Condition = Condition & " AND b.IdNo='" & Trim(TxtMemID.Text) & "'"
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
            Session("GData") = dtData
            ViewState("ReqDate") = "ReqDate"
            ViewState("Sort_Order") = "DESC"
            If dtData.Rows.Count > 0 Then
                'BtnReject.Visible = True
                'btnApproove.Visible = True
            Else
                'BtnReject.Visible = False
                'btnApproove.Visible = False
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        If e.SortExpression = ViewState("ReqDate").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        'Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/Upaarrow.png", "~/Images/DownArrow.png")
                        'tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        'tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = e.SortExpression Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        'Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/Upaarrow.png", "~/Images/DownArrow.png")
                        'tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        'tableCell.Controls.Add(img)
                    End If
                Next
            End If

        Else
            RebindData(e.SortExpression, "ASC")
        End If
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("GData"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("ReqDate") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub




    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""

        'If ChkMem.Checked = True Then
        '    Condition = Condition & " AND IDNo='" & Trim(txtMemId.Text) & "'"
        'End If
        BindData(Condition)
    End Sub
    'Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
    '    If e.Row.RowType = DataControlRowType.Header Then
    '        DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
    '    End If
    'End Sub
    Private Sub AprvAction(ByVal AprvType As String, ByVal ApprvStatus As String)
        Try


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
                    If ApprvStatus = "A" Then
                        Dim str As String = " select * from TrnPinRequest where  ReqNo='" & lblReqno.Text.Trim & "'"
                        dtData = New DataTable
                        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                        If dtData.Rows.Count > 0 Then
                            For i As Integer = 0 To dtData.Rows.Count - 1
                                sql = " Update TrnPinReqMain SET Status='A', IsApprove = 'Y',ApproveDate=GEtdate(),ApprovedBy='" & Val(Session("UserID")) & "' where FormNo='" & Val(dtData.Rows(i)("FormNo")) & "' And ReqNo='" & Val(dtData.Rows(i)("ReqNo")) & "' " & _
                            "Exec  Sp_IssueEpins '" & dtData.Rows(i)("IdNo") & "'," & Val(dtData.Rows(i)("KitId")) & "," & Val(dtData.Rows(i)("qty")) & ",' Against Req No." & Val(dtData.Rows(i)("ReqNo")) & "'," & Session("UserID") & ";" & _
                           "UPDATE TrnPinRequest SET DispQty=DispQty+" & Val(dtData.Rows(i)("qty")) & ", DepositAmount=KitAmount * '" & Val(dtData.Rows(i)("qty")) & "' WHERE ReqNO='" & Val(dtData.Rows(i)("ReqNo")) & "' and KitId='" & Val(dtData.Rows(i)("KitId")) & "' ;" & _
                           "UPDATE TrnPinRequest SET Status=CASE WHEN DispQty>=Qty THEN 'C' ELSE 'P' END WHERE ReqNO='" & Val(dtData.Rows(i)("ReqNo")) & "' and kitId='" & Val(dtData.Rows(i)("KitId")) & "' ;" & _
                                       "UPDATE TrnPinReqMain Set Status='S',SentQty=SentQty+" & Val(dtData.Rows(i)("qty")) & " WHERE ReqNO='" & Val(dtData.Rows(i)("ReqNo")) & "' ;" & _
                            "UPDATE TrnPinReqMain Set Status=Case when SentQty=TotalQty then 'C' Else 'S' End   WHERE ReqNO='" & Val(dtData.Rows(i)("ReqNo")) & "' ;" & _
                                       "INSERT INTO TrnPinDispatch (ReqNo,FormNo,Idno,KitId,KitName,Qty,DispQty," & _
                   " Status,PayMode,TransNo,Amount,AccountNo,BankName," & _
                   " BranchName,RDate,RecTimeStamp,KitAmount,DepositAmount," & _
                   " DispatchDate,UserId,Remarks)" & _
                   " SELECT ReqNo,FormNo,Idno,KitId,KitName,'" & Val(dtData.Rows(i)("qty")) & "',DispQty," & _
                   " Status,'','0',KitAmount * " & Val(dtData.Rows(i)("qty")) & ",'0',''," & _
                   " '',RDate,Getdate(),KitAmount,DepositAmount,Getdate(),'" & Session("UserID") & "','Pin Issued on Req.' FROM TrnPinRequest WHERE ReqNo='" & Val(dtData.Rows(i)("ReqNo")) & "' and KitId='" & Val(dtData.Rows(i)("KitId")) & "'; " & _
                           " Update M_FormGeneration Set ReqNo=" & Val(dtData.Rows(i)("ReqNo")) & " where TransNo In (Select Top " & Val(dtData.Rows(i)("qty")) & " TransNo From M_FormGeneration where IssuedIdno='" & dtData.Rows(i)("Idno") & "' and ProdId='" & Val(dtData.Rows(i)("KitId")) & "' order by TransNo Desc  )" & _
                        " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
                        "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Sent Pin ','Pin Issued','" & Remark & "',Getdate(),'" & Val(dtData.Rows(i)("Formno")) & "')"

                            Next
                        End If

                    Else
                        sql = sql & ";Update TrnPinReqMain SET Status='" & ApprvStatus & "', IsApprove = '" & AprvType & "',ApproveDate=GEtdate(),ApprovedBy='" & Val(Session("UserID")) & "' where FormNo='" & lbl.Text & "' And ReqNo='" & lblReqno.Text & "'"
                        sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,memberId)Values" & _
                 "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Approve Pin ','Approve Pin','" & Remark & "',Getdate(),'" & Val(lbl.Text) & "')"

                    End If

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
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ChkMem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkMem.CheckedChanged
        TxtMemID.Enabled = ChkMem.Checked
    End Sub

    Protected Sub btnApproove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApproove.Click
        AprvAction("A", "A")
    End Sub

    Protected Sub BtnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReject.Click
        AprvAction("R", "R")
    End Sub

End Class
