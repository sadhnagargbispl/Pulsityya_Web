Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Net
Imports System.Globalization
Partial Class App_UI_Application_Pages_ActivateID
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim dtIDList As New DataTable
    Dim dbConnect As cls_DataAccess
    Dim objGen As clsGeneral = New clsGeneral

#Region "Page Events"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        divSingle.Visible = False
        divMultiple.Visible = False
        BtnActivate.Visible = False
        BtnActivateSingle.Visible = False
        lblError.Visible = False
        lblrecordcount.Text = ""

        If Not Page.IsPostBack Then
            Fill_KitCombo()
            FillPaymode()
            txtStartDate.Enabled = False : txtEndDate.Enabled = False
            txtMemberId.Text = ""
            lblError.Text = ""
            If Request.QueryString.HasKeys Then
                If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                    rdblistChoice.SelectedValue = "single"
                    txtMemberId.Text = Request.QueryString("key").ToString
                    ShowDetail()
                End If
            Else
                txtMemberId.Text = ""
                lblError.Text = ""
            End If
        End If

        If String.Equals(rdblistChoice.SelectedValue.ToLower(), "single") = True Then
            divSingle.Visible = True
        Else
            divMultiple.Visible = True
        End If
    End Sub
#End Region

#Region "Fill Kit Combo"

    Private Sub FillPaymode()
        Dim strQuery As String = "SELECT * FROM M_PayModeMaster WHERE ActiveStatus='Y'"
        Dim tmpTable As New DataTable
        tmpTable = objDAL.GetData(strQuery)
        With DdlPaymode
            .DataSource = tmpTable
            .DataValueField = "PID"
            .DataTextField = "Paymode"
            .DataBind()
        End With

    End Sub
    Private Sub Fill_KitCombo()
        Dim Sql As String = "Select * FROM M_KitMaster WHERE AllowTopup='Y'"
        Dim Dt As New DataTable
        Dt = objDAL.GetData(Sql)
        With CmbKit
            .DataSource = Dt
            .DataTextField = "KitName"
            .DataValueField = "KitID"
            .DataBind()
        End With
        With CmbMKit
            .DataSource = Dt
            .DataTextField = "KitName"
            .DataValueField = "KitID"
            .DataBind()
        End With
    End Sub

#End Region

#Region " Excel Template download"
    'Protected Sub btnTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTemplate.Click
    '    Dim fileToDownload = Server.MapPath("~/App_UI/Resources/Templates/TopUpMembersTemplate.xlsx")
    '    Dim filename As String = Path.GetFileName(fileToDownload)
    '    Response.ContentType = "application/ms-excel"
    '    Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", filename))
    '    Dim fileData As Byte() = System.IO.File.ReadAllBytes(fileToDownload)
    '    Response.OutputStream.Write(fileData, 0, fileData.Length)
    'End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
#End Region

