Imports System.Data
Partial Class ListSeminarTicket
    Inherits System.Web.UI.Page
    Dim obj As DAL
    Dim quantity As String
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            If Not Page.IsPostBack Then
                Session("PageName") = "Epin / Generate Epin"
                FillProgram()
                FillGrid()
            End If
        End If


    End Sub

    Public Sub FillProgram()
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = String.Empty
            str = "Select * from ("
            str &= " Select MeetingID,Program from M_MeetingMaster Where ActiveStatus = 'Y' And Cast(MeetingTime as Date ) >=  Cast(Getdate() As date)"
            str &= " Union All "
            str &= " Select 0 As  MeetingID, '--Select Seminar--'"
            str &= " ) Temp Order by MeetingID"

            Dim Dt As New DataTable
            Dt = obj.GetData(str)
            ddlProgram.DataSource = Dt
            ddlProgram.DataTextField = "Program"
            ddlProgram.DataValueField = "MeetingID"
            ddlProgram.DataBind()
        Catch ex As Exception

        End Try
    End Sub



    Public Sub FillGrid()
        Try
            obj = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim str As String = String.Empty


            str = " Select  a.Program, Replace(Convert(Varchar,a.MeetingTime,106),' ','-')  As Date,"
            str &= " Convert(Varchar,a.MeetingTime,108) As Time, c.StateName,a.cityName,TicketNo,ScratchNo,"
            str &= "  Isnull(d.Idno,'') As  Idno,Isnull(d.MemfirstName,'') Name,"
            str &= " Case When   Isnull(d.Idno,'') <> '' then  Replace(Convert( Varchar, b.SoldDAte,106),' ','-') else '' end as SoldDAte"
            str &= " from M_MeetingMaster a Inner join M_SeminarTicketGeneration as b on a.meetingId = b.Programid"
            str &= " Inner join M_StateDivMaster as c  on a.statecode = c.StateCode and c.ActiveStatus = 'Y'"
            str &= "  Left Join M_memberMaster d on b.Formno = d.Formno "
            str &= " Where  a.ActiveStatus = 'Y' And b.ActiveStatus = 'Y' "
            If (ddlProgram.SelectedIndex > 0) Then
                str &= " And  a.meetingId = " & ddlProgram.SelectedValue & ""
            End If



            Dim Dt As New DataTable
            Dt = obj.GetData(str)
            GvData.DataSource = Dt
            GvData.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShow.Click
        Try
            FillGrid()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        FillGrid()
    End Sub
End Class
