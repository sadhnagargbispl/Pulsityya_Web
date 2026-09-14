Imports System.Data
Imports System.Data.SqlClient
Partial Class AddDistrict
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
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If String.IsNullOrEmpty(Request("DistrictCode")) = False Then
                StateCodeQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("DistrictCode")))
            End If
            If Not Page.IsPostBack Then
                ClearAll()
                FillCountry()
                If Session("AStatus") = "OK" Then
                    If String.IsNullOrEmpty(Request("DistrictCode")) = False Then
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
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillCountry()
        Try
            Dim ds As DataSet = New DataSet()
            Dim str As String = " sp_GetDistrictDrop "
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            If (ds.Tables(0).Rows.Count > 0) Then
                ddlState.DataSource = ds.Tables(0)
                ddlState.DataTextField = "StateName"
                ddlState.DataValueField = "StateCode"
                ddlState.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BindData()
        Try

        
            Dim sql As String = "Select * From " + objDAL.tblDistrictMaster + " Where DistrictCode='" & StateCodeQS & "' AND " + objDAL.activeCondition
            Dt = New DataTable
            Dt = objDAL.GetData(sql)
            If Dt.Rows.Count > 0 Then

                ddlState.SelectedValue = Dt.Rows(0)("StateCode")
                txtDistrictName.Text = Dt.Rows(0)("DistrictName")

                txtStateCOde.Text = Dt.Rows(0)("DistrictCode")
                txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
                If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                    rdblist.SelectedIndex = 0
                Else
                    rdblist.SelectedIndex = 1
                End If


            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Try

        
            Dim Sql As String
            If ddlState.SelectedValue = 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Please Select State! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If

            If (txtDistrictName.Text = "") Then
                scrname = "<SCRIPT language='javascript'>alert('Please Enter District Name! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If

            If rdblist.SelectedIndex = 0 Then
                txtActiveStatus.Text = "Y"
            Else
                txtActiveStatus.Text = "N"
            End If
            If String.IsNullOrEmpty(Request("DistrictCode")) = False Then
                ''Sql = "Update " & objDAL.tblDistrictMaster & " set DistrictName='" & txtDistrictName.Text.ToUpper() & "'  Where DistrictCode = '" & txtStateCOde.Text & "' AND  ActiveStatus = '" & rdblist.Text & "'"
                Sql = " EXEC Sp_AddEditDistrictMaster '" & txtStateCOde.Text & "','" & txtDistrictName.Text.ToUpper() & "',"
                Sql &= " '" & ddlState.SelectedValue & "','" & txtActiveStatus.Text & "'"

            Else
                ''Sql = "Insert into " & objDAL.tblDistrictMaster & " (DistrictName, StateCode,ActiveStatus) values ('" & txtDistrictName.Text.ToUpper() & "','" & ddlState.SelectedValue & "','" & txtActiveStatus.Text & "')"
                Sql = " EXEC Sp_AddEditDistrictMaster 0,'" & txtDistrictName.Text.ToUpper() & "',"
                Sql &= " '" & ddlState.SelectedValue & "','" & txtActiveStatus.Text & "'"
            End If
            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Sql)

            If String.IsNullOrEmpty(Request("DistrictCode")) = False And updateEffect > 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated');" & "</SCRIPT>"
            ElseIf updateEffect > 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully');" & "</SCRIPT>"
            End If

            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ClearAll()
        Try

        
            txtDistrictName.Text = ""
            txtStateCOde.Text = ""

            txtActiveStatus.Text = ""
            txtIPAdrs.Text = ""
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub txtDistrictName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDistrictName.TextChanged
        Try

            If ddlState.SelectedValue = 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Please Select State! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
            CheckIdno()
        Catch ex As Exception

        End Try
    End Sub
    Private Function CheckIdno() As String
        Try
            Dim s As String = ""
            Dim sql As String = ""
            sql = " select DistrictName, ActiveStatus from M_DistrictMaster  where DistrictName ='" & txtDistrictName.Text.Trim & "'"
            '" On a.Formno=b.Formno where idno='" & txtDistrictName.Text.Trim & "'"
            Dim Dt As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            Dt = objDAL.GetData(sql)
            If Dt.Rows.Count > 0 Then
                txtDistrictName.Text = ""
                scrname = "<SCRIPT language='javascript'>alert(' This District Name Already insert ! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                Exit Function
            End If
        Catch ex As Exception

        End Try
    End Function
End Class