#Region "Upload Excel File to activate members"
    'Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
    '    divUpload.Visible = True
    '    ''btnUpload.Visible = False
    'End Sub

    'Protected Sub btnUpload1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload1.Click

    '    Dim connString As String = ""
    '    If fileUpload.HasFile = True Then
    '        'SavePath
    '        Dim savePath As String = Server.MapPath("~/App_UI/Resources/Templates/")

    '        ' Get the name of the file to upload.
    '        Dim fileName As String = fileUpload.FileName

    '        ' Create the path and file name to check for duplicates.
    '        Dim pathToCheck As String = savePath + fileName

    '        ' Create a temporary file name to use for checking duplicates.
    '        Dim tempfileName As String

    '        ' Check to see if a file already exists with the
    '        ' same name as the file to upload.        
    '        If (System.IO.File.Exists(pathToCheck)) Then
    '            Dim counter As Integer = 2
    '            While (System.IO.File.Exists(pathToCheck))
    '                ' If a file with this name already exists,
    '                ' prefix the filename with a number.
    '                tempfileName = counter.ToString() + fileName
    '                pathToCheck = savePath + tempfileName
    '                counter = counter + 1
    '            End While
    '            fileName = tempfileName
    '        End If

    '        ' Append the name of the file to upload to the path.
    '        savePath += fileName

    '        ' Call the SaveAs method to save the uploaded
    '        ' file to the specified directory.
    '        fileUpload.SaveAs(savePath)
    '        'fileUpload.SaveAs(Server.MapPath("~/App_UI/Resources/Templates/"))
    '        Dim strFileType As String = Path.GetExtension(fileUpload.FileName).ToLower()
    '        Dim path__1 As String = fileUpload.PostedFile.FileName

    '        If String.Equals(strFileType, ".xls") = True Or String.Equals(strFileType, ".xlsx") = True Then
    '            'Connection String to Excel Workbook
    '            If strFileType.Trim() = ".xls" Then
    '                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & savePath & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
    '            ElseIf strFileType.Trim() = ".xlsx" Then
    '                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & savePath & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2"""
    '            End If

    '            Dim objConn As New OleDbConnection(connString)
    '            If objConn.State = ConnectionState.Closed Then
    '                objConn.Open()
    '            End If
    '            ' Get the data table containg the schema guid.
    '            Dim dbSchema As DataTable = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
    '            Dim firstSheetName As String = dbSchema.Rows(0)("TABLE_NAME").ToString()
    '            Dim objCommand As New OleDbCommand("SELECT * FROM [" + firstSheetName + "]", objConn)

    '            'Dim query As String = "SELECT * FROM [MemberID$]"
    '            'Dim cmd As New OleDbCommand(Query, conn)
    '            Dim da As New OleDbDataAdapter(objCommand)
    '            dtIDList = New DataTable
    '            da.Fill(dtIDList)
    '            da.Dispose()
    '            objConn.Close()
    '            objConn.Dispose()

    '            FillGridData(dtIDList)
    '            lblkit.visible = True : CmbMKit.Visible = True
    '            If GvData.Rows.Count > 0 Then
    '                'btnUpload.Visible = True
    '                BtnActivate.Visible = True
    '            End If
    '        Else
    '            lblError.Text = "Please upload excel sheet same as template provided to proceed."
    '            lblError.ForeColor = Drawing.Color.Red
    '            lblError.Visible = True
    '            'btnUpload.Visible = True
    '        End If
    '    Else
    '        lblError.Text = "Please upload file to proceed."
    '        lblError.ForeColor = Drawing.Color.Red
    '        lblError.Visible = True
    '        ' btnUpload.Visible = True

    '    End If
    'End Sub
#End Region

#Region "Fill Member details in grid"
    Private Sub FillGridData(Optional ByVal Condition As String = "")
        Dim sb As New StringBuilder
        'For Each dr As DataRow In dt.Rows
        '    'strArray = "'" & dr(0).ToString() & "',"
        '    sb.Append("'" & dr(0).ToString() & "',")
        'Next
        'sb.Remove(sb.Length - 1, 1)
        Dim qry As String = "Select 'false' as Status, KitName,IdNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,Replace(Convert(varchar,DOJ,106),' ','-') as DOJ,Paymode,ChDDNo,CASE WHEN PID='1' THEN '' ELSE Replace(Convert(varchar,ChDDDate,106),' ','-') END as ChDDDate,ChDDBank,CASE WHEN IsTopup='Y' THEN 'Yes' ELSE 'No' End as TopupStatus from V#MemberDetail where IsTopup='N' " & Condition
        '"select * from " & objDAL.tblMemberMaster & " where IdNo IN(" & sb.ToString() & ") AND istopup='N'"
        dtData = New DataTable
        dtData = objDAL.GetData(qry)
        GvData.DataSource = dtData
        GvData.DataBind()
        Session("GridData") = dtData
        GvData.Visible = True
        lblrecordcount.Text = "Record Count : " & GvData.Rows.Count
        If dtData.Rows.Count > 0 Then
            BtnActivate.Visible = True
        Else : BtnActivate.Visible = False
        End If
    End Sub
#End Region

