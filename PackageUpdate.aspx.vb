Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data.OleDb

Partial Class App_UI_Application_Pages_PackageUpdate
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim dtIDList As New DataTable
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral


#Region "Page Events"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        divDropDown.Visible = False
        divSingle.Visible = False
        divMultiple.Visible = False
        'btnUpdate.Visible = False
        divUpload.Visible = False
        lblError.Visible = False
        lblrecordcount.Text = ""
        If Not Page.IsPostBack Then
            txtMemberId.Text = ""
            lblError.Text = ""
            FillPackageDDL()
        End If

        If String.Equals(rdblistChoice.SelectedValue.ToLower(), "single") = True Then
            divSingle.Visible = True
        Else
            divMultiple.Visible = True
        End If
    End Sub
#End Region

#Region "Fill Packages"
    Private Sub FillPackageDDL()
        'Fill Package drop down list
        Dim qry1 As String = "Select * from " & objDAL.tblKitMaster & " Where ActiveStatus='Y' AND " & objDAL.activeCondition
        objModuleFun.FillCombo(qry1, ddlPackage, "KitName", "KitId")
    End Sub
#End Region

#Region "Template download"
    Protected Sub btnTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTemplate.Click
        Dim fileToDownload = Server.MapPath("~/App_UI/Resources/Templates/UpdatePackTemplate.xlsx")
        Dim filename As String = Path.GetFileName(fileToDownload)
        Response.ContentType = "application/ms-excel"
        Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", filename))
        Dim fileData As Byte() = System.IO.File.ReadAllBytes(fileToDownload)
        Response.OutputStream.Write(fileData, 0, fileData.Length)
    End Sub


    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
#End Region

