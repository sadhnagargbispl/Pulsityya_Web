Imports System.Data
Imports System.Data.SqlClient

Imports System.IO
Imports System.Net
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class Add_beneficiary_details
    Inherits System.Web.UI.Page
    Dim objDAL As DAL
    Dim dt As New DataTable
    Dim dtData As New DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Try
            If Session("AStatus") = "OK" Then
            Else
                Response.Redirect("logout.aspx")
            End If

            If Not Page.IsPostBack Then
                Filldate()
                FillCityPinDetail()

                FillReport2()
            End If


        Catch ex As Exception

        End Try
    End Sub

    'Private Sub FillCityPinDetail()
    '    Try

    '        Dim sql As String = String.Empty
    '        sql = " Select SessID,     "
    '        sql &= " (Convert(Varchar,SessID) +'-- ' + Replace(Convert(Varchar,FrmDate,106),' ','-') +' - '+ Replace(Convert(Varchar,ToDate,106),' ','-')) as Session"
    '        sql &= " From M_SessnMaster Where todate is not null"
    '        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '        dt = objDAL.GetData(sql)
    '        ddlstate.DataSource = dt
    '        ddlstate.DataTextField = "Session"
    '        ddlstate.DataValueField = "SessID"
    '        ddlstate.DataBind()
    '        ddlstate.Items.Insert(0, "--Select Session--")
    '    Catch ex As Exception

    '    End Try
    'End Sub


    Private Sub FillCityPinDetail()
        Try

            'Dim sql As String = "select * from WithdrawTransaction"
            'dtData = New DataTable
            'dtData = objDAL.GetData(sql)
            'If dtData.Rows.Count > 0 Then
            '    CmbMessage.DataSource = dtData
            '    CmbMessage.DataTextField = "message"
            '    CmbMessage.DataValueField = "id"
            '    CmbMessage.DataBind()
            '    CmbMessage.Items.Insert(0, "--Select Message--")
            'End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Filldate()
        Try


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim Str As String = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate "
            dtData = New DataTable
            dtData = objDAL.GetData(Str)
            If dtData.Rows.Count > 0 Then
                txtStartDate.Text = dtData.Rows(0)("CurrentDate")
                txtEndDate.Text = dtData.Rows(0)("CurrentDate")
            End If
        Catch ex As Exception

        End Try
    End Sub
    'Public Sub BindData()
    '    'lblError.Text = ""
    '    lblErr.Text = ""
    '    lblCount.Text = ""
    '    Dim Condition As String = ""
    '    Dim formno As String = ""
    '    Dim scrName As String = ""
    '    Dim Idno As String = "0"

    '    'If txtStartDate.Text = "" Then
    '    '    startDate = Session("CompDate")
    '    'Else
    '    '    startDate = txtStartDate.Text
    '    'End If
    '    'If txtEndDate.Text = "" Then
    '    '    endDate = Format(Date.Now, "dd-MMM-yyyy")
    '    'Else
    '    '    endDate = txtEndDate.Text
    '    'End If
    '    'If ChkKit.Checked = True Then
    '    '    Condition = Condition & " And KitName='" & CmbKit.SelectedItem.Text & "'"
    '    'Else
    '    '    Idno = "0"
    '    'End If

    '    Dim qry1 As String = ""

    '    ' qry1 = "select memberid,KitName as PackageName,Name,Doorno,Street,Postoffice,ViaTk,Distt,States,Pincode,Mobno1,Mobno2,datetimes from V#ProfileDetail_U where 1=1" & Condition & ""
    '    qry1 = "select b.Idno,TransId as TransactionId,a.Amount,A.AcCountNo,A.IFSCCode,Message,Replace(Convert(Varchar,A.RectimeStamp,106),' ','-')As TransactionDate from WithdrawTransaction as a,M_memberMaster as b where a.formno=b.formno " & Condition & ""
    '    If ChkMember.Checked = True Then
    '        'Idno = GetFormNo()
    '        'If RbtUser.SelectedValue = "M" Then

    '        If txtMemberId.Text <> "" Then

    '            Condition = Condition & " And Idno='" & txtMemberId.Text & "'"


    '        End If
    '    Else
    '        Idno = "0"

    '    End If
    '    If ChMessage.Checked = True Then
    '        Condition = Condition & " And message='" & CmbMessage.SelectedItem.Text & "'"
    '    Else
    '        Idno = "0"
    '    End If
    '    Dim startDate As Date
    '    Dim endDate As Date
    '    If txtStartDate.Text = "" Then
    '        startDate = Session("CompDate")
    '    Else
    '        qry1 &= "     And  Cast(A.RectimeStamp As Date) >= '" & txtStartDate.Text & "'"
    '    End If
    '    If txtEndDate.Text = "" Then
    '        endDate = Format(Date.Now, "dd-MMM-yyyy")
    '    Else
    '        qry1 &= "     And  Cast(A.RectimeStamp As Date) >= '" & txtEndDate.Text & "'"
    '    End If
    '    dtData = New DataTable
    '    dtData = objDAL.GetData(qry1)
    '    If dtData.Rows.Count > 0 Then
    '        GvData.DataSource = dtData
    '        GvData.DataBind()
    '        Session("LogReport") = dtData
    '        GvData.Visible = True
    '        gvContainer.Visible = True
    '        lblCount.Text = "Total : " & dtData.Rows.Count
    '    Else
    '        GvData.Visible = False
    '        gvContainer.Visible = False
    '        lblErr.Text = "No Record Found!!"
    '    End If

    'End Sub

    Private Sub FillReport()
        Try

            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            Dim Idno As String = "0"

            Dim qry1 As String = ""

            'qry1 = "select memberid,KitName as PackageName,Name,Doorno,Street,Postoffice,ViaTk,Distt,States,Pincode,Mobno1,Mobno2,datetimes from V#ProfileDetail_U where 1=1" & Condition & ""
            qry1 = "select b.Idno,TransId as TransactionId,a.Amount,A.AcCountNo,A.IFSCCode,Message,Replace(Convert(Varchar,A.RectimeStamp,106),' ','-')As TransactionDate from WithdrawTransaction as a, M_memberMaster as b where a.formno=b.formno " & Condition & ""

           
            Dim startDate As Date
            Dim endDate As Date

            If ChMessage.Checked = True Then
                'Condition = Condition & " And message='" & CmbMessage.SelectedItem.Text & "'"
                'qry1 &= " And message= '" & CmbMessage.SelectedItem.Text & "' order by id desc"
                qry1 &= " And message= '" & CmbMessage.SelectedItem.Text & "'"
            End If
            If ChkMember.Checked = True Then
                'Condition = Condition & " And Idno='" & txtMemberId.Text & "'"
                'qry1 &= "     And  Idno = '" & txtMemberId.Text & "' order by id desc"
                qry1 &= " And  Idno = '" & txtMemberId.Text & "'"
            End If

            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                'startDate = txtStartDate.Text
                qry1 &= "     And  Cast(A.RectimeStamp As Date) >= '" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                'endDate = txtEndDate.Text
                'qry1 &= "     And  Cast(A.RectimeStamp As Date) >= '" & txtEndDate.Text & "'"
                qry1 &= "     And  Cast(A.RectimeStamp As Date) <= '" & txtEndDate.Text & "'"
            End If


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(qry1)
            GvData.DataSource = dt
            GvData.DataBind()

            Session("GData") = dt
        Catch ex As Exception

        End Try
    End Sub


    Private Sub FillReport2()
        Try

            Dim Condition As String = ""
            Dim formno As String = ""
            Dim scrName As String = ""
            Dim Idno As String = "0"

            Dim qry1 As String = ""

            ' qry1 = "select memberid,KitName as PackageName,Name,Doorno,Street,Postoffice,ViaTk,Distt,States,Pincode,Mobno1,Mobno2,datetimes from V#ProfileDetail_U where 1=1" & Condition & ""
            qry1 = "select b.Idno,TransId as TransactionId,a.Amount,A.AcCountNo,A.IFSCCode,Message,Replace(Convert(Varchar,A.RectimeStamp,106),' ','-')As TransactionDate from WithdrawTransaction as a, M_memberMaster as b where a.formno=b.formno " & Condition & "order by id desc"


            Dim startDate As Date
            Dim endDate As Date

            If ChMessage.Checked = True Then
                'Condition = Condition & " And message='" & CmbMessage.SelectedItem.Text & "'"
                qry1 &= " And message= '" & CmbMessage.SelectedItem.Text & "'"
            End If
            If ChkMember.Checked = True Then
                'Condition = Condition & " And Idno='" & txtMemberId.Text & "'"
                qry1 &= "     And  Idno = '" & txtMemberId.Text & "'"
            End If

            If txtStartDate.Text = "" Then
                startDate = Session("CompDate")
            Else
                startDate = txtStartDate.Text
                'qry1 &= "     And  Cast(A.RectimeStamp As Date) >= '" & txtStartDate.Text & "'"
            End If
            If txtEndDate.Text = "" Then
                endDate = Format(Date.Now, "dd-MMM-yyyy")
            Else
                endDate = txtEndDate.Text
                'qry1 &= "     And  Cast(A.RectimeStamp As Date) >= '" & txtEndDate.Text & "'"
            End If


            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            dt = objDAL.GetData(qry1)
            GvData.DataSource = dt
            GvData.DataBind()

            Session("GData") = dt
        Catch ex As Exception

        End Try
    End Sub


    Private Sub FillReport_TeamBigway()
        Try

            'Dim sql As String = String.Empty
            'sql = " Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],d.mobl as [Mobile No.],e.Idno [Sponser IDNo],e.MemFirstName as [Sponser Name],"
            'sql &= " b.Rank,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as Date "
            'sql &= " from M_SessWiseBv  as A Inner join MstRanks As b on a.RankID = b.RankiD"
            'sql &= " left join M_SessnMAster as c on a.SessID = c.SessID "
            'sql &= " Left join M_memberMaster as d on a.formno = d.formno "
            'sql &= " Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 "
            'If (ddlstate.SelectedIndex > 0) Then
            '    sql &= "      And  b.RankID = " & ddlstate.SelectedValue & " "
            'End If
            'If (txtMemberID.Text <> "") Then
            '    sql &= "     And  d.Idno = '" & txtMemberID.Text & "' "
            'End If



            'objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            'dt = objDAL.GetData(sql)
            'GvData.DataSource = dt
            'GvData.DataBind()

            'Session("GData") = dt
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Session("CompID") = "1006" Then
        '    FillReport_TeamBigway()
        'Else
        FillReport()
        'End If

    End Sub

    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        If Session("CompID") = "1006" Then
            FillReport_TeamBigway()
        Else
            FillReport()
        End If
    End Sub


    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try

            'If Session("CompID") = "1006" Then
            '    FillReport_TeamBigway()
            'Else
            FillReport()
            'End If
            ExportExcel()

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")

        End Try
    End Sub

    Private Sub ExportExcel()
        Dim dt As DataTable = Session("GData")
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Incentive")
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=OnlineTransactionsReport.xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub


End Class
