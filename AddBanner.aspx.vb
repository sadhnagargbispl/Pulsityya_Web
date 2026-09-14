Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Partial Class App_UI_Application_Pages_AddBanner
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim BIDQs As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        If String.IsNullOrEmpty(Request("bannerid")) = False Then
            BIDQs = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("bannerid")))
        End If

        If Not Page.IsPostBack Then
            Session("Imagepath") = ""
            ClearAll()
          
            txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
            If String.IsNullOrEmpty(Request("bannerid")) = False Then
                BtnSave.Text = "Modify"
                BindData()
            End If
        End If
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From M_BannerMaster Where bannerid ='" & Val(BIDQs.ToString()) & "' "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            LblPreImage.Visible = True
            txtRemarks.Text = Dt.Rows(0)("Remark")
            
            Session("Imagepath") = Dt.Rows(0)("Imagepath")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
           
            If String.Equals(txtActiveStatus.Text, 1) = True Then
                rdblist.SelectedIndex = 1
            Else
                rdblist.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub ClearAll()

        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
       
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
     
        'If rdblist.SelectedIndex = 0 Then
        txtActiveStatus.Text = rdblist.SelectedValue
        'Else
        'txtActiveStatus.Text = rdblist.SelectedIndex
        ' End If
        Dim FlNm As String = ""
        Dim flnm2 As String = ""
        If FlUpld1.HasFile Then
            flnm2 = Format(Now, "yyMMddhhmmssfff") & Path.GetExtension(FlUpld1.PostedFile.FileName)
            FlNm = "/images/banner/" & Format(Now, "yyMMddhhmmssfff") & Path.GetExtension(FlUpld1.PostedFile.FileName)
            FlUpld1.PostedFile.SaveAs(Server.MapPath("~/images/banner/") & flnm2)
        Else
            If String.IsNullOrEmpty(Request("bannerid")) = False Then
                FlNm = Session("Imagepath")
            Else
                scrname = "<SCRIPT language='javascript'>alert('Please Upload Banner Image. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
                Exit Sub
            End If
        End If



        If String.IsNullOrEmpty(Request("bannerid")) = False Then
            Sql = "Update M_BannerMaster SET ActiveStatus='N' Where bannerid='" & BIDQs & "';"
            Sql = Sql & " Insert into M_BannerMaster (bannerid,Imagepath,Remark,ActiveStatus,RectimeStamp) values " & _
            "((Select IsNULL(Max(bannerid)+1,1) From M_BannerMaster),'" & FlNm & "',@Remarks,'" & txtActiveStatus.Text & "',Getdate()) "
        Else
            Sql = " Insert into M_BannerMaster (bannerid,Imagepath,Remark,ActiveStatus,RectimeStamp) " & _
           "Values((Select IsNULL(Max(bannerid)+1,1) From M_BannerMaster),'" & FlNm & "',@Remarks,'" & txtActiveStatus.Text & "',GetDate() ) "

        End If

        Dim updateEffect As Integer = 0
        'updateEffect = objDAL.UpdateData(Sql, "@ProdDetail", Trim(TxtDtlDesc.Text))
        If objDAL.objSQlConnection.State = ConnectionState.Closed Then objDAL.objSQlConnection.Open()
        Dim Cmd As New SqlCommand(Sql, objDAL.objSQlConnection)
        Cmd.Parameters.AddWithValue("@Remarks", Trim(txtRemarks.Text))

        updateEffect = Cmd.ExecuteNonQuery()
        objDAL.objSQlConnection.Close()
        If String.IsNullOrEmpty(Request("bannerid")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
        ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If
        Session("ProdImg") = ""
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

End Class