#Region "Upload Excel File to Update Package"
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        divUpload.Visible = True
        btnUpload.Visible = False
    End Sub

    Protected Sub btnUpload1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload1.Click
        Dim connString As String = ""
        If fileUpload.HasFile = True Then
            'SavePath
            Dim savePath As String = Server.MapPath("~/App_UI/Resources/Templates/")

            ' Get the name of the file to upload.
            Dim fileName As String = fileUpload.FileName

            ' Create the path and file name to check for duplicates.
            Dim pathToCheck As String = savePath + fileName

            ' Create a temporary file name to use for checking duplicates.
            Dim tempfileName As String

            ' Check to see if a file already exists with the
            ' same name as the file to upload.        
            If (System.IO.File.Exists(pathToCheck)) Then
                Dim counter As Integer = 2
                While (System.IO.File.Exists(pathToCheck))
                    ' If a file with this name already exists,
                    ' prefix the filename with a number.
                    tempfileName = counter.ToString() + fileName
                    pathToCheck = savePath + tempfileName
                    counter = counter + 1
                End While
                fileName = tempfileName
            End If

            ' Append the name of the file to upload to the path.
            savePath += fileName

            ' Call the SaveAs method to save the uploaded
            ' file to the specified directory.
            fileUpload.SaveAs(savePath)
            'fileUpload.SaveAs(Server.MapPath("~/App_UI/Resources/Templates/"))
            Dim strFileType As String = Path.GetExtension(fileUpload.FileName).ToLower()
            Dim path__1 As String = fileUpload.PostedFile.FileName

            If String.Equals(strFileType, ".xls") = True Or String.Equals(strFileType, ".xlsx") = True Then
                'Connection String to Excel Workbook
                If strFileType.Trim() = ".xls" Then
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & savePath & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
                ElseIf strFileType.Trim() = ".xlsx" Then
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & savePath & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2"""
                End If

                Dim objConn As New OleDbConnection(connString)
                If objConn.State = ConnectionState.Closed Then
                    objConn.Open()
                End If
                ' Get the data table containg the schema guid.
                Dim dbSchema As DataTable = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                Dim firstSheetName As String = dbSchema.Rows(0)("TABLE_NAME").ToString()
                Dim objCommand As New OleDbCommand("SELECT * FROM [" + firstSheetName + "]", objConn)

                Dim da As New OleDbDataAdapter(objCommand)
                dtIDList = New DataTable
                da.Fill(dtIDList)
                da.Dispose()
                objConn.Close()
                objConn.Dispose()

                FillGridData(dtIDList)

                If GvData.Rows.Count > 0 Then
                    btnUpload.Visible = True
                    divDropDown.Visible = True
                End If
            Else
                lblError.Text = "Please upload excel sheet same as template provided to proceed."
                lblError.ForeColor = Drawing.Color.Red
                lblError.Visible = True
                btnUpload.Visible = True
            End If
        Else
            lblError.Text = "Please upload file to proceed."
            lblError.ForeColor = Drawing.Color.Red
            lblError.Visible = True
            btnUpload.Visible = True
        End If
    End Sub
#End Region

#Region "Fill Member details in grid"
    Private Sub FillGridData(ByVal dt As DataTable)
        Dim sb As New StringBuilder
        For Each dr As DataRow In dt.Rows
            'strArray = "'" & dr(0).ToString() & "',"
            sb.Append("'" & dr(0).ToString() & "',")
        Next
        sb.Remove(sb.Length - 1, 1)
        Dim qry As String = "Select a.KitName,a.KitAmount,b.IdNo as IdNumber,b.MemFirstName as MemberFirstName,b.MemLastName as LastName from " & objDAL.tblKitMaster & " as a," & objDAL.tblMemberMaster & " as b where a.KitId=b.KitId AND b.IdNo IN(" & sb.ToString() & ") AND b.IsTopUp='N'"
        dtData = New DataTable
        dtData = objDAL.GetData(qry)
        GvData.DataSource = dtData
        GvData.DataBind()
        GvData.Visible = True
        lblrecordcount.Text = "Record Count : " & GvData.Rows.Count
    End Sub
#End Region

#Region "Activate Single Member ID"

    Protected Sub btnShowSingleDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowSingleDetail.Click
        Dim idNo As String
        If txtMemberId.Text <> "" Then
            idNo = txtMemberId.Text
            'Dim topupstatus As String
            Dim qry As String = "Select a.KitName,a.KitAmount,b.IdNo as IdNumber,b.MemFirstName as MemberFirstName,b.MemLastName as LastName from " & objDAL.tblKitMaster & " as a," & objDAL.tblMemberMaster & " as b where a.KitId=b.KitId AND b.IdNo like '" & idNo & "' AND b.IsTopUp='N'"
            '"Select KitName,IdNumber,FirstName,LastName,dateofjoining,ChDDNo,ChDDDate,ChDDBank,TopupStatus from V#TopUpReport Where IdNumber like '" & idNo & "'"
            dtData = New DataTable
            dtData = objDAL.GetData(qry)
            If dtData.Rows.Count = 0 Then
                lblError.Text = "Member ID not exist. Please provide correct member ID."
                lblError.ForeColor = Drawing.Color.Red
                lblError.Visible = True
            Else
                'topupstatus = dtData.Rows(0)("TopupStatus").ToString()
                ' If String.Equals(topupstatus.ToUpper(), "N") = True Then
                GvData.DataSource = dtData
                GvData.DataBind()
                GvData.Visible = True
                divDropDown.Visible = True
                lblrecordcount.Text = "Record Count : " & GvData.Rows.Count
            End If
        Else
            lblError.Text = "Member Id can not be blank. Please provide member ID to proceed."
            lblError.ForeColor = Drawing.Color.Red
            lblError.Visible = True
        End If
    End Sub
#End Region

#Region "Update Package"
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim idNo As String
        Dim a As Integer
        Dim procName As String
        Dim paravalue As String
        procName = "Sp_UpdatePackage"
        Dim paraname As String = "@IDNo;@KitID"
        If divSingle.Visible = True Then
            If txtMemberId.Text <> "" Then
                idNo = txtMemberId.Text
                paravalue = idNo & ";" & ddlPackage.SelectedValue
                a = objDAL.ExecuteProcedure(procName, paraname, paravalue)
            Else
                lblError.Text = "Member Id can not be blank. Please provide member ID to proceed."
                lblError.ForeColor = Drawing.Color.Red
                lblError.Visible = True
            End If
        Else
            For i As Integer = 0 To GvData.Rows.Count - 1
                paravalue = GvData.Rows(i).Cells(2).Text & ";" & ddlPackage.SelectedValue
                a = objDAL.ExecuteProcedure(procName, paraname, paravalue)
            Next
        End If
        If a <> 0 Then
            lblError.Text = "Member Activated Successfully."
            lblError.ForeColor = Drawing.Color.Green
            lblError.Visible = True
            txtMemberId.Text = ""
            'btnUpdate.Visible = False
            divDropDown.Visible = False
            'Clear grid view data
            GvData.DataSource = Nothing
            GvData.DataBind()
        Else
            lblError.Text = "Not able to activate selected members due to some issue."
            lblError.ForeColor = Drawing.Color.Green
            lblError.Visible = True
        End If
    End Sub
#End Region

    Protected Sub rdblistChoice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdblistChoice.SelectedIndexChanged
        GvData.DataSource = Nothing
        GvData.DataBind()
    End Sub
End Class
