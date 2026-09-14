Imports System.Data

Partial Class App_UI_Application_Pages_AddBank
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim BankCodeQS As String
    Dim Sql As String = ""
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
        If String.IsNullOrEmpty(Request("BankCode")) = False Then
            BankCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("BankCode")))
        End If
        If Not Page.IsPostBack Then
            clearAll()
            If Session("AStatus") = "OK" Then
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

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        'Dim Sql1 As String
        'Dim dt12 As DataTable
        'Sql1 = " Select * From " + objDAL.tblBankMaster + " Where BankName='" & txtBankName.Text & "' AND " + objDAL.activeCondition
        'objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'dt12 = New DataTable
        'dt12 = objDAL.GetData(Sql1)
        'If dt12.Rows.Count > 0 Then
        '    scrname = "<SCRIPT language='javascript'>alert('Bank Name already exist!!');" & "</SCRIPT>"
        '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        '    txtBankName.Text = ""
        'End If
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
        If String.IsNullOrEmpty(Request("BankCode")) = False Then
            Sql = "Update " + objDAL.tblBankMaster + " set BankName='" & txtBankName.Text & "',AcNo='" & txtAccountNo.Text & "',IFSCode='" & txtIFSCode.Text & "',Remarks='" & txtRemarks.Text & "' Where BankCode = '" + txtBankCode.Text + "' "
            'Sql = "Update " + objDAL.tblBankMaster + " set RowStatus = 'N' Where BankCode = '" + txtBankCode.Text + "' "
            'Sql = Sql & " insert into " & objDAL.tblBankMaster & " (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) values('" & Val(txtBankCode.Text) & "','" & txtBankName.Text & "','" & txtAccountNo.Text & "','" & txtIFSCode.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y') "
        Else
            Sql = "insert into " & objDAL.tblBankMaster & " (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" & txtBankName.Text & "','" & txtAccountNo.Text & "','" & txtIFSCode.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y' From " & objDAL.tblBankMaster
        End If

        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        'If String.IsNullOrEmpty(Request("BankCode")) = False And updateEffect <> 0 Then
        '    scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
        'ElseIf updateEffect <> 0 Then
        '    scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
        'Else
        '    scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
        'End If
        'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        If Not String.IsNullOrEmpty(Request("BankCode")) AndAlso updateEffect <> 0 Then
            scrname = "<script type='text/javascript'>alert('Successfully Updated!!');if (parent && parent.hs) {parent.location.reload(); parent.hs.close();}</script>"
            ElseIf updateEffect <> 0 Then
            scrname = "<script type='text/javascript'>alert('Save Successfully!!');if (parent && parent.hs) {parent.location.reload(); parent.hs.close();}</script>"
        Else
            scrname = "<script type='text/javascript'>alert('Data not saved Successfully!!');</script>"
        End If
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "ClosePopup", scrname, False)

    End Sub

    Private Sub ClearAll()
        txtBankCode.Text = ""
        txtBankName.Text = ""
        txtAccountNo.Text = ""
        txtIFSCode.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
    End Sub

    Protected Sub txtBankName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBankName.TextChanged
        Dim dt1 As DataTable
        If BtnSave.Text = "Modify" Then
        Else
            Sql = " Select * From " + objDAL.tblBankMaster + " Where BankName='" & txtBankName.Text & "' AND " + objDAL.activeCondition
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt1 = New DataTable
            dt1 = objDAL.GetData(Sql)
            If dt1.Rows.Count > 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Bank Name already exist!!');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                txtBankName.Text = ""
            End If
        End If
       
    End Sub
End Class
