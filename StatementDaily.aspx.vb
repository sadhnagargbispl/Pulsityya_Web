Imports System.Data.SqlClient
Imports System.Data
Partial Class Css_StatementDaily
    Inherits System.Web.UI.Page
    Dim objGen As clsGeneral = New clsGeneral
    Dim ObjDAL As DAL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GetCompID()
                getData()
                objGen.GetConnectionByComp()
                objGen.GetInvDataBaseByComp()
                imglogo.Src = Session("Logo")
                Dim strQuery As String = ""
                ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                If (Request.QueryString("Formno") <> Nothing) Then
                    strQuery = " Exec Sp_StateMentDaily " & Request.QueryString("Formno") & ",'" & Val(Request("PayoutNo")) & "'"
                ElseIf (Session("Formno") <> Nothing) Then
                    strQuery = " Exec Sp_StateMentDaily " & Val(Session("Formno")) & ",'" & Val(Request("PayoutNo")) & "'"
                Else
                    Response.Redirect("logout.aspx")
                End If

                Dim ds As DataSet = New DataSet()
                ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strQuery)
                If (ds.Tables(0).Rows.Count > 0) Then
                    MemName.InnerText = ds.Tables(0).Rows(0)("Name") & ""
                    IDNO.InnerText = ds.Tables(0).Rows(0)("Idno")
                    Add.InnerText = ds.Tables(0).Rows(0)("Address1")
                    City.InnerText = ds.Tables(0).Rows(0)("City") & ""
                    District.InnerText = ds.Tables(0).Rows(0)("District") & ""
                    Mobile.InnerText = ds.Tables(0).Rows(0)("Mobl") & ""
                    PinCode.InnerText = ds.Tables(0).Rows(0)("PinCode") & ""
                    State.InnerText = ds.Tables(0).Rows(0)("StateName") & ""

                    PayoutTime.InnerText = ds.Tables(1).Rows(0)("SessID")
                    Period.InnerText = ds.Tables(1).Rows(0)("PayoutDate") & ""

                    gvincome.DataSource = ds.Tables(2)
                    gvincome.DataBind()

                    gvDeduction.DataSource = ds.Tables(3)
                    gvDeduction.DataBind()

                    TotalEarnings.InnerText = ds.Tables(4).Rows(0)("TotalEarning")
                    TotalDeductions.InnerText = ds.Tables(4).Rows(0)("TotalDeduction")
                    NetPayble.InnerText = ds.Tables(4).Rows(0)("NetPay")

                    BfXBV.InnerText = ds.Tables(5).Rows(0)("LegXBvBF")
                    BfYBV.InnerText = ds.Tables(5).Rows(0)("LegYBvBF")

                    NewXBV.InnerText = ds.Tables(5).Rows(0)("LegXBv")
                    NewYBV.InnerText = ds.Tables(5).Rows(0)("LegYBv")

                    MatchedXBV.InnerText = ds.Tables(5).Rows(0)("wkrlegbv")
                    MatchedYBV.InnerText = ds.Tables(5).Rows(0)("wkrlegbv")

                    CfXBV.InnerText = ds.Tables(5).Rows(0)("LegXBvCF")
                    CfYBV.InnerText = ds.Tables(5).Rows(0)("LegYBvCF")



                    If (Session("CompID") = "1009" Or Session("CompID") = "1023") Then
                        divlevelIncomeGoldwings.Visible = True
                        RepLevelGoldwings.DataSource = ds.Tables(6)
                        RepLevelGoldwings.DataBind()

                    End If

                    If (Session("CompID") = "1023") Then
                        divDownlineIncome.Visible = True
                        repdownlineincome.DataSource = ds.Tables(7)
                        repdownlineincome.DataBind()

                    End If

                    If (Session("CompID") = "1030") Then
                        divMentorshipLife.Visible = True
                        repMentorship.DataSource = ds.Tables(6)
                        repMentorship.DataBind()

                    End If

                End If


            End If
        Catch ex As Exception

        End Try


    End Sub

    Public Function GetCompID() As String
        Dim url As String = String.Empty
        Dim conn As SqlConnection

        Try
            url = HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICADMIN.", "").Replace("ADMIN.", "")
            Dim str As String = String.Empty
            ''str = " Select ID,Logo from M_CompanyMasterNew Where IsActive = 1 And (Upper(URL) = '" & url.ToUpper().Trim() & "' OR  Upper(URL) = 'LOCALHOST') "

            If url = "LOCALHOST" Then
                str = " Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID from M_CompanyMasterNew Where IsActive = 1 And ID='" & ConfigurationManager.AppSettings("CompanyID") & "'"
            Else
                str = " Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID from M_CompanyMasterNew Where IsActive = 1 And (Upper(URL) = '" & url.ToUpper().Trim() & "') "

            End If


            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand
            conn = New SqlConnection(Application("sConnect"))
            conn.Open()
            cmd = New SqlCommand(str, conn)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                Session("CompID") = dRead("ID")
                Session("CompName") = dRead("Name")
                Session("Logo") = dRead("Logo")
            Else
                Response.Redirect("UnderCons.aspx", False)
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


    Private Sub getData()
        Try
            Dim dbConnect As New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dbConnect.OpenConnection()
            Dim dRead As SqlDataReader
            Dim cmd As SqlCommand

            cmd = New SqlCommand("select * from M_CompanyMaster ", dbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If dRead.Read Then
                ''Session("CompName") = dRead("CompName")
                Session("CompAdd") = dRead("CompAdd")
                Session("CompWeb") = IIf(dRead("WebSite") = "", "index.asp", dRead("WebSite"))
                ''Session("Title") = dRead("CompTitle")
                Session("CompMail") = dRead("CompMail")
                Session("CompMobile") = dRead("MobileNo")
                Session("ClientId") = dRead("smsSenderId")
                Session("SmsId") = dRead("smsUserNm")
                Session("SmsPass") = dRead("smPass")
                Session("MailPass") = dRead("mailPass")
                Session("MailHost") = dRead("mailHost")
                Session("AdminWeb") = dRead("AdminWeb")
                Session("CompCST") = dRead("CompCSTNo")
                Session("CompState") = dRead("CompState")
                Session("CompDate") = Format(dRead("RecTimeStamp"), "dd-MMM-yyyy")
                Session("Spons") = "PR10000001"
                Session("CompWeb1") = dRead("WebSite")
                Session("CompMovieWeb") = ""
                Session("SmsAPI") = ""
                Session("CompShortUrl") = IIf(HttpContext.Current.Request.Url.Host.ToUpper.StartsWith("HTTPS") = True, "https://", "https://") & HttpContext.Current.Request.Url.Host & "/" & Session("JoinPage")  ''dRead("UrlShort")
            Else
                Session("CompName") = ""
                Session("CompAdd") = ""
                Session("CompWeb") = ""
                Session("Title") = "Welcome"
            End If
            dRead.Close()

           
        Catch
            Session("CompName") = ""
            Session("CompAdd") = ""
            Session("CompWeb") = ""
        End Try
    End Sub
End Class
