Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddCountry
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
        If String.IsNullOrEmpty(Request("CountryCode")) = False Then
            StateCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("CountryCode")))
        End If
        If Not Page.IsPostBack Then
            clearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("CountryCode")) = False Then
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
        Dim sql As String = "Select * From M_CountryMaster Where CID='" & StateCodeQS & "' AND " + objDAL.activeCondition
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtStateName.Text = Dt.Rows(0)("CountryName")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtStateCode.Text = Dt.Rows(0)("CountryCode")
            txtStdCode.Text = Dt.Rows(0)("StdCode")
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
        If String.IsNullOrEmpty(Request("CountryCode")) = False Then
            'Sql = " Update M_CountryMaster set RowStatus = 'N' Where CID = '" & StateCodeQS & "'"
            'Sql = Sql & " insert into M_CountryMaster "
            'Sql = Sql & " (CID,CountryName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,CountryCode,StdCode)"
            'Sql = Sql & " values('" & Val(StateCodeQS) & "','" & txtStateName.Text & "','" & txtRemarks.Text & "',"
            'Sql = Sql & " '" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',"
            'Sql = Sql & " '" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & txtStateCode.Text & "','" & txtStdCode.Text & "')"
            Sql = " Sp_AddEditCountry '" & Val(StateCodeQS) & "','" & txtStateName.Text & "','" & txtRemarks.Text & "',"
            Sql &= " '" & txtActiveStatus.Text & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','','','" & txtStateCode.Text & "',"
            Sql &= " '" & txtStdCode.Text & "'"
        Else
            'Sql = "Insert into M_CountryMaster (CID,CountryName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,CountryCode,StdCode)"
            'Sql &= "  Select Case When Max(CountryCode) Is Null Then '1' Else Max(CountryCode)+1 END as StateCode,'" & txtStateName.Text & "',"
            'Sql &= "  '" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',"
            'Sql &= "  '" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & txtStateCode.Text & "','" & txtStdCode.Text & "' From M_CountryMaster "
            Sql = " Sp_AddEditCountry '0','" & txtStateName.Text & "','" & txtRemarks.Text & "',"
            Sql &= " '" & txtActiveStatus.Text & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','','','" & txtStateCode.Text & "',"
            Sql &= " '" & txtStdCode.Text & "'"
        End If

        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("StateCode")) = False And updateEffect <> 0 Then
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
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
    End Sub
End Class
