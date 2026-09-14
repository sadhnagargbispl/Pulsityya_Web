Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class FlatImage
    Inherits System.Web.UI.Page
    Public FormNo As String
    Dim dt As New DataTable
    Dim obj As DAL
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'Dim url As String = String.Empty

        'Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri

        'url = IIf(tempProtocol.StartsWith("http://"), "http://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.")


        'url = url.ToLower
        Dim Id As String
        If Request.QueryString.HasKeys And Not Request.QueryString("Id") Is Nothing Then
            Id = Val(Crypto.Decrypt(Replace(Request.QueryString("Id"), " ", "+")))
            Dim sql As String
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            sql = "SELECT Item as Imgpath  FROM dbo.SSplitString( (select Images From V#Projectmaster where Id='" & Id & "'), ',')"
            dt = obj.GetData(sql)
            If dt.Rows.Count > 0 Then
                RptPhotos.DataSource = dt
                RptPhotos.DataBind()
            End If
        End If
       
    End Sub
   
End Class
