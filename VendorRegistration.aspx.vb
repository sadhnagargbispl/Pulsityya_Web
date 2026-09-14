
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class VendorRegistration
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
    Dim CatIDQS As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.BtnUpgrade.Attributes.Add("onclick", DisableTheButton(Me.Page, Me.BtnUpgrade))
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
                    FillCategory()
                    FillSubCategory()
                    FillState()
                    If String.IsNullOrEmpty(Request("CatID")) = False Then
                        CatIDQS = Request("CatID")
                    End If
                    If String.IsNullOrEmpty(Request("CatID")) = False Then
                        BtnUpgrade.Text = "Approve"
                        BindData()
                    End If
                End If
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub BindData()
        Dim sql As String = "select * from V#VendorReport where id = '" & CatIDQS & "'"
        dt = New DataTable
        dt = objDAL.GetData(sql)
        If dt.Rows.Count > 0 Then
            TxtIDNo.Text = dt.Rows(0)("idno")
            TxtIDNo.Enabled = False
            LblMemName.Text = dt.Rows(0)("memfirstname")
            LblMemName.Enabled = False
            LblMobileNo.Text = dt.Rows(0)("mobileno")
            LblMobileNo.Enabled = False
            DDlCategory.SelectedValue = dt.Rows(0)("catid")
            'DDlCategory.Enabled = False
            DDlSubCaegory.SelectedValue = dt.Rows(0)("subcatid")
            'DDlSubCaegory.Enabled = False
            TxtshopName.Text = dt.Rows(0)("shopname")
            'TxtshopName.Enabled = False
            TxtCity.Text = dt.Rows(0)("city")
            'TxtCity.Enabled = False
            DDlState.SelectedValue = dt.Rows(0)("statecode")
            'DDlState.Enabled = False
            TxtPicCode.Text = dt.Rows(0)("pincode")
            'TxtPicCode.Enabled = False
            txtPARTNER.Text = dt.Rows(0)("Rankid1")
            'txtPARTNER.Enabled = False
            txtMASTER.Text = dt.Rows(0)("Rankid2")
            'txtMASTER.Enabled = False
            txtAGENCY.Text = dt.Rows(0)("Rankid3")
            'txtAGENCY.Enabled = False
            txtAGENT.Text = dt.Rows(0)("Rankid4")
            'txtAGENT.Enabled = False
            'txtEMALL.Text = dt.Rows(0)("Rankid5")
            'txtEMALL.Enabled = False
            TxtCashback.Text = dt.Rows(0)("Cashback")
            TXtBonus.Text = dt.Rows(0)("Commission")
            RbtDelivery.SelectedValue = dt.Rows(0)("n")
            txtsponsorid.Text = dt.Rows(0)("sponsorid")
            lblsponsorname.Text = dt.Rows(0)("Sponsorname")
            lblsponsorname.Visible = True
            'TXtBonus.Enabled = False
            'TxtCashback.Enabled = False
            'TrDilivery.Visible = True
            'txtRemarks.Text = dt.Rows(0)("Remarks")
            'txtCatId.Text = dt.Rows(0)("CatID")
            'txtActiveStatus.Text = dt.Rows(0)("ActiveStatus")
            'If String.Equals(txtActiveStatus.Text.ToUpper(), "Y") = True Then
            '    rdblist.SelectedIndex = 0
            'Else
            '    rdblist.SelectedIndex = 1
            'End If
        End If
    End Sub
    Private Function DisableTheButton(ByVal pge As Control, ByVal btn As Control) As String
        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ")
        sb.Append("this.value = 'Please Wait...';")
        sb.Append("this.disabled = true;")
        sb.Append(pge.Page.GetPostBackEventReference(btn))
        sb.Append(";")
        Return sb.ToString()
    End Function
    Private Sub FillCategory()
        Try
            Dim ds As DataSet = New DataSet()
            Dim str As String = "select * from V#SelectCategory"
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            If (ds.Tables(0).Rows.Count > 0) Then
                DDlCategory.DataSource = ds.Tables(0)
                DDlCategory.DataTextField = "CatName"
                DDlCategory.DataValueField = "CatID"
                DDlCategory.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillSubCategory()
        Try
            Dim ds As DataSet = New DataSet()
            Dim str As String = "select * from V#SelectSubCategory"
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            If (ds.Tables(0).Rows.Count > 0) Then
                DDlSubCaegory.DataSource = ds.Tables(0)
                DDlSubCaegory.DataTextField = "CatName"
                DDlSubCaegory.DataValueField = "SubCatID"
                DDlSubCaegory.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillState()
        Try
            Dim ds As DataSet = New DataSet()
            Dim str As String = " Select StateCode,StateName from M_StateDivMaster Where ActiveStatus =  'Y' And  RowStatus = 'Y' Order by StateName"
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str)
            DDlState.DataSource = ds.Tables(0)
            DDlState.DataValueField = "StateCode"
            DDlState.DataTextField = "StateName"
            DDlState.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Function Check_IdNo() As Boolean
        Try
            Sql = "Select a.mobl,a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName,IsNull(c.Idno,'') as SponsorId,"
            Sql &= " isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,"
            Sql &= "a.LegNo,B.KitName,a.BV,b.bv as KBv,Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') "
            Sql &= "else '' end as UpgradeDate,a.ActiveStatus,a.FLD1,a.Planid,a.isblock, a.Fld4 from M_KitMaster as b,M_MemberMaster as a "
            Sql &= "Left Join M_MemberMaster as c on a.RefFormno=c.Formno  "
            Sql &= " where a.KitId=b.KitId and  (b.RowStatus='Y')  and a.IDNo='" & TxtIDNo.Text & "' and a.IsBlock='N'"
            Dim Dt_ As New DataTable
            Dt_ = obj.GetData(Sql)
            If Dt_.Rows.Count = 0 Then
                lblError.Text = " Please enter correct Member ID."
                lblError.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                LblMemName.Text = ""
                LblCondition.Text = ""
                LblNewKitid.Text = ""
                BtnUpgrade.Enabled = False
                Return False
            ElseIf Dt_.Rows(0)("isblock") = "Y" Then
                lblError.Text = "ID is blocked. Please unblock it for activation"
                lblError.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                LblMemName.Text = ""
                LblCondition.Text = ""
                LblNewKitid.Text = ""
                BtnUpgrade.Enabled = False
                Return False
            ElseIf Dt_.Rows(0)("Planid") > 0 Then
                lblError.Text = "This Member Id Rank Already Allotted."
                lblError.ForeColor = Drawing.Color.Red
                TxtIDNo.Text = ""
                LblMemName.Text = ""
                LblCondition.Text = ""
                LblNewKitid.Text = ""
                BtnUpgrade.Enabled = False
                Return False
            Else
                LblKitId.Text = Dt_.Rows(0)("KitId")
                lblError.Text = ""
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblFormno.Text = Dt_.Rows(0)("Formno")
                LblMobileNo.Text = Dt_.Rows(0)("mobl")
                BtnUpgrade.Enabled = True
                Return True
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Function
    Private Function Check_AgentIdNo() As Boolean
        Try
            Sql = "select * from V#RankAgent where 1 = 1 and MemberId='" & txtsponsorid.Text & "'"
            Dim Dt_ As New DataTable
            Dt_ = obj.GetData(Sql)
            If Dt_.Rows.Count = 0 Then
                lblError.Text = " Please enter correct Agent ID."
                lblError.ForeColor = Drawing.Color.Red
                txtsponsorid.Text = ""
                lblsponsorname.Text = ""
                lblrankid.Text = 0
                lblvendorid.Text = 0
                BtnUpgrade.Enabled = False
                Return False
            Else
                lblrankid.Text = Dt_.Rows(0)("RankId")

                lblvendorid.Text = Dt_.Rows(0)("RankId") + 2
                lblError.Text = ""
                lblsponsorname.Text = Dt_.Rows(0)("MemberName")
                lblsponsorname.Visible = True
                lblsponsorformno.Text = Dt_.Rows(0)("formno")
                BtnUpgrade.Enabled = True
                Return True
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
                Try
                    Sql = "select * from VendorRegistartion where formno  = '" & LblFormno.Text & "'"
                    Dim Dt_ As New DataTable
                    Dt_ = obj.GetData(Sql)
                    If Dt_.Rows.Count > 0 Then
                        lblError.Text = "Already Register."
                        lblError.ForeColor = Drawing.Color.Red
                        TxtIDNo.Text = ""
                        LblKitId.Text = ""
                        LblMemName.Text = ""
                        LblFormno.Text = ""
                        LblMobileNo.Text = ""
                    End If
                Catch ex As Exception
                    Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
                    Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
                    Response.Write("Try later.")
                End Try
            Else
                BtnUpgrade.Enabled = False
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Sub
    Protected Function SaveVendorDetail() As String
        Try
            Dim strQry As String = ""
            Try
                Dim updateeffect As Integer
                Dim StrSql As String = "Insert into Trnactivecadmin (Transid,Rectimestamp) values(" & HdnCheckTrnns.Value & ",getdate())"
                updateeffect = objDAL.SaveData(StrSql)
                Dim i As Integer = 0
                If updateeffect > 0 Then
                    If String.IsNullOrEmpty(Request("CatID")) = False Then
                        Sql = "select * from V#VendorReport where id = '" & Request("CatID") & "'"
                        Dim Dt_ As New DataTable
                        Dt_ = obj.GetData(Sql)
                        If Dt_.Rows.Count > 0 Then
                            LblFormno.Text = Dt_.Rows(0)("FormNo")
                        End If
                        strQry = "UPDATE VendorRegistartion SET activestatus = 'Y',IsApprove = 'Y',ApproveDate = GETDATE(),CatID = '" & DDlCategory.SelectedValue & "',"
                        strQry &= "SubCatID = '" & DDlSubCaegory.SelectedValue & "',ShopName = '" & TxtshopName.Text & "',City = '" & TxtCity.Text & "',StateCode = '" & DDlState.SelectedValue & "', "
                        strQry &= "PinCode = '" & TxtPicCode.Text & "',Cashback = '" & TxtCashback.Text & "',Rankid1 = '" & txtPARTNER.Text & "',Rankid2 = '" & txtMASTER.Text & "',"
                        strQry &= "Rankid3 = '" & txtAGENCY.Text & "',Rankid4 = '" & txtAGENT.Text & "',Rankid5 = '0',Commission = '" & TXtBonus.Text & "',sponsorid = '" & lblsponsorformno.Text & "' "
                        strQry &= " where id = '" & Request("CatID") & "' "
                        strQry &= "Exec Sp_IdWiseRankvendor '" & LblFormno.Text & "','" & lblvendorid.Text & "','" & lblsponsorformno.Text & "';"
                    Else
                        Sql = "select * from VendorRegistartion where formno  = '" & LblFormno.Text & "'"
                        Dim Dt_ As New DataTable
                        Dt_ = obj.GetData(Sql)
                        If Dt_.Rows.Count > 0 Then
                            lblError.Text = "Already Register."
                            lblError.ForeColor = Drawing.Color.Red
                            TxtIDNo.Text = ""
                            LblKitId.Text = ""
                            LblMemName.Text = ""
                            LblFormno.Text = ""
                            LblMobileNo.Text = ""
                        Else
                            strQry = "INSERT INTO VendorRegistartion(FormNo,MobileNo,CatID,SubCatID,ShopName,City,StateCode,PinCode,Cashback,"
                            strQry &= "RedistartionDate,Rankid1,Rankid2,Rankid3,Rankid4,Rankid5,Commission,sponsorid)"
                            strQry &= "VALUES('" & LblFormno.Text & "','" & LblMobileNo.Text & "','" & DDlCategory.SelectedValue & "','" & DDlSubCaegory.SelectedValue & "',"
                            strQry &= "'" & TxtshopName.Text & "','" & TxtCity.Text & "','" & DDlState.SelectedValue & "','" & TxtPicCode.Text & "'"
                            strQry &= ",'" & TxtCashback.Text & "',GETDATE(),'" & txtPARTNER.Text & "','" & txtMASTER.Text & "','" & txtAGENCY.Text & "',"
                            strQry &= "'" & txtAGENT.Text & "','0','" & TXtBonus.Text & "','" & lblsponsorformno.Text & "');"
                            strQry &= "Exec Sp_IdWiseRankvendor '" & LblFormno.Text & "','" & lblvendorid.Text & "','" & lblsponsorformno.Text & "';"
                        End If
                    End If
                    i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, strQry))
                    If (i > 0) Then
                        If String.IsNullOrEmpty(Request("CatID")) = False Then
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Vendor Registration Updated.!');location.replace('VendorRegistration.aspx');", True)
                        Else
                            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Vendor Registration Successfully');location.replace('VendorRegistration.aspx');", True)
                        End If

                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Vendor Registration Not Successfully');location.replace('VendorRegistration.aspx');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Try Again After Some Time');location.replace('VendorRegistration.aspx');", True)
                End If
            Catch ex As Exception
            End Try
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Function
    Protected Sub BtnUpgrade_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpgrade.Click
        Try
            If TxtIDNo.Text = "" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter Member ID.!');</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                Exit Sub
            End If
            If LblMobileNo.Text = "" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter Mobile No.!');</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                Exit Sub
            End If
            If Not String.IsNullOrEmpty(LblMobileNo.Text) Then
                Dim moblno As String = LblMobileNo.Text
                Dim check As String = moblno.Substring(0, 1)
                If check = "0" Then
                    LblMobileNo.Text = "0"
                    BtnUpgrade.Enabled = True
                    Dim scrname As String = "<SCRIPT language='javascript'>alert('Invalid Mobile No.!');</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                    Exit Sub
                End If
                Dim mobileNumber As String = LblMobileNo.Text
                If Regex.IsMatch(mobileNumber, "^\d{10,}$") Then
                Else
                    LblMobileNo.Text = "0"
                    BtnUpgrade.Enabled = True
                    Dim scrname As String = "<SCRIPT language='javascript'>alert('Invalid Mobile No.!');</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                    Exit Sub
                End If
            End If
            If DDlCategory.SelectedValue = "0" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Please Select Category.!');</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                Exit Sub
            End If
            If DDlSubCaegory.SelectedValue = "0" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Please Select Sub Category.!');</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                Exit Sub
            End If
            If TxtshopName.Text = "" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter Shop Name.!');</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                Exit Sub
            End If
            If TxtCity.Text = "" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter City.!');</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                Exit Sub
            End If
            If DDlState.SelectedValue = "0" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Please Select State.!');</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                Exit Sub
            End If
            If TxtPicCode.Text = "" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter PinCode.!');</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                Exit Sub
            End If
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
            'If txtEMALL.Text = "" Then
            '    Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter EMALL.!');</SCRIPT>"
            '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
            '    Exit Sub
            'End If
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
            If String.IsNullOrEmpty(Request("CatID")) = False Then
                If txtsponsorid.Text = "" Then
                    Dim scrname As String = "<SCRIPT language='javascript'>alert('Enter Sponsor Id.!');</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Login Error", scrname, False)
                    Exit Sub
                ElseIf txtsponsorid.Text <> "" Then
                    If Check_AgentIdNo() = True Then
                        SaveVendorDetail()
                    Else
                        BtnUpgrade.Enabled = False
                    End If
                Else
                    SaveVendorDetail()
                End If
            Else
                SaveVendorDetail()
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
    Public Function GenerateRandomStringAdmin(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next
        Return sResult
    End Function
    Protected Sub txtsponsorid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtsponsorid.TextChanged
        Try
            If Check_AgentIdNo() = True Then
            Else
                BtnUpgrade.Enabled = False
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Sub
    Protected Sub DDlSubCaegory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlSubCaegory.SelectedIndexChanged
        Dim sql As String = "Select * From M_SubCatMaster Where SubCatID = '" & DDlSubCaegory.SelectedValue & "' "
        dt = New DataTable
        dt = objDAL.GetData(sql)
        If dt.Rows.Count > 0 Then
            txtPARTNER.Text = dt.Rows(0)("Rankid1")
            txtMASTER.Text = dt.Rows(0)("Rankid2")
            txtAGENCY.Text = dt.Rows(0)("Rankid3")
            txtAGENT.Text = dt.Rows(0)("Rankid4")
            TxtCashback.Text = dt.Rows(0)("Cashback")
            TXtBonus.Text = dt.Rows(0)("Commission")
        End If
    End Sub
End Class