#Region "Activate Single Member ID"
    Protected Sub btnShowSingleDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowSingleDetail.Click
        ShowDetail()
    End Sub

    Private Sub ShowDetail()
        Dim idNo As String
        If txtMemberId.Text <> "" Then
            idNo = txtMemberId.Text
            Dim topupstatus As String
            Dim qry As String = "Select 'true' as Status, KitName,IdNo,RTRIM(MemFirstName +' ' + MemLastName) as MemName,Replace(Convert(varchar,DOJ,106),' ','-') as DOJ,Paymode,ChDDNo,CASE WHEN PID='1' THEN '' ELSE Replace(Convert(varchar,ChDDDate,106),' ','-') END as ChDDDate,ChDDBank,IsTopup,CASE WHEN IsTopup='Y' THEN 'Yes' ELSE 'No' End as TopupStatus from V#MemberDetail where IdNo like '" & idNo & "'"
            dtData = New DataTable
            dtData = objDAL.GetData(qry)
            If dtData.Rows.Count = 0 Then
                lblError.Text = "Member ID not exist. Please provide correct member ID."
                lblError.ForeColor = Drawing.Color.Red
                lblError.Visible = True
            Else
                topupstatus = dtData.Rows(0)("IsTopup").ToString()
                If String.Equals(topupstatus.ToUpper(), "N") = True Then
                    GvData.DataSource = dtData
                    GvData.DataBind()
                    GvData.Visible = True
                    BtnActivateSingle.Visible = True
                    lblrecordcount.Text = "Record Count : " & GvData.Rows.Count

                Else
                    lblError.Text = "Member ID already Activated. Please provide another member ID."
                    lblError.ForeColor = Drawing.Color.Red
                    lblError.Visible = True
                End If
            End If
        Else
            lblError.Text = "Member Id can not be blank. Please provide member ID to proceed."
            lblError.ForeColor = Drawing.Color.Red
            lblError.Visible = True
        End If
    End Sub

    Protected Sub BtnActivateSingle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnActivateSingle.Click
        Dim idNo As String
        If txtMemberId.Text <> "" Then
            idNo = txtMemberId.Text
            Dim qry As String = "Select * from " & objDAL.tblMemberMaster & " Where IdNo like '" & idNo & "'"
            Dim isTopup As String
            Dim procName As String
            dtData = New DataTable
            Dim Chk As CheckBox
            dtData = objDAL.GetData(qry)
            If dtData.Rows.Count = 0 Then
                lblError.Text = "Member ID not exist. Please provide correct member ID."
                lblError.ForeColor = Drawing.Color.Red
                lblError.Visible = True
            Else
                isTopup = dtData.Rows(0)("IsTopUp").ToString()
                If Trim(TxtDDDate.Text) = "" Then
                    TxtDDDate.Text = Format(Now, "dd-MMM-yyyy")
                End If
                If String.Equals(isTopup.ToUpper(), "N") = True Then
                    ' Activate Selected member
                    'Chk = DirectCast(GvData.FindControl("chkSelect"), CheckBox)
                    ' If Chk.Checked = True Then
                    procName = "Sp_ActivateMembers"
                    Dim paraname As String = "@IDNo;@KitID"
                    Dim paravalue As String = idNo & ";" & Val(CmbKit.SelectedValue)
                    Dim a As Integer = objDAL.ExecuteProcedure(procName, paraname, paravalue)
                    '****** Update Payment Detail ******
                    paraname = "@PID;@Paymode;@ChqNo;@ChqDate;@Bank;@Branch"
                    paravalue = Val(DdlPaymode.SelectedValue) & ";" & DdlPaymode.SelectedItem.Text & ";" & Trim(TxtDDNo.Text) & ";" & Trim(TxtDDDate.Text) & ";" & Trim(TxtIssuedBank.Text) & ";" & Trim(TxtIssuedBranch.Text)
                    qry = "UPDATE M_MemberMaster SET PID=@PID,Paymode=@Paymode,ChDDNo=@ChqNo,ChDDBankID='0',ChDDBank=@Bank,ChddDate=@ChqDate,ChDDBranch=@Branch WHERE IDNO='" & idNo & "'"
                    objDAL.UpdateData(qry, paraname, paravalue)
                    '******  ******

                    sendSMS(idNo)
                    If a <> 0 Then
                        lblError.Text = "Member Activated Successfully."
                        lblError.ForeColor = Drawing.Color.Green
                        lblError.Visible = True
                        txtMemberId.Text = ""
                        BtnActivate.Visible = False

                        'Clear grid view data
                        GvData.DataSource = Nothing
                        GvData.DataBind()
                    Else
                        lblError.Text = "Not able to activate selected member due to some issue."
                        lblError.ForeColor = Drawing.Color.Green
                        lblError.Visible = True
                    End If
                    'End If
                Else
                    lblError.Text = "Member Id already activated. Please provide another one."
                    lblError.ForeColor = Drawing.Color.Red
                    lblError.Visible = True
                End If
            End If
        Else
            lblError.Text = "Member Id can not be blank. Please provide member ID to proceed."
            lblError.ForeColor = Drawing.Color.Red
            lblError.Visible = True
        End If
    End Sub
