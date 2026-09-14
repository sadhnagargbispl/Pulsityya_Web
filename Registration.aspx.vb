
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Globalization
Imports System.Net.Mail

Partial Class Registration
    Inherits System.Web.UI.Page
    Dim _dblAvailLeg As Double = 0
    Private dbGeneral As New clsGeneral
    Private dbConnect As cls_DataAccess
    Dim ObjDAL As DAL
    Private cmd As New SqlCommand
    Private dRead As SqlDataReader
    Public DsnName, UserName, Passw As String
    Private strQuery, strCaptcha As String
    Dim tmpTable As New Data.DataTable
    Dim QryCls As New AccClass.MyAccClass.NewClass
    Dim minSpnsrNoLen, minScrtchLen As Integer
    Dim Upln, dblSpons, dblState, dblBank, dblIdNo As Double
    Dim dblDistrict, dblTehsil, IfSC As String
    Dim dblPlan As String
    Dim CurrDt As DateTime
    Dim scrname As String
    Dim LastInsertID As String = ""
    Dim InVoiceNo As String
    Dim SupplierId As Integer
    Dim BillNo As String
    Dim TaxType As String
    Dim BillDate As String
    Dim SBillNo As Integer
    Dim SoldBy As String = "WR"
    Dim FType As String
    Dim objGen As clsGeneral = New clsGeneral
    Private Function ClearInject(ByVal StrObj As String) As String
        StrObj = Replace(Replace(Replace(StrObj, ";", ""), "'", ""), "=", "")
        Return Trim(StrObj)
    End Function
    Public Function GetCompID() As String
        Dim url As String = String.Empty
        Dim conn As SqlConnection

        Try
            url = HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("CPANEL.", "").Replace("LOGIN.", "")
            'Dim str As String = String.Empty
            'url = HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("CPANEL.", "")
            Dim str As String = String.Empty
            ''str = " Select ID,Logo from M_CompanyMasterNew Where IsActive = 1 And (Upper(URL) = '" & url.ToUpper().Trim() & "' OR  Upper(URL) = 'LOCALHOST') "

            If url = "LOCALHOST" Then
                str = " Select ID,Logo,PartyCode from M_CompanyMasterNew Where IsActive = 1 And ID='" & ConfigurationManager.AppSettings("CompanyID") & "'"
            Else
                str = " Select ID,Logo,PartyCode from M_CompanyMasterNew Where IsActive = 1 And (Upper(URL) = '" & url.ToUpper().Trim() & "') "
            End If

            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand
            conn = New SqlConnection(Application("sConnect"))
            conn.Open()

            cmd = New SqlCommand(str, conn)
            dRead = cmd.ExecuteReader
            If dRead.Read Then

                Session("CompID") = dRead("ID")

                Session("Logo") = dRead("Logo")
                ''imgLogo.Src = "http://superadmin.bisplindia.in/images/Logo/" & dRead("Logo")
                Session("WRPartyCode") = dRead("PartyCode")


            End If
            dRead.Close()
            conn.Close()

        Catch ex As Exception
            If Not conn Is Nothing Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
        End Try
        GetCompID = url
    End Function
    Private Sub Pages()
        Try
            Dim dtMenu As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            ''And  a.ActiveStatus = 'Y' And a.RowStatus ='Y'
            Dim str As String = " Select a.MenuId as MenuId, a.MenuName as MenuName,a.ParentId as ParentId, a.OnSelect as OnSelect,ActiveStatus from "
            str &= " M_CompWiseWebMenuMasterDis a Where MenuId in (1,3)  And a.CompanyID = '" & HttpContext.Current.Session("CompID") & "' order by Convert(decimal,RTRIM(LTRIM(a.Hierar))),a.MenuId"
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dtMenu = ds.Tables(0)
            Session("IndexPage") = dtMenu.Rows(0)("OnSelect")
            Session("ActiveStatusJoin") = dtMenu.Rows(0)("ActiveStatus")
            Session("JoinPage") = dtMenu.Rows(1)("OnSelect")
            ''aJoining.HRef = Session("JoinPage")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ColumnName()
        Try
            Dim dtMenu As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            Dim str As String = " Select ColName1,ColName2,ColName3 "
            str &= " from M_CompanyColSetting a  Where  a.CompanyID = '" & HttpContext.Current.Session("CompID") & "'"
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dtMenu = ds.Tables(0)
            Session("ColName1") = dtMenu.Rows(0)("ColName1")
            Session("ColName2") = dtMenu.Rows(0)("ColName2")
            Session("ColName3") = dtMenu.Rows(0)("ColName3")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub GetSmsTemplate()
        Try
            Dim dtMenu As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            Dim str As String = " exec Sp_GetSmsTemplate '" & HttpContext.Current.Session("CompID") & "' "
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dtMenu = ds.Tables(0)
            If (dtMenu.Rows.Count > 0) Then
                Session("JoiningSms") = dtMenu.Rows(0)("JoiningSms")
            Else
                Session("JoiningSms") = ""
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub GetPageSetting()
        Try
            Dim dt As DataTable = New DataTable
            Dim ds As DataSet = New DataSet
            Dim str As String = " Select * "
            str &= " from M_NewJoingPageCompWise Where CompID = '" & HttpContext.Current.Session("CompID") & "' and isActive= 1  "
            ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
            dt = ds.Tables(0)
            For counter As Integer = 0 To dt.Rows.Count
                Session(dt.Rows(counter)(2).ToString) = dt.Rows(counter)(2) & "|" & dt.Rows(counter)(3) & "|" & dt.Rows(counter)(4) & "|" & dt.Rows(counter)(5) & "|" & dt.Rows(counter)(6) & "|" & dt.Rows(counter)(7) & "|" & dt.Rows(counter)(8) & "|" & dt.Rows(counter)(9)
            Next counter
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim tempUrl As String = HttpContext.Current.Request.Url.ToString()
            If Session("TransID") = "" Then
                Session("TransID") = Format(DateTime.Now(), "yyyyMMddhhmmssfff")
            End If
            If (tempUrl.ToString().ToUpper().Contains("TRANSID") = False And tempUrl.ToString().ToUpper().Contains("?") = True) Then
                Session("TransID") = Format(DateTime.Now(), "yyyyMMddhhmmssfff")
                tempUrl = tempUrl & "&TransID=" & Session("TransID")
                Try
                    Response.Redirect(tempUrl, False)
                    Response.End()
                Catch ex As Exception
                End Try
                Exit Sub
            End If
            HiddenCompID.Value = Session("compid")
            If (Session("ActiveStatusJoin") = "N") Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please contact to admin.!');location.replace('Logout.aspx');", True)
                Exit Sub
            End If
            dbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dbConnect.OpenConnection()
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            txtUplinerId.Text = Replace(Replace(Replace(Trim(txtUplinerId.Text), "'", ""), "=", ""), ";", "")
            Dim sr As String = ""
            Dim sbstr() As String
            Dim Key As String = ""
            Dim K As String = ""
            If Not Page.IsPostBack Then
                GetSmsTemplate()
                Fill_State()
                If Session("CompID") = "1091" Or Session("CompID") = "1100" Then
                    DivUserName.Visible = True
                Else
                    DivUserName.Visible = False
                End If

                divAdhar.Visible = False

                If (Session("CompID") = 1007) Then
                    Divdob.Visible = True
                    divAddress.Visible = True
                    divTermVadic.Visible = True
                    divTermAll.Visible = False
                    TxtLife.Visible = False
                    txtAddLn1.Attributes.Add("Class", "form-control validate[required]")
                ElseIf Session("CompID") = "1091" Then
                    txtMobileNo.Attributes.Add("Class", "  validate[required,custom[mobile]]")
                ElseIf Session("CompID") = "1103" Then
                    'DivLeg1.Visible = False
                    txtMobileNo.CssClass &= " validate[required,custom[mobile]]"
                    lblmobilestyle.Text = "<span style='color:red !important;font-weight:bold;font-size:1.4em'>*</span>"
                    txtEMailId.CssClass &= " validate[required]"
                    lblemailstyle.Text = "<span style='color:red !important;font-weight:bold;font-size:1.4em'>*</span>"
                ElseIf Session("CompID") = "1100" Or Session("CompID") = "1103" Then
                    divPan.Visible = False
                Else
                    Divdob.Visible = False
                    divAddress.Visible = False
                    divTermAll.Visible = True
                    divTermVadic.Visible = False
                    txtAddLn1.Attributes.Add("Class", "form-control")
                    TxtLife.Visible = False
                    txtEMailId.Visible = True
                    divTermConditionNIGT.Visible = False
                    divPan.Visible = True
                End If

                If (Session("CompID") = 1084) Then
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6)
                End If
                If Session("CompID") = "1091" Or Session("CompID") = "1100" Or Session("CompID") = "1103" Then
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6)
                End If
                If Session("CompID") = 1091 Then
                    If txtPanNo.Text = "" Then
                        txtPanNo.Attributes.Add("CssClass", "form-control validate[custom[panno]]")
                    End If
                End If

                ClrCtrl()
                RbtnLegNo.Items.Add("Left")
                RbtnLegNo.Items.Add("Right")
                If Len(Request.QueryString("s")) > 0 Then
                    K = Request("s")
                    K = K.Replace(" ", "+")
                    sr = Crypto.Decrypt(K)
                    sbstr = sr.Split("/")
                    Dim UplinerFormno As String = sbstr(1)
                    Dim s As String = " select * from M_MemberMaster where Formno='" & UplinerFormno & "'"
                    Dim dt As DataTable
                    dt = New DataTable
                    ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    dt = ObjDAL.GetData(s)
                    If dt.Rows.Count > 0 Then
                        txtUplinerId.Text = dt.Rows(0)("Idno")
                    End If
                    Dim LegNo As String = sbstr(3)
                    txtUplinerId.ReadOnly = True
                    txtRefralId.Text = Session("Idno")
                    txtMobileNo.Text = Session("PhN1")
                    txtPhNo.Text = Session("Mobl")
                    txtEMailId.Text = Session("EMail")
                    If LegNo = 1 Then
                        RbtnLegNo.SelectedIndex = 0
                    Else
                        RbtnLegNo.SelectedIndex = 1
                    End If
                    RbtnLegNo.Enabled = False
                    Session("iLeg") = LegNo
                End If
                If Request.QueryString("ref") IsNot Nothing Then
                    Dim req As String = Request.QueryString("ref").Replace(" ", "+")
                    Dim str As String = Crypto.Decrypt(req)
                    Dim rfAr() As String = str.Split("/")
                    If Session("compid") = "1103" Then
                        If rfAr.Length >= 1 Then
                            If rfAr(0) <> "" And rfAr(1) = "1" Then
                                If rfAr.Length > 2 Then
                                    If rfAr(2) = "Join Now" Then
                                        txtRefralId.Text = Get_IDNo(rfAr(0))
                                        'txtUplinerId.Text = Get_IDNo(rfAr(0))
                                        RbtnLegNo.SelectedIndex = 0
                                        RbtnLegNo.Enabled = False
                                        GoTo refLink
                                    Else
                                        txtRefralId.Text = (rfAr(0))
                                        RbtnLegNo.SelectedIndex = 0
                                        RbtnLegNo.Enabled = False
                                        GoTo refLink
                                    End If
                                Else
                                    txtRefralId.Text = (rfAr(0))
                                    RbtnLegNo.SelectedIndex = 0
                                    RbtnLegNo.Enabled = False
                                    GoTo refLink
                                End If
                            ElseIf rfAr(0) <> "" And rfAr(1) = "2" Then
                                If rfAr.Length > 2 Then
                                    If rfAr(2) = "Join Now" Then
                                        txtRefralId.Text = Get_IDNo(rfAr(0))
                                        'txtUplinerId.Text = Get_IDNo(rfAr(0))
                                        RbtnLegNo.SelectedIndex = 1
                                        RbtnLegNo.Enabled = False
                                        GoTo refLink
                                    Else
                                        txtRefralId.Text = (rfAr(0))
                                        RbtnLegNo.SelectedIndex = 1
                                        RbtnLegNo.Enabled = False
                                        GoTo refLink
                                    End If
                                Else
                                    txtRefralId.Text = (rfAr(0))
                                    RbtnLegNo.SelectedIndex = 1
                                    RbtnLegNo.Enabled = False
                                    GoTo refLink
                                End If

                            ElseIf rfAr(0) <> "" And rfAr(1) = "3" Then
                                If rfAr(2) = "Join Now" Then
                                    txtRefralId.Text = Session("idno")
                                    txtUplinerId.Text = (rfAr(0))
                                    RbtnLegNo.SelectedIndex = 0
                                    RbtnLegNo.Enabled = False
                                    GoTo refLink
                                Else
                                    txtRefralId.Text = (rfAr(0))
                                    RbtnLegNo.SelectedIndex = 0
                                    RbtnLegNo.Enabled = False
                                    GoTo refLink
                                End If


                            Else
                                If rfAr(0) <> "" And rfAr(1) = "2" Then
                                    If rfAr(2) = "Join Now" Then
                                        txtRefralId.Text = Session("idno")
                                        txtUplinerId.Text = (rfAr(0))
                                        RbtnLegNo.SelectedIndex = 1
                                        RbtnLegNo.Enabled = False
                                        GoTo refLink
                                    Else
                                        txtRefralId.Text = (rfAr(0))
                                        RbtnLegNo.SelectedIndex = 1
                                        RbtnLegNo.Enabled = False
                                        GoTo refLink
                                    End If

                                End If
                            End If
                        End If
                    Else
                        If rfAr.Length >= 1 Then
                            If rfAr(0) <> "" And rfAr(1) = "1" Then
                                txtRefralId.Text = (rfAr(0))
                                RbtnLegNo.SelectedIndex = 0
                                RbtnLegNo.Enabled = False
                                GoTo refLink
                            ElseIf rfAr(0) <> "" And rfAr(1) = "0" Then
                                txtRefralId.Text = (rfAr(0))
                                RbtnLegNo.SelectedIndex = 0
                                RbtnLegNo.Enabled = False
                                GoTo refLink

                            Else
                                If rfAr(0) <> "" And rfAr(1) = "2" Then
                                    txtRefralId.Text = (rfAr(0))
                                    RbtnLegNo.SelectedIndex = 1
                                    RbtnLegNo.Enabled = False
                                    GoTo refLink
                                End If
                            End If
                        End If
                    End If
                    'If rfAr.Length >= 1 Then

                    '    If rfAr(0) <> "" And rfAr(1) = "1" Then
                    '        txtRefralId.Text = (rfAr(0))
                    '        RbtnLegNo.SelectedIndex = 0
                    '        RbtnLegNo.Enabled = False
                    '        GoTo refLink
                    '    Else
                    '        If rfAr(0) <> "" And rfAr(1) = "2" Then
                    '            txtRefralId.Text = (rfAr(0))
                    '            RbtnLegNo.SelectedIndex = 1
                    '            RbtnLegNo.Enabled = False
                    '            GoTo refLink
                    '        End If
                    '    End If

                    'End If

                    If Len(Request.QueryString("RefFormNo")) > 0 Then
                        txtRefralId.Text = Get_IDNo(Request.QueryString("RefFormNo"))
