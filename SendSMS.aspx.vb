Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Globalization
Partial Class App_UI_Application_Pages_SendSMS
    Inherits System.Web.UI.Page
    Dim Obj As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " SMS / Bulk SMS"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                BindData()
            End If
        End If
    End Sub

    Public Sub BindData()
        Dim sql As String = "Select 'false' as Status,a.IdNo,a.FormNo,RTRIM(LTRIM(RTRIM(a.Prefix))+' '+a.MemFirstName + ' '+ a.MemLastName) as MemName,DOJ,replace(Convert(varchar,a.DOJ,106),' ','-') as JoinDate,a.RefFormNo,a.City ,a.Mobl FROM M_memberMaster as a," & _
" (Select ROW_NUMBER() Over(PARTITION BY Mobl Order BY DOJ) as RwNo ,Mobl,IDNO FROM M_MemberMaster) as b" & _
" WHERE a.IdNo=b.IdNo AND b.RwNo=1"
        Dim Dt_ As New DataTable
        Dt_ = Obj.GetData(sql)
        GrdMemList.DataSource = Dt_
        GrdMemList.DataBind()
        Session("MemberData") = Dt_
    End Sub

    Private Sub SendSMS()
        Dim Chk As CheckBox
        Dim Lbl As Label
        Dim client As New WebClient
        Dim baseurl As String
        Dim data As Stream
        Dim Cnt As Integer
        Dim sms As String = TxtSMS.Text
        For Each Gvr As GridViewRow In GrdMemList.Rows
            Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
            Lbl = DirectCast(Gvr.FindControl("LblMobl"), Label)
            If Chk.Checked = True Then
                Try
                    Dim Mobl As String = Lbl.Text

                    ' baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Mobl & "&msg=" & sms & ""
                    baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & Mobl & "&SenderId=" & Session("ClientId") & ""

                    data = client.OpenRead(baseurl)
                    Dim reader As New StreamReader(data)
                    Dim s As String
                    s = reader.ReadToEnd()
                    data.Close()
                    reader.Close()
                Catch ex As Exception
                    'MsgBox(ex.Message)
                End Try
            End If
        Next
    End Sub

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        SendSMS()
    End Sub

    Protected Sub GrdMemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdMemList.PageIndexChanging
        GrdMemList.PageIndex = e.NewPageIndex
        GrdMemList.DataSource = Session("MemberData")
        GrdMemList.DataBind()
        ''  Session("grdIndex") = GvData.PageIndex
    End Sub

    Protected Sub GrdMemList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdMemList.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & DirectCast(e.Row.FindControl("ChkSelectAll"), CheckBox).ClientID & "')")
        End If
    End Sub
End Class
