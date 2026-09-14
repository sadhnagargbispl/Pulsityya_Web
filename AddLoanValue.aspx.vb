Imports System.Data
Partial Class AddLoanValue
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim scrname As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If Session("AStatus") = "OK" Then
                    FillProduct()
                    'Session("PageName") = "Member / Coin Value"
                    'FillData(Val(Request.QueryString("id")))
                Else
                    Response.Redirect("Logout.aspx")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnFundTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFundTransfer.Click
        Try

            Dim loanno As String = ""
            Sql = "select isnull(Max(Loanno),10000)+1 as Loanno from TrnIdLoan"
            Dim dt As New DataTable
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            dt = obj.GetData(Sql)
            If dt.Rows.Count > 0 Then
                loanno = dt.Rows(0)("Loanno")
            End If
            Dim Amount As String
            Dim ChequeNo As String
            Dim i As Integer = 1
            Dim ChequeDate As String
            CheckIdno()
            If DDlProduct.SelectedValue <> 0 Then
                If DDlSerial.SelectedItem.Text <> "Choose Serial" Then
                    If TxtIDNo.Text <> "" Then
                        If Val(TxtFormNo.Text) <> 0 Then
                            If Val(TxtMonth.Text) > 0 And Val(TxtMonth.Text) <= 10 Then

                                Sql = "insert into TrnIdLoan(Loanno,LoanDate,Formno,Prodid,Productname,LoanCount,TotalAmount,AdvanceAmount," & _
                    " RemainingAmount,Status,Remark,SerialNo)" & _
                    " Values( '" & loanno & "',getdate(),'" & TxtFormNo.Text & "','" & DDlProduct.SelectedValue & "','" & DDlProduct.SelectedItem.Text & "'," & _
                    " '" & TxtMonth.Text & "','" & TxtFund.Text & "','" & TxtROI.Text & "'" & _
                    " ,'" & TxtRemaining.Text & "','Y','" & TxtRemarks.Text.Trim & "','" & DDlSerial.SelectedValue & "' );"
                                For Each Gvr As GridViewRow In GrdDirect.Rows
                                    Amount = DirectCast(Gvr.FindControl("LblAMount"), Label).Text
                                    ChequeNo = DirectCast(Gvr.FindControl("TxtCheqUeNo"), TextBox).Text
                                    ChequeDate = DirectCast(Gvr.FindControl("TxtCheqUeDate"), TextBox).Text
                                    ' For i = 0 To GrdDirect.Rows.Count - 1
                                    If ChequeDate <> "" Then

                                        If ChequeNo <> "" Then

                                            Sql = Sql & "insert into TrnLoanDetail(Loanno,Formno,EmiNo,DueDate,ChequeNo,Amount,ChequeDate)" & _
                                            " Values('" & loanno & "','" & TxtFormNo.Text & "','" & i & "',DateAdd(Month," & i & ",GetDate()),'" & ChequeNo & "','" & Amount & "',Cast('" & (ChequeDate) & "' as date));"
                                        Else
                                            scrname = "<SCRIPT language='javascript'>alert(' Enter Cheque No ! ');" & "</SCRIPT>"
                                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                                            Exit Sub


                                        End If
                                    Else
                                        scrname = "<SCRIPT language='javascript'>alert(' Enter Cheque Date ! ');" & "</SCRIPT>"
                                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                                        Exit Sub


                                    End If


                                    i = i + 1
                                Next
                                Dim j As Integer = 0
                                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                                Sql = Sql & ";exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..[DispatchLoanOrder1] '" & loanno & "'"

                                j = obj.SaveData(Sql)
                                If j > 0 Then
                                    Sql = "Select Top 1 * from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain where FCode='" & TxtIDNo.Text & "' and OrderNo='" & loanno & "' "
                                    obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                                    dt = obj.GetData(Sql)
                                    Dim billno As String = ""
                                    If dt.Rows.Count > 0 Then
                                        billno = Base64Encode(dt.Rows(0)("userBillno"))
                                    End If
                                    Dim pageurl As String = "https://franchise.alkamediindia.in/Invoice/DownloadPdf?Pm=" & billno & "&id=" & TxtIDNo.Text & ""
                                    FillProduct()
                                    FillGrid()
                                    TxtRemarks.Text = ""
                                    TxtFormNo.Text = ""
                                    TxtIDNo.Text = ""
                                    TxtName.Text = ""
                                    TxtROI.Text = ""
                                    TxtMonth.Text = ""
                                    TxtFund.Text = ""
                                    TxtRemaining.Text = ""
                                    TxtROI.Text = ""
                                    'Response.Redirect(pageurl)
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.[GetType](), "alert", "alert(' saved sucessfully');window.location ='" & pageurl & "';", True)
                                    'scrname = "<SCRIPT language='javascript'>alert(' Save SuccessFully ! ');" & "</SCRIPT>"
                                    'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                                    'Exit Sub
                                    Exit Sub
                                End If
                            Else
                                scrname = "<SCRIPT language='javascript'>alert(' Enter EMI No Between 1 to 10  ! ');" & "</SCRIPT>"
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                                Exit Sub

                            End If
                        Else
                            scrname = "<SCRIPT language='javascript'>alert(' Enter Valid Member ID ! ');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                            Exit Sub

                        End If
                    Else
                        scrname = "<SCRIPT language='javascript'>alert(' Enter Member ID ! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                        Exit Sub

                    End If
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Choose Serial No! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                    Exit Sub

                End If
            Else
                scrname = "<SCRIPT language='javascript'>alert('Choose Product! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                Exit Sub

            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Shared Function Base64Encode(ByVal plainText As String) As String
        Dim plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText)
        Return System.Convert.ToBase64String(plainTextBytes)
    End Function
    Private Function FillGrid() As String

        Dim Dt As New DataTable
        Dim scrname As String = ""
        GrdDirect.DataSource = Nothing
        GrdDirect.DataBind()
        Sql = "select 0 as loanno,'' as ChequeNo,0.00 as Amount,'' as ChequeDate"
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Dim dt As New DataTable
        Dt = obj.GetData(Sql)
        If Dt.Rows.Count > 0 Then
            Session("ProductList") = Dt
        End If

    End Function
    Private Function FillProduct() As String
        Dim sql As String = "select * from(select 0 as prodid,'Choose Product' as ProductName,0 as MRP Union all " & _
