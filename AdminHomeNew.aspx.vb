Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class App_UI_Application_Pages_AdminHomeNew
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("AStatus") = "OK" And Session("UserPermission") = 1 Then
            Session("PageName") = "Home"

        Else
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        If Not Page.IsPostBack Then
            'BindData()
            'BindDataSummary()

            BindData()
        End If
    End Sub
    Private Function Base64Encode(ByVal plainText As String) As String
        Dim plainTextBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(plainText)
        Return Convert.ToBase64String(plainTextBytes)
    End Function
    Public Sub BindData(Optional ByVal Condition As String = "")

        Dim url As String = String.Empty

        Dim tempProtocol As String = HttpContext.Current.Request.Url.AbsoluteUri
        If Session("Compid") = "1093" Then
            url = IIf(tempProtocol.StartsWith("http://"), "https://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "NETWORK.")
        Else
            url = IIf(tempProtocol.StartsWith("http://"), "https://", "https://") & HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("BASICMLM.", "").Replace("ADMIN.", "CPANEL.").Replace("ADMIN.", "CPANEL.")
        End If
        url = url.ToLower

        Dim sql As String = ""
        If Session("CompID") = "1074" Then
            sql = "Select top 500  A.IDNo,  A.Prefix +' '+ A.MemFirstName + ' '+ A.MemLastName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'https://login.cashlessbazar.in' as Site," & _
        " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
        "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode,'' as AadharNo,'' as acno From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
        ElseIf Session("CompID") = "1007" Then
            sql = "Select top 500  A.IDNo,  A.Prefix +' '+ A.MemFirstName + ' '+ A.MemLastName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
        " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
        "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode,A.AadharNo,a.acno as acno From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
        ElseIf Session("CompID") = "1092" Then
            sql = "Select top 500  A.IDNo,  A.MemFirstName + ' '+ A.MemLastName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'https://login.a7kart.com' as Site," & _
       " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
       "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,case when A.presenterid='0' then'0' else  CONCAT('AM', A.presenterid)  end as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode,'' as AadharNo,'' as acno From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
        ElseIf Session("CompID") = "1091" Then
            sql = "Select top 500  A.IDNo,  A.Prefix +' '+ A.MemFirstName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
        " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
        "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode,'' as Aadharno,'' as acno From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"

        ElseIf Session("CompID") <> "1025" Then
            sql = "Select top 500  A.IDNo,  A.Prefix +' '+ A.MemFirstName + ' '+ A.MemLastName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
        " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
        "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
"     CASE WHEN f.ActiveStatusA='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), f.UpgradeDateA , 106), ' ', '-') +''+STUFF(RIGHT( CONVERT(VARCHAR,f.UpgradeDateA ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgradeDateA," & _
" D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode,'' as Aadharno,'' as acno,k.KitName as [KitNamePlanA] ,k.KitAmount AS [KitAmountPlanA] From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID inner join M_MemberMaster as f  On A.FormNo=f.FormNo inner join M_kitMaster as k  On K.KitID=F.KitIDA  " & Condition & " Order by A.Doj Desc"


        ElseIf Session("CompID") <> "1067" Then
            sql = "Select top 500  A.IDNo,A.Prefix +' '+ A.MemFirstName + ' '+ A.MemLastName As Name1,D.KitName,D.KitAmount,D.Bv,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
    " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
    "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,cm.CountryName,'true' as CountryVisible,cm.stdcode,'' as Aadharno,'' as acno From M_MemberMaster As A with(nolock) Inner Join M_StateDivMaster As B with(nolock) " & _
" on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo " & _
" Left Join M_MemberMaster As e with(nolock) On A.UpLnFormNo=e.Formno " & _
" Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID " & _
"left join M_countrymaster as cm with(nolock) on cm.cid=a.CountryId and cm.Rowstatus='Y'" & Condition & " Order by A.Doj Desc"
        Else
            sql = "Select top 500  A.IDNo,A.Prefix +' '+ A.MemFirstName + ' '+ A.MemLastName As Name1,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" & url & "' as Site," & _
     " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
     "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,cm.CountryName,'true' as CountryVisible,cm.stdcode,'' as Aadharno,'' as acno From M_MemberMaster As A with(nolock) Inner Join M_StateDivMaster As B with(nolock) " & _
" on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo " & _
" Left Join M_MemberMaster As e with(nolock) On A.UpLnFormNo=e.Formno " & _
" Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID " & _
"left join M_countrymaster as cm with(nolock) on cm.cid=a.CountryId and cm.Rowstatus='Y'" & Condition & " Order by A.Doj Desc"
        End If
        dtData = New DataTable
        dtData = objDAL.GetData(sql)
        For Each Dr As DataRow In dtData.Rows
            If Session("compid") = "1102" Then
                Dim ref11 As String = "Login"
                Dim info1 As String = Dr("IDNo").ToString() & ";" & Dr("Passw").ToString()
                Dim red As String = Base64Encode(ref11)
                Dim ww As String = Base64Encode(info1)
                Dr("LgnID") = "https://makeandgrowth.com/Account/Directlogin?refs=" & red & "&info=" & ww
                'ElseIf Session("compid") = "1106" Then
                '    Dim ref11 As String = "Login"
                '    Dim info1 As String = Dr("IDNo").ToString() & ";" & Dr("Passw").ToString()
                '    Dim red As String = Base64Encode(ref11)
                '    Dim ww As String = Base64Encode(info1)
                '    Dr("LgnID") = "https://myhemalika.com/Account/Directlogin?refs=" & red & "&info=" & ww
            Else
                Dr("LgnID") = Crypto.Encrypt("uid=" & Dr("IDNo") & "&pwd=" & Dr("Passw"))
            End If
        Next
        GvData.DataSource = dtData
        GvData.DataBind()
      
        If Session("compid") = "1025" Then
            GvData.Columns(14).Visible = True
            GvData.Columns(15).Visible = True
            GvData.Columns(5).Visible = False
        Else
            GvData.Columns(14).Visible = False
            GvData.Columns(16).Visible = False
            GvData.Columns(5).Visible = False
        End If
        If Session("compid") = "1095" Then
            GvData.Columns(14).Visible = True
        Else
            GvData.Columns(14).Visible = False

        End If
        If Session("CompID") = "1067" Then
            GvData.Columns(11).Visible = False
            GvData.Columns(10).Visible = False
            'GvData.Columns(7).Visible = False
            GvData.Columns(5).Visible = False
        Else
            GvData.Columns(10).Visible = True
            GvData.Columns(11).Visible = True
            GvData.Columns(5).Visible = False
        End If
        If Session("CompID") = "1091" Then
            GvData.Columns(12).Visible = False
        Else
            GvData.Columns(12).Visible = True
        End If
        If Session("Compid") = "1055" Then
            'GvData.Columns(11).Visible = False
            GvData.Columns(5).Visible = False
        End If
        If Session("Compid") = "1041" Then
            GvData.Columns(11).Visible = False
            GvData.Columns(5).Visible = False
        End If
        If Session("Compid") = "1110" Then
            GvData.Columns(11).Visible = False
        End If
        If Session("Compid") = "1066" Then
            'GvData.Columns(8).Visible = False
            GvData.Columns(9).Visible = False
            GvData.Columns(10).Visible = False
            GvData.Columns(11).Visible = False
            GvData.Columns(12).Visible = False
            GvData.Columns(15).Visible = False
            GvData.Columns(5).Visible = True
        Else
            GvData.Columns(5).Visible = False
        End If
        If Session("CompId") = "1007" Then
            GvData.Columns(14).Visible = True
            'GvData.Columns(16).Visible = False
            GvData.Columns(5).Visible = False
            GvData.Columns(10).Visible = True
            GvData.Columns(11).Visible = True
            GvData.Columns(15).Visible = False
            GvData.Columns(22).Visible = True
            GvData.Columns(23).Visible = True
        End If
        If Session("CompId") = "1090" Then
            GvData.Columns(14).Visible = True
            'GvData.Columns(16).Visible = False
            GvData.Columns(5).Visible = False
            GvData.Columns(10).Visible = True
            GvData.Columns(11).Visible = True
            GvData.Columns(15).Visible = False
            GvData.Columns(22).Visible = True
        End If

        Session("MemberData") = dtData
        ViewState("Idno") = "Idno"
        ViewState("Sort_Order") = "ASC"
        If dtData.Rows.Count > 0 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub
    Protected Sub GvData_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles GvData.PreRender
        If GvData.Columns.Count > 13 Then
            Dim tf As TemplateField = CType(GvData.Columns(13), TemplateField)

            If Session("compid") IsNot Nothing AndAlso Session("compid").ToString() = "1107" Then
                tf.HeaderText = "Package EV"
            Else
                tf.HeaderText = "Package BV"
            End If
        End If
    End Sub
    Protected Sub GvData_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GvData.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim lbl As Label = CType(e.Row.FindControl("lblHeader"), Label)
            If lbl IsNot Nothing Then
                lbl.Text = If(Session("compid").ToString() = "1107", "Package EV", "Package BV")
            End If
        End If
    End Sub
    Protected Sub GvData_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvData.PageIndexChanging
        GvData.PageIndex = e.NewPageIndex
        GvData.DataSource = Session("MemberData")
        GvData.DataBind()
    End Sub
    Protected Sub Gvdata_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        If e.SortExpression = ViewState("Idno").ToString() Then
            If ViewState("Sort_Order").ToString() = "ASC" Then
                RebindData(e.SortExpression, "DESC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "DESC"
                    ' Dim lbText As String = DirectCast(GvData.HeaderRow.Cells(i).Controls(0), LinkButton).Text
                    If lbText = ViewState("Sort_Order").ToString() Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        'Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/Upaarrow.png", "~/Images/DownArrow.png")
                        'tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        'tableCell.Controls.Add(img)
                    End If
                Next
            Else
                RebindData(e.SortExpression, "ASC")
                For i As Integer = 0 To GvData.Columns.Count - 1
                    Dim lbText As String = "ASC"
                    If lbText = e.SortExpression Then
                        Dim tableCell As TableCell = GvData.HeaderRow.Cells(i)
                        'Dim img As New Image()
                        'img.ImageUrl = If((ViewState("Sort_Order").ToString() = "Asc"), "~/Images/Upaarrow.png", "~/Images/DownArrow.png")
                        'tableCell.Controls.Add(New LiteralControl("&nbsp;"))
                        'tableCell.Controls.Add(img)
                    End If
                Next
            End If

        Else
            RebindData(e.SortExpression, "ASC")
        End If
    End Sub
    Private Sub RebindData(ByVal sColimnName As String, ByVal sSortOrder As String)
        Dim dt As DataTable = CType(Session("MemberData"), DataTable)
        dt.DefaultView.Sort = sColimnName + " " + sSortOrder
        GvData.DataSource = dt
        GvData.DataBind()

        ViewState("Idno") = sColimnName
        ViewState("Sort_Order") = sSortOrder
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim Condition As String = ""
        If Session("CompID") = 1007 Then
            If ddlSearch1.SelectedValue = "0" Then
                Exit Sub
            ElseIf ddlSearch1.SelectedValue = "StateName" Then
                Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch1.SelectedValue = "DOJ" Then
                If Session("CompId") = "1055" Then
                    Condition = " Where Replace(Convert(varchar,Case when Cast(a.DOJ as Date)>='01-Jan-2022' then a.DOj else '01-Jan-2022' end ,106),' ','-') like '%" & txtSrchText.Text & "%'"

                Else
                    Condition = " Where Replace(Convert(varchar, a.Doj,106),' ','-') like '%" & txtSrchText.Text & "%'"

                End If
                '     Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch1.SelectedValue = "MemName" Then
                Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
            ElseIf ddlSearch1.SelectedValue = "RMemName" Then
                Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
            ElseIf ddlSearch1.SelectedValue = "KitName" Then
                Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch1.SelectedValue = "RMemID" Then
                Condition = " Where e.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch1.SelectedValue = "IDNo" Then
                Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch1.SelectedValue = "Panno" Then
                Condition = " Where a.Panno = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch1.SelectedValue = "Aadharno" Then
                Condition = " Where a.Aadharno = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch1.SelectedValue = "acno" Then
                Condition = " Where a.acno = '" & txtSrchText.Text & "' "
            Else
                Condition = " Where a." & ddlSearch1.SelectedValue & " like '%" & txtSrchText.Text & "%'"
            End If
        Else
            If ddlSearch.SelectedValue = "0" Then
                Exit Sub
            ElseIf ddlSearch.SelectedValue = "StateName" Then
                Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch.SelectedValue = "DOJ" Then
                If Session("CompId") = "1055" Then
                    Condition = " Where Replace(Convert(varchar,Case when Cast(a.DOJ as Date)>='01-Jan-2022' then a.DOj else '01-Jan-2022' end ,106),' ','-') like '%" & txtSrchText.Text & "%'"

                Else
                    Condition = " Where Replace(Convert(varchar, a.Doj,106),' ','-') like '%" & txtSrchText.Text & "%'"

                End If
                '     Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch.SelectedValue = "MemName" Then
                Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
            ElseIf ddlSearch.SelectedValue = "RMemName" Then
                Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
            ElseIf ddlSearch.SelectedValue = "KitName" Then
                Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch.SelectedValue = "RMemID" Then
                Condition = " Where e.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch.SelectedValue = "IDNo" Then
                Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch.SelectedValue = "Panno" Then
                Condition = " Where a.Panno = '" & txtSrchText.Text & "' "

            Else

                Condition = " Where a." & ddlSearch.SelectedValue & " like '%" & txtSrchText.Text & "%'"


            End If
        End If

        BindData(Condition)
    End Sub

    Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
        Me.BindData(1)
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Exportdata()
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

    Public Sub Exportdata()
        Dim Condition As String = ""
        If Session("CompID") = 1007 Then
            If ddlSearch1.SelectedValue = "0" Then
                Condition = ""

            ElseIf ddlSearch1.SelectedValue = "StateName" Then
                Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch1.SelectedValue = "DOJ" Then
                If Session("CompId") = "1055" Then
                    Condition = " Where Replace(Convert(varchar,Case when Cast(a.DOJ as Date)>='01-Jan-2022' then a.DOj else '01-Jan-2022' end ,106),' ','-') like '%" & txtSrchText.Text & "%'"

                Else
                    Condition = " Where Replace(Convert(varchar, a.Doj,106),' ','-') like '%" & txtSrchText.Text & "%'"

                End If
                '   Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch1.SelectedValue = "MemName" Then
                Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
            ElseIf ddlSearch1.SelectedValue = "RMemName" Then
                Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
            ElseIf ddlSearch1.SelectedValue = "KitName" Then
                Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch1.SelectedValue = "RMemID" Then
                Condition = " Where e.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch1.SelectedValue = "IDNo" Then
                Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch1.SelectedValue = "Panno" Then
                Condition = " Where a.Panno = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch1.SelectedValue = "Aadharno" Then
                Condition = " Where a.Aadharno = '" & txtSrchText.Text & "' "

            Else

                Condition = " Where a." & ddlSearch1.SelectedValue & " like '%" & txtSrchText.Text & "%'"

            End If

        Else
            If ddlSearch.SelectedValue = "0" Then
                Condition = ""

            ElseIf ddlSearch.SelectedValue = "StateName" Then
                Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch.SelectedValue = "DOJ" Then
                If Session("CompId") = "1055" Then
                    Condition = " Where Replace(Convert(varchar,Case when Cast(a.DOJ as Date)>='01-Jan-2022' then a.DOj else '01-Jan-2022' end ,106),' ','-') like '%" & txtSrchText.Text & "%'"

                Else
                    Condition = " Where Replace(Convert(varchar, a.Doj,106),' ','-') like '%" & txtSrchText.Text & "%'"

                End If
                '   Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch.SelectedValue = "MemName" Then
                Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
            ElseIf ddlSearch.SelectedValue = "RMemName" Then
                Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
            ElseIf ddlSearch.SelectedValue = "KitName" Then
                Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
            ElseIf ddlSearch.SelectedValue = "RMemID" Then
                Condition = " Where e.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch.SelectedValue = "IDNo" Then
                Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
            ElseIf ddlSearch.SelectedValue = "Panno" Then
                Condition = " Where a.Panno = '" & txtSrchText.Text & "' "


            Else

                Condition = " Where a." & ddlSearch.SelectedValue & " like '%" & txtSrchText.Text & "%'"
            End If
        End If

        Dim dtTemp As New DataTable
        Dim dg As New DataGrid
        Try
            Dim sql As String = ""
            If Session("CompId") = "1041" Then
                sql = "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName," & _
                          " A.Email,A.Mobl As MobileNo,D.KitName as packageName,D.KitAmount as PackageMRP,IsNull(REPLACE(CONVERT(VARCHAR, a.Doj , 106), ' ', '-'),' ') as DojDate," & _
               " STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime,A.City,B.StateName,A.Passw, " & _
               " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as UpgrdDate," & _
               " CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' " & _
               " and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus From M_MemberMaster As A with(nolock) " & _
               " Inner Join M_StateDivMaster As B with(nolock) on A.StateCode=B.StateCode and b.RowStatus='Y' Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e " & _
               " On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
            ElseIf Session("CompID") = "1007" Then
                sql = "Select top 500  A.IDNo,  A.Prefix +' '+ A.MemFirstName + ' '+ A.MemLastName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID," & _
            " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
            "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
    " CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
    " ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
    " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
    " D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode,''''+A.AadharNo as [Aadhar No] From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
    " Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"

            ElseIf Session("CompId") = "1025" Then
                sql = "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName," & _
                          " A.Email,A.Mobl As MobileNo,D.KitName as packageName,D.KitAmount as PackageMRP,D.Bv as PackageBv,IsNull(REPLACE(CONVERT(VARCHAR, a.Doj , 106), ' ', '-'),' ') as DojDate," & _
               " STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime,A.City,B.StateName,cm.countryName,cm.stdCode,A.Passw, " & _
               " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as UpgrdDate," & _
               " CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' " & _
               " and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status,'Rs 0.00/-' As Balance,a.ActiveStatus From M_MemberMaster As A with(nolock) " & _
               " Inner Join M_StateDivMaster As B with(nolock) on A.StateCode=B.StateCode and b.RowStatus='Y' Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo " & _
               " Left Join M_MemberMaster As e with(nolock)" & _
               " On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID " & _
               "left join M_countrymaster as cm with(nolock) on cm.cid=a.CountryId and cm.Rowstatus='Y'" & Condition & " Order by A.Doj Desc"
            ElseIf Session("CompId") = "1055" Then
                sql = "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName," & _
                                         " A.Email,A.Mobl As MobileNo,D.KitName as packageName,D.KitAmount as PackageMRP,D.Bv as PackageBv,IsNull(REPLACE(CONVERT(VARCHAR, Case when Cast(a.Doj as Date)>='01-Jan-2022' then a.Doj else '01-Jan-2022' end , 106), ' ', '-'),' ') as DojDate," & _
                              " STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime,A.City,B.StateName,A.Passw, " & _
                              " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as UpgrdDate," & _
                              " CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' " & _
                              " and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status,'Rs 0.00/-' As Balance,a.ActiveStatus,a.Panno From M_MemberMaster As A with(nolock) " & _
                              " Inner Join M_StateDivMaster As B with(nolock) on A.StateCode=B.StateCode and b.RowStatus='Y' Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e " & _
                              " On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
            ElseIf Session("CompID") = "1070" Then
                sql = "Select top 500  A.IDNo,  A.Prefix +' '+ A.MemFirstName + ' '+ A.MemLastName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'' as Site," & _
        " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
        "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
" D.KitName,D.KitAmount,R.BV as BV,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode From M_MemberMaster As A " & _
"Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo " & _
"Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno " & _
" Inner join M_kitMaster As D On A.KitID=D.KitID" & _
" Left join V#GetRepuricome As R On a.formno=R.formno " & _
" " & Condition & " Order by A.Doj Desc"
                '            sql = "Select top 500  A.IDNo,  A.Prefix +' '+ A.MemFirstName + ' '+ A.MemLastName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'' as Site," & _
                '        " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
                '        "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,'' as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
                '" CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
                '" ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
                '" CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
                '" D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'' as CountryName,'false' as CountryVisible,'' as StdCode From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
                '" Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"

            ElseIf Session("CompID") = "1066" Then
                sql = "Select top 500  A.IDNo,  A.MemFirstName + ' '+ A.MemLastName As Name1 ,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'https://login.alkamediindia.in' as Site," & _
           " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo," & _
           "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName,case when A.presenterid='0' then'0' else  CONCAT('AM', A.presenterid)  end as PresenterId, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, " & _
    " CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=""Img.aspx?ID='+ a.IDNO + '"" onclick=""return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )"" >'+ A.MemFirstName +'</a>'" & _
    " ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status," & _
    " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate," & _
    " D.KitName,D.KitAmount,D.Bv,a.panno,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,'false' as CountryVisible,'' as StdCode From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' " & _
    " Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"

            Else


                sql = "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName," & _
                                          " A.Email,A.Mobl As MobileNo,k.KitName as [KitNamePlanA] ," & _
                "CASE WHEN F.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, f.UpgradeDateA , 106), ' ', '-'),'') ELSE '' END as [Activ.Date Plan A]," & _
                 " D.KitName as [packageName Plan B],CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as [UpgrdDate Plan B]," & _
                                "IsNull(REPLACE(CONVERT(VARCHAR, a.Doj , 106), ' ', '-'),' ') as DojDate," & _
                               " STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime,A.City,B.StateName,A.Passw, " & _
                               " CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' " & _
                               " and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status,'Rs 0.00/-' As Balance,a.ActiveStatus,a.Panno From M_MemberMaster As A with(nolock) " & _
                               " Inner Join M_StateDivMaster As B with(nolock) on A.StateCode=B.StateCode and b.RowStatus='Y' Left Join M_MemberMaster As C with(nolock) On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e " & _
                               " On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D with(nolock) On A.KitID=D.KitID inner join M_MemberMaster as f On A.FormNo=f.FormNo  inner join M_kitMaster as k  On K.KitID=F.KitIDA " & Condition & " Order by A.Doj Desc"



            End If

            dtTemp = New DataTable
            dtTemp = objDAL.GetData(sql)
            dg.DataSource = dtTemp
            dg.DataBind()
            If Session("Compid") = "1066" Then
                GvData.Columns(5).Visible = True
            Else
                GvData.Columns(5).Visible = False
            End If
            If Session("CompId") = "1007" Then
                GvData.Columns(14).Visible = True
                'GvData.Columns(16).Visible = False
                GvData.Columns(5).Visible = False
                GvData.Columns(10).Visible = True
                GvData.Columns(11).Visible = True
                GvData.Columns(15).Visible = False
                GvData.Columns(22).Visible = True
            End If
            If Session("CompId") = "1090" Then
                GvData.Columns(14).Visible = True
                'GvData.Columns(16).Visible = False
                GvData.Columns(5).Visible = False
                GvData.Columns(10).Visible = True
                GvData.Columns(11).Visible = True
                GvData.Columns(15).Visible = False
                GvData.Columns(22).Visible = True
            End If
            ExportToExcel("MemberDetail.xls", dg)
        Catch ex As Exception
            Response.Write(ex.Message & "Error In Exporting File")
        End Try

    End Sub


    Protected Sub btnExportCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportCsv.Click
        Dim strQuery As String = ""
        Dim Condition As String = ""
        If ddlSearch.SelectedValue = "0" Then
            Condition = ""

        ElseIf ddlSearch.SelectedValue = "StateName" Then
            Condition = " Where b.StateName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "DOJ" Then
            Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "MemName" Then
            Condition = " Where (a.MemFirstName like '%" & txtSrchText.Text & "%' OR a.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "RMemName" Then
            Condition = " Where (c.MemFirstName like '%" & txtSrchText.Text & "%' OR c.MemFirstName like '%" & txtSrchText.Text & "%') "
        ElseIf ddlSearch.SelectedValue = "KitName" Then
            Condition = " Where d.KitName like '%" & txtSrchText.Text & "%'"
        ElseIf ddlSearch.SelectedValue = "RMemID" Then
            Condition = " Where c.IDNO = '" & txtSrchText.Text & "' "
        ElseIf ddlSearch.SelectedValue = "IDNo" Then
            Condition = " Where a.IDNO = '" & txtSrchText.Text & "' "
        Else
            Condition = " Where a." & ddlSearch.SelectedValue & " like '%" & txtSrchText.Text & "%'"
        End If
        strQuery = "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName,A.Email,A.Mobl As MobileNo,D.KitName as packageName,D.KitAmount as PackageMRP,D.Bv as PackageBv,IsNull(REPLACE(CONVERT(VARCHAR, a.Doj , 106), ' ', '-'),' ') as DojDate,STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime,A.City,B.StateName,A.Passw, " & _
            " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as UpgrdDate,CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' then 'Active' Else 'Pending' End As Status,'Rs 0.00/-' As Balance,a.ActiveStatus From M_MemberMaster As A Inner Join M_StateDivMaster As B on A.StateCode=B.StateCode Left Join M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join M_kitMaster As D On A.KitID=D.KitID " & Condition & " Order by A.Doj Desc"
        Dim dt As DataTable = objDAL.GetData(strQuery)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", _
                "attachment;filename=MemberDetail.csv")
        Response.Charset = ""
        Response.ContentType = "application/text"

        Dim sb As New StringBuilder()
        For k As Integer = 0 To dt.Columns.Count - 1
            'add separator
            sb.Append(dt.Columns(k).ColumnName + ","c)
        Next
        'append new line
        sb.Append(vbCr & vbLf)
        For i As Integer = 0 To dt.Rows.Count - 1
            For k As Integer = 0 To dt.Columns.Count - 1
                'add separator
                sb.Append(dt.Rows(i)(k).ToString().Replace(",", ";") + ","c)
            Next
            'append new line
            sb.Append(vbCr & vbLf)
        Next
        Response.Output.Write(sb.ToString())
        Response.Flush()
        Response.End()
    End Sub
End Class
