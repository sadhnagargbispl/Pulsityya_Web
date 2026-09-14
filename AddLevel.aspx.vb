Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddLevel
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim LevelCodeQS As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("AId")) = False Then
            LevelCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("AId")))
        End If
        If Not Page.IsPostBack Then
            ClearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("AId")) = False Then
                    BtnSave.Text = "Modify"
                    BindData()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        ' txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From M_LevelMaster Where AId='" & LevelCodeQS & "'"
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtLevelName.Text = Dt.Rows(0)("LevelName")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtLevelCOde.Text = Dt.Rows(0)("AId")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
        If String.IsNullOrEmpty(Request("Aid")) = False Then
            Sql = " Insert into TempLevelMaster Select *,'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',GetDate() from M_Levelmaster where AId='" & txtLevelCOde.Text & "'"
            Sql = Sql & "Update M_LevelMaster set LevelName='" & txtLevelName.Text & "', Remarks='" & txtRemarks.Text & "',ActiveStatus='" & txtActiveStatus.Text & "' Where StateCode = '" & txtLevelCOde.Text & "'"
            'Sql = Sql & " insert into M(StateCode,StateName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,CountryCode) values('" & Val(txtLevelCOde.Text) & "','" & txtStateName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','1')"
        Else
            Sql = "Insert into M_LevelMaster(LevelName,Remarks,ActiveStatus,UserId,RecTimeStamp) values('" & txtLevelName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','" & Val(Session("UserID")) & "',Getdate())"
        End If

        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("Aid")) = False And updateEffect <> 0 Then
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
        txtLevelName.Text = ""
        txtLevelCOde.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
    End Sub
End Class
