Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Enable_disable

    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral
    Dim VId As Integer

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member / Visiting Verify"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try



            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                If Session("AStatus") = "OK" Then
                    If Request.QueryString.HasKeys Then
                        'If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        '    txtMemId.Text = Request.QueryString("key")

                        '    ChkMem.Checked = True
                        '    BindData(" AND IDNo='" & Request.QueryString("key") & "'")
                        'End If
                    Else

                    End If
                    'BindData()
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BindData()
        Try

            Dim dt As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String
            sql = " insert into EnMenu(MenuName,PageName,Activity,datetimes,OnSelected,menuId,StatusID)Values" & _
               "('" & ("Add Beneficiary Detail") & "','Beneficiary Detail','Enable Menu',GETDATE(),'" & ("Enable_disable.aspx") & "','" & ("122") & "','" & ("1") & "')"
            'objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim i As Integer = objDAL.SaveData(sql)
            'If i <> 0 Then
            '    '   divBank.Visible = False
            '    ''scrname = "<SCRIPT language='javascript'>alert('Profile Successfully Updated');" & "</SCRIPT>"
            '    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Profile Successfully Updated');location.replace('ProfileUp.aspx');", True)
            'Else
            'scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" & "</SCRIPT>"
            'Me.RegisterStartupScript("MyAlert", scrname)
            'FillDetail()
            Exit Sub
            'End If
            'ddlstate.DataSource = dt
            'ddlstate.DataTextField = "MenuName"
            'ddlstate.DataValueField = "ID"
            'ddlstate.DataBind()
            'ddlstate.Items.Insert(0, "--Select Menu--")
        Catch ex As Exception

        End Try
    End Sub
    'Public Sub BindData(Optional ByVal Condition As String = "")
    '    Try


    '        'Dim sql As String = ""
    '        'sql = " select a.VId,a.IDNo,a.Name,a.city,a.State,b.Id,"
    '        'sql &= "(select count(*)as cnt from TrnVisitingMaster b where a.Vid = b.Vid) as TotalMember,b.Name As MemName,b.Age,b.Pincode,b.CityName,b.stateName,Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,Isnull(b.VerifyBy,'') as VerifyBy ,"
    '        'sql &= " (Case When b.ActiveStatus = 'N' Then 'Pending' "
    '        'sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
    '        'sql &= " When b.ActiveStatus = 'Y' Then 'Approved'  End ) As  Status,"
    '        'sql &= " Replace(Convert(Varchar,b.ReqRectimestamp,106),' ','-') As rectimestamp"
    '        'sql &= "  From M_VisitingMaster as a,TrnVisitingMaster as b  "
    '        'sql &= " Where a.VId = b.VId "
    '        'If (Session("CompID") = "1055") Then
    '        Dim dt As DataTable = New DataTable
    '        Dim strQuery As String = "select * from M_CompWiseWebMenuMaster where CompanyId='" & Session("CompID") & "'"
    '        dt = SqlHelper.ExecuteDataset(CommandType.Text, strQuery).Tables(0)
    '        'Dim sql As String = "select * from M_CompWiseWebMenuMaster where CompanyId='" & Session("CompID") & "'"
    '        'dtData = New DataTable
    '        'dtData = objDAL.GetData(sql)
    '        'GvData.DataSource = dtData
    '        'GvData.DataBind()
    '        'Session("GData") = dtData
    '        'If dtData.Rows.Count > 0 Then
    '        '    BtnVerifiy.Enabled = True
    '        '    BTnUnVerification.Enabled = True
    '        '    'BtnExport.Enabled = True
    '        'Else
    '        '    BtnVerifiy.Enabled = False
    '        '    BTnUnVerification.Enabled = False
    '        '    'BtnExport.Enabled = False
    '        'End If
    '        'End If


    '        'If ChkMem.Checked = True Then

    '        '    If txtMemId.Text <> "" Then
    '        '        sql &= " Where a.IDNo = '" & txtMemId.Text & "'"
    '        '    End If
    '        'End If
    '        'If DDlVerify.SelectedValue <> "S" Then
    '        '    If (DDlVerify.SelectedValue <> "Y") Then
    '        '        sql &= " And b.ActiveStatus = '" & DDlVerify.SelectedValue & "'"
    '        '    Else
    '        '        sql &= " And b.ActiveStatus = '" & DDlVerify.SelectedValue & "'"
    '        '    End If
    '        'End If


    '    Catch ex As Exception

    '    End Try
    'End Sub


    'Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
    '    Try
    '        Dim dtTemp As New DataTable

    '        Dim sql As String = ""
    '        sql = " select a.IDNo,a.Name,a.State,a.city,"
    '        sql &= "b.Name As [Member Name],b.Age,b.Pincode,b.CityName,b.stateName, Isnull(Convert(Varchar,b.VerifyDate),'') as VerifyDate ,"
    '        sql &= " (Case When b.ActiveStatus = 'N' Then 'Pending' "
    '        sql &= " When b.ActiveStatus = 'R' Then 'Rejected' "
    '        sql &= " When b.ActiveStatus = 'Y' Then 'Approved'  End ) As  Status"
    '        'sql &= " Replace(Convert(Varchar,b.ReqRectimestamp,106),' ','-') As rectimestamp"
    '        sql &= "  From M_VisitingMaster as a,trnvisitingMaster as b  "
    '        sql &= " Where a.Vid = b.Vid"
    '        If ChkMem.Checked = True Then

    '            If txtMemId.Text <> "" Then
    '                sql &= " And a.IDNo = '" & txtMemId.Text & "'"
    '            End If
    '        End If
    '        If DDlVerify.SelectedValue <> "S" Then
    '            sql &= " And b.ActiveStatus = '" & DDlVerify.SelectedValue & "'"
    '        End If




    '        Dim dg As New DataGrid
    '        dtTemp = New DataTable
    '        dtTemp = objDAL.GetData(sql)

    '        dg.DataSource = dtTemp
    '        dg.DataBind()

    '        ExportToExcel("VisitList.xls", dg)

    '    Catch ex As Exception
    '        Response.Write(ex.Message & "Error In Exporting File")
    '    End Try

    'End Sub
    'Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
    '    Dim sw As New System.IO.StringWriter
    '    Dim htw As System.Web.UI.HtmlTextWriter
    '    Response.Clear()
    '    Response.Buffer = True
    '    Response.ContentType = "application/vnd.xls"
    '    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
    '    Response.Charset = ""
    '    dg.EnableViewState = False
    '    htw = New HtmlTextWriter(sw)
    '    dg.RenderControl(htw)
    '    Response.Write(sw.ToString())
    '    Response.End()
    'End Sub
    Private Sub BindData1()
        Try

            Dim dt As New DataTable
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String
            sql = " insert into EnMenu(MenuName,PageName,Activity,datetimes,OnSelected,menuId,StatusID)Values" & _
                "('" & ("Add Beneficiary Detail") & "','Beneficiary Detail','Enable Menu',GETDATE(),'" & ("Enable_disable.aspx") & "','" & ("122") & "','" & ("0") & "')"
            Dim i As Integer = objDAL.SaveData(sql)
            Exit Sub
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click


        BindData()
    End Sub

    'Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
    '    If e.Row.RowType = DataControlRowType.Header Then
    '        DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
    '    End If
    'End Sub

    Protected Sub BtnVerifiy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVerifiy.Click
        Try
            Dim objDAL1 As New DAL(Application("sConnect"))
            Dim str As String = ""
            Dim scrname As String
            Dim Qry As String = ""
            Dim Condition As String = ""
            str = str & "Update M_CompWiseWebMenuMasterDis Set ActiveStatus = 'Y', RowStatus = 'Y' where MenuId='122' AND CompanyId = '" & HttpContext.Current.Session("CompID") & "' "
            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL1.UpdateData(str, 1)
            End If
            If i <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert(' Enable successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            Else
                scrname = "<SCRIPT language='javascript'>alert(' Enable unsuccessfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            End If
            BindData()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnUnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnVerify.Click
        Try
            Dim objDAL1 As New DAL(Application("sConnect"))
            Dim str As String = ""
            Dim scrname As String
            Dim Condition As String = ""
            str = str & "Update M_CompWiseWebMenuMasterDis Set ActiveStatus = 'N', RowStatus = 'N' where MenuId='122' AND CompanyId = '" & HttpContext.Current.Session("CompID") & "' "
            Dim i As Integer = 0
            If str <> "" Then
                i = objDAL1.UpdateData(str, 1)
            End If
            If i <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert(' Disable successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            Else
                scrname = "<SCRIPT language='javascript'>alert(' Disable unsuccessfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)


            End If
            BindData1()

        Catch ex As Exception

        End Try

    End Sub



    'Protected Sub FillDetail()
    '    Try


    '        Dim s As String = ""
    '        s = "Select * from M_KYCReject where activeStatus='Y'"
    '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        Dim Dt As DataTable
    '        Dt = New DataTable
    '        Dt = objDAL.GetData(s)
    '        If Dt.Rows.Count > 0 Then
    '            DDlREason.DataValueField = "KId"
    '            DDlREason.DataTextField = "reason"
    '            DDlREason.DataSource = Dt
    '            DDlREason.DataBind()
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class




