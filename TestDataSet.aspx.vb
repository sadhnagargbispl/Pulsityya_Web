Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Xml
Imports System.Web.Script.Serialization

Partial Class TestDataSet
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim apiUrl As String = "https://api.cashfree.com/pg/orders/ORDER_20231215015015310/payments"
        Dim apiVersion As String = "2022-09-01"
        Dim clientId As String = "167039f0a3bf29bd3ad61dfdaa930761"
        Dim clientSecret As String = "7d80011f9b0902863ccf43ecc401f3904d889d5c"
        Dim responseJson As String = MakeApiRequest(apiUrl, apiVersion, clientId, clientSecret)
        Dim jss = New JavaScriptSerializer()
        Dim data = jss.Deserialize(Of Object)(responseJson)
        Dim status As String = data("payment_status")

        'Dim firstChar As Char = "{"
        'Dim lastChar As Char = "}"

        'Dim modifiedString As String = firstChar & responseJson.Substring(1, responseJson.Length - 2)
        'modifiedString = modifiedString.Remove(modifiedString.Length - 1, 1) & lastChar
        'Response.Write(modifiedString)
        'Dim ds As New DataSet()
        'ds = convertJsonStringToDataSet(responseJson)
        ' ''Context.Response.Write(responseJson)
    End Sub
    Function ReplaceFirstAndLast(ByVal input As String, ByVal firstReplacement As String, ByVal lastReplacement As String) As String
        ' Check if the string has at least two characters
        If input.Length >= 2 Then
            ' Replace the first and last characters
            Dim modifiedString As String = firstReplacement & input.Substring(1, input.Length - 2) & lastReplacement
            Return modifiedString
        Else
            ' Return the original string if it has less than two characters
            Return input
        End If
    End Function
    Public Function convertJsonStringToDataSet(ByVal jsonString As String) As DataSet
        Dim xd As XmlDocument = New XmlDocument()
        jsonString = "{ ""rootNode"": {" & jsonString.Trim().TrimStart("{"c).TrimEnd("}"c) & "} }"
        xd = CType(JsonConvert.DeserializeXmlNode(jsonString), XmlDocument)
        Dim ds As DataSet = New DataSet()
        ds.ReadXml(New XmlNodeReader(xd))
        Return ds
    End Function
    Function MakeApiRequest(ByVal apiUrl As String, ByVal apiVersion As String, ByVal clientId As String, ByVal clientSecret As String) As String
        System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType)
        Dim request As HttpWebRequest = CType(WebRequest.Create(apiUrl), HttpWebRequest)
        request.Method = "GET"
        If Not request.Headers.AllKeys.Contains("x-api-version") Then
            request.Headers.Add("x-api-version", apiVersion)
        End If
        If Not request.Headers.AllKeys.Contains("x-client-id") Then
            request.Headers.Add("x-client-id", clientId)
        End If
        If Not request.Headers.AllKeys.Contains("x-client-secret") Then
            request.Headers.Add("x-client-secret", clientSecret)
        End If
        Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
        Try
            If response.StatusCode = HttpStatusCode.OK Then
                Using reader As New StreamReader(response.GetResponseStream())
                    Return reader.ReadToEnd()
                End Using
            Else
                Console.WriteLine("Error: {response.StatusCode} - {response.StatusDescription}")
                Return Nothing
            End If
        Finally
            response.Close()
        End Try
    End Function
End Class