#End Region

#Region "Activate Multiple members"
    Protected Sub BtnActivate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnActivate.Click
        Dim procName As String = "Sp_ActivateMembers"
        Dim paraname As String = "@IDNo;@KitID"
        Dim paravalue As String
        Dim a As Integer
        Dim idNo As String
        Dim Chk As CheckBox
        Dim Count_ As Integer = 0
        For i As Integer = 0 To GvData.Rows.Count - 1
            paravalue = ""
            idNo = GvData.Rows(i).Cells(2).Text
            Chk = DirectCast(GvData.FindControl("chkSelect"), CheckBox)
            If Chk.Checked = True Then
                Dim qry As String = "Select * from " & objDAL.tblMemberMaster & " Where IdNo like '" & paravalue & "'"
                Dim isTopup As String
                dtData = New DataTable
                dtData = objDAL.GetData(qry)
                If dtData.Rows.Count = 0 Then
                    'lblError.Text = "Member ID not exist. Please provide correct member ID."
                    'lblError.ForeColor = Drawing.Color.Red
                    'lblError.Visible = True
                Else
                    isTopup = dtData.Rows(0)("IsTopUp").ToString()
                    If String.Equals(isTopup.ToUpper(), "N") = True Then
                        paravalue = idNo & ";" & Val(CmbMKit.SelectedValue)
                        a = objDAL.ExecuteProcedure(procName, paraname, paravalue)
                        '****** Update Payment Detail ******
                        paraname = "@PID;@Paymode;@ChqNo;@ChqDate;@Bank;@Branch"
                        paravalue = Val(ddlCash.SelectedValue) & ";" & Trim(ddlCash.SelectedItem.Text) & ";;" & Format(Now, "dd-MMM-yyyy") & ";;"
                        qry = "UPDATE M_MemberMaster SET PID=@PID,Paymode=@Paymode,ChDDNo=@ChqNo,ChDDBankID='0',ChDDBank=@Bank,ChddDate=@ChqDate,ChDDBranch=@Branch WHERE IDNO='" & idNo & "'"
                        objDAL.UpdateData(qry, paraname, paravalue)
                        '******  ******
                        sendSMS(paravalue)
                        Count_ += 1
                    End If
                End If
            End If
        Next

        If a <> 0 Then
            lblError.Text = Count_ & " Members Activated Successfully."
            lblError.ForeColor = Drawing.Color.Green
            lblError.Visible = True

            'Clear grid view data
            GvData.DataSource = Nothing
            GvData.DataBind()
        Else
            'btnUpload.Visible = True
            lblError.Text = "Members not Activated."
            lblError.ForeColor = Drawing.Color.Red
            lblError.Visible = True

        End If
        GvData.Visible = False
        'Lblkit.Visible = False : CmbMKit.Visible = False : LblPMode.Visible = False : ddlCash.Visible = False
    End Sub
