


Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class App_UI_Application_Pages_Imgid
    Inherits System.Web.UI.Page
    Public FormNo As String
    Dim dt As DataTable
    Dim obj As DAL
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FormNo = Request("ID")
        Dim sql As String
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Request("Type") IsNot Nothing Then
            If Request("Type") = "Blog" Then
                sql = "select case When ImgPath='' then '' else ImgPath" & _
    " end as ImageLnk1 from M_Testimonials  where AId='" & Request("AId") & "'"
                dt = New DataTable

                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250



                End If
            ElseIf Request("Type") = "IdVerified" Then
                sql = "select case When  Idproof='' then '" & Session("CompWeb") & "Images/no_photo.jpg' else Idproof" & _
    " end as ImageLnk1 from M_Membermaster  where Formno='" & Request("ID") & "'"
                dt = New DataTable

                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250



                End If
            Else

                Image1.ImageUrl = "ImgHandler.ashx?id=" & Request("ID") & "&Type=" & Request("Type")
                LblNewPic.Visible = True
                LblPic.Visible = True
                LblUpdatePic.Visible = True
                ImageUpload.Visible = True

                Upload.Visible = True
                Cancel.Visible = True
            End If
        Else
            Image1.ImageUrl = "ImgHandler.ashx?id=" & Request("ID")
            LblNewPic.Visible = True
            LblPic.Visible = True
            LblUpdatePic.Visible = True
            ImageUpload.Visible = True
            Upload.Visible = True
            Cancel.Visible = True

        End If
    End Sub


End Class

