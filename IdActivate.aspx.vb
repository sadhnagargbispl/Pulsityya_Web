Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class App_UI_Application_Pages_IdActivate
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim Sql As String = ""
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Adp As SqlDataAdapter
    Dim dRead As SqlDataReader
    Dim Ds As New DataSet
    Dim dt As New DataTable
    Dim StrQuery As String
    Dim ScrName As String
    Dim objDAL As DAL
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Conn.Open()

            Dim str = "exec('Create table Trnactivecadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," & _
"ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnactivecadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')"
            Dim i As Integer = 0
            i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)


            If Session("AStatus") = "OK" Then
                Session("PageName") = "Member / ID Activate "
                If Not Page.IsPostBack Then
                    HdnCheckTrnns.Value = GenerateRandomStringAdmin(6)
                    FillKit()
                    If Session("compid") = 1064 Then
                        utr.Visible = False
                        remark.Visible = True
                        droputr.Visible = True
                        crntBV.Visible = True
                    Else
                        utr.Visible = False
                        remark.Visible = False
                        crntBV.Visible = False
                        droputr.Visible = False
                    End If
                End If
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ''obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Public Sub FillKit(Optional ByVal condition As String = "")
        Try
            Sql = " select * from (select 0 as Kitid,'--Choose Package--' as KitName,0 as TopupSeq Union all "
            Sql &= "Select kitId,KitName,TopUpSeq From M_KitMaster Where Activestatus='Y' and  RowStatus='Y' and kitid in (3) )as a "
            Sql &= "where  1=1 " & condition & " Order By kitId"
            Dim Dt As New DataTable
            Dt = obj.GetData(Sql)
            DDlKit.DataSource = Dt
            DDlKit.DataTextField = "KitName"
            DDlKit.DataValueField = "KitId"
            DDlKit.DataBind()
            Session("MKit") = Dt
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ''obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub
    Private Function Check_IdNo() As Boolean
        Try
            Dim billtype As String = ""
            Dim dt1 As New DataTable
            dt1 = DirectCast(Session("MKit"), DataTable)
            Dim Dr() As DataRow = dt1.Select("KitID='" & DDlKit.SelectedValue & "'")
            Sql = "Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName,IsNull(c.Idno,'') as SponsorId,"
            Sql &= " isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,"
            Sql &= "a.LegNo,B.KitName,a.BV,b.bv as KBv,Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') "
            Sql &= "else '' end as UpgradeDate,a.ActiveStatus,a.FLD1,a.Planid,a.isblock, a.Fld4 from M_KitMaster as b,M_MemberMaster as a "
            Sql &= "Left Join M_MemberMaster as c on a.RefFormno=c.Formno  "
            Sql &= " where a.KitId=b.KitId and  (b.RowStatus='Y')  and a.IDNo='" & TxtIDNo.Text & "' and a.IsBlock='N'"
            Dim Dt_ As New DataTable
            Dt_ = obj.GetData(Sql)
            If Dt_.Rows(0)("isblock") = "Y" Then
                lblError.Text = "ID is blocked. Please unblock it for activation"
                lblError.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                LblMemName.Text = ""
                LblCondition.Text = ""
                LblKitName.Text = ""
                LblNewKitid.Text = ""
                GrdDirects1.Visible = False
                BtnUpgrade.Enabled = False
                Return False
            ElseIf Dt_.Rows.Count = 0 Then
                lblError.Text = " Please enter correct Member ID."
                lblError.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                LblMemName.Text = ""
                LblCondition.Text = ""
                LblKitName.Text = ""
                LblNewKitid.Text = ""
                GrdDirects1.Visible = False
                BtnUpgrade.Enabled = False
                Return False
            Else
                If (Session("CompID") = "1039") Then
                    If (Dt_.Rows(0)("ActiveStatus") = "N") Then
                        LblKitId.Text = Dt_.Rows(0)("KitId")
                        GrdDirects1.DataSource = Dt_
                        GrdDirects1.DataBind()
                        GrdDirects1.Visible = True
                        lblError.Text = ""
                        LblMemName.Text = Dt_.Rows(0)("MemName")
                        LblFormno.Text = Dt_.Rows(0)("Formno")
                        LblKitName.Text = Dt_.Rows(0)("KitName")
                        LblCondition.Text = "and kitID  =  12"
                        LblMemName.ForeColor = Drawing.Color.Black
                        BtnUpgrade.Enabled = True
                        Return True
                    Else
                        LblKitId.Text = Dt_.Rows(0)("KitId")
                        GrdDirects1.DataSource = Dt_
                        GrdDirects1.DataBind()
                        GrdDirects1.Visible = True
                        lblError.Text = ""
                        LblMemName.Text = Dt_.Rows(0)("MemName")
                        TxtSponsorid.Text = Dt_.Rows(0)("SponsorId")
                        TxtSponsorName.Text = Dt_.Rows(0)("SponsorName")
                        If (Session("CompID") = "1064") Then
                            LblCrntBV.Text = Dt_.Rows(0)("KBv")
                        End If
                        LblFormno.Text = Dt_.Rows(0)("Formno")
                        If Session("CompId") = 1083 Then
                            If (Dt_.Rows(0)("ActiveStatus") = "Y") Then
                                ScrName = "<SCRIPT language='javascript'>alert('This id Already Activate!! ');" & "</SCRIPT>"
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", ScrName, False)
                            Else
                                LblCondition.Text = ""
                                Return True
                            End If
                        ElseIf Session("CompId") = 1084 Then
                            If Dt_.Rows(0)("Is_FranKit") = "N" Then
                                LblCondition.Text = "and TopupSeq>='" & Dt_.Rows(0)("TopupSeq") & "'"
                            Else
                                LblCondition.Text = "and Is_FranKit='" & Dt_.Rows(0)("Is_FranKit") & "'"
                            End If
                            Return True
                        ElseIf Session("CompId") = 1091 Then
                            If (Dt_.Rows(0)("ActiveStatus") = "Y") Then
                                ScrName = "<SCRIPT language='javascript'>alert('This id Already Activate!! ');location.replace('IdActivate.aspx');" & "</SCRIPT>"
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", ScrName, False)
                            Else
                                LblCondition.Text = ""
                                Return True
                            End If
                        Else
                            LblKitName.Text = Dt_.Rows(0)("KitName")
                            If Session("CompId") = "1081" Then
                                If Dt_.Rows(0)("Is_FranKit") = "N" Then
                                    LblCondition.Text = "and TopupSeq>='" & Dt_.Rows(0)("TopupSeq") & "'"
                                Else
                                    LblCondition.Text = "and Is_FranKit='" & Dt_.Rows(0)("Is_FranKit") & "'"
                                End If
                            End If
                            If (Session("CompID") = "1064") Then
                                If Dt_.Rows(0)("Fld4") = "3" And LblKitId.Text = "1" Then
                                    LblCondition.Text = "and Kitid=10"
                                ElseIf Dt_.Rows(0)("Fld4") = "4" And LblKitId.Text = "1" Then
                                    LblCondition.Text = "and Kitid=13"
                                ElseIf Dt_.Rows(0)("Fld4") = "2" And LblKitId.Text = "1" Then
                                    LblCondition.Text = "and Kitid=7"
                                ElseIf Dt_.Rows(0)("Fld4") = "5" And LblKitId.Text = "1" Then
                                    LblCondition.Text = "and Kitid=31"
                                End If
                            End If
                            LblMemName.ForeColor = Drawing.Color.Black
                            BtnUpgrade.Enabled = True
                            Return True
                        End If
                    End If
                Else
                    LblKitId.Text = Dt_.Rows(0)("KitId")
                    GrdDirects1.DataSource = Dt_
                    GrdDirects1.DataBind()
                    GrdDirects1.Visible = True
                    lblError.Text = ""
                    LblMemName.Text = Dt_.Rows(0)("MemName")
                    TxtSponsorid.Text = Dt_.Rows(0)("SponsorId")
                    TxtSponsorName.Text = Dt_.Rows(0)("SponsorName")
                    If (Session("CompID") = "1064") Then
                        LblCrntBV.Text = Dt_.Rows(0)("KBv")
                    End If
                    LblFormno.Text = Dt_.Rows(0)("Formno")
                    LblKitName.Text = Dt_.Rows(0)("KitName")

                    If (Session("CompID") = "1103") Then
                        Dim Dt_Kit As DataTable = New DataTable()
                        Dim s1 As String = "Exec GetBillTypeByRepurchase '" & LblFormno.Text & "','" & Val(LblKitId.Text) & "'"
                        Dt_Kit = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, s1).Tables(0)

                        If Dt_Kit.Rows.Count > 0 Then
                            billtype = Dt_Kit.Rows(0)("BillType")
                            If billtype.ToString().ToUpper() = "R" Then
                                LblCondition.Text = "and TopupSeq = '" & Dt_.Rows(0)("TopupSeq") & "'"
                            Else
                                LblCondition.Text = "and TopupSeq > '" & Dt_.Rows(0)("TopupSeq") & "'"
                            End If
                        Else
                            LblCondition.Text = "and TopupSeq > '" & Dt_.Rows(0)("TopupSeq") & "'"
                        End If

                    Else
                        LblCondition.Text = "and TopupSeq > '" & Dt_.Rows(0)("TopupSeq") & "'"
                    End If
                    Return True
                    If Session("CompId") = 1083 Then
                        If (Dt_.Rows(0)("ActiveStatus") = "Y") Then
                            ScrName = "<SCRIPT language='javascript'>alert('This id Already Activate!! ');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", ScrName, False)
                        Else
                            LblCondition.Text = ""
                            Return True
                        End If
                    ElseIf Session("CompId") = 1084 Then
                        If Dt_.Rows(0)("Is_FranKit") = "N" And (Dt_.Rows(0)("ActiveStatus") = "N") Then
                            LblCondition.Text = "and TopupSeq>'" & Dt_.Rows(0)("TopupSeq") & "'"
                        ElseIf Dt_.Rows(0)("Is_FranKit") = "N" And (Dt_.Rows(0)("ActiveStatus") = "Y") Then
                            LblCondition.Text = "and TopupSeq>='" & Dt_.Rows(0)("TopupSeq") & "'"
                        Else
                            LblCondition.Text = "and Is_FranKit='" & Dt_.Rows(0)("Is_FranKit") & "'"
                        End If
                        Return True


                    ElseIf Session("CompId") = 1091 Then
                        If (Dt_.Rows(0)("ActiveStatus") = "Y") Then
                            ScrName = "<SCRIPT language='javascript'>alert('This id Already Activate!! ');location.replace('IdActivate.aspx');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", ScrName, False)
                        Else
                            'LblCondition.Text = ""
                            LblCondition.Text = "and TopupSeq>'" & Dt_.Rows(0)("TopupSeq") & "'"
                            Return True
                        End If
                    Else
                        If (Session("CompID") = "1057") Then
                            LblCondition.Text = ""
                        Else
                            LblCondition.Text = "and TopupSeq>'" & Dt_.Rows(0)("TopupSeq") & "'"
                        End If
                        If Session("CompId") = "1081" Then
                            If Dt_.Rows(0)("Is_FranKit") = "N" And (Dt_.Rows(0)("ActiveStatus") = "N") Then
                                LblCondition.Text = "and TopupSeq>'" & Dt_.Rows(0)("TopupSeq") & "'"
                            ElseIf Dt_.Rows(0)("Is_FranKit") = "N" And (Dt_.Rows(0)("ActiveStatus") = "Y") Then
                                LblCondition.Text = "and TopupSeq>='" & Dt_.Rows(0)("TopupSeq") & "'"
                            Else
                                LblCondition.Text = "and Is_FranKit='" & Dt_.Rows(0)("Is_FranKit") & "'"
                            End If
                        End If
                        If (Session("CompID") = "1064") Then
                            If Dt_.Rows(0)("Fld4") = "3" And LblKitId.Text = "1" Then
                                LblCondition.Text = "and Kitid=10"
                            ElseIf Dt_.Rows(0)("Fld4") = "4" And LblKitId.Text = "1" Then
                                LblCondition.Text = "and Kitid=13"
                            ElseIf Dt_.Rows(0)("Fld4") = "2" And LblKitId.Text = "1" Then
                                LblCondition.Text = "and Kitid=7"
                            ElseIf Dt_.Rows(0)("Fld4") = "5" And LblKitId.Text = "1" Then
                                LblCondition.Text = "and Kitid=31"
                            End If
                        End If
                        LblMemName.ForeColor = Drawing.Color.Black
                        BtnUpgrade.Enabled = True
                        Return True
                    End If
                End If
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Function
    Protected Function CheckAmount() As Boolean
        Try
            Dim Dt As DataTable
            Dim str As String
            str = "Select * From dbo.ufnGetBalance('" & LblFormno.Text & "','R')"
            Dt = New DataTable
            Dt = objDAL.GetData(str)
            If Dt.Rows(0)("Balance") > 0 Then
                AvailableBal.InnerText = Dt.Rows(0)("Balance")
                AvailableBal.Visible = True
            Else
                AvailableBal.InnerText = 0.0
                AvailableBal.Visible = True
                ScrName = "<SCRIPT language='javascript'>alert('Insufficient Balance!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", ScrName, False)
            End If
            Session("ServiceWallet") = AvailableBal.InnerText
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            objDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Function
    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Try
            If Check_IdNo() = True Then
                FillKit(LblCondition.Text.Trim)
                If Val(DDlKit.SelectedValue) = 0 Then
                    BtnUpgrade.Enabled = False
                Else
                    BtnUpgrade.Enabled = True
                End If
            Else
                BtnUpgrade.Enabled = False
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Sub
    Protected Function Updtmaster() As Boolean
        Try
            Dim strQry, Result As String
            Dim formno As String = ""
            Dim Partycode As String = ""
            Dim Address As String = ""
            Try
                Dim updateeffect As Integer
                Dim StrSql As String = "INSERT INTO Trnactivecadmin(Transid,Rectimestamp)VALUES(" & HdnCheckTrnns.Value & ",GETDATE())"
                updateeffect = objDAL.SaveData(StrSql)
                If updateeffect > 0 Then
                    Result = "Hello"
                    Dim s As String = ""
                    If (Session("compid") = 1059) Then
                        strQry = "Exec Sp_activateId_MyShopy '" & Trim(TxtIDNo.Text) & "'," & Val(DDlKit.SelectedValue) & ";"
                    ElseIf Session("compid") = 1064 Then
                        strQry = "Exec Sp_activateId_BlueCity '" & Trim(TxtIDNo.Text) & "'," & Val(DDlKit.SelectedValue) & ",'" & Trim(TxtUTRno.Text) & "','" & Trim(TxtUTRRemark.Text) & "';"
                    ElseIf Session("compid") = 1081 Then
                        Dim Bill_No As String = GenerateRandomString(6)
                        strQry = "exec Sp_activateId '" & Trim(TxtIDNo.Text) & "','" & Val(DDlKit.SelectedValue) & "','" & Val(LblFormno.Text) & "','" & Bill_No & "'"
                    ElseIf Session("compid") = 1084 Then
                        Dim Bill_No As String = GenerateRandomString(6)
                        strQry = "exec Sp_activateId '" & Trim(TxtIDNo.Text) & "','" & Val(DDlKit.SelectedValue) & "','" & Val(LblFormno.Text) & "','" & Bill_No & "'"
                    ElseIf Session("compid") = "1089" Then
                        Dim Bill_No As String = GenerateRandomString(6)
                        strQry = "exec Sp_ActivateMemberMegaAdmin '" & Trim(TxtIDNo.Text) & "','" & Val(DDlKit.SelectedValue) & "',"
                        strQry &= "'" & Val(LblFormno.Text) & "','" & Bill_No & "','" & Session("UserName") & "'"
                    ElseIf Session("compid") = 1101 Then
                        Dim Bill_No As String = GenerateRandomString(6)
                        strQry = "exec Sp_activateId '" & Trim(TxtIDNo.Text) & "'," & Val(DDlKit.SelectedValue) & ",'" & Bill_No & "'"
                    ElseIf (Session("CompID") = "1103") Then
                        Dim Dt_Kit As DataTable = New DataTable()
                        Dim s1 As String = "Exec GetBillTypeByRepurchase '" & LblFormno.Text & "','" & Val(DDlKit.SelectedValue) & "'"
                        Dt_Kit = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, s1).Tables(0)
                        If Dt_Kit.Rows.Count > 0 Then
                            Dim Bill_No As String = GenerateRandomString(6)
                            strQry = "exec Sp_activateId '" & Trim(TxtIDNo.Text) & "'," & Val(DDlKit.SelectedValue) & ",'" & Bill_No & "','" & Dt_Kit.Rows(0)("BillType") & "'"
                            'Dim Bill_No As String = GenerateRandomStringactive(6)
                            'Sql = "exec Sp_ActivateMember '" & txtMemberId.Text.Trim & "','" & Val(CmbKit.SelectedValue) & "',"
                            'Sql &= "'" & Val(Session("Formno")) & "','" & Bill_No & "','" & Dt_Kit.Rows(0)("BillType") & "'"
                        End If
                    ElseIf (Session("CompID") = "1095") Then
                        Dim Bill_No As String = GenerateRandomString(6)
                        strQry = "exec Sp_activateId '" & Trim(TxtIDNo.Text) & "'," & Val(DDlKit.SelectedValue) & ",'" & Bill_No & "'"
                    ElseIf (Session("CompID") = "1108") Then
                        Dim Bill_No As String = GenerateRandomString(6)
                        strQry = "exec Sp_activateId '" & Trim(TxtIDNo.Text) & "'," & Val(DDlKit.SelectedValue) & ",'" & Bill_No & "'"
                    Else
                        strQry = "Exec Sp_activateId '" & Trim(TxtIDNo.Text) & "'," & Val(DDlKit.SelectedValue) & ";"
                    End If
                    Dim i As Integer = 0
                    i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, strQry))
                    If (i > 0) Then
                        Result = "SUCCESS"
                        If Result Like "SUCCESS" Then
                            Updtmaster = True
                        Else
                            Updtmaster = False
                        End If
                        'Result = Dr("Result")
                        'Dr.Close()
                    End If
                   
                Else
                    Response.Redirect("IdActivate.aspx")
                End If
            Catch ex As Exception
                Updtmaster = False
            End Try
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Function
    Private Sub sendSMS(ByVal sms As String, ByVal Mobl As String)
        Try
            If Len(Mobl) >= 10 And IsNumeric(Mobl) = True Then
                Dim client As New WebClient
                Dim baseurl As String
                Dim data As Stream
                Try
                    baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & Mobl & "&SenderId=" & Session("ClientId") & ""
                    data = client.OpenRead(baseurl)
                    Dim reader As New StreamReader(data)
                    Dim s As String
                    s = reader.ReadToEnd()
                    data.Close()
                    reader.Close()
                Catch ex As Exception
                End Try
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ''obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Public Function activationMail(ByVal IdNo As String, ByVal Email As String, ByVal MemberName As String, ByVal Password As String, ByVal Mobl As String, ByVal Link As String) As Boolean
        Try
            Dim dt As DataTable
            Dim sql As String = ""
            Dim userEmail As String = ""
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(Email)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%""> " & _
                     "<tr>" & _
                     "<td>" & _
                     " <span style="" font-weight: bold;"">Date : " & Format(Now, "dd-MMM-yyyy") & " </span>" & _
                      " <br />" & _
                     " Welcome to " & Session("CompName") & " " & _
                     " <h4>Dear " & MemberName & ",</h4>" & _
                     " Congratulations, Your Account has been Activated with " & Session("CompName") & "." & _
                     " <br />" & _
                        "---------------------------------------------------------------------------------" & _
                     " <h2 style=""margin-top:5px;margin-bottom:5px"">Account Activation Information </h2>" & _
                       "----------------------------------------------------------------------------------" & _
                     "<br/>" & _
                     " Your " & Session("CompName") & "  ID No is: " & _
                     "<br/>" & _
                     " Login ID: " & IdNo.Trim.ToUpper & " " & _
                     "<br/>" & _
                     " Login Password : " & Password & "" & _
                     "<br/>" & _
                     " Members Login: " & Session("CompWeb1") & "" & _
                     " <br />" & _
                     "--------------------------------------------------------------------------------------" & _
                     " <br />" & _
                     " PROTECT YOUR PASSWORD" & _
                     " <br />" & _
                     " NEVER give your password to anyone, including " & Session("CompName") & " staff. " & _
                     " Protect yourself against fraudulent websites by opening up a new web browser (e.g. Internet Explorer) and typing in the " & _
                     " " & Session("CompName") & " URL every time you log in to your account" & _
                     " <br />" & _
                     " -------------------------------------------------------------------------------------" & _
                     " </td>" & _
                      " <tr/> " & _
                     " <tr> " & _
                     " <td> " & _
                     " <b>With Best Regards </b>," & _
                     " <br /> " & _
                     "  VISIONROOTS SERVICES PVT. LTD. " & _
                     " <br /> " & _
                     "" & Session("CompWeb1") & " " & _
                     " <br /> " & _
                     " <br /> " & _
                     " Please do not reply to this email. This mailbox is not monitored and you will not receive a response. For assistance, use Contact Us link on our website or " & _
                     " send email to support@visionroots.info" & _
                     " <br />" & _
                     " <br />" & _
                      " You are receiving this email because you signed up to be our awesome associate. " & _
              " If you no longer wish to receive further mails from us you can unsubscribe here " & _
              " <a href=""" & Link & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">unsubscribe@visionroots.info</a> " & _
               " <br />" & _
 "(This is a computer generated email and does not require any signatures)" & _
                     " <br />" & _
                     " <br />" & _
                     " </td>" & _
                     " </tr>" & _
                     " </table>"

            MyMessage.Subject = " !!! " & Session("CompName") & " !!! Account Activation"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True

            Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ''obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Function
    Public Function UpgradeMail(ByVal IdNo As String, ByVal Email As String, ByVal MemberName As String, ByVal sponsorName As String, ByVal Kitamount As String, ByVal PackageName As String, ByVal Link As String) As Boolean
        Try
            Dim dt As DataTable
            Dim sql As String = ""
            Dim userEmail As String = ""


            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(Email)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%""> " & _
                     "<tr>" & _
                     "<td>" & _
                     " Date : " & Format(Now, "dd-MMM-yyyy") & " " & _
                     "<br/>" & _
                     " Welcome to " & Session("CompName") & " " & _
                     " <h4>Dear " & MemberName & ",</h4>" & _
                     " Congratulations, Your Account has been Upgraded with " & Session("CompName") & "." & _
                     " <br />" & _
                      "--------------------------------------------------------------------------------" & _
                                 " <h2 style=""margin-top:5px;margin-bottom:5px"">Account Upgrade Confirmation </h2>" & _
                     "---------------------------------------------------------------------------------" & _
                     "<br/>" & _
                     " Your " & Session("CompName") & "  ID No is: " & _
                     "<br/>" & _
                     " Login ID: " & IdNo.Trim.ToUpper & " " & _
                     "<br/>" & _
                     " Package Name: " & PackageName & "" & _
                     "<br/>" & _
                     " Amount: " & Kitamount & "" & _
                     "<br/>" & _
                     " Sponsor Name: " & sponsorName & "" & _
                     "<br/>" & _
                   "-----------------------------------------------------------------------------------" & _
                     " <br />" & _
                     " Assuring you of best services always and wishing you continued success in your journey with " & Session("CompName") & ". " & _
                     " <br />" & _
                    " We look forward to a long term association and prosperous future together." & _
                     " <br />" & _
                     "</td>" & _
                     "</tr>" & _
                     "<tr>" & _
                     "<td>" & _
                     "<br/>" & _
                    " With Best Regards, " & _
                    "<br/>" & _
                    " VISIONROOTS SERVICES PVT. LTD. " & _
                    "<br/>" & _
                     " " & Session("CompWeb1") & "" & _
                     "<br/>" & _
                     "<br/>" & _
                    "Please do not reply to this email. This mailbox is not monitored and you will not receive a response." & _
                    "<br/>" & _
                    " For assistance, use Contact Us link on our website or send email to support@visionroots.info" & _
             "<br/>" & _
              "<br/>" & _
              " You are receiving this email because you signed up to be our awesome associate. " & _
              " If you no longer wish to receive further mails from us you can unsubscribe here " & _
              " <a href=""" & Link & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">unsubscribe@visionroots.info</a> " & _
 "(This is a computer generated email and does not require any signatures)" & _
                     " <br />" & _
                     " <br />" & _
                     " </td>" & _
                     " </tr>" & _
                     " </table>"

            MyMessage.Subject = " !!! " & Session("CompName") & " !!! VRID Upgrade Confirmation"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True

            Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ''obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Function
    Protected Sub BtnUpgrade_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpgrade.Click
        Try
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
        Dim scrname As String
        Dim Remark As String
        Remark = " Package Upgrade of Idno:" & TxtIDNo.Text & ""
        Try
            lblError.Text = ""
            If Session("compid") = 1064 Then
                Dim s1 As String = ""
                If TxtUTRno.Text <> "" Then
                    s1 = "  select Count(UTRNO) as UTRNO from RepurchIncome where UTRNO='" & TxtUTRno.Text.Trim & "'"
                    objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    Dim Dt1 As DataTable
                    Dt1 = New DataTable
                    Dt1 = objDAL.GetData(s1)
                    If Dt1.Rows(0)("UTRNO") > 0 Then
                        scrname = "<SCRIPT language='javascript'>alert('Already Activated by this UTRNO No.');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                        TxtUTRno.Text = ""
                        Exit Sub
                    End If
                End If
            End If
            If Trim(TxtIDNo.Text) = "" Then
                lblError.Text = "Enter Member ID."
                Exit Sub
            Else
                If Check_IdNo() = False Then
                    lblError.Text = "Invalid Member ID."
                    Exit Sub
                End If
                If DDlKit.SelectedValue = 0 Then
                    scrname = "<SCRIPT language='javascript'>alert('Choose Package Name');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                    Exit Sub
                End If
                If Updtmaster() Then
                    clear()
                    scrname = "<SCRIPT language='javascript'>alert('Successfully Id Activated');;location.replace('IdActivate.aspx');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                Else
                    scrname = "<SCRIPT language='javascript'>alert(' Id Activated Unsuccessfully');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
                End If
            End If
        Catch ex As Exception
            scrname = "<SCRIPT language='javascript'>alert('" & ex.Message & "');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Error", scrname, False)
        End Try
    End Sub
    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Try

            clear()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            '' obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try

    End Sub
    Protected Sub clear()
        TxtIDNo.Text = "" : LblMemName.Text = "" : lblError.Text = "" : BtnUpgrade.Enabled = False
        GrdDirects1.Visible = False : LblFormno.Text = "" : LblKitId.Text = ""
        LblKitName.Text = "" : LblNewKitid.Text = "" : TxtSponsorid.Text = "" : TxtSponsorName.Text = ""
        : TxtUTRno.Text = "" : TxtUTRRemark.Text = ""
        FillKit()

    End Sub




    Protected Sub DropDownList1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.TextChanged
        If (DropDownList1.Text) = "O" Then
            utr.Visible = True

        Else
            utr.Visible = False

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

    Public Function GenerateRandomStringAdmin(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function
End Class
