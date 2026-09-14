Imports System.Data
Imports System.Data.SqlClient
Public Class cls_DataAccess

    Public cnnObject As SqlConnection
    Public _SerucityCode As String = ""
    Private cmd As SqlCommand
    Private tran As SqlTransaction
    Private _ConnectionString As String

    Public Event ConnectionOpen()


    Public Sub New(ByVal strConnectionString As String)
        '        _SerucityCode = strSecurityCode
        _ConnectionString = strConnectionString
    End Sub
    Public Function ClearInject(ByVal StrObj As String) As String
        StrObj = Replace(Replace(Replace(StrObj, ";", ""), "'", ""), "=", "")
        Return StrObj
    End Function
    Public Function OpenConnection() As SqlConnection
        Try
            If (cnnObject Is Nothing) Then
                cnnObject = New SqlConnection(_ConnectionString)
            End If
            If (cnnObject.State = ConnectionState.Closed Or _
                cnnObject.State = ConnectionState.Broken) Then
                cnnObject.Open()
                RaiseEvent ConnectionOpen()
            End If
            Return cnnObject
        Catch e As Exception
            RaiseEvent ConnectionOpen()
            Return Nothing
        End Try
    End Function
    'Public Function closeConnection() As Boolean
    '    Dim bool As Boolean = False
    '    Try
    '        If Not cnnObject Is Nothing Then
    '            If cnnObject.State = ConnectionState.Open Then
    '                cnnObject.Close()
    '            End If
    '        End If
    '        bool = True
    '    Catch ex As Exception

    '    End Try
    '    Return bool
    'End Function


    Public Function closeConnection()
        Try
            If cnnObject.State = ConnectionState.Open Then
                cnnObject.Close()
            End If
        Catch ex As Exception

        End Try


    End Function


    Public Function ExecuteScaller_old(ByVal strQuery As String) As String

        If cnnObject Is Nothing Or cnnObject.State = ConnectionState.Closed Then
            OpenConnection()
        End If

        Dim cmd As New SqlCommand
        Try
            cmd.Connection = cnnObject
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strQuery

            Return cmd.ExecuteScalar().ToString()
            '  Return cmd.ExecuteScalar().ToString
        Catch e As Exception
            'MsgBox(e.Message)
            Return ""
        End Try
    End Function


    Public Function ExistOrNot(ByVal strQuery As String) As String

        Dim _returnValue As String = ""

        If cnnObject Is Nothing Or cnnObject.State = ConnectionState.Closed Then
            OpenConnection()
        End If

        Dim da As New SqlDataAdapter(strQuery, cnnObject)
        Dim DTable As New DataTable

        Try
            da.Fill(DTable)

            If DTable.Rows.Count > 0 Then
                _returnValue = DTable.Rows(0)(0)
            End If
        Catch e As Exception
            'MsgBox(e.Message & "error in exits")
        End Try
        da.Dispose()
        DTable = Nothing
        Return _returnValue
    End Function


    Public Function Fill_Data_Tables(ByVal strQuery As String, ByRef DTable As DataTable) As DataTable

        If cnnObject Is Nothing Then
            OpenConnection()
        End If

        Dim da As New SqlDataAdapter(strQuery, cnnObject)
        DTable = New DataTable
        Try
            ' da.SelectCommand.Transaction = tran
            da.Fill(DTable)
            ' tran.Commit()

        Catch e As Exception
            ''MsgBox(e.Message)
            Fill_Data_Tables = Nothing
            Exit Function
        End Try
        da.Dispose()
        Fill_Data_Tables = DTable

    End Function

    Public Function Fill_Data_Tables_new(ByRef tran As SqlTransaction, ByVal strQuery As String, ByRef DTable As DataTable) As DataTable

        If cnnObject Is Nothing Or cnnObject.State = ConnectionState.Closed Then
            OpenConnection()
        End If

        Dim da As New SqlDataAdapter(strQuery, cnnObject)
        DTable = New DataTable

        Try
            da.Fill(DTable)
        Catch e As Exception
            'MsgBox(e.Message & " Error in Filling Data " & strQuery.ToString)
            Fill_Data_Tables_new = Nothing
            Exit Function
        End Try
        Fill_Data_Tables_new = DTable
        da.Dispose()
    End Function
    Public Sub Fill_DataSET_Tables(ByVal strQuery As String, ByRef DS As DataSet, ByVal strTableName As String)

        If cnnObject Is Nothing Then
            OpenConnection()
        End If


        Dim da As New SqlDataAdapter(strQuery, cnnObject)

        Try
            da.Fill(DS, strTableName)
        Catch e As Exception
            'MsgBox(e.Message & " Error in Filling Data " & strQuery.ToString)
            Exit Sub
        End Try
        da.Dispose()
    End Sub

    Public Function returnRandom(ByVal iLen As Integer) As String
        returnRandom = ""
        If cnnObject Is Nothing Then
            OpenConnection()
        End If
        Dim cmm As SqlCommand
        Dim dRead As SqlDataReader

        Try
            cmm = New SqlCommand("select Left(NewId()," & iLen & ") as RandomNum", cnnObject)
            dRead = cmm.ExecuteReader
            'Response.Write(Cmm.CommandText)
            'Response.End()
            If dRead.Read = False Then
                dRead.Close()
            Else
                returnRandom = dRead("RandomNum")
                Exit Function
            End If
        Catch e As Exception
            'MsgBox(e.Message & " Error in Filling Data " & strQuery.ToString)
            Exit Function
        End Try
        dRead.Dispose()
    End Function

    Public Function Fire_Query(ByVal Query As String) As Integer

        If cnnObject Is Nothing Or cnnObject.State = ConnectionState.Closed Then
            OpenConnection()
        End If


        Dim affectedRows As Integer = 0

        Try
            tran = cnnObject.BeginTransaction
            cmd = New SqlCommand(Query, cnnObject)
            cmd.CommandTimeout = 0
            cmd.Transaction = tran
            affectedRows += cmd.ExecuteNonQuery()
            tran.Commit()
            Return affectedRows

        Catch e As Exception

            tran.Rollback()
            'MsgBox(e.Message)
            Throw e
        Finally
            cmd = Nothing
            '  tran = Nothing
        End Try

    End Function

    Public Function Fire_Query_For_Procedure(ByVal Query As String) As Integer

        If cnnObject Is Nothing Or cnnObject.State = ConnectionState.Closed Then
            OpenConnection()
        End If

        Dim affectedRows As Integer = 0

        Try
            tran = cnnObject.BeginTransaction
            cmd = New SqlCommand(Query, cnnObject)
            cmd.CommandTimeout = 0
            cmd.Transaction = tran
            cmd.ExecuteNonQuery()
            tran.Commit()
            affectedRows = 1
            Return affectedRows
        Catch e As Exception
            tran.Rollback()
            'MsgBox(e.Message)
            'MyLogError.WriteFile(e.Message)
            Throw e
        Finally
            cmd = Nothing
            '  tran = Nothing
        End Try

    End Function




    Public Function Get_ServerDate() As DateTime
        Dim SqlD As String
        Dim SqlDs As New DataSet

        'SqlD = "Select Convert(char (13),getdate(),113) +    convert(varchar(10),getdate(),108)   as Dts"
        SqlD = "Select Cast(Convert(Varchar,Getdate(),106) as DateTime) as Dts"
        ' SqlD = "Select GetDate() as Dts"

        Fill_DataSET_Tables(SqlD, SqlDs, "MyDate")
        If cnnObject Is Nothing Or cnnObject.State = ConnectionState.Closed Then
            OpenConnection()
        End If

        If SqlDs.Tables("MyDate").Rows.Count > 0 Then
            Get_ServerDate = SqlDs.Tables("MyDate").Rows(0).Item("Dts")
        Else
            Get_ServerDate = Format(Date.Now, "dd-MMM-yyyy")
        End If
        ' MsgBox(Format(Get_ServerDate, "dd-MMM-yyyy"))
        Get_ServerDate = Format(Get_ServerDate, "dd-MMM-yyyy")
    End Function

End Class
