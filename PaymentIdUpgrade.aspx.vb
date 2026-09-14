Imports System.Data
Partial Class PaymentIdUpgrade
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Member / Upgrade Package "
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

                If Not Page.IsPostBack Then

                    fillBalanceEP()
                End If
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        deactiveidpackagelist(TxtIDNo.Text)
    End Sub
    Protected Sub deactiveidpackagelist(ByVal toidno As String)
        Dim _Output As String = ""
        Try
            Dim dt As New DataTable
            Dim DtState As New DataTable
            Dim query As String = ""
            ' obj = New DAL
            query = "select activeStatus,formno,Kitid,memfirstName+''+memlastName as MemberName" & _
                      " from M_MemberMaster where idNo='" & toidno.Trim & "'"
            dt = obj.GetData(query)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("activeStatus") = "N" Then
                    query = "Select KitId,KitName,KitAmount,Cast((kitamount/85) as Numeric(18,2)) as EP  " & _
                    " From M_KitMaster where ActiveStatus='Y' and Rowstatus='Y' "
                    query &= "  and Kitamount>0 And TopupSeq = 1  Order By KitName "
                ElseIf dt.Rows(0)("activeStatus") = "Y" Then


                    query = "Select KitId,KitName,KitAmount,Cast((kitamount/85) as Numeric(18,2)) as EP" & _
                    " From M_KitMaster where ActiveStatus='Y' and Rowstatus='Y' "
                    query &= "  and Kitamount>0 And  " & _
                    " TopupSeq =(select (TopupSeq+1)as Topupseq from M_KitMaster where Kitid='" & dt.Rows(0)("kitid") & "'" & _
                    " ) Order By KitName "
                End If

                If query <> "" Then


                    DtState = obj.GetData(query)

                    If DtState.Rows.Count > 0 Then
                        LblKitId.Text = DtState.Rows(0)("KitId")
                        TxtPackage.Text = DtState.Rows(0)("KitName")
                        TxtAmount.Text = DtState.Rows(0)("KitAmount")
                        TxtEP.Text = DtState.Rows(0)("EP")

                    Else
                        'scrname = "<SCRIPT language='javascript'>alert('Id Already Upgraded!');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Id Already Upgraded.!');", True)
                        Exit Sub
                    End If
                End If

                LblMemName.Text = dt.Rows(0)("membername")
                LblFormno.Text = dt.Rows(0)("Formno")
                ' TxtCompanyEP.Text = dt.Rows(0)("companyBalance")
                If Val(TxtEP.Text) > Val(TxtCompanyEP.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Insufficient Company Balance !');", True)
                    Exit Sub
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Invalid ID No.!');", True)
                Exit Sub
            End If


        Catch ex As Exception

        End Try

    End Sub
    Private Function GetFormNo(ByVal IDNO As String) As Integer
        Dim FrmNo As Integer = 0
        Dim Dt As New DataTable
        Dim Str As String = "Select FormNo,ActiveStatus,Mobl,Address1 From M_MemberMaster WHERE IDNO='" & IDNO & "'"
        Dt = obj.GetData(Str)
        If Dt.Rows.Count > 0 Then
            FrmNo = Dt.Rows(0)("FormNo")
        End If

       
        Return FrmNo
    End Function
    Private Sub Idactivation(ByVal toidno As String, ByVal status As String, ByVal packageid As Integer, ByVal paymenttoken As String, ByVal paymentid As String, ByVal PaymenttType As String)
        Dim sql As String = ""
        Dim query As String = ""
        Dim _output As String = ""
        If checkpaymentid(paymentid) Then


            If checkbalanceep(toidno, "P", packageid) Then
                ' Dim FormNo As Integer = GetFormNo(fromIdno)
                Dim Remark As String
                Remark = " Package Upgrade of Idno:" & TxtIDNo.Text & ""
                sql = "  exec Sp_ActivateMemberApp '" & toidno.Trim & "','" & Val(packageid) & "'"


                sql &= " Declare  @idno varchar(50),@formno int,@KitID Int,@KitName varchar(150),@TravelPoint varchar(150),          "
                sql &= " @Rp numeric(18,2),@Bv numeric(18,2),@Pv numeric(18,2),@address1 varchar(500),@kitAmt numeric(18,2) ,@Orderno int, @Qty int , @TQty int,@EP Numeric(18,2),"
                sql &= " @OrderItem int"

                sql &= " Set @Orderno  =  (Select Isnull(Max(orderNo),100000) + 1 From trnorder) "
                sql &= " select @idno = a.idno,@formno = formno,@KitID = b.KitID,@KitName = b.KitName,@TravelPoint = '' ,"
                sql &= " @Rp = b.Rp,@Bv = b.Bv,@Pv = b.PV,@address1 = a.address1,@kitAmt = b.KitAmount,@EP=Cast(b.KItAmount/85 as Numeric(18,2)) "
                sql &= " from M_MemberMaster as a,M_KitMaster as b where a.KitId=b.KitId And  b.activeStatus='Y'"
                sql &= " and b.RowStatus='Y' and  Idno= '" & toidno.Trim & "'"


                sql &= " Select @Qty =   Count(*) from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as a,M_KitMaster as b "
                sql &= " where a.brandCode=b.KitId  and a.activeStatus='Y' and b.ActiveStatus='Y' and b.RowStatus='Y'"
                sql &= " and  b.kitId=@KitID"


                sql &= " Select @TQty  = @Qty +   Sum(Qty) from "
                sql &= " " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as a,M_KitProductDetail as b "
                sql &= " where  a.ProdId=b.ProdId  and b.activeStatus='Y' and b.RowStatus='Y' and b.KitId=@KitID "


                sql &= " Select @OrderItem  = @Qty + Count(*) from "
                sql &= " " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as a,M_KitProductDetail as b "
                sql &= " where  a.ProdId=b.ProdId  and b.activeStatus='Y' and b.RowStatus='Y' and b.KitId=@KitID "





                sql &= " Insert Into TrnorderDetail(OrderNo,FormNo,ProductID,Qty,Rate,NetAmount,RecTimeStamp,DispDate,DispStatus,DispQty,"
                sql &= " RemQty,DispAmt,MRP,DP,ProductName,ImgPath,RP,BV,FSEssId,ProdType)"
                sql &= " Select @Orderno,@formno,a.ProdId,@Qty,0, 0,Getdate(),'','N',@Qty,@Qty,0, "
                sql &= " a.MRP,a.DP,a.ProductName,'',b.RP,b.bv, "
                sql &= " (Select Max(FSessId) from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_FiscalMaster),'P' from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as a,M_KitMaster as b "
                sql &= " Where a.brandCode=b.KitId  and a.activeStatus='Y' and b.ActiveStatus='Y' and b.RowStatus='Y' "
                sql &= " and  b.kitId=@KitID "


                sql &= " Insert Into TrnorderDetail "
                sql &= " (OrderNo,FormNo,ProductID,Qty,Rate,NetAmount,RecTimeStamp,DispDate,DispStatus,DispQty,"
                sql &= " RemQty,DispAmt,MRP,DP,ProductName,ImgPath,RP,BV,FSEssId,Prodtype,TotalDiscount)"
                sql &= " Select @Orderno,@formno,a.ProdId,b.Qty,a.MRP,(b.Qty * a.MRP) - b.DiscAmt,Getdate(),'','N',b.Qty,b.Qty,"
                sql &= " (b.Qty * a.MRP) - b.DiscAmt,a.MRP,a.MRP - Convert(Decimal(18,2),DISCAmt/Qty),a.ProductName,'',0,a.BV,((Select Max(FSessId) from " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_FiscalMaster)),'F', b.DiscAmt from "
                sql &= " " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..M_ProductMaster as a,M_KitProductDetail as b "
                sql &= " where  a.ProdId=b.ProdId  and b.activeStatus='Y' and b.RowStatus='Y' and b.KitId=@KitID "


                sql &= " Insert INTO TrnOrder(OrderNo,OrderDate,MemFirstName,MemLastName,"
                sql &= " Address1,Address2,CountryID,CountryName,StateCode,City,PinCode, "
                sql &= " Mobl,EMail,FormNo,UserType,Passw,PayMode,ChDDNo,ChDate,"
                sql &= " ChAmt,BankName,BranchName,Remark,OrderAmt,OrderItem, "
                sql &= " OrderQty,ActiveStatus,HostIp,RecTimeStamp,IsTransfer,DispatchDate,DispatchStatus,DispatchQty,RemainQty,"

                sql &= " DispatchAmount,Shipping,SessID,RewardPoint,CourierName, "
                sql &= " DocketNo,OrderFor,IsConfirm,OrderType,Discount,OldShipping,"
                sql &= " ShippingStatus,IdNo,FSessId,BankAmt,OtherAmt,WalletAmt,"
                sql &= " KitName,Bv,TravelPoint)           "
                sql &= " select @Orderno,Cast(Convert(varchar,GETDATE(),106) as Datetime),MemFirstName , MemLastName ,"
                sql &= " @address1 , Address2 , CountryID , 'India' , StateCode , City , Case when PinCode='' then 0 else Pincode end as Pincode ,           "
                sql &= " Mobl, EMail ,@formno,'', Passw ,'',0,'', "
                sql &= "  0,'','','Top Up With:' + @KitName, @kitAmt,@OrderItem, "
                sql &= " @TQty,'Y','C',Getdate(),'Y','','N',0,@TQty,  "

                sql &= " 0,0,(Select Max(SessID) from M_SessnMAster),'0','',  "
                sql &= " 0,'WR','Y','T',0,0,"
                sql &= " 'N',IDno,'1','0','0',0,"
                sql &= " @KitName,@Bv,0 "
                sql &= " from M_memberMaster where Formno=@formno          "



                sql &= " Insert into " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnPaymentConfirmation(SNo,ConfirmBy,OrderNo,FormNo,OrderAmt,IsConfirm,RecTimeStamp,UserID,OrderFor,          "
                sql &= " IDNO,ActiveStatus,OrdType,FSessId)          "
                sql &= " select Case When Max(SNo) Is Null Then '1001' Else Max(SNo)+1 END as SNo,'WR',@Orderno,          "
                sql &= " @formno,@kitAmt,'Y',Getdate(),0,'WR',@idno,'Y','D',1           "
                sql &= " from  " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnPaymentConfirmation      "




                sql &= " INSERT INTO  RepurchIncome(SessID,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType,SoldBy,MSessID,KitID,Remarks,DSessid,PVValue)                                                 "
                sql &= " VALUES ((Select IsNULL(Max(SessID),1) From M_SessnMaster),@formno,@Orderno,Cast(Convert(varchar,Getdate(),106) as DateTime),@Bv,'N','A','HO'"
                sql &= "  ,( SELECT  IsNULL(Max(SessID), 1) FROM M_MonthSessnMaster),@KitID,'',convert(varchar,cast(cast(getdate() as varchar) as datetime),112),@Pv)"

                'If PaymenttType = "P" And status = "captured" Then


                sql &= "  insert into onlinepayment(Orderno,status,PaymentId,paymenttoken,activeStatus,Formno)" & _
                " values(@OrderNo,'" & status & "','" & paymentid & "','" & paymenttoken & "','Y',@Formno)"
                sql &= " Exec Sp_GenerateEP 'Debit', @EP;"
                sql &= "Insert into M_GenerateEPHistory (Formno,EP,VcType,Remark,Balance) Values"
                sql &= "(0,@EP,'D','EP Deduction By Id Activation of IdNo:'+'" & toidno & "', (Select Balance from M_GenerateEP)-@EP)"
                sql = sql & "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,memberId)Values" & _
       "('" & Val(Session("UserID")) & "','" & Session("UserName") & "','Upgrade Package ','Upgrade Package','" & Remark & "',Getdate(),'" & LblFormno.Text & "')"



                query = " Begin Try   Begin Transaction " & sql & " Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH"

                '  Comm = New SqlCommand(query, Conn)
                Dim i As Integer = obj.SaveData(query)
                If i > 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Id Activation Successfully');", True)
                    clear()
                    Exit Sub
                Else
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Id Not Activated');", True)
                    Exit Sub

                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Invalid Company Balance');", True)
                Exit Sub
            End If

        Else
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Payment id already used');", True)
            Exit Sub
        End If


    End Sub
    Protected Sub clear()
        fillBalanceEP()
        TxtpaymentId.Text = ""
        TxtPackage.Text = ""
        TxtIDNo.Text = ""
        LblKitId.Text = ""
        TxtEP.Text = ""
        TxtAmount.Text = ""
        LblMemName.Text = ""
        LblFormno.Text = ""
        lblError.Text = ""
    End Sub
    Private Function checkpaymentid(ByVal paymentid As String) As Boolean
        'Dim sda As SqlDataAdapter
        Dim sql1 As String = ""
        Dim dt1 As New DataTable

        sql1 = "select * from onlinepayment where paymentid='" & paymentid.Trim & "' "

        dt1 = New DataTable
        dt1 = obj.GetData(sql1)
        If dt1.Rows.Count > 0 Then
            Return False
        Else
            Return True
        End If


    End Function
    Private Function checkbalanceep(ByVal toidno As String, ByVal type As String, ByVal packageid As Integer) As Boolean
        'Dim sda As SqlDataAdapter
        Dim sql1 As String = ""
        Dim dt1 As New DataTable
        Dim ep As String = ""
        sql1 = "select cast(( Kitamount/85) as Numeric(18,2)) as Ep from M_kitmaster where kitid='" & packageid & "' "

        dt1 = New DataTable
        dt1 = obj.GetData(sql1)
        If dt1.Rows.Count > 0 Then
            ep = dt1.Rows(0)("EP")
        End If
        sql1 = ""
        'If type = "P" Then
        sql1 = "Select Balance from M_GenerateEP"

        'sda = New SqlDataAdapter(sql1, Conn)
        dt1 = New DataTable
        dt1 = obj.GetData(sql1)

        If (dt1.Rows.Count > 0) Then
            If Val(ep) <= Val(dt1.Rows(0)("Balance")) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
    Protected Sub fillBalanceEP()
        Dim sql1 As String = ""
        Dim dt1 As DataTable
        sql1 = "Select Balance from M_GenerateEP"

        'sda = New SqlDataAdapter(sql1, Conn)
        dt1 = New DataTable
        dt1 = obj.GetData(sql1)

        If (dt1.Rows.Count > 0) Then
            TxtCompanyEP.Text = dt1.Rows(0)("Balance")
        End If
    End Sub
    Protected Sub BtnUpgrade_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpgrade.Click
        Dim scrname As String
        
        Try
            Idactivation(TxtIDNo.Text.Trim, "Captured", LblKitId.Text, TxtpaymentId.Text, TxtpaymentId.Text, "P")
            
            
            If obj.SaveData(Sql) <> 0 Then

                scrname = "<SCRIPT language='javascript'>alert('ID upgraded Successfully!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                lblError.Text = " ID upgraded successfully."
                TxtIDNo.Text = "" : TxtEP.Text = "" : TxtPackage.Text = "" : LblMemName.Text = "" : BtnUpgrade.Enabled = False
                ' BtnExport.Visible = True
            End If
            'End If
        Catch ex As Exception
            scrname = "<SCRIPT language='javascript'>alert('" & ex.Message & "');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        clear()
    End Sub
End Class
