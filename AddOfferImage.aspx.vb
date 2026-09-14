Imports System.Data
Imports System.IO


Partial Class AddOfferImage
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
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("PId")) = False Then
            ProductCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("PId")))
            Session("PId") = ProductCodeQS
        End If
        If Not Page.IsPostBack Then
            'ClearAll()
            If Session("AStatus") = "OK" Then

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
        Dim Filename As String = ""
        'If DDlImageType.SelectedItem.Text = "Certificate" And RbtFileType.SelectedValue = "I" Then


        If ImageUpload.HasFile Then
            If ImageUpload.PostedFile IsNot Nothing AndAlso ImageUpload.PostedFile.FileName <> "" Then
                Dim strExtension As String = System.IO.Path.GetExtension(ImageUpload.FileName)
                If (strExtension.ToUpper() = ".JPG") Or (strExtension.ToUpper() = ".GIF") Or (strExtension.ToUpper() = ".JPEG") Or (strExtension.ToUpper() = ".BMP") Or (strExtension.ToUpper() = ".PNG") Then

                    FlNm = Format(Now, "yyyyMMddhhmmssfff") & strExtension
                    ImgFl1 = Format(Now, "yyyyMMddhhmmssfff") & strExtension
                    ImageType = "0"
                    ImageUpload.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & ImgFl1)
                    If Session("Compid") = "1009" Then
                        Filename = "https://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm

                    Else
                        Filename = "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm

                    End If
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



        If String.IsNullOrEmpty(Request("PId")) = False Then
            '   str = "Insert into TempPopUpmaster Select *,GetDate(),'" & Val(Session("UserID")) & "' from M_PopUpMaster where PId='" & Val(Session("PId")) & "'"

            Sql = ";Update M_OfferImage set ImgPath='" & Filename & "',ActiveStatus='" & RbtStatus.SelectedValue & "'" & _
            " where Id='" & Val(Session("PId")) & "'"
        Else
            Sql = "Insert Into M_OfferImage(ImgPath,ActiveStatus,RectimeStamp,Userid)" & _
            " Values('" & Filename & "'," & _
            " '" & RbtStatus.SelectedValue & "',GetDate(),'" & Session("Userid") & "')"
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





    Private Sub BindData()
        Dim sql As String = "Select * From M_OfferImage Where Id='" & ProductCodeQS & "'  "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            'RbtEventType.SelectedValue = Dt.Rows(0)("EventType")

            RbtStatus.SelectedValue = Dt.Rows(0)("ActiveStatus")
            Session("ProdImg") = Dt.Rows(0)("ImgPath")
           
           

        End If
    End Sub
    Protected Sub ValidateFileSize(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        Try

            '   If RbtFileType.SelectedValue = "I" Or RbtFileType.SelectedValue = "C" Or RbtFileType.SelectedValue = "N" Then
            Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream)
            Dim height As Integer = img.Height
            Dim width As Integer = img.Width
            Dim size As Decimal = Math.Round((CDec(ImageUpload.PostedFile.ContentLength) / CDec(1024)), 2)
            If size > 800 Then
                CustomValidator1.ErrorMessage = "File size must not exceed 800 KB."
                e.IsValid = False
            End If
            'End If
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



End Class
