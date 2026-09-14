Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class App_UI_Application_Pages_AddSeminarNew
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim StateCodeQS As String
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
        If String.IsNullOrEmpty(Request("IId")) = False Then
            StateCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("IId")))
        End If
        If Not Page.IsPostBack Then
            Session("Img") = ""
            ClearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("IId")) = False Then
                    BtnSave.Text = "Modify"
                    BindData()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        ' txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        'txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From M_SeminarMaster Where IId='" & StateCodeQS & "'"
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtStateName.Text = Dt.Rows(0)("ImageType")
            ' txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtStateCOde.Text = Dt.Rows(0)("IId")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            Session("Img") = Dt.Rows(0)("Image")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        Dim IId As String
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If


        Dim FlNm As String = ""
        Dim flnm2 As String = ""
        If FlUpld1.HasFile Then
            flnm2 = Format(Now, "yyMMddhhmmssfff") & Path.GetExtension(FlUpld1.PostedFile.FileName)
            FlNm = "images/Seminar/" & Format(Now, "yyMMddhhmmssfff") & Path.GetExtension(FlUpld1.PostedFile.FileName)
            FlUpld1.PostedFile.SaveAs(Server.MapPath("~/images/Seminar/") & flnm2)
        Else
            If String.IsNullOrEmpty(Request("banner_id")) = False Then
                FlNm = Session("BannerImg")
            Else
                scrname = "<SCRIPT language='javascript'>alert('Please Upload Banner Image. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
                Exit Sub
            End If
        End If


        If String.IsNullOrEmpty(Request("IId")) = False Then
            Sql = " insert into TempSeminarMaster Select *,'" & Val(Session("UserID")) & "',GetDate(),'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' from M_SeminarMaster where IId='" & StateCodeQS & "'"
            Sql = Sql & " Update M_SeminarMaster set ActiveStatus='" & txtActiveStatus.Text & "',ImageType='" & txtStateName.Text & " ',Image = '" & FlNm & "' Where IId = '" & StateCodeQS & "'"

        Else
            Dim str As String = "select IsNull (Max(IId+1),1) as IId from M_SeminarMaster"
            Dim dt As New DataTable
            dt = objDAL.GetData(str)
            If (dt.Rows.Count > 0) Then
                IId = dt.Rows(0)("IId")

            End If
            Sql = "Insert into M_SeminarMaster (IId,ImageType,ActiveStatus,RectimeStamp,Image)Values('" & IId & "','" & txtStateName.Text & "','" & txtActiveStatus.Text & "',GetDate(),'" & FlNm & "')"
        End If

        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("IId")) = False And updateEffect <> 0 Then
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
        txtStateName.Text = ""
        txtStateCOde.Text = ""
        'txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        'txtIPAdrs.Text = ""
    End Sub
End Class
