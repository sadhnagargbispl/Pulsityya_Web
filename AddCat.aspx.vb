Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddCat
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim CatIDQS As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub
    Public Function GenerateRandomString(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("CatID")) = False Then
            CatIDQS = Request("CatID")
        End If
        Dim str = "exec('Create table Trnrejectbyadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
 "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnfundtransferbyadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
        Dim i As Integer = 0
        i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)

        If Not Page.IsPostBack Then
            HdnCheckTrnns.Value = GenerateRandomString(6)
            ClearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("CatID")) = False Then
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
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select * From M_CatMaster Where CatID='" & CatIDQS & "' AND " + objDAL.activeCondition
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtCatName.Text = Dt.Rows(0)("CatName")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtCatId.Text = Dt.Rows(0)("CatID")
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
        Dim updateeffects As Integer
        Dim StrSql As String = "Insert into Trnrejectbyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
        updateeffects = objDAL.SaveData(StrSql)
        If updateeffects > 0 Then
            If rdblist.SelectedIndex = 0 Then
                txtActiveStatus.Text = "Y"
            Else
                txtActiveStatus.Text = "N"
            End If
            If String.IsNullOrEmpty(Request("CatID")) = False Then
                Sql = "Update M_CatMaster set CatName = '" & txtCatName.Text & "',Remarks = '" & txtRemarks.Text & "',"
                Sql &= "ActiveStatus = '" & txtActiveStatus.Text & "',LastModified = 'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' Where CatID = '" & txtCatId.Text & "'"
            Else
                Sql = "Insert into M_CatMaster (CatName,Remarks,ActiveStatus,LastModified,UserId,RowStatus)values('" & txtCatName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "','New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Val(Session("UserID")) & "','Y')"
            End If
            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Sql)
            If String.IsNullOrEmpty(Request("CatID")) = False And updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated!!');" & "</SCRIPT>"
            ElseIf updateEffect <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
            End If
        Else
            Response.Redirect("CategoryMaster.aspx")
        End If
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

    Private Sub ClearAll()
        txtCatName.Text = ""
        txtCatId.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
    End Sub

End Class
