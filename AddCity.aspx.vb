Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddCity
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim CityCodeQS As String
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
        If String.IsNullOrEmpty(Request("CityCode")) = False Then
            CityCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("CityCode")))

        End If

        If Not Page.IsPostBack Then
            ClearAll()

            ' Setting up values for GroupId and IP Address
            txtStateCode.Text = Session("StateCode")
            txtStateName.Text = Session("StateName")
            'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
            txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
            If String.IsNullOrEmpty(Request("CityCode")) = False Then
                BtnSave.Text = "Modify"
                BindData()
            End If
        End If
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select a.StateName as StateName,b.* From " & objDAL.tblStateMaster & " as a," & objDAL.tblCityStateMaster & " as b Where b.CityCode='" & Val(CityCodeQS.ToString()) & "' AND a.StateCode=b.StateCode AND b.RowStatus='Y'"
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtStateCode.Text = Dt.Rows(0)("StateCode")
            txtStateName.Text = Dt.Rows(0)("StateName")
            txtCityName.Text = Dt.Rows(0)("CityName")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtCityCode.Text = Dt.Rows(0)("CityCode")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub

    Private Sub ClearAll()
        txtStateName.Text = ""
        txtStateCode.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        txtCityCode.Text = ""
        txtCityName.Text = ""
    End Sub


    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
        If String.IsNullOrEmpty(Request("CityCode")) = False Then
            Sql = "Update " & objDAL.tblCityStateMaster & " SET RowStatus='N' Where CityCode='" & CityCodeQS & "';"
            Sql = Sql & " insert into " & objDAL.tblCityStateMaster & " (CityCode,StateCode,CityName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) values('" & Val(txtCityCode.Text) & "','" & Val(txtStateCode.Text) & "','" & txtCityName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y') "
        Else
            Sql = " insert into " & objDAL.tblCityStateMaster & " (CityCode,StateCode,CityName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) Select Case When Max(CityCode) Is Null Then '1' Else Max(CityCode)+1 END as CityCode,'" & Val(txtStateCode.Text) & "','" & txtCityName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y' From " & objDAL.tblCityStateMaster
        End If

        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("CityCode")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
        ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        End If

        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub
End Class