"select Prodid,ProductName,MRP from Alkamediinv..M_ProductMaster where ActiveStatus='Y'  and Imported='J') as a order by prodid"
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim dt As New DataTable
        dt = obj.GetData(sql)
        If dt.Rows.Count > 0 Then
            DDlProduct.DataSource = dt
            DDlProduct.DataValueField = "Prodid"
            DDlProduct.DataTextField = "ProductName"
            DDlProduct.DataBind()
            TxtFund.Text = dt.Rows(0)("MRP")
            TxtROI.Text = Val(dt.Rows(0)("MRP")) * 0.5
            TxtRemaining.Text = Val(dt.Rows(0)("MRP")) * 0.5
            Session("KitTable") = dt
            sql = ""
        End If

    End Function
    Private Function CheckIdno() As String
        Try
            Dim s As String = ""
            Dim sql As String = ""
            sql = " select a.formno,a.MemfirstName ,Isnull(b.Loanno,0) as Loanno,a.ActiveStatus from M_memberMaster as a Left Join TrnIdLoan as b " & _
            " On a.Formno=b.Formno where idno='" & TxtIDNo.Text.Trim & "'"
            Dim Dt As New DataTable
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            Dt = obj.GetData(sql)
            If Dt.Rows.Count > 0 Then
                If Dt.Rows(0)("ActiveStatus") = "N" Then
                    If Dt.Rows(0)("Loanno") = 0 Then
                        TxtName.Text = Dt.Rows(0)("MemfirstName")
                        TxtFormNo.Text = Dt.Rows(0)("formno")
                    Else
                        TxtName.Text = ""
                        TxtFormNo.Text = 0

                        scrname = "<SCRIPT language='javascript'>alert(' This ID Already Have Loan ! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                        Exit Function

                    End If

                Else

                    TxtName.Text = ""
                    TxtFormNo.Text = 0
                    scrname = "<SCRIPT language='javascript'>alert(' ID Already Activate ! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                    Exit Function

                End If
            Else

                TxtName.Text = ""
                TxtFormNo.Text = 0
                scrname = "<SCRIPT language='javascript'>alert(' Enter Valid Member ID! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                Exit Function

            End If
        Catch ex As Exception

        End Try
    End Function

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        CheckIdno()
    End Sub

    Protected Sub DDlProduct_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlProduct.SelectedIndexChanged
        Dim Dt As New DataTable
        Dt = DirectCast(Session("KitTable"), DataTable)
        Dim Dr() As DataRow = Dt.Select("ProdID='" & DDlProduct.SelectedValue & "'")
        If Dr.Length > 0 Then
            TxtFund.Text = Dr(0)("MRP")
            TxtROI.Text = Val(Dr(0)("MRP")) * 0.5
            TxtRemaining.Text = Val(Dr(0)("MRP")) * 0.5
        End If
        Dim str As String = "select * from (select 1 as sno,'Choose Serial' as SerialNo,0 as Qty Union all select 2 as SNo, SerialNo,sum(Qty) as Qty from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..Im_CurrentStock where ProdId = '" & DDlProduct.SelectedValue & "' and FCode = 'WR' group by SerialNo having sum(Qty)>0) as s Order by SNo"
        Dim dt1 As New DataTable
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        dt1 = obj.GetData(str)
        If dt1.Rows.Count > 0 Then
            DDlSerial.DataTextField = "SerialNo"
            DDlSerial.DataValueField = "SerialNo"
            DDlSerial.DataSource = dt1
            DDlSerial.DataBind()
        End If
    End Sub

    Protected Sub TxtMonth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtMonth.TextChanged
        Try
            Dim Dt As New DataTable
            Dim scrname As String = ""
            GrdDirect.DataSource = Nothing
            GrdDirect.DataBind()
            Sql = "select 0 as loanno,'' as ChequeNo,0.00 as Amount,'' as ChequeDate"
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            'Dim dt As New DataTable
            Dt = obj.GetData(Sql)
            If Dt.Rows.Count > 0 Then
                Session("ProductList") = Dt
            End If

            If Val(TxtMonth.Text) > 0 And Val(TxtMonth.Text) <= 10 Then


                If Val(TxtMonth.Text) > 0 Then
                    Dim dr_ As DataRow
                    For i = 0 To Val(TxtMonth.Text) - 1

                        dr_ = Dt.NewRow

                        dr_("Loanno") = Val(i + 1)
                        ' dr_("Qty") = Val(TxtQty.Text)
                        dr_("Amount") = Math.Round((TxtRemaining.Text) / Val(TxtMonth.Text), 2)
                        dr_("ChequeNo") = ""
                        Dt.Rows.Add(dr_)


                    Next
                End If
                Dim Dr() As DataRow = Dt.Select("Loanno='0'")
                If Dr.Length > 0 Then
                    For i = 0 To Dr.Length - 1
                        Dt.Rows.Remove(Dr(i))
                    Next
                End If

                GrdDirect.DataSource = Dt
                GrdDirect.DataBind()
            Else
                scrname = "<SCRIPT language='javascript'>alert(' Enter EMI No Between 1 to 10 ! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                Exit Sub

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TxtROI_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtROI.TextChanged
        Try
            If Val(TxtFund.Text) >= Val(TxtROI.Text) Then
                If Val(TxtROI.Text) < Val(TxtFund.Text * 0.5) Then
                    TxtROI.Text = TxtFund.Text * 0.5

                    TxtRemaining.Text = Val(TxtFund.Text) - Val(TxtROI.Text)
                    scrname = "<SCRIPT language='javascript'>alert(' Advance Amount Would  be 50 % Or Greater Than Total Amount ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                    Exit Sub
                Else
                    TxtRemaining.Text = Val(TxtFund.Text) - Val(TxtROI.Text)
                End If
            Else

                TxtROI.Text = TxtFund.Text * 0.5
                TxtRemaining.Text = Val(TxtFund.Text) - Val(TxtROI.Text)
                scrname = "<SCRIPT language='javascript'>alert(' Advance Amount Would Not be  Greater Than Total Amount ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        FillProduct()
        FillGrid()
        TxtRemarks.Text = ""
        TxtFormNo.Text = ""
        TxtIDNo.Text = ""
        TxtName.Text = ""
        TxtROI.Text = ""
        TxtMonth.Text = ""
        TxtFund.Text = ""
        TxtRemaining.Text = ""
        TxtROI.Text = ""

    End Sub
End Class