refLink:                If Trim(txtRefralId.Text) <> "" Then
                            FillReferral()
                        End If
                        txtRefralId.ReadOnly = True
                        'If Session("compid") = "1103" Then
                        '    If Trim(txtUplinerId.Text) <> "" Then
                        '        FillSponsor()
                        '        txtUplinerId.ReadOnly = True
                        '    End If
                        'End If
                    End If
                End If
                If Request.QueryString("PhN1") IsNot Nothing Then
                    Dim req1 As String = Request.QueryString("PhN1").Replace(" ", "+")
                    Dim str1 As String = Crypto.Decrypt(req1)
                    Dim rfAr1() As String = str1.Split("/")
                    If rfAr1.Length >= 1 Then
                        If rfAr1(0) <> "" And rfAr1(1) = "1" Then
                            txtMobileNo.Text = (rfAr1(0))
                            RbtnLegNo.SelectedIndex = 0
                            RbtnLegNo.Enabled = False
                            GoTo refLink1
                        Else
                            If rfAr1(0) <> "" And rfAr1(1) = "2" Then
                                txtMobileNo.Text = (rfAr1(0))
                                RbtnLegNo.SelectedIndex = 1
                                RbtnLegNo.Enabled = False
                                GoTo refLink1
                            End If
                        End If
                    End If
                    If Len(Request.QueryString("PhN1")) > 0 Then
                        txtMobileNo.Text = Get_IDNo(Request.QueryString("PhN1"))
refLink1:               If Trim(txtMobileNo.Text) <> "" Then
                            FillMobileNo()
                        End If
                    End If
                End If
                If Request.QueryString("Email") IsNot Nothing Then
                    Dim req2 As String = Request.QueryString("Email").Replace(" ", "+")
                    Dim str2 As String = Crypto.Decrypt(req2)
                    Dim rfAr2() As String = str2.Split("/")
                    If rfAr2.Length >= 1 Then
                        If rfAr2(0) <> "" And rfAr2(1) = "1" Then
                            txtEMailId.Text = (rfAr2(0))
                            RbtnLegNo.SelectedIndex = 0
                            RbtnLegNo.Enabled = False
                            GoTo refLink3
                        Else
                            If rfAr2(0) <> "" And rfAr2(1) = "2" Then
                                txtEMailId.Text = (rfAr2(0))
                                RbtnLegNo.SelectedIndex = 1
                                RbtnLegNo.Enabled = False
                                GoTo refLink3
                            End If
                        End If
                    End If
                    If Len(Request.QueryString("Email")) > 0 Then
                        txtEMailId.Text = Get_IDNo(Request.QueryString("Email"))
