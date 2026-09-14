Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Imaging
Imports System.IO

Partial Class AddPackage
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
        Dim Sql As String = "Select Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END as SrStart FROM MM_KitMaster"
        Dim Dt As New DataTable
        Dt = objDAL.GetData(Sql)
        If Dt.Rows.Count > 0 Then
            txtSerialStart.Text = Dt.Rows(0)("SrStart")
        End If
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From MM_KitMaster Where KitId='" & KitIdQS & "' AND " + objDAL.activeCondition
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
                    encoderParams = Nothing ' PNG doesn't support quality compression

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

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Try
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

            If Convert.ToDouble(txtKitAmt.Text) > 0 Then
                JoinColr = "Green.jpg"
            Else
                JoinColr = "red.jpg"
            End If

            If ImageUpload.Enabled Then
                If Not ImageUpload.HasFile Then
                    ScriptManager.RegisterClientScriptBlock(Page, Me.GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", False)
                    Return
                End If
            End If

            If ImageUpload.HasFile Then
                strextension = System.IO.Path.GetExtension(ImageUpload.FileName)
                If strextension.ToUpper() = ".JPG" Or strextension.ToUpper() = ".JPEG" Or strextension.ToUpper() = ".PNG" Then
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream)
                    Dim height As Integer = img.Height
                    Dim width As Integer = img.Width
                    Dim size As Decimal = Math.Round(CDec(ImageUpload.PostedFile.ContentLength) / 1024, 1)

                    If size > 1024 Then
                        Dim scrname As String = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Close", scrname, False)
                        Return
                    Else
                        flAddrs = ImageUpload.PostedFile.FileName
                        'ImageUpload.PostedFile.SaveAs(Server.MapPath("MM_voucher/") & flAddrs)
                        Dim fileName As String = flAddrs
                        Dim savePath As String = Server.MapPath("MM_voucher/") & fileName
                        ImageUpload.PostedFile.SaveAs(savePath)
                        CompressAndSaveImage(ImageUpload.PostedFile.InputStream, savePath, strextension, 50L) ' Quality 50%
                        adrsProof = "https://" & HttpContext.Current.Request.Url.Host & "/MM_voucher/" & fileName
                    End If
                Else
                    Dim scrname As String = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Close", scrname, False)
                    Return
                End If
            Else
                adrsProof = ""
            End If

            If Not String.IsNullOrEmpty(Request("KitId")) Then
                Str = "Insert into TempMMKitMaster([KId],[KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],"
                Str &= "[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],[MRecTimeStamp],[MUserID]) " & _
                       "Select [KId], [KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks]," & _
                       "[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],GetDate(),'" & Session("UserID") & "' from MM_KitMaster as a where a.KitId='" & txtKitId.Text & "';"

                Sql = Str & ";Update MM_KitMaster set KitId='" & txtKitId.Text & "',KitName='" & ClearInject(txtkitName.Text) & "',JoinAmount='" & txtJoinAmt.Text & "'," & _
                      "KitAmount='" & ClearInject(txtKitAmt.Text) & "',KitUnit='" & ClearInject(txtKitUnit.Text) & "',SerialStart='" & ClearInject(txtSerialStart.Text) & "'," & _
                      "RefIncome='" & ClearInject(txtRefIn.Text) & "',PoolIncome='" & ClearInject(txtPoolIn.Text) & "'," & _
                      "SpillIncome='" & ClearInject(txtSpillIn.Text) & "',BinaryIncome='" & ClearInject(txtBinaryIn.Text) & "',BV='" & ClearInject(txtBV.Text) & "',PV='" & ClearInject(txtPV.Text) & "',RP='" & ClearInject(txtRP.Text) & "'," & _
                      "Capping='" & ClearInject(txtCapping.Text) & "',Remarks='" & ClearInject(txtRemarks.Text) & "',ActiveStatus='" & ClearInject(txtActiveStatus.Text) & "'," & _
                      "LastModified='Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "'," & _
                      "JoinColor='" & RbtColor.SelectedValue & "',UserCode='" & Session("UserName") & "',UserId='" & Session("UserID") & "',TopUpSeq='" & ClearInject(TxtTopUp.Text) & "',kitimg='" & adrsProof & "' where KitId='" & ClearInject(txtKitId.Text) & "'"
            Else
                Str = "select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId from MM_KitMaster "
                Dim Dt As New DataTable()
                Dt = objDAL.GetData(Str)

                If Dt.Rows.Count > 0 Then
                    KitId = Dt.Rows(0)("KitId").ToString()
                End If

                Sql = " insert into MM_KitMaster (KitId,KitName,JoinAmount,KitAmount,KitUnit,SerialStart," & _
                      "RefIncome,PoolIncome,SpillIncome,BinaryIncome,BV,PV,RP,Capping,Remarks,ActiveStatus," & _
                      "LastModified,UserCode,UserId,IPAdrs,RowStatus,JoinColor,TopUpSeq,kitimg) " & _
                      "Select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId," & _
                      "'" & ClearInject(txtkitName.Text) & "','" & Convert.ToDouble(txtJoinAmt.Text) & "'," & _
                      "'" & Convert.ToDouble(txtKitAmt.Text) & "','" & Convert.ToDouble(txtKitUnit.Text) & "'," & _
                      "Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END,'" & Convert.ToDouble(txtRefIn.Text) & "'," & _
                      "'" & Convert.ToDouble(txtPoolIn.Text) & "','" & Convert.ToDouble(txtSpillIn.Text) & "','" & Convert.ToDouble(txtBinaryIn.Text) & "'," & _
                      "'" & Convert.ToDouble(txtBV.Text) & "','" & Convert.ToDouble(txtPV.Text) & "','" & Convert.ToDouble(txtRP.Text) & "'," & _
                      "'" & Convert.ToDouble(txtCapping.Text) & "','" & ClearInject(txtRemarks.Text) & "','" & txtActiveStatus.Text & "'," & _
                      "'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "'," & _
                      "'" & Convert.ToDouble(Session("UserID")) & "','" & ClearInject(txtIPAdrs.Text) & "','Y','" & RbtColor.SelectedValue & "'," & _
                      "'" & Convert.ToDouble(TxtTopUp.Text) & "','" & adrsProof & "' From MM_KitMaster "
            End If

            Dim Str_Sql As String = String.Empty
            Str_Sql = "Begin Try Begin Transaction " & Sql & " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH"

            Dim updateEffect As Integer = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql))

            If String.IsNullOrEmpty(Request("KitId")) = False And updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
            ElseIf updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
            End If

            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

        Catch Ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('" & Ex.Message & "');", True)
        End Try
    End Sub
    Private Function ClearInject(ByVal StrObj As String) As String
        Try
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "")
            StrObj = StrObj.Trim()
        Catch Ex As Exception
            Throw New Exception(Ex.Message)
        End Try
        Return StrObj
    End Function

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
