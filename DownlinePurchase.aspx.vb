Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Data.SqlClient
Partial Class DownLinePurchase

    Inherits System.Web.UI.Page
    '  Dim ChinarAPI As New ChinarWebRef.Service
    ' Dim Conn As SqlConnection
    ' Dim Comm As SqlCommand
    Dim Objdal As DAL
    Dim dt As DataTable
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                        txtMemberId.Text = Request.QueryString("key").ToString
                        GetData()
                    End If
                End If
            End If
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub

    
    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        GetData()
    End Sub

    Private Sub GetData()
        Dim condition3 As String = ""
        Dim condition As String = ""
        Dim scrname As String = ""
        Dim Formno As String = get_FormNo(Trim(txtMemberId.Text))
        If Val(Formno) = 0 Then
            scrname = "<SCRIPT language='javascript'>alert('Please Check ID No.');" & "</SCRIPT>"
            Me.RegisterStartupScript("MyAlert", scrname)
            txtMemberId.Focus()
            Exit Sub
        End If
        If RbtLeg.SelectedValue <> 0 Then
            condition3 = condition3 & "AND B.LegNo='" & RbtLeg.SelectedValue & "'"
        End If
        If txtFromDate.Text <> "" And TxtToDate.Text <> "" Then
            Dim FrmDate As String = txtFromDate.Text
            Dim ToDate As String = TxtToDate.Text
            Try
                Dim Dt As DateTime = FrmDate
            Catch ex As Exception
                scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                Exit Sub
            End Try
            Try
                Dim Dt As DateTime = ToDate
            Catch ex As Exception
                scrname = "<SCRIPT language='javascript'>alert('Check End Date.. ');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                Exit Sub
            End Try
            If FrmDate <> "" And ToDate <> "" Then


                condition = condition & " And Cast(Replace(Convert(Varchar,temp.BillDate,106),' ','-') as Date)>='" & FrmDate & "' And Cast(Replace(Convert(Varchar,Temp.BillDate,106),' ','-') as Date)<='" & ToDate & "'"
            End If
        End If
    
        If RbtRequestType.SelectedValue = "R" Then

            condition = condition & " and temp.RequestType='Repurchase'"
        ElseIf RbtRequestType.SelectedValue = "T" Then
            condition = condition & " and temp.requestType<>'Repurchase'"
        End If
       
        
        '  Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        ' Conn.Open()
        Dim s As String

        If Session("CompId") <> "1030" Then
            s = "select temp.* from (Select  Replace(Convert(Varchar,c.BillDate,106),' ' ,'-') as BillDate" & _
          " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
       " Case when a.Orderno<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
       " Case when c.BillType='R' then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP,Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
       " Case when c.billtype='R'  then 'Repurchase'  else 'Joining' end as RequestType " & _
        " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
        " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
        " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
        " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
      " Left Join Trnorder as t On Cast(t.ORderNO as Varchar)=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
        "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
       "  Group by c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome, " & _
       " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
       " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "

        Else
            s = "select temp.* from (Select  Replace(Convert(Varchar,c.BillDate,106),' ' ,'-') as BillDate" & _
          " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
       " Case when a.Orderno<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
       " Case when (c.BillType='R' Or BillType='S' )then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP,Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
       " Case when (c.BillType='R' Or BillType='S' )  then 'Repurchase'  else 'Joining' end as RequestType " & _
        " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
        " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
        " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
        " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
      " Left Join Trnorder as t On Cast(t.ORderNO as Varchar)=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
        "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
       "  Group by c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome, " & _
       " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
       " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "


        End If
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dt = Objdal.GetData(s)
        If dt.Rows.Count > 0 Then
            BtnExport.Enabled = True
        Else
            BtnExport.Enabled = True
        End If
        Session("DataLeftPurchase") = dt
        GrdDirects.DataSource = dt
        GrdDirects.DataBind()
    End Sub

    Private Function get_FormNo(ByVal IDNo As String) As String
        'Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        'Conn.Open()
        Dim FormNo As String = ""
        'Dim d As SqlDataReader
        Objdal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        dt = Objdal.GetData("Select FormNo From M_MemberMaster Where IDNo='" & IDNo & "'")
        ' Comm = New SqlCommand(, Conn)
        'dr = Comm.ExecuteReader
        If dt.Rows.Count > 0 Then
            IDNo = dt.Rows(0)("FormNo")
        End If
        Return IDNo
    End Function

    Protected Sub GrdDirects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdDirects.PageIndexChanging
        GrdDirects.PageIndex = e.NewPageIndex
        GrdDirects.DataSource = Session("DataLeftPurchase")
        GrdDirects.DataBind()
    End Sub

    Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        Try
            Dim dtTemp As New DataTable
            Dim dg As New DataGrid
            Dim scrname As String = ""
            Dim condition3 As String = ""
            Dim Condition1 As String = ""
            Dim Formno As String = get_FormNo(Trim(txtMemberId.Text))
            If Val(Formno) = 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Please Check ID No.');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                txtMemberId.Focus()
                Exit Sub
            End If
            If RbtLeg.SelectedValue <> 0 Then
                condition3 = condition3 & "AND B.LegNo='" & RbtLeg.SelectedValue & "'"
            End If
            If txtFromDate.Text <> "" And TxtToDate.Text <> "" Then
                Dim FrmDate As String = txtFromDate.Text
                Dim ToDate As String = TxtToDate.Text
                Try
                    Dim Dt As DateTime = FrmDate
                Catch ex As Exception
                    scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" & "</SCRIPT>"
                    Me.RegisterStartupScript("MyAlert", scrname)
                    Exit Sub
                End Try
                Try
                    Dim Dt As DateTime = ToDate
                Catch ex As Exception
                    scrname = "<SCRIPT language='javascript'>alert('Check End Date.. ');" & "</SCRIPT>"
                    Me.RegisterStartupScript("MyAlert", scrname)
                    Exit Sub
                End Try
                If FrmDate <> "" And ToDate <> "" Then


                    Condition1 = Condition1 & " And Cast(Replace(Convert(Varchar,Temp.BillDate,106),' ','-') as Date)>='" & FrmDate & "' And Cast(Replace(Convert(Varchar,Temp.BillDate,106),' ','-') as Date)<='" & ToDate & "'"
                End If
            End If
            If RbtRequestType.SelectedValue = "R" Then

                Condition1 = Condition1 & " and Temp.Status='Repurchase'"
            ElseIf RbtRequestType.SelectedValue = "T" Then
                Condition1 = Condition1 & " and Temp.Status<>'Repurchase'"
            End If

            Dim ObjDal As DAL
            ObjDal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
            Dim s As String
            '   s = "select temp.* from (Select  Replace(Convert(Varchar,c.BillDate,106),' ' ,'-') as BillDate,m.idNo,(m.MemFirstName+ +M.MemLastName)as  " & _
            '            " MemberName,b.MLevel,k.KitName as PackageName,Case when a.Orderno<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
            '            " sum(c.RepurchIncome) as Bv,Case when c.BillType='R' then 0 else  Isnull(Sum(c.Sp),0) end as SP,Case when b.LegNo=1 Then 'Group A' else  " & _
            '         " 'Group B' end as LegNo,Case when c.billtype='R'  then 'Repurchase'  else 'Combo Request' end as RequestType  " & _
            '         "  FROM M_MemTreeRelation as b,M_MemberMaster as M," & _
            '         " RepurchIncome as c Left Join (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
            '" FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain) as a On (a.BillNo =c.BillNo  Or c.BillNo=a.OrderNo) " & _
            '         " and c.Formno=a.Formno Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y' " & _
            '          " Left Join Trnorder as t On Cast(t.ORderNO as Varchar)=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
            '         " WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
            '         "  Group by c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & Condition1 & "" & _
            '         " Order By Year(Temp.BillDate)Desc,Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            If Session("Compid") <> "1030" Then
                s = "select temp.Billdate,temp.Idno,Temp.MemberName,Temp.LegNo as GroupName,Temp.PackageName,Temp.Amount as BillAmount,Temp.BV as RepurchaseBV,Temp.Status,Temp.SP" & _
        " from (Select  Replace(Convert(Varchar,c.BillDate,106),' ' ,'-') as BillDate" & _
       " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
    " Case when a.Orderno<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
    " Case when c.BillType='R' then 0 else  Isnull(Sum(c.Sp),0) end as SP,Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
    " Case when c.billtype='R'  then 'Repurchase'  else 'Joining' end as Status " & _
     " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
     " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
     " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo  Or c.BillNo=a.OrderNo) and RwNO=1 " & _
     " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
   " Left Join Trnorder as t On Cast(t.ORderNO as Varchar)=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
     "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
    "  Group by c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome, " & _
    " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & Condition1 & " Order By Year(Temp.BillDate)Desc," & _
    " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "

            Else
                s = "select temp.Billdate,temp.Idno,Temp.MemberName,Temp.LegNo as GroupName,Temp.PackageName,Temp.Amount as BillAmount,Temp.BV as RepurchaseBV,Temp.Status,Temp.SP" & _
        " from (Select  Replace(Convert(Varchar,c.BillDate,106),' ' ,'-') as BillDate" & _
       " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
    " Case when a.Orderno<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
    " Case when (c.BillType='R' Or BillType='S') then 0 else  Isnull(Sum(c.Sp),0) end as SP,Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
    " Case when (c.BillType='R' Or BillType='S')  then 'Repurchase'  else 'Joining' end as Status " & _
     " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
     " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
     " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo  Or c.BillNo=a.OrderNo) and RwNO=1 " & _
     " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
   " Left Join Trnorder as t On Cast(t.ORderNO as Varchar)=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
     "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
    "  Group by c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome, " & _
    " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & Condition1 & " Order By Year(Temp.BillDate)Desc," & _
    " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "

            End If

            dtTemp = New DataTable
            dtTemp = ObjDal.GetData(s)
            dg.DataSource = dtTemp
            dg.DataBind()

            ExportToExcel("DownlinePurchase.xls", dg)

        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try
    End Sub
    Private Sub ExportToExcel(ByVal strFileName As String, ByRef dg As DataGrid)
        Dim sw As New System.IO.StringWriter
        Dim htw As System.Web.UI.HtmlTextWriter
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.xls"
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName)
        Response.Charset = ""
        dg.EnableViewState = False
        htw = New HtmlTextWriter(sw)
        dg.RenderControl(htw)
        Response.Write(sw.ToString())
        Response.End()
    End Sub

    Protected Sub txtMemberId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberId.TextChanged

    End Sub
End Class