refLink3:               If Trim(txtEMailId.Text) <> "" Then
                            Fillemail()
                        End If
                    End If
                End If
                FillPaymode()
                dbGeneral.Fill_Date_box(ddlDOBdt, ddlDOBmnth, ddlDOBYr, 1940, Now.AddYears(-18).Year)
                dbGeneral.Fill_Date_box(DDlMDay, DDLMMonth, DDLMYear, 1940, Now.Year)
                FillBankMaster()
                FindSession()
                GetConfigDtl()
                vsblCtrl(False, True)
            End If
            Try
                Dim dRead As SqlDataReader
                Dim cmd As SqlCommand
                cmd = New SqlCommand("Select top 1 SessId as SessId from M_MonthSessnMaster order by SessID desc", dbConnect.cnnObject)
                dRead = cmd.ExecuteReader
                If dRead.Read Then
                    Session("Dsessid") = dRead("SessID")
                Else
                    Session("Dsessid") = 0
                End If
                dRead.Close()
                cmd.Cancel()
            Catch
            End Try
            'If Session("compid") = "1103" Then
            '    If Session("IsGetExtreme") = "N" Then
            '        rwSpnsr.Visible = True
            '    Else
            '        rwSpnsr.Visible = True
            '    End If
            'Else

            'End If
            If Session("IsGetExtreme") = "N" Then
                rwSpnsr.Visible = True
            Else
                rwSpnsr.Visible = False
            End If
        Catch ex As Exception

        End Try
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
    Private Sub FillPaymode()
        Try
            strQuery = "SELECT * FROM M_PayModeMaster WHERE ActiveStatus='Y'"
            dbConnect.Fill_Data_Tables(strQuery, tmpTable)
            DdlPaymode.DataSource = tmpTable
            DdlPaymode.DataValueField = "PID"
            DdlPaymode.DataTextField = "Paymode"
            DdlPaymode.DataBind()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub GetConfigDtl()
        Try
            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand
            cmd = New SqlCommand("select * from M_ConfigMaster ", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("IsGetExtreme") = dRead("IsGetExtreme")
                Session("IsTopUp") = dRead("IsTopUp")
                Session("IsSendSMS") = dRead("IsSendSMS")
                Session("IdNoPrefix") = dRead("IdNoPrefix")
                Session("IsFreeJoin") = dRead("IsFreeJoin")
                Session("IsStartJoin") = dRead("IsStartJoin")
                Session("JoinStartFrm") = dRead("JoinStartFrm")
                Session("IsSubPlan") = dRead("IsSubPlan")
            Else
                Session("IsGetExtreme") = "N"
                Session("IsTopUp") = "N"
                Session("IsSendSMS") = "N"
                Session("IdNoPrefix") = ""
                Session("IsFreeJoin") = "N"
                Session("IsStartJoin") = "N"
                Session("JoinStartFrm") = "01-Sep-2011"
                Session("IsSubPlan") = "N"
            End If
            dRead.Close()
        Catch
            Session("CompName") = ""
            Session("CompAdd") = ""
            Session("CompWeb") = ""
        End Try
    End Sub
    Private Function Get_IDNo(ByVal MyFormNo As Integer) As String
        Try
            Dim IdNo As String = ""
            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand
            cmd = New SqlCommand("select IdNo from M_MemberMaster WHERE FormNo='" & MyFormNo & "'", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                IdNo = dRead("IdNo")
            End If
            dRead.Close()
            Return IdNo
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Function
    Protected Sub vsblCtrl(ByVal IsVsbl As Boolean, ByVal IsOnlyDv As Boolean)
        Try
            If (Not IsOnlyDv) Then
                txtUplinerId.Enabled = Not IsVsbl
                txtRefralId.Enabled = Not IsVsbl
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
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
    Public Sub SaveIntoDB()
        Try
            If (Session("ActiveStatusJoin") = "N") Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please contact to admin.!');location.replace('Logout.aspx');", True)
                Exit Sub
            End If
            'If Regex.IsMatch(txtPinCode.Text, "^[1-9][0-9]{5}$") Then
            'Else
            '    scrname = "<SCRIPT language='javascript'>alert('Invaild Pin Code.');" & "</SCRIPT>"
            '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
            '    Exit Sub
            'End If
            Dim IsPanCard As Char
            Dim strQry As String = ""
            Dim strDOB, strDOM, strDOJ, s As String
            Dim iLeg As Integer
            Dim cGender, cMarried As Char
            cGender = "M"
            cMarried = "N"
            Dim HostIp As String = Context.Request.UserHostAddress.ToString
            Dim DistrictCode, CityCode, VillageCode As Integer
            Try
                If Validt_SpnsrDtl("") = "OK" Then
                    If (RbtnLegNo.SelectedIndex = 0) Then
                        iLeg = 1
                    ElseIf (RbtnLegNo.SelectedIndex = 1) Then
                        iLeg = 2
                    Else
                        chkterms.Checked = False
                        CmdSave.Enabled = True
                        scrname = "<SCRIPT language='javascript'>alert('Choose Position.');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Choose Position.');", True)
                        RbtnLegNo.Enabled = True
                        Exit Sub
                    End If
                    If RbtPan.SelectedValue = "Y" Then
                        IsPanCard = "Y"
                        If txtPanNo.Text = "" Then
                            chkterms.Enabled = True
                            CmdSave.Enabled = True
                            scrname = "<SCRIPT language='javascript'>alert('Enter PAN No.');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Enter PAN No.');", True)
                            Exit Sub
                        End If
                    ElseIf RbtPan.SelectedValue = "N" Then
                        IsPanCard = "N"
                    Else
                        chkterms.Enabled = True
                        CmdSave.Enabled = True
                        scrname = "<SCRIPT language='javascript'>alert('Choose PAN No Available Or Not.');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Choose PAN No Available Or Not.');", True)
                        Exit Sub
                    End If
                    TxtPasswd.Text = GenerateRandomString(6)
                    Dim s1 As String = ""
                    If (Session("CompID") = 1057) Then
                        If txtEMailId.Text = "" Then
                            chkterms.Enabled = True
                            CmdSave.Enabled = True
                            lblemail.Visible = True
                            lblemail.Text = "Enter E mail id."
                            Exit Sub
                        Else
                            lblemail.Visible = False
                        End If
                        If txtPanNo.Text = "" Then
                            chkterms.Enabled = True
                            CmdSave.Enabled = True
                            lblpan.Visible = True
                            lblpan.Text = "Enter Pan no."
                            Exit Sub
                        Else
                            lblpan.Visible = False
                        End If
                    End If
                    If (Session("CompID") = 1091) Then
                        If txtEMailId.Text = "" Then
                            chkterms.Enabled = True
                            CmdSave.Enabled = True
                            lblemail.Visible = True
                            lblemail.Text = "Enter E mail id."
                            Exit Sub
                        Else
                            lblemail.Visible = False
                        End If
                    End If
                    If (Session("CompID") = 1057) Then
                        If txtMobileNo.Text <> "" Then
                            s1 = "select Count(mobl) as mobileno from M_Membermaster where Mobl='" & txtMobileNo.Text.Trim & "'"
                            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                            Dim Dt1 As DataTable
                            Dt1 = New DataTable
                            Dt1 = ObjDAL.GetData(s1)
                            If Dt1.Rows(0)("mobileno") >= 1 Then
                                CmdSave.Enabled = True
                                chkterms.Checked = False
                                lblMobileNo.Visible = True
                                lblMobileNo.Text = "Already Registerd by this Mobile Number."
                                Exit Sub
                            Else
                                lblMobileNo.Visible = False
                            End If
                        End If
                        If txtEMailId.Text <> "" Then
                            s1 = "select Count(Email) as Email from M_Membermaster where Email='" & txtEMailId.Text.Trim & "'"
                            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                            Dim Dt1 As DataTable
                            Dt1 = New DataTable
                            Dt1 = ObjDAL.GetData(s1)
                            If Dt1.Rows(0)("Email") >= 1 Then
                                CmdSave.Enabled = True
                                chkterms.Checked = False
                                lblemail.Visible = True
                                lblemail.Text = "Already Registerd by this E mail id."
                                Exit Sub
                            Else
                                lblemail.Visible = True
                            End If
                        End If
                        If txtPanNo.Text <> "" Then
                            s1 = "select Count(Panno) as PanNo from M_Membermaster where Panno='" & txtPanNo.Text.Trim & "'"
                            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                            Dim Dt1 As DataTable
                            Dt1 = New DataTable
                            Dt1 = ObjDAL.GetData(s1)
                            If Dt1.Rows(0)("Panno") >= 3 Then
                                txtPanNo.Text = ""
                                CmdSave.Enabled = True
                                chkterms.Checked = False
                                lblpan.Visible = True
                                lblpan.Text = "Panno already exist in 3 another ids."
                                Exit Sub
                            Else
                                lblpan.Visible = False
                            End If
                        End If
                    End If


                    If (Session("CompID") = 1084) Then
                        If txtPanNo.Text <> "" Then
                            s1 = "select Count(Panno) as PanNo from M_Membermaster where Panno='" & txtPanNo.Text.Trim & "'"
                            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                            Dim Dt1 As DataTable
                            Dt1 = New DataTable
                            Dt1 = ObjDAL.GetData(s1)
                            If Dt1.Rows(0)("Panno") >= 1 Then
                                txtPanNo.Text = ""
                                CmdSave.Enabled = True
                                chkterms.Checked = False
                                lblpan.Visible = True
                                lblpan.Text = "Panno already exist in 1 another ids."
                                Exit Sub
                            Else
                                lblpan.Visible = False
                            End If
                        End If
                    End If
                    If (Session("CompID") = 1091) Then
                        If txtPanNo.Text <> "" Then
                            s1 = "select Count(Panno) as PanNo from M_Membermaster where Panno='" & txtPanNo.Text.Trim & "'"
                            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                            Dim Dt1 As DataTable
                            Dt1 = New DataTable
                            Dt1 = ObjDAL.GetData(s1)
                            If Dt1.Rows(0)("Panno") >= 1 Then
                                txtPanNo.Text = ""
                                CmdSave.Enabled = True
                                chkterms.Checked = False
                                lblpan.Visible = True
                                lblpan.Text = "Panno already exist in 1 another ids."
                                Exit Sub
                            Else
                                lblpan.Visible = False
                            End If
                        End If
                    End If
                    If txtMobileNo.Text = "" Then
                        txtMobileNo.Text = "0"
                    End If
                    Dim q As String = ""
                    Dim i As Integer = 0
                    Dim Dt As DataTable
                    Dim BankCode As Integer = 0
                    If CmbBank.SelectedItem.Text.ToUpper = "OTHERS" Then
                        If TxtBank.Text <> "" Then
                            q = "Select * from M_BankMaster where BankName='" & TxtBank.Text.Trim & "' and Activestatus='Y'and RowStatus='Y' "
                            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                            Dt = New DataTable
                            Dt = ObjDAL.GetData(q)
                            If Dt.Rows.Count = 0 Then
                                q = ""
                                q = "insert into M_BankMaster (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) " & _
    " Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" & TxtBank.Text.ToUpper & "','0','0', " & _
    " '','Y','Add by " & Session("IdNo") & " at " & DateTime.Now.ToString() & "','" & Session("MemName") & "','" & Val(Session("FormNo")) & "','','Y' From M_BankMaster "
                                cmd = New SqlCommand(q, dbConnect.cnnObject)
                                i = cmd.ExecuteNonQuery()
                                If i > 0 Then
                                    q = " select Max(BankCode)as BankCode from M_BankMaster where ActiveStatus='Y' and RowStatus='Y'"
                                    cmd = New SqlCommand(q, dbConnect.cnnObject)
                                    dRead = cmd.ExecuteReader
                                    If dRead.Read Then
                                        dblBank = dRead("BankCode")
                                    End If
                                    dRead.Close()
                                End If
                            Else
                                dblBank = Dt.Rows(0)("BankCode")
                            End If
                        End If
                    Else
                        dblBank = CmbBank.SelectedValue
                    End If
                    Dim AreaCode As Integer = 0
                    AreaCode = 0
                    Dim RegestType As String = ""
                    If RbCategory.SelectedValue = "IN" Then
                        RegestType = "IN"
                    Else
                        RegestType = CbSubCategory.SelectedValue
                    End If
                    Dim PostalAreaCode As Integer = 0
                    strDOB = ddlDOBdt.Text & "-" & ddlDOBmnth.Text & "-" & ddlDOBYr.Text
                    strDOM = DDlMDay.Text & "-" & DDLMMonth.Text & "-" & DDLMYear.Text
                    strDOJ = Format(dbConnect.Get_ServerDate(), "dd-MMM-yyyy")
                    dblDistrict = ClearInject(ddlDistrict.Text.ToUpper)
                    dblTehsil = ClearInject(ddlTehsil.Text.ToUpper)
                    If (dvpin.Visible = True) Then
                        If dblDistrict Is Nothing Then
                            dblDistrict = ""
                        End If
                        If ddlState.SelectedValue = 0 Then
                            dblState = 0
                        Else
                            dblState = ddlState.SelectedValue
                        End If
                        DistrictCode = 0
                        DistrictCode = 0
                        CityCode = 0
                        VillageCode = 0
                    Else
                        dblDistrict = ""
                        dblState = 0
                        DistrictCode = 0
                        DistrictCode = 0
                        CityCode = 0
                        VillageCode = 0
                    End If
                    IfSC = ClearInject(txtIfsCode.Text.ToUpper)
                    dblPlan = 0
                    InVoiceNo = 0
                    If Session("SessID") = 0 Then
                        FindSession()
                    End If
                    Dim Name As String = ""
                    Dim fathername As String = ""
                    If RbCategory.SelectedValue = "IN" Then
                        Name = ClearInject(txtFrstNm.Text.ToUpper)
                        fathername = ClearInject(txtFNm.Text.ToUpper)
                    Else
                        fathername = ClearInject(txtFrstNm.Text.ToUpper)
                        Name = ClearInject(TxtCompanyName.Text.ToUpper)
                    End If
                    If TxtAccountNo.Text <> "" Or Trim(txtIfsCode.Text) <> "" Then
                        If Trim(TxtAccountNo.Text) = "" Then
                            chkterms.Checked = False
                            CmdSave.Enabled = True
                            scrname = "<SCRIPT language='javascript'>alert('Enter Account No.');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Enter Account No.');", True)
                            Exit Sub
                        End If
                        If CmbBank.SelectedValue = 0 Then
                            chkterms.Checked = False
                            CmdSave.Enabled = True
                            scrname = "<SCRIPT language='javascript'>alert('Choose Bank Name');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Choose Bank Name.');", True)
                            Exit Sub
                        End If
                        If TxtBranchName.Text = "" Then
                            chkterms.Checked = False
                            CmdSave.Enabled = True
                            scrname = "<SCRIPT language='javascript'>alert('Enter Branch Name.');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Choose Branch Name.');", True)
                            Exit Sub
                        End If
                        If DDLAccountType.SelectedValue = "" Then
                            chkterms.Checked = False
                            CmdSave.Enabled = True
                            scrname = "<SCRIPT language='javascript'>alert('Enter Account Name.');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Enter Account Name.');", True)
                            Exit Sub
                        End If
                        If txtIfsCode.Text = "" Then
                            chkterms.Checked = False
                            CmdSave.Enabled = True
                            scrname = "<SCRIPT language='javascript'>alert('Enter IFSC Code.');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Enter IFSC Code.');", True)
                            Exit Sub
                        End If
                    End If
                    If (Val(Session("CompID")) = 1007) Then
                        If (Val(Session("Uplnr")) = 0) Then
                            scrname = "<SCRIPT language='javascript'>alert('Invalid Upline Id Please try Again.!!');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Invalid Upline Id Please try Again.!!');", True)
                            Exit Sub
                        End If
                    End If


                    If Session("compid") = "1091" Or Session("CompID") = "1100" Then
                        If txtUName.Text = "" Then
                            CmdSave.Enabled = True
                            chkterms.Checked = False
                            scrname = "<SCRIPT language='javascript'>alert('Please Enter User Name .!!  ');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                            Exit Sub
                        End If


                        If txtUName.Text <> "" Then
                            s1 = "select Count(IDNo) as IDNo from M_Membermaster where IDNo='" & txtUName.Text.Trim() & "'"
                            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                            Dim Dt1 As DataTable
                            Dt1 = New DataTable
                            Dt1 = ObjDAL.GetData(s1)
                            If Dt1.Rows(0)("IDNo") >= 1 Then
                                CmdSave.Enabled = True
                                chkterms.Checked = False
                                scrname = "<SCRIPT language='javascript'>alert('User Name already exists.!!  ');" & "</SCRIPT>"
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                                Exit Sub
                            End If
                        End If

                    End If
                    Dim isOk As Integer = 0
                    If Session("CompId") = "1091" Then
                        Dim Strqueryquer As String = ""
                       Strqueryquer = "Insert into Trnjoining (Transid) values(" & HdnCheckTrnns.Value & ")"
                        Dim isOk1 As Integer = 0
                        isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(dbConnect.cnnObject, CommandType.Text, Strqueryquer))
                        If isOk1 > 0 Then
                            strQry = " insert into m_memberMaster (SessId,IdNo,CardNo,FormNo,KitId,UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo,"
                            strQry &= "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation,"
                            strQry &= "NomineeName,Address1,Address2,Post,Tehsil,City,District,StateCode,CountryId,"
                            strQry &= "PinCode,PhN1,Fax,Mobl,MarrgDate,Passw,Doj,Relation,PanNo,BankID,MICRCode,BranchName,EMail,BV,"
                            strQry &= "UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp,"
                            strQry &= " PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,AadharNo,AadharNo2,AAdharNo3,AreaName,AreaCode,fld5)"
                            strQry &= "Values(" & Val(Session("SessID")) & ",'" & txtUName.Text.Trim() & "',0 ,0," & Val(Session("Kitid")) & "," & Val(Session("Uplnr")) & ""
                            strQry &= ",0," & iLeg & ",0," & Val(Session("Refral")) & ",'" & ClearInject(txtFrstNm.Text.ToUpper) & "',''"
                            strQry &= ",'" & CmbType.SelectedValue & "','" & ClearInject(txtFNm.Text.ToUpper) & "','" & strDOB & "','" & cGender & "','',"
                            strQry &= "'" & ClearInject(txtNominee.Text.ToUpper) & "','" & ClearInject(txtAddLn1.Text.ToUpper) & "','','"
                            strQry &= "" & "','" & dblTehsil & "','" & dblTehsil & "','" & dblDistrict & "'," & dblState & ",1,'" & _
                            txtPinCode.Text & "','" & txtPhNo.Text & "','CHOOSE ACCOUNT TYPE','" & txtMobileNo.Text & "','" & _
                            strDOM & "','" & ClearInject(TxtPasswd.Text) & "',Getdate(),'" & _
                            ClearInject(txtRelation.Text.ToUpper) & "','" & ClearInject(txtPanNo.Text.ToUpper) & "','" & dblBank & "','" & Trim(ClearInject(TxtMICR.Text.ToUpper)) & "','" & Trim(TxtBranchName.Text.ToUpper) & "','" & ClearInject(txtEMailId.Text) & "'," & _
                            Val(Session("Bv")) & ",0,'" & ClearInject(TxtPasswd.Text) & "','" & ClearInject(TxtPasswd.Text) & "','" & Session("JoinStatus") & "','" & InVoiceNo & "','" & Session("RP") & "','" & HostIp & "'," & _
                             Val(DdlPaymode.SelectedValue) & ",'" & Trim(DdlPaymode.SelectedItem.Text.ToUpper) & "','" & ClearInject(TxtDDNo.Text) & "','0','" & ClearInject(TxtIssueBank.Text.ToUpper) & "','" & Trim(TxtDDDate.Text) & "'," & _
                             " '" & ClearInject(TxtIssueBranch.Text) & "','N','" & ClearInject(TxtAAdhar1.Text) & "','" & ClearInject(TxtAadhar2.Text) & "','" & ClearInject(TxtAadhar3.Text) & "','" & TxtVillage.Text & "','" & VillageCode & "','A')"
                            isOk = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strQry))
                            LastInsertID = 0
                            If isOk <> 0 Then
                                Dim DtTab As DataTable = New DataTable()
                                Dim membername As String = ""
                                Dim Email As String = ""
                                Dim Password As String = ""
                                Dim sqlStr As String = ""
                                If Session("CompID") = "1103" Then
                                    sqlStr = "SELECT TOP 1 a.IDNO,a.formno,b.IsBill,a.Passw,a.MemFirstname,a.MemlastName,a.Email FROM m_MemberMaster as a,"
                                    sqlStr &= "m_KitMaster as b where a.kitid=b.kitid ORDER BY mid DESC"
                                Else
                                    sqlStr = "SELECT TOP 1 a.IDNO,a.formno,b.IsBill,a.Passw,a.MemFirstname,a.MemlastName,a.Email FROM m_MemberMaster as a,"
                                    sqlStr &= "m_KitMaster as b where a.kitid=b.kitid AND a.IDno = '" & txtUName.Text & "' ORDER BY mid DESC"
                                End If
                                DtTab = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, sqlStr).Tables(0)
                                If DtTab.Rows.Count > 0 Then
                                    membername = DtTab.Rows(0)("MemfirstName") & " " & DtTab.Rows(0)("MemLastName")
                                    Email = DtTab.Rows(0)("Email")
                                    LastInsertID = DtTab.Rows(0)("IDNO")
                                    Password = DtTab.Rows(0)("Passw")
                                    Session("Kit") = DtTab.Rows(0)("IsBill")
                                Else
                                    LastInsertID = "10001"
                                End If
                                If Session("IsSendSMS") = "Y" Then
                                    sendSMS()
                                End If
                                If Session("CompID") = "1103" Then
                                    If (Email <> "") Then
                                        SendToMemberMail(LastInsertID, Email, membername, Password)
                                    End If
                                Else
                                    If (Email <> "") Then
                                        SendToMemberMailsolfit(LastInsertID, Email, membername, Password)
                                    End If
                                End If


                                CmdSave.Enabled = True
                                Session("LASTID") = LastInsertID
                                Session("Join") = "YES"
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Key", "alert('Cogratulations! Your have successfully registered And Login details has been sent on your registered Email Id.!'); window.parent.location.href='Registration.aspx';", True)
                                Exit Sub
                                ClrCtrl()
                            Else
                                CmdSave.Enabled = True
                                chkterms.Checked = False
                                scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" & "</SCRIPT>"
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Try Again Later.');", True)
                            End If
                        Else
                            scrname = "<SCRIPT language='javascript'>alert('Try Later.!');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Try Again Later.');", True)
                        End If
                    Else
                        strQry = " insert into m_memberMaster (" & _
                            "SessId,IdNo,CardNo,FormNo,KitId," & _
                            "UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo," & _
                            "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation," & _
                            "NomineeName,Address1,Address2,Post," & _
                            "Tehsil,City,District,StateCode,CountryId," & _
                            "PinCode,PhN1,Fax,Mobl,MarrgDate," & _
                            "Passw,Doj,Relation,PanNo," & _
                            "BankID,MICRCode,BranchName,EMail,BV," & _
                            "UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp," & _
                           " PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,AadharNo,AadharNo2,AAdharNo3,AreaName,AreaCode)"
                        strQry = strQry & "Values(" & Val(Session("SessID")) & ",0,0 " & _
                                       ",0," & Val(Session("Kitid")) & "," & Val(Session("Uplnr")) & _
                                       ",0," & iLeg & ",0," & _
                                       Val(Session("Refral")) & ",'" & ClearInject(txtFrstNm.Text.ToUpper) & "',''" & _
                                       ",'" & CmbType.SelectedValue & "','" & ClearInject(txtFNm.Text.ToUpper) & "','" & strDOB & "','" & cGender & "','','" & _
                                       ClearInject(txtNominee.Text.ToUpper) & "','" & ClearInject(txtAddLn1.Text.ToUpper) & "','','" & _
                                       "" & "','" & dblTehsil & "','" & dblTehsil & "','" & _
                                       dblDistrict & "'," & dblState & ",1,'" & _
                                       txtPinCode.Text & "','" & txtPhNo.Text & "','CHOOSE ACCOUNT TYPE','" & txtMobileNo.Text & "','" & _
                                       strDOM & "','" & ClearInject(TxtPasswd.Text) & "',Getdate(),'" & _
                                       ClearInject(txtRelation.Text.ToUpper) & "','" & ClearInject(txtPanNo.Text.ToUpper) & "','" & dblBank & "','" & Trim(ClearInject(TxtMICR.Text.ToUpper)) & "','" & Trim(TxtBranchName.Text.ToUpper) & "','" & ClearInject(txtEMailId.Text) & "'," & _
                                       Val(Session("Bv")) & ",0,'" & ClearInject(TxtPasswd.Text) & "','" & ClearInject(TxtPasswd.Text) & "','" & Session("JoinStatus") & "','" & InVoiceNo & "','" & Session("RP") & "','" & HostIp & "'," & _
                                        Val(DdlPaymode.SelectedValue) & ",'" & Trim(DdlPaymode.SelectedItem.Text.ToUpper) & "','" & ClearInject(TxtDDNo.Text) & "','0','" & ClearInject(TxtIssueBank.Text.ToUpper) & "','" & Trim(TxtDDDate.Text) & "'," & _
                                        " '" & ClearInject(TxtIssueBranch.Text) & "','N','" & ClearInject(TxtAAdhar1.Text) & "','" & ClearInject(TxtAadhar2.Text) & "','" & ClearInject(TxtAadhar3.Text) & "','" & TxtVillage.Text & "','" & VillageCode & "')"
                        cmd = New SqlCommand(strQry, dbConnect.cnnObject)
                        cmd.CommandTimeout = 0
                        isOk = cmd.ExecuteNonQuery()
                        LastInsertID = 0
                        If isOk <> 0 Then
                            Dim membername As String = ""
                            Dim Email As String = ""
                            Dim Password As String = ""
                            cmd = New SqlCommand("SELECT TOP 1 a.IDNO,a.formno,b.IsBill,a.Passw,a.MemFirstname,a.MemlastName,a.Email FROM m_MemberMaster as a,m_KitMaster as b where a.kitid=b.kitid ORDER BY mid DESC", _
                                                 dbConnect.cnnObject)
                            dRead = cmd.ExecuteReader
                            If dRead.Read Then
                                membername = dRead("MemfirstName") & " " & dRead("MemLastName")
                                Email = dRead("Email")
                                LastInsertID = dRead("IDNO")
                                Password = dRead("Passw")
                                Session("Kit") = dRead("IsBill")
                            Else
                                LastInsertID = "10001"
                            End If
                            dRead.Close()
                            If Session("IsSendSMS") = "Y" Then
                                sendSMS()
                            End If
                            CmdSave.Enabled = True

                            If Session("CompId") = "1057" Then
                                If (Email <> "") Then
                                    SendToMemberMailzaradobit(LastInsertID, Email, membername, Password)
                                End If
                            End If
                            If Session("CompId") = "1060" Then
                                If (Email <> "") Then
                                    SendToMemberMail(LastInsertID, Email, membername, Password)
                                End If
                            End If
                            Session("LASTID") = LastInsertID
                            Session("Join") = "YES"
                            Response.Redirect("Welcome.Aspx?IDNo=" & LastInsertID, False)
                            scrname = "<SCRIPT language='javascript'>alert('Cogratulations! Your have successfully registered And Login details has been sent on your mobile and registered Email Id! ');" & "</SCRIPT>"
                            ClrCtrl()
                        Else
                            CmdSave.Enabled = True
                            chkterms.Checked = False
                            scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Try Again Later.');", True)
                        End If
                    End If
                End If
            Catch e As Exception
                CmdSave.Enabled = True
                chkterms.Checked = False
                scrname = "<SCRIPT language='javascript'>alert('" & e.Message & "');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('" & e.Message & "');", True)
                Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
                Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
                ObjDAL.WriteToFile(text & e.Message)
                Response.Write("Try later.")
                Return
            End Try
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Public Function checkpanno(ByVal panno As String) As Boolean
        Dim sql As String = ""
        Dim obj As DAL
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim dt As New DataTable
        sql = "select count(Panno)as Panno from M_Membermaster where Panno='" & panno.Trim & "'"
        dt = New DataTable
        dt = obj.GetData(sql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("panno") > 3 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function
    Public Function SendToMemberMailotp(ByVal otp As String) As Boolean
        Try
            Dim smtpClient = New SmtpClient()
            Dim mail = New MailMessage()
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress("info@solfit.in")
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(txtEMailId.Text)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            mail.Subject = "Thanks For Connecting!!!"
            'StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%""> " & _
            '         "<tr>" & _
            '         "<td>" & _
            '         "Your OTP for Registration is <span style=""font-weight: bold;"">" & otp & "</span> (valid for 5 minutes)." & _
            '         "<br />" & _
            '         "</td>" & _
            '         "</tr>" & _
            '        "</table>"
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%""> " & _
   " <tr>" & _
      "  <td>" & _
      " प्रिय <br /><br />" & _
         "   आपका <strong>Solfit Energy Private Limited</strong> में स्वागत है! आपने हमारे साथ जुड़ने का निर्णय लिया है, इसके लिए हम आपका आभार व्यक्त करते हैं।<br />" & _
         "   कंपनी में शामिल होने से पहले, कृपया हमारी नियम एवं शर्तों (Terms & Conditions) को ध्यानपूर्वक पढ़ें और अपनी सहमति प्रदान करें। आपकी सदस्यता तभी सक्रिय होगी जब आप नीचे दिए गए OTP को दर्ज करेंगे।<br />" & _
         "   <strong>आपके पंजीकरण के लिए आपका OTP </strong> <span style=""font-weight: bold;"">" & otp & "</span> है (5 मिनट के लिए मान्य)।<br />" & _
         "   <hr>" & _
         "   <strong>  नियम एवं शर्तें (Terms & Conditions):</strong><br />" & _
          "  <strong>1️⃣ स्वतंत्र सहभागिता:</strong> मैं, यह स्वीकार करता/करती हूँ कि मैंने <strong>Solfit Energy Private Limited</strong> को पूरी तरह समझकर, बिना किसी दबाव, बिना किसी प्रलोभन या बिना किसी गलत जानकारी के जॉइन किया है।<br />" & _
           " <strong>2️⃣ कोई दावा नहीं:</strong> मैं सहमति देता/देती हूँ कि किसी भी परिस्थिति में मैं कंपनी, इसके निदेशकों, अधिकारियों या किसी अधिकृत प्रतिनिधि के खिलाफ कानूनी दावा या क्षतिपूर्ति की मांग नहीं करूंगा/करूंगी।<br />" & _
          "  <strong>3️⃣ सोशल मीडिया आचार संहिता:</strong> मैं यह स्वीकार करता/करती हूँ कि मैं <strong>Solfit Energy Private Limited</strong> या इसके प्रतिनिधियों के खिलाफ कभी भी सोशल मीडिया (Facebook, WhatsApp, Instagram, YouTube, Twitter आदि) पर नकारात्मक टिप्पणी, पोस्ट, वीडियो या अन्य किसी भी प्रकार की मानहानिकारक सामग्री प्रकाशित नहीं करूंगा/करूंगी।<br />" & _
          "  <strong>4️⃣ कोई निवेश अनिवार्य नहीं:</strong> मैं स्वीकार करता/करती हूँ कि <strong>Solfit Energy Private Limited</strong> में शामिल होने के लिए किसी भी प्रकार का अनिवार्य निवेश नहीं है।<br />" & _
          "  <strong>5️⃣ कमाई और सफलता मेरी मेहनत पर निर्भर है:</strong> मैं यह समझता/समझती हूँ कि <strong>Solfit Energy Private Limited</strong> में मेरी सफलता पूरी तरह मेरी मेहनत, क्षमता और रणनीति पर निर्भर करेगी।<br />" & _
          "  <strong>6️⃣ बिजनेस पॉलिसी में बदलाव का अधिकार:</strong> मैं यह सहमति देता/देती हूँ कि कंपनी समय-समय पर अपने बिजनेस नियमों और शर्तों में बदलाव कर सकती है।<br />" & _
          "  <strong>7️⃣ नियमों का उल्लंघन होने पर कार्रवाई:</strong> यदि मैं कंपनी के नियमों का उल्लंघन करता/करती हूँ, तो <strong>Solfit Energy Private Limited</strong> को मेरी सदस्यता रद्द करने, बोनस रोकने और कानूनी कदम उठाने का पूरा अधिकार होगा।<br />" & _
           " <hr>" & _
          "  <strong>Dear,</strong><br /><br />" & _
           " Welcome to <strong>Solfit Energy Private Limited</strong>! We thank you for deciding to join us.<br />" & _
           " Before joining the company, please read our Terms & Conditions carefully and give your consent. Your membership will be activated only when you enter the OTP given below.<br />" & _
           " <strong>Your OTP for Registration is</strong> <span style=""font-weight: bold;"">" & otp & "</span> (valid for 5 minutes).<br />" & _
         " <hr>" & _
          " <strong>  Terms & Conditions:</strong><br />" & _
           " <strong>1️⃣ Free Participation:</strong> I acknowledge that I have joined <strong>Solfit Energy Private Limited</strong> with full understanding and without any pressure, inducement or false information.<br />" & _
           " <strong>2️⃣ No Claim:</strong> I agree that under no circumstances will I make any legal claim or seek compensation against the company, its directors, officers or any authorized representative.<br />" & _
           " <strong>3️⃣ Social Media Code of Conduct:</strong> I agree that I will never post negative comments, posts, videos or any other defamatory content on social media against <strong>Solfit Energy Private Limited</strong> or its representatives.<br />" & _
           " <strong>4️⃣ No Investment Required:</strong> I accept that there is no mandatory investment of any kind to join <strong>Solfit Energy Private Limited</strong>.<br />" & _
           " <strong>5️⃣ Earnings and success depend on my hard work:</strong> I understand that my success in <strong>Solfit Energy Private Limited</strong> will depend entirely on my hard work, ability and strategy.<br />" & _
           " <strong>6️⃣ Right to Change Business Policy:</strong> I agree that the Company may change its business terms and conditions from time to time.<br />" & _
           " <strong>7️⃣ Action in case of violation of rules:</strong> If I violate the rules of the Company, <strong>Solfit Energy Private Limited</strong> will have full right to cancel my membership, stop the bonus and take legal action." & _
       " </td>" & _
   " </tr>" & _
"</table>"
            MyMessage.Subject = "Thanks For Connecting!!!"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True
            Dim smtp As New Net.Mail.SmtpClient("smtp.gmail.com")
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Credentials = New Net.NetworkCredential("info@solfit.in", "lppsphtxliatuvdq")
            smtp.Send(MyMessage)
            txtRefralId.ReadOnly = True
            txtFrstNm.ReadOnly = True
            CmbType.Enabled = True
            txtFNm.ReadOnly = True
            txtPinCode.ReadOnly = True
            Dim c As Integer = 0
            If RbtnLegNo.SelectedValue <> "0" Then
                RbtnLegNo.Enabled = False
            Else
                RbtnLegNo.Enabled = True
                c = c + 1
            End If
            If CmbType.SelectedItem.Text <> "" Then
                CmbType.Enabled = False
            Else
                CmbType.Enabled = True
                c = c + 1
            End If
            If ddlState.SelectedValue <> "0" Then
                ddlState.Enabled = False
            Else
                ddlState.Enabled = True
                c = c + 1
            End If
            txtUName.ReadOnly = True
            ddlDistrict.ReadOnly = True
            ddlTehsil.ReadOnly = True
            txtMobileNo.ReadOnly = True
            txtEMailId.ReadOnly = True
            txtPanNo.ReadOnly = True
            Return True
        Catch ex As Exception
        End Try
    End Function
    Public Function SendToMemberMail(ByVal IdNo As String, ByVal Email As String, ByVal MemberName As String, ByVal Password As String) As Boolean
        Try
            Dim dt As DataTable = New DataTable()
            Dim sql As String = ""
            Dim userEmail As String = ""
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(Email)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%; color:black;""> " & _
                     "<tr>" & _
                     "<td>" & _
                     "<span style=""font-weight: bold;""><h2>Dear " & MemberName & ",</h2></span>" & _
                     "Congratulations! you have successfully registered with  " & Session("CompName") & " .<br />" & _
                     "Your username and password are given below : <br />" & _
                     "<strong>User ID: " & IdNo & "</strong><br />" & _
                     "<strong>Password: " & Password & "</strong><br />" & _
                     "You may login to the Member Center at: <a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompWeb") & "</a><br />" & _
                     "<span style=""color: #0099FF; font-weight: bold;"">Regards,</span><br />" & _
                     "<a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompName") & "</a><br />" & _
                     "<br />" & _
                     "<br />" & _
                     "</td>" & _
                     "</tr>" & _
                    "</table>"
            MyMessage.Subject = "Welcome and Congratulations!"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True
            Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True
        Catch ex As Exception
        End Try
    End Function
    Public Function SendToMemberMailzaradobit(ByVal IdNo As String, ByVal Email As String, ByVal MemberName As String, ByVal Password As String) As Boolean
        Try
            Dim dt As DataTable = New DataTable()
            Dim sql As String = ""
            Dim userEmail As String = ""
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(Email)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%; color:black;""> " & _
                     "<tr>" & _
                     "<td>" & _
                     "<span style=""font-weight: bold;""><h2>Dear " & MemberName & ",</h2></span>" & _
                     "Congratulations! you have successfully registered with  " & Session("CompName") & " .<br />" & _
                     "Your username and password are given below : <br />" & _
                     "<strong>User ID: " & IdNo & "</strong><br />" & _
                     "<strong>Password: " & Password & "</strong><br />" & _
                     "You may login to the Member Center at: <a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">'https://www.cpanel.zaradobits.in/'</a><br />" & _
                     "<span style=""color: #0099FF; font-weight: bold;"">Regards,</span><br />" & _
                     "<a href=" & Session("CompWeb") & " target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompName") & "</a><br />" & _
                     "<br />" & _
                     "<br />" & _
                     "</td>" & _
                     "</tr>" & _
                    "</table>"
            MyMessage.Subject = "Welcome and Congratulations!"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True
            Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True
        Catch ex As Exception
        End Try
    End Function
    Public Function SendToMemberMailEco(ByVal IdNo As String, ByVal Email As String, ByVal MemberName As String, ByVal Password As String) As Boolean
        Try
            Dim dt As DataTable = New DataTable()
            Dim sql As String = ""
            Dim userEmail As String = ""
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(Email)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%; color:black;""> " & _
                     "<tr>" & _
                     "<td>" & _
                     "<span style=""font-weight: bold;""><h2>Dear " & MemberName & ",</h2></span>" & _
                     "Congratulations! you have successfully registered with  " & Session("CompName") & " .<br />" & _
                     "Your username and password are given below : <br />" & _
                     "<strong>User ID: " & IdNo & "</strong><br />" & _
                     "<strong>Password: " & Password & "</strong><br />" & _
                     "You may login to the Member Center at: <a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">'https://www.cpanel.ecoflowsglobal.com/'</a><br />" & _
                     "<span style=""color: #0099FF; font-weight: bold;"">Regards,</span><br />" & _
                     "<a href=" & Session("CompWeb") & " target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompName") & "</a><br />" & _
                     "<br />" & _
                     "<br />" & _
                     "</td>" & _
                     "</tr>" & _
                    "</table>"
            MyMessage.Subject = "Welcome and Congratulations!"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True
            Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True
        Catch ex As Exception
        End Try
    End Function
    Public Function SendToMemberMailsolfit(ByVal IdNo As String, ByVal Email As String, ByVal MemberName As String, ByVal Password As String) As Boolean
        Try
            Dim dt As DataTable = New DataTable()
            Dim sql As String = ""
            Dim userEmail As String = ""
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(Email)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%; color:black;""> " & _
                     "<tr>" & _
                     "<td>" & _
                     "<span style=""font-weight: bold;""><h2>Dear " & MemberName & ",</h2></span>" & _
                     "Congratulations! you have successfully registered with  " & Session("CompName") & " .<br />" & _
                     "Your username and password are given below : <br />" & _
                     "<strong>User ID: " & IdNo & "</strong><br />" & _
                     "<strong>Password: " & Password & "</strong><br />" & _
                     "You may login to the Member Center at: <a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">'https://www.cpanel.solfit.in/'</a><br />" & _
                     "<span style=""color: #0099FF; font-weight: bold;"">Regards,</span><br />" & _
                     "<a href=" & Session("CompWeb") & " target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompName") & "</a><br />" & _
                     "<br />" & _
                     "<br />" & _
                     "</td>" & _
                     "</tr>" & _
                    "</table>"
            MyMessage.Subject = "Welcome and Congratulations!"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True
            Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True
        Catch ex As Exception
        End Try
    End Function
    Public Function SendToMemberMailtrueway(ByVal IdNo As String, ByVal Email As String, ByVal MemberName As String, ByVal Password As String) As Boolean
        Try
            Dim dt As DataTable = New DataTable()
            Dim sql As String = ""
            Dim userEmail As String = ""
            Dim StrMsg As String = ""
            Dim SendFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(Session("CompMail"))
            Dim SendTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(Email)
            Dim MyMessage As Net.Mail.MailMessage = New Net.Mail.MailMessage(SendFrom, SendTo)
            StrMsg = "<table style=""margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%; color:black;""> " & _
                     "<tr>" & _
                     "<td>" & _
                     "<span style=""font-weight: bold;""><h2>Dear " & MemberName & ",</h2></span>" & _
                     "Congratulations! you have successfully registered with  " & Session("CompName") & " .<br />" & _
                     "Your username and password are given below : <br />" & _
                     "<strong>User ID: " & IdNo & "</strong><br />" & _
                     "<strong>Password: " & Password & "</strong><br />" & _
                     "You may login to the Member Center at: <a href=""" & Session("CompWeb") & """ target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">'https://www.cpanel.trueway.online/'</a><br />" & _
                     "<span style=""color: #0099FF; font-weight: bold;"">Regards,</span><br />" & _
                     "<a href=" & Session("CompWeb") & " target=""_blank"" style=""color:#0000FF; text-decoration:underline;"">" & Session("CompName") & "</a><br />" & _
                     "<br />" & _
                     "<br />" & _
                     "</td>" & _
                     "</tr>" & _
                    "</table>"
            MyMessage.Subject = "Welcome and Congratulations!"
            MyMessage.Body = StrMsg
            MyMessage.IsBodyHtml = True
            Dim smtp As New Net.Mail.SmtpClient(Session("MailHost"))
            smtp.UseDefaultCredentials = False
            smtp.Port = 587
            smtp.EnableSsl = True
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Credentials = New Net.NetworkCredential(Session("CompMail"), Session("MailPass"))
            smtp.Send(MyMessage)
            Return True
        Catch ex As Exception
        End Try
    End Function
    Private Sub ClrCtrl()
        txtAddLn1.Text = ""
        txtEMailId.Text = ""
        txtFNm.Text = ""
        txtFrstNm.Text = ""
        txtMobileNo.Text = ""
        txtNominee.Text = ""
        txtPanNo.Text = ""
        txtPhNo.Text = ""
        txtPinCode.Text = ""
        txtRelation.Text = ""
        txtUplinerId.Text = ""
        lblUplnrNm.Text = ""
        ddlDistrict.Text = ""
        ddlTehsil.Text = ""
        TxtBranchName.Text = ""
        TxtAccountNo.Text = ""
        txtIfsCode.Text = ""
        txtRefralId.Text = ""
        lblRefralNm.Text = ""
        txtUplinerId.Enabled = True
        txtRefralId.Enabled = True
        RbtnLegNo.Enabled = True
    End Sub
    Private Sub FillBankMaster()
        Try
            strQuery = "SELECT BankCode as Bid,BANKNAME as Bank FROM M_BankMaster WHERE ACTIVESTATUS='Y' and Rowstatus='Y' ORDER BY BankName"
            dbConnect.Fill_Data_Tables(strQuery, tmpTable)
            CmbBank.DataSource = tmpTable
            CmbBank.DataValueField = "Bid"
            CmbBank.DataTextField = "Bank"
            CmbBank.DataBind()
            CmbBank.SelectedIndex = 0
            TxtBank.Text = CmbBank.SelectedItem.Text
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Public Function Validt_SpnsrDtl(ByVal chkby As String) As String
        Try
            Validt_SpnsrDtl = ""
            If Session("IsGetExtreme") = "N" Then
                If txtUplinerId.Text = "" Then
                    scrname = "<SCRIPT language='javascript'>alert('Check Placement Id');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                    txtUplinerId.Focus()
                    Exit Function
                End If
            End If
            txtRefralId.Text = Replace(Replace(Replace(Trim(txtRefralId.Text), "'", ""), "=", ""), ";", "")
            txtUplinerId.Text = Replace(Replace(Replace(Trim(txtUplinerId.Text), "'", ""), "=", ""), ";", "")
            If (Trim(txtRefralId.Text) <> "") Then
                Try
                    cmd = New SqlCommand("Select FormNo,MemFirstName + ' ' + MemLastName as MemName,ActiveStatus from M_MemberMaster " & _
                                         " where Idno='" & txtRefralId.Text & "' ", _
                                         dbConnect.cnnObject)
                    dRead = cmd.ExecuteReader
                    If dRead.Read = False Then
                        scrname = "<SCRIPT language='javascript'>alert('Sponsor ID Not Exist.');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                        Validt_SpnsrDtl = ""
                        dRead.Close()
                        vsblCtrl(False, True)
                        Exit Function
                    Else

                        Session("Kitid") = 1

                        Session("Bv") = 0
                        Session("JoinStatus") = "N"
                        Session("RP") = 0
                        Validt_SpnsrDtl = "OK"
                    End If
                    Session("Refral") = dRead("FormNo")
                    lblRefralNm.Text = dRead("MemName")
                    dRead.Close()
                    cmd.Cancel()
                Catch ex As Exception
                    Response.Write("Please check sponsor ID.")
                End Try
            Else
                scrname = "<SCRIPT language='javascript'>alert('Check Sponsor ID.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                txtRefralId.Focus()
                Exit Function
            End If
            If Session("IsGetExtreme") = "N" Then
                If (Trim(txtUplinerId.Text) <> "") Then
                    '          dbConnect.OpenConnection()
                    Try

                        cmd = New SqlCommand("Select FormNo,MemFirstName + ' ' + MemLastName as MemName  from M_MemberMaster " & _
                                             " where Idno='" & txtUplinerId.Text & "'", _
                                             dbConnect.cnnObject)
                        dRead = cmd.ExecuteReader
                        If dRead.Read = False Then
                            Validt_SpnsrDtl = ""
                            scrname = "<SCRIPT language='javascript'>alert('Sponsor ID Not Exist');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                            vsblCtrl(False, True)

                            Exit Function
                        End If
                        Session("Uplnr") = dRead("FormNo")
                        Validt_SpnsrDtl = "OK"
                        lblUplnrNm.Text = dRead("MemName")
                        dRead.Close()
                        cmd.Cancel()

                    Catch ex As Exception
                        Response.Write("Incorrect Place under ID")
                    End Try
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Check Sponsor ID');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                    Validt_SpnsrDtl = ""

                    Exit Function
                End If
            Else
                txtUplinerId.Text = 0
                lblUplnrNm.Text = ""
                Session("Uplnr") = 0
            End If

            If Session("IsGetExtreme") = "N" Then
                If Val(Session("Refral")) <> Val(Session("Uplnr")) Then
                    ''Checking If Entered Upliner ID Exists In Sponsor Downline Or Not
                    ' dbConnect.OpenConnection()
                    cmd = New SqlCommand("Select * from M_MemTreeRelation " & _
                                         " where FormNo=" & Session("Refral") & " And FormNoDwn=" & Session("Uplnr") & " ", _
                                         dbConnect.cnnObject)
                    dRead = cmd.ExecuteReader
                    If dRead.Read = False Then
                        Validt_SpnsrDtl = ""
                        scrname = "<SCRIPT language='javascript'>alert('Place Under Does Not Exist In Sponsor Downline!!');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                        dRead.Close()
                        vsblCtrl(False, True)
                        Exit Function
                    End If
                    dRead.Close()
                    cmd.Cancel()
                End If
            End If

            If Session("IsGetExtreme") = "N" Then
                If Not checkAvailLeg() Then
                    Validt_SpnsrDtl = ""
                    vsblCtrl(False, True)
                    Exit Function
                End If
            End If

            RbtnLegNo.Enabled = False
            txtUplinerId.Enabled = False
            txtRefralId.Enabled = False

            ' txtPIN.Enabled = False
            ' txtScratch.Enabled = False
            'cmdNext.Visible = False
            Exit Function
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Function
    Protected Sub CmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSave.Click
        Try
            If chkterms.Checked = False Then
                scrname = "<SCRIPT language='javascript'>alert('Please select Terms and Condtions');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                Exit Sub
            Else
                SaveIntoDB()
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub sendSMS(ByVal Username As String, ByVal MobileNo As String, ByVal Otp As Integer)

        If Len(MobileNo) >= 10 And IsNumeric(MobileNo) = True Then
            Dim client As New WebClient
            Dim baseurl As String
            Dim data As Stream
            Dim time As DateTime = DateTime.Now
            Dim format As String = "dd-MMM-yyyy HH:mm:ss "

            Dim sms As String = ""
            If (Session("CompID") = "1091") Then
                sms = "Your OTP is " & Otp & " for Registration/Product Purchase for IDno " & Username & ". Regard: Solfit Team"
                'Your OTP is {#var1#} for Registration/Product Purchase for IDno {#var2#}. Regard: Solfit Team
            Else
                sms = "" & Otp & " is OTP to verify your registration in KNR"
            End If
            Try

                'baseurl = " http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & txtMobileNo.Text & "&SenderId=" & Session("ClientId") & ""
                baseurl = " http://64.227.180.129/ApiSmsHttp?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & txtMobileNo.Text & "&SenderId=" & Session("ClientId") & "&ServiceName=SMSTRANS&MessageType=1"
                data = client.OpenRead(baseurl)
                Dim reader As New StreamReader(data)
                Dim s As String
                s = reader.ReadToEnd()
                data.Close()
                reader.Close()
            Catch ex As Exception
                ' MsgBox(ex.Message)
            End Try
        End If


    End Sub
    Private Sub FindSession()
        Try
            cmd = New SqlCommand("Select top 1 SessId as SessId from M_SessnMaster order by SessID desc", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read = True Then
                Session("SessID") = dRead("SessID")
            Else
                errMsg.Text = "Session Not Exist. Please Enter New Session."
                Exit Sub
            End If
            dRead.Close()
            cmd.Cancel()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Private Function checkAvailLeg() As Boolean
        Try


            Dim iLegNo, iformNo As Integer
            If RbtnLegNo.SelectedIndex = 0 Then
                iLegNo = 1
            ElseIf RbtnLegNo.SelectedIndex = 1 Then
                iLegNo = 2
            Else
                checkAvailLeg = False
                scrname = "<SCRIPT language='javascript'>alert('Choose Position.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                Exit Function
            End If

            '       dbConnect.OpenConnection()
            ''Get Upliner Info
            cmd = New SqlCommand("Select * from M_MemberMaster where IdNo='" & _
                                 txtUplinerId.Text & "' ", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read = True Then
                iformNo = dRead("FormNo")
            Else
                errMsg.Text = "Check Placeunder Id."
                scrname = "<SCRIPT language='javascript'>alert('" & errMsg.Text & "');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                dRead.Close()
                Exit Function
            End If
            dRead.Close()

            cmd = New SqlCommand("SELECT COUNT(*) AS CNT FROM M_MemberMaster " & _
                                 " WHERE UpLnFormNo=" & iformNo & _
                                 " And  LegNo=" & iLegNo, dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read = True Then
                If dRead("CNT") > 0 Then
                    errMsg.Text = IIf(iLegNo = 1, "LEFT", "RIGHT") & _
                                " Position already used, please select correct Position!"
                    scrname = "<SCRIPT language='javascript'>alert('" & errMsg.Text & "');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                    checkAvailLeg = False
                    Exit Function
                Else
                    _dblAvailLeg = iformNo
                    checkAvailLeg = True
                End If
            Else
                errMsg.Text = "Error In Position Selection."
                scrname = "<SCRIPT language='javascript'>alert('" & errMsg.Text & "');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                dRead.Close()
                Exit Function
            End If
            dRead.Close()

            Return checkAvailLeg
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Function
    Private Sub sendSMS()
        Try


            'dbConnect.OpenConnection()
            Dim MemberPass As String = ""
            Dim MemberTransPassw As String = ""
            cmd = New SqlCommand("Select *,DATEADD(day, 15, Doj) as Maxdate from m_MemberMaster where IDNo = '" & LastInsertID & "'", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read = True Then
                Session("SMSIDNo") = dRead("IDNo")
                Session("SMSIDPass") = dRead("Passw")
                Session("Name") = dRead("MemFirstName")
                Session("MaxDate") = dRead("MaxDate")
                Session("SMSTransPassw") = dRead("EPassw")
            End If
            dRead.Close()
            MemberPass = Session("SMSIDPass")
            MemberTransPassw = Session("SMSTransPassw")
            MemberPass = MemberPass.Replace("%", "%25")
            MemberPass = MemberPass.Replace("&", "%26")
            MemberPass = MemberPass.Replace("#", "%23")
            MemberPass = MemberPass.Replace("'", "%22")
            MemberPass = MemberPass.Replace(",", "%2C")
            MemberPass = MemberPass.Replace("(", "%28")
            MemberPass = MemberPass.Replace(")", "%29")
            MemberPass = MemberPass.Replace("*", "%2A")
            MemberPass = MemberPass.Replace("!", "%21")
            MemberPass = MemberPass.Replace("/", "%2F")
            MemberPass = MemberPass.Replace("@", "%40")
            MemberTransPassw = MemberTransPassw.Replace("%", "%25")
            MemberTransPassw = MemberTransPassw.Replace("&", "%26")
            MemberTransPassw = MemberTransPassw.Replace("#", "%23")
            MemberTransPassw = MemberTransPassw.Replace("'", "%22")
            MemberTransPassw = MemberTransPassw.Replace(",", "%2C")
            MemberTransPassw = MemberTransPassw.Replace("(", "%28")
            MemberTransPassw = MemberTransPassw.Replace(")", "%29")
            MemberTransPassw = MemberTransPassw.Replace("*", "%2A")
            MemberTransPassw = MemberTransPassw.Replace("!", "%21")
            MemberTransPassw = MemberTransPassw.Replace("/", "%2F")
            MemberTransPassw = MemberTransPassw.Replace("@", "%40")
            If Len(txtMobileNo.Text) >= 10 And IsNumeric(txtMobileNo.Text) = True Then
                Dim client As New WebClient
                Dim baseurl As String
                Dim data As Stream
                'Dim Sms As String = " Welcome to " & Session("CompName") & ". Your login details are ID: " & Session("SMSIDNo") & "/Login Pwd:" & MemberPass & "/Trans Pwd:" & MemberTransPassw & " Visit " & Session("CompWeb1") & " for more details."
                '   Welcome To *, Thank You For Registration.Your ID Is * and Password is *. Visit .* Best of luck.
                Dim sms As String

                sms = "Welcome To " & Session("CompName") & ", Thank You For Registration.Your ID Is " & Session("SMSIDNo") & " and Password is " & Session("SMSIDPass") & ". Visit " & Session("CompWeb1") & " Best of luck. Regard : UCM"

                Try

                    ''baseurl = "http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & txtMobileNo.Text & "&SenderId=" & Session("ClientId") & ""

                    baseurl = Session("JoiningSms").ToString().Replace("{#var1#}", Session("SMSIDNo")).Replace("{#var2#}", Session("SMSIDPass")).Replace("{#var3#}", Session("SMSTransPassw")).Replace("{#var4#}", Session("CompWeb1")).Replace("$MOB$", txtMobileNo.Text)




                    ' baseurl = "http://103.233.79.246//submitsms.jsp?user=" & Session("SmsId") & "&key=5d0f10e5faXX&mobile=" & txtMobileNo.Text.Trim & "&message=" & sms & "&senderid=" & Session("ClientId") & "&accusage=1"
                    data = client.OpenRead(baseurl)
                    Dim reader As New StreamReader(data)
                    Dim s As String
                    s = reader.ReadToEnd()
                    data.Close()
                    reader.Close()
                Catch ex As Exception
                    'MsgBox(ex.Message)
                End Try
                'Dim sms1 As String = "Dear " & Session("Name") & " (" & Session("SMSIDNo") & "), Kindly submit scan copy of your Joining form, KYC, PAN copy and cancelled cheque on kyc@dreamtouchindia.com till " & Session("MaxDate") & ""
                'Try
                '    baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & txtMobileNo.Text & "&msg=" & sms1 & ""
                '    data = client.OpenRead(baseurl)
                '    Dim reader As New StreamReader(data)
                '    Dim s As String
                '    s = reader.ReadToEnd()
                '    data.Close()
                '    reader.Close()
                'Catch ex As Exception
                '    MsgBox(ex.Message)
                'End Try
            End If
            ClrCtrl()
            dRead.Close()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try

    End Sub
    Private Sub MaxInvoiceNo()
        Try
            strQuery = " SELECT Max(BillNo)+1 as BillNo FROM M_MemberMAster"
            dbConnect.Fill_Data_Tables(strQuery, tmpTable)
            Dim DR As DataRow
            DR = tmpTable.Rows(0)
            If tmpTable.Rows.Count > 0 Then
                InVoiceNo = DR("BillNo")
            End If
            tmpTable.Clear()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
            MsgBox(ex.Message)
            Exit Sub
        End Try
    End Sub
    Private Sub FindPV()
        Try
            ' dbConnect.OpenConnection()
            cmd = New SqlCommand("Select top 1 PV from M_KitMaster where Kitid = '" & Session("KitId") & "'", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read = True Then
                Session("BV") = dRead("PV")
            Else
                Exit Sub
            End If
            dRead.Close()
            cmd.Cancel()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Protected Sub txtUplinerId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUplinerId.TextChanged
        FillSponsor()
    End Sub
    Private Sub FillSponsor()
        Try


            errMsg.Text = ""
            lblErrEpin.Text = ""
            Dim i As Integer = 0
            txtUplinerId.Text = Replace(Replace(Replace(Trim(txtUplinerId.Text), ";", ""), "'", ""), "=", "")
            strQuery = "Select FormNo,MemFirstName + ' ' + MemLastName as MemName  from M_MemberMaster where IDNo='" & txtUplinerId.Text & "'"
            cmd = New SqlCommand(strQuery, dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read = True Then
                lblUplnrNm.Text = dRead("MemName")
                Session("Uplnr") = dRead("FormNo")
                i += 1
            Else
                errMsg.Text = "Invalid PlaceUnder ID!!"
                lblErrEpin.Text = "Invalid PlaceUnder ID!!"
                scrname = "<SCRIPT language='javascript'>alert('" & errMsg.Text & "');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)

            End If
            dRead.Close()
            cmd.Cancel()
            If Session("compid") = "1103" Then
            Else
                If i = 1 Then
                    checkAvailLeg()
                End If
            End If

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub FillReferral()
        Try
            lblErrEpin.Text = ""
            errMsg.Text = ""
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            txtRefralId.Text = Replace(Replace(Replace(Trim(txtRefralId.Text), ";", ""), "'", ""), "=", "")
            strQuery = ""
            strQuery = "Select FormNo,MemFirstName + ' ' + MemLastName as MemName  from M_MemberMaster where IDNo='" & txtRefralId.Text & "' and IsBlock='N' "
            ' dbConnect.OpenConnection()
            cmd = New SqlCommand(strQuery, dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read = True Then
                lblRefralNm.Text = dRead("MemName")

            Else
                scrname = "<SCRIPT language='javascript'>alert('Invalid Sponsor Id .');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)

                txtRefralId.Text = ""
                dRead.Close()
                Exit Sub


            End If
            dRead.Close()
            cmd.Cancel()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Protected Sub CmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCancel.Click
        ClrCtrl()
    End Sub
    Protected Sub txtRefralId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRefralId.TextChanged
        FillReferral()
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            dbConnect.closeConnection()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)


        End Try
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            dbConnect.closeConnection()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

        End Try
    End Sub
    Protected Sub TxtPostPincode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtPostPincode.TextChanged
        Dim sql As String = ""
        If Val(TxtPostPincode.Text) <> 0 Then
            sql = "select a.Statename,b.DistrictName,c.CityName,d.VillageName,d.Pincode,a.StateCode,b.DistrictCode" & _
                  " ,c.CityCode,d.VillageCode from M_STateDivMaster as a with( NoLock) Inner Join M_DistrictMaster as b " & _
                  " with( NoLock) On a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' " & _
                  " Inner Join  M_CityStatemaster as c with( NoLock)  On b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' " & _
                  " Inner Join M_VillageMaster as d with( NoLock) On c.CityCode=d.CityCode and d.ActiveStatus='Y'" & _
                  " where  d.Pincode='" & Val(TxtPostPincode.Text) & "'" & _
            " Union all " & _
            "select '' as stateName,'' as Districtname,'' as CityName,'Others','',0,0,0,381264"
            Dim Dt As DataTable
            Dt = New DataTable
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dt = ObjDAL.GetData(sql)
            If Dt.Rows.Count > 0 Then
                TxtpostState.Text = Dt.Rows(0)("StateName")
                HPostStateCode.Value = Dt.Rows(0)("StateCode")
                TxtPostDistrict.Text = Dt.Rows(0)("DistrictName")
                HPostDistrict.Value = Dt.Rows(0)("DistrictCode")
                TxtPostCity.Text = Dt.Rows(0)("CityName")
                HPostCity.Value = Dt.Rows(0)("CityCode")
                With DDlPostVillage
                    .DataSource = Dt
                    .DataValueField = "VillageCode"
                    .DataTextField = "VillageName"
                    .DataBind()
                    .SelectedIndex = 0
                End With
                DDlPostVillage.Focus()
            Else
                TxtpostState.Text = ""
                HPostStateCode.Value = 0
                TxtPostCity.Text = ""
                HPostCity.Value = 0
                TxtPostDistrict.Text = ""
                HPostDistrict.Value = 0
                DDlPostVillage.Items.Clear()
                TxtPostPincode.Focus()
                TxtPostPincode.Text = ""
                scrname = "<SCRIPT language='javascript'>alert('Post Pincode Not exist.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Post Pincode Not exist.');", True)

            End If
        End If
    End Sub
    Private Sub Fill_State()
        Try
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim dtMaster As DataTable = New DataTable()
            Dim str As String = "Select StateCode,StateName from M_StateDivMaster Where ActiveStatus = 'Y' And RowStatus =  'Y' Order by StateName"
            dtMaster = ObjDAL.GetData(str)
            If (dtMaster.Rows.Count > 0) Then
                ddlState.DataSource = dtMaster
                ddlState.DataValueField = "StateCode"
                ddlState.DataTextField = "StateName"
                ddlState.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillMobileNo()
        Try
            lblErrEpin.Text = ""
            errMsg.Text = ""
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            txtMobileNo.Text = Replace(Replace(Replace(Trim(txtMobileNo.Text), ";", ""), "'", ""), "=", "")
            strQuery = ""
            'strQuery = "Select FormNo,PhN1 as MobileNo,ActiveStatus  from M_MemberMaster where mobl='" & txtMobileNo.Text & "' and IsBlock='N' "
            strQuery = "select Count(phn1) as phn1 from M_Membermaster where phn1='" & txtMobileNo.Text.Trim & "'"
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Dt1 As DataTable
            Dt1 = New DataTable
            Dt1 = ObjDAL.GetData(strQuery)
            If Dt1.Rows(0)("phn1") >= 7 Then
                CmdSave.Enabled = True
                chkterms.Checked = False
                lblMobileNo.Text = "Already Registerd by this Mobile."
                txtMobileNo.Text = ""
                'scrname = "<SCRIPT language='javascript'>alert('Panno already exist in 3 another ids.');" & "</SCRIPT>"
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                Exit Sub
            Else
                lblMobileNo.Text = ""
                dRead.Close()
                Exit Sub
            End If
            ' dbConnect.OpenConnection()

            'cmd = New SqlCommand(strQuery, dbConnect.cnnObject)
            'dRead = cmd.ExecuteReader
            'If dRead.Read = True Then
            '    If (dRead("ActiveStatus") = "N") Then
            '        txtMobileNolbl.Text = "Already Registerd by this Mobile No."
            '        'scrname = "<SCRIPT language='javascript'>alert('Already Registerd by this Mobile No.');" & "</SCRIPT>"
            '        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
            '        txtMobileNo.Text = ""
            '        'txtMobileNolbl.Text = ""
            '        'CmdSave.Visible = False
            '    Else
            '        'scrname = "<SCRIPT language='javascript'>alert('Please Valid Mobile No.');" & "</SCRIPT>"
            '        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
            '        'txtMobileNo.Text = ""
            '        dRead.Close()
            '        Exit Sub
            '    End If

            'Else
            '    txtMobileNolbl.Text = ""
            '    'CmdSave.Visible = True
            '    'scrname = "<SCRIPT language='javascript'>alert('Invalid Mobile No.');" & "</SCRIPT>"
            '    'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)

            '    'txtMobileNo.Text = ""
            '    dRead.Close()
            '    Exit Sub


            'End If
            dRead.Close()
            cmd.Cancel()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub FillMobileNozara()
        'Try
        lblErrEpin.Text = ""
        errMsg.Text = ""
        ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        txtMobileNo.Text = Replace(Replace(Replace(Trim(txtMobileNo.Text), ";", ""), "'", ""), "=", "")
        strQuery = ""
        'strQuery = "Select FormNo,PhN1 as MobileNo,ActiveStatus  from M_MemberMaster where mobl='" & txtMobileNo.Text & "' and IsBlock='N' "
        strQuery = "select Count(mobl) as mobl from M_Membermaster where mobl='" & txtMobileNo.Text.Trim & "'"
        ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim Dt1 As DataTable
        Dt1 = New DataTable
        Dt1 = ObjDAL.GetData(strQuery)
        If Dt1.Rows(0)("mobl") >= 1 Then
            CmdSave.Enabled = True
            chkterms.Checked = False
            lblMobileNo.Visible = True
            lblMobileNo.Text = "Already Registerd by this Mobile."
            txtMobileNo.Text = ""
            'scrname = "<SCRIPT language='javascript'>alert('Panno already exist in 3 another ids.');" & "</SCRIPT>"
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
            Exit Sub
        Else
            lblMobileNo.Text = ""
            lblMobileNo.Visible = False
            Exit Sub
        End If

        'dRead.Close()
        'cmd.Cancel()
        'Catch ex As Exception
        '    Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
        '    Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
        '    
        '    Response.Write("Try later.")
        'End Try
    End Sub
    Private Sub Fillemail()
        Try
            lblErrEpin.Text = ""
            errMsg.Text = ""
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            txtEMailId.Text = Replace(Replace(Replace(Trim(txtEMailId.Text), ";", ""), "'", ""), "=", "")
            strQuery = ""
            'strQuery = "Select FormNo,Email,ActiveStatus  from M_MemberMaster where email='" & txtEMailId.Text & "' and IsBlock='N' "

            'If txtPanNo.Text <> "" Then
            strQuery = "select Count(email) as email from M_Membermaster where email='" & txtEMailId.Text.Trim & "'"
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Dt1 As DataTable
            Dt1 = New DataTable
            Dt1 = ObjDAL.GetData(strQuery)
            If Dt1.Rows(0)("email") >= 7 Then
                CmdSave.Enabled = True
                chkterms.Checked = False
                lblemail.Visible = True
                lblemail.Text = "Already Registerd by this email id."
                txtEMailId.Text = ""
                'scrname = "<SCRIPT language='javascript'>alert('Panno already exist in 3 another ids.');" & "</SCRIPT>"
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                Exit Sub
            Else
                lblemail.Text = ""
                lblemail.Visible = False
                dRead.Close()
                Exit Sub
            End If

            dRead.Close()
            cmd.Cancel()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub Fillemailzara()
        'Try
        lblErrEpin.Text = ""
        errMsg.Text = ""
        ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        txtEMailId.Text = Replace(Replace(Replace(Trim(txtEMailId.Text), ";", ""), "'", ""), "=", "")
        strQuery = ""
        'strQuery = "Select FormNo,Email,ActiveStatus  from M_MemberMaster where email='" & txtEMailId.Text & "' and IsBlock='N' "

        'If txtPanNo.Text <> "" Then
        strQuery = "select Count(email) as email from M_Membermaster where email='" & txtEMailId.Text.Trim & "'"
        ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim Dt1 As DataTable
        Dt1 = New DataTable
        Dt1 = ObjDAL.GetData(strQuery)
        If Dt1.Rows(0)("email") >= 1 Then
            CmdSave.Enabled = True
            chkterms.Checked = False
            lblemail.Visible = True
            lblemail.Text = "Already Registerd by this email id."
            txtEMailId.Text = ""
            'scrname = "<SCRIPT language='javascript'>alert('Panno already exist in 3 another ids.');" & "</SCRIPT>"
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
            Exit Sub
        Else
            lblemail.Text = ""
            lblemail.Visible = False
            Exit Sub
        End If

        'dRead.Close()
        'cmd.Cancel()
        'Catch ex As Exception
        '    Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
        '    Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
        '    
        '    Response.Write("Try later.")
        'End Try
    End Sub
    Private Sub Fillpannozara()
        'Try
        lblErrEpin.Text = ""
        errMsg.Text = ""
        ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        txtPanNo.Text = Replace(Replace(Replace(Trim(txtPanNo.Text), ";", ""), "'", ""), "=", "")
        strQuery = ""
        'strQuery = "Select FormNo,Email,ActiveStatus  from M_MemberMaster where email='" & txtEMailId.Text & "' and IsBlock='N' "

        'If txtPanNo.Text <> "" Then
        strQuery = "select Count(panno) as panno from M_Membermaster where panno='" & txtPanNo.Text.Trim & "'"
        ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim Dt1 As DataTable
        Dt1 = New DataTable
        Dt1 = ObjDAL.GetData(strQuery)
        If Dt1.Rows(0)("panno") >= 3 Then
            CmdSave.Enabled = True
            chkterms.Checked = False
            lblpan.Text = "Panno already exist in 3 another ids."
            lblpan.Visible = True
            txtPanNo.Text = ""
            'scrname = "<SCRIPT language='javascript'>alert('Panno already exist in 3 another ids.');" & "</SCRIPT>"
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
            Exit Sub
        Else
            lblpan.Text = ""
            lblpan.Visible = False
            dRead.Close()
            Exit Sub
        End If

        'dRead.Close()
        'cmd.Cancel()
        'Catch ex As Exception
        '    Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
        '    Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
        '    
        '    Response.Write("Try later.")
        'End Try
    End Sub
    Protected Sub txtUName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUName.TextChanged
        FillIDno()
    End Sub
    Private Sub FillIDno()
        Try
            lblErrEpin.Text = ""
            errMsg.Text = ""
            strQuery = "select Count(IDNo) as IDNo from M_Membermaster where IDNo='" & txtUName.Text.Trim() & "'"
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Dt1 As DataTable
            Dt1 = New DataTable
            Dt1 = ObjDAL.GetData(strQuery)
            If Dt1.Rows(0)("IDNo") >= 1 Then
                txtUName.Text = ""
                scrname = "<SCRIPT language='javascript'>alert('User Name already exists.!!  ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                Exit Sub
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ObjDAL.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub

    Protected Sub btngenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btngenerate.Click
        If chkterms.Checked = False Then
            scrname = "<SCRIPT language='javascript'>alert('Please select Terms and Condtions');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Exit Sub
        Else

        End If
        Dim OTP_ As Integer = 0
        Dim Rs As New Random
        OTP_ = Rs.Next(100001, 999999)

        Dim OTPP_ As Integer = 0
        Dim Rs1 As New Random
        OTPP_ = Rs1.Next(200001, 999999)
        Dim Emailid As String = txtEMailId.Text
        Dim membername As String = txtFrstNm.Text
        Dim mobileno As String = txtMobileNo.Text

        ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim i As Integer = 0
        Dim R As String = ""
        R = "INSERT into AdminLogin(UserID,Username,Passw,MobileNo,Otp,Emailotp) VALUES ('0','" & txtFrstNm.Text & "','" & TxtPassword.Text & "','" & txtMobileNo.Text & "','" & OTP_ & "','" & OTPP_ & "')"

        i = ObjDAL.SaveData(R)
        If i > 0 Then
            divOtp.Visible = True
            SendToMemberMailotp(OTPP_)
            sendSMS(membername, mobileno, OTPP_)
            'DivTerms.Visible = False
        Else
            ''myModal.Visible = False
            scrname = "<SCRIPT language='javascript'>alert('Try Again.');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)

            Exit Sub
        End If
    End Sub

    Protected Sub BtnPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPassword.Click
        Try
            If chkterms.Checked = False Then
                scrname = "<SCRIPT language='javascript'>alert('Please select Terms and Condtions');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                Exit Sub
                'Else
                '    SaveIntoDB()
            End If
            Dim Dt As New DataTable
            Dim strr As String = "Select TOP 1 * from AdminLogin as a where " & _
          " UserName='" & txtFrstNm.Text & "' and Mobileno='" & Val(txtMobileNo.Text) & "'" & _
          "and EmailOTP='" & Val(TxtPassword.Text) & "' ORDER BY AID DESC "
            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dt = ObjDAL.GetData(strr)
            If Dt.Rows.Count > 0 Then
            Else
                scrname = "<script language='javascript'>alert('Invalid Mobile OTP.');</script>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Invalid Mobile OTP.');", True)
                Exit Sub
            End If







            Dim TransPassw As String = TxtPassword.Text
            TransPassw = TransPassw.Trim
            Dim Dt1 As DataTable

            Dt1 = New DataTable


            ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Session("OtpCount") = Val(Session("OtpCount")) + 1
            Dim str As String = "Select TOP 1 * from AdminLogin as a where " & _
            " UserName='" & txtFrstNm.Text & "' and Mobileno='" & Val(txtMobileNo.Text) & "'" & _
            "and EmailOTP='" & Val(TxtPassword.Text) & "'  ORDER BY AID DESC "


            Dt1 = ObjDAL.GetData(str)
            If Dt1.Rows.Count > 0 Then
                SaveIntoDB()
            Else


                TxtPassword.Text = ""
                If Session("OtpCount") >= 3 Then
                    Session("OtpCount") = 0
                    scrname = "<script language='javascript'>alert('You have tried 3 times with invalid OTP.\n Please generate OTP again.');</script>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('You have tried 3 times with invalid OTP.\n Please generate OTP again.');", True)

                Else
                    scrname = "<script language='javascript'>alert('Invalid OTP.');</script>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "alert", "alert('Invalid OTP.');", True)
                    'lblOTPMsg.Text = "Invalid OTP."
                    ''myModal.Visible = True
                End If

            End If

        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)

            Response.Write("Try later.")
        End Try
    End Sub
End Class

