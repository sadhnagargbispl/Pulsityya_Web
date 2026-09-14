Imports System.Data
Imports System.Data.SqlClient
Partial Class SponsorTree
    Inherits System.Web.UI.Page
    Dim Comm As SqlCommand
    Dim Conn As SqlConnection
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Session("AStatus") = "OK" Then
            Dim DownFormNo As String = get_FormNo(DownLineFormNo.Value)
            If DownFormNo = "" Then
                Dim scrname As String = "<SCRIPT language='javascript'>alert('Member ID Not Exist.!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                DownLineFormNo.Value = ""
            Else
                'TreeFrame.Attributes.Item("src") = "Referaltree.aspx?DownLineFormNo=" & DownFormNo
                Response.Redirect("Referaltree.aspx?DownLineFormNo=" & DownFormNo)
            End If
            
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") <> "OK" Then
            Response.Redirect("Default.aspx")
        Else
            Session("PageName") = "Member / Sponsor Tree"

        End If
    End Sub
    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("Home.aspx")
    End Sub

    Private Function get_FormNo(ByVal IDNo As String) As String
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        Dim FormNo As String = ""
        Dim dr As SqlDataReader
        Comm = New SqlCommand("Select FormNo From M_MemberMaster Where IDNo='" & IDNo & "'", Conn)
        dr = Comm.ExecuteReader
        If dr.Read = True Then
            FormNo = dr("FormNo")
        End If
        dr.Close()
        Comm.Cancel()
        Conn.Close()
        Return FormNo
    End Function
End Class
