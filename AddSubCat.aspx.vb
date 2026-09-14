
Imports System.Data
Imports System.Data.SqlClient

Partial Class AddSubCat
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
            FillCategory()
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
    Private Sub FillCategory()
        Try
            Dim ds As DataSet = New DataSet()
            Dim str As String = "Select CatName,CatID From M_CatMaster"
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            If (ds.Tables(0).Rows.Count > 0) Then
                ddlcountry.DataSource = ds.Tables(0)
                ddlcountry.DataTextField = "CatName"
                ddlcountry.DataValueField = "CatID"
                ddlcountry.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindData()
        Dim sql As String = "Select * From M_SubCatMaster Where SubCatID = '" & CatIDQS & "' AND " + objDAL.activeCondition
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            txtCatName.Text = Dt.Rows(0)("CatName")
            txtRemarks.Text = Dt.Rows(0)("Remarks")
            txtCatId.Text = Dt.Rows(0)("SubCatID")
            txtActiveStatus.Text = Dt.Rows(0)("ActiveStatus")
            txtPARTNER.Text = Dt.Rows(0)("Rankid1")
            'txtPARTNER.Enabled = False
            txtMASTER.Text = Dt.Rows(0)("Rankid2")
            'txtMASTER.Enabled = False
            txtAGENCY.Text = Dt.Rows(0)("Rankid3")
            'txtAGENCY.Enabled = False
            txtAGENT.Text = Dt.Rows(0)("Rankid4")
            'txtAGENT.Enabled = False
            'txtEMALL.Text = dt.Rows(0)("Rankid5")
            'txtEMALL.Enabled = False
            TxtCashback.Text = Dt.Rows(0)("Cashback")
            TXtBonus.Text = Dt.Rows(0)("Commission")
            ddlcountry.SelectedValue = Dt.Rows(0)("CatID")
            If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
                rdblist.SelectedIndex = 0
            Else
                rdblist.SelectedIndex = 1
            End If
        End If
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        If txtPARTNER.Text = "" Then
            Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter PARTNER.!');</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            Exit Sub
        End If
        If txtMASTER.Text = "" Then
            Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter MASTER.!');</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            Exit Sub
        End If
        If txtAGENCY.Text = "" Then
            Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter AGENCY.!');</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            Exit Sub
        End If
        If txtAGENT.Text = "" Then
            Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter AGENT.!');</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            Exit Sub
        End If
        If TxtCashback.Text = "" Then
            Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter Cashback.!');</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            Exit Sub
        End If
        If TxtCashback.Text = "0" Then
            Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter Valid Cashback.!');</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            Exit Sub
        End If
        If TXtBonus.Text = "" Then
            Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter Bonus.!');</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            Exit Sub
        End If
        If TXtBonus.Text = "0" Then
            Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter Valid Bonus.!');</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            Exit Sub
        End If
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
                Sql = "Update M_SubCatMaster set CatName = '" & txtCatName.Text & "',Remarks = '" & txtRemarks.Text & "',catID = '" & ddlcountry.SelectedValue & "', "
                Sql &= "ActiveStatus = '" & txtActiveStatus.Text & "',LastModified = 'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "',"
                Sql &= "Cashback = '" & TxtCashback.Text & "',Rankid1 = '" & txtPARTNER.Text & "',Rankid2 = '" & txtMASTER.Text & "',"
                Sql &= "Rankid3 = '" & txtAGENCY.Text & "',Rankid4 = '" & txtAGENT.Text & "',Rankid5 = '0',Commission = '" & TXtBonus.Text & "' "
                Sql &= "Where SubCatID = '" & txtCatId.Text & "'"
            Else
                Sql = "Insert into M_SubCatMaster (CatName,Remarks,ActiveStatus,LastModified,UserId,RowStatus,CatID,Cashback,Commission,Rankid1,Rankid2,Rankid3,Rankid4,Rankid5)"
                Sql &= "values('" & txtCatName.Text & "','" & txtRemarks.Text & "','" & txtActiveStatus.Text & "',"
                Sql &= "'New by " & Session("UserName") & " at " & DateTime.Now.ToString() & "','" & Val(Session("UserID")) & "',"
                Sql &= "'Y','" & ddlcountry.SelectedValue & "','" & TxtCashback.Text & "','" & txtPARTNER.Text & "','" & txtMASTER.Text & "',"
                Sql &= "'" & txtAGENCY.Text & "','" & txtAGENT.Text & "','0','" & TXtBonus.Text & "')"
            End If

            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Sql)

            If String.IsNullOrEmpty(Request("CatID")) = False And updateEffect <> 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Successfully Updated.!');location.replace('SubCategoryMaster.aspx');", True)
            ElseIf updateEffect <> 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Save Successfully!.');location.replace('SubCategoryMaster.aspx');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Data not saved Successfully.!');location.replace('SubCategoryMaster.aspx');", True)
            End If
        Else
            Response.Redirect("SubCategoryMaster.aspx")
        End If
    End Sub

    Private Sub ClearAll()
        txtCatName.Text = ""
        txtCatId.Text = ""
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
    End Sub

End Class
