Imports System.Web
Imports System.IO
Imports System.Data
Imports System.Data.Odbc
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Collections

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Service
    Inherits System.Web.Services.WebService
    Public USerIp As String
    Dim SecurityCode As String = "ax001425^%$"
    Dim SecCodeNew As String = "px005625^%$"
    Dim ExeFile As String = "DreamTouch.exe"
    Dim PdbFile As String = "DreamTouch.pdb"
    Dim Duid As String = "dreamth"
    Dim Dpwd As String = "tch@2#dre@m1"

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function
    <WebMethod()> Public Function FireQuery(ByVal Query As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String) As Boolean
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        Dim adp As OdbcDataAdapter
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";")
        cnn.Open()
        Dim cmd As OdbcCommand = New OdbcCommand(Query, cnn)
        cmd.ExecuteNonQuery()
        Return True
    End Function
    <WebMethod()> _
    Public Function RetrData(ByRef ds As DataSet, ByVal TbName As String, ByVal Query As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String)
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        Dim adp As OdbcDataAdapter
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";")
        cnn.Open()
        Dim cmd As OdbcCommand = New OdbcCommand(Query, cnn)
        adp = New OdbcDataAdapter(cmd)
        adp.Fill(ds, TbName)
    End Function
    <WebMethod()> _
    Public Function SaveData(ByVal ds As DataSet, ByVal TbName As String, ByVal Query As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String, ByRef ErrMsg As String)
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        Dim adp As New OdbcDataAdapter
        ErrMsg = ""
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";")
        cnn.Open()
        Try
            Dim Cmm As New OdbcCommand
            Cmm = New OdbcCommand(Query, cnn)
            adp = New OdbcDataAdapter(Cmm)
            Dim cmd As New OdbcCommandBuilder(adp)
            adp.InsertCommand = cmd.GetInsertCommand
            adp.UpdateCommand = cmd.GetUpdateCommand
            adp.Update(ds.Tables(TbName))
        Catch ex As Exception
            ErrMsg = ex.Message
        End Try
    End Function
    <WebMethod()> _
    Public Function FillData(ByVal Query As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String, ByVal Err As String)
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        Dim adp As OdbcDataAdapter
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";")
        cnn.Open()
        Try
            Dim Cmm As New OdbcCommand
            Cmm = New OdbcCommand(Query, cnn)
        Catch ex As Exception

        End Try
        'Dim cmd As OdbcCommand = New OdbcCommand(Query, cnn)
        adp = New OdbcDataAdapter(Query, cnn)
        Err = Query
        '  adp.Fill(Ds, TblName)
    End Function
    '**************************************************************************************************
    <WebMethod()> _
        Public Function GetData(ByVal Query As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String) As String
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        Dim dr As OdbcDataReader
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";")
        cnn.Open()
        Dim cmd As OdbcCommand = New OdbcCommand(Query, cnn)
        dr = cmd.ExecuteReader
        If dr.Read = False Then
            GetData = ""
        Else
            GetData = dr(0)
        End If
        dr.Close()
        cnn.Close()
    End Function

    <WebMethod()> _
        Public Function GetRow(ByVal Query As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String) As String
        Dim dr As OdbcDataReader
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        Dim i, j As Integer
        i = 1
        j = 0
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";Timeout=1000000")
        cnn.Open()
        Dim cmd As OdbcCommand = New OdbcCommand(Query, cnn)
        cmd.CommandTimeout = 300000
        dr = cmd.ExecuteReader
        While dr.Read
            If j = 0 Then
                For i = 0 To dr.FieldCount - 1
                    GetRow = GetRow & dr.GetName(i) & ";"
                Next i
                GetRow = GetRow & "|"
            End If
            If j = 0 Then
                For i = 0 To dr.FieldCount - 1
                    GetRow = GetRow & dr.GetDataTypeName(i) & ";"
                Next i
                GetRow = GetRow & "|"
            End If
            j = j + 1

            For i = 0 To dr.FieldCount - 1
                GetRow = GetRow & dr(i) & ";"
            Next
            GetRow = GetRow & "|"
        End While
        dr.Close()
    End Function
    <WebMethod()> _
    Public Function Login(ByVal CardNo As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String, ByVal VersionNo As String) As String
        Dim ip As String
        ip = Context.Request.UserHostAddress.ToString()
        Dim dr As OdbcDataReader
        Dim i, j As Integer
        i = 1
        j = 0
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";")
        cnn.Open()
        Dim cmd As OdbcCommand = New OdbcCommand("Select ActiveStatus from M_CardIssue where Cardno='" & CardNo & "'", cnn)
        dr = cmd.ExecuteReader
        If dr.Read = False Then
            Login = "Invalid Card."
        Else
            If dr(0) = "Y" Then
                Login = "OK"
            Else
                Login = "This Card Has Been Blocked. Please Contact SOBM H/O."
            End If
        End If
        dr.Close()
        cmd = New OdbcCommand("Update M_CardIssue Set LastIp='" & ip & "',LastLoginTime=GetDate(),Version='" & VersionNo & "' where CardNo='" & CardNo & "'", cnn)
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Login = "Login Failed. Try Again."
        End Try
        cnn.Close()
    End Function
    <WebMethod()> Public Function LoginWithoutCard(ByVal CodeNo As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String, ByVal VersionNo As String) As String
        Dim ip As String
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        ip = Context.Request.UserHostAddress.ToString()
        Dim dr As OdbcDataReader
        Dim i, j As Integer
        i = 1
        j = 0
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";")
        cnn.Open()
        Dim cmd As OdbcCommand = New OdbcCommand("Select Status from M_UserMaster where FCode='" & CodeNo & "'", cnn)
        dr = cmd.ExecuteReader
        If dr.Read = False Then
            LoginWithoutCard = "Invalid Card."
        Else
            If dr(0) = "Y" Then
                LoginWithoutCard = "OK"
            Else
                LoginWithoutCard = "This Card Has Been Blocked. Please Contact H/O."
            End If
        End If
        dr.Close()
        cmd = New OdbcCommand("Update M_UserMaster Set LastIp='" & ip & "',LastLoginTime=GetDate(),Version='" & VersionNo & "' where FCode='" & CodeNo & "'", cnn)
        'MsgBox(cmd.CommandText)
        'LoginWithoutCard = cmd.CommandText
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            LoginWithoutCard = "Login Failed. Try Again."
        End Try
        cnn.Close()
    End Function
    <WebMethod()> _
        Public Function GetLoginStatus(ByVal CardNo As String, ByVal Dsn As String, ByVal UID As String, ByVal Pwd As String) As String
        Dim ip As String
        If Trim(UID) = "" Or Trim(Pwd) = "" Then '21Oct16
            UID = Duid : Pwd = Dpwd
        End If
        ip = Context.Request.UserHostAddress.ToString()
        Dim dr As OdbcDataReader
        Dim i, j As Integer
        i = 1
        j = 0
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & Dsn & ";UID=" & UID & ";PWD=" & Pwd & ";")
        cnn.Open()
        Dim cmd As OdbcCommand = New OdbcCommand("Select LastIP from M_CardIssue where Cardno='" & CardNo & "' and ActiveStatus='Y'", cnn)
        dr = cmd.ExecuteReader
        If dr.Read = False Then
            GetLoginStatus = "Invalid Card."
        Else
            If dr(0) = ip Then
                GetLoginStatus = "OK"
            Else
                GetLoginStatus = "This Card has using at another place. Please check."
            End If
        End If
        dr.Close()
        cnn.Close()
    End Function
    <WebMethod()> _
    Public Function DownloadFile(ByRef FileName As String, ByVal SecCode As String) As Byte()
        If SecCode = SecCodeNew Then
            Dim fs1 As System.IO.FileStream
            fs1 = System.IO.File.Open(Server.MapPath("downloads\" & FileName), IO.FileMode.Open, IO.FileAccess.Read)
            Dim b1() As Byte
            ReDim Preserve b1(fs1.Length)
            fs1.Read(b1, 0, fs1.Length)
            fs1.Close()
            Return b1
        End If
    End Function

    <WebMethod()> _
    Public Function SetAuthentication(ByVal SecCode As String) As String
        If SecCode <> SecurityCode Then
            SetAuthentication = "False"
        Else
            SetAuthentication = "True"
            SetAuthentication = SecCodeNew
        End If
    End Function

    <WebMethod()> _
Public Function CheckValidCard(ByVal CardNo As String, ByVal Pwd As String, ByVal DsnName As String, ByVal UId As String, ByVal Passw As String, ByVal MyMsg As String) As Boolean
        If Trim(UId) = "" Or Trim(Pwd) = "" Then '21Oct16
            UId = Duid : Pwd = Dpwd
        End If
        Dim dr As OdbcDataReader
        Dim MyCmd As New OdbcCommand
        Dim Cmd As String
        CheckValidCard = False
        Dim cnn As OdbcConnection = New OdbcConnection("DSN=" & DsnName & ";UID=" & UId & ";PWD=" & Passw & ";")
        cnn.Open()
        MyCmd = New OdbcCommand("Select CardNo,Password from M_CardIssue where Cardno='" & CardNo & "' and Password='" & Pwd & "' and ActiveStatus='Y'", cnn)
        dr = MyCmd.ExecuteReader
        If dr.Read = False Then
            CheckValidCard = False
            MyMsg = "Invalid Card..."
        Else
            CheckValidCard = True
            MyMsg = "OK"
        End If
        'If CheckValidCard = True Then
        '    Cmd = "Exec GetBalance(" & CardNo & ")"
        '    MyCmd = New OdbcCommand(Cmd, cnn)
        '    Bal = MyCmd.ExecuteNonQuery()
        'End If
        dr.Close()
        cnn.Close()
    End Function

    <WebMethod()> _
   Public Function GetFileList(ByVal SecCode As String) As String
        If SecCode = SecCodeNew Then
            GetFileList = GetFileList & ExeFile & ";"
            GetFileList = GetFileList & PdbFile & ";"
        Else
            GetFileList = "False"
        End If
    End Function

    <WebMethod()> _
 Public Function NextExeFile(ByVal SecCode As String) As String
        If SecCode = SecCodeNew Then
            NextExeFile = NextExeFile & ExeFile
        Else
            NextExeFile = "False"
        End If
    End Function


    <WebMethod()> _
        Public Function UploadFile(ByVal FileData As Byte(), ByVal FileName As String, ByVal SecCode As String) As Boolean
        UploadFile = False
        Try
            If SecCode = SecCodeNew Then
                Dim fs1 As System.IO.FileStream
                fs1 = System.IO.File.Open("D:\onlineshop\ProdImg\" & FileName, IO.FileMode.Create, IO.FileAccess.Write)
                fs1.Write(FileData, 0, FileData.Length)
                fs1.Close()
                UploadFile = True
            End If
        Catch ex As Exception
            UploadFile = False
            Exit Function
        End Try
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function SetValid(ByVal Sno As Long, ByVal PCode As String) As Boolean
        On Error GoTo lblError
        SetValid = False
        If Sno <= 0 Then
            SetValid = False
            Application.Lock()
            Application(PCode) = Sno
            Application.UnLock()
            Exit Function
        End If
        Application.Lock()
        Application(PCode) = Sno
        Application(PCode & "ip") = Context.Request.UserHostAddress
        Application.UnLock()
        SetValid = True
        Exit Function
lblError:
        Application.Lock()
        Application(PCode) = Sno
        Application.UnLock()
        SetValid = False
    End Function

    '<WebMethod(EnableSession:=True)> _
    'Public Function GetValid(ByVal Sno As String, ByVal PCode As String, ByVal Location As String) As Boolean
    '    GetValid = False
    '    If Application(PCode) <> Sno Or Sno <= 0 Then
    '        Dim xmlstr As New MSXML.XMLHTTPRequest
    '        xmlstr.open("GET", "http://api.hostip.info/get_html.php?ip=" & Application(PCode & "ip"), False)
    '        xmlstr.send()
    '        Location = xmlstr.responseText
    '        GetValid = False
    '    Else
    '        GetValid = True
    '    End If
    'End Function
    <WebMethod()> _
    Public Function CheckConn() As String
        CheckConn = "OK"
    End Function

    <WebMethod()> _
    Public Function GetIp() As String
        USerIp = Context.Request.UserHostAddress.ToString
        GetIp = USerIp
    End Function

    <WebMethod()> _
      Public Function Upload_File(ByVal f As Byte(), ByVal fileName As String) As String
        ' the byte array argument contains the content of the file
        ' the string argument contains the name and extension
        ' of the file passed in the byte array
        Try
            Dim ms As New MemoryStream(f)
            Dim fs As New FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/TransientStorage/") & fileName, FileMode.Create)

            ms.WriteTo(fs)

            ' clean up
            ms.Close()
            fs.Close()
            fs.Dispose()

            ' return OK if we made it this far
            Return "OK"
        Catch ex As Exception
            ' return the error message if the operation fails
            Return ex.Message.ToString()
        End Try
    End Function

End Class