#End Region

    Protected Sub rdblistChoice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdblistChoice.SelectedIndexChanged
        GvData.DataSource = Nothing
        GvData.DataBind()

        'CmbMKit.Visible = False : Lblkit.Visible = False : LblPMode.Visible = False : ddlCash.Visible = False
    End Sub

    Private Sub sendSMS(ByVal LastInsertID As String)
        Dim dRead As SqlDataReader
        Dim Comm As New SqlCommand
        dbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dbConnect.OpenConnection()
        Comm = New SqlCommand("Select A.*,B.KitName from m_MemberMaster As A,M_KitMaster As B where A.KitID=B.KitID And A.IDNo = '" & LastInsertID & "'", dbConnect.cnnObject)
        dRead = Comm.ExecuteReader
        If dRead.Read = True Then
            Session("UPGRDID") = dRead("IDNo")
            Session("UPGRDName") = dRead("MemFirstName")
            Session("UPGRDMobileNo") = dRead("Mobl")
            Session("UPGRDKit") = dRead("KitName")
            Session("UPGRDPassw") = dRead("Passw")
        End If
        dRead.Close()

        If Len(Session("UPGRDMobileNo")) >= 10 And IsNumeric(Session("UPGRDMobileNo")) = True Then

            Dim client As New WebClient
            Dim baseurl As String
            Dim data As Stream
            Dim sms As String = "Dear " & Session("UPGRDName") & ", Your id " & Session("UPGRDID") & " is successfully topup by " & Session("UPGRDKit") & " and password is " & Session("UPGRDPassw") & ". Best of luck, Regards: " & Session("CompName") & ""

            Try
                baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Session("UPGRDMobileNo") & "&msg=" & sms & ""
                data = client.OpenRead(baseurl)
                Dim reader As New StreamReader(data)
                Dim s As String
                s = reader.ReadToEnd()
                data.Close()
                reader.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End If

        '  ClrCtrl()
        dRead.Close()

    End Sub

    Protected Sub ChkDateWise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkDateWise.CheckedChanged
        If ChkDateWise.Checked = True Then
            txtStartDate.Enabled = True : txtEndDate.Enabled = True
        Else
            txtStartDate.Enabled = False : txtEndDate.Enabled = False
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim Condition As String = ""
        If Trim(TxtSearch.Text) <> "" Then
            If DdlSearch.SelectedValue = "UplineIDNo" Or DdlSearch.SelectedValue = "ReferalIDNo" Then
                Condition = " AND " & DdlSearch.SelectedValue & " = '" & Trim(TxtSearch.Text) & "'"
            Else
                Condition = " AND " & DdlSearch.SelectedValue & " like '%" & Trim(TxtSearch.Text) & "%'"
            End If
        End If
        If DdlSearch.SelectedValue = "0" Then Condition = ""
        If ChkDateWise.Checked = True Then
            If Trim(txtStartDate.Text) <> "" Then
                Condition = Condition & " AND DOJ>='" & Trim(txtStartDate.Text) & "' "
            End If
            If Trim(txtEndDate.Text) <> "" Then
                Condition = Condition & " AND DOJ<='" & Trim(txtEndDate.Text) & "' "
            End If
        End If
        FillGridData(Condition)
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("GridData")
        GvData.DataBind()
    End Sub

    Protected Sub GvData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvData.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
        If rdblistChoice.SelectedValue = "single" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Protected Sub DdlPaymode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlPaymode.SelectedIndexChanged
        If DdlPaymode.SelectedValue = "1" Then
            DvPaymode.Visible = False
        Else
            DvPaymode.Visible = True
            If DdlPaymode.SelectedValue = "2" Or DdlPaymode.SelectedValue = "3" Then
                LblDDNo.Text = DdlPaymode.SelectedItem.Text & " No. : " : LblDDDate.Text = DdlPaymode.SelectedItem.Text & " Date : "
            ElseIf DdlPaymode.SelectedValue = "4" Then
                LblDDNo.Text = "Transaction No. : " : LblDDDate.Text = "Transaction Date : "
            End If
        End If
    End Sub
End Class
