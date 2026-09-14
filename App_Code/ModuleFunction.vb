Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Net

Public Class ModuleFunction
    Dim dt As DataTable
    Dim objDal As DAL

    Public Function EncodeBase64(ByVal data As String) As String
        Dim s As String = data.Trim.Replace(" ", "+")
        If ((s.Length Mod 4) _
                    > 0) Then
            s = s.PadRight((s.Length + (4 - (s.Length Mod 4))), Microsoft.VisualBasic.ChrW(61))
        End If
        Return s
    End Function

    Public Sub FillCombo(ByVal qry As String, ByRef ddlToBeFill As DropDownList, Optional ByVal dataText As String = "", Optional ByVal dataValue As String = "")
        dt = New DataTable
        dt = objDal.GetData(qry)
        ddlToBeFill.DataSource = dt
        ddlToBeFill.DataTextField = dataText
        ddlToBeFill.DataValueField = dataValue
        ddlToBeFill.DataBind()
    End Sub

    'Public Sub New()
    '    objDal = New DAL()
    'End Sub

    Public Sub New(ByVal strConnectionString As String)
        '        _SerucityCode = strSecurityCode
        objDal = New DAL(strConnectionString)

    End Sub

    ''' <summary>
    ''' method to get Client ip address
    ''' </summary>
    ''' <param name="GetLan"> set to true if want to get local(LAN) Connected ip address</param>
    ''' <returns></returns>
    Public Function GetVisitorIPAddress(Optional ByVal GetLan As Boolean = False) As String
        Dim visitorIPAddress As String = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")

        If [String].IsNullOrEmpty(visitorIPAddress) Then
            visitorIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        End If

        If String.IsNullOrEmpty(visitorIPAddress) Then
            visitorIPAddress = HttpContext.Current.Request.UserHostAddress
        End If

        If String.IsNullOrEmpty(visitorIPAddress) OrElse visitorIPAddress.Trim() = "::1" Then
            GetLan = True
            visitorIPAddress = String.Empty
        End If

        If GetLan Then
            If String.IsNullOrEmpty(visitorIPAddress) Then
                'This is for Local(LAN) Connected ID Address
                Dim stringHostName As String = Dns.GetHostName()
                'Get Ip Host Entry
                Dim ipHostEntries As IPHostEntry = Dns.GetHostEntry(stringHostName)
                'Get Ip Address From The Ip Host Entry Address List
                Dim arrIpAddress As IPAddress() = ipHostEntries.AddressList

                Try
                    visitorIPAddress = arrIpAddress(arrIpAddress.Length - 2).ToString()
                Catch
                    Try
                        visitorIPAddress = arrIpAddress(0).ToString()
                    Catch
                        Try
                            arrIpAddress = Dns.GetHostAddresses(stringHostName)
                            visitorIPAddress = arrIpAddress(0).ToString()
                        Catch
                            visitorIPAddress = "127.0.0.1"
                        End Try
                    End Try
                End Try
            End If
        End If
        Return visitorIPAddress
    End Function
End Class
