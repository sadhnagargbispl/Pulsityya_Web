Imports System.Data
Imports System.Data.SqlClient

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Dim obj As DAL
    Dim Dt As DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim pinpoint As String
        Try

            If Not Page.IsPostBack Then
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                Dt = New DataTable
                Dim str As String = ""


                If Session("Status") = "OK" Then
                    Dim Strq As String = "Select Profilepic,idno,formno from M_MembeRMaster where Formno='" & Session("Formno") & "'"
                    Dt = New DataTable
                    obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    Dt = obj.GetData(Strq)
                    If Dt.Rows.Count > 0 Then
                        Image1.ImageUrl = Dt.Rows(0)("profilepic")
                    End If
                    'str = "select * from MovieTickets where Formno='" & Session("Formno") & "'"
                    'Dt = New DataTable
                    'obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    'Dt = obj.GetData(str)
                    'If Dt.Rows.Count > 0 Then
                    '    liPortal.Visible = True
                    '    str = "select a.IdNo,(a.MemFirstname+' '+a.MemLastname) as Name,a.Passw,a.Email,a.Mobl,a.Address1," & _
                    '         "  a.DOj, Case when a.ProfilePic='' then 'images/no_photo.jpg' " & _
                    '         " else a.ProfilePic end as ProfilePic,c.KitName,IsNull(b.IdNo,' ') as sponsorId,IsNull((b.MemFirstname+' '+b.MemLastname),' ') as sponsorName from M_Membermaster as a Left Join M_MemberMaster as b On a.RefFormno=b.Formno " & _
                    '         " ,M_KitMaster as c where   a.KitId=c.KitId " & _
                    '         " and (c.RowStatus='Y') and a.Formno='" & Session("Formno") & "'"
                    '    Dt = obj.GetData(str)
                    '    Dim qr As String = ""
                    '    Dim qr1 As String = ""
                    '    If Dt.Rows.Count > 0 Then

                    '        Dim stri As String
                    '        stri = Dt.Rows(0)("Idno") & ";" & Dt.Rows(0)("Passw") & ";" & Dt.Rows(0)("Name") & ";" & Dt.Rows(0)("Email") & ";" & Dt.Rows(0)("Mobl") & ";" & Format(Dt.Rows(0)("doj"), "dd-MMM-yyyy") & ""
                    '        Dim qre As String = Encryptdata(stri)
                    '        liMovie.InnerHtml = "  <a id=""ADeal6"" class=""Navigation_Black"" href=""" & Session("CompMovieWeb") & "/controller.aspx?user_info=" & qre & "&log_key=B72B800B-D978-43EE-9C73-F92B0D495767"" target=""_blank"">Movie Portal</a>"

                    '    End If

                    'Else
                    '    liPortal.Visible = False
                    'End If


                End If
            End If

        Catch ex As Exception

        End Try




    End Sub

    Private Function Encryptdata(ByVal Data As String) As String
        Try

            Dim strmsg As String = String.Empty
            Dim encode(Data.Length) As Byte
            encode = Encoding.UTF8.GetBytes(Data)
            strmsg = Convert.ToBase64String(encode)
            Return strmsg

        Catch ex As Exception

        End Try
    End Function

    Private Sub FillBalance()
        Try

     
            Dim Str As String = " Select * from dbo.ufnGetBalance('" & Session("Formno") & "','M')"
            Dim Dt As New DataTable
            Dt = obj.GetData(Str)
            If Dt.Rows.Count > 0 Then
                ' lblaccountbalance.InnerText = Dt.Rows(0)("Balance")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try

        Catch ex As Exception

        End Try
    End Sub
End Class

