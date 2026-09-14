Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class clsGeneral
    Public objSQlConnection As SqlConnection
    Public Connection As New SqlClient.SqlConnectionStringBuilder
    Public Sub Fill_Date_box(ByRef cday As DropDownList, ByRef cMonth As DropDownList, ByRef cYear As DropDownList, Optional ByVal YearStart As Integer = 1950, Optional ByVal yearEnd As Integer = 2010)

        For i As Int16 = 1 To 31
            cday.Items.Add(i.ToString.PadLeft(2, "0"))
        Next

        For i As Int16 = 1 To 12
            cMonth.Items.Add(Strings.Left(MonthName(i), 3).Trim.ToUpper)
        Next

        For i As Integer = YearStart To yearEnd
            cYear.Items.Add(i)
        Next
    End Sub

    Public Sub FillCmb(ByRef Cmb As DropDownList, ByRef strTbl As DataTable, ByRef strValFld As String, ByRef strTxtFld As String)
        With Cmb
            .DataSource = strTbl
            .DataValueField = strValFld
            .DataTextField = strTxtFld
            .DataBind()
        End With
    End Sub


    Private Function RandomNumber(ByVal min As Integer, ByVal max As Integer) As Integer
        Dim random As New Random()
        Return random.Next(min, max)
    End Function 'RandomNumber 

    Private Function RandomString(ByVal size As Integer, ByVal lowerCase As Boolean) As String
        Dim builder As New StringBuilder()
        Dim random As New Random()
        Dim ch As Char
        Dim i As Integer
        For i = 0 To size - 1
            ch = Convert.ToChar(Convert.ToInt32((26 * random.NextDouble() + 65)))
            builder.Append(ch)
        Next
        If lowerCase Then
            Return builder.ToString().ToLower()
        End If
        Return builder.ToString()
    End Function 'RandomString 

    Public Function GenerateRandomCode() As String
        Dim builder As New StringBuilder()
        builder.Append(RandomString(1, True))
        builder.Append(RandomNumber(1, 9))
        builder.Append(RandomString(1, True))
        builder.Append(RandomNumber(1, 9))
        builder.Append(RandomString(1, True))
        Dim myRandomStr As String
        myRandomStr = builder.ToString()
        Return myRandomStr
    End Function 'GenerateRandomCode 
    Public Function myMsgBx(ByVal sMessage As String) As String
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" & sMessage & "');"
        msg += "</script>"
        Return msg
    End Function
    Public Function ClrAllCtrl() As String
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += " rstCtrl(); "
        msg += "</script>"
        Return msg
    End Function



    Public Function GetConnectionByComp() As String
        Dim ds As DataSet = New DataSet
        Dim msg As String
        Dim CompID As String = "1108"
        'Dim str As String = " Exec Proc_GetConnection1 '" + CompID + "' "
        'Connection.ConnectionString = ConfigurationManager.ConnectionStrings("sconstr").ConnectionString
        'objSQlConnection = New SqlClient.SqlConnection(Connection.ConnectionString)
        'ds = SqlHelper.ExecuteDataset(objSQlConnection, CommandType.Text, str)
        msg = "Data Source =103.193.74.91,1533;Initial Catalog=darju9;Integrated Security=false;User ID=usrdarju9;PWD=Ju9!7@#DrVsh;Max Pool Size=2000;Pooling=true"
        HttpContext.Current.Session("MlmDatabase" & CompID) = msg
        Return msg
    End Function



    Public Function GetInvDataBaseByComp() As String
        Dim ds As DataSet = New DataSet
        Dim msg As String
        Dim CompID As String = "1108"
        'Dim str As String = " Exec Proc_GetConnection1 '" + CompID + "' "
        'Connection.ConnectionString = ConfigurationManager.ConnectionStrings("sconstr").ConnectionString
        'objSQlConnection = New SqlClient.SqlConnection(Connection.ConnectionString)
        'ds = SqlHelper.ExecuteDataset(objSQlConnection, CommandType.Text, str)
        msg = "Data Source =103.193.74.91,1533;Initial Catalog=darju9;Integrated Security=false;User ID=usrdarju9;PWD=Ju9!7@#DrVsh;Max Pool Size=2000;Pooling=true"
        HttpContext.Current.Session("InvDatabase" & CompID) = msg
        Return msg
    End Function



End Class
