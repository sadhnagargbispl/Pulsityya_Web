Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddNews
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

        Try
            Dim str = "exec('Create table Trnnews ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
"ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnnews] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
            Dim i As Integer = 0
            i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
        Catch ex As Exception

        End Try

        If String.IsNullOrEmpty(Request("NewsId")) = False Then
            UserIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("NewsId")))
        End If
        If Not Page.IsPostBack Then
            HdnCheckTrnns.Value = GenerateRandomStringJoining(6)
            ClearAll()
            'BindType()

            ' Setting up values for GroupId and IP Address
            'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
            txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()

            If String.IsNullOrEmpty(Request("NewsId")) = False Then
                BtnSave.Text = "Modify"
                BindData()

            End If
        End If
    End Sub
    Public Function GenerateRandomStringJoining(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function
    'Private Sub BindType()
    '    'Fill Group drop down list
    '    Dim qry1 As String = "Select * from " & objDAL.tblNewsTypeMaster & " Where ActiveStatus='Y' AND " & objDAL.activeCondition
    '    objModuleFun.FillCombo(qry1, DDLType, "Type", "Prefix")

    '    'If String.IsNullOrEmpty(Session("Prefix")) = False Then
    '    '    DDLType.SelectedValue = Session("Prefix")
    '    'End If
    'End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From " + objDAL.tblNewsMaster + " Where NewsId='" & UserIdQS & "' AND " + objDAL.activeCondition
        'Dim sql As String = ""
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            'DDLType.SelectedValue = Dt.Rows(0)("NType")
            txtNewsID.Text = Dt.Rows(0)("NewsId")
            txtHeading.Text = Dt.Rows(0)("NewsHdr")
            txtDetail.Text = Dt.Rows(0)("NewsDtl")
            txtFrmDate.Text = Format(Dt.Rows(0)("FrmDate"), "dd-MMM-yyyy")
            txtToDate.Text = Format(Dt.Rows(0)("ToDate"), "dd-MMM-yyyy")
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
        txtHeading.Text = ""
        txtDetail.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        txtNewsID.Text = ""
        txtFrmDate.Text = ""
        txtToDate.Text = ""
    End Sub


    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Strquery = "Insert into Trnnews (Transid) values(" & HdnCheckTrnns.Value & ")"
        Dim isOk1 As Integer = 0
        isOk1 = objDAL.SaveData(Strquery)
        If isOk1 > 0 Then

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
            Dim ToDate As String
            ToDate = txtToDate.Text
            Try
                Dim Dt As DateTime = FrmDate
            Catch ex As Exception
                scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                Exit Sub
            End Try
            Try
                Dim Dt As DateTime = ToDate
            Catch ex As Exception
                scrname = "<SCRIPT language='javascript'>alert('Check End Date.. ');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                Exit Sub
            End Try
            ' DDLType.SelectedValue = "N"
            If String.IsNullOrEmpty(Request("NewsId")) = False Then

                Sql = "Update " & objDAL.tblNewsMaster & " SET RowStatus='N' Where NewsId='" & UserIdQS & "';"
                Sql = Sql & " Insert into " & objDAL.tblNewsMaster + "(NewsId,NewsHdr,NewsDtl,FrmDate,ToDate,NType," & _
                "Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) Values('" & Val(txtNewsID.Text) & "'" & _
                ",N'" & txtHeading.Text & "',N'" & txtDetail.Text & "','" & FrmDate & "','" & ToDate & "','N','" & txtRemarks.Text & "'" & _
                ",'" & txtActiveStatus.Text & "','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "'," & _
                "'" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "','Y') "

            Else
                Sql = " Insert into " & objDAL.tblNewsMaster + "(NewsId,NewsHdr,NewsDtl,FrmDate,ToDate,NType," & _
                "Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) Select Case When Max(NewsId) Is Null Then '1' Else Max(NewsId)+1 END as NewsId " & _
                " ,N'" & txtHeading.Text & "',N'" & txtDetail.Text & "','" & FrmDate & "','" & ToDate & "','N','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "', " & _
                " 'New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Session("UserName") & "','" & Val(Session("UserID")) & "','" & txtIPAdrs.Text & "'," & _
                "'Y' From " & objDAL.tblNewsMaster
            End If

            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Sql)

            If String.IsNullOrEmpty(Request("NewsId")) = False And updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');location.replace('AddNews.aspx');" & "</SCRIPT>"
                'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record Updated successfully !!!');", True)
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "update", scrname, False)
            ElseIf updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');location.replace('AddNews.aspx');" & "</SCRIPT>"
                'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record Save successfully !!!');", True)
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "save", scrname, False)
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');location.replace('AddNews.aspx');" & "</SCRIPT>"
                'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Record not saved successfully !!!');", True)
                'ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "do not save", scrname, False)
            End If

           
        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('This News already register.!');location.replace('NewsNSeminarMaster.aspx');", True)
            Exit Sub
        End If
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        ClearAll()
    End Sub
End Class
