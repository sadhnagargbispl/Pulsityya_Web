Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Data.SqlClient
Imports System.Globalization

Partial Class AddofferNew
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Dim Sql As String = ""
    Dim OfferIDQS As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If String.IsNullOrEmpty(Request("OfferID")) = False Then
            OfferIDQS = Request("OfferID")
        End If
        If Not Page.IsPostBack Then
            Fill_Kit()
            OfferForValiDate()
            Fill_Lvel()
            If Session("AStatus") = "OK" Then
                'If Session("compid") = "1010" Or Session("compid") = "1091" Then
                If Session("compid") = "1010" Or Session("compid") = "1103" Or Session("compid") = "1108" Then
                    If String.IsNullOrEmpty(Request("OfferID")) = False Then
                        CalendarExtender2.StartDate = DateTime.Today
                        BtnFundTransfer.Text = "Modify"
                        BindGridView()
                    End If
                End If
            Else
                Response.Redirect("logout.aspx")
            End If
        End If
    End Sub
    Private Sub Fill_Kit()
        Try
            Dim str As String = ""
            Dim dt As DataTable = New DataTable()
            'str = "Select KitID,KitNAme from M_KitMaster Where ActiveStatus =  'Y' And joinAmount > 0 Order by KitID "
            str = "Select RankId,Rank from MstRanks Where ActiveStatus =  'Y' Order by RankID "
            'dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables(0)
            dt = objDAL.GetData(str)
            If (dt.Rows.Count > 0) Then
                gv.DataSource = dt
                gv.DataBind()

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Fill_Lvel()
        Try
            Dim str As String = ""
            Dim dt As DataTable = New DataTable()
            If Session("compid") = "1010" Then
                str = " Select rewardid as RankId,Rank from M_rewardmaster Where ActiveStatus =  'Y'  AND rewardid<=10 Order by RankID "
                dt = objDAL.GetData(str)
                If (dt.Rows.Count > 0) Then
                    GridView4.DataSource = dt
                    GridView4.DataBind()
                    GridView5.DataSource = dt
                    GridView5.DataBind()
                End If
            ElseIf Session("compid") = "1108" Then
                str = " Select rewardid as RankId,Rank from M_rewardmaster Where ActiveStatus =  'Y'  AND rewardid<=10 Order by RankID "
                dt = objDAL.GetData(str)
                If (dt.Rows.Count > 0) Then
                    GridView4.DataSource = dt
                    GridView4.DataBind()
                    GridView5.DataSource = dt
                    GridView5.DataBind()
                End If
            ElseIf Session("compid") = "1103" Then
                str = " Select RankId,Rank from MstRanks Where ActiveStatus =  'Y'  Order by RankID "
                dt = objDAL.GetData(str)
                If (dt.Rows.Count > 0) Then
                    GridView6.DataSource = dt
                    GridView6.DataBind()
                    GridView5.DataSource = dt
                    GridView5.DataBind()
                End If
            Else
                str = "Select RankId,Rank from MstRanks Where ActiveStatus =  'Y' Order by RankID "
                dt = objDAL.GetData(str)
                If (dt.Rows.Count > 0) Then
                    GridView1.DataSource = dt
                    GridView1.DataBind()
                    GridView2.DataSource = dt
                    GridView2.DataBind()
                    GridView3.DataSource = dt
                    GridView3.DataBind()
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub rbtOfferFor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOfferFor.SelectedIndexChanged
        Try
            OfferForValiDate()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub OfferForValiDate()
        Try
            If Session("compid") = "1010" Then
                If (RadioButtonList1.SelectedValue = "T") Then
                    div5.Visible = True
                    div7.Visible = False
                    div1.Visible = False
                    div6.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                    div2.Visible = False
                ElseIf (RadioButtonList1.SelectedValue = "A") Then
                    div2.Visible = False
                    div6.Visible = True
                    div7.Visible = False
                    div5.Visible = False
                    div1.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                End If
            ElseIf Session("compid") = "1108" Then
                If (RadioButtonList1.SelectedValue = "T") Then
                    div5.Visible = True
                    div7.Visible = False
                    div1.Visible = False
                    div6.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                    div2.Visible = False
                ElseIf (RadioButtonList1.SelectedValue = "A") Then
                    div2.Visible = False
                    div6.Visible = True
                    div7.Visible = False
                    div5.Visible = False
                    div1.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                End If
            ElseIf Session("compid") = "1103" Then
                If (RadioButtonList1.SelectedValue = "T") Then
                    div5.Visible = False
                    div7.Visible = True
                    div1.Visible = False
                    div6.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                    div2.Visible = False
                ElseIf (RadioButtonList1.SelectedValue = "A") Then
                    div2.Visible = False
                    div7.Visible = False
                    div6.Visible = True
                    div5.Visible = False
                    div1.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                End If
            ElseIf Session("compid") = "1091" Then
                If (rbtOfferFor.SelectedValue = "P") Then
                    div5.Visible = False
                    div7.Visible = False
                    div6.Visible = False
                    divPackage.Visible = True
                    divAll.Visible = False
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                    div1.Visible = False
                    div2.Visible = False
                ElseIf (rbtOfferFor.SelectedValue = "T") Then
                    div5.Visible = False
                    div6.Visible = False
                    div1.Visible = True
                    div7.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                    div2.Visible = False
                ElseIf (rbtOfferFor.SelectedValue = "A") Then
                    div5.Visible = False
                    div6.Visible = False
                    div2.Visible = True
                    div7.Visible = False
                    div1.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                ElseIf (rbtOfferFor.SelectedValue = "B") Then
                    div5.Visible = False
                    div6.Visible = False
                    div1.Visible = True
                    div7.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                    div2.Visible = False
                    'div5.Visible = False
                    'div6.Visible = False
                    'div7.Visible = False
                    'div3.Visible = False
                    'div1.Visible = False
                    'divPackage.Visible = False
                    'divAll.Visible = True
                    'selfbv.Visible = True
                    'DirectBV.Visible = True
                    'Div4.Visible = True
                End If
            Else
                If (rbtOfferFor.SelectedValue = "P") Then
                    div5.Visible = False
                    div7.Visible = False
                    div6.Visible = False
                    divPackage.Visible = True
                    divAll.Visible = False
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                    div1.Visible = False
                    div2.Visible = False
                ElseIf (rbtOfferFor.SelectedValue = "T") Then
                    div5.Visible = False
                    div6.Visible = False
                    div1.Visible = True
                    div7.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                    div2.Visible = False
                ElseIf (rbtOfferFor.SelectedValue = "A") Then
                    div5.Visible = False
                    div6.Visible = False
                    div2.Visible = True
                    div7.Visible = False
                    div1.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = False
                    DirectBV.Visible = False
                    Div4.Visible = False
                ElseIf (rbtOfferFor.SelectedValue = "B") Then
                    div5.Visible = False
                    div6.Visible = False
                    div7.Visible = False
                    div3.Visible = False
                    div1.Visible = False
                    divPackage.Visible = False
                    divAll.Visible = True
                    selfbv.Visible = True
                    DirectBV.Visible = True
                    Div4.Visible = True
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub
    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        Try
            OfferForValiDate()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click
        Try
            Dim query As String = ""
            Dim querydet As String = ""
            Dim formNo As String = ""
            Dim voucherNo As String = ""
            Dim scrName As String
            Dim offerno As String = ""
            Dim K As String = ""
            Dim Str As String = "Select isnull(Max(OfferID)+1,1001) as OfferID from M_MemberOfferNew"
            Dim Dt As New DataTable
            Dt = objDAL.GetData(Str)
            If Dt.Rows.Count > 0 Then
                offerno = Dt.Rows(0)("OfferID")
            End If
            If Session("compid") = "1010" Or Session("compid") = "1103" Or Session("compid") = "1108" Then
                If String.IsNullOrEmpty(Request("OfferID")) = False Then
                    Dim enteredDate As DateTime
                    ' Try to parse the entered date from the TextBox
                    If DateTime.TryParse(txtEndDate.Text, enteredDate) Then
                        ' Compare the entered date with today's date
                        If enteredDate < DateTime.Today Then
                            ' Show a message or handle the error when a past date is entered
                            LblDateMsg.Text = "You cannot select or enter a past date."
                            LblDateMsg.Visible = True
                            Exit Sub
                        Else
                            ' Proceed with saving the form if the date is valid
                            LblDateMsg.Visible = False
                            ' Your save logic here
                        End If
                    Else
                        ' If the date format is invalid
                        LblDateMsg.Text = "Invalid date format. Please enter a valid date."
                        LblDateMsg.Visible = True
                        Exit Sub
                    End If

                    Dim dateValue As DateTime
                    If (RadioButtonList1.SelectedValue = "T") Then
                        querydet = " Update M_MemberOfferNew Set OfferName = '" & txtoffer.Text & "',"
                        querydet &= "EndDate = '" & txtEndDate.Text & "' WHERE OfferID = '" & Request("OfferID") & "' "
                        querydet &= " Update OfferAddMaster Set OfferName = '" & Trim(txtoffer.Text) & "',"
                        querydet &= "EndDate = '" & txtEndDate.Text & "',LastModified='Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "'"
                        querydet &= " WHERE OfferID = '" & Request("OfferID") & "' "
                    End If
                    If (RadioButtonList1.SelectedValue = "A") Then
                        querydet = " Update M_MemberOfferNew Set OfferName = '" & Trim(txtoffer.Text) & "',"
                        querydet &= "EndDate = '" & txtEndDate.Text & "' WHERE OfferID = '" & Request("OfferID") & "' "
                        querydet &= " Update OfferAddMaster Set OfferName = '" & Trim(txtoffer.Text) & "',"
                        querydet &= "EndDate = '" & txtEndDate.Text & "',LastModified='Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "'"
                        querydet &= " WHERE OfferID = '" & Request("OfferID") & "' "
                    End If
                    query = " insert Into TempOfferAddMaster (OfferID,OfferName,StartDate,EndDate,ActiveStatus,OfferType,Reward,DirectBv,Matchingbv,Remark,PaidDate,PaidStatus)"
                    query &= "select OfferID,OfferName,StartDate,EndDate,ActiveStatus,OfferType,Reward,DirectBv,Matchingbv,Remark,PaidDate,PaidStatus from "
                    query &= "OfferAddMaster where OfferID = '" & Request("OfferID") & "'"
                    K = " Begin Try Begin Transaction " & query & querydet & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH "
                Else
                    If (RadioButtonList1.SelectedValue = "T") Then
                        If Session("compid") = "1103" Then
                            For Each row As GridViewRow In GridView6.Rows
                                Dim LeftActive As String = (CType(row.FindControl("txtLeftActivess"), TextBox)).Text
                                Dim LeftDirectActive As String = (CType(row.FindControl("txtLeftActivess1"), TextBox)).Text
                                Dim RightActive As String = (CType(row.FindControl("txtLeftActivessf"), TextBox)).Text
                                If (Val(LeftActive) > 0 Or Val(LeftDirectActive) > 0 Or Val(RightActive) > 0) Then
                                    querydet &= " insert into TrnMemberOfferNew (OfferID,RankID,selfbv,DirectBv,MatchingBv,Reward) "
                                    querydet &= "Values('" & offerno & "', '0','" & txtselfbv.Text & "','" & LeftDirectActive & "','" & LeftActive & "','" & RightActive & "');"
                                    querydet &= "INSERT INTO OfferAddMaster(OfferID,OfferName,StartDate,EndDate,ActiveStatus,OfferType,Reward,DirectBv,Matchingbv,PaidStatus)"
                                    querydet &= "Values('" & offerno & "','" & Trim(txtoffer.Text) & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',"
                                    querydet &= "'" & rbtnstatus.SelectedValue & "','" & RadioButtonList1.SelectedValue & "','" & RightActive & "',"
                                    querydet &= "'" & Val(LeftDirectActive) & "','" & Val(LeftActive) & "','N');"
                                End If
                            Next
                        Else
                            For Each row As GridViewRow In GridView4.Rows
                                Dim LeftActive As String = (CType(row.FindControl("txtLeftActivess"), TextBox)).Text
                                Dim RightActive As String = (CType(row.FindControl("txtLeftActivessf"), TextBox)).Text
                                If (Val(LeftActive) > 0 Or Val(RightActive) > 0) Then
                                    querydet &= " insert into TrnMemberOfferNew (OfferID,RankID,selfbv,DirectBv,MatchingBv,Reward) "
                                    querydet &= "Values('" & offerno & "', '0','" & txtselfbv.Text & "','" & txtdirectbv.Text & "','" & LeftActive & "','" & RightActive & "');"
                                    querydet &= "INSERT INTO OfferAddMaster(OfferID,OfferName,StartDate,EndDate,ActiveStatus,OfferType,Reward,DirectBv,Matchingbv,PaidStatus)"
                                    querydet &= "Values('" & offerno & "','" & Trim(txtoffer.Text) & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',"
                                    querydet &= "'" & rbtnstatus.SelectedValue & "','" & RadioButtonList1.SelectedValue & "','" & RightActive & "',"
                                    querydet &= "'" & Val(txtdirectbv.Text) & "','" & Val(LeftActive) & "','N');"
                                End If
                            Next
                        End If

                    End If
                    If (RadioButtonList1.SelectedValue = "A") Then
                        For Each row As GridViewRow In GridView5.Rows
                            Dim LeftActiveSSES As String = (CType(row.FindControl("LeftActiveSSES"), TextBox)).Text
                            Dim LeftActivessfES As String = (CType(row.FindControl("LeftActivessfES"), TextBox)).Text
                            If (Val(LeftActiveSSES) > 0 Or Val(LeftActivessfES) > 0) Then
                                querydet &= " insert into TrnMemberOfferNewDirect (OfferID,RankID,selfbv,DirectBv,MatchingBv,Reward) "
                                querydet &= "Values('" & offerno & "', '0','" & txtselfbv.Text & "','" & LeftActiveSSES & "','0','" & LeftActivessfES & "');"
                                querydet &= "INSERT INTO OfferAddMaster(OfferID,OfferName,StartDate,EndDate,ActiveStatus,OfferType,Reward,DirectBv,Matchingbv,PaidStatus)"
                                querydet &= "Values('" & offerno & "','" & Trim(txtoffer.Text) & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',"
                                querydet &= "'" & rbtnstatus.SelectedValue & "','" & RadioButtonList1.SelectedValue & "','" & LeftActivessfES & "','" & Val(LeftActiveSSES) & "','0','N');"
                            End If
                        Next
                    End If
                    query = " insert Into M_MemberOfferNew (OfferID,OfferName,StartDate,EndDate,SelfBv,DirectBv,ActiveStatus,Rectimestamp,Offertype)"
                    query &= " values('" & offerno & "','" & Trim(txtoffer.Text) & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',"
                    query &= " '" & Val(txtselfbv.Text) & "','" & Val(txtdirectbv.Text) & "','" & rbtnstatus.SelectedValue & "',getdate(),'" & RadioButtonList1.SelectedValue & "');"
                    K = " Begin Try Begin Transaction " & query & querydet & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH "
                End If
            Else
                If (rbtOfferFor.SelectedValue = "P") Then
                    For Each row As GridViewRow In gv.Rows
                        Dim txtLeftActive As String = (CType(row.FindControl("txtLeftActive"), TextBox)).Text
                        Dim txtRightActive As String = (CType(row.FindControl("txtRightActive"), TextBox)).Text
                        Dim hdnKitId As String = (CType(row.FindControl("hdnKitId"), HiddenField)).Value
                        If (Val(txtLeftActive) > 0 Or Val(txtRightActive) > 0 Or txtRightActive <> "") Then
                            querydet &= " insert into TrnMemberOfferNew (OfferID,RankID,selfbv,DirectBv,MatchingBv,Reward) Values( "
                            querydet &= " '" & offerno & "', '" & hdnKitId & "','" & txtselfbv.Text & "','" & txtdirectbv.Text & "','" & txtLeftActive & "','" & txtRightActive & "');"
                        End If
                    Next
                End If
                If (rbtOfferFor.SelectedValue = "T") Then
                    For Each row As GridViewRow In GridView1.Rows
                        Dim LeftActive As String = (CType(row.FindControl("txtLeftActivess"), TextBox)).Text
                        Dim RightActive As String = (CType(row.FindControl("txtLeftActivessf"), TextBox)).Text
                        If (Val(LeftActive) > 0 Or Val(RightActive) > 0 Or RightActive <> "") Then
                            querydet &= " insert into TrnMemberOfferNew (OfferID,RankID,selfbv,DirectBv,MatchingBv,Reward) Values( "
                            querydet &= " '" & offerno & "', '0','" & txtselfbv.Text & "','" & txtdirectbv.Text & "'," & _
                           " '" & LeftActive & "','" & RightActive & "');"
                        End If
                    Next
                End If
                If (rbtOfferFor.SelectedValue = "A") Then
                    For Each row As GridViewRow In GridView2.Rows
                        Dim LeftActiveSSES As String = (CType(row.FindControl("LeftActiveSSES"), TextBox)).Text
                        Dim LeftActivessfES As String = (CType(row.FindControl("LeftActivessfES"), TextBox)).Text
                        If (Val(LeftActiveSSES) > 0 Or Val(LeftActivessfES) > 0) Then
                            querydet &= " insert into TrnMemberOfferNewDirect (OfferID,RankID,selfbv,DirectBv,MatchingBv,Reward) Values( "
                            querydet &= " '" & offerno & "', '0','" & txtselfbv.Text & "','" & LeftActiveSSES & "'," & _
                           " '0','" & LeftActivessfES & "');"
                        End If
                    Next
                End If
                If (rbtOfferFor.SelectedValue = "B") Then
                    querydet = "insert into M_NormalNew (OfferName,StartDate,EndDate,SelfBv,DirectBv,OfferID,Reward)values('" & Trim(txtoffer.Text) & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "','" & Val(txtselfbv.Text) & "','" & Val(txtdirectbv.Text) & "','" & offerno & "','" & TxtReward.Text & "');"
                End If
                If (rbtOfferFor.SelectedValue = "B") Then
                Else
                    If (querydet.Length < 10) Then
                        scrName = "<SCRIPT language='javascript'>alert('Please enter value in Active left or Active right.!');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                        Exit Sub
                    End If
                End If
                query = " insert Into M_MemberOfferNew (OfferID,OfferName,StartDate,EndDate,SelfBv,DirectBv,ActiveStatus,Rectimestamp,Offertype)"
                query &= " values('" & offerno & "','" & Trim(txtoffer.Text) & "','" & txtStartDate.Text & "','" & txtEndDate.Text & "',"
                query &= " '" & Val(txtselfbv.Text) & "','" & Val(txtdirectbv.Text) & "','" & rbtnstatus.SelectedValue & "',getdate(),'" & rbtOfferFor.SelectedValue & "');"
                K = " Begin Try Begin Transaction " & query & querydet & " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      "
            End If
            If String.IsNullOrEmpty(Request("OfferID")) = False Then
                Dim i As Integer = 0
                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, K))
                If (i > 0) Then
                    scrName = "<SCRIPT language='javascript'>alert('Successfully Updated.!');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                End If
            Else
                Dim i As Integer = 0
                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, K))
                If (i > 0) Then
                    scrName = "<SCRIPT language='javascript'>alert('Successfully Saved.!');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrName, False)
                End If
            End If

            scrName = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrName, False)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Private Sub BindGridView()
        Try
            DivStatus.Visible = False
            DivOfferFor.Visible = False
            div5.Visible = False
            Dim sql As String = "SELECT * FROM M_MemberOfferNew WHERE OfferID = '" & OfferIDQS & "'"
            Dim Dt As DataTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sql).Tables(0)
            If Dt.Rows.Count > 0 Then
                txtoffer.Text = Dt.Rows(0)("OfferName")
                txtStartDate.Text = Dt.Rows(0)("StartDate")
                txtStartDate.ReadOnly = True
                txtEndDate.Text = Dt.Rows(0)("EndDate")
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
