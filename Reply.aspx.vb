Imports System.Data
Imports System.Data.SqlClient
Partial Class App_UI_Application_Pages_Reply
    Inherits System.Web.UI.Page
    Dim Dt As New DataTable
    Dim scrname As String
    Dim objDAL As DAL
    Dim DbConnect As cls_DataAccess
    Dim objModuleFun As ModuleFunction
    Dim CIdQS As String
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
        Dim str = "exec('Create table TrnReplyadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
"ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[TrnReplyadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
        Dim i As Integer = 0
        i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)


        If String.IsNullOrEmpty(Request("CId")) = False Then
            CIdQS = Request("CId")
            If Not Page.IsPostBack Then
                HdnCheckTrnns.Value = GenerateRandomString(6)
                If Session("AStatus") = "OK" Then
                    If String.IsNullOrEmpty(Request("CId")) = False Then
                        BindData()
                    End If
                Else
                    scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                End If
            End If
        End If
        'txtIPAdrs.Text = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
        '  txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress()
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

    Private Sub BindData()
        CIdQS = (Request("CId").Trim)
        Dim sql As String = "  Select M.IDNo,M.MemName,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate FROM" & _
" (Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*" & _
"  FROM M_ComplaintMaster as a,M_MemberMaster as b WHERE a.IDNo=b.IDNo AND a.CID='" & CIdQS & "') as M LEFT JOIN M_SolutionMaster as S" & _
"  ON M.CID=S.CID "
        Dt = New DataTable
        Dt = objDAL.GetData(sql)
        If Dt.Rows.Count > 0 Then
            LblMemName.Text = Dt.Rows(0)("MemName") & "[" & Dt.Rows(0)("IDNo") & "]"
            LblCType.Text = Dt.Rows(0)("CType")
            TxtComplaint.Text = Dt.Rows(0)("Complaint")
            TxtPreReply.Text = ""
            For i As Integer = 0 To Dt.Rows.Count - 1
                If TxtPreReply.Text <> "" Then
                    TxtPreReply.Text = TxtPreReply.Text & Environment.NewLine & "-----------------------------------------" & Environment.NewLine
                End If
                If Trim(Dt.Rows(i)("Solution").ToString()) <> "" Then
                    TxtPreReply.Text = TxtPreReply.Text & Dt.Rows(i)("SDate") & ": " & Environment.NewLine & Dt.Rows(i)("Solution")
                End If
            Next

        End If
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim updateeffect As Integer
        Dim StrSql As String = "Insert into TrnReplyadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
        updateeffect = objDAL.SaveData(StrSql)

        If updateeffect > 0 Then


            Dim Sql As String
            CIdQS = Request("CId")
            Sql = "UPDATE M_ComplaintMaster SET IsReplied='Y' WHERE CID='" & CIdQS & "'; Insert into M_SolutionMaster(CId,Solution) VALUES (" & CIdQS & ",@Solution)"
            DbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            DbConnect.OpenConnection()
            Dim Cmd As New SqlCommand(Sql, DbConnect.cnnObject)
            Cmd.Parameters.AddWithValue("@Solution", Trim(TxtReply.Text))
            Dim UpdtEffect As Integer = Cmd.ExecuteNonQuery()
            If UpdtEffect = 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Reply not sent. ');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Reply has been sent successfully. ');" & "</SCRIPT>"
                TxtComplaint.Text = "" : TxtReply.Text = ""
                'SendMail()
            End If
            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
        Else
            scrname = "<SCRIPT language='javascript'>alert('This complaint already replied.');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
            scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Exit Sub
        End If
       
    End Sub


    Private Sub ClearAll()
        LblMemName.Text = "" : LblCType.Text = ""
        TxtComplaint.Text = ""
        TxtPreReply.Text = ""
        TxtReply.Text = ""
    End Sub
End Class
