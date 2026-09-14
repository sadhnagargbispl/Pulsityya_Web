Imports System.Data
Imports System.Data.SqlClient

Partial Class App_UI_Application_Pages_AddFund
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim KitIdQS As String
    Dim objGen As clsGeneral = New clsGeneral
    Dim Sql As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        objModuleFun = New ModuleFunction(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If String.IsNullOrEmpty(Request("KitId")) = False Then
            '   KitIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("KitId")))
            KitIdQS = Request("KitId")
        End If
        If Not Page.IsPostBack Then
            Pages()
            ClearAll()
            If Session("AStatus") = "OK" Then
                If String.IsNullOrEmpty(Request("Fid")) = False Then
                   

                Else
                    BindFromDate()
                    'Fill_SeriesStart()
                End If
            Else
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        'txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
    End Sub

    
    Public Sub BindFromDate()
        Dim startDate As Date
        Dim Ds As DataSet

        '        'Dim endDate As Date
        '    Dim sql As String = "select Sessid,Replace(Convert(Varchar,FrmDate,106),' ','-') as FromDate,Replace(Convert(Varchar,ToDate,106),' ','-') as Todate from D_SessnMaster order by Sessid Desc"
        '    objModuleFun.FillCombo(sql, DDlFromDate, "FromDate", "Sessid")
        '    objModuleFun.FillCombo(sql, DDltodate, "ToDate", "Sessid")
        Try
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetWeeklySessionNew")
            ddlSession.DataSource = Ds.Tables(0)
            ddlSession.DataValueField = "SessID"
            ddlSession.DataTextField = "SessnName"
            ddlSession.DataBind()
            
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Pages()
        Try
            Dim dtMenu As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            Dim str As String = " Select * from M_CompWiseWebMenuMaster Where MenuID = 123 And CompanyID = '" & HttpContext.Current.Session("CompID") & "'  "
            str &= " And ActiveStatus = 'Y' And RowStatus ='Y'"
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dtMenu = ds.Tables(0)
            If (dtMenu.Rows.Count > 0) Then
                Session("txtActiveStatus") = "Y"
            Else
                Session("txtActiveStatus") = "N"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click

        Dim Str As String
        Dim KitId As String = ""
        Dim JoinColr As String = ""


        Dim dt As DataTable = New DataTable
        Dim ds As DataSet = New DataSet
        Str = "select sessid from M_FundMaster where sessid='" & ddlSession.SelectedValue & "' "
        ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Str)
        dt = ds.Tables(0)
        If (dt.Rows.Count > 0) Then

            scrname = "<SCRIPT language='javascript'>alert('You Are Already Add.!');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Exit Sub
        Else
            Str = "Insert into M_FundMaster(MasterRepurch,BrandAmount,ActiveStatus,RecTimeStamp,sessid)values(" & _
      "" & Val(txtMasterRepurehes.Text) & "," & Val(txtBrandAmount.Text) & ",'Y',getdate(),'" & ddlSession.SelectedValue & "') "
            Dim updateEffect As Integer = 0
            updateEffect = objDAL.UpdateData(Str)
            'If String.IsNullOrEmpty(Request("fId")) = False And updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!!');" & "</SCRIPT>"
            'ElseIf updateEffect <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Save Successfully!! ');window.top.location.reload();" & "</SCRIPT>"
            'Else
            'scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!! ');" & "</SCRIPT>"
            'End If

            'scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        End If
    End Sub
    'Private Function Check_IdNo() As Boolean
    '    Try
    '        Sql = "Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl  From M_MONTHSessnmaster WHERE IDNO='" & Trim(TxtIDNo.Text) & "' and IsBlock='N'"
    '        Dim Dt_ As New DataTable
    '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

    '        Dt_ = objDAL.GetData(Sql)
    '        If Dt_.Rows.Count = 0 Then
    '            LblMemName.Text = " Please enter correct Member ID."
    '            LblMemName.ForeColor = Drawing.Color.Red
    '            TxtIDNo.Text = ""
    '            'BtnFundTransfer.Enabled = False
    '            Return False
    '        Else
    '            LblMemName.Text = Dt_.Rows(0)("MemName")
    '            LblMobl.Text = Dt_.Rows(0)("Mobl")
    '            LblMemName.ForeColor = Drawing.Color.Black
    '            TxtFormNo.Text = Dt_.Rows(0)("FormNo")
    '            'lblError.Text = ""
    '            ' BtnFundTransfer.Enabled = True
    '            Return True
    '        End If

    '    Catch ex As Exception

    '    End Try

    'End Function
    Private Sub ClearAll()

        txtMasterRepurehes.Text = ""



    End Sub
End Class
