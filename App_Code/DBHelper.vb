Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data
''' <summary>
''' Summary description for DBHelper
''' </summary>
Public Class DBHelper
    Private connection As SqlConnection
    Private adapter As SqlDataAdapter
    Private command As SqlCommand
    Private dataTable As DataTable
    Public Sub New()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sqlQuery">passing SQL Query here
    ''' <returns>DataTable object is returned</returns>
    Public Function GetTable(ByVal sqlQuery As String) As DataTable
        'creating new instance of Datatable

        dataTable = New DataTable()
        connection = Provider.GetConnection()
        command = New SqlCommand(sqlQuery, connection)
        'Open SQL Connection
        connection.Open()
        Try
            adapter = New SqlDataAdapter(command)
            adapter.Fill(dataTable)
        Catch
        Finally
            'Closing Sql Connection 
            connection.Close()
        End Try
        Return dataTable
    End Function
End Class
Public Class Provider
    Public Shared Function GetConnection() As SqlConnection
        'creating SqlConnection
        'Return New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        GetConnection = New SqlConnection("Data Source=216.218.185.82;Network Library=DBMSSOCN;Initial Catalog=FortuneLife;User ID=ForLife;Password=Fortune@11$Life;")
    End Function
End Class