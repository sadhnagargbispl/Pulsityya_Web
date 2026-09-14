Imports System.Data
Imports System.Data.SqlClient
Partial Class UpdateBoNanzaPaid
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
            Dim str = "exec('Create table TrnBoNanzaUpdate ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,"
            str &= "PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,"
            str &= "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] "
            str &= "ALTER TABLE [dbo].[TrnBoNanzaUpdate] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
            Dim i As Integer = 0
            i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
        Catch ex As Exception
        End Try
        If String.IsNullOrEmpty(Request("Offerid")) = False Then
            UserIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("Offerid")))
        End If
        If Not Page.IsPostBack Then
            HdnCheckTrnns.Value = GenerateRandomStringJoining(6)
            ClearAll()
            txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
            If String.IsNullOrEmpty(Request("Offerid")) = False Then
                'BindData()
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
    Private Sub ClearAll()
        txtRemarks.Text = ""
        txtActiveStatus.Text = ""
        txtIPAdrs.Text = ""
        txtFrmDate.Text = ""
    End Sub
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Strquery = "Insert into TrnBoNanzaUpdate (Transid) values(" & HdnCheckTrnns.Value & ")"
        Dim isOk1 As Integer = 0
        isOk1 = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Strquery)
        If isOk1 > 0 Then
            Dim Sql As String
            Dim FrmDate As String
            FrmDate = txtFrmDate.Text
            Try
                Dim Dt As DateTime = FrmDate
            Catch ex As Exception
                scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" & "</SCRIPT>"
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", scrname, True)
                Exit Sub
            End Try
            Sql = "Update BonanzaDetail SET PaidStatus = 'Y',Remark = '" & txtRemarks.Text & "',PaidDate = '" & txtFrmDate.Text & "',"
            Sql &= "LastModifyDate = 'Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' Where OfferID  = '" & Request("Offerid") & "' "
            Sql &= "  AND id = '" & Request("aid") & "' AND formno = '" & Request("formno") & "' AND OfferType = '" & Request("Offertype") & "' ;"
            Sql &= "insert into TempBonanzaDetail(FormNo,OfferID,MatchingRp,LeftRp,RightRp,OfferType,ReqDirectBV,AchieveBV,AchieveStatus,"
            Sql &= "Remark,PaidDate,PaidStatus,LastModified)"
            Sql &= " select FormNo,OfferID,MatchingRp,LeftRp,RightRp,OfferType,ReqDirectBV,AchieveBV,AchieveStatus,"
            Sql &= " '" & txtRemarks.Text & "','" & txtFrmDate.Text & "',"
            Sql &= "'Y','Modified by " & Session("UserName") & " at " & DateTime.Now.ToString() & "' from BonanzaDetail "
            Sql &= " Where OfferID  = '" & Request("Offerid") & "' AND id = '" & Request("aid") & "' AND formno = '" & Request("formno") & "'"
            Sql &= " AND OfferType = '" & Request("Offertype") & "'  "
            Dim updateEffect As Integer = 0
            updateEffect = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, Sql)
            If (updateEffect > 0) Then
                scrname = "<SCRIPT language='javascript'>alert('Successfully Updated.!');location.replace('UpdateBoNanzaPaid.aspx');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Data not Updated Successfully!! ');location.replace('UpdateBoNanzaPaid.aspx');" & "</SCRIPT>"
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('This News already Updated.!');location.replace('BonanzaReportWellValue.aspx');", True)
            Exit Sub
        End If
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        ClearAll()
    End Sub
End Class
