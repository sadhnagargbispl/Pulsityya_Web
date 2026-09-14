<%@ WebHandler Language="VB" Class="ImgHandler" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Public Class ImgHandler : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "image/gif"
        'Dim connectionString As String = "Server=164.132.18.172;UID=ubgshp;PWD=S#09B!81ee$;Database=BigShopee;Pooling=False;Connect Timeout=100000000"
        Dim connectionString As String = "Server=111.118.190.137;UID=ubgshp;PWD=S#09B!81ee$;Database=BigShopee;Pooling=False;Connect Timeout=100000000"
        Dim connection As New SqlConnection(connectionString)
        connection.Open()
        Try
            Dim sql As String = ""
            If context.Request.QueryString("type") = "Address" Then
                sql = "Select IsNULL(AddrssProof,'') As ImgLoad from M_MemberMaster where Formno=" & Val(Replace(Replace(Replace(Trim(context.Request.QueryString("id")), "'", ""), "=", ""), ";", "")) & ""
            ElseIf context.Request.QueryString("type") = "Identity" Then
                sql = "Select IsNULL(IdentityProof,'') As ImgLoad  from M_MemberMaster where Formno=" & Val(Replace(Replace(Replace(Trim(context.Request.QueryString("id")), "'", ""), "=", ""), ";", "")) & ""
            ElseIf context.Request.QueryString("type") = "Photo" Then
                sql = "Select IsNULL(MemPic,'') As ImgLoad  from M_MemberMaster where Formno=" & Val(Replace(Replace(Replace(Trim(context.Request.QueryString("id")), "'", ""), "=", ""), ";", "")) & ""
            ElseIf context.Request.QueryString("type") = "Sign" Then
                sql = "Select IsNULL(Signature,'') As ImgLoad  from M_MemberMaster where Formno=" & Val(Replace(Replace(Replace(Trim(context.Request.QueryString("id")), "'", ""), "=", ""), ";", "")) & ""
            Else : sql = "Select IsNULL(MemPic,'') As ImgLoad  from M_MemberMaster where IDno='" & Replace(Replace(Replace(Trim(context.Request.QueryString("id")), "'", ""), "=", ""), ";", "") & "'"
            End If
            If sql <> "" Then
                Dim cmd As New SqlCommand(sql, connection)
                ''cmd.Parameters.AddWithValue("@Id", context.Request.QueryString("id"))
                cmd.Prepare()
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                If dr.Read() Then
                    context.Response.BinaryWrite(DirectCast(dr("ImgLoad"), Byte()))
                Else
                    context.Response.WriteFile("images/NoImage.png")
                End If
                dr.Close()
                connection.Close()
            Else
                context.Response.WriteFile("images/NoImage.png")
            End If
        Catch ex As Exception
            ' Throw ex
            context.Response.WriteFile("images/NoImage.jpg")
        Finally
            connection.Close()
        End Try
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class