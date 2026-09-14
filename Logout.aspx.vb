
Partial Class Logout
    Inherits System.Web.UI.Page
    Dim objDal As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ExpiresAbsolute = DateAndTime.Now
        Session.Abandon()
        Response.Cookies.Remove("")
        Response.Cookies.Clear()

        Dim userId As Integer = Val(Session("UserID"))
        Dim username As String = Session("UserName")
        Dim sql As String = "Update " & objDal.tblUserMaster & " set LastLogOutTime='" & DateAndTime.Now.ToString() & "',LoginStatus='N' where UserId='" & userId & "' AND UserName like'%" & username & "%' AND " + objDal.activeCondition
        Dim a As Integer = objDal.UpdateData(sql)

        Dim nextpage As String = "Default.aspx"
        Response.Write("<script language=javascript>")

        Response.Write("{")
        Response.Write(" var Backlen=history.length;")

        Response.Write(" history.go(-Backlen);")
        Response.Write(" window.location.href='" & nextpage & "'; ")

        Response.Write("}")
        Response.Write("</script>")

        Session("IDARR") = Nothing
        Session("AStatus") = ""
        Session("Username") = ""
        Session("Idno") = ""
        Session("MemName") = ""
        Session("FSessID") = ""
        Session("KitId") = ""
        Session("Uid") = ""
        Session("UserID") = ""
        ''Session.Abandon()


        Response.Redirect("Default.aspx")
    End Sub
End Class
