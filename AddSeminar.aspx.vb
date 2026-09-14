Imports System.Data
Imports System.Data.SqlClient
Partial Class App_UI_Application_Pages_AddSeminar
    Inherits System.Web.UI.Page
    Dim _dblAvailLeg As Double = 0
    Private dbGeneral As New clsGeneral
    Private dbConnect As cls_DataAccess
    Dim strQuery, scrname As String
    Dim TmpTable As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            dbConnect = New cls_DataAccess(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dbConnect.OpenConnection()
            If Not Page.IsPostBack Then

                FillStateMaster()
                Fill_TimeCombo()
                If String.IsNullOrEmpty(Request("MeetingID")) = False Then
                    TxtMeetingID.Text = Request("MeetingID")
                    BtnAdd.Text = "Modify"
                    FillDetail()
                End If
            End If
        Else
            Response.Redirect("logout.aspx")
        End If
    End Sub

    Private Sub FillDetail()
        strQuery = "Select *,Replace(Convert(varchar,MeetingTime,106),' ','-') as MDate,SUBSTRING(Convert(varchar,MeetingTime,109),13,2) as MHH,DatePart(mi,MeetingTime) as Mmm,SUBSTRING(Convert(varchar,MeetingTime,109),25,2) as Mtt FROM M_MeetingMaster WHERE MeetingID='" & Val(TxtMeetingID.Text) & "'"
        Dim Dt As New DataTable
        Dt = dbConnect.Fill_Data_Tables(strQuery, Dt)
        If Dt.Rows.Count > 0 Then
            TxtAddr.Text = Dt.Rows(0)("Address")
            TxtSpeaker.Text = Dt.Rows(0)("Speaker")
            TxtContactNo.Text = Dt.Rows(0)("ContactNo")
            TxtContPerson.Text = Dt.Rows(0)("ContactPerson")
            CmbState.SelectedValue = Dt.Rows(0)("StateCode")
            FillDistrictMaster()
            If Val(Dt.Rows(0)("DistrictCode")) > 0 Then
                ddlDistrict.SelectedValue = Dt.Rows(0)("DistrictCode")
            Else
                TxtOtherDist.Text = Dt.Rows(0)("DistrictName")
            End If
            FillCityMaster()
            If Val(Dt.Rows(0)("CityCode")) > 0 Then
                ddlTehsil.SelectedValue = Dt.Rows(0)("CityCode")
            Else
                TxtOtherCity.Text = Dt.Rows(0)("CityName")
            End If
            RbStatus.SelectedValue = Dt.Rows(0)("ActiveStatus")
            TxtProg.Text = Dt.Rows(0)("Program")
            TxtRemarks.Text = Dt.Rows(0)("Remarks")
            TxtDate.Text = Dt.Rows(0)("MDate")
            DdlHH.SelectedIndex = Val(Trim(Dt.Rows(0)("MHh"))) - 1
            DdlMM.SelectedValue = Val(Dt.Rows(0)("Mmm"))
            Ddltt.SelectedValue = Dt.Rows(0)("Mtt")
            txtMrp.Text = Dt.Rows(0)("Mrp")
            txtTraningWallet.Text = Dt.Rows(0)("Person")
        End If
    End Sub

    Private Sub FillStateMaster()
        strQuery = "SELECT STATECODE,STATENAME as State FROM M_StateDivMaster WHERE ACTIVESTATUS='Y' ORDER BY STATENAME"
        dbConnect.OpenConnection()
        dbConnect.Fill_Data_Tables(strQuery, TmpTable)
        With CmbState
            .DataSource = TmpTable
            .DataValueField = "STATECODE"
            .DataTextField = "State"
            .DataBind()
            If TmpTable.Rows.Count > 0 Then
                .SelectedIndex = 1
                'FillCityMaster()
            End If
        End With

    End Sub

    Private Sub FillDistrictMaster()
        strQuery = "SELECT DistrictCODE,DistrictNAME FROM M_DistrictMaster WHERE ACTIVESTATUS='Y' AND RowStatus='Y' AND StateCode='" & Val(CmbState.SelectedValue) & "' ORDER BY DistrictNAME"
        dbConnect.OpenConnection()
        dbConnect.Fill_Data_Tables(strQuery, TmpTable)
        With ddlDistrict
            .SelectedIndex = -1
            .DataSource = TmpTable
            .DataValueField = "DistrictCode"
            .DataTextField = "DistrictName"
            .DataBind()
        End With
        If TmpTable.Rows.Count > 0 Then
            ddlDistrict.SelectedIndex = 0
            FillCityMaster()
        End If
    End Sub

    Private Sub FillCityMaster()
        strQuery = "SELECT CityCODE,CityNAME FROM M_CityStateMaster WHERE ACTIVESTATUS='Y' AND DistrictCode='" & Val(ddlDistrict.SelectedValue) & "' ORDER BY CityNAME"
        TmpTable = New DataTable
        dbConnect.OpenConnection()
        dbConnect.Fill_Data_Tables(strQuery, TmpTable)
        With ddlTehsil
            .DataSource = TmpTable
            .DataValueField = "CityCode"
            .DataTextField = "CityName"
            .DataBind()
        End With
        If TmpTable.Rows.Count > 0 Then
            ddlTehsil.SelectedIndex = 0
        End If
    End Sub

    Private Sub Fill_TimeCombo()
        DdlHH.Items.Clear() ': DdlMM.Items.Clear()
        For i As Integer = 1 To 12
            DdlHH.Items.Add(i)
        Next
        'DdlMM.Items.Add("00") : DdlMM.Items.Add("15") : DdlMM.Items.Add("30") : DdlMM.Items.Add("45")
    End Sub

    Private Sub AddMeeting()
        Dim DistName, TehsilName As String
        Dim DistCode, CityCode As Integer
        Try

            If Trim(TxtOtherDist.Text) = "" Then
                If ddlDistrict.Items.Count > 0 Then
                    DistName = ddlDistrict.SelectedItem.Text : DistCode = ddlDistrict.SelectedValue
                Else
                    DistName = "" : DistCode = 0
                End If
            Else
                DistName = Trim(TxtOtherDist.Text) : DistCode = 0
            End If

            If Trim(TxtOtherCity.Text) = "" Then
                If ddlTehsil.Items.Count > 0 Then
                    TehsilName = ddlTehsil.SelectedItem.Text : CityCode = ddlTehsil.SelectedValue
                Else
                    TehsilName = "" : CityCode = "0"
                End If
            Else
                TehsilName = Trim(TxtOtherCity.Text) : CityCode = "0"
            End If
            dbConnect.OpenConnection()
            If String.IsNullOrEmpty(Request("MeetingID")) = False Then
                strQuery = "UPDATE M_MeetingMaster SET Speaker=@Speaker,MeetingTime='" & Trim(TxtDate.Text) & " " & DdlHH.SelectedItem.Text & ":" & DdlMM.SelectedItem.Text & " " & Ddltt.SelectedItem.Text & "'," & _
    "CityCode='" & CityCode & "',CityName=@CityName,DistrictCode='" & DistCode & "',DistrictName=@DistName,StateCode='" & Val(CmbState.SelectedValue) & "',Address=@Address,ContactNo=@ContactNo," & _
    "ContactPerson=@ContactPerson,Remarks=@Remarks,Program=@Program,ActiveStatus='" & RbStatus.SelectedValue & "',Mrp = '" & txtMrp.Text & "',TraningW_Per = '" & txtTraningWallet.Text & "' WHERE MeetingID='" & Val(TxtMeetingID.Text) & "'"
            Else
                strQuery = "INSERT INTO M_MeetingMaster (MeetingID,Speaker,MeetingTime,CityCode,CityName,DistrictCode,DistrictName,StateCode,Address,ContactNo,ContactPerson,Remarks,ActiveStatus,UserType,UserID,Program,Mrp,TraningW_Per)" & _
          "Select CASE WHEN MAX(MeetingID) IS NULL THEN '1' ELSE Max(MeetingID)+1 END,@Speaker,'" & Trim(TxtDate.Text) & " " & DdlHH.SelectedItem.Text & ":" & DdlMM.SelectedItem.Text & " " & Ddltt.SelectedItem.Text & "'," & _
          "'" & CityCode & "',@CityName,'" & DistCode & "',@DistName,'" & Val(CmbState.SelectedValue) & "',@Address,@ContactNo,@ContactPerson,@Remarks,'" & RbStatus.SelectedValue & "','A','" & Val(Session("UserID")) & "',@Program,'" & txtMrp.Text & "','" & txtTraningWallet.Text & "' FROM M_MeetingMaster;"
            End If

            Dim SqlCmd As New SqlCommand(strQuery, dbConnect.cnnObject)
            SqlCmd.Parameters.AddWithValue("@Speaker", Trim(TxtSpeaker.Text))
            SqlCmd.Parameters.AddWithValue("@CityName", TehsilName)
            SqlCmd.Parameters.AddWithValue("@DistName", DistName)
            SqlCmd.Parameters.AddWithValue("@Address", Trim(TxtAddr.Text))
            SqlCmd.Parameters.AddWithValue("@ContactNo", Trim(TxtContactNo.Text))
            SqlCmd.Parameters.AddWithValue("@ContactPerson", Trim(TxtContPerson.Text))
            SqlCmd.Parameters.AddWithValue("@Remarks", Trim(TxtRemarks.Text))
            SqlCmd.Parameters.AddWithValue("@Program", Trim(TxtProg.Text))
            SqlCmd.ExecuteNonQuery()
            If String.IsNullOrEmpty(Request("MeetingID")) = False Then
                scrname = "<SCRIPT language='javascript'>alert('Seminar updated successfully.');" & "</SCRIPT>"
            Else
                scrname = "<SCRIPT language='javascript'>alert('Seminar added successfully.');" & "</SCRIPT>"
            End If

            Me.RegisterStartupScript("MyAlert", scrname)
            ClearData()

        Catch ex As Exception
            scrname = "<SCRIPT language='javascript'>alert('" & ex.Message & "');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
        End Try
        scrname = "<SCRIPT language='javascript'> window.top.location.reload();" & "</SCRIPT>"
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
    End Sub

    Protected Sub CmbState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbState.SelectedIndexChanged
        FillDistrictMaster()
    End Sub

    Protected Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        FillCityMaster()
    End Sub

    Private Sub ClearData()
        TxtAddr.Text = "" : TxtContactNo.Text = "" : TxtContPerson.Text = "" : TxtDate.Text = "" : TxtOtherCity.Text = ""
        TxtOtherDist.Text = "" : TxtRemarks.Text = "" : TxtSpeaker.Text = ""
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        AddMeeting()
    End Sub

End Class
