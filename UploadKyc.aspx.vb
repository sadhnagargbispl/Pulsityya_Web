Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Globalization
Partial Class UploadKyc
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
    Dim tmpTable As New Data.DataTable
    Dim objGen As clsGeneral = New clsGeneral
    Dim dblBank As Double
    Dim image As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Conn.Open()
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Member / Upload Kyc "
                If Not Page.IsPostBack Then
                    'If (Session("CompID") = "1007") Then
                    '    If (GetuploadformKycPerStatus() = True) Then
                    '        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please upload your product purchase registration form then upload KYC detail');location.replace('Home.aspx');", True)
                    '        Exit Sub
                    '    End If
                    '    If (GetuploadformKycVerStatus() = True) Then
                    '        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Upload the form before uploading KYC documents, if your form already uploaded then please wait till approval of your form.');location.replace('Home.aspx');", True)
                    '        Exit Sub
                    '    End If

                    '    If (GetKycPerStatus() = False) Then
                    '        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Kyc option is temporarily off. Please try after some time.!!');location.replace('Home.aspx');", True)
                    '        Exit Sub
                    '    End If

                    'End If


                    Fill_State()
                    FillIdtypeMaster()
                    'loadImagesAddress()

                    FillBankMaster()
                    'loadImagesBank()

                    If Session("IsAddressverified") = "Y" Then
                    End If
                    'loadImagesPan()

                    'loadImagesform()

                    'loadImagesgst()
                    If (Session("CompID") = 1047) Then
                        divcity.Visible = False
                    Else
                        divcity.Visible = True
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
    Private Sub loadImagesgst()
        Try

            'If (Session("CompID") = 1007) Then
            '    divGst.Visible = True
            'Else
            '    divGst.Visible = False
            'End If

            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim c As Integer = 0
            Dim str As String = ""
            'Dim cmd As SqlCommand
            Dim status As String = ""
            Dim Dt As New DataTable
            'Dim dRead As SqlDataReader
            str = "Select a.IDNo,a.MemFirstName As MemName, " & _
             " cASE WHEN b.GSTImage LIKE '%.pdf' Or b.GstImage like '%.PDF' THEN b.gstimage else '' end as PDFLink1 ," & _
            " Case when b.GstImage like '%.pdf' Or b.GstImage like '%.PDF' then '' else b.GSTImage end as GStImage,Replace(Convert(Varchar,b.GSTImageDate,106),' ','-')as GSTImagedate, " & _
                                     " b.IsGSTVerified,Case when b.IsGSTVerified<>'N' then " & _
                                     " Replace(CONVERT(varchar,b.GSTVerifyDate ,106),' ','-') " & _
                                     " Else '' End as GstVerifyDate,CASE WHEN b.IsGSTVerified='Y' THEN " & _
                                     " 'Verified' when b.IsGSTVerified='R' then 'Rejected' Else 'Verification Due'" & _
                                     " END AS GSTVerf," & _
                                     " case when b.IsGSTVerified='R' then b.GSTRemark else '' end as RejectRemark,Isnull(f.Reason,'')As RejectReason, " & _
                                     " AadharNo3 as GSTNo" & _
                                     " From M_MemberMaster as a Inner Join KycVerify as b On a.Formno=b.Formno " & _
                                     " Left Join M_KycReject as f On b.GSTrejectId=f.Kid " & _
                                     " where a.Formno='" & LblFormno.Text & "'"
            Dt = obj.GetData(str)
            If (Dt.Rows.Count = 0) Then

            ElseIf (Dt.Rows.Count > 0) Then
                'lblid.Text = Dt.Rows(0)("idno")
                txtgst.Text = Dt.Rows(0)("GSTNo")
                'txtGstNo.Text = Dt.Rows(0)("AadharNo3")
                lblgstverstatus.Text = Dt.Rows(0)("GSTVerf")
                If Dt.Rows(0)("GSTNo") = "" Then
                    txtgst.ReadOnly = False
                Else
                    txtgst.Text = Dt.Rows(0)("GSTNo")
                    txtgst.ReadOnly = True
                End If
                If IsDBNull(Dt.Rows(0)("GSTImagedate")) = True Then
                    Lblgstverdate.Text = ""
                Else
                    Lblgstverdate.Text = Dt.Rows(0)("GSTImagedate")
                End If

                status = Dt.Rows(0)("GSTVerf")
                If IsDBNull(Dt.Rows(0)("GSTImage")) = True And IsDBNull(Dt.Rows(0)("PDFLink1")) = True Then
                    ShowgstIdentity.ImageUrl = "~/images/no_photo.jpg"
                    GstCard.HRef = "~/images/no_photo.jpg"
                    'PanCard.Visible = True
                    'GStPdf.Visible = False
                ElseIf IsDBNull(Dt.Rows(0)("GStImage")) <> True Then

                    ShowgstIdentity.ImageUrl = Dt.Rows(0)("gstimage")
                    lblgstimage.Text = Dt.Rows(0)("GSTImage")
                    GstCard.HRef = Dt.Rows(0)("GSTImage")

                    'PanCard.Visible = True
                    'GStPdf.Visible = False
                ElseIf Dt.Rows(0)("PDFLink1") <> "" Then
                    ' LblPdf.Text = Dt.Rows(0)("pdfLink")
                    GstCard.HRef = Dt.Rows(0)("pdfLink1")
                    ShowgstIdentity.ImageUrl = "~\images\pdf.png"
                    GstCard.Target = "_blank"
                End If
                If Session("CompID") = 1066 Then
                    If status <> "Verification Due" And Dt.Rows(0)("IsGSTVerified") = "N" Then

                        txtgst.Enabled = True
                    ElseIf status = "Verification Due" And txtgst.Text = "" Then
                        txtgst.Enabled = True
                    ElseIf Dt.Rows(0)("IsGSTVerified") <> "R" And IsDBNull(txtgst.Text) <> True Then
                        txtgst.Enabled = False
                    Else
                        txtgst.Enabled = True
                        c = c + 1
                    End If
                    'If IsDBNull(Dt.Rows(0)("GSTImage")) <> True Or status <> "Verification Due" And Dt.Rows(0)("IsGSTVerified") = "N" Then
                    If status <> "Verification Due" And Dt.Rows(0)("IsGSTVerified") = "N" Then
                        upgst.Enabled = True
                    ElseIf status = "Verification Due" And txtgst.Text = "" Then
                        upgst.Enabled = True
                    ElseIf (IsDBNull(Dt.Rows(0)("GSTImage")) <> True Or Dt.Rows(0)("PDFLink1") <> "") And Dt.Rows(0)("IsGSTVerified") <> "R" Then

                        upgst.Enabled = False
                    Else
                        upgst.Enabled = True
                        c = c + 1
                    End If
                    If status <> "Verification Due" And Dt.Rows(0)("IsGSTVerified") = "N" Then
                        BtnIdentity.Visible = True
                    ElseIf status = "Verification Due" And txtpan.Text = "" Then
                        BtnIdentity.Visible = True
                    ElseIf Dt.Rows(0)("IsGSTVerified") <> "R" And c = 0 Then

                        BtnIdentity.Visible = False
                        '  Fuidentity.Enabled = False
                        ' txtpan.Enabled = False
                    Else
                        BtnIdentity.Visible = True
                        ' Fuidentity.Visible = True
                        '  txtpan.Enabled = True
                    End If
                Else
                    If Dt.Rows(0)("IsGSTVerified") <> "R" And IsDBNull(txtgst.Text) <> True Then
                        'txtgst.Enabled = False
                        txtgst.Enabled = True
                    Else
                        txtgst.Enabled = True
                        c = c + 1
                    End If
                    If (IsDBNull(Dt.Rows(0)("GSTImage")) <> True Or Dt.Rows(0)("PDFLink1") <> "") And Dt.Rows(0)("IsGSTVerified") <> "R" Then
                        'upgst.Enabled = False
                        upgst.Enabled = True
                    Else
                        upgst.Enabled = True
                        c = c + 1
                    End If
                    If Dt.Rows(0)("IsGSTVerified") <> "R" And c = 0 Then
                        'BtnIdentity.Visible = False
                        BtnIdentity.Visible = True
                    Else
                        BtnIdentity.Visible = True

                    End If
                End If
                LblgstRemark.Text = Dt.Rows(0)("RejectRemark")
                LbLgstrejectRemark.Text = Dt.Rows(0)("RejectReason")
            End If
            If status = "Verification Due" Then
                gstVerifyDate.Visible = False
                Lblgstverdate.Visible = False
                LblgstVerfReason.Visible = False
                LblgstVerfRemark.Visible = False
                BtnIdentity.Enabled = True
                LbLgstrejectRemark.Text = ""
                gstVerifyDate.Text = ""
                Divgstverify.Attributes.Add("style", "color:black")
            ElseIf status = "Rejected" Then
                gstVerifyDate.Visible = True
                Lblgstverdate.Visible = True
                LblgstVerfReason.Visible = True
                LblgstVerfRemark.Visible = True
                BtnIdentity.Enabled = True
                gstVerifyDate.Text = "Reject Date:"
                Divgstverify.Attributes.Add("style", "color:red")
            Else
                gstVerifyDate.Visible = True
                Lblgstverdate.Visible = True
                LblgstVerfReason.Visible = False
                LblgstVerfRemark.Visible = False
                txtgst.Enabled = False
                upgst.Enabled = False
                BtnIdentity.Visible = False
                LbLgstrejectRemark.Text = ""
                gstVerifyDate.Text = "Verify Date:"
                Divgstverify.Attributes.Add("style", "color:Green")

            End If
            LblgstVerification.Visible = True
            ' DbConnect.cnnObject.Close()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub loadImagesform()
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim c As Integer = 0
            Dim status As String = ""
            Dim str As String = ""
            Dim dt As New DataTable
            If (CheckFormno(LblFormno.Text) = 1) Then
                str = " Select formno,FrontSideForm,BackSideForm,Remark,Adminremark,ActiveStatus,Replace(Convert(Varchar,VerifyDate,106),' ','-')as VerifyDate,(Case When ActiveStatus = 'N' Then 'Verification Due'  When ActiveStatus = 'R' Then 'Rejected'  When ActiveStatus = 'A' Then 'verify'  End ) As  Status from M_FormUpload Where Formno = '" & LblFormno.Text & "'"
                dt = obj.GetData(str)
                If (dt.Rows.Count > 0) Then

                    If dt.Rows(0)("FrontSideForm") = "" Then
                        Showfrontform.ImageUrl = "~/images/no_photo.jpg"
                        Frontform.HRef = "~/images/no_photo.jpg"
                    Else
                        Showfrontform.ImageUrl = dt.Rows(0)("FrontSideForm")
                        Frontform.HRef = dt.Rows(0)("FrontSideForm")
                        lblfrontform.Text = dt.Rows(0)("FrontSideForm")
                    End If
                    If dt.Rows(0)("BackSideForm") = "" Then
                        Showbackform.ImageUrl = "~/images/no_photo.jpg"
                        Backform.HRef = "~/images/no_photo.jpg"

                    Else
                        Showbackform.ImageUrl = dt.Rows(0)("BackSideForm")
                        lblbackform.Text = dt.Rows(0)("BackSideForm")
                        Backform.HRef = dt.Rows(0)("BackSideForm")

                    End If

                    txtRemark.Text = dt.Rows(0)("Remark")
                    lblformverstatus.Text = dt.Rows(0)("status")
                    LbLformrejectRemark.Text = dt.Rows(0)("Adminremark")
                    If IsDBNull(dt.Rows(0)("FrontSideForm")) = True Then
                        Lblformverdate.Text = ""
                    Else
                        Lblformverdate.Text = dt.Rows(0)("VerifyDate")
                        formVerifyDate.Visible = True
                    End If

                    If (dt.Rows(0)("ActiveStatus") = "A" Or dt.Rows(0)("ActiveStatus") = "N") Then
                        upfrontform.Enabled = False
                        upbackform.Enabled = False
                        txtRemark.Enabled = False
                        BtnIdentity.Enabled = False
                    Else
                        upfrontform.Enabled = True
                        upbackform.Enabled = True
                        txtRemark.Enabled = True
                        BtnIdentity.Enabled = True

                    End If
                Else
                    upfrontform.Enabled = True
                    upbackform.Enabled = True
                    txtRemark.Enabled = True
                    BtnIdentity.Visible = True
                    BtnIdentity.Enabled = True
                    Showfrontform.ImageUrl = "~/images/no_photo.jpg"
                    Frontform.HRef = "~/images/no_photo.jpg"
                    Showbackform.ImageUrl = "~/images/no_photo.jpg"
                    Backform.HRef = "~/images/no_photo.jpg"
                    txtRemark.Text = ""
                    lblformverstatus.Text = ""
                    Lblformverdate.Text = ""
                    LbLformrejectRemark.Text = ""
                End If
            
            End If
        Catch ex As Exception
            Dim scrname As String = ""
            scrname = "<SCRIPT language='javascript'>alert('" & ex.Message & "');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
        End Try
    End Sub
    Private Function CheckFormno(ByVal Formno) As Integer
        Try
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim c As Integer = 0
            Dim status As Integer
            Dim str As String = ""
            Dim dt As New DataTable
            str = " Select Count(*) As Cnt From M_FormUpload Where Formno = '" & Formno & "'"
            dt = Obj.GetData(str)
            If (Val(dt.Rows(0)("Cnt")) > 0) Then
                status = 1
            Else
                'status = 0
                status = 1
            End If
            Return status
        Catch ex As Exception

        End Try
    End Function
    Private Function CheckFormno1(ByVal Formno) As Integer
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim c As Integer = 0
            Dim status As Integer
            Dim str As String = ""
            Dim dt As New DataTable
            str = " Select Count(*) As Cnt From M_FormUpload Where Formno = '" & Formno & "'"
            dt = obj.GetData(str)
            If (Val(dt.Rows(0)("Cnt")) > 0) Then
                status = 1
            Else
                status = 0
            End If
            Return status
        Catch ex As Exception

        End Try
    End Function
    Private Sub loadImagesPan()
        Try

            
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim c As Integer = 0
            Dim str As String = ""
            'Dim cmd As SqlCommand
            Dim status As String = ""
            Dim Dt As New DataTable
            'Dim dRead As SqlDataReader
            str = "Select a.IDNo,a.MemFirstName As MemName,a.Panno, b.PanImg,Replace(Convert(Varchar,b.PANImgDate,106),' ','-')as PanProofdate, " & _
                                     " b.IsPanVerified,Case when b.IsPanVerified<>'N' then " & _
                                     " Replace(CONVERT(varchar,b.PanVerifyDate ,106),' ','-') " & _
                                     " Else '' End as PanVerifyDate,CASE WHEN b.IsPanVerified='Y' THEN " & _
                                     " 'Verified' when b.IsPanVerified='R' then 'Rejected' Else 'Verification Due'" & _
                                     " END AS PanVerf," & _
                                     " case when b.IsPanVerified='R' then b.PanRemarks else '' end as RejectRemark,Isnull(f.Reason,'')As RejectReason, " & _
                                     " AadharNo3 " & _
                                     " From M_MemberMaster as a Inner Join KycVerify as b On a.Formno=b.Formno " & _
                                     " Left Join M_KycReject as f On b.PanRejectId=f.Kid " & _
                                     " where a.Formno='" & LblFormno.Text & "'"
            Dt = obj.GetData(str)
            If (Dt.Rows.Count = 0) Then

            ElseIf (Dt.Rows.Count > 0) Then
                'lblid.Text = Dt.Rows(0)("idno")
                'hdnSessn.Value = Crypto.Encrypt(Dt.Rows(0)("idno"))
                txtpan.Text = Dt.Rows(0)("Panno")
                'txtGstNo.Text = Dt.Rows(0)("AadharNo3")
                lblpanverstatus.Text = Dt.Rows(0)("PanVerf")

                If IsDBNull(Dt.Rows(0)("PanProofdate")) = True Then
                    Lblpanverdate.Text = ""
                Else
                    Lblpanverdate.Text = Dt.Rows(0)("PanProofdate")
                End If

                status = Dt.Rows(0)("PanVerf")
                If Dt.Rows(0)("PanImg") = "" Then
                    ShowpanIdentity.ImageUrl = "~/images/no_photo.jpg"
                    PanCard.HRef = "~/images/no_photo.jpg"

                Else
                    ShowpanIdentity.ImageUrl = Dt.Rows(0)("PanImg")
                    lblpan.Text = Dt.Rows(0)("PanImg")
                    PanCard.HRef = Dt.Rows(0)("PanImg")
                End If
                If txtpan.Text.Trim.ToUpper.Length < 10 Then
                    txtpan.Enabled = True
                    c = c + 1
                ElseIf txtpan.Text.Trim.ToUpper.Length = 10 And Dt.Rows(0)("IsPanVerified") <> "R" Then
                    txtpan.Enabled = False
                End If


                'If Dt.Rows(0)("IsPanVerified") <> "" Then
                '     txtGstNo.Enabled = False
                'Else
                '    txtGstNo.Enabled = True
                'End If



                If Dt.Rows(0)("PanImg") <> "" And Dt.Rows(0)("IsPanVerified") <> "R" Then
                    uppan.Enabled = False
                Else
                    uppan.Enabled = True
                    c = c + 1
                End If
                LblpanRemark.Text = Dt.Rows(0)("RejectRemark")
                LbLpanrejectRemark.Text = Dt.Rows(0)("RejectReason")

                If Dt.Rows(0)("IsPanVerified") <> "R" And c = 0 Then
                    BtnIdentity.Visible = False
                    '  Fuidentity.Enabled = False
                    ' txtpan.Enabled = False
                Else
                    BtnIdentity.Visible = True
                    ' Fuidentity.Visible = True
                    '  txtpan.Enabled = True
                End If
            End If
            If status = "Verification Due" Then
                panVerifyDate.Visible = False
                Lblpanverdate.Visible = False
                LblpanVerfReason.Visible = False
                LblpanVerfRemark.Visible = False
                BtnIdentity.Enabled = True
                LbLpanrejectRemark.Text = ""
                panVerifyDate.Text = ""
                Divpanverify.Attributes.Add("style", "color:black")
            ElseIf status = "Rejected" Then
                panVerifyDate.Visible = True
                Lblpanverdate.Visible = True
                LblpanVerfReason.Visible = True
                LblpanVerfRemark.Visible = True
                BtnIdentity.Enabled = True
                panVerifyDate.Text = "Reject Date:"
                Divpanverify.Attributes.Add("style", "color:red")
            Else
                panVerifyDate.Visible = True
                Lblpanverdate.Visible = True
                LblpanVerfReason.Visible = False
                LblpanVerfRemark.Visible = False
                LbLpanrejectRemark.Text = ""
                panVerifyDate.Text = "Verify Date:"
                Divpanverify.Attributes.Add("style", "color:Green")

            End If
            LblpanVerification.Visible = True
            ' DbConnect.cnnObject.Close()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub FillBankMaster()
        Try

            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Strquery As String = ""
            Strquery = "SELECT BankCode as Bid,BANKNAME as Bank FROM M_BankMaster WHERE ACTIVESTATUS='Y' ORDER BY BANKCode"
            ' DbConnect.OpenConnection()
            tmpTable = obj.GetData(Strquery)
            ' DbConnect.Fill_Data_Tables(Strquery, tmpTable)
            With cmbbank
                .DataSource = tmpTable
                .DataValueField = "Bid"
                .DataTextField = "Bank"
                .DataBind()
                .SelectedIndex = 0
            End With
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub loadImagesBank()
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = ""
            ' Dim cmd As SqlCommand
            Dim status As String = ""
            Dim c As Integer = 0
            'Dim dRead As SqlDataReader
            Dim Dt As New DataTable
            str = "Select a.IDNo,a.MemFirstName As MemName,a.Panno,a.Acno,a.BAnkid,a.IFscode,a.Fax,a.Branchname,b.BankProof," & _
                                   " Case when b.ISbankverified<>'N' then Replace(CONVERT(varchar,b.BankVerifyDate,106),' ','-')" & _
                                   " Else '' End as BankProofDate,b.isBankverified,CASE WHEN b.IsBankVerified='Y' THEN 'Verified' " & _
                                   " when b.IsBankVerified='R' then 'Rejected' Else 'Verification Due' END AS BankVerf," & _
                                  " Case when b.IsBankVerified='R' then b.BankProofRemark else '' end as RejectRemark,Isnull(f.Reason,' ')As RejectReason" & _
                                   " From M_MemberMaster as a inner join KycVerify as b On a.Formno=b.Formno " & _
                                   " Left Join M_KycReject as f On b.BankRejectId=f.Kid " & _
                                   " where a.Formno='" & LblFormno.Text & "'"
            Dt = obj.GetData(str)
            If (Dt.Rows.Count = 0) Then

            ElseIf (Dt.Rows.Count > 0) Then
                'lblid.Text = Dt.Rows(0)("idno")
                'hdnSessn.Value = Crypto.Encrypt(Dt.Rows(0)("idno"))
                Txtacno.Text = Dt.Rows(0)("Acno")
                lblbankverstatus.Text = Dt.Rows(0)("BankVerf")

                Txtcode.Text = Dt.Rows(0)("IFscode")
                Txtbranch.Text = Dt.Rows(0)("Branchname")
                cmbbank.SelectedValue = Dt.Rows(0)("BAnkid")
                If IsDBNull(Dt.Rows(0)("BankProofDate")) = True Then
                    Lblbankverdate.Text = ""
                Else
                    Lblbankverdate.Text = Dt.Rows(0)("BankProofDate")

                End If
                'DDLAccountType.SelectedValue = Dt.Rows(0)("Fax")
                LbLbankrejectRemark.Text = Dt.Rows(0)("RejectReason")

                LblBankRemark.Text = Dt.Rows(0)("RejectRemark")
                status = Dt.Rows(0)("BankVerf")
                If Dt.Rows(0)("BankProof") = "" Then
                    Showbankid.ImageUrl = "~/images/no_photo.jpg"
                    BankProof.HRef = "~/images/no_photo.jpg"

                Else
                    Showbankid.ImageUrl = Dt.Rows(0)("BankProof")
                    lblbank.Text = Dt.Rows(0)("BankProof")
                    BankProof.HRef = Dt.Rows(0)("BankProof")
                End If
                If lblbank.Text <> "" And Dt.Rows(0)("IsBankVerified") <> "R" Then
                    upbank.Enabled = False
                Else
                    upbank.Enabled = True
                    c = c + 1
                End If
                If Dt.Rows(0)("IsBankVerified") <> "R" And Dt.Rows(0)("BankProof") <> "" Then
                    Txtacno.Enabled = False
                Else
                    Txtacno.Enabled = True
                    c = c + 1
                End If
                If Dt.Rows(0)("IsBankVerified") <> "R" And Txtacno.Text.Length > 10 Then
                    Txtacno.Enabled = False
                Else
                    Txtacno.Enabled = True
                    c = c + 1
                End If
                If Dt.Rows(0)("Fax").ToString.ToUpper <> "CHOOSE ACCOUNT TYPE" And Dt.Rows(0)("IsBankVerified") <> "R" Then
                    DDLAccountType.Enabled = False
                Else
                    DDLAccountType.Enabled = True
                    c = c + 1
                End If
                If Val(Dt.Rows(0)("BAnkid")) <> 0 And Dt.Rows(0)("IsBankVerified") <> "R" Then
                    cmbbank.Enabled = False
                Else
                    cmbbank.Enabled = True
                    c = c + 1
                End If
                If Dt.Rows(0)("Branchname").ToString <> "" And Dt.Rows(0)("IsBankVerified") <> "R" Then
                    Txtbranch.Enabled = False
                Else
                    Txtbranch.Enabled = True
                    c = c + 1
                End If
                If Dt.Rows(0)("IFscode").ToString <> "" And Dt.Rows(0)("IsBankVerified") <> "R" Then
                    Txtcode.Enabled = False
                Else
                    Txtcode.Enabled = True
                    c = c + 1
                End If

                If Dt.Rows(0)("isBankverified").ToString.ToUpper <> "R" And c = 0 Then
                    BtnIdentity.Visible = False
                   
                Else
                    BtnIdentity.Visible = True
                End If

            End If

            If status = "Verification Due" Then
                BankVerifyDate.Visible = False
                Lblbankverdate.Visible = False
                LblbankVerfReason.Visible = False
                LblbankVerfRemark.Visible = False
                BtnIdentity.Enabled = True
                LbLbankrejectRemark.Text = ""
                BankVerifyDate.Text = ""
                DivBankVerify.Attributes.Add("style", "color:black")
            ElseIf status = "Rejected" Then
                BankVerifyDate.Visible = True
                Lblbankverdate.Visible = True
                LblbankVerfReason.Visible = True
                LblbankVerfRemark.Visible = True
                BtnIdentity.Enabled = True
                BankVerifyDate.Text = "Reject Date:"
                DivBankVerify.Attributes.Add("style", "color:red")
            Else
                BankVerifyDate.Visible = True
                Lblbankverdate.Visible = True
                LblbankVerfReason.Visible = False
                LblbankVerfRemark.Visible = False
                LbLbankrejectRemark.Text = ""
                BankVerifyDate.Text = "Verify Date:"
                DivBankVerify.Attributes.Add("style", "color:Green")

            End If
            LblbankVerification.Visible = True
            
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Protected Sub CmbBank_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbbank.SelectedIndexChanged
        If cmbbank.SelectedItem.Text.ToUpper = "OTHERS" Then
            divBank.Visible = True
            Txtbank.Focus()
            Txtbank.Text = ""
        Else
            divBank.Visible = False
            Txtbank.Text = ""
            Txtbranch.Focus()
        End If

    End Sub
    Private Function GetuploadformKycPerStatus() As Boolean
        Try

            Dim result As Boolean = False
            Dim dt12 As DataTable = New DataTable()
            Dim Ds12 As New DataSet
            Dim str12 As String = " select FrontSideForm,BackSideForm from M_FormUpload where Formno= '" & LblFormno.Text & "'"
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str12)
            dt12 = Ds12.Tables(0)
            If (dt12.Rows.Count = 0) Then
                result = False
            ElseIf (dt12.Rows(0)("FrontSideForm") = "") Then
                result = True
            ElseIf (dt12.Rows(0)("BackSideForm") = "") Then
                result = True
            Else
                result = False
            End If
            Return result
        Catch ex As Exception

        End Try
    End Function
    Private Function GetuploadformKycVerStatus() As Boolean
        Try

            Dim result As Boolean = False
            Dim dt12 As DataTable = New DataTable()
            Dim Ds12 As New DataSet
            Dim str12 As String = " select Activestatus from M_FormUpload where Formno= '" & LblFormno.Text & "' "
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str12)
            dt12 = Ds12.Tables(0)
            If (dt12.Rows.Count = 0) Then
                result = True
            ElseIf (dt12.Rows(0)("ActiveStatus") = "A") Then
                result = False
            ElseIf (dt12.Rows(0)("ActiveStatus") = "N") Then
                result = True
            ElseIf (dt12.Rows(0)("ActiveStatus") = "R") Then
                result = True
            Else
                result = False
            End If
            Return result
        Catch ex As Exception

        End Try
    End Function
    Private Function GetKycPerStatus() As Boolean
        Try

            Dim result As Boolean = False
            Dim dt12 As DataTable = New DataTable()
            Dim Ds12 As New DataSet
            Dim str12 As String = "   Select IsWithDra From m_KycPer"
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str12)
            dt12 = Ds12.Tables(0)
            If (dt12.Rows(0)("IsWithDra").ToString().ToUpper() = "Y") Then
                result = True
            Else
                result = False
            End If
            Return result
        Catch ex As Exception

        End Try
    End Function
    Private Sub Fill_State()
        Try
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim dt As DataTable = New DataTable()
            Dim str As String = " Select StateCode,StateName from M_STateDivMaster Where ActiveStatus ='Y' And RowStatus = 'Y' Order by StateCode"
            dt = Obj.GetData(str)
            If (dt.Rows.Count > 0) Then
                ddlState.DataSource = dt
                ddlState.DataValueField = "StateCode"
                ddlState.DataTextField = "StateName"
                ddlState.DataBind()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillIdtypeMaster()
        Try
            Dim strQuery As String = ""
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            strQuery = "SELECT Id,IdType  FROM M_IdTypeMaster WHERE ACTIVESTATUS='Y' "
            tmpTable = Obj.GetData(strQuery)
            With DDLAddressProof
                .DataSource = tmpTable
                .DataValueField = "Id"
                .DataTextField = "IdType"
                .DataBind()
                .SelectedIndex = 0
            End With
            For s = 0 To tmpTable.Rows.Count - 1
                If tmpTable.Rows(s)("id") <> "0" Then
                    'LblIdproofText.Text = LblIdproofText.Text & tmpTable.Rows(s)("Idtype") & ","
                End If
            Next
            
        Catch e As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Obj.WriteToFile(text & e.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub loadImagesAddress()
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim c As Integer = 0
            Dim status As String = ""
            Dim str As String = ""
            Dim dt As New DataTable
            str = " Select a.IDNo,a.MemFirstName As MemName,a.Address1,a.City,a.Tehsil,a.District,AreaCode,"
            str &= " CityCode,DistrictCode, a.Statecode,a.Pincode,b.IdproofNo,b.AddrProof,b.BackAddressProof,Case when b.IsAddrssverified<>'N' then "
            str &= " Replace(CONVERT(varchar,b.AddrssVerifyDate,106),' ','-') Else '' End as AddrProofDate, "
            str &= " b.IsAddrssverified,CASE WHEN b.IsAddrssverified='Y' THEN 'Verified' when b.IsAddrssverified='R'  "
            str &= " then 'Rejected' Else 'Verification Due' END AS idVerf,Case when b.IsAddrssverified='R'  "
            str &= " then b.AddrssRemark else'' end as RejectRemark,BackAddressDate,c.IdType,c.id,Isnull(f.Reason,'')As RejectReason,A.aadharno   "
            str &= " From M_MemberMaster  AS A Inner Join KycVerify as b On a.Formno=b.formno Left Join M_KycReject as f On f.Kid=b.AddressRejectId "
            str &= " Inner Join M_IdTypeMaster as c  On b.IdTYpe=c.Id and C.ActiveStatus='Y' where  b.Formno='" & LblFormno.Text & "'"
            dt = obj.GetData(str)
            If (dt.Rows.Count = 0) Then

            ElseIf (dt.Rows.Count > 0) Then
                'lblid.Text = dt.Rows(0)("idno")
                'hdnSessn.Value = Crypto.Encrypt(dt.Rows(0)("idno"))
                txtaddrs.Text = dt.Rows(0)("Address1")
                Txtpincode.Text = dt.Rows(0)("Pincode")
                lblverstatus.Text = dt.Rows(0)("idVerf")
                DDLAddressProof.SelectedValue = dt.Rows(0)("Id")
                ddlState.SelectedValue = dt.Rows(0)("Statecode")
                txtDistrict.Text = dt.Rows(0)("District")
                txtCity.Text = dt.Rows(0)("City")
                If Session("CompId") = "1007" Then
                    DDLAddressProof.SelectedValue = 1
                    'TxtIdProofNo.Text = dt.Rows(0)("aadharno")
                    TxtIdProofNo.Text = dt.Rows(0)("IdProofNo")
                Else
                    TxtIdProofNo.Text = dt.Rows(0)("IdProofNo")
                End If
                If IsDBNull(dt.Rows(0)("AddrProofDate")) = True Then
                    Lblverdate.Text = ""
                Else
                    Lblverdate.Text = dt.Rows(0)("AddrProofDate")
                End If
                LblRemark.Text = dt.Rows(0)("RejectRemark")
                LbLrejectRemark.Text = dt.Rows(0)("RejectReason")
                status = dt.Rows(0)("Idverf")
                If dt.Rows(0)("AddrProof") = "" Then
                    ShowIdentity.ImageUrl = "~/images/no_photo.jpg"
                    FrontAddress.HRef = "~/images/no_photo.jpg"
                Else
                    ShowIdentity.ImageUrl = dt.Rows(0)("AddrProof")
                    lblimage.Text = dt.Rows(0)("AddrProof")
                    FrontAddress.HRef = dt.Rows(0)("AddrProof")
                End If
                If dt.Rows(0)("BackAddressProof") = "" Then
                    showBackImage.ImageUrl = "~/images/no_photo.jpg"
                    BackAddress.HRef = "~/images/no_photo.jpg"

                Else
                    showBackImage.ImageUrl = dt.Rows(0)("BackAddressProof")
                    LblBackImage.Text = dt.Rows(0)("BackAddressProof")
                    BackAddress.HRef = dt.Rows(0)("BackAddressProof")

                End If
                If Val(Txtpincode.Text) <> 0 And dt.Rows(0)("IsAddrssVerified") <> "R" Then
                    Txtpincode.Enabled = False
                Else
                    Txtpincode.Enabled = True
                    c = c + 1
                End If
                If (txtDistrict.Text) <> "" And dt.Rows(0)("IsAddrssVerified") <> "R" Then
                    txtDistrict.Enabled = False
                Else
                    txtDistrict.Enabled = True
                    c = c + 1
                End If
                If (txtCity.Text) <> "" And dt.Rows(0)("IsAddrssVerified") <> "R" Then
                    txtCity.Enabled = False
                Else
                    c = c + 1
                    txtCity.Enabled = True
                End If
                If (ddlState.SelectedValue) <> 0 And dt.Rows(0)("IsAddrssVerified") <> "R" Then
                    ddlState.Enabled = False
                Else
                    c = c + 1
                    ddlState.Enabled = True
                End If
                If (txtaddrs.Text) <> "" And dt.Rows(0)("IsAddrssVerified") <> "R" Then
                    txtaddrs.Enabled = False
                Else
                    txtaddrs.Enabled = True
                    c = c + 1
                End If
                If DDLAddressProof.SelectedValue > 0 And dt.Rows(0)("IsAddrssVerified") <> "R" And TxtIdProofNo.Text.Trim <> "" Then
                    DDLAddressProof.Enabled = False
                Else
                    DDLAddressProof.Enabled = True
                    c = c + 1
                End If
                If TxtIdProofNo.Text.Trim <> "" And dt.Rows(0)("IsAddrssVerified") <> "R" Then
                    TxtIdProofNo.Enabled = False
                Else
                    TxtIdProofNo.Enabled = True
                    c = c + 1
                End If
                If dt.Rows(0)("AddrProof") <> "" And dt.Rows(0)("IsAddrssVerified") <> "R" Then
                    Fuidentity.Enabled = False
                Else
                    Fuidentity.Enabled = True
                    c = c + 1
                End If
                If dt.Rows(0)("BackAddressProof") <> "" And dt.Rows(0)("IsAddrssVerified") <> "R" Then
                    FileUpload1.Enabled = False
                Else
                    FileUpload1.Enabled = True
                    c = c + 1
                End If
                If dt.Rows(0)("IsAddrssVerified") <> "R" And c = 0 Then
                    BtnIdentity.Visible = False
                    LbLrejectRemark.Text = ""
                Else
                    BtnIdentity.Visible = True
                End If
            End If
            If status = "Verification Due" Then
                VerifyDate.Visible = False
                Lblverdate.Visible = False
                LblVerfReason.Visible = False
                LblVerfRemark.Visible = False
                LbLrejectRemark.Text = ""
                VerifyDate.Text = ""
                BtnIdentity.Enabled = True
                DivVerify.Attributes.Add("style", "color:black")
            ElseIf status = "Rejected" Then
                VerifyDate.Visible = True
                Lblverdate.Visible = True
                LblVerfReason.Visible = True
                LblVerfRemark.Visible = True
                BtnIdentity.Enabled = True
                VerifyDate.Text = "Reject Date:"
                DivVerify.Attributes.Add("style", "color:red")
            Else
                VerifyDate.Visible = True
                Lblverdate.Visible = True
                LblVerfReason.Visible = False
                LblVerfRemark.Visible = False
                LbLrejectRemark.Text = ""
                VerifyDate.Text = "Verify Date:"
                DivVerify.Attributes.Add("style", "color:Green")
            End If
            LblVerification.Visible = True
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Function Check_IdNo() As Boolean
        Try
            Sql = "Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName,IsNull(c.Idno,'') as SponsorId,"
            Sql &= " isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,"
            Sql &= "a.LegNo,B.KitName,a.BV,b.bv as KBv,Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') "
            Sql &= "Else '' end as UpgradeDate,a.ActiveStatus,a.FLD1,a.Planid,a.isblock , a.Fld4 from M_KitMaster as b,M_MemberMaster as a "
            Sql &= "Left Join M_MemberMaster as c on a.RefFormno=c.Formno where a.KitId=b.KitId and  (b.RowStatus='Y')  "
            Sql &= "and a.IDNo='" & TxtIDNo.Text & "'"

            Dim Dt_ As New DataTable
            Dt_ = obj.GetData(Sql)
            If (Dt_.Rows.Count = 0) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Invalid Idno.!');location.replace('UploadKyc.aspx');", True)
                Exit Function
            ElseIf (Dt_.Rows(0)("ActiveStatus") = "N") Then
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblFormno.Text = Dt_.Rows(0)("Formno")
                LblMemName.ForeColor = Drawing.Color.Black
                BtnIdentity.Enabled = False
                BtnIdentity.Visible = False
                'loadImagesAddress()
                ddlkyctype.SelectedValue = 0
                divAddress.Visible = False
                divbankproof.Visible = False
                divPanproof.Visible = False
                divform.Visible = False
                divgstn.Visible = False
                Return True
            Else
                LblMemName.Text = Dt_.Rows(0)("MemName")
                LblFormno.Text = Dt_.Rows(0)("Formno")
                LblMemName.Visible = True
                ddlkyctype.SelectedValue = 0
                divAddress.Visible = False
                divbankproof.Visible = False
                divPanproof.Visible = False
                divform.Visible = False
                divgstn.Visible = False
                BtnIdentity.Enabled = False
                BtnIdentity.Visible = False
                Return True
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Function

    Protected Sub TxtIDNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtIDNo.TextChanged
        Try
            If Check_IdNo() = True Then
                '    BtnIdentity.Enabled = True

                'Else
                '    BtnIdentity.Enabled = False
            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            Response.Write("Try later.")
        End Try
    End Sub

    Protected Sub ddlkyctype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlkyctype.SelectedIndexChanged
        If ddlkyctype.SelectedValue = "A" Then
            divAddress.Visible = True
            divbankproof.Visible = False
            divPanproof.Visible = False
            divform.Visible = False
            divgstn.Visible = False
            'BtnIdentity.Enabled = True
            'BtnIdentity.Visible = True
            loadImagesAddress()
        ElseIf ddlkyctype.SelectedValue = "B" Then
            divAddress.Visible = False
            divbankproof.Visible = True
            divPanproof.Visible = False
            divform.Visible = False
            divgstn.Visible = False
            'BtnIdentity.Enabled = True
            'BtnIdentity.Visible = True
            loadImagesBank()
        ElseIf ddlkyctype.SelectedValue = "P" Then
            divAddress.Visible = False
            divbankproof.Visible = False
            divPanproof.Visible = True
            divform.Visible = False
            divgstn.Visible = False
            'BtnIdentity.Enabled = True
            'BtnIdentity.Visible = True
            panlink.Visible = True
            loadImagesPan()

        ElseIf ddlkyctype.SelectedValue = "F" Then
            divAddress.Visible = False
            divbankproof.Visible = False
            divPanproof.Visible = False
            divform.Visible = True
            divgstn.Visible = False
            'BtnIdentity.Enabled = True
            'BtnIdentity.Visible = True
            loadImagesform()
        ElseIf ddlkyctype.SelectedValue = "G" Then
            divAddress.Visible = False
            divbankproof.Visible = False
            divPanproof.Visible = False
            divform.Visible = False
            divgstn.Visible = True
            'BtnIdentity.Enabled = True
            'BtnIdentity.Visible = True
            loadImagesgst()
        Else
            divAddress.Visible = False
            divbankproof.Visible = False
            divPanproof.Visible = False
            divform.Visible = False
            divgstn.Visible = False
            BtnIdentity.Enabled = False
            BtnIdentity.Visible = False
        End If
    End Sub

    Protected Sub BtnIdentity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnIdentity.Click
        Try
            If ddlkyctype.SelectedValue = "A" Then
                SaveAddressproof()
            ElseIf ddlkyctype.SelectedValue = "B" Then
                SaveBankproof()
            ElseIf ddlkyctype.SelectedValue = "P" Then
                Savepanproof()
            ElseIf ddlkyctype.SelectedValue = "F" Then
                Saveformproof()
            ElseIf ddlkyctype.SelectedValue = "G" Then
                Savegstproof()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Select Kyc Type');location.replace('UploadKyc.aspx');", True)
                Exit Sub

            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub Savegstproof()
        Try


            Dim FlNm As String = ""
            Dim scrname As String = ""
            Dim Obj As DAL
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim gstProof As String
            Dim strextension As String = ""
            Dim Dt1 As New DataTable
            Dim str As String = ""
            Dim Remark As String = ""
            Dim s As String = ""
            If txtgst.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Please Enter GST No !! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If

            If (Session("CompID") = "1007") Then
                If (GetuploadformKycPerStatus() = True) Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please upload your product purchase registration form then upload KYC detail');location.replace('Home.aspx');", True)
                    Exit Sub
                End If
                If (GetuploadformKycVerStatus() = True) Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Upload the form before uploading KYC documents.');location.replace('Home.aspx');", True)
                    Exit Sub
                End If
            End If

            If (upgst.Enabled = True) Then
                If upgst.HasFile = False Then
                    scrname = "<SCRIPT language='javascript'>alert('Please upload GSTN image jpg/jpeg/png/pdf file of upto 1 mb size only!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                End If
            End If
            ''modify by deeksha (Session("CompID") = "1013") 



            If upgst.HasFile Then
                strextension = System.IO.Path.GetExtension(upgst.FileName)
                If (strextension.ToUpper() = ".JPG") Or (strextension.ToUpper() = ".JPEG") Or (strextension.ToUpper() = ".PNG") Then
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(upgst.PostedFile.InputStream)
                    Dim height As Integer = img.Height
                    Dim width As Integer = img.Width
                    Dim size As Decimal = Math.Round((CDec(upgst.PostedFile.ContentLength) / CDec(1024)), 1)
                    If size > 1024 Then
                        scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/pdf of upto 1mb size only!! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                        Exit Sub
                    Else
                        FlNm = Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(upgst.PostedFile.FileName)
                        upgst.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm)
                        gstProof = "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm
                    End If
                ElseIf (strextension.ToUpper() = ".PDF") Then
                    FlNm = Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(upgst.PostedFile.FileName)
                    upgst.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm)
                    gstProof = "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm


                Else
                    scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG and PDF extension file!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                End If
            Else
                gstProof = lblgstimage.Text
            End If
            str = "Select a.IDNo,a.MemFirstName As MemName,a.Aadharno3 as GSTNo, b.GStImage,Replace(Convert(Varchar,b.GStImageDate,106),' ','-')as GStImageProofdate, " & _
                                 " b.IsGSTVerified,Case when b.IsGSTVerified<>'N' then " & _
                                 " Replace(CONVERT(varchar,b.GStVerifyDate ,106),' ','-') " & _
                                 " Else '' End as GStVerifyDate,CASE WHEN b.IsGSTVerified='Y' THEN " & _
                                 " 'Verified' when b.IsGSTVerified='R' then 'Rejected' Else 'Not Verified'" & _
                                 " END AS PanVerf,case when b.IsGSTVerified='R' then b.GstRemark else '' end as RejectRemark " & _
                                 " From M_MemberMaster as a Inner Join KycVerify as b On a.Formno=b.Formno where a.Formno='" & LblFormno.Text & "'"
            Dt1 = Obj.GetData(str)
            If Dt1.Rows.Count > 0 Then

                If ClearInject(Dt1.Rows(0)("GSTNo")) <> ClearInject(txtgst.Text) Then
                    Remark = Remark & " GSTNo,"
                End If
                If ClearInject(IsDBNull(Dt1.Rows(0)("GStImage")) = True) <> ClearInject(gstProof) Then
                    Remark = Remark & " GST Cerificate,"
                End If

            End If

            Dim Qry As String = "Insert Into TempMemberMaster Select *,'Update GST Certificate - " & Context.Request.UserHostAddress.ToString & "',GetDate(),'U' From M_MemberMaster Where FormNo='" & LblFormno.Text & "'"
            Qry = Qry & "Insert Into TempKycVerify (formno,	IdType,	IdProofNo,	IdProof,	IdProofDate,	IsIdVerified,	IdVerifyDate,	IdRemark,"
            Qry = Qry & " IdUserid,	AddrProof,	AddrProofDate,	IsAddrssVerified,	AddrssVerifyDate,	AddrssRemark,AddrssUserId,	BankProof,	BankProofDate,	BackAddressProof,	BackAddressDate,	IsBankVerified,"
            Qry = Qry & " BankProofRemark,	BankVerifyDate,	BankUserId,	PanImg,	PANImgDate,	IsPanVerified,	PanRemarks,	PanVerifyDate,	PanUserid,	IdRejectId,AddressRejectId,	BankRejectId,	PanRejectId,"
            Qry = Qry & " GSTImageDate,	GSTVerifyDate,	IsGSTVerified,	GSTRemark,	GSTrejectId	,GSTImage,Rectimestamp) "
            Qry = Qry & " Select formno,	IdType,	IdProofNo,	IdProof,	IdProofDate,	IsIdVerified,	IdVerifyDate,	IdRemark, IdUserid,	AddrProof,	AddrProofDate,	IsAddrssVerified,	AddrssVerifyDate,	AddrssRemark,"
            Qry = Qry & " AddrssUserId,	BankProof,	BankProofDate,	BackAddressProof,	BackAddressDate,	IsBankVerified,BankProofRemark,	BankVerifyDate,	BankUserId,	PanImg,	PANImgDate,	IsPanVerified,	PanRemarks,	PanVerifyDate,	PanUserid,	IdRejectId, "
            Qry = Qry & "AddressRejectId,	BankRejectId,	PanRejectId,GSTImageDate,	GSTVerifyDate,	IsGSTVerified,	GSTRemark,	GSTrejectId	,GSTImage,GEtdate() From KycVerify Where FormNo='" & LblFormno.Text & "'"
            Qry = Qry & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
            "(0,'" & LblMemName.Text & "','GST Certificate','GSt Certificate Update','" & Remark & "',Getdate(),'" & LblFormno.Text & "')"
            Dim sql As String = Qry & "Update m_MemberMaster set AAdharNo3='" & txtgst.Text.ToUpper & "' where Formno= '" & LblFormno.Text & "'"
            sql = sql & " Update KycVerify Set  GStImage='" & gstProof & "',GStImageDate=Getdate(),GStVerifyDate=Getdate(),IsGSTVerified='Y' where Formno= '" & LblFormno.Text & "'"
         
            Dim j As Integer = Obj.SaveData(sql)

            If j <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert('GST Certificate Upload successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
                loadImagesgst()
            Else
                scrname = "<SCRIPT language='javascript'>alert('GST Certificate Upload unsuccessfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Sub Saveformproof()
        Try
            Dim scrname As String = ""
            If (Session("CompID") = "1007") Then
                If (GetKycPerStatus() = False) Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Kyc option is temporarily off. Please try after some time.!!');location.replace('Home.aspx');", True)
                    Exit Sub
                End If

            End If
            If txtRemark.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Please Enter Remark!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", ScrName, False)
                Exit Sub
            End If


            Dim strextension As String = ""
            Dim FlNm As String = ""
            Dim frontformProof As String
            Dim BackformProof As String
            If (upfrontform.Enabled = True) Then
                If upfrontform.HasFile = False Then
                    scrname = "<SCRIPT language='javascript'>alert('Please upload Front form image jpg/jpeg/png/pdf file of upto 1 mb size only!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                End If
            End If


            If upfrontform.HasFile Then
                strextension = System.IO.Path.GetExtension(upfrontform.FileName)
                If (strextension.ToUpper() = ".JPG") Or (strextension.ToUpper() = ".JPEG") Or (strextension.ToUpper() = ".PNG") Then
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(upfrontform.PostedFile.InputStream)
                    Dim height As Integer = img.Height
                    Dim width As Integer = img.Width
                    Dim size As Decimal = Math.Round((CDec(upfrontform.PostedFile.ContentLength) / CDec(1024)), 1)
                    If size > 1024 Then
                        ScrName = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 1mb size only!! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", ScrName, False)
                        Exit Sub
                    Else
                        FlNm = Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(upfrontform.PostedFile.FileName)
                        upfrontform.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm)
                        frontformProof = "https://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm
                    End If
                Else
                    ScrName = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", ScrName, False)
                    Exit Sub
                End If
            Else
                frontformProof = lblfrontform.Text
            End If
            If (upbackform.Enabled = True) Then
                If upbackform.HasFile = False Then
                    scrname = "<SCRIPT language='javascript'>alert('Please upload Back form image jpg/jpeg/png/pdf file of upto 1 mb size only!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                End If
            End If

            If upbackform.HasFile Then
                strextension = System.IO.Path.GetExtension(upbackform.FileName)
                If (strextension.ToUpper() = ".JPG") Or (strextension.ToUpper() = ".JPEG") Or (strextension.ToUpper() = ".PNG") Then
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(upbackform.PostedFile.InputStream)
                    Dim height As Integer = img.Height
                    Dim width As Integer = img.Width
                    Dim size As Decimal = Math.Round((CDec(upbackform.PostedFile.ContentLength) / CDec(1024)), 1)
                    If size > 1024 Then
                        ScrName = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 1mb size only!! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", ScrName, False)
                        Exit Sub
                    Else
                        FlNm = Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(upbackform.PostedFile.FileName)
                        upbackform.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm)
                        BackformProof = "https://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm
                    End If
                Else
                    ScrName = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", ScrName, False)
                    Exit Sub
                End If
            Else


                BackformProof = lblbackform.Text

            End If



            Dim Str As String
            If (CheckFormno1(LblFormno.Text) = 0) Then
                Str = ""
                Str = "Insert Into M_FormUpload (Formno,FrontSideForm,BackSideForm,ActiveStatus,Remark,AdminRemark,VerifyBy)"
                Str &= "Values('" & LblFormno.Text & "','" & frontformProof & "','" & BackformProof & "','A','" & txtRemark.Text & "','','" & Val(Session("UserID")) & "')"
                Str &= "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
           "(0,'" & LblMemName.Text & "','Form,','Form Update','" & txtRemark.Text & "',Getdate(),'" & LblFormno.Text & "')"
            End If
            If (CheckFormno1(LblFormno.Text) = 1) Then
                Str = ""
                Str = " Update M_FormUpload Set FrontSideForm ='" & frontformProof & "',BackSideForm = '" & BackformProof & "',"
                Str &= " ActiveStatus ='A',VerifyDate=getdate(),Remark ='" & txtRemark.Text & "',VerifyBy='" & Val(Session("UserID")) & "' Where Formno = '" & LblFormno.Text & "'"
                Str &= "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
           "(0,'" & LblMemName.Text & "','Form,','Form Update','" & txtRemark.Text & "',Getdate(),'" & LblFormno.Text & "')"
            End If
            Dim j As Integer = obj.SaveData(Str)

            If j <> 0 Then
                ScrName = "<SCRIPT language='javascript'>alert('Form Upload successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", ScrName, False)

            Else
                ScrName = "<SCRIPT language='javascript'>alert('Form Upload unsuccessfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", ScrName, False)
            End If
            loadImagesform()
        Catch ex As Exception
            Dim scrname As String = ""
            scrname = "<SCRIPT language='javascript'>alert('" & ex.Message & "');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
        End Try
    End Sub
    Private Sub Savepanproof()
        Try
            Dim scrname As String = ""
            If txtpan.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Please Enter Pan card Number.');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", ScrName)
                Exit Sub
            End If

            If (Session("CompID") = "1007") Then
                If (GetuploadformKycPerStatus() = True) Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please upload your product purchase registration form then upload KYC detail');location.replace('Home.aspx');", True)
                    Exit Sub
                End If
                If GetaadharStatus() = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please upload Adhar Card then upload PAN Card!!');location.replace('Home.aspx');", True)
                    Exit Sub
                End If
                If (GetuploadformKycVerStatus() = True) Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Upload the form before uploading KYC documents.');location.replace('Home.aspx');", True)
                    Exit Sub
                End If
                If (GetKycPerStatus() = False) Then
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Kyc option is temporarily off. Please try after some time.!!');location.replace('Home.aspx');", True)
                    Exit Sub
                End If
            End If
            Dim FlNm As String = ""

            Dim Obj As DAL
            Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim panProof As String
            Dim strextension As String = ""
            Dim Dt1 As New DataTable
            Dim str As String = ""
            Dim Remark As String = ""
            Dim s As String = ""

            If (uppan.Enabled = True) Then
                If uppan.HasFile = False Then
                    ScrName = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 1 mb size only!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", ScrName, False)
                    Exit Sub
                End If
            End If
            If (Session("CompID") = "1007") Then
                If txtpan.Text <> "" Then
                    s = "select Count(Panno) as PanNo from M_Membermaster where Panno='" & txtpan.Text.Trim & "' and Formno<>'" & LblFormno.Text & "'"
                    Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    Dim Dt As DataTable
                    Dt = New DataTable
                    Dt = Obj.GetData(s)
                    'If Dt.Rows(0)("Panno") >= 1 Then
                    If Dt.Rows(0)("Panno") >= 1000000 Then
                        BtnIdentity.Enabled = True

                        scrname = "<SCRIPT language='javascript'>alert('Your Pan card Number already registered on another Ids');" & "</SCRIPT>"
                        Me.RegisterStartupScript("MyAlert", scrname)
                        Exit Sub
                    End If
                End If
            End If
            If uppan.HasFile Then
                strextension = System.IO.Path.GetExtension(uppan.FileName)
                If (strextension.ToUpper() = ".JPG") Or (strextension.ToUpper() = ".JPEG") Or (strextension.ToUpper() = ".PNG") Then
                    Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(uppan.PostedFile.InputStream)
                    Dim height As Integer = img.Height
                    Dim width As Integer = img.Width
                    Dim size As Decimal = Math.Round((CDec(uppan.PostedFile.ContentLength) / CDec(1024)), 1)
                    If size > 1024 Then
                        ScrName = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 1mb size only!! ');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", ScrName, False)
                        Exit Sub
                    Else
                        FlNm = Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(uppan.PostedFile.FileName)
                        uppan.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm)
                        panProof = "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm
                    End If
                Else
                    ScrName = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", ScrName, False)
                    Exit Sub
                End If
            Else
                panProof = lblpan.Text
            End If
            str = "Select a.IDNo,a.MemFirstName As MemName,a.Panno, b.PanImg,Replace(Convert(Varchar,b.PANImgDate,106),' ','-')as PanProofdate, " & _
                                 " b.IsPanVerified,Case when b.IsPanVerified<>'N' then " & _
                                 " Replace(CONVERT(varchar,b.PanVerifyDate ,106),' ','-') " & _
                                 " Else '' End as PanVerifyDate,CASE WHEN b.IsPanVerified='Y' THEN " & _
                                 " 'Verified' when b.IsPanVerified='R' then 'Rejected' Else 'Not Verified'" & _
                                 " END AS PanVerf,case when b.IsPanVerified='R' then b.PanRemarks else '' end as RejectRemark " & _
                                 " From M_MemberMaster as a Inner Join KycVerify as b On a.Formno=b.Formno where a.Formno='" & LblFormno.Text & "'"
            Dt1 = Obj.GetData(str)
            If Dt1.Rows.Count > 0 Then

                If ClearInject(Dt1.Rows(0)("Panno")) <> ClearInject(txtpan.Text) Then
                    Remark = Remark & " PANNo,"
                End If
                If ClearInject(Dt1.Rows(0)("PanImg")) <> ClearInject(panProof) Then
                    Remark = Remark & " PanCardImage,"
                End If

            End If
            Dim Qry As String = "Insert Into TempMemberMaster Select *,'Update PanCard - " & Context.Request.UserHostAddress.ToString & "',GetDate(),'U' From M_MemberMaster Where FormNo='" & LblFormno.Text & "'"
            Qry = Qry & "Insert Into TempKycVerify Select *,GetDate(),'" & LblFormno.Text & "' From KycVerify Where FormNo='" & LblFormno.Text & "'"
            Qry = Qry & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
            "(0,'" & LblMemName.Text & "','Pancard','PanCard Update','" & Remark & "',Getdate(),'" & LblFormno.Text & "')"

            Dim sql As String = Qry & "Update m_MemberMaster set Panno='" & txtpan.Text.ToUpper & "' where Formno= '" & LblFormno.Text & "'"
            sql = sql & " Update KycVerify Set  PanImg='" & panProof & "',PANImgDate=Getdate(),PanVerifyDate=Getdate(),IsPanVerified='Y' where Formno= '" & LblFormno.Text & "'"
            '  DbConnect.Fire_Query(Qry)

            ' DbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            ' DbConnect.OpenConnection()
            Dim j As Integer = Obj.SaveData(sql)

            If j <> 0 Then
                ScrName = "<SCRIPT language='javascript'>alert('Pancard Upload successfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", ScrName, False)
                loadImagesPan()
            Else
                ScrName = "<SCRIPT language='javascript'>alert('Pancard Upload unsuccessfuly. ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", ScrName, False)

            End If
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
    End Sub
    Private Function GetaadharStatus() As Boolean
        Try
            Dim result As Boolean = False
            Dim dt12 As DataTable = New DataTable()
            Dim Ds12 As New DataSet
            Dim str12 As String = "select count(*)as cnt from KycVerify where formno='" & LblFormno.Text & "' and (IsAddrssverified='R' or AddrProof='')"
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), CommandType.Text, str12)
            dt12 = Ds12.Tables(0)
            If (dt12.Rows(0)("cnt") > 0) Then
                result = True
            Else
                result = False
            End If
            Return result
        Catch ex As Exception

        End Try
    End Function
    Private Sub SaveBankproof()
        Dim FlNm As String = ""
        Dim scrname As String = ""
        Dim i As Integer
        Dim Obj As DAL
        Dim Remark As String = ""
        Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Dim bankProof As String
        Dim strextension As String = ""
        Dim BankCode As Integer = 0
        Dim Str As String = ""
        Dim Dt1 As New DataTable
        If DDLAccountType.SelectedValue = "0" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Select Account Type');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Exit Sub
        End If
        If Txtacno.Text = "" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Account No');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Exit Sub
        End If
        If cmbbank.SelectedValue = "0" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Select Bank Name');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
            Exit Sub
        End If

        If (Session("CompID") = "1007") Then
            If (GetuploadformKycPerStatus() = True) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please upload your product purchase registration form then upload KYC detail');location.replace('Home.aspx');", True)
                Exit Sub
            End If
            If (GetuploadformKycVerStatus() = True) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Upload the form before uploading KYC documents.');location.replace('Home.aspx');", True)
                Exit Sub
            End If
            If (GetKycPerStatus() = False) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Kyc option is temporarily off. Please try after some time.!!');location.replace('Home.aspx');", True)
                Exit Sub
            End If
        End If
        

        If (upbank.Enabled = True) Then
            If upbank.HasFile = False Then
                scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 1 mb size only!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
        End If


        If upbank.HasFile Then
            strextension = System.IO.Path.GetExtension(upbank.FileName)
            If (strextension.ToUpper() = ".JPG") Or (strextension.ToUpper() = ".JPEG") Or (strextension.ToUpper() = ".PNG") Then
                Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(upbank.PostedFile.InputStream)
                Dim height As Integer = img.Height
                Dim width As Integer = img.Width
                Dim size As Decimal = Math.Round((CDec(upbank.PostedFile.ContentLength) / CDec(1024)), 1)
                If size > 1024 Then
                    scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 1mb size only!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                Else
                    FlNm = Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(upbank.PostedFile.FileName)
                    upbank.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm)
                    bankProof = "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm
                End If
            Else
                scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
        Else


            bankProof = lblbank.Text

        End If
        Dt1 = New DataTable
        Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        Str = "Select a.IDNo,a.MemFirstName As MemName,a.Panno,a.Acno,a.BAnkid,a.IFscode,a.Fax,a.Branchname,b.BankProof," & _
                             " Case when b.ISbankverified<>'N' then Replace(CONVERT(varchar,b.BankVerifyDate,106),' ','-')" & _
                             " Else '' End as BankProofDate,b.isBankverified,CASE WHEN b.IsBankVerified='Y' THEN 'Verified' " & _
                             " when b.IsBankVerified='R' then 'Rejected' Else 'Not Verified' END AS BankVerf," & _
                            " Case when b.IsBankVerified='R' then b.BankProofRemark else '' end as RejectRemark" & _
                             " From M_MemberMaster as a inner join KycVerify as b On a.Formno=b.Formno where a.Formno='" & LblFormno.Text & "'"
        Dt1 = Obj.GetData(Str)
        If Dt1.Rows.Count > 0 Then

            If Val(Dt1.Rows(0)("BankId")) <> cmbbank.SelectedValue Then
                Remark = Remark & " Bank,"
            End If
            If ClearInject(Dt1.Rows(0)("BranchName")) <> ClearInject(Txtbranch.Text) Then
                Remark = Remark & " BranchName,"
            End If
            If ClearInject(Dt1.Rows(0)("AcNo")) <> ClearInject(Txtacno.Text) Then
                Remark = Remark & " AccountNo,"
            End If
            If ClearInject(Dt1.Rows(0)("IFSCode")) <> ClearInject(Txtcode.Text) Then
                Remark = Remark & " IFSCCode,"
            End If
            If ClearInject(Dt1.Rows(0)("BankProof")) <> ClearInject(bankProof) Then
                Remark = Remark & " BankProof,"
            End If
            If Dt1.Rows(0)("Fax") <> DDLAccountType.SelectedItem.Text Then
                Remark = Remark & " Account Type,"
            End If
        End If
        Dim Dt As DataTable

        If cmbbank.SelectedItem.Text.ToUpper = "OTHERS" Then
            If Txtbank.Text <> "" Then


                Dim Q As String
                Q = "Select * from M_BankMaster where BankName='" & Txtbank.Text.Trim & "' and Activestatus='Y'and RowStatus='Y' "
                Obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                Dt = New DataTable
                Dt = Obj.GetData(Q)
                If Dt.Rows.Count = 0 Then
                    Q = ""

                    Q = "insert into M_BankMaster (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) " & _
                " Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" & Txtbank.Text & "','0','0', " & _
                " '','Y','Add by " & Session("IdNo") & " at " & DateTime.Now.ToString() & "','" & LblFormno.Text & "','" & LblFormno.Text & "','','Y' From M_BankMaster "
                    'cmd = New SqlCommand(Q, _
                    '                     DbConnect.cnnObject)
                    i = Obj.SaveData(Q)
                    If i > 0 Then
                        Q = " select Max(BankCode)as BankCode from M_BankMaster where ActiveStatus='Y' and RowStatus='Y'"
                        Dt1 = New DataTable
                        Dt1 = Obj.GetData(Q)
                        If Dt1.Rows.Count > 0 Then
                            dblBank = Dt1.Rows(0)("BankCode")
                        End If
                        'cmd = New SqlCommand(Q, DbConnect.cnnObject)
                        'dRead = cmd.ExecuteReader
                        'If dRead.Read Then
                        '    dblBank = dRead("BankCode")
                        'End If
                        'dRead.Close()
                    End If
                Else
                    dblBank = Dt.Rows(0)("BankCode")
                End If

            End If
        Else
            dblBank = cmbbank.SelectedValue
        End If

        If Txtbank.Text <> "" Or Trim(Txtcode.Text) <> "" Then


            If cmbbank.SelectedValue = 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Choose Bank Name');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                Exit Sub
            End If

            If Txtbranch.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter Branch Name.');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                Exit Sub
            End If


            If Txtcode.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter IFSC Code.');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                Exit Sub
            End If
        End If



        Dim Qry As String = " Insert Into TempMemberMaster Select *,'Update BankProof - " & Context.Request.UserHostAddress.ToString & "',GetDate(),'U' From M_MemberMaster Where FormNo='" & LblFormno.Text & "'"
        Qry = Qry & "Insert Into TempKycVerify Select *,GetDate(),'" & LblFormno.Text & "' From KycVerify Where FormNo='" & LblFormno.Text & "'"
        Qry = Qry & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
        "(0,'" & LblMemName.Text & "','BankProof','Bank Detail Update','" & Remark & "',Getdate(),'" & LblFormno.Text & "')"
        Dim sql As String = Qry & " Update m_MemberMaster set Acno='" & Txtacno.Text & "',Bankid='" & dblBank & "',IFscode='" & Txtcode.Text.ToUpper & "'," & _
       " Branchname='" & Txtbranch.Text.ToUpper & "',Fax='" & DDLAccountType.SelectedItem.Text & "' where Formno= '" & LblFormno.Text & "'"
        sql = sql & " Update KycVerify Set  BankProof='" & bankProof & "',BankProofDate=Getdate(),BankVerifyDate=Getdate(),IsBankVerified='Y' where Formno= '" & LblFormno.Text & "'"

        Dim j As Integer = Obj.SaveData(sql)

        If j <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert(' Bank Proof Upload successfuly. ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
            FillBankMaster()
            loadImagesBank()
            divBank.Visible = False
            Txtbank.Text = ""


        Else
            scrname = "<SCRIPT language='javascript'>alert(' Bank Proof Upload unsuccessfuly. ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)

            loadImagesBank()
        End If
    End Sub
    Private Sub SaveAddressproof()
        Dim FlNm As String = ""
        Dim FlNm1 As String = ""
        Dim scrname As String = ""
        Dim i As Integer = 0
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If txtaddrs.Text = "" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Address');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Exit Sub
        End If
        If Txtpincode.Text = "" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Pincode');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Exit Sub
        End If
        If txtDistrict.Text = "" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Enter District');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Exit Sub
        End If
        If txtCity.Text = "" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Enter City');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Exit Sub
        End If
        If TxtIdProofNo.Text = "" Then
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Aadharno/Voter Id/Driving Licence no');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Exit Sub
        End If


        If (Session("CompID") = "1007") Then
            If (GetuploadformKycPerStatus() = True) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please upload your product purchase registration form then upload KYC detail');location.replace('home.aspx');", True)
                Exit Sub
            End If
            If (GetuploadformKycVerStatus() = True) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Please Upload the form before uploading KYC documents.');location.replace('home.aspx');", True)
                Exit Sub
            End If
            If (GetKycPerStatus() = False) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Key", "alert('Kyc option is temporarily off. Please try after some time.!!');location.replace('home.aspx');", True)
                Exit Sub
            End If
        End If
        Dim s1 As String = ""
        Dim dt1 As New DataTable
        Dim Condition As String = ""

        If Session("CompId") = 1007 Then
            If DDLAddressProof.SelectedValue = "1" Then
                If TxtIdProofNo.Text <> "" Then
                    s1 = "select Count(AadharNo) as AadharNo from M_Membermaster where AadharNo='" & TxtIdProofNo.Text.Trim & "' AND FORMNO<>'" & LblFormno.Text & "'"
                    obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

                    dt1 = New DataTable
                    dt1 = obj.GetData(s1)
                    'If dt1.Rows(0)("AadharNo") >= 7 Then
                    If dt1.Rows(0)("AadharNo") >= 10000000 Then
                        scrname = "<SCRIPT language='javascript'>alert('Already Registerd by this Aadhar No.');" & "</SCRIPT>"
                        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Login Error", scrname, False)
                        Exit Sub
                    Else
                        Condition = ",AadharNo='" & TxtIdProofNo.Text.Trim & "'"
                    End If
                End If
            End If
        End If
        Dim AdrsProof As String
        Dim BackAdrsProof As String
        Dim strextension2 As String = ""
        Dim strextension1 As String = ""
        Dim dt2 As New DataTable
        Dim str As String = ""
        Dim Remark As String = ""
        If (Fuidentity.Enabled = True) Then
            If Fuidentity.HasFile = False Then
                scrname = "<SCRIPT language='javascript'>alert('Please upload front address proof jpg/jpeg/png/ image of upto 1 mb size only!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
        End If

        If Fuidentity.HasFile Then
            FlNm = ""
            strextension2 = System.IO.Path.GetExtension(Fuidentity.FileName)
            If (strextension2.ToUpper() = ".JPG") Or (strextension2.ToUpper() = ".JPEG") Or (strextension2.ToUpper() = ".PNG") Then
                Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(Fuidentity.PostedFile.InputStream)
                Dim height As Integer = img.Height
                Dim width As Integer = img.Width
                Dim size As Decimal = Math.Round((CDec(Fuidentity.PostedFile.ContentLength) / CDec(1024)), 1)
                If size > 1024 Then
                    scrname = "<SCRIPT language='javascript'>alert('Please upload front address proof jpg/jpeg/png/ image of upto 1mb size only!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                Else
                    FlNm = "F" & Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(Fuidentity.PostedFile.FileName)
                    Fuidentity.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm)
                    AdrsProof = "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm
                End If
            Else
                scrname = "<SCRIPT language='javascript'>alert('You can upload front address proof only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
        Else
            AdrsProof = lblimage.Text
        End If
        If (FileUpload1.Enabled = True) Then
            If FileUpload1.HasFile = False Then
                scrname = "<SCRIPT language='javascript'>alert('Please upload Back address proof jpg/jpeg/png/ image of upto 1 mb size only!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
        End If

        If FileUpload1.HasFile Then
            FlNm1 = ""
            strextension1 = System.IO.Path.GetExtension(FileUpload1.FileName)
            If (strextension1.ToUpper() = ".JPG") Or (strextension1.ToUpper() = ".JPEG") Or (strextension1.ToUpper() = ".PNG") Then
                Dim img1 As System.Drawing.Image = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream)
                Dim height As Integer = img1.Height
                Dim width As Integer = img1.Width
                Dim size As Decimal = Math.Round((CDec(FileUpload1.PostedFile.ContentLength) / CDec(1024)), 1)
                If size > 1024 Then
                    scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 1mb size only!! ');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Exit Sub
                Else
                    FlNm1 = "B" & Format(Now, "yyMMddhhmmssfff") & Session("CompID") & Path.GetExtension(FileUpload1.PostedFile.FileName)
                    FileUpload1.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") & FlNm1)
                    BackAdrsProof = "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & FlNm1
                End If
            Else
                scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
        Else
            BackAdrsProof = LblBackImage.Text
        End If
        str = " Select a.IDNo,a.MemFirstName As MemName,a.Address1,a.City,a.Tehsil,a.District, " & _
                             " a.Statecode,a.Pincode,b.IdproofNo,b.AddrProof,b.BackAddressProof,Case when b.IsAddrssverified<>'N' then " & _
                              " Replace(CONVERT(varchar,b.AddrssVerifyDate,106),' ','-') Else '' End as AddrProofDate, " & _
                              " b.IsAddrssverified,CASE WHEN b.IsAddrssverified='Y' THEN 'Verified' when b.IsAddrssverified='R'  " & _
                              " then 'Rejected' Else 'Not Verified' END AS idVerf,Case when b.IsAddrssverified='R'  " & _
                              " then b.AddrssRemark else'' end as RejectRemark,BackAddressDate,c.IdType,c.id ,a.Aadharno  " & _
                              " From M_MemberMaster  AS A Inner Join KycVerify as b On a.Formno=b.formno  Inner Join  " & _
                              " M_IdTypeMaster as c  On b.IdTYpe=c.Id and C.ActiveStatus='Y'  where a.Formno='" & LblFormno.Text & "'"
        dt1 = obj.GetData(str)
        If dt1.Rows.Count > 0 Then

            If ClearInject(dt1.Rows(0)("Address1")) <> ClearInject(txtaddrs.Text) Then
                Remark = Remark & "Address ,"
            End If
            If ClearInject(dt1.Rows(0)("City")) <> ClearInject(txtCity.Text) Then
                Remark = Remark & " City ,"

            End If
            If ClearInject((dt1.Rows(0)("District"))) <> ClearInject(txtDistrict.Text) Then
                Remark = Remark & " District,"
            End If
            If ClearInject(dt1.Rows(0)("PinCode")) <> ClearInject(Txtpincode.Text) Then
                Remark = Remark & " PinCode,"
            End If
            If ClearInject(dt1.Rows(0)("AddrProof")) <> ClearInject(AdrsProof) Then
                Remark = Remark & " FrontAddressProof,"
            End If
            If ClearInject(dt1.Rows(0)("BackAddressProof")) <> ClearInject(BackAdrsProof) Then
                Remark = Remark & " BackAddressProof,"
            End If
            If ClearInject(dt1.Rows(0)("id")) <> ClearInject(DDLAddressProof.SelectedValue) Then
                Remark = Remark & " AddressProofType,"
            End If
            If Session("Compid") = 1007 Then
                If ClearInject(dt1.Rows(0)("Aadharno")) <> ClearInject(TxtIdProofNo.Text.Trim) Then
                    Remark = Remark & "Aadharno,"
                End If
            Else
                If ClearInject(dt1.Rows(0)("IdProofNo")) <> ClearInject(TxtIdProofNo.Text.Trim) Then
                    Remark = Remark & "AddressProofNo,"
                End If
            End If
        End If
        If DDLAddressProof.SelectedValue = "0" Then
            scrname = "<SCRIPT language='javascript'>alert('Choose ID Proof Type.');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            Exit Sub
        End If
        Dim Qry As String = "Insert Into TempMemberMaster Select *,'Update Address Proof - " & Context.Request.UserHostAddress.ToString & "',GetDate(),'U' From M_MemberMaster Where FormNo='" & LblFormno.Text & "'"
        Qry = Qry & "Insert Into TempKycVerify Select *,GetDate(),'" & LblFormno.Text & "' From KycVerify Where FormNo='" & LblFormno.Text & "'"
        Qry = Qry & " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" & _
        "(0,'" & LblMemName.Text & "','AddressProof Detail','AddressProof Detail Update','" & Remark & "',Getdate(),'" & LblFormno.Text & "')"
        Dim sql As String = Qry & "Update m_MemberMaster set Address1='" & txtaddrs.Text.ToUpper & "'," & _
       " Tehsil='" & txtCity.Text.ToUpper & "',City='" & txtCity.Text.ToUpper & "',District='" & txtDistrict.Text.ToUpper & "',StateCode='" & ddlState.SelectedValue & "'," & _
     " Pincode='" & Txtpincode.Text & "' ," & _
      " CityCode='0',DistrictCode='0'" & Condition & " where Formno= '" & LblFormno.Text & "'"
        sql = sql & "Update KycVerify Set Idtype='" & DDLAddressProof.SelectedValue & "',IdProofNo='" & TxtIdProofNo.Text.Trim.ToUpper & "',AddrProof='" & AdrsProof & "'," & _
        "BackAddressProof='" & BackAdrsProof & "',BackAddressDate=Getdate(),AddrssVerifyDate=Getdate(),IsaddrssVerified='Y' where Formno= '" & LblFormno.Text & "'"
        Dim j As Integer = obj.SaveData(sql)
        If j <> 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Address Proof Upload successfuly. ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
        Else
            scrname = "<SCRIPT language='javascript'>alert('Address Proof Upload unsuccessfuly. ');" & "</SCRIPT>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Image", scrname, False)
        End If
        loadImagesAddress()
    End Sub
    Private Function ClearInject(ByVal StrObj As String) As String
        StrObj = Replace(Replace(Replace(StrObj, ";", ""), "'", ""), "=", "")
        Return StrObj
    End Function
End Class
