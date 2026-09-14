Imports System.Data
Imports System.Data.SqlClient
Partial Class AddCityNew
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim CityCode As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

        
            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If String.IsNullOrEmpty(Request("CityCode")) = False Then
                CityCode = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("CityCode")))

            End If
            If Not Page.IsPostBack Then
                If Session("AStatus") = "OK" Then
                    FillState()
                    If String.IsNullOrEmpty(Request("CityCode")) = False Then
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

    Private Sub FillDistrict(ByVal StateCode As Integer)
        Try
            Dim ds As DataSet = New DataSet()
            Dim str As String = " sp_FillDistrict '" & StateCode & "'"
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            If (ds.Tables(0).Rows.Count > 0) Then
                ddlDistrictCode.DataSource = ds.Tables(0)
                ddlDistrictCode.DataTextField = "DistrictName"
                ddlDistrictCode.DataValueField = "DistrictCode"
                ddlDistrictCode.DataBind()

            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub FillState()
        Try
            Dim ds As DataSet = New DataSet()
            Dim str As String = " sp_FillState"
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

        

            'Dim sql As String = "Select * From " + objDAL.tblCityStateMaster + " Where CityCode = '" & DistrictCode & "'"
            Dim sql As String = " Exec Sp_FillCityDataBind '" & CityCode & "'"
            Dt = New DataTable
            Dt = objDAL.GetData(sql)
            If Dt.Rows.Count > 0 Then
                ddlState.SelectedValue = Dt.Rows(0)("StateCode")
                FillDistrict(ddlState.SelectedValue)
                ddlDistrictCode.SelectedValue = Dt.Rows(0)("DistrictCode")
                txtCityCode.Text = Dt.Rows(0)("CityCode")
                txtCityName.Text = Dt.Rows(0)("CityName")
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
            End If
            If ddlDistrictCode.SelectedValue = 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Please Select District! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If

            If rdblist.SelectedIndex = 0 Then
                txtActiveStatus.Text = "Y"
            Else
                txtActiveStatus.Text = "N"
            End If
            If String.IsNullOrEmpty(Request("CityCode")) = False Then
                ''Sql = "Update " & objDAL.tblCityStateMaster & " set CityName='" & txtCityName.Text.ToUpper & "'  Where CityCode = '" & ddlDistrictCode.Text & "' AND  ActiveStatus = '" & rdblist.Text & "'"

                Sql = " exec Sp_AddEditCityMaster '" & txtCityCode.Text & "','" & txtCityName.Text.ToUpper & "',"
                Sql &= " '" & ddlDistrictCode.SelectedValue & "','" & txtActiveStatus.Text & "'"
            Else

                ''Sql = "Insert into " & objDAL.tblCityStateMaster & " (CityName, DistrictCode,ActiveStatus) values ('" & txtCityName.Text.ToUpper & "','" & ddlDistrictCode.SelectedValue & "','" & txtActiveStatus.Text & "')"
                Sql = " exec Sp_AddEditCityMaster 0,'" & txtCityName.Text.ToUpper & "',"
                Sql &= " '" & ddlDistrictCode.SelectedValue & "','" & txtActiveStatus.Text & "'"
            End If
            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Sql)

            If String.IsNullOrEmpty(Request("CityCode")) = False And updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated');" & "</SCRIPT>"
            ElseIf updateEffect <> 0 Then
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
            'DropDownList1.SelectedItem.Text = ""
            ddlState.SelectedIndex = -1
            ddlDistrictCode.SelectedIndex = -1
            txtCityCode.Text = ""
            txtCityName.Text = ""
            txtActiveStatus.Text = ""
            txtIPAdrs.Text = ""
        Catch ex As Exception

        End Try
    End Sub

    Private Function CheckIdno() As String
        Try
            Dim s As String = ""
            Dim sql As String = ""
            sql = " select CityName, ActiveStatus from m_CityStateMaster  where CityName ='" & txtCityName.Text & "'"
            '" On a.Formno=b.Formno where idno='" & txtDistrictName.Text.Trim & "'"
            Dim Dt As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dt = objDAL.GetData(sql)
            If Dt.Rows.Count > 0 Then
                ''txtCityName.Text = ""
                scrname = "<SCRIPT language='javascript'>alert(' This City Name Already insert ! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                Exit Function
            End If
        Catch ex As Exception

        End Try
    End Function

    Protected Sub ddlState_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlState.TextChanged
        Try
            Dim sql As String = ""
            'sql = "select a.Statename,b.DistrictName,c.CityName,a.StateCode,b.DistrictCode" & _
            ' " ,c.CityCode from M_STateDivMaster as a with( NoLock) Inner Join M_DistrictMaster as b " & _
            ' " with( NoLock) On a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' " & _
            ' " Inner Join  M_CityStatemaster as c with( NoLock)  On b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' " & _
            ' "where  a.StateCode='" & ddlState.SelectedValue & "'"
            sql = " Exec  sp_FillDistrict '" & ddlState.SelectedValue & "'"

            Dim Dt As DataTable
            Dt = New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dt = objDAL.GetData(sql)
            If Dt.Rows.Count > 0 Then
                ddlDistrictCode.DataSource = Dt
                ddlDistrictCode.DataTextField = "DistrictName"
                ddlDistrictCode.DataValueField = "DistrictCode"
                ddlDistrictCode.DataBind()
                ddlDistrictCode.SelectedIndex = 0
            Else
                ddlDistrictCode.Items.Clear()
            End If
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub txtCityName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCityName.TextChanged
        If ddlState.SelectedValue = 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Please Select State! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        End If
        If ddlDistrictCode.SelectedValue = 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Please Select District! ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        End If
        CheckIdno()
    End Sub
End Class
