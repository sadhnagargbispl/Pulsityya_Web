
Imports System.Data
Imports System.IO
Imports System.IO.MemoryStream
Imports System.Drawing
Imports System.Data.SqlClient
Partial Class Pancard
    Inherits System.Web.UI.Page
    Dim DbConnect As cls_DataAccess
    Private cmd As New SqlCommand
    Private dRead As SqlDataReader
    Dim dblBank As Double
    Dim tmpTable As New Data.DataTable
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member /Pan Card Verify"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub



    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Try
    '        DbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        DbConnect.OpenConnection()

    '        If Session("Status") = "OK" Then
    '            If Not Page.IsPostBack Then
    '                If Session("IsAddressverified") = "Y" Then


    '                    loadImages()
    '                End If
    '            End If
    '        Else
    '            Response.Redirect("logout.aspx")
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                loadImages()
            Else

            End If
        End If

    End Sub





    Private Sub loadImages()
        Try


            Dim cmd As SqlCommand
            Dim status As String = ""
            Dim dRead As SqlDataReader
            cmd = New SqlCommand("Select IDNo,MemFirstName As MemName,Panno,AddrProof," & _
                                 " isAddrssverified,Case when IsAddrssverified<>'N' then " & _
                                 " Replace(CONVERT(varchar,AddrssVerifyDate ,106),' ','-') " & _
                                 " Else '' End as Adressproofdate,CASE WHEN IsAddrssVerified='Y' THEN " & _
                                 " 'Verified' when IsAddrssVerified='R' then 'Rejected' Else 'Not Verified'" & _
                                 " END AS AddrVerf,case when IsAddrssVerified='R' then AddrssRemark else '' end as RejectRemark From M_MemberMaster where Formno='" & Session("Formno") & "'", DbConnect.cnnObject)
            dRead = cmd.ExecuteReader
            If (dRead.Read() = True) Then
                lblid.Text = dRead.Item("idno")
                txtpan.Text = dRead.Item("Panno")
                lblverstatus.Text = dRead.Item("AddrVerf")
                If IsDBNull(dRead.Item("Adressproofdate")) = True Then
                    Lblverdate.Text = ""
                Else
                    Lblverdate.Text = dRead.Item("Adressproofdate")
                End If

                status = dRead.Item("AddrVerf")
                If dRead.Item("AddrProof") = "" Then
                    ShowIdentity.ImageUrl = "~/images/no_photo.jpg"

                Else
                    ShowIdentity.ImageUrl = dRead.Item("AddrProof")
                    lblimage.Text = dRead.Item("AddrProof")
                End If
                LblRemark.Text = dRead.Item("RejectRemark")

                If dRead.Item("IsAddrssVerified") = "Y" Then
                    BtnIdentity.Visible = False
                    Fuidentity.Enabled = False
                    txtpan.Enabled = False
                Else
                    BtnIdentity.Visible = True
                    Fuidentity.Visible = True
                End If
            End If
            If status = "Not Verified" Then

                Lblverdate.Visible = False
                Lblon.Visible = False
            End If
            ' DbConnect.cnnObject.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Function ClearInject(ByVal StrObj As String) As String
        StrObj = Replace(Replace(Replace(StrObj, ";", ""), "'", ""), "=", "")
        Return StrObj
    End Function

    Protected Sub BtnIdentity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnIdentity.Click
        'DbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        ' DbConnect.OpenConnection()
        Try


            Dim FlNm As String = ""
            Dim scrname As String = ""
            Dim Obj As DAL
            Obj = New DAL((HttpContext.Current.Session("MlmDatabase" & Session("CompID"))))
            Dim AdrsProof As String
            Dim strextension As String = ""
            Dim Dt1 As New DataTable
            Dim str As String = ""
            Dim Remark As String = ""
            If Fuidentity.HasFile Then
                strextension = System.IO.Path.GetExtension(Fuidentity.FileName)
                If (strextension.ToUpper() = ".JPG") Or (strextension.ToUpper() = ".JPEG") Or (strextension.ToUpper() = ".PNG") Then
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(Fuidentity.PostedFile.InputStream)
                    Dim height As Integer = img.Height
                    Dim width As Integer = img.Width
                    Dim size As Decimal = Math.Round((CDec(Fuidentity.PostedFile.ContentLength) / CDec(1024)), 1)
                    If size > 1024 Then
                        scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 1mb size only!! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        Exit Sub
                    Else
                        FlNm = Format(Now, "yyMMddhhmmssfff") & Path.GetExtension(Fuidentity.PostedFile.FileName)
                        Fuidentity.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm)
                        AdrsProof = "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm
                    End If
                Else
                    scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                End If
            Else
                AdrsProof = lblimage.Text
            End If
            str = "select * from M_MemberMaster where Formno='" & Session("Formno") & "'"
            Dt1 = Obj.GetData(str)
            If Dt1.Rows.Count > 0 Then

                If ClearInject(Dt1.Rows(0)("Panno")) <> ClearInject(txtpan.Text) Then
                    Remark = Remark & " PANNo,"
                End If
                If ClearInject(Dt1.Rows(0)("AddrProof")) <> ClearInject(AdrsProof) Then
                    Remark = Remark & " PanCard,"
                End If

            End If

            Dim sql As String = "Update m_MemberMaster set AddrProof = '" & AdrsProof & "',AddrProofDate= GetDate(),Panno='" & txtpan.Text.ToUpper & "' where Formno= '" & Session("Formno") & "'"

            Dim Qry As String = sql & "Insert Into TempMemberMaster Select *,'Update PanCard - " & Context.Request.UserHostAddress.ToString & "',GetDate(),'U' From M_MemberMaster Where FormNo='" & Val(Session("FormNo")) & "'"
            Qry = Qry & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
            "(0,'" & Session("MemName") & "','Pancard','PanCard Update','" & Remark & "',Getdate(),'" & Session("FormNo") & "')"
            '  DbConnect.Fire_Query(Qry)

            ' DbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            ' DbConnect.OpenConnection()
            Dim j As Integer = DbConnect.Fire_Query(Qry)

            If j <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Pancard Upload successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
                loadImages()
            Else
                scrname = "<SCRIPT language='javascript'>alert('Pancard Upload unsuccessfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            End If
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            DbConnect.closeConnection()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            DbConnect.closeConnection()
        Catch ex As Exception

        End Try
    End Sub

End Class
