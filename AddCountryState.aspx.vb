Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddState
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
        If String.IsNullOrEmpty(Request("StateCode")) = False Then
            StateCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("StateCode")))
        End If
        If Not Page.IsPostBack Then
            ClearAll()
            FillCountry()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("StateCode")) = False Then
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

    Private Sub FillCountry()
        Try
            Dim ds As DataSet = New DataSet()
            Dim str As String = " Sp_GetCountry 'Form','0',''"
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            If (ds.Tables(0).Rows.Count > 0) Then
                ddlcountry.DataSource = ds.Tables(0)
                ddlcountry.DataTextField = "Country"
                ddlcountry.DataValueField = "CID"
                ddlcountry.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From " + objDAL.tblStateMaster + " Where StateCode='" & StateCodeQS & "' AND " + objDAL.activeCondition
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            ddlcountry.SelectedValue = Dt.Rows(0)("CountryCode")
            txtStateName.Text = Dt.Rows(0)("StateName")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtStateCOde.Text = Dt.Rows(0)("StateCode")
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
        If String.IsNullOrEmpty(Request("StateCode")) = False Then
            Sql = "Update " & objDAL.tblStateMaster & " set RowStatus = 'N' Where StateCode = '" & txtStateCOde.Text & "'"
            Sql = Sql & " insert into " & objDAL.tblStateMaster & " (StateCode,StateName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,CountryCode) values('" & Val(txtStateCOde.Text) & "','" & txtStateName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & ddlcountry.SelectedValue & "')"
        Else
            Sql = "Insert into " & objDAL.tblStateMaster & " (StateCode,StateName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,CountryCode) Select Case When Max(StateCode) Is Null Then '1' Else Max(StateCode)+1 END as StateCode,'" & txtStateName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & ddlcountry.SelectedValue & "' From " & objDAL.tblStateMaster
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
