Imports System.Data
Imports System.IO

Partial Class App_UI_Application_Pages_GalleryMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Master / Gallery Master"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnShowRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowRecord.Click
        BindData()
        btnShowRecord.Visible = False
        lblView.Visible = False
        'txtSearch.Text = ""
        'ddlSearchFields.SelectedIndex = 0
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'txtSearch.Text = ""
            btnShowRecord.Visible = False
            lblView.Visible = False
            If Session("AStatus") = "OK" Then
                BindData()
            End If
        End If
    End Sub

    Protected Sub BindData()
        Dim LblFileType As New Label
        Dim LblVideo As New Label
        Dim i As Integer = 0
        Dim ImgImage As New Image
        'Dim Sql As String = " Select PId,Remark,case When (EventType='E' Or EventType='S') And FileType ='I' then '<img src=""../Resources/images/UploadImage/'+ ImagePath + '"" Height=""50px"" /><br />' else '' end as ImageLnk,case When EventType='C'  And FileType ='I' then '<img src=""../../img/Certificate-01.jpg"" Height=""50px"" /><br />' else '' end as CertificateLnk,case When FileType ='I' then ImagePath else '' end as ImagePath,case When FileType ='D' then '<a href=""../Resources/images/UploadImage/'+ DocPath + '"" Height=""50px"" /><br />' else '' end as DocPath ,case When FileType ='D' then ImagePath else '' end as ImagePath1,case When FileType ='V' then ImagePath else '' end as VideoPath ,case When FileType ='I' then 'Image' when FileType='D' then '<img src=""../Resources/images/DocumentImage.jpg"" Height=""50px"" /><br />' else '<img src=""../Resources/images/Videobutton.png"" Height=""50px"" /><br />' end as FileType from ProductGallery where ActiveStatus='Y' "
        ' Dim Sql As String = " Select PId,Remark,b.ImageType,case When FileType ='I'  then '<img src=""images/UploadImage/'+ ImagePath + '"" Height=""50px"" /><br />' else '' end as ImageLnk,case When FileType ='N'  then '<img src=""../images/UploadImage/'+ ImagePath + '"" Height=""50px"" /><br />' else '' end as NewsImage,case When FileType ='C'  then '<img src=""../../img/Certificate-01.jpg"" Height=""50px"" /><br />' else '' end as CertificateLnk,case When FileType ='I' then ImagePath else '' end as ImagePath,case When FileType ='N' then ImagePath else '' end as NewsPath,case When FileType ='D' then '<a href=""../images/UploadImage/'+ DocPath + '"" Height=""50px"" /><br />' else '' end as DocPath ,case When FileType ='D' then ImagePath else '' end as ImagePath1,case When FileType ='L' then ImagePath else '' end as LinkPath ,case When FileType ='V' then ImagePath else '' end as VideoPath ,case When FileType ='I' Or FileType='C'Or FileType='N' then 'Image' when FileType='D' Or FileType='L' then 'Document' else 'Video' end as FileType,CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status from ProductGallery as a,M_ImageTypeMaster as b where a.IID=b.IId  "
        Dim sql As String = " select * from( Select PId,Remark,b.ImageType, case When FileType ='I'  then " & _
             " '<img src=""images/UploadImage/'+ ImagePath + '"" Height=""50px"" /><br />' else '' end as ImageLnk," & _
      " case When FileType ='N'  then '<img src=""../images/UploadImage/'+ ImagePath + '"" Height=""50px"" /><br />' else '' end as NewsImage," & _
    " case When FileType ='C'  then '<img src=""../../img/Certificate-01.jpg"" Height=""50px"" /><br />' else '' end as CertificateLnk," & _
    " case When FileType ='I' then ImagePath else '' end as ImagePath, case When FileType ='N' then ImagePath else '' end as NewsPath," & _
    " case When FileType ='D' then '<a href=""../images/Document/'+ DocPath + '"" Height=""50px"" /><br />' else '' end as DocPath ," & _
    " case When FileType ='D' then ImagePath else '' end as ImagePath1, case When FileType ='L' then ImagePath else '' end as LinkPath ," & _
   "  case When FileType ='V' then ImagePath else '' end as VideoPath , case When FileType ='I' Or FileType='C'Or FileType='N' then 'Image' when FileType='D' " & _
    " Or FileType='L' then 'Document' else 'Video' end as FileType, CASE WHEN a.ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status " & _
     "  from ProductGallery as a,M_ImageTypeMaster as b where a.IID=b.IId  " & _
      "  Union All" & _
    " Select PId,Remark,'' as ImageType,'' as ImageLnk, " & _
         "   '' as NewsImage,'' as CertificateLnk,'' as ImagePath,'' as NewsPath,case When FileType ='D' then '<a href=""../images/Document/'+ DocPath + '"" Height=""50px"" /><br />' else '' end as DocPath ," & _
 " case When FileType ='D' then ImagePath else '' end as ImagePath1," & _
               " '' as LinkPath,'' as VideoPath,case When FileType ='I' Or FileType='C'Or FileType='N' then 'Image' when FileType='D' " & _
        " Or FileType='L' then 'Document' else 'Video' end as FileType, CASE WHEN ActiveStatus='Y' Then 'Active' ELSE 'DeActive' END AS Status " & _
            " from ProductGallery  where ActiveStatus='Y' and FileType='D' ) as temp "
                dtData = objDAL.GetData(Sql)
                GvData.DataSource = dtData
                GvData.DataBind()
                Session("GData") = dtData

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GData")
        GvData.DataBind()
    End Sub

    Protected Sub DeleteGroup(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim PId, scrname As String
        Dim GVRw As GridViewRow
        GVRw = CType(sender.Parent.Parent, GridViewRow)
        'GrpID = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        PId = DirectCast(GVRw.FindControl("LblGrpID"), Label).Text
        Dim Sql As String = "Update ProductGallery  SET ActiveStatus='N' WHERE PId='" & Val(PId.ToString()) & "' "
        Dim updateEffect As Integer = objDAL.UpdateData(Sql)
        If updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" & "</SCRIPT>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Group Deletion", scrname, False)
        BindData()
    End Sub
    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        Dim i As Integer
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim stts As String = DirectCast(e.Row.FindControl("lblStatus"), Label).Text
            If String.Equals(stts.ToLower(), "deactive") = True Then
                'e.Row.BackColor = Drawing.Color.Red
                e.Row.Style("background-image") = "images/redback2.jpg"
                For i = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style("color") = "red"
                Next
            End If
        End If
    End Sub


End Class
