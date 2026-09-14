Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports System.Web.UI.WebControls.RadioButton
Partial Class App_UI_Application_Pages_SMSSending
    Inherits System.Web.UI.Page
    Dim Obj As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " SMS / SMS Id Wise"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                '    BindData()
                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        TxtIDNo.Text = Request.QueryString("key").ToString
                        CmbSrchType.SelectedValue = "I"
                        BindData()
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub BindData()
        Dim sql As String
        
        '"Select 'false' as Status,a.IdNo,a.FormNo,RTRIM(LTRIM(RTRIM(a.Prefix))+' '+a.MemFirstName + ' '+ a.MemLastName) as MemName,DOJ,replace(Convert(varchar,a.DOJ,106),' ','-') as JoinDate,a.RefFormNo,a.City ,a.Mobl FROM M_memberMaster as a," & _
        '" (Select ROW_NUMBER() Over(PARTITION BY Mobl Order BY DOJ) as RwNo ,Mobl,IDNO FROM M_MemberMaster) as b" & _
        '" WHERE a.IdNo=b.IdNo AND b.RwNo=1"
        If CmbSrchType.SelectedValue = "I" Then
            sql = " Select 'true' as Status,a.FormNo,a.IDNO,RTRIM(a.MemFirstName + ' '+ a.MemLastName) as MemName,a.DOJ, " & _
            " replace(Convert(varchar,a.DOJ,106),' ','-') as JoinDate,a.RefFormNo,a.City ,a.Passw,a.Mobl,'' as leg FROM M_MemberMaster as a" & _
           " WHERE a.IdNo='" & Trim(TxtIDNo.Text) & "'"
        ElseIf CmbSrchType.SelectedValue = "S" Then 'Upliner Tree
            sql = "Select 'true' as Status,a.FormNo,a.IDNO,RTRIM(a.MemFirstName + ' '+ a.MemLastName) as MemName,a.DOJ," & _
            " replace(Convert(varchar,a.DOJ,106),' ','-') as JoinDate,a.RefFormNo,a.City ,a.Mobl,a.Passw,CASE WHEN b.LegNo=1 Then 'Left' Else 'Right' End as Leg FROM M_MemberMaster as a,M_MemTreeRelation as b,M_MemberMaster as U" & _
            " WHERE b.FormNo=U.FormNo AND U.IDNo='" & Trim(TxtIDNo.Text) & "' AND b.FormNoDwn=a.FormNo AND b.LegNo=" & RbLeg.SelectedValue
        ElseIf CmbSrchType.SelectedValue = "R" Then
            sql = "Select 'true' as Status,a.FormNo,a.IdNo,a.FormNo,RTRIM(a.MemFirstName + ' '+ a.MemLastName) as MemName,a.DOJ, " & _
            " replace(Convert(varchar,a.DOJ,106),' ','-') as JoinDate,a.RefFormNo,a.City ,a.Mobl,a.Passw,'' as leg FROM M_memberMaster as a,M_MemberMaster as b" & _
            " WHERE a.RefFormNo=b.FormNo AND b.IdNo='" & Trim(TxtIDNo.Text) & "'"
        Else
            sql = " select Idno,Passw,Epassw,(MemFirstName+''+MemlastName) as MemberName,Mobl from M_memberMaster"
        End If
        
        Dim Dt_ As New DataTable
        Dt_ = Obj.GetData(sql)

        Dim sms As String = ""
        For i = 0 To Dt_.Rows.Count - 1
            sms = "Congratulations, Dear " & Dt_.Rows(i)("MemberName") & ",  Welcome to BBR Family, Your Login ID - " & Dt_.Rows(i)("Idno") & ", Password - " & Dt_.Rows(i)("Passw") & ", Trans Pass - " & Dt_.Rows(i)("EPassw") & ", For update and check your status details, please visit: www.shoptrade.co.in"
            SendSMS(sms, Dt_.Rows(i)("Mobl"))
        Next
        GrdMemList.DataSource = Dt_
        GrdMemList.DataBind()
        Session("MemberData") = Dt_
        If Dt_.Rows.Count > 0 Then
            DvData.Visible = True
        Else
            DvData.Visible = False
        End If
    End Sub

    Private Sub SendSMS(ByVal sms As String, ByVal Mobl As String)
        Dim Chk As CheckBox
        Dim Lbl As Label
        Dim client As New WebClient
        Dim baseurl As String
        Dim data As Stream
        Dim Cnt As Integer


        'For Each Gvr As GridViewRow In GrdMemList.Rows
        '    Dim sms As String = TxtSMS.Text
        '    Chk = DirectCast(Gvr.FindControl("chkSelect"), CheckBox)
        '    Lbl = DirectCast(Gvr.FindControl("LblMobl"), Label)

        '    sms = "Congratulations, Dear " & Session("Name") & ",  Welcome to BBR Family, Your Login ID - " & Session("SMSIDNo") & ", Password - " & MemberPass & ", Trans Pass - " & MemberTransPassw & ", For update and check your status details, please visit: " & Session("CompWeb1") & ""


        '    If Chk.Checked = True Then
        Try
            'Dim Mobl As String = Lbl.Text
            Cnt += 1
            'baseurl = "http://103.250.30.4/SendSMS/sendmsg.php?uname=" & Session("SmsId") & "&pass=" & Session("SmsPass") & "&send=" & Session("ClientId") & "&dest=" & Mobl & "&msg=" & sms & ""
            baseurl = " http://49.50.77.216/API/SMSHttp.aspx?UserId=" & Session("SmsId") & "&pwd=" & Session("SmsPass") & "&Message=" & sms & "&Contacts=" & Mobl & "&SenderId=" & Session("ClientId") & ""

            data = client.OpenRead(baseurl)
            Dim reader As New StreamReader(data)
            Dim s As String
            s = reader.ReadToEnd()
            data.Close()
            reader.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        '    End If
        'Next
        If Cnt > 0 Then
            Dim strScript As String = "<script language='javascript'>alert('" & Cnt & " SMS Sent.');</script>"
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Sent", strScript, False)
        End If
    End Sub

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        'SendSMS()
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
        If CmbSrchType.SelectedValue = "S" Then
            e.Row.Cells(4).Visible = True
        Else
            e.Row.Cells(4).Visible = False
        End If
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        BindData()
    End Sub

    Protected Sub CmbSrchType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbSrchType.SelectedIndexChanged
        If CmbSrchType.SelectedValue = "S" Then
            RbLeg.Visible = True
            lblReg.Visible = True
        Else
            RbLeg.Visible = False
            lblReg.Visible = False
        End If
    End Sub
End Class
