Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_Adddispatch
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim UserIdQS As String
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
        If String.IsNullOrEmpty(Request("DId")) = False Then
            UserIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("DId")))
        End If
        If Not Page.IsPostBack Then
            ClearAll()
            'BindType()

            ' Setting up values for GroupId and IP Address
            'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
            txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()

            If String.IsNullOrEmpty(Request("DId")) = False Then
                BtnSave.Text = "Modify"
                BindData()

            End If
        End If
    End Sub
    'Private Sub BindType()
    '    'Fill Group drop down list
    '    Dim qry1 As String = "Select * from " & objDAL.tblNewsTypeMaster & " Where ActiveStatus='Y' AND " & objDAL.activeCondition
    '    objModuleFun.FillCombo(qry1, DDLType, "Type", "Prefix")

    '    'If String.IsNullOrEmpty(Session("Prefix")) = False Then
    '    '    DDLType.SelectedValue = Session("Prefix")
    '    'End If
    'End Sub

    Private Sub BindData()
        'Dim sql As String = "Select * From " + objDAL.tbldispatchMaster + " Where DId='" & UserIdQS & "' AND " + objDAL.activeCondition
        Dim sql As String = "Select *,b.Kitname From " + objDAL.tbldispatchMaster + " as a inner join M_kitmaster as b on a.kitid=b.kitid Where a.DId='" & UserIdQS & "' AND a.Rowstatus='Y' "
        'Dim sql As String = ""
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            'DDLType.SelectedValue = Dt.Rows(0)("NType")
            txtDID.Text = Dt.Rows(0)("DId")
            txtIdno.Text = Dt.Rows(0)("Idno")
            'txtDetail.Text = Dt.Rows(0)("Re")
            txtFrmDate.Text = Format(Dt.Rows(0)("Date"), "dd-MMM-yyyy")
            'txtToDate.Text = Format(Dt.Rows(0)("ToDate"), "dd-MMM-yyyy")
            txtkitid.Text = Dt.Rows(0)("Kitid")
            txtpackage.Text = Dt.Rows(0)("Kitname")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub
    Private Sub ClearAll()
        txtIdno.Text = ""
        'txtDetail.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        txtDID.Text = ""
        txtFrmDate.Text = ""
        'txtToDate.Text = ""
    End Sub


    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Sql As String
        If rdblist.SelectedIndex = 0 Then
            txtActiveStatus.Text = "Y"
        Else
            txtActiveStatus.Text = "N"
        End If

        

        'Dim FrmDate As Date = txtFrmDate.Text
        'Dim Todate As Date = txtToDate.Text
        Dim FrmDate As String
        FrmDate = txtFrmDate.Text
        'Dim ToDate As String
        'ToDate = txtToDate.Text
        Try
            Dim Dt As DateTime = FrmDate
        Catch ex As Exception
            scrname = "<SCRIPT language='javascript'>alert('Check Date.. ');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Exit Sub
        End Try
        'Try
        '    Dim Dt As DateTime = ToDate
        'Catch ex As Exception
        '    scrname = "<SCRIPT language='javascript'>alert('Check End Date.. ');" & "</SCRIPT>"
        '    Me.RegisterStartupScript("MyAlert", scrname)
        '    Exit Sub
        'End Try
        ' DDLType.SelectedValue = "N"
        If String.IsNullOrEmpty(Request("DId")) = False Then

            Sql = "Update " & objDAL.tbldispatchMaster & " SET RowStatus='N' Where DId='" & UserIdQS & "';"
            Sql = Sql & " Insert into " & objDAL.tbldispatchMaster + "(DId,Idno,Date,Remarks,ActiveStatus,RowStatus,kitid) Values('" & Val(txtDID.Text) & "','" & txtIdno.Text & "','" & FrmDate & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Y','" & txtkitid.Text & "') "

        Else
            Sql = " Insert into " & objDAL.tbldispatchMaster + "(DId,Idno,Date,Remarks,ActiveStatus,RowStatus,kitid) Select Case When Max(DId) Is Null Then '1' Else Max(DId)+1 END as DId,'" & txtIdno.Text & "','" & FrmDate & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','Y','" & txtkitid.Text & "' From " & objDAL.tbldispatchMaster
        End If

        Dim updateEffect As Integer = 0
        updateEffect = objDAL.UpdateData(Sql)

        If String.IsNullOrEmpty(Request("DId")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
            'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record Updated successfully !!!');", True)
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "update", scrname, False)
        ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record Save successfully !!!');", True)
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "save", scrname, False)
        Else
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
            'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record not saved successfully !!!');", True)
            'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "do not save", scrname, False)
        End If

        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        ClearAll()

    End Sub
    Private Function Get_IDNo() As String
        Try


            Dim IdNo As String = ""
            'Dim dbConnect As New cls_DataAccess(Application("Connect"))
            'dbConnect.OpenConnection()
            'Dim dRead As SqlDataReader
            'Dim cmd As SqlCommand
            'cmd = New SqlCommand("select IdNo,MemFirstName + ' ' + MemLastName as MemName " & _
            '                                 " from M_MemberMaster WHERE idno='" & txtIdno.Text & "'", dbConnect.cnnObject)
            'dRead = cmd.ExecuteReader

            'Dim sql As String = "select IdNo,MemFirstName + ' ' + MemLastName as MemName from M_MemberMaster WHERE idno='" & txtIdno.Text & "'"
            Dim sql As String = "select a.IdNo,a.MemFirstName + ' ' + a.MemLastName as MemName,b.kitid,b.kitname from M_MemberMaster as a inner join M_kitmaster as b on a.kitid=b.kitid WHERE idno='" & txtIdno.Text & "'"
            Dim dtData As New DataTable
            dtData = objDAL.GetData(sql)
            If dtData.Rows.Count > 0 Then
                IdNo = dtData.Rows(0)("IdNo")
                lblRefralNm.Text = dtData.Rows(0)("MemName")
                txtpackage.Text = dtData.Rows(0)("KitName")
                txtkitid.Text = dtData.Rows(0)("KitId")
                Return IdNo
            Else
                scrname = "<SCRIPT language='javascript'>alert('Idno does not exist. ');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

                Exit Function
            End If
            'If dRead.Read Then
            '    IdNo = dRead("IdNo")
            '    lblRefralNm.Text = dRead("MemName")
            'End If
            'dRead.Close()
            'Return IdNo
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            'objGen.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Function

    Protected Sub txtIdno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIdno.TextChanged
        If txtIdno.Text <> "" Then
            Get_IDNo()
        Else
            scrname = "<SCRIPT language='javascript'>alert('Idno does not exist. ');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)

            Exit Sub
        End If
    End Sub
End Class
