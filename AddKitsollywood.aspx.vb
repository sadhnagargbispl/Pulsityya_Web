Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq

Partial Class App_UI_Application_Pages_AddKitsollywood
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim KitIdQS As String
    Dim objGen As clsGeneral = New clsGeneral
    Dim Sql As String = ""

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
                    FillKit()
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
    Public Sub FillKit(Optional ByVal condition As String = "")
        Try

            Sql = " select * from (select 0 as Acid,'--Choose Voucher Category--' as Purchasename Union all "
            Sql &= "Select Acid,Purchasename From Purchasetype Where Activestatus='N' )as a "
            Sql &= "where  1=1 " & condition & " Order By Acid"
            Dim Dt As New DataTable
            Dt = objDAL.GetData(Sql)
            DDltype.DataSource = Dt
            DDltype.DataTextField = "Purchasename"
            DDltype.DataValueField = "Acid"
            DDltype.DataBind()
            Session("MKit") = Dt
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ''obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

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
        Dim sql As String = "Select * From " + objDAL.tblKitMaster + " as a left join Purchasetype as b on a.pid=b.Acid Where a.KitId='" & KitIdQS & "' AND " + objDAL.activeCondition
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
            txtrank1.Text = Dt.Rows(0)("rankid1")
            txtrank2.Text = Dt.Rows(0)("rankid2")
            txtrank3.Text = Dt.Rows(0)("rankid3")
            txtrank4.Text = Dt.Rows(0)("rankid4")
            txtrank5.Text = Dt.Rows(0)("rankid5")
            txtcashback.Text = Dt.Rows(0)("cashback")
            txtdiscount.Text = Dt.Rows(0)("discount")
            DDltype.SelectedValue = Dt.Rows(0)("Pid")
            FillKit()
            'DDltype.SelectedItem.Text = Dt.Rows(0)("PurchaseName")
            If Not IsDBNull(Dt.Rows(0)("PurchaseName")) Then
                DDltype.SelectedItem.Text = Dt.Rows(0)("PurchaseName").ToString()
            End If
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
        Dim adrsProof As String = ""
        Dim flAddrs As String = ""
        Dim strextension As String = ""
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
        If DDltype.SelectedValue = "0" Then
            ScriptManager.RegisterClientScriptBlock(Page, Me.GetType(), "Close", "<SCRIPT language='javascript'>alert('Please select voucher type!!');</SCRIPT>", False)
            Return
        End If
        
        If txtrank1.Text = "" Then
            txtrank1.Text = "0"
        End If
        If txtrank2.Text = "" Then
            txtrank2.Text = "0"
        End If
        If txtrank3.Text = "" Then
            txtrank3.Text = "0"
        End If
        If txtrank4.Text = "" Then
            txtrank4.Text = "0"
        End If
        If txtrank5.Text = "" Then
            txtrank5.Text = "0"
        End If
        If txtcashback.Text = "" Then
            txtcashback.Text = "0"
        End If
        If txtdiscount.Text = "" Then
            txtdiscount.Text = "0"
        End If
        If String.IsNullOrEmpty(Request("KitId")) = False Then
            If ImageUpload.HasFile Then
                strextension = System.IO.Path.GetExtension(ImageUpload.FileName).ToUpper()

                If strextension = ".JPG" Or strextension = ".JPEG" Or strextension = ".PNG" Then
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream)
                    Dim height As Integer = img.Height
                    Dim width As Integer = img.Width
                    Dim size As Decimal = Math.Round(ImageUpload.PostedFile.ContentLength / 1024D, 1)

                    If size > 1024 Then ' Greater than 1 MB
                        Dim scrname As String = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!!');</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Close", scrname, False)
                        Return
                    Else
                        flAddrs = ImageUpload.PostedFile.FileName
                        Dim fileName As String = flAddrs ' You can add logic to generate unique names if needed
                        Dim savePath As String = Server.MapPath("assets/img/") & fileName
                        ImageUpload.PostedFile.SaveAs(savePath)
                        CompressAndSaveImage(ImageUpload.PostedFile.InputStream, savePath, strextension, 50L) ' Quality = 50
                        adrsProof = "https://" & HttpContext.Current.Request.Url.Host & "/assets/img/" & fileName
                    End If
                Else
                    Dim scrname As String = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!!');</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Close", scrname, False)
                    Return
                End If
            Else
                adrsProof = ""
            End If
            If adrsProof = "" Then
                Str = "select kitimg from m_kitMaster where kitid='" & Val(txtKitId.Text) & "'"
                Dt = New DataTable
                Dt = objDAL.GetData(Str)
                If Dt.Rows.Count > 0 Then
                    adrsProof = Dt.Rows(0)("kitimg")
                End If
            End If

            Str = "Insert into TempKitMaster([KId],[KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],[MRecTimeStamp],[MUserID])Select [KId], [KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],GetDate(),'" & Val(Session("UserID")) & "' from M_Kitmaster as a where a.KitId='" & Val(txtKitId.Text) & "';"

            Sql = Str & ";Update M_KitMaster set KitId='" & Val(txtKitId.Text) & "',KitName='" & txtkitName.Text & "',JoinAmount='" & Val(txtJoinAmt.Text) & "',KitAmount='" & Val(txtKitAmt.Text) & "',KitUnit='" & Val(txtKitUnit.Text) & "',SerialStart='" & Val(txtSerialStart.Text) & "',RefIncome='" & Val(txtRefIn.Text) & "',PoolIncome='" & Val(txtPoolIn.Text) & "',SpillIncome='" & Val(txtSpillIn.Text) & "',BinaryIncome='" & Val(txtBinaryIn.Text) & "',BV='" & Val(txtBV.Text) & "',PV='" & Val(txtPV.Text) & "',RP='" & Val(txtRP.Text) & "',Capping='" & Val(txtCapping.Text) & "',Remarks='" & txtRemarks.Text & "',ActiveStatus='" & txtActiveStatus.Text & "',LastModified='Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',JoinColor='" & RbtColor.SelectedValue & "'," & _
       " UserCode='" & Val(Session("UserName")) & "',UserId='" & Val(Session("UserID")) & "',TopUpSeq='" & Val(TxtTopUp.Text) & "',kitimg='" & adrsProof & "',rankid1='" & txtrank1.Text & "',rankid2='" & txtrank2.Text & "',rankid3='" & txtrank3.Text & "',rankid4='" & txtrank4.Text & "',rankid5='" & txtrank5.Text & "',Pid='" & DDltype.SelectedValue & "',cashback='" & txtcashback.Text & "',Discount='" & txtdiscount.Text & "' where KitId='" & Val(txtKitId.Text) & "'"

            If (Session("KitProductMaster") = "Y") Then
                Sql = Sql & ";exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..InsertProduct '" & Val(txtKitId.Text) & "';"
            End If
        Else
            If ImageUpload.Enabled Then
                If Not ImageUpload.HasFile Then
                    ScriptManager.RegisterClientScriptBlock(Page, Me.GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!!');</SCRIPT>", False)
                    Return
                End If
            End If
            If ImageUpload.HasFile Then
                strextension = System.IO.Path.GetExtension(ImageUpload.FileName).ToUpper()

                If strextension = ".JPG" Or strextension = ".JPEG" Or strextension = ".PNG" Then
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream)
                    Dim height As Integer = img.Height
                    Dim width As Integer = img.Width
                    Dim size As Decimal = Math.Round(ImageUpload.PostedFile.ContentLength / 1024D, 1)

                    If size > 1024 Then ' Greater than 1 MB
                        Dim scrname As String = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!!');</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Close", scrname, False)
                        Return
                    Else
                        flAddrs = ImageUpload.PostedFile.FileName
                        Dim fileName As String = flAddrs ' You can add logic to generate unique names if needed
                        Dim savePath As String = Server.MapPath("assets/img/") & fileName
                        ImageUpload.PostedFile.SaveAs(savePath)
                        CompressAndSaveImage(ImageUpload.PostedFile.InputStream, savePath, strextension, 50L) ' Quality = 50
                        adrsProof = "https://" & HttpContext.Current.Request.Url.Host & "/assets/img/" & fileName
                    End If
                Else
                    Dim scrname As String = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!!');</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Close", scrname, False)
                    Return
                End If
            Else
                adrsProof = ""
            End If


            Str = "select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId from m_kitMaster "
            Dt = New DataTable
            Dt = objDAL.GetData(Str)
            If Dt.Rows.Count > 0 Then
                KitId = Dt.Rows(0)("KitId")
            End If
            Sql = " insert into " & objDAL.tblKitMaster & " (KitId,KitName,JoinAmount,KitAmount,KitUnit,SerialStart,RefIncome,PoolIncome,SpillIncome,BinaryIncome,BV,PV,RP,Capping,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,JoinColor,TopUpSeq,kitimg,rankid1,rankid2,rankid3,rankid4,rankid5,Cashback,Pid,Disount)" & _
            " Select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId,'" & txtkitName.Text & "','" & Val(txtJoinAmt.Text) & "','" & Val(txtKitAmt.Text) & "','" & Val(txtKitUnit.Text) & "'," & _
            " Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END,'" & Val(txtRefIn.Text) & "','" & Val(txtPoolIn.Text) & "','" & Val(txtSpillIn.Text) & "','" & Val(txtBinaryIn.Text) & "','" & Val(txtBV.Text) & "','" & Val(txtPV.Text) & "','" & Val(txtRP.Text) & "','" & Val(txtCapping.Text) & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Val(Session("UserName")) & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & RbtColor.SelectedValue & "','" & Val(TxtTopUp.Text) & "','" & adrsProof & "','" & txtrank1.Text & "','" & txtrank2.Text & "','" & txtrank3.Text & "','" & txtrank4.Text & "','" & txtrank5.Text & "','" & txtcashback.Text & "','" & DDltype.SelectedValue & "','" & txtdiscount.Text & "' From " & objDAL.tblKitMaster

            If (Session("KitProductMaster") = "Y") Then
                Sql = Sql & ";exec " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..InsertProduct '" & Val(KitId) & "';"
            End If

        End If

        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("KitId")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');location.reload(KitMastersollywood.aspx);" & "</SCRIPT>"
        ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');location.reload(KitMastersollywood.aspx);" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');location.reload(KitMastersollywood.aspx);" & "</SCRIPT>"
        End If

        'scrname = "<SCRIPT language='javascript'> window.top.location.reload(KitMastersollywood.aspx);" & "</SCRIPT>"
        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        scrname = "window.top.location.href='KitMastersollywood.aspx';"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "RedirectPage", scrname, True)
    End Sub
    Private Sub CompressAndSaveImage(ByVal inputStream As Stream, ByVal savePath As String, ByVal extension As String, Optional ByVal quality As Long = 50L)
        Using img As System.Drawing.Image = System.Drawing.Image.FromStream(inputStream)
            Dim encoderParams As New EncoderParameters(1)
            Dim codec As ImageCodecInfo = Nothing

            Select Case extension.ToLower()
                Case ".jpg", ".jpeg"
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(Function(c) c.MimeType = "image/jpeg")
                    encoderParams.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality)

                Case ".png"
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(Function(c) c.MimeType = "image/png")
                    encoderParams = Nothing ' PNG does not support quality settings

                Case ".gif"
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(Function(c) c.MimeType = "image/gif")
                    encoderParams = Nothing

                Case Else
                    Throw New Exception("Unsupported file type.")
            End Select

            If codec IsNot Nothing Then
                If encoderParams IsNot Nothing Then
                    img.Save(savePath, codec, encoderParams)
                Else
                    img.Save(savePath, codec, Nothing)
                End If
            End If
        End Using
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
