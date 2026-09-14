Imports System.Data
Imports System.IO


Partial Class App_UI_Application_Pages_AddImage
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim tmpTable As New Data.DataTable
    'Dim tmpTable As Data.DataTable
    Dim ProductCodeQS As String
    Private dbConnect As cls_DataAccess
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("PId")) = False Then
            ProductCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("PId")))
            Session("PId") = ProductCodeQS
        End If
        If Not Page.IsPostBack Then
            ClearAll()
            If Session("AStatus") = "OK" Then
                FillImageType()
                If String.IsNullOrEmpty(Request("PId")) = False Then
                    BtnUpdate.Text = "Modify"
                    BindData()


                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        'txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
        'BindData()

    End Sub


    Protected Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
        Dim Sql, PId As String
        Dim FlNm As String = ""
        Dim ImgFl1 As String = ""
        Dim ImgFl2 As String = ""
        Dim DocPath As String = ""
        Dim ImageType As String = ""
        'If DDlImageType.SelectedItem.Text = "Certificate" And RbtFileType.SelectedValue = "I" Then

        If RbtFileType.SelectedValue = "C" Then
            If ImageUpload.HasFile Then
                If ImageUpload.PostedFile IsNot Nothing AndAlso ImageUpload.PostedFile.FileName <> "" Then
                    Dim strExtension As String = System.IO.Path.GetExtension(ImageUpload.FileName)
                    If (strExtension.ToUpper() = ".JPG") Or (strExtension.ToUpper() = ".GIF") Or (strExtension.ToUpper() = ".JPEG") Or (strExtension.ToUpper() = ".BMP") Or (strExtension.ToUpper() = ".PNG") Then


                        ImgFl1 = "Certificate-01.jpg"
                        ImageType = "0"
                        ImageUpload.PostedFile.SaveAs(Server.MapPath("img/") & ImgFl1)
                    Else
                        scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg,.gif,.jpeg,.bmp,.png extension file!! ');" & "</SCRIPT>"
                        ' End If
                        'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        Exit Sub
                    End If
                End If
            Else
                ImageType = 0
                ImgFl1 = Session("ProdImg")
                DocPath = ""
            End If
            'End If


        ElseIf RbtFileType.SelectedValue = "I" Then
            If ImageUpload.HasFile Then
                If ImageUpload.PostedFile IsNot Nothing AndAlso ImageUpload.PostedFile.FileName <> "" Then
                    FlNm = Format(Now, "yyyyMMddhhmmssfff")
                    Dim strExtension As String = System.IO.Path.GetExtension(ImageUpload.FileName)
                    'If (strExtension.ToUpper() = ".JPG") Or (strExtension.ToUpper() = ".GIF") Or (strExtension.ToUpper() = ".JPEG") Or (strExtension.ToUpper() = ".BMP") Or (strExtension.ToUpper() = ".PNG") Then
                    If (strExtension.ToUpper() = ".JPG") Or (strExtension.ToUpper() = ".GIF") Or (strExtension.ToUpper() = ".PNG") Or (strExtension.ToUpper() = ".JPEG") Then
                        Dim FileName As String = Server.MapPath("images\UploadImage\" & FlNm & strExtension)
                        ' Resize Image Before Uploading to DataBase
                        Dim imageToBeResized As System.Drawing.Image = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream)
                        Dim imageHeight As Integer = imageToBeResized.Height
                        Dim imageWidth As Integer = imageToBeResized.Width
                        Dim maxHeight As Integer = 500
                        Dim maxWidth As Integer = 600
                        imageHeight = (imageHeight * maxWidth) / imageWidth
                        imageWidth = maxWidth
                        If imageHeight > maxHeight Then
                            imageWidth = (imageWidth * maxHeight) / imageHeight
                            imageHeight = maxHeight
                        End If
                        Dim bitmap As New Drawing.Bitmap(imageToBeResized, imageWidth, imageHeight)
                        Dim stream As System.IO.MemoryStream = New MemoryStream()
                        If strExtension.ToUpper() = ".JPG" Or strExtension.ToUpper() = ".JPEG" Then bitmap.Save(FileName, System.Drawing.Imaging.ImageFormat.Jpeg)
                        If strExtension.ToUpper() = ".PNG" Then bitmap.Save(FileName, System.Drawing.Imaging.ImageFormat.Png)
                        If strExtension.ToUpper() = ".GIF" Then bitmap.Save(FileName, System.Drawing.Imaging.ImageFormat.Gif)

                        ImgFl1 = FlNm & strExtension
                        ImgFl2 = ""
                        ImageType = DDlImageType.SelectedValue

                    Else
                        scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg,.gif,.jpeg,.png extension file!! ');" & "</SCRIPT>"
                        ' End If
                        'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        Exit Sub
                    End If
                End If
            Else
                ImageType = DDlImageType.SelectedValue
                ImgFl1 = Session("ProdImg")
                DocPath = ""
            End If

        ElseIf RbtFileType.SelectedValue = "N" Then
            If ImageUpload.HasFile Then
                If ImageUpload.PostedFile IsNot Nothing AndAlso ImageUpload.PostedFile.FileName <> "" Then
                    Dim strExtension As String = System.IO.Path.GetExtension(ImageUpload.FileName)
                    If (strExtension.ToUpper() = ".JPG") Or (strExtension.ToUpper() = ".GIF") Or (strExtension.ToUpper() = ".JPEG") Or (strExtension.ToUpper() = ".BMP") Or (strExtension.ToUpper() = ".PNG") Then

                        ImgFl1 = Format(Now, "yyMMddhhmmssfff") & "_1" & Path.GetExtension(ImageUpload.PostedFile.FileName)
                        ImageUpload.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & ImgFl1)
                        ImgFl2 = ""
                        ImageType = 0
                    Else
                        scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg,.gif,.jpeg,.bmp,.png extension file!! ');" & "</SCRIPT>"
                        ' End If
                        'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        Exit Sub
                    End If
                End If
            Else
                ImageType = 0
                ImgFl1 = Session("ProdImg")
                DocPath = ""
            End If


        ElseIf RbtFileType.SelectedValue = "D" Then
            Dim FlAddrs As String = ""
            Dim FlBank As String = ""
            Dim FlPan As String = ""
            Dim BaNKProof As String = ""
            Dim FlBackAddrs As String = ""
            If ImageUpload.HasFile Then
                '    Dim strExtension As String = System.IO.Path.GetExtension(ImageUpload.FileName)
                '    If (strExtension.ToUpper() = ".DOCX") Or (strExtension.ToUpper() = ".XLSX") Or (strExtension.ToUpper() = ".PDF") Or (strExtension.ToUpper() = ".DOC") Or (strExtension.ToUpper() = ".XLS") Or (strExtension.ToUpper() = ".PPT") Or (strExtension.ToUpper() = ".PPTX") Then
                '        'Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream)
                '        'Dim height As Integer = img.Height
                '        'Dim width As Integer = img.Width
                '        'Dim size As Decimal = Math.Round((CDec(ImageUpload.PostedFile.ContentLength) / CDec(1024)), 1)
                '        'If size > 1024 Then
                '        '    scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 5 mb size only!! ');" & "</SCRIPT>"
                '        '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                '        '    Exit Sub
                '        'Else
                '        FlBank = "PDF" & Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(ImageUpload.PostedFile.FileName)
                '        ImageUpload.PostedFile.SaveAs(Server.MapPath("images/Document/") & FlBank)
                '        BaNKProof = "https://" & HttpContext.Current.Request.Url.Host & "/images/Document/" & FlBank
                '    End If
                'Else
                '    scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                '    Exit Sub
                'End If
                'Else


                'BaNKProof = LblBankImage.Text
                If ImageUpload.PostedFile IsNot Nothing AndAlso ImageUpload.PostedFile.FileName <> "" Then
                    Dim strExtension As String = System.IO.Path.GetExtension(ImageUpload.FileName)
                    If (strExtension.ToUpper() = ".DOCX") Or (strExtension.ToUpper() = ".XLSX") Or (strExtension.ToUpper() = ".PDF") Or (strExtension.ToUpper() = ".DOC") Or (strExtension.ToUpper() = ".XLS") Or (strExtension.ToUpper() = ".PPT") Or (strExtension.ToUpper() = ".PPTX") Then
                        ImageType = 0
                        ' ImgFl1 = ImageUpload.PostedFile.FileName
                        ImgFl2 = "PDF" & Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(ImageUpload.PostedFile.FileName)
                        ImageUpload.PostedFile.SaveAs(Server.MapPath("images/Document/") & ImgFl2)
                        ImgFl1 = "https://" & HttpContext.Current.Request.Url.Host & "/images/Document/" & ImgFl2
                    Else
                        scrname = "<SCRIPT language='javascript'>alert('You can upload only .doc,.docx,.xls,.Pdf,.xlsx,.ppt,.pptx extension file!! ');" & "</SCRIPT>"
                        ' End If
                        'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        Exit Sub

                    End If
                End If
            Else
                ImageType = 0
                ImgFl1 = Session("ProdImg")
                ImgFl2 = Session("Docpath")
            End If
        ElseIf RbtFileType.SelectedValue = "V" Then
            ImageType = DDlImageType.SelectedValue
            ImgFl1 = txtVideo.Text
            DocPath = ""
        ElseIf RbtFileType.SelectedValue = "L" Then
            ImageType = 0
            ImgFl1 = txtVideo.Text
            DocPath = ""

        End If


        Dim str As String = "select IsNull (Max(PId+1),1) as PId from ProductGallery"
        Dim dt As New DataTable
        dt = objDAL.GetData(str)
        If (dt.Rows.Count > 0) Then
            PId = dt.Rows(0)("PId")

        End If

        If String.IsNullOrEmpty(Request("PId")) = False Then
            str = "Insert into TempProductGallery Select *,'" & Val(Session("UserID")) & "',GetDate() from ProductGallery where PId='" & Val(Session("PId")) & "'"

            Sql = str & ";Update ProductGallery set Remark='" & txtRemark.Text & "',ImagePath='" & ImgFl1 & "',ActiveStatus='" & RbtStatus.SelectedValue & "',FileType='" & RbtFileType.SelectedValue & "',DocPath='" & ImgFl2 & "',IId='" & ImageType & "' where PId='" & Val(Session("PId")) & "'"
        Else
            Sql = "Insert Into ProductGallery(PId,ImagePath,FileType,Remark,ActiveStatus,RecTimeStamp,EventType,Docpath,IId)Values('" & PId & "','" & ImgFl1 & "','" & RbtFileType.SelectedValue & "','" & txtRemark.Text & "','" & RbtStatus.SelectedValue & "',GetDate(),'','" & ImgFl2 & "','" & ImageType & "')"
        End If
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.SaveData(Sql)

        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

    End Sub

    Private Sub ClearAll()
        txtRemark.Text = ""

    End Sub

    Protected Sub RbtFileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtFileType.SelectedIndexChanged
        If RbtFileType.SelectedValue = "I" Then
            LblImagePath.Text = "Image Path:"
            PVideo.Visible = False
            PimagePath.Visible = True
            PImageType.Visible = True
        ElseIf RbtFileType.SelectedValue = "V" Then
            LblVideo.Text = "Video Url:"
            PVideo.Visible = True
            PimagePath.Visible = False
            PImageType.Visible = True
        ElseIf RbtFileType.SelectedValue = "C" Then
            PVideo.Visible = False
            PimagePath.Visible = True
            PImageType.Visible = False
            LblImagePath.Text = "Certificate Path :"
        ElseIf RbtFileType.SelectedValue = "L" Then
            LblVideo.Text = "Link Url:"
            PVideo.Visible = "True"
            PimagePath.Visible = False
            PImageType.Visible = False
        ElseIf RbtFileType.SelectedValue = "N" Then
            LblImagePath.Text = "News Image Path:"
            PVideo.Visible = False
            PimagePath.Visible = True
            PImageType.Visible = False
        Else
            LblImagePath.Text = "Document Path:"
            PVideo.Visible = False
            PimagePath.Visible = True
            PImageType.Visible = False
        End If


    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From ProductGallery Where PId='" & ProductCodeQS & "'  "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            'RbtEventType.SelectedValue = Dt.Rows(0)("EventType")
            DDlImageType.SelectedValue = Dt.Rows(0)("IId")
            RbtFileType.SelectedValue = Dt.Rows(0)("FileType")
            RbtStatus.SelectedValue = Dt.Rows(0)("ActiveStatus")
            Session("ProdImg") = Dt.Rows(0)("ImagePath")
            txtRemark.Text = Dt.Rows(0)("Remark")
            txtVideo.Text = Dt.Rows(0)("ImagePath")
            Session("DocPath") = Dt.Rows(0)("Docpath")
            If RbtFileType.SelectedValue = "I" Then
                LblImagePath.Text = "Image Path:"
                PVideo.Visible = False
                PimagePath.Visible = True
                PImageType.Visible = True
            ElseIf RbtFileType.SelectedValue = "V" Then
                LblVideo.Text = "Video Url:"
                PVideo.Visible = True
                PimagePath.Visible = False
                PImageType.Visible = True
            ElseIf RbtFileType.SelectedValue = "C" Then
                PVideo.Visible = False
                PimagePath.Visible = True
                PImageType.Visible = False
                LblImagePath.Text = "Certificate Path :"
            ElseIf RbtFileType.SelectedValue = "L" Then
                LblVideo.Text = "Link Url:"
                PVideo.Visible = "True"
                PimagePath.Visible = False
                PImageType.Visible = False
            ElseIf RbtFileType.SelectedValue = "N" Then
                LblImagePath.Text = "News Image Path:"
                PVideo.Visible = False
                PimagePath.Visible = True
                PImageType.Visible = False
            Else
                LblImagePath.Text = "Document Path:"
                PVideo.Visible = False
                PimagePath.Visible = True
                PImageType.Visible = False
            End If
            'If RbtEventType.SelectedValue = "C" Then
            '    RbtFileType.Items(1).Enabled = False
            '    RbtFileType.Items(2).Enabled = False
            'Else
            '    RbtFileType.Items(1).Enabled = True
            '    RbtFileType.Items(2).Enabled = True


            'End If


        End If
    End Sub
    Protected Sub ValidateFileSize(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        Try

            If RbtFileType.SelectedValue = "I" Or RbtFileType.SelectedValue = "C" Or RbtFileType.SelectedValue = "N" Then
                Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream)
                Dim height As Integer = img.Height
                Dim width As Integer = img.Width
                Dim size As Decimal = Math.Round((CDec(ImageUpload.PostedFile.ContentLength) / CDec(1024)), 2)
                If size > 800 Then
                    CustomValidator1.ErrorMessage = "File size must not exceed 800 KB."
                    e.IsValid = False
                End If
            End If
        Catch ex As Exception
        End Try

    End Sub

    'Protected Sub GvData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GvData.SelectedIndexChanged

    'End Sub

    'Protected Sub RbtEventType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtEventType.SelectedIndexChanged

    '    If RbtEventType.SelectedValue = "C" Then
    '        RbtFileType.Items(1).Enabled = False
    '        RbtFileType.Items(2).Enabled = False
    '    Else
    '        RbtFileType.Items(1).Enabled = True
    '        RbtFileType.Items(2).Enabled = True


    '    End If

    'End Sub


    Private Sub FillImageType()
        Dim strQuery As String = ""
        Dim dbConnect As New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dbConnect.OpenConnection()


        strQuery = "SELECT IId,ImageType  FROM M_ImageTypeMaster WHERE ACTIVESTATUS='Y' "

        tmpTable = objDAL.GetData(strQuery)
        'dbConnect.OpenConnection()
        dbConnect.Fill_Data_Tables(strQuery, tmpTable)
        With DDlImageType
            .DataSource = tmpTable
            .DataValueField = "IId"
            .DataTextField = "ImageType"
            .DataBind()
            .SelectedIndex = 1
        End With
    End Sub

    Protected Sub DDlImageType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlImageType.SelectedIndexChanged

    End Sub
End Class
