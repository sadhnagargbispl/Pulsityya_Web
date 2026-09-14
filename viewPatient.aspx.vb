Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Services
Partial Class App_UI_Application_Pages_viewPatient
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction
    Dim objGen As clsGeneral = New clsGeneral
    Dim VId As Integer

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" Then
            Session("PageName") = " Member / Visiting Verify"
        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'objDAL = New DAL()
            'objModuleFun = New ModuleFunction()
            If Not Page.IsPostBack Then

                If String.IsNullOrEmpty(Request("VId")) = False Then
                    'VId = Crypto.Decrypt(objModuleFun.EncodeBase64(Request("VId")))
                    VId = Request("VId")

                    BindData()
                    DataGrid()
                End If

            Else

            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub BindData()
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim sql As String = " Exec Sp_Viewpatientdetails '" & VId & "' "

            Dim Dt1 = New DataTable
            Dt1 = objDAL.GetData(sql)
            'GvData.DataSource = Dt1
            'If Dt1.Rows.Count > 0 Then
            '    Lblmemname.Text = Dt1.Rows(0)("InstituteName")
            '    LblCityname.Text = Dt1.Rows(0)("CityName")
            '    'lblcourseName.Text = Dt1.Rows(0)("CourseName")
            '    lblId.Text = Dt1.Rows(0)("Id")
            'End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub DataGrid()
        Try
            Dim sql As String = " Exec Sp_Viewpatientdetails '" & VId & "' "
            Dim Dt1 = New DataTable
            Dt1 = objDAL.GetData(sql)

            'GvData.DataSource = Dt1
            'GvData.DataBind()
            Session("GData") = Dt1
            If Dt1.Rows.Count > 0 Then
                txtnameofpatient.Text = Dt1.Rows(0)("Name Of Patient")
                txtFatherName.Text = Dt1.Rows(0)("Father/Husband Name")
                txtcontactno.Text = Dt1.Rows(0)("Contact No")
                txtaddress.Text = Dt1.Rows(0)("Address")
                txtage.Text = Dt1.Rows(0)("Address")
                txtMaritalStatus.Text = Dt1.Rows(0)("MaritalStatus")
                txtOccupation.Text = Dt1.Rows(0)("Occupation")
                txtChildren.Text = Dt1.Rows(0)("Children")
                txtGeneticDisease.Text = Dt1.Rows(0)("Children")
                txtFamilyHistory.Text = Dt1.Rows(0)("FamilyHistory")
                txtCancer.Text = Dt1.Rows(0)("Cancer")
                txtParkinson.Text = Dt1.Rows(0)("Parkinson")
                txtArthritis.Text = Dt1.Rows(0)("Arthritis")
                txtHeartDisease.Text = Dt1.Rows(0)("HeartDisease")
                txtBP.Text = Dt1.Rows(0)("BP")
                txtThyroid.Text = Dt1.Rows(0)("Thyroid")
                txtChickenpox.Text = Dt1.Rows(0)("Chickenpox")
                txtTyphoid.Text = Dt1.Rows(0)("Typhoid")
                txtJaundice.Text = Dt1.Rows(0)("Jaundice")
                txtMeasles.Text = Dt1.Rows(0)("Measles")
                txtDengue.Text = Dt1.Rows(0)("Dengue")
                txtAccident.Text = Dt1.Rows(0)("Accident")
                txtOperation.Text = Dt1.Rows(0)("Operation")
                txtHospitalization.Text = Dt1.Rows(0)("Hospitalization")
                txtConstipation.Text = Dt1.Rows(0)("Constipation")
                txtGasAcidity.Text = Dt1.Rows(0)("GasAcidity")
                txtSleep.Text = Dt1.Rows(0)("Sleep")
                txtHeadache.Text = Dt1.Rows(0)("Headache")
                txtPain.Text = Dt1.Rows(0)("Pain")
                txtCraving.Text = Dt1.Rows(0)("Craving")
                txtSweet.Text = Dt1.Rows(0)("Sweet")
                txtSpicy.Text = Dt1.Rows(0)("Spicy")
                txtRestlessness.Text = Dt1.Rows(0)("Restlessness")
                txtGrief.Text = Dt1.Rows(0)("Grief")
                txtDepression.Text = Dt1.Rows(0)("Depression")
                txtAlcohol.Text = Dt1.Rows(0)("Alcohol")
                txtSmoking.Text = Dt1.Rows(0)("Smoking")
                txtTobaccoChewing.Text = Dt1.Rows(0)("TobaccoChewing")
                txtSinusitis.Text = Dt1.Rows(0)("Sinusitis")
                txtColdCough.Text = Dt1.Rows(0)("ColdCough")
                txtAllergy.Text = Dt1.Rows(0)("Allergy")
                txtNasalPolyps.Text = Dt1.Rows(0)("NasalPolyps")
                txtEars.Text = Dt1.Rows(0)("Ears")
                txtEyes.Text = Dt1.Rows(0)("Eyes")
                txtHair.Text = Dt1.Rows(0)("Hair")
                txtMouth.Text = Dt1.Rows(0)("Mouth")
                txtThroat.Text = Dt1.Rows(0)("Throat")
                txtLungs.Text = Dt1.Rows(0)("Lungs")
                txtNose.Text = Dt1.Rows(0)("Nose")
                txtStomach.Text = Dt1.Rows(0)("Stomach")
                txtSpleen.Text = Dt1.Rows(0)("Spleen")
                txtRenalProblem.Text = Dt1.Rows(0)("RenalProblem")
                txtKidneyStone.Text = Dt1.Rows(0)("KidneyStone")
                txtUTI.Text = Dt1.Rows(0)("UTI")
                txtKnees.Text = Dt1.Rows(0)("Knees")
                txtdate.Text = Dt1.Rows(0)("date")


                txtnameofpatient.ReadOnly = True
                txtFatherName.ReadOnly = True
                txtcontactno.ReadOnly = True
                txtaddress.ReadOnly = True
                txtage.ReadOnly = True
                txtMaritalStatus.ReadOnly = True
                txtOccupation.ReadOnly = True
                txtChildren.ReadOnly = True
                txtGeneticDisease.ReadOnly = True
                txtFamilyHistory.ReadOnly = True
                txtCancer.ReadOnly = True
                txtParkinson.ReadOnly = True
                txtArthritis.ReadOnly = True
                txtHeartDisease.ReadOnly = True
                txtBP.ReadOnly = True
                txtThyroid.ReadOnly = True
                txtChickenpox.ReadOnly = True
                txtTyphoid.ReadOnly = True
                txtJaundice.ReadOnly = True
                txtMeasles.ReadOnly = True
                txtDengue.ReadOnly = True
                txtAccident.ReadOnly = True
                txtOperation.ReadOnly = True
                txtHospitalization.ReadOnly = True
                txtConstipation.ReadOnly = True
                txtGasAcidity.ReadOnly = True
                txtSleep.ReadOnly = True
                txtHeadache.ReadOnly = True
                txtPain.ReadOnly = True
                txtCraving.ReadOnly = True
                txtSweet.ReadOnly = True
                txtSpicy.ReadOnly = True
                txtRestlessness.ReadOnly = True
                txtGrief.ReadOnly = True
                txtDepression.ReadOnly = True
                txtAlcohol.ReadOnly = True
                txtSmoking.ReadOnly = True
                txtTobaccoChewing.ReadOnly = True
                txtSinusitis.ReadOnly = True
                txtColdCough.ReadOnly = True
                txtAllergy.ReadOnly = True
                txtNasalPolyps.ReadOnly = True
                txtEars.ReadOnly = True
                txtEyes.ReadOnly = True
                txtHair.ReadOnly = True
                txtMouth.ReadOnly = True
                txtThroat.ReadOnly = True
                txtLungs.ReadOnly = True
                txtNose.ReadOnly = True
                txtStomach.ReadOnly = True
                txtSpleen.ReadOnly = True
                txtRenalProblem.ReadOnly = True
                txtKidneyStone.ReadOnly = True
                txtUTI.ReadOnly = True
                txtKnees.ReadOnly = True
                txtdate.ReadOnly = True
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
