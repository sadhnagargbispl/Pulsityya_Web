
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Imports System.Web.HttpApplicationState
Imports System.IO

Imports System.IO.MemoryStream
Imports System.Drawing
Imports System.Globalization


Public Class DAL
    Public objSQlConnection As SqlConnection

    Public objSQlConnection1 As SqlConnection

    Public sda As New SqlDataAdapter
    Public dt As New DataTable
    Public request As HttpRequest
    Public AppUrl As String = ""
    Public Connection As New SqlClient.SqlConnectionStringBuilder
    Dim sqlCmd As New SqlCommand
    Public activeCondition As String = "RowStatus='Y'"
    Public tblUserGrpMaster As String = "M_UserGroupMaster"
    Public tblUserMaster As String = "M_UserMaster"
    Public tblStateMaster As String = "M_StateDivMaster"
    Public tblDistrictMaster As String = "M_DistrictMaster"
    Public tblCityStateMaster As String = "M_CityStateMaster"
    Public tblBankMaster As String = "M_BankMaster"
    Public tblNewsMaster As String = "M_NewsSeminarMaster"
    Public tblNewsTypeMaster As String = "M_NewsTypeMaster"
    Public tblCountryMaster As String = "M_CountryMaster"
    Public tblKitMaster As String = "M_KitMaster"
    Public tblFundMaster As String = "M_FundMaster "
    Public tblAchieverMaster As String = "M_AchieverMaster"
    Public tblMeetingMaster As String = "M_MeetingMaster"
    Public tblUserPermision As String = "M_UserPermissionMaster"
    Public tblMenuMaster As String = "M_WebMenuMaster"
    Public tblSearchCriteria As String = "M_SearchCriteriaMaster"
    Public tblMemberMaster As String = "M_MemberMaster"
    Public tblKitProductMaster As String = "M_KitProductMaster"
    Public tblCTypeMaster As String = "M_ComplaintTypeMaster"
    Public tbldispatchMaster As String = "M_ProductDispatchmaster"
    Private _ConnectionString As String
    'Public Sub New()
    '    'Setting Connection String
    '    'Connection.ConnectionString = "Server=164.132.18.172;UID=ubgshp;PWD=S#09B!81ee$;Database=BigShopee;Pooling=False;Connect Timeout=200000000"

    '    ' Connection.ConnectionString = "Server=111.118.190.137;UID=ubgshp;PWD=S#09B!81ee$;Database=BigShopee;Pooling=False;Connect Timeout=200000000"
    '    Connection.ConnectionString = ConfigurationManager.ConnectionStrings("sconstr").ConnectionString
    '    objSQlConnection1 = New SqlClient.SqlConnection(Connection.ConnectionString)

    '    Dim ds As DataSet = New DataSet
    '    Dim str As String = " Exec Proc_GetConnection 1003 "
    '    ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.Text, str)

    '    objSQlConnection = New SqlClient.SqlConnection(ds.Tables(0).Rows(0)("ConnectionString").ToString())



    '    'Creating request object and setting it up
    '    'request = HttpContext.Current.Request
    '    'AppUrl = AppUrl + request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath
    'End Sub



    Public Sub New(ByVal strConnectionString As String)
        '        _SerucityCode = strSecurityCode
        _ConnectionString = strConnectionString
        objSQlConnection = New SqlClient.SqlConnection(_ConnectionString)
    End Sub

    Public Function SaveData(ByVal qry As String) As Integer
        Try

       
            'objSQlConnection.Open()
            If objSQlConnection.State = ConnectionState.Closed Then
                objSQlConnection.Open()
            End If
            sqlCmd = New SqlCommand(qry, objSQlConnection)
            Dim a As Integer = sqlCmd.ExecuteNonQuery()
            'objSQlConnection.Close()
            If objSQlConnection.State = ConnectionState.Open Then
                objSQlConnection.Close()
            End If
            Return a
        Catch ex As Exception
            If Not objSQlConnection Is Nothing Then
                If objSQlConnection.State = ConnectionState.Open Then
                    objSQlConnection.Close()
                End If
            End If
        End Try

    End Function

    Public Function SaveData1(ByVal qry As String) As Integer
        Try


            'objSQlConnection.Open()
            If objSQlConnection.State = ConnectionState.Closed Then
                objSQlConnection.Open()
            End If
            sqlCmd = New SqlCommand(qry, objSQlConnection)
            sqlCmd.CommandTimeout = 0
            Dim a As Integer = sqlCmd.ExecuteNonQuery()
            'objSQlConnection.Close()
            If objSQlConnection.State = ConnectionState.Open Then
                objSQlConnection.Close()
            End If
            Return a
        Catch ex As Exception
            If Not objSQlConnection Is Nothing Then
                If objSQlConnection.State = ConnectionState.Open Then
                    objSQlConnection.Close()
                End If
            End If
        End Try
    End Function

    Public Function GetData(ByVal qry As String) As DataTable
        Dim tempDt As DataTable
        Try


            If objSQlConnection.State = ConnectionState.Closed Then
                objSQlConnection.Open()
            End If
            sda = New SqlDataAdapter(qry, objSQlConnection)
            dt = New DataTable
            sda.Fill(dt)
            tempDt = dt

        Catch ex As Exception
            If Not objSQlConnection Is Nothing Then
                If objSQlConnection.State = ConnectionState.Open Then
                    objSQlConnection.Close()
                End If
            End If
        End Try
        Return tempDt
    End Function

    Public Function UpdateData(ByVal qry As String, Optional ByVal ParaName As String = "", Optional ByVal ParaValue As String = "") As Integer
        'objSQlConnection.Open()
        Dim j As Integer = 0
        Try

       
            If objSQlConnection.State = ConnectionState.Closed Then
                objSQlConnection.Open()
            End If
            Dim strParaName(), strParaValue() As String
            Dim i As Integer = 0

            If String.IsNullOrEmpty(ParaName) = True And String.IsNullOrEmpty(ParaValue) = True Then
                sqlCmd = New SqlCommand(qry, objSQlConnection)
            Else
                strParaName = ParaName.Split(";")
                strParaValue = ParaValue.Split(";")
                sqlCmd = New SqlCommand(qry, objSQlConnection)
                For i = 0 To strParaName.Count - 1
                    sqlCmd.Parameters.AddWithValue(strParaName(i), strParaValue(i))
                Next
            End If
            Dim a As Integer = sqlCmd.ExecuteNonQuery()
            'objSQlConnection.Close()
            If objSQlConnection.State = ConnectionState.Open Then
                objSQlConnection.Close()
            End If
            j = a
        Catch ex As Exception
            If Not objSQlConnection Is Nothing Then
                If objSQlConnection.State = ConnectionState.Open Then
                    objSQlConnection.Close()
                End If
            End If
        End Try
        Return j
    End Function

    Public Function ExecuteProcedure(ByVal procname As String, Optional ByVal ParaName As String = "", Optional ByVal ParaValue As String = "") As Integer
        Dim j As Integer = 0
        Try


            Dim cmd As SqlCommand = objSQlConnection.CreateCommand
            Dim strParaName(), strParaValue() As String
            If objSQlConnection.State = ConnectionState.Closed Then
                objSQlConnection.Open()
            End If
            'objSQlConnection.Open()
            cmd.CommandType = CommandType.StoredProcedure
            If String.IsNullOrEmpty(ParaName) = False And String.IsNullOrEmpty(ParaValue) = False Then
                strParaName = ParaName.Split(";")
                strParaValue = ParaValue.Split(";")
                For i = 0 To strParaName.Count - 1
                    cmd.Parameters.AddWithValue(strParaName(i), strParaValue(i))
                Next
            End If
            cmd.CommandText = procname
            Dim a As Integer = cmd.ExecuteNonQuery()
            'objSQlConnection.Close()
            If objSQlConnection.State = ConnectionState.Open Then
                objSQlConnection.Close()
            End If
            j = a

        Catch ex As Exception
            If Not objSQlConnection Is Nothing Then
                If objSQlConnection.State = ConnectionState.Open Then
                    objSQlConnection.Close()
                End If
            End If
        End Try
        Return j
    End Function

    Public Function GenerateTreeProc(ByVal strqry As String) As DataTable
        'conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'conn.Open()
        Dim tempDt As DataTable
        Try

        
            If objSQlConnection.State = ConnectionState.Closed Then
                objSQlConnection.Open()
            End If
            Dim comm As New SqlCommand(strqry, objSQlConnection)
            comm.CommandTimeout = 100000000
            Dim sda As New SqlDataAdapter(comm)
            dt = New DataTable
            sda.Fill(dt)
            If objSQlConnection.State = ConnectionState.Open Then
                objSQlConnection.Close()
            End If
            tempDt = dt

        Catch ex As Exception
            If Not objSQlConnection Is Nothing Then
                If objSQlConnection.State = ConnectionState.Open Then
                    objSQlConnection.Close()
                End If
            End If
        End Try
        Return tempDt
        'Comm = New SqlCommand(strQuery, conn)
        'Comm.CommandTimeout = 100000000
        'Adp1 = New SqlDataAdapter(Comm)
        'Adp1.Fill(dsGetQry)
    End Function

    Public Function ExecProcDataSet(ByVal strqry As String) As DataSet
        Dim dsGetData As New DataSet
        Try



            If objSQlConnection.State = ConnectionState.Closed Then
                objSQlConnection.Open()
            End If
            Dim comm As New SqlCommand(strqry, objSQlConnection)
            comm.CommandTimeout = 100000000
            Dim sda As New SqlDataAdapter(comm)
            'dt = New DataTable
            sda.Fill(dsGetData)
            If objSQlConnection.State = ConnectionState.Open Then
                objSQlConnection.Close()
            End If

        Catch ex As Exception
            If Not objSQlConnection Is Nothing Then
                If objSQlConnection.State = ConnectionState.Open Then
                    objSQlConnection.Close()
                End If
            End If
        End Try
        Return dsGetData
        'Comm = New SqlCommand(strQuery, conn)
        'Comm.CommandTimeout = 100000000
        'Adp1 = New SqlDataAdapter(Comm)
        'Adp1.Fill(dsGetQry)
    End Function

    Public Function UpdateMultipleFields(ByVal tblName As String, Optional ByVal ParaName As String = "", Optional ByVal ParaValue As String = "", Optional ByVal whereCond As String = "") As Integer
        'objSQlConnection.Open()
        Dim j As Integer = 0
        Try

        
            Dim SubPart As String = ""
            If objSQlConnection.State = ConnectionState.Closed Then
                objSQlConnection.Open()
            End If
            Dim strParaName(), strParaValue() As String
            Dim i As Integer = 0

            If String.IsNullOrEmpty(ParaName) = True And String.IsNullOrEmpty(ParaValue) = True Then
            Else
                strParaName = ParaName.Split(";")
                strParaValue = ParaValue.Split(";")
                For i = 0 To strParaName.Count - 1
                    SubPart = SubPart & strParaName(i) & "='" & strParaValue(i) & "',"
                Next
            End If
            SubPart = SubPart.Remove(SubPart.Length - 1, 1)

            Dim qry As String = " update " & tblName & " set " & SubPart & whereCond
            sqlCmd = New SqlCommand(qry, objSQlConnection)
            Dim a As Integer = sqlCmd.ExecuteNonQuery()
            ' Dim a As Integer = sqlCmd.ExecuteNonQuery()
            'objSQlConnection.Close()
            If objSQlConnection.State = ConnectionState.Open Then
                objSQlConnection.Close()
            End If
            j = a
        Catch ex As Exception
            If Not objSQlConnection Is Nothing Then
                If objSQlConnection.State = ConnectionState.Open Then
                    objSQlConnection.Close()
                End If
            End If
        End Try
        Return j
    End Function
    Public Function ClearInject(ByVal StrObj As String) As String
        Try
            StrObj = Replace(Replace(Replace(StrObj, ";", ""), "'", ""), "=", "")
        Catch ex As Exception

        End Try
        Return StrObj
    End Function

    Public Sub FillCombo(ByVal qry As String, ByRef CmbNm As DropDownList, ByVal DisFld As String, ByVal ValFld As String)
        Try

       
            dt = New DataTable
            dt = GetData(qry)
            With CmbNm
                .DataSource = dt
                .DataTextField = DisFld
                .DataValueField = ValFld
                .DataBind()
            End With
        Catch ex As Exception

        End Try
    End Sub
    Public Sub WriteToFile(ByVal text As String)
        Try

            Dim path As String = HttpContext.Current.Server.MapPath("~/images/ErrorLog.txt")
            Using writer As New StreamWriter(path, True)
                writer.WriteLine(text)
                writer.WriteLine("--------------------------------------------------------")
                writer.Close()
            End Using
        Catch ex As Exception

        End Try

    End Sub
End Class

