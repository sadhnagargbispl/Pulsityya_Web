Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Data.SqlClient
Partial Class DownlinePurchaseLife

    Inherits System.Web.UI.Page
    '  Dim ChinarAPI As New ChinarWebRef.Service
    Dim Conn As SqlConnection
    Dim Comm As SqlCommand
    Dim Ad As SqlDataAdapter
    Dim dt As DataTable
    Dim objGen As clsGeneral = New clsGeneral
    Dim objdal As DAL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("AStatus") = "OK" Then
            If Not Page.IsPostBack Then
                ' FillLevel()
                If Request.QueryString.HasKeys Then
                    If Request.QueryString("key") <> "" And Not Request.QueryString("key") Is Nothing Then
                       
                        txtMemberId.Text = Request.QueryString("key").ToString
                        GetData()
                    End If
                End If
                If Session("CompID") = "1096" Then
                    'Rbtbsntype.Visible = True
                    If Rbtbsntype.SelectedValue = "T" Then
                        LblType.Text = "leg"
                        ddlLevel.Visible = False
                        RbtLeg.Visible = True
                    Else
                        'FillLevel()
                        LblType.Text = "Level"
                        ddlLevel.Visible = True
                        RbtLeg.Visible = False

                    End If
                End If
            End If
        Else
            Response.Redirect("Logout.aspx")
        End If
    End Sub
    Protected Sub FillLevel()
        ' Conn = New SqlConnection(Application("Connect"))
        'Conn.Open()

        Try
            Dim ds As New DataSet
            Dim Formno As String = get_FormNo(Trim(txtMemberId.Text))
            Dim prms As SqlParameter() = New SqlParameter(1) {}
            prms(0) = New SqlParameter("@FormNo", Formno)
            prms(1) = New SqlParameter("@type", "N")

            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session("MlmDatabase" & Session("CompID")), "sp_GetLevel", prms)
            ddlLevel.DataSource = ds.Tables(0)
            DdlLevel.DataTextField = "LevelName"
            DdlLevel.DataValueField = "MLevel"
            DdlLevel.DataBind()
            ' Conn.Close()
        Catch ex As Exception
            Dim path As String = HttpContext.Current.Request.Url.AbsoluteUri
            Dim text As String = path & ":  " & Format(Now, "dd-MMM-yyyy hh:mm:ss:fff " & Environment.NewLine)
            ' obj.WriteToFile(text & ex.Message)
            Response.Write("Try later.")
        End Try
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
        If Session("CompID") = "1096" Then
            'If RbtRequestType.SelectedValue = "T" Then
            '    If RbtLeg.SelectedValue <> 0 Then
            '        condition3 = condition3 & "AND B.LegNo='" & RbtLeg.SelectedValue & "'"

            '    End If
            'Else
            If ddlLevel.SelectedValue > 0 Then
                condition3 = condition3 & " And b.Mlevel='" & ddlLevel.SelectedValue & "'"
            End If


            'End If
        Else
            If Session("CompID") = "1096" Then
                'If RbtRequestType.SelectedValue = "T" Then
                '    If RbtLeg.SelectedValue <> 0 Then
                '        condition3 = condition3 & "AND B.LegNo='" & RbtLeg.SelectedValue & "'"

                '    End If
                'Else
                If ddlLevel.SelectedValue > 0 Then
                    condition3 = condition3 & " And b.Mlevel='" & ddlLevel.SelectedValue & "'"
                End If


                'End If
            Else
            If RbtRequestType.SelectedValue = "T" Then
                If RbtLeg.SelectedValue <> 0 Then
                    condition3 = condition3 & "AND B.LegNo='" & RbtLeg.SelectedValue & "'"

                End If
            Else
                If ddlLevel.SelectedValue > 0 Then
                    condition3 = condition3 & " And b.Mlevel='" & ddlLevel.SelectedValue & "'"
                End If


            End If
        End If
            

        End If
        
        If ChkDate.Checked Then
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


                    condition = condition & " And Cast(Replace(Convert(Varchar,temp.BillDate1,106),' ','-') as Date)>='" & FrmDate & "' And Cast(Replace(Convert(Varchar,Temp.BillDate1,106),' ','-') as Date)<='" & ToDate & "'"
                End If
            End If

        End If

       
        If Session("CompID") = "1096" Then
            If Rbtbsntype.SelectedValue = "R" Then
                condition = condition & " and temp.RequestType='Repurchase'"
            End If
        Else
            If RbtRequestType.SelectedValue = "R" Then

                condition = condition & " and temp.RequestType='Repurchase'"
            ElseIf RbtRequestType.SelectedValue = "T" Then
                condition = condition & " and temp.requestType<>'Repurchase'"
            End If
        End If
        

        'condition1 = " And a.Doj>='" & FrmDate & "' And a.Doj<='" & ToDate & "'"
        'condition2 = " And a.UpgradeDate>='" & FrmDate & "' And a.UpgradeDate<='" & ToDate & "'"

        Dim s As String
        's = "select temp.* from (Select  Replace(Convert(Varchar,c.BillDate,106),' ' ,'-') as BillDate,m.idNo,(m.MemFirstName+ +M.MemLastName)as  " & _
        '            " MemberName,b.MLevel,k.KitName as PackageName,Isnull(a.NetPayable,0) as Amount,sum(c.RepurchIncome) as Bv,Case when c.BillType='R' then 0 else  Isnull(Sum(c.Sp),0) end as SP,Case when b.LegNo=1 Then 'Group A' else  " & _
        '         " 'Group B' end as LegNo,Case when c.billtype='R'  then 'Repurchase'  else 'Combo Request' end as RequestType  " & _
        '         "  FROM M_MemTreeRelation as b,M_MemberMaster as M," & _
        '         " RepurchIncome as c Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as a On (a.BillNo =c.BillNo  Or c.BillNo=a.OrderNo) " & _
        '         " and c.Formno=a.Formno Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
        '         "  Group by c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,a.NetPayable,b.Legno,c.BillType,k.KitName) as Temp  where 1=1 " & condition & "" & _
        '         " Order By Year(Temp.BillDate)Desc,Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
        If RbtRequestType.SelectedValue = "T" Then
            If Session("CompID") = 1060 Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
           " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
        " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
        " sum(k.BV) as BV, " & _
        " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
        " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
        " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.billdate as billdate1 " & _
         " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
         " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
         " FROM TrnBillMain) as a    On (a.BillNo =c.BillNo ) " & _
         " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
       " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
         "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
        "  Group by c.rectimestamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
        " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
        " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            ElseIf Session("CompID") = 1081 Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
                           " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
                        " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
                        " sum(k.BV) as BV, " & _
                        " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
                        " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
                        " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.billdate as billdate1 " & _
                         " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
                         " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
                         " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
                         " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
                       " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
                         "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
                        "  Group by c.rectimestamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
                        " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
                        " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            ElseIf Session("CompID") = 1057 Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
                           " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
                        " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
                        " sum(k.BV) as BV, " & _
                        " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
                        " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
                        " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.billdate as billdate1 " & _
                         " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
                         " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
                         " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
                         " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
                       " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
                         "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
                        "  Group by c.rectimestamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
                        " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
                        " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            ElseIf Session("CompID") = 1084 Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
                           " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
                        " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
                        " sum(k.BV) as BV, " & _
                        " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
                        " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
                        " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.billdate as billdate1 " & _
                         " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
                         " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
                         " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
                         " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
                       " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
                         "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
                        "  Group by c.rectimestamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
                        " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
                        " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            ElseIf Session("CompID") = 1096 Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
                           " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
                        " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
                        " sum(k.BV) as BV, " & _
                        " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
                        " Mlevel as LegNo," & _
                        " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.billdate as billdate1 " & _
                         " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
                         " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
                         " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
                         " and c.Formno=a.Formno     Left Join M_KitMaster as k On k.Kitid>1 and K.RowStatus='Y'" & _
                       " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
                         "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
                        "  Group by c.rectimestamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
                        " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
                        " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            Else
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
                           " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
                        " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
                        " sum(k.BV) as BV, " & _
                        " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
                        " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
                        " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.billdate as billdate1 " & _
                         " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
                         " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
                         " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
                         " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
                       " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
                         "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
                        "  Group by c.rectimestamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
                        " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
                        " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            End If
            
            '     s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
            '    " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
            ' " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
            ' " sum(c.PVValue) as BV, " & _
            ' " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
            ' " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
            ' " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.billdate as billdate1 " & _
            '  " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
            '  " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
            '  " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
            '  " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
            '" Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
            '  "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
            ' "  Group by c.rectimestamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
            ' " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
            ' " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "


        Else
            If Session("CompID") = 1060 Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
          " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
       " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
       " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
       " Mlevel as LegNo," & _
       " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.Billdate as Billdate1 " & _
        " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
        " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
        " FROM TrnBillMain) as a    On (a.BillNo =c.BillNo ) " & _
        " and c.Formno=a.Formno    " & _
      " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
        "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "  and b.Mlevel<=10" & _
       "  Group by c.RectimeStamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
       " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
       " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            ElseIf Session("CompID") = "1081" Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
          " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
       " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
       " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
       " Mlevel as LegNo," & _
       " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.Billdate as Billdate1 " & _
        " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
        " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
        " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
        " and c.Formno=a.Formno    " & _
      " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
        "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "  and b.Mlevel<=10" & _
       "  Group by c.RectimeStamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
       " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
       " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            ElseIf Session("CompID") = "1057" Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
          " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
       " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
       " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
       " Mlevel as LegNo," & _
       " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.Billdate as Billdate1 " & _
        " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
        " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
        " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
        " and c.Formno=a.Formno    " & _
      " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
        "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "  and b.Mlevel<=10" & _
       "  Group by c.RectimeStamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
       " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
       " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "

            ElseIf Session("CompID") = "1084" Then
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
          " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
       " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
       " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
       " Mlevel as LegNo," & _
       " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.Billdate as Billdate1 " & _
        " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
        " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
        " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
        " and c.Formno=a.Formno    " & _
      " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
        "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "  and b.Mlevel<=10" & _
       "  Group by c.RectimeStamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
       " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
       " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            Else
                s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
          " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
       " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
       " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
       " Mlevel as LegNo," & _
       " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,c.Billdate as Billdate1 " & _
        " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
        " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
        " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
        " and c.Formno=a.Formno    " & _
      " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
        "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "  and b.Mlevel<=10" & _
       "  Group by c.RectimeStamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
       " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
       " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            End If
           



        End If
        ' Comm = New SqlCommand(s)
        'Comm.Connection = Conn
        'Ad = New SqlDataAdapter(Comm)
        objdal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

        dt = New DataTable
        dt = objdal.GetData(s)
        ' Ad.Fill(dt)
       
        If Session("CompID") = "1096" Then
            If Rbtbsntype.SelectedValue = "R" Then
                LblTotalLeftEP.Text = "0"
                LblTotalRightEP.Text = "0"
                LBlLeftEp.Visible = False
                LblRightEP.Visible = False
                LblTotalLeftEP.Visible = False
                LblTotalRightEP.Visible = False
                LblTotalEP.Visible = False
                lbltotal.Visible = False
                GrdDirects.Columns(5).Visible = False
            Else

                If dt.Rows.Count > 0 Then
                    LblTotalEP.Text = dt.Compute("Sum(BV)", "").ToString
                    BtnExport.Enabled = True
                    LblTotalLeftEP.Text = "0"
                    LblTotalRightEP.Text = "0"
                    LBlLeftEp.Visible = True
                    LblRightEP.Visible = True
                    LblTotalLeftEP.Visible = True
                    LblTotalRightEP.Visible = True

                    ' Dim sum = 

                    If (IsDBNull(Convert.ToString(dt.Compute("SUM(BV)", "LegNo='MLevel'")))) = True Then
                        LblTotalLeftEP.Text = "0"
                    Else
                        LblTotalLeftEP.Text = Convert.ToString(dt.Compute("SUM(BV)", "Legno='MLevel'"))
                        If LblTotalLeftEP.Text = "" Then
                            LblTotalLeftEP.Text = "0"
                        End If

                    End If
                    'If (IsDBNull(Convert.ToString(dt.Compute("SUM(BV)", "LegNo='Group B'")))) = True Then
                    '    LblTotalRightEP.Text = 0
                    'Else
                    '    LblTotalRightEP.Text = Convert.ToString(dt.Compute("SUM(BV)", "LegNo='Group B'"))
                    '    If LblTotalRightEP.Text = "" Then
                    '        LblTotalRightEP.Text = "0"
                    '    End If
                    'End If

                Else
                    BtnExport.Enabled = True
                End If

                GrdDirects.Columns(5).Visible = True

            End If
        Else
            If RbtRequestType.SelectedValue = "R" Then
                LblTotalLeftEP.Text = "0"
                LblTotalRightEP.Text = "0"
                LBlLeftEp.Visible = False
                LblRightEP.Visible = False
                LblTotalLeftEP.Visible = False
                LblTotalRightEP.Visible = False
                LblTotalEP.Visible = False
                lbltotal.Visible = False
                GrdDirects.Columns(5).Visible = False
            Else

                If dt.Rows.Count > 0 Then
                    LblTotalEP.Text = dt.Compute("Sum(BV)", "").ToString
                    BtnExport.Enabled = True
                    LblTotalLeftEP.Text = "0"
                    LblTotalRightEP.Text = "0"
                    LBlLeftEp.Visible = True
                    LblRightEP.Visible = True
                    LblTotalLeftEP.Visible = True
                    LblTotalRightEP.Visible = True

                    ' Dim sum = 

                    If (IsDBNull(Convert.ToString(dt.Compute("SUM(BV)", "LegNo='Group A'")))) = True Then
                        LblTotalLeftEP.Text = "0"
                    Else
                        LblTotalLeftEP.Text = Convert.ToString(dt.Compute("SUM(BV)", "Legno='Group A'"))
                        If LblTotalLeftEP.Text = "" Then
                            LblTotalLeftEP.Text = "0"
                        End If

                    End If
                    If (IsDBNull(Convert.ToString(dt.Compute("SUM(BV)", "LegNo='Group B'")))) = True Then
                        LblTotalRightEP.Text = 0
                    Else
                        LblTotalRightEP.Text = Convert.ToString(dt.Compute("SUM(BV)", "LegNo='Group B'"))
                        If LblTotalRightEP.Text = "" Then
                            LblTotalRightEP.Text = "0"
                        End If
                    End If

                Else
                    BtnExport.Enabled = True
                End If

                GrdDirects.Columns(5).Visible = True

            End If
        End If


        Session("DataLeftPurchase") = dt
        GrdDirects.DataSource = dt
        GrdDirects.DataBind()
    End Sub

    Private Function get_FormNo(ByVal IDNo As String) As String
        Conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        Conn.Open()
        Dim FormNo As String = ""
        Dim dr As SqlDataReader
        Comm = New SqlCommand("Select FormNo From M_MemberMaster Where IDNo='" & IDNo & "'", Conn)
        dr = Comm.ExecuteReader
        If dr.Read = True Then
            IDNo = dr("FormNo")
        End If
        dr.Close()
        Comm.Cancel()
        Conn.Close()
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
            Dim condition As String = ""
            Dim Formno As String = get_FormNo(Trim(txtMemberId.Text))
            If Val(Formno) = 0 Then
                scrname = "<SCRIPT language='javascript'>alert('Please Check ID No.');" & "</SCRIPT>"
                Me.RegisterStartupScript("MyAlert", scrname)
                txtMemberId.Focus()
                Exit Sub
            End If
            If RbtRequestType.SelectedValue = "T" Then
                If RbtLeg.SelectedValue <> 0 Then
                    condition3 = condition3 & "AND B.LegNo='" & RbtLeg.SelectedValue & "'"

                End If
            Else
                If ddlLevel.SelectedValue > 0 Then
                    condition3 = condition3 & " And b.Mlevel='" & ddlLevel.SelectedValue & "'"
                End If


            End If
            
            If ChkDate.Checked Then
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


                        condition = condition & " And Cast(Replace(Convert(Varchar,temp.BillDate1,106),' ','-') as Date)>='" & FrmDate & "' And Cast(Replace(Convert(Varchar,Temp.BillDate1,106),' ','-') as Date)<='" & ToDate & "'"
                    End If
                End If

            End If

            If Session("CompID") = "1096" Then
                If Rbtbsntype.SelectedValue = "R" Then
                    condition = condition & " and temp.RequestType='Repurchase'"
                End If
            Else
                If RbtRequestType.SelectedValue = "R" Then

                    condition = condition & " and temp.RequestType='Repurchase'"
                ElseIf RbtRequestType.SelectedValue = "T" Then
                    condition = condition & " and temp.requestType<>'Repurchase'"
                End If
            End If

            'condition1 = " And a.Doj>='" & FrmDate & "' And a.Doj<='" & ToDate & "'"
            'condition2 = " And a.UpgradeDate>='" & FrmDate & "' And a.UpgradeDate<='" & ToDate & "'"

            Dim s As String
            's = "select temp.* from (Select  Replace(Convert(Varchar,c.BillDate,106),' ' ,'-') as BillDate,m.idNo,(m.MemFirstName+ +M.MemLastName)as  " & _
            '            " MemberName,b.MLevel,k.KitName as PackageName,Isnull(a.NetPayable,0) as Amount,sum(c.RepurchIncome) as Bv,Case when c.BillType='R' then 0 else  Isnull(Sum(c.Sp),0) end as SP,Case when b.LegNo=1 Then 'Group A' else  " & _
            '         " 'Group B' end as LegNo,Case when c.billtype='R'  then 'Repurchase'  else 'Combo Request' end as RequestType  " & _
            '         "  FROM M_MemTreeRelation as b,M_MemberMaster as M," & _
            '         " RepurchIncome as c Left Join " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain as a On (a.BillNo =c.BillNo  Or c.BillNo=a.OrderNo) " & _
            '         " and c.Formno=a.Formno Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
            '         "  Group by c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,a.NetPayable,b.Legno,c.BillType,k.KitName) as Temp  where 1=1 " & condition & "" & _
            '         " Order By Year(Temp.BillDate)Desc,Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
            If RbtRequestType.SelectedValue = "T" Then
                If Session("CompID") = "1081" Then
                    s = "select temp.Billdate,Idno,MemberName,Legno as GroupName,PackageName,Amount as BillAMount,BV as EP,RequestType as Status " & _
                " from (Select  REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
               " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
            " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
            " sum(K.BV) as Bv, " & _
            " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
            " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
            " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,C.Billdate as BillDate1 " & _
             " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
             " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
             " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
             " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
           " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
             "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
            "  Group by c.BillDate,c.RectimeStamp, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
            " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
            " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
                ElseIf Session("CompID") = "1057" Then
                    s = "select temp.Billdate,Idno,MemberName,Legno as GroupName,PackageName,Amount as BillAMount,BV As PV,RequestType as Status " & _
                " from (Select  REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
               " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
            " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
            " sum(K.BV) as Bv, " & _
            " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
            " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
            " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,C.Billdate as BillDate1 " & _
             " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
             " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
             " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
             " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
           " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
             "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
            "  Group by c.BillDate,c.RectimeStamp, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
            " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
            " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "

                ElseIf Session("CompID") = "1084" Then
                    s = "select temp.Billdate,Idno,MemberName,Legno as GroupName,PackageName,Amount as BillAMount,BV As PV,RequestType as Status " & _
                " from (Select  REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
               " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
            " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
            " sum(K.BV) as Bv, " & _
            " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
            " Case when b.LegNo=1 Then 'Left' else 'Right' end as LegNo," & _
            " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,C.Billdate as BillDate1 " & _
             " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
             " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
             " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
             " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
           " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
             "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
            "  Group by c.BillDate,c.RectimeStamp, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
            " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
            " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
                ElseIf Session("CompID") = 1096 Then
                    s = "select temp.* from (Select REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
                               " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
                            " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
                            " sum(k.BV) as BV, " & _
                            " Mlevel as LegNo," & _
                            " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType " & _
                             " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
                             " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
                             " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
                             " and c.Formno=a.Formno     Left Join M_KitMaster as k On k.Kitid>1 and K.RowStatus='Y'" & _
                           " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
                             "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
                            "  Group by c.rectimestamp,c.BillDate, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
                            " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
                            " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
                Else
                    s = "select temp.Billdate,Idno,MemberName,Legno as GroupName,PackageName,Amount as BillAMount,BV as EP,RequestType as Status " & _
                " from (Select  REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
               " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,k.KitName as PackageName, " & _
            " Case when Cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount," & _
            " sum(c.pvvALUE) as Bv, " & _
            " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
            " Case when b.LegNo=1 Then 'Group A' else 'Group B' end as LegNo," & _
            " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,C.Billdate as BillDate1 " & _
             " FROM M_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
             " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
             " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
             " and c.Formno=a.Formno     Left Join M_KitMaster as k On c.KitId=k.KitId and K.RowStatus='Y'" & _
           " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
             "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & "" & _
            "  Group by c.BillDate,c.RectimeStamp, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo, " & _
            " a.NetPayable,b.Legno,c.BillType,k.KitName,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
            " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
                End If
                


            Else

                If Session("CompId") = "1081" Then
                    s = "select temp.Billdate,Idno,MemberName,Mlevel as Level,Amount as BillAMount,Repurchincome as EP,RequestType as Status  from ( " & _
               " Select  REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
              " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
           " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
           " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
           " Mlevel as LegNo," & _
           " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,C.Billdate as Billdate1 " & _
            " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
            " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
            " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
            " and c.Formno=a.Formno    " & _
          " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
            "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & " and b.Mlevel<=10" & _
           "  Group by c.BillDate,C.RectimeStamp, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
           " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
           " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "
                ElseIf Session("CompID") = "1057" Then
                    s = "select temp.Billdate,Idno,MemberName,Mlevel as Level,Amount as BillAMount,Repurchincome as PV,RequestType as Status  from ( " & _
               " Select  REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
              " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
           " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
           " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
           " Mlevel as LegNo," & _
           " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,C.Billdate as Billdate1 " & _
            " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
            " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
            " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
            " and c.Formno=a.Formno    " & _
          " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
            "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & " and b.Mlevel<=10" & _
           "  Group by c.BillDate,C.RectimeStamp, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
           " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
           " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "

                ElseIf Session("CompID") = "1084" Then
                    s = "select temp.Billdate,Idno,MemberName,Mlevel as Level,Amount as BillAMount,Repurchincome as PV,RequestType as Status  from ( " & _
               " Select  REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
              " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
           " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
           " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
           " Mlevel as LegNo," & _
           " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,C.Billdate as Billdate1 " & _
            " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
            " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
            " FROM TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
            " and c.Formno=a.Formno    " & _
          " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
            "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & " and b.Mlevel<=10" & _
           "  Group by c.BillDate,C.RectimeStamp, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
           " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
           " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "

                Else
                    s = "select temp.Billdate,Idno,MemberName,Mlevel as Level,Amount as BillAMount,BV as EP,RequestType as Status  from ( " & _
               " Select  REPLACE(CONVERT(VARCHAR(11), c.RecTimeStamp , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,c.RecTimeStamp ,100 ) ,7), 6, 0, ' ') as BillDate" & _
              " ,m.idNo,(m.MemFirstName+ +M.MemLastName)as   MemberName,b.MLevel,'' as PackageName, " & _
           " Case when cast(t.Orderno as Varchar)<>'' then Isnull(t.OrderAmt,0) else Isnull(a.NetPayable,0)end  as Amount,sum(c.RepurchIncome) as Bv, " & _
           " Case when c.BillType in  ('R','S') then 0 else  Isnull(Sum(c.Pvvalue),0) end as TP," & _
           " Mlevel as LegNo," & _
           " Case when c.billtype in  ('R','S')  then 'Repurchase'  else 'Joining' end as RequestType,C.Billdate as Billdate1 " & _
            " FROM R_MemTreeRelation as b,M_MemberMaster as M, RepurchIncome as c Left Join " & _
            " (Select ROW_NUMBER() Over(PARTITION BY OrderNo Order BY Formno) as Rwno, Orderno ,BillNo,formno,Netpayable" & _
            " FROM " & HttpContext.Current.Session("InvDatabase" & Session("CompID")) & "..TrnBillMain)    as a    On (a.BillNo =c.BillNo ) " & _
            " and c.Formno=a.Formno    " & _
          " Left Join Trnorder as t On Replace(Cast(t.ORderNO as Varchar),'Order','')=Cast(c.BillNo as Varchar) and c.Formno=t.Formno" & _
            "  WHERE b.FormNoDwn = c.FormNo And c.Formno = m.Formno And b.FormNo ='" & Val(Formno) & "'  " & condition3 & " and b.Mlevel<=10" & _
           "  Group by c.BillDate,C.RectimeStamp, M.Idno,M.MemFirstName,M.MemLastName,b.MLevel ,c.RepurchIncome,t.OrderNo ," & _
           " a.NetPayable,b.Legno,c.BillType,a.Orderno,t.OrderAmt) as Temp  where 1=1 " & condition & " Order By Year(Temp.BillDate)Desc," & _
           " Month(Temp.BillDate)Desc,Day(temp.BillDate)Desc ,Mlevel "


                End If
               

            End If
            objdal = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

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
        FillLevel()
    End Sub

    Protected Sub RbtRequestType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtRequestType.SelectedIndexChanged
        If Session("CompID") = "1096" Then
            'If RbtRequestType.SelectedValue = "T" Then
            '    LblType.Text = "leg"
            '    ddlLevel.Visible = False
            '    RbtLeg.Visible = True
            'Else
            'FillLevel()
            LblType.Text = "Level"
            ddlLevel.Visible = True
            RbtLeg.Visible = False

            'End If
        Else
        If RbtRequestType.SelectedValue = "T" Then
            LblType.Text = "leg"
            ddlLevel.Visible = False
            RbtLeg.Visible = True
        Else
            'FillLevel()
            LblType.Text = "Level"
            ddlLevel.Visible = True
            RbtLeg.Visible = False

        End If
        End If
       
        LblTotalEP.Text = "0"
        GrdDirects.DataSource = Nothing
        GrdDirects.DataBind()
    End Sub
    Protected Sub Rbtbsntype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Rbtbsntype.SelectedIndexChanged
        If Session("CompID") = "1096" Then
            If RbtRequestType.SelectedValue = "T" Then
                LblType.Text = "leg"
                ddlLevel.Visible = False
                RbtLeg.Visible = True
            Else
                'FillLevel()
                LblType.Text = "Level"
                ddlLevel.Visible = True
                RbtLeg.Visible = False

            End If
        End If

            LblTotalEP.Text = "0"
            GrdDirects.DataSource = Nothing
            GrdDirects.DataBind()
    End Sub
End Class
