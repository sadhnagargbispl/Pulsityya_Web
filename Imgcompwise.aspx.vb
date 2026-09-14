Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class Imgcompwise
    Inherits System.Web.UI.Page
    Public FormNo As String
    Dim dt As DataTable
    Dim obj As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim url As String = String.Empty

        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri

        url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")


        url = url.ToLower



        FormNo = Request("ID")

        Dim sql As String
        obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Request("Type") IsNot Nothing Then
            If Request("Type") = "Blog" Then
                sql = "select case When ImgPath='' then '' else ImgPath" & _
    " end as ImageLnk1 from M_Testimonials  where AId='" & Request("AId") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                End If
            ElseIf Request("Type") = "Payment" Then
                sql = "select CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else MlmUrl + 'images/UploadImage/" & Session("CompId") & "/'+ScannedFile " & _
   " end as ImageLnk1,d.IdNo,d.MemFirstName,a.Amount,a.ChqNo from WalletReq as a,m_companymaster as p with(nolock),m_membermaster  as d with(nolock)  where Reqno='" & Request("ID") & "'and a.Formno=d.Formno"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    lblidno.Text = dt.Rows(0)("idno")
                    lblname.Text = dt.Rows(0)("MemFirstName")
                    LabelAmount.Text = dt.Rows(0)("Amount")
                    LabeTransactionNo.Text = dt.Rows(0)("ChqNo")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250

                    dividno.Visible = False
                    divname.Visible = False

                    divbankname.Visible = False
                    divaccno.Visible = False
                    divbranchname.Visible = False
                    divifsccode.Visible = False

                    divpan.Visible = False

                    divfname.Visible = False
                    addharno.Visible = False
                    lbname.Visible = False
                    lblfname.Visible = False
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False

                    Type.Visible = False
                    lbl4.Visible = False
                    lblidtype.Visible = False
                    lbl5.Visible = False
                    lblcity.Visible = False
                    lbl7.Visible = False
                    lbldistrict.Visible = False
                    lbl6.Visible = False
                    lblstate.Visible = False

                    idproofno.Visible = False
                    divaddress.Visible = False

                    divpincode.Visible = False
                    dividno.Visible = True
                    divname.Visible = True
                End If
            ElseIf Request("Type") = "productPayment" Then
                If Session("CompID") = 1091 Then
                    sql = "select top 1 case When ImageUpload = '' then 'https://cpanel.solfit.in/Images/no_photo.jpg' else 'https://cpanel.solfit.in/images/UploadImage/'+ ImageUpload end as ImageLnk1 "
                    sql &= "from TrnProductorderDetail  where orderno = '" & Request("ID") & "'"
                End If
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250

                    dividno.Visible = False
                    divname.Visible = False

                    divbankname.Visible = False
                    divaccno.Visible = False
                    divbranchname.Visible = False
                    divifsccode.Visible = False

                    divpan.Visible = False

                    divfname.Visible = False
                    addharno.Visible = False
                    lbname.Visible = False
                    lblfname.Visible = False
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False

                    Type.Visible = False
                    lbl4.Visible = False
                    lblidtype.Visible = False
                    lbl5.Visible = False
                    lblcity.Visible = False
                    lbl7.Visible = False
                    lbldistrict.Visible = False
                    lbl6.Visible = False
                    lblstate.Visible = False

                    idproofno.Visible = False
                    divaddress.Visible = False

                    divpincode.Visible = False
                    divAmount.Visible = False
                    divTransactionNo.Visible = False
                End If

            ElseIf Request("Type") = "seminar" Then
                sql = "select case When ScannedFile='' then '" & url & "Images/no_photo.jpg' else '" & url & "/images/UploadImage/'+ ScannedFile" & _
    " end as ImageLnk1 from SeminarReq  where Reqno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250



                End If
            ElseIf Request("Type") = "PinRequest" Then
                sql = "select case When Imgpath='' then '" & url & "Images/no_photo.jpg' else '" & url & "/images/UploadImage/'+ ImgPath" & _
    " end as ImageLnk1 from TrnPinReqMain  where Reqno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250



                End If



            ElseIf Request("Type") = "BackAddress" Then
                '            sql = "select case When BackAddressProof='' then '" & url & "Images/no_photo.jpg' else BackAddressProof" & _
                '" end as ImageLnk1 from KycVerify  where Formno='" & Request("ID") & "'"
                sql = "select A.IdProofNo,b.IDNo,RTRIM(b.MemFirstName) as MemName,B.MemFName,B.AadharNo,B.mobl,B.Address1,B.pincode,b.City,b.Tehsil,b.District,c.Statename,d.idtype,case When BackAddressProof='' then '" & url & "Images/no_photo.jpg' else BackAddressProof" & _
    " end as ImageLnk1 from KycVerify as a inner join M_MemberMaster as b on a.formno=b.formno inner join M_StatedivMaster as c on b.statecode=c.statecode inner join M_IdTypeMaster as d on a.IdType=d.Id where a.Formno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    lblidno.Text = dt.Rows(0)("idno")
                    lblname.Text = dt.Rows(0)("MemName")
                    'lblfname.Text = dt.Rows(0)("memFname")
                    lblidtype.Text = dt.Rows(0)("idtype")
                    lblidproofno.Text = dt.Rows(0)("IdProofNo")

                    lblcity.Text = dt.Rows(0)("City")
                    lbldistrict.Text = dt.Rows(0)("District")
                    lblstate.Text = dt.Rows(0)("statename")
                    lbladdress.Text = dt.Rows(0)("Address1")
                    lblpincode.Text = dt.Rows(0)("pincode")
                    divbankname.Visible = False
                    divaccno.Visible = False
                    divbranchname.Visible = False
                    divifsccode.Visible = False

                    divpan.Visible = False

                    divfname.Visible = False
                    addharno.Visible = False
                    lbname.Visible = False
                    lblfname.Visible = False
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250



                End If

            ElseIf Request("Type") = "FrontAddress" Then
                If Session("CompID") = 1091 Then
                    sql = "select A.idtype,A.IdProofNo,b.IDNo,RTRIM(b.MemFirstName) as MemName,B.MemFName,B.AadharNo,B.mobl,B.Address1,B.pincode,b.City,b.Tehsil,b.District,c.Statename,case When AddrProof='' then '" & url & "Images/no_photo.jpg' else AddrProof" & _
    " end as ImageLnk1 from KycVerify as a inner join M_MemberMaster as b on a.formno=b.formno inner join M_StatedivMaster as c on b.statecode=c.statecode where a.Formno='" & Request("ID") & "'"
                Else
                    sql = "select A.idtype,A.IdProofNo,b.IDNo,RTRIM(b.MemFirstName +' ' +b.MemLastName) as MemName,B.MemFName,B.AadharNo,B.mobl,B.Address1,B.pincode,b.City,b.Tehsil,b.District,c.Statename,case When AddrProof='' then '" & url & "Images/no_photo.jpg' else AddrProof" & _
    " end as ImageLnk1 from KycVerify as a inner join M_MemberMaster as b on a.formno=b.formno inner join M_StatedivMaster as c on b.statecode=c.statecode where a.Formno='" & Request("ID") & "'"

                End If

                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    lblidno.Text = dt.Rows(0)("idno")
                    lblname.Text = dt.Rows(0)("MemName")
                    lblfname.Text = dt.Rows(0)("memFname")
                    'lablaadharno.Text = dt.Rows(0)("AadharNo")
                    lablaadharno.Text = dt.Rows(0)("IdProofNo")

                    divbankname.Visible = False
                    divaccno.Visible = False
                    divbranchname.Visible = False
                    divifsccode.Visible = False

                    divpan.Visible = False


                    idproofno.Visible = False
                    divaddress.Visible = False
                    Type.Visible = False
                    lbl4.Visible = False
                    lblidtype.Visible = False
                    lbl5.Visible = True
                    lblcity.Visible = True
                    lbl7.Visible = True
                    lbldistrict.Visible = True
                    lbl6.Visible = True
                    lblstate.Visible = True

                    lbl8.Visible = True
                    lblpincode.Visible = True

                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250
                End If



            ElseIf Request("Type") = "FrontSideForm" Then
                sql = " select d.idno as Sponsor,RTRIM(d.MemFirstName +' ' +d.MemLastName) As SponsorName ,b.IDNo,RTRIM(b.MemFirstName) as MemName,B.MemFName,B.Address1,B.pincode,B.AadharNo,B.email,B.panno,B.mobl,B.Address1,b.City,b.Tehsil,b.District,c.Statename,case When FrontSideForm='' then '" & url & "Images/no_photo.jpg' else FrontSideForm end as ImageLnk1 " & _
    "  From M_FormUpload as a inner join M_MemberMaster as b on a.formno=b.formno inner join M_StatedivMaster as c on b.statecode=c.statecode inner join M_MemberMaster as d on B.RefFormNo=d.formno where a.Formno='" & Request("ID") & "'"
                '             sql = " select case When FrontSideForm='' then '" & url & "Images/no_photo.jpg' else FrontSideForm end as ImageLnk1 " & _
                '"  From M_FormUpload where Formno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    divsponsor.Visible = True
                    dividno.Visible = True
                    divsponsorname.Visible = True
                    lblsponsoridno.Text = dt.Rows(0)("Sponsor")
                    lblsponsorname.Text = dt.Rows(0)("SponsorName")
                    lblidno.Text = dt.Rows(0)("idno")
                    lblname.Text = dt.Rows(0)("MemName")
                    lblfname.Text = dt.Rows(0)("memFname")
                    divname.Visible = True
                    divbankname.Visible = False
                    divaccno.Visible = False
                    divbranchname.Visible = False
                    divifsccode.Visible = False
                    divmobile.Visible = True
                    lblmobl.Text = dt.Rows(0)("mobl")
                    divemail.Visible = True
                    lblemail.Text = dt.Rows(0)("Email")
                    divpan.Visible = True
                    lblpanno.Text = dt.Rows(0)("panno")
                    divfname.Visible = True
                    Type.Visible = False
                    addharno.Visible = True
                    lablaadharno.Text = dt.Rows(0)("AadharNo")
                    idproofno.Visible = False
                    divcity.Visible = True
                    divdistrict.Visible = True
                    divstate.Visible = True
                    lblcity.Text = dt.Rows(0)("City")
                    lbldistrict.Text = dt.Rows(0)("District")
                    lblstate.Text = dt.Rows(0)("statename")
                    divaddress.Visible = True
                    lbladdress.Text = dt.Rows(0)("Address1")
                    divpincode.Visible = True
                    lblpincode.Text = dt.Rows(0)("pincode")

                    LblNewPic.Visible = False
                    LblPic.Visible = False


                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    divAmount.Visible = False
                    divTransactionNo.Visible = False
                End If

            ElseIf Request("Type") = "BackSideForm" Then
                '        sql = " select case When BackSideForm='' then '" & url & "Images/no_photo.jpg' else BackSideForm end as ImageLnk1 " & _
                '"  From M_FormUpload where Formno='" & Request("ID") & "'"
                sql = " select d.idno as Sponsor,RTRIM(d.MemFirstName +' ' +d.MemLastName) As SponsorName ,b.IDNo,RTRIM(b.MemFirstName) as MemName,B.MemFName,B.Address1,B.pincode,B.AadharNo,B.email,B.panno,B.mobl,B.Address1,b.City,b.Tehsil,b.District,c.Statename,case When BackSideForm='' then '" & url & "Images/no_photo.jpg' else BackSideForm end as ImageLnk1 " & _
        "  From M_FormUpload as a inner join M_MemberMaster as b on a.formno=b.formno inner join M_StatedivMaster as c on b.statecode=c.statecode inner join M_MemberMaster as d on B.RefFormNo=d.formno where a.Formno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    divsponsor.Visible = True
                    dividno.Visible = True
                    divsponsorname.Visible = True
                    lblsponsoridno.Text = dt.Rows(0)("Sponsor")
                    lblsponsorname.Text = dt.Rows(0)("SponsorName")
                    lblidno.Text = dt.Rows(0)("idno")
                    lblname.Text = dt.Rows(0)("MemName")
                    lblfname.Text = dt.Rows(0)("memFname")
                    divname.Visible = True
                    divbankname.Visible = False
                    divaccno.Visible = False
                    divbranchname.Visible = False
                    divifsccode.Visible = False
                    divmobile.Visible = True
                    lblmobl.Text = dt.Rows(0)("mobl")
                    divemail.Visible = True
                    lblemail.Text = dt.Rows(0)("Email")
                    divpan.Visible = True
                    lblpanno.Text = dt.Rows(0)("panno")
                    divfname.Visible = True
                    Type.Visible = False
                    addharno.Visible = True
                    lablaadharno.Text = dt.Rows(0)("AadharNo")
                    idproofno.Visible = False
                    divcity.Visible = True
                    divdistrict.Visible = True
                    divstate.Visible = True
                    lblcity.Text = dt.Rows(0)("City")
                    lbldistrict.Text = dt.Rows(0)("District")
                    lblstate.Text = dt.Rows(0)("statename")
                    divaddress.Visible = True
                    lbladdress.Text = dt.Rows(0)("Address1")
                    divpincode.Visible = True
                    lblpincode.Text = dt.Rows(0)("pincode")

                    LblNewPic.Visible = False
                    LblPic.Visible = False


                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'dividno.Visible = False
                    'divname.Visible = False
                    'divbankname.Visible = False
                    'divaccno.Visible = False
                    'divbranchname.Visible = False
                    'divifsccode.Visible = False
                    'divpan.Visible = False

                    'divfname.Visible = False
                    'type.Visible = False
                    'addharno.Visible = False
                    'idproofno.Visible = False
                    'divcity.Visible = False
                    'divdistrict.Visible = False
                    'divstate.Visible = False
                    'LblNewPic.Visible = False
                    'LblPic.Visible = False
                    'divaddress.Visible = False
                    'divpincode.Visible = False
                    'LblUpdatePic.Visible = False
                    'ImageUpload.Visible = False
                    'Upload.Visible = False
                    'Cancel.Visible = False
                    divAmount.Visible = False
                    divTransactionNo.Visible = False
                End If


            ElseIf Request("Type") = "FrontAddressSpecial" Then
                sql = "select case When AddrProof='' then '" & url & "Images/no_photo.jpg' else AddrProof" & _
    " end as ImageLnk1 from SKycVerify  where Formno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")

                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250
                End If



            ElseIf Request("Type") = "BackAddressSpecial" Then
                sql = "select case When BackAddressProof='' then '" & url & "Images/no_photo.jpg' else BackAddressProof" & _
    " end as ImageLnk1 from SKycVerify  where Formno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250



                End If

            ElseIf Request("Type") = "BankProof" Then
                '        sql = "select case When BankProof='' then '" & url & "/Images/no_photo.jpg' else BankProof" & _
                '" end as ImageLnk1 from KycVerify  where Formno='" & Request("ID") & "'"

                sql = "select b.IDNo,RTRIM(b.MemFirstName) as MemName,b.AcNo,b.Branchname,b.mobl,b.Ifscode,c.Bankname,case When BankProof='' then '" & url & "/Images/no_photo.jpg' else BankProof" & _
        " end as ImageLnk1 from KycVerify as a inner join M_MemberMaster as b on a.formno=b.formno inner join M_BAnkMaster as c On b.Bankid=c.Bankcode where a.Formno='" & Request("ID") & "'"

                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    lblidno.Text = dt.Rows(0)("idno")
                    lblname.Text = dt.Rows(0)("MemName")
                    lblbankname.Text = dt.Rows(0)("Bankname")
                    lblaccno.Text = dt.Rows(0)("AcNo")
                    lblbranchname.Text = dt.Rows(0)("Branchname")
                    lblifsccode.Text = dt.Rows(0)("Ifscode")
                    divpan.Visible = False
                    'dividno.Visible = False
                    'divname.Visible = False
                    divfname.Visible = False
                    Type.Visible = False
                    addharno.Visible = False
                    idproofno.Visible = False
                    divcity.Visible = False
                    divdistrict.Visible = False
                    divstate.Visible = False
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    divaddress.Visible = False
                    divpincode.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250
                    divAmount.Visible = False
                    divTransactionNo.Visible = False

                End If
            ElseIf Request("Type") = "Pancard" Then
                sql = "select b.IDNo,RTRIM(b.MemFirstName) as MemName,b.panno,case When PanImg='' then '" & url & "/Images/no_photo.jpg' else PanImg" & _
        " end as ImageLnk1 from KycVerify as a inner join M_MemberMaster as b on a.formno=b.formno where a.Formno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    lblidno.Text = dt.Rows(0)("idno")
                    lblname.Text = dt.Rows(0)("MemName")
                    lblpanno.Text = dt.Rows(0)("panno")

                    divbankname.Visible = False
                    divaccno.Visible = False
                    divbranchname.Visible = False
                    divifsccode.Visible = False

                    'dividno.Visible = False
                    'divname.Visible = False
                    divfname.Visible = False
                    Type.Visible = False
                    addharno.Visible = False
                    idproofno.Visible = False
                    divcity.Visible = False
                    divdistrict.Visible = False
                    divstate.Visible = False
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    divaddress.Visible = False
                    divpincode.Visible = False
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250

                    divAmount.Visible = False
                    divTransactionNo.Visible = False
                End If
            ElseIf Request("Type") = "GST" Then
                sql = "select case When GstImage='' then '" & url & "/Images/no_photo.jpg' else GstImage" & _
        " end as ImageLnk1 from KycVerify  where Formno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250


                End If
            ElseIf Request("Type") = "Invoice" Then
                sql = "select Invoiceurl as  ImagePath from Invoice where Formno='" & Val(Request("ID")) & "' and Id='" & Val(Request("Reqid")) & "'"
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImagePath")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                End If
            ElseIf Request("Type") = "Booking" Then
                sql = "select case When ScanneFile='' then '" & url & "Images/no_photo.jpg' else '" & url & "/images/UploadImage/'+ ScanneFile" & _
    " end as ImageLnk1 from BookingRequest  where Reqno='" & Request("ID") & "'"
                dt = New DataTable
                obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                dt = obj.GetData(sql)
                If dt.Rows.Count > 0 Then
                    Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                    LblNewPic.Visible = False
                    LblPic.Visible = False
                    LblUpdatePic.Visible = False
                    ImageUpload.Visible = False
                    Upload.Visible = False
                    Cancel.Visible = False
                    'Image1.Height = 250
                    'Image1.Width = 250


                End If
                ' add below 17 Feb 2022
                'ElseIf Session("CompID") = 1055 Or Request("Type") = "Complainproof" Then
            ElseIf Session("CompID") = 1055 Then
                '            sql = "select case When Complainproof='' then '" & url & "Images/no_photo.jpg' else '" & url & "/images/UploadImage/'+ Complainproof" & _
                '" end as ImageLnk1 from M_ComplaintMaster where Formno='" & Request("ID") & "'"
                If Request("Type") = "ComplainProof" Then
                    sql = "select case When Complainproof='' then '" & url & "Images/no_photo.jpg' else  Complainproof" & _
             " end as ImageLnk1 from M_ComplaintMaster where CID='" & Request("ID") & "'"


                    dt = New DataTable
                    obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    dt = obj.GetData(sql)
                    If dt.Rows.Count > 0 Then
                        Image1.ImageUrl = dt.Rows(0)("ImageLnk1")
                        LblNewPic.Visible = False
                        LblPic.Visible = False
                        LblUpdatePic.Visible = False
                        ImageUpload.Visible = False
                        Upload.Visible = False
                        Cancel.Visible = False
                        'Image1.Height = 250
                        'Image1.Width = 250


                    End If
                Else
                    'ElseIf Session("CompID") = 1055 And Request("Type") = "transImg" Then
                    '            sql = "select case When Complainproof='' then '" & url & "Images/no_photo.jpg' else '" & url & "/images/UploadImage/'+ Complainproof" & _
                    '" end as ImageLnk1 from M_ComplaintMaster where Formno='" & Request("ID") & "'"

                    sql = "select case When transImg='' then '" & url & "Images/no_photo.jpg' else  transImg" & _
                 " end as Image from M_FranchiseRegistration where ID='" & Request("ID") & "'"


                    dt = New DataTable
                    obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    dt = obj.GetData(sql)
                    If dt.Rows.Count > 0 Then
                        Image1.ImageUrl = dt.Rows(0)("Image")
                        LblNewPic.Visible = False
                        LblPic.Visible = False
                        LblUpdatePic.Visible = False
                        ImageUpload.Visible = False
                        Upload.Visible = False
                        Cancel.Visible = False
                        'Image1.Height = 250
                        'Image1.Width = 250


                    End If
                End If

                'ElseIf Session("CompID") = 1055 And Request("Type") = "transImg" Then
                '    '            sql = "select case When Complainproof='' then '" & url & "Images/no_photo.jpg' else '" & url & "/images/UploadImage/'+ Complainproof" & _
                '    '" end as ImageLnk1 from M_ComplaintMaster where Formno='" & Request("ID") & "'"

                '    sql = "select case When transImg='' then '" & url & "Images/no_photo.jpg' else  transImg" & _
                ' " end as Image from M_FranchiseRegistration where ID='" & Request("ID") & "'"


                '    dt = New DataTable
                '    obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                '    dt = obj.GetData(sql)
                '    If dt.Rows.Count > 0 Then
                '        Image1.ImageUrl = dt.Rows(0)("Image")
                '        LblNewPic.Visible = False
                '        LblPic.Visible = False
                '        LblUpdatePic.Visible = False
                '        ImageUpload.Visible = False
                '        Upload.Visible = False
                '        Cancel.Visible = False
                '        'Image1.Height = 250
                '        'Image1.Width = 250


                '    End If

            Else

                Image1.ImageUrl = "ImgHandler.ashx?id=" & Request("ID") & "&Type=" & Request("Type")
                LblNewPic.Visible = True
                LblPic.Visible = True
                LblUpdatePic.Visible = True
                ImageUpload.Visible = True

                Upload.Visible = True
                Cancel.Visible = True
            End If
        Else
            Image1.ImageUrl = "ImgHandler.ashx?id=" & Request("ID")
            LblNewPic.Visible = True
            LblPic.Visible = True
            LblUpdatePic.Visible = True
            ImageUpload.Visible = True
            Upload.Visible = True
            Cancel.Visible = True

        End If
    End Sub
    Protected Sub Upload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Upload.Click
        Dim type As String
        type = Request("Type")
        Try
            If ImageUpload.PostedFile IsNot Nothing AndAlso ImageUpload.PostedFile.FileName <> "" Then
                Dim strExtension As String = System.IO.Path.GetExtension(ImageUpload.FileName)
                If (strExtension.ToUpper() = ".JPG") Or (strExtension.ToUpper() = ".GIF") Then
                    ' Resize Image Before Uploading to DataBase
                    Dim imageToBeResized As System.Drawing.Image = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream)
                    Dim imageHeight As Integer = imageToBeResized.Height
                    Dim imageWidth As Integer = imageToBeResized.Width
                    Dim maxHeight As Integer = 200
                    Dim maxWidth As Integer = 200
                    imageHeight = (imageHeight * maxWidth) / imageWidth
                    imageWidth = maxWidth
                    If imageHeight > maxHeight Then
                        imageWidth = (imageWidth * maxHeight) / imageHeight
                        imageHeight = maxHeight
                    End If
                    Dim bitmap As New Drawing.Bitmap(imageToBeResized, imageWidth, imageHeight)
                    Dim stream As System.IO.MemoryStream = New MemoryStream()
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg)
                    stream.Position = 0
                    Dim image As Byte() = New Byte(stream.Length) {}
                    stream.Read(image, 0, image.Length)
                    'Create SQL Connection 
                    Dim con As New SqlConnection()
                    con.ConnectionString = (HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
                    con.Open()
                    ' Create SQL Command 
                    Dim cmd As New SqlCommand()
                    Dim sql As String
                    If type = "Address" Then
                        sql = "Update M_MemberMaster set AddrssProof = @Image,DtAddrssProof=GetDate() where FormNo= '" & FormNo & "'"
                    ElseIf type = "Identity" Then
                        sql = "Update M_MemberMaster set IdentityProof = @Image where FormNo= '" & FormNo & "'"
                    Else
                        sql = "Update M_MemberMaster set memPic = @Image where FormNo= '" & FormNo & "'"
                    End If

                    cmd.CommandText = sql
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    Dim UploadedImage As New SqlParameter("@Image", SqlDbType.Image, image.Length)
                    UploadedImage.Value = image
                    cmd.Parameters.Add(UploadedImage)
                    'con.Open()
                    Dim result As Integer = cmd.ExecuteNonQuery()
                    con.Close()
                    If result > 0 Then
                        'DistImage.ImageUrl = "ImgHandler.ashx?id=" & Session("Formno") & "&type=Photo"
                    End If
                End If
            Else
                'DistImage.ImageUrl = "ImgHandler.ashx?id=" & Session("Formno") & "&type=Photo"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        LblPic.Visible = False
        ImageUpload.Visible = False
        LblNewPic.Visible = False
        Upload.Visible = False
        LblUpdatePic.Visible = True
        ''LnkPhoto.Visible = True
        Cancel.Visible = False
    End Sub

    'Protected Sub LnkPhoto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkPhoto.Click
    '    tblImage.Visible = True
    '    Image1.Visible = False
    '    'LnkPhoto.Visible = False
    '    ImageUpload.Visible = True
    '    Upload.Visible = True
    '    Cancel.Visible = True
    'End Sub

    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.Click
        ImageUpload.Visible = False
        Upload.Visible = False
        'LnkPhoto.Visible = True
        Cancel.Visible = False
        'tblImage.Visible = False
    End Sub

End Class
