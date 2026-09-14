Imports System.Data.SqlClient
Imports System.Data
Partial Class MonthlyStatement
    Inherits System.Web.UI.Page
    Dim objGen As clsGeneral = New clsGeneral
    Dim ObjDAL As DAL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Session("AStatus") = "OK" Then
                If Not IsPostBack Then
                    GetCompID()
                    getData()
                    objGen.GetConnectionByComp()
                    objGen.GetInvDataBaseByComp()
                    imglogo.Src = Session("Logo")
                    Dim strQuery As String = ""
                    ObjDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    ' If (Request.QueryString("Formno") <> Nothing) Then
                    'strQuery = " Exec Sp_StateMentMonthly " & Val(Request.QueryString("Formno")) & ",'" & Val(Request("PayoutNo")) & "'"
                    If (Request("formno") <> Nothing) Then
                        strQuery = " Exec Sp_StateMentMonthly " & Request("formno") & ",'" & Request("PayoutNo") & "'"
                    Else
                        Response.Redirect("logout.aspx")
                    End If

                    Dim ds As DataSet = New DataSet()
                    ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, strQuery)
                    If (ds.Tables(0).Rows.Count > 0) Then
                        MemName.InnerText = ds.Tables(0).Rows(0)("Name") & ""
                        IDNO.InnerText = ds.Tables(0).Rows(0)("Idno")
                        City.InnerText = ds.Tables(0).Rows(0)("City") & ""
                        Add.InnerText = ds.Tables(0).Rows(0)("Address1") & ""
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

                        If (Session("CompID") = "1030" Or Session("CompID") = "1038") Then
                            divMentorshipLife.Visible = True
                            repMentorship.DataSource = ds.Tables(5)
                            repMentorship.DataBind()

                        End If
                        If Session("CompId") = "1075" Then
                            divproholistic.Visible = True
                            rptproholistic.DataSource = ds.Tables(5)
                            rptproholistic.DataBind()
                        End If
                        If ds.Tables(6).Rows.Count > 0 Then
                            If (Session("CompID") = "1030") Then
                                DivSlefRepurBV.Visible = True
                                selfREP.InnerText = ds.Tables(6).Rows(0)("SelfBv")
                            ElseIf (Session("CompID") = "1038") Then
                                DivSlefRepurBV.Visible = True
                                selfREP.InnerText = ds.Tables(6).Rows(0)("SelfBv")
                            Else
                                DivSlefRepurBV.Visible = False
                                selfREP.InnerText = ds.Tables(6).Rows(0)("SelfBv")
                            End If


                        End If
                        If ds.Tables(7).Rows.Count > 0 Then
                            rptgross.DataSource = ds.Tables(7)
                            rptgross.DataBind()
                        End If
                        If ds.Tables(8).Rows.Count > 0 Then
                            rptclose.DataSource = ds.Tables(8)
                            rptclose.DataBind()

                        End If

                        If ds.Tables(9).Rows.Count > 0 Then
                            If (Session("CompID") = "1038") Then
                                divUcmPoint.Visible = True
                                TdLeadership.InnerText = ds.Tables(9).Rows(0)("LBCnt")
                                TdMasterLeadership.InnerText = ds.Tables(9).Rows(0)("MLBCnt")
                                Tdsupermasterbonus.InnerText = ds.Tables(9).Rows(0)("SMBCnt")
                                TdRoyalty.InnerText = ds.Tables(9).Rows(0)("RoyaltyCnt")
                                TdTravelling.InnerText = ds.Tables(9).Rows(0)("TravelCnt")
                                TdCar.InnerText = ds.Tables(9).Rows(0)("CarCnt")
                                TdHouse.InnerText = ds.Tables(9).Rows(0)("HouseCnt")
                                'TdTravelling.InnerText = "0.00"
                                'TdCar.InnerText = "0.00"
                                'TdHouse.InnerText = "0.00"
                            Else
                                divUcmPoint.Visible = False
                                TdLeadership.InnerText = "0.00"
                                TdMasterLeadership.InnerText = "0.00"
                                TdRoyalty.InnerText = "0.00"
                                TdTravelling.InnerText = "0.00"
                                TdCar.InnerText = "0.00"
                                TdHouse.InnerText = "0.00"

                            End If
                        Else
                        End If

                        If ds.Tables(9).Rows.Count > 0 Then
                            If (Session("CompID") = "1030") Then
                                DivLifeSalePV.Visible = False
                                DivLifeSaleEP.Visible = False
                                TDSalePV.InnerText = ds.Tables(9).Rows(0)("pv")
                                TDSaleEP.InnerText = ds.Tables(9).Rows(0)("ep")
                            End If
                        End If

                        If (Session("CompID") = "1030") Then
                            If ds.Tables(10).Rows.Count > 0 Then
                                DivSponsorSaleEP.Visible = True
                                FrnRep.DataSource = ds.Tables(10)
                                FrnRep.DataBind()
                            Else
                                DivSponsorSaleEP.Visible = True
                                FrnRep.DataSource = ds.Tables(10)
                                FrnRep.DataBind()
                            End If
                        End If



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
            url = HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("CPANEL.", "").Replace("BASICADMIN.", "").Replace("ADMIN.", "")
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
