Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddKit
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim KitIdQS As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("KitId")) = False Then
            '   KitIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("KitId")))
            KitIdQS = Request("KitId")
        End If
        If Not Page.IsPostBack Then
            Pages()
            ClearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("KitId")) = False Then
                    BtnSave.Text = "Modify"
                    If Session("compid") = 1010 Then
                        CouponAmount.Enabled = False
                        NoofCoupon.Enabled = False
                        txtwellcoupon.Enabled = False
                        txtwellcouponamt.Enabled = False
                    End If
       
                    BindData()

                Else
                    Fill_SeriesStart()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub

    Private Sub Fill_SeriesStart()
        Dim Sql As String = "Select Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END as SrStart FROM M_KitMaster"
        Dim Dt As New DataTable
        Dt = objDAL.GetData(Sql)
        If Dt.Rows.Count > 0 Then
            txtSerialStart.Text = Dt.Rows(0)("SrStart")
        End If
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From " + objDAL.tblKitMaster + " Where KitId='" & KitIdQS & "' AND " + objDAL.activeCondition
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        Dim Dat1 As String
        If Dt.Rows.Count > 0 Then
            LblKitDate.Text = Format(Dt.Rows(0)("Rectimestamp"), "dd-MMM-yyyy")
            Dat1 = Format(Date.Now, "dd-MMM-yyyy")
            If LblKitDate.Text = Dat1 Then
                txtBV.Enabled = True
                txtPV.Enabled = True
                txtRP.Enabled = True
            Else

                txtBV.Enabled = False
                txtPV.Enabled = False
                txtRP.Enabled = False
            End If
            If Session("CompId") = "1033" Then
                DDlPlan.SelectedValue = Dt.Rows(0)("Plantype")
            End If
            If Session("CompId") = "1010" Then
                TxtRewardPonit.Text = Dt.Rows(0)("RewardPoint")
                
            End If
            txtKitId.Text = Dt.Rows(0)("KitId")
            txtkitName.Text = Dt.Rows(0)("KitName")
            txtJoinAmt.Text = Dt.Rows(0)("JoinAmount")
            txtKitAmt.Text = Dt.Rows(0)("KitAmount")
            txtKitUnit.Text = Dt.Rows(0)("KitUnit")
            txtSerialStart.Text = Dt.Rows(0)("SerialStart")
            txtRefIn.Text = Dt.Rows(0)("RefIncome")
            txtPoolIn.Text = Dt.Rows(0)("PoolIncome")
            txtSpillIn.Text = Dt.Rows(0)("SpillIncome")
            txtBinaryIn.Text = Dt.Rows(0)("BinaryIncome")
            txtBV.Text = Dt.Rows(0)("BV")
            txtPV.Text = Dt.Rows(0)("PV")
            txtRP.Text = Dt.Rows(0)("RP")
            TxtTopUp.Text = Dt.Rows(0)("TopUpSeq")
            txtCapping.Text = Dt.Rows(0)("Capping")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
          
          
            RbtColor.SelectedValue = Dt.Rows(0)("JoinColor")
            If Session("compid") = 1010 Then
                NoofCoupon.Text = Dt.Rows(0)("CouponQty")
                CouponAmount.Text = Dt.Rows(0)("CouponAmount")
                txtwellcoupon.Text = Dt.Rows(0)("WELLSMARTNOOFCOUPON")
                txtwellcouponamt.Text = Dt.Rows(0)("WELLSMARTCOUPONAMOUNT")
            End If
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub


    Private Sub Pages()
        Try
            Dim dtMenu As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            Dim str As String = " Select * from M_CompWiseWebMenuMaster Where MenuID = 123 And CompanyID = '" & HttpContext.Current.Session("CompID") & "'  "
            str &= " And ActiveStatus = 'Y' And RowStatus ='Y'"
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dtMenu = ds.Tables(0)
            If (dtMenu.Rows.Count > 0) Then
                Session("KitProductMaster") = "Y"
            Else
                Session("KitProductMaster") = "N"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        Dim Str As String
        Dim KitId As String = ""
        Dim JoinColr As String = ""
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
        If txtKitAmt.Text > 0 Then
            JoinColr = "Green.jpg"
        Else
            JoinColr = "red.jpg"
        End If
        If Session("CompId") = "1010" Then
            If txtwellcoupon.Text <> "" And txtwellcouponamt.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Enter well smart coupon amount.!');", True)
                Exit Sub
            End If

            If txtwellcoupon.Text = "" And txtwellcouponamt.Text <> "" Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Enter well smart no of coupon.!');", True)
                Exit Sub
            End If
        End If
        If String.IsNullOrEmpty(Request("KitId")) = False Then
            If Session("CompId") <> "1033" Then
                Str = "Insert into TempKitMaster([KId],[KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],[MRecTimeStamp],[MUserID])Select [KId], [KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],GetDate(),'" & Val(Session("UserID")) & "' from M_Kitmaster as a where a.KitId='" & Val(txtKitId.Text) & "';"
                If Session("compid") = "1010" Then
                    Sql = Str & ";Update M_KitMaster set KitId='" & Val(txtKitId.Text) & "',KitName='" & txtkitName.Text & "',JoinAmount='" & Val(txtJoinAmt.Text) & "',KitAmount='" & Val(txtKitAmt.Text) & "',KitUnit='" & Val(txtKitUnit.Text) & "',SerialStart='" & Val(txtSerialStart.Text) & "',RefIncome='" & Val(txtRefIn.Text) & "',PoolIncome='" & Val(txtPoolIn.Text) & "',SpillIncome='" & Val(txtSpillIn.Text) & "',BinaryIncome='" & Val(txtBinaryIn.Text) & "',BV='" & Val(txtBV.Text) & "',PV='" & Val(txtPV.Text) & "',RP='" & Val(txtRP.Text) & "',Capping='" & Val(txtCapping.Text) & "',Remarks='" & txtRemarks.Text & "',ActiveStatus='" & txtActiveStatus.Text & "',LastModified='Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',JoinColor='" & RbtColor.SelectedValue & "'," & _
               " UserCode='" & Val(Session("UserName")) & "',UserId='" & Val(Session("UserID")) & "',TopUpSeq='" & Val(TxtTopUp.Text) & "',RewardPoint='" & Val(TxtRewardPonit.Text) & "' where KitId='" & Val(txtKitId.Text) & "'"
                Else
                    Sql = Str & ";Update M_KitMaster set KitId='" & Val(txtKitId.Text) & "',KitName='" & txtkitName.Text & "',JoinAmount='" & Val(txtJoinAmt.Text) & "',KitAmount='" & Val(txtKitAmt.Text) & "',KitUnit='" & Val(txtKitUnit.Text) & "',SerialStart='" & Val(txtSerialStart.Text) & "',RefIncome='" & Val(txtRefIn.Text) & "',PoolIncome='" & Val(txtPoolIn.Text) & "',SpillIncome='" & Val(txtSpillIn.Text) & "',BinaryIncome='" & Val(txtBinaryIn.Text) & "',BV='" & Val(txtBV.Text) & "',PV='" & Val(txtPV.Text) & "',RP='" & Val(txtRP.Text) & "',Capping='" & Val(txtCapping.Text) & "',Remarks='" & txtRemarks.Text & "',ActiveStatus='" & txtActiveStatus.Text & "',LastModified='Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',JoinColor='" & RbtColor.SelectedValue & "'," & _
               " UserCode='" & Val(Session("UserName")) & "',UserId='" & Val(Session("UserID")) & "',TopUpSeq='" & Val(TxtTopUp.Text) & "' where KitId='" & Val(txtKitId.Text) & "'"

                End If
                If (Session("KitProductMaster") = "Y") Then
                    Sql = Sql & ";exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..InsertProduct '" & Val(txtKitId.Text) & "';"
                End If
           
            Else
                Str = "Insert into TempKitMaster([KId],[KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],[MRecTimeStamp],[MUserID],[PlanType])Select [KId], [KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],GetDate(),'" & Val(Session("UserID")) & "',PlanType from M_Kitmaster as a where a.KitId='" & Val(txtKitId.Text) & "';"

                Sql = Str & ";Update M_KitMaster set KitId='" & Val(txtKitId.Text) & "',KitName='" & txtkitName.Text & "',JoinAmount='" & Val(txtJoinAmt.Text) & "',KitAmount='" & Val(txtKitAmt.Text) & "',KitUnit='" & Val(txtKitUnit.Text) & "',SerialStart='" & Val(txtSerialStart.Text) & "',RefIncome='" & Val(txtRefIn.Text) & "',PoolIncome='" & Val(txtPoolIn.Text) & "',SpillIncome='" & Val(txtSpillIn.Text) & "',BinaryIncome='" & Val(txtBinaryIn.Text) & "',BV='" & Val(txtBV.Text) & "',PV='" & Val(txtPV.Text) & "',RP='" & Val(txtRP.Text) & "',Capping='" & Val(txtCapping.Text) & "',Remarks='" & txtRemarks.Text & "',ActiveStatus='" & txtActiveStatus.Text & "',LastModified='Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "'," & _
                " JoinColor='" & RbtColor.SelectedValue & "',PlanType='" & Val(DDlPlan.SelectedValue) & "'," & _
                " UserCode='" & Val(Session("UserName")) & "',UserId='" & Val(Session("UserID")) & "',TopUpSeq='" & Val(TxtTopUp.Text) & "' where KitId='" & Val(txtKitId.Text) & "'"
                If (Session("KitProductMaster") = "Y") Then
                    Sql = Sql & ";exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..InsertProduct '" & Val(txtKitId.Text) & "';"
                End If

            End If


            'objDAL.UpdateData(Str)
            'Sql = "Update " & objDAL.tblKitMaster & " set RowStatus = 'N' Where KitId = '" & Val(txtKitId.Text) & "'"
            'Sql = Sql & " insert into " & objDAL.tblKitMaster & " (KitId,KitName,JoinAmount,KitAmount,KitUnit,
            'SerialStart,RefIncome,PoolIncome,SpillIncome,BinaryIncome,
            'BV,PV,RP,Capping,Remarks,
            'ActiveStatus,LastModified,UserCode,UserId,IPAdrs,
            'RowStatus)"
            ' values('" & Val(txtKitId.Text) & "','" & txtkitName.Text & "','" & Val(txtJoinAmt.Text) & "','" & Val(txtKitAmt.Text) & "','" & Val(txtKitUnit.Text) & "'
            ','" & txtSerialStart.Text & "','" & Val(txtRefIn.Text) & "','" & Val(txtPoolIn.Text) & "','" & Val(txtSpillIn.Text) & "','" & Val(txtBinaryIn.Text) & "',
            '" & Val(txtBV.Text) & "','" & Val(txtPV.Text) & "','" & Val(txtRP.Text) & "','" & Val(txtCapping.Text) & "','" & txtRemarks.Text & "',
            '" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y')"
        Else
            Str = "select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId from m_kitMaster "
            Dt = New DataTable
            Dt = objDAL.GetData(Str)
            If Dt.Rows.Count > 0 Then
                KitId = Dt.Rows(0)("KitId")
            End If

            If Session("CompId") <> "1033" Then

                If Session("CompId") = "1010" Then
                    If txtwellcoupon.Text = "" Then
                        txtwellcoupon.Text = "0"
                    End If
                    If txtwellcouponamt.Text = "" Then
                        txtwellcouponamt.Text = "0"
                    End If

                    Sql = " insert into " & objDAL.tblKitMaster & " (KitId,KitName,JoinAmount,KitAmount,KitUnit,SerialStart,RefIncome," & _
                          "PoolIncome,SpillIncome,BinaryIncome,BV,PV,RP,Capping,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,JoinColor," & _
                          "TopUpSeq,CouponQty,CouponAmount,RewardPoint,WELLSMARTNOOFCOUPON,WELLSMARTCOUPONAMOUNT) " & _
                          "Select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId,'" & txtkitName.Text & "','" & Val(txtJoinAmt.Text) & "'," & _
                    "'" & Val(txtKitAmt.Text) & "','" & Val(txtKitUnit.Text) & "',Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END," & _
                    "'" & Val(txtRefIn.Text) & "','" & Val(txtPoolIn.Text) & "','" & Val(txtSpillIn.Text) & "','" & Val(txtBinaryIn.Text) & "','" & Val(txtBV.Text) & "'," & _
                    "'" & Val(txtPV.Text) & "','" & Val(txtRP.Text) & "','" & Val(txtCapping.Text) & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "'," & _
                    "'Modified by " & Val(Session("UserName")) & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "'," & _
                    "'" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & RbtColor.SelectedValue & "'," & _
                    "'" & Val(TxtTopUp.Text) & "','" & NoofCoupon.Text & "','" & CouponAmount.Text & "','" & TxtRewardPonit.Text & "','" & txtwellcoupon.Text & "','" & txtwellcouponamt.Text & "' From " & objDAL.tblKitMaster
                    'Sql = " insert into " & objDAL.tblKitMaster & " (KitId,KitName,JoinAmount,KitAmount,KitUnit,SerialStart,RefIncome,PoolIncome,SpillIncome,BinaryIncome,BV,PV,RP,Capping,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,JoinColor,TopUpSeq,CouponQty,CouponAmount,RewardPoint) Select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId,'" & txtkitName.Text & "','" & Val(txtJoinAmt.Text) & "','" & Val(txtKitAmt.Text) & "','" & Val(txtKitUnit.Text) & "',Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END,'" & Val(txtRefIn.Text) & "','" & Val(txtPoolIn.Text) & "','" & Val(txtSpillIn.Text) & "','" & Val(txtBinaryIn.Text) & "','" & Val(txtBV.Text) & "','" & Val(txtPV.Text) & "','" & Val(txtRP.Text) & "','" & Val(txtCapping.Text) & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Val(Session("UserName")) & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & RbtColor.SelectedValue & "','" & Val(TxtTopUp.Text) & "','" & NoofCoupon.Text & "','" & CouponAmount.Text & "','" & TxtRewardPonit.Text & "' From " & objDAL.tblKitMaster
                Else
                    Sql = " insert into " & objDAL.tblKitMaster & " (KitId,KitName,JoinAmount,KitAmount,KitUnit,SerialStart,RefIncome,PoolIncome,SpillIncome,BinaryIncome,BV,PV,RP,Capping,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,JoinColor,TopUpSeq) Select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId,'" & txtkitName.Text & "','" & Val(txtJoinAmt.Text) & "','" & Val(txtKitAmt.Text) & "','" & Val(txtKitUnit.Text) & "',Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END,'" & Val(txtRefIn.Text) & "','" & Val(txtPoolIn.Text) & "','" & Val(txtSpillIn.Text) & "','" & Val(txtBinaryIn.Text) & "','" & Val(txtBV.Text) & "','" & Val(txtPV.Text) & "','" & Val(txtRP.Text) & "','" & Val(txtCapping.Text) & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Val(Session("UserName")) & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & RbtColor.SelectedValue & "','" & Val(TxtTopUp.Text) & "' From " & objDAL.tblKitMaster
                End If
                If (Session("KitProductMaster") = "Y") Then
                    Sql = Sql & ";exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..InsertProduct '" & Val(KitId) & "';"
                End If



            Else
                Sql = " insert into " & objDAL.tblKitMaster & " (KitId,KitName,JoinAmount,KitAmount,KitUnit,SerialStart,RefIncome,PoolIncome,SpillIncome,BinaryIncome,BV,PV,RP,Capping,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,JoinColor,TopUpSeq,PlanType)" & _
                " Select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId,'" & txtkitName.Text & "','" & Val(txtJoinAmt.Text) & "','" & Val(txtKitAmt.Text) & "','" & Val(txtKitUnit.Text) & "'," & _
                " Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END,'" & Val(txtRefIn.Text) & "','" & Val(txtPoolIn.Text) & "','" & Val(txtSpillIn.Text) & "','" & Val(txtBinaryIn.Text) & "','" & Val(txtBV.Text) & "','" & Val(txtPV.Text) & "','" & Val(txtRP.Text) & "','" & Val(txtCapping.Text) & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Val(Session("UserName")) & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & RbtColor.SelectedValue & "','" & Val(TxtTopUp.Text) & "','" & Val(DDlPlan.SelectedValue) & "' From " & objDAL.tblKitMaster

                If (Session("KitProductMaster") = "Y") Then
                    Sql = Sql & ";exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..InsertProduct '" & Val(KitId) & "';"
                End If

            End If

        End If

            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Sql)

            If String.IsNullOrEmpty(Request("KitId")) = False And updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
            ElseIf updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
            End If

            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

    Private Sub ClearAll()
        txtKitId.Text = ""
        txtkitName.Text = ""
        txtJoinAmt.Text = 0
        txtKitAmt.Text = 0
        txtKitUnit.Text = 0
        txtSerialStart.Text = ""
        txtRefIn.Text = 0
        txtPoolIn.Text = 0
        txtSpillIn.Text = 0
        txtBinaryIn.Text = 0
        txtBV.Text = 0
        txtPV.Text = 0
        txtRP.Text = 0
        txtCapping.Text = 0
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        TxtTopUp.Text = 0
    End Sub
End Class
