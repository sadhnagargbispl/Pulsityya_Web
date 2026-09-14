Imports System.Data

Partial Class AddCountryBank
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim BankCodeQS As String
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
        If String.IsNullOrEmpty(Request("BankCode")) = False Then
            BankCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("BankCode")))
        End If
        If Not Page.IsPostBack Then
            ClearAll()
            If Session("AStatus") = "OK" Then
                GetCountry("Form")
                If String.IsNullOrEmpty(Request("BankCode")) = False Then
                    BtnSave.Text = "Modify"
                    BindData()
                End If
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From " + objDAL.tblBankMaster + " Where BankCode='" & BankCodeQS & "' AND " + objDAL.activeCondition
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            DDlCountry.SelectedValue = Dt.Rows(0)("CountryCode")
            txtBankName.Text = Dt.Rows(0)("BankName")
            txtIFSCode.Text = Dt.Rows(0)("IFSCode")
            txtAccountNo.Text = Dt.Rows(0)("AcNo")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtBankCode.Text = Dt.Rows(0)("BankCode")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub
    Private Sub GetCountry(ByVal cond As String)
        Try
            Dim countryid As String = ""
            If cond = "Form" Then
                countryid = "0"
            Else
                countryid = ddlCountry.SelectedValue

            End If
            Dim ds As DataSet = New DataSet()
            Dim str As String = " Sp_GetCountry '" & cond & "','" & countryid & "',''"
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            If (ds.Tables(0).Rows.Count > 0) Then
                ddlCountry.DataSource = ds.Tables(0)
                If cond = "Form" Then
                    ddlCountry.DataTextField = "Country"
                    ddlCountry.DataValueField = "Cid"
                    ddlCountry.DataBind()
                End If

            Else

                ddlCountry.Items.Insert(0, "---Select Country---")
            End If


        Catch ex As Exception

        End Try
    End Sub
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
        If String.IsNullOrEmpty(Request("BankCode")) = False Then
            Sql = "Update " + objDAL.tblBankMaster + " set RowStatus = 'N' Where BankCode = '" + txtBankCode.Text + "' "
            Sql = Sql & " insert into " & objDAL.tblBankMaster & " (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,CountryCode) " & _
            " values('" & Val(txtBankCode.Text) & "','" & txtBankName.Text & "','" & txtAccountNo.Text & "','" & txtIFSCode.Text & "','" & txtRemarks.Text & "'," & _
            " '" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & DDlCountry.SelectedValue & "') "
        Else
            Sql = "insert into " & objDAL.tblBankMaster & " (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,CountryCode) " & _
            " Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" & txtBankName.Text & "','" & txtAccountNo.Text & "','" & txtIFSCode.Text & "'," & _
            " '" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y','" & DDlCountry.SelectedValue & "' From " & objDAL.tblBankMaster
        End If

        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("BankCode")) = False And updateEffect <> 0 Then
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
        txtBankCode.Text = ""
        txtBankName.Text = ""
        txtAccountNo.Text = ""
        txtIFSCode.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        DDlCountry.SelectedValue = 0
    End Sub
End Class
