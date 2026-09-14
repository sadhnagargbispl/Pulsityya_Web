Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Partial Class App_UI_Application_Pages_SessionClosing
    Inherits System.Web.UI.Page
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim obj As DAL
    Dim dt As New DataTable
    Dim Sql As String = ""
    Dim objGen As clsGeneral = New clsGeneral


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            Session("PageName") = "Payout / Generate Payout"
            If Not Page.IsPostBack Then
                FillDetail()
                Fillsession()
            End If
            'LblSession.Text = Session("CurrentSessn")

        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub FillDetail()
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        Dim s As String = ""
        s = "Select Sessid from M_SessnMaster where ToDate is  Null"
        dt = obj.GetData(s)
        If dt.Rows.Count > 0 Then
            TxtSession.Text = dt.Rows(0)("Sessid")
        End If

        dt = New DataTable
        obj = New DAL((HttpContext.Current.Session("MlmDatabase" & Session("CompID"))))
        Dim sql As String
        sql = "select SessId,CONVERT(VARCHAR,FrmDate,106) as FrmDate,CONVERT(VARCHAR,ToDate,106) as ToDate, Case when OnWebSite ='Y' then 'Showing' else 'Not Showing' end as WebSite,Case when OnWebSite ='Y' then 'True' else 'False' end as Status   from M_SessnMaster where ToDate IS NOT NULL AND IsPayoutCalc='Y' Order by Sessid Desc "
        dt = obj.GetData(sql)
        Session("DirectData") = dt
        GvData.DataSource = dt
        GvData.DataBind()

        Conn.Close()

    End Sub
    
    
    Protected Sub UpdateRecord()
        Dim sql As String
        Dim scrname As String
        Dim onwebsite As String
        Dim LblOnwebsite As Label
        Dim lblsess As Label
        Dim chweb As CheckBox
        For Each Gvr As GridViewRow In GvData.Rows
            LblOnwebsite = DirectCast(Gvr.FindControl("LblOnWebsite"), Label)
            lblsess = DirectCast(Gvr.FindControl("LblSessId"), Label)
            chweb = DirectCast(Gvr.FindControl("chwebsite"), CheckBox)

            If chweb.Checked = True Then
                'If LblOnwebsite.Text = "Showing" Then
                onwebsite = "Y"
            Else
                onwebsite = "N"

            End If
            ' If chweb.Checked = True Then
            sql = "Update M_SessnMaster Set  Onwebsite='" & onwebsite & "' where SessId='" & lblsess.Text & "' "
            If obj.UpdateData(sql) <> 0 Then
                scrname = "<SCRIPT language='javascript'>alert(' Successfully Update!! ');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
            End If
            ' End If

        Next
        FillDetail()
        'End If
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Try
            Dim sessiondate As DateTime = txtSessionDate.Text
            Dim nextsessiondate As DateTime = sessiondate.AddDays(1)
            Dim scrname As String
            Dim sess As String = ""
            Dim sql1 As String = ""
            Dim MaxSess As String = Session("CurrentSessn") + 1
            sql1 = "select Max(Sessid)as SessId from M_SessnMaster"
            dt = obj.GetData(sql1)
            If dt.Rows.Count > 0 Then
                sess = dt.Rows(0)("SessId")
            End If
            If sess = MaxSess Then
                Exit Sub
            Else
                Sql = "Update M_SessnMaster Set Todate='" & txtSessionDate.Text & "' where ToDate Is  NULL"
                obj.UpdateData(Sql)
                Dim str As String


                str = "insert into M_SessnMaster(SessId,FrmDate,ToDate,OnWebSite,Ach,Fund,Rate)values( '" & (Session("CurrentSessn") + 1) & "' , '" & Format(nextsessiondate, "dd-MMM-yyyy") & "' ,NULL,'N',0,0,0)"
                'obj.SaveData(str)
                If obj.SaveData(str) <> 0 Then
                    scrname = "<SCRIPT language='javascript'>alert('Session Date Successfully Saved.');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                    FillDetail()
                    Fillsession()
                    'Else
                    '    scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');" & "</SCRIPT>"
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub


    Protected Sub Fillsession()
        Dim sql As String
        sql = "select Top 1*, case when IspayoutCalc='Y' then 'false' else 'True' end as Visiblestatus from M_SessnMaster where ToDate is Not Null Order By SessId Desc;"
        dt = obj.GetData(sql)
        If dt.Rows.Count > 0 Then
            TxtSession1.Text = dt.Rows(0)("SessId")
            TxtSessStart.Text = Format(dt.Rows(0)("FrmDate"), "dd-MMM-yyyy")
            TxtSessClose.Text = Format(dt.Rows(0)("ToDate"), "dd-MMM-yyyy")
            BtnPayoutCalc.Visible = dt.Rows(0)("VisibleStatus")
        Else
            TxtSession1.Text = ""
            TxtSessStart.Text = ""
            TxtSessClose.Text = ""

        End If



    End Sub

    Protected Sub BtnPayoutCalc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPayoutCalc.Click
        Try
            Dim str As String
            Dim sql As String = ""
            Dim scrname As String
            str = "Exec BinaryPayout '" & TxtSession1.Text & "';"

            If obj.SaveData1(str) <> 0 Then
                sql = "Insert Into TempSessnMaster select * from M_SessnMaster where SessId= '" & TxtSession1.Text & "'"
                sql = sql & "Update M_SessnMaster Set IsPayoutCalc='Y',PayoutCalcDate=GetDate() where SessId='" & TxtSession1.Text & "' "
                obj.UpdateData(sql)
                scrname = "<SCRIPT language='javascript'>alert('Payout Successfully Calculate.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Upgraded", scrname, False)
                FillDetail()


            End If
        Catch ex As Exception
            Response.Write(ex.Message)

        End Try


    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        Gvdata.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("DirectData")
        GvData.DataBind()
    End Sub
End Class
