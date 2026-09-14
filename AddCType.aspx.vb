Imports System.Data
Imports System.Data.SqlClient
Partial Class App_UI_Application_Pages_AddCType
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim CTypeIdQS As String
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
        If String.IsNullOrEmpty(Request("Type")) = False Then
            CTypeIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("Type")))
        End If
        If Not Page.IsPostBack Then
            clearAll()
            If Session("AStatus") = "OK" Then
                FillUser()
                If String.IsNullOrEmpty(Request("Type")) = False Then
                    BtnSave.Text = "Modify"
                    BindData()

                End If

            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From " + objDAL.tblCTypeMaster + " Where CTypeId='" & CTypeIdQS & "' AND " + objDAL.activeCondition
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtCType.Text = Dt.Rows(0)("CType")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtCTypeID.Text = Dt.Rows(0)("CTypeId")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            TxtEmail.Text = Dt.Rows(0)("ToUserEmail")

            DDlUser.SelectedValue = Dt.Rows(0)("ToUserId")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub

    Private Sub FillUser()
        Dim strquery As String = ""
        Dim tmpTable As DataTable
        strquery = "SELECT UserId,Username FROM M_Usermaster WHERE ACTIVESTATUS='Y' And RowStatus='Y'  ORDER BY UserId"
        'dbConnect.OpenConnection()
        tmpTable = New DataTable
        tmpTable = objDAL.GetData(strQuery)
        'dbConnect.Fill_Data_Tables(strQuery, tmpTable)
        With DDlUser
            .DataSource = tmpTable
            .DataValueField = "UserId"
            .DataTextField = "UserName"
            .DataBind()
            .SelectedIndex = 1
        End With
    End Sub


    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If
        If String.IsNullOrEmpty(Request("Type")) = False Then
            Sql = "Update " & objDAL.tblCTypeMaster & " SET RowStatus='N' Where CTypeId='" & CTypeIdQS & "';"
            Sql = Sql & " Insert into " & objDAL.tblCTypeMaster + "(CTypeId,CType,Remarks,ActiveStatus,LastModified,UserId,ToUserId,ToUserEmail) Values('" & Val(txtCTypeID.Text) & "',@CType,@Remarks,'" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Val(Session("UserID")) & "',@ToUserId,@ToUserEmail)"
        Else
            Sql = "Insert into " + objDAL.tblCTypeMaster + "(CTypeId,CType,Remarks,ActiveStatus,LastModified,UserId,RowStatus,ToUserId,ToUserEmail) Select Case When Max(CTypeId) Is Null Then '1' Else Max(CTypeId)+1 END as CTypeId,@CType,@Remarks,'" & txtActiveStatus.Text & "','New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Val(Session("UserID")) & "','Y',@ToUserId,@ToUserEmail From " & objDAL.tblCTypeMaster
        End If
        Dim Par As String = "@CType;@Remarks;@ToUserId;@ToUserEmail"
        Dim ParVal As String = Trim(txtCType.Text) & ";" & Trim(txtRemarks.Text) & ";" & Val(DDlUser.SelectedValue) & " ;" & Trim(TxtEmail.Text) & ""
        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql, Par, ParVal)

        If String.IsNullOrEmpty(Request("Type")) = False And updateEffect <> 0 Then
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
        txtCType.Text = ""
        txtCTypeID.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        TxtEmail.Text = ""
    End Sub
End Class
