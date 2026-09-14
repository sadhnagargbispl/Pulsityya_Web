Imports System.Data.SqlClient
Imports System.Data
Partial Class Referaltree
    Inherits System.Web.UI.Page
    Dim strQuery As String
    Dim minDeptLevel As Integer
    Dim conn As New SqlConnection
    Dim Comm As New SqlCommand
    Dim Adp1 As SqlDataAdapter
    Dim dsGetQry As New DataSet
    Dim strDrawKit As String
    Dim objGen As clsGeneral = New clsGeneral

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("AStatus") = "OK" Then
                Call ValidateTree()
            Else
                Response.Redirect("logout.aspx")
            End If
        End If
    End Sub

    Private Sub ValidateTree()
        Dim strSelectedFormNo As String
        '---- If User Not Set MinDept level than set 10 dept level
        If Request("DeptLevel") = "" Then
            'minDeptLevel = 3
            minDeptLevel = 1
        Else
            minDeptLevel = Request("DeptLevel")
        End If

        ''If (Request("DownLineFormNo") = Session("FormNO")) Or Request("DownLineFormNo") = "" Then
        ''    strSelectedFormNo = Session("FORMNO")
        ''    ''Label1.Text = "form No:" & Session("FORMNO")
        ''Else
        ''    If CheckDownLineMemberTree() = False Then
        ''        Response.Write("Please Check DownLine Member ID")
        ''        Response.End()
        ''    End If
        ''    'Label1.Text = "Not Match :" & Request("DownLineFormNo") & " Fromno :" & Session("FORMNO")
        ''    strSelectedFormNo = Request("DownLineFormNo")
        ''End If

        If Request("DownLineFormNo") <> "" Then
            '    strSelectedFormNo = Request("DownLineFormNo")
            'Else
            'If CheckDownLineMemberTree() = False Then
            '    Response.Write("Please Check DownLine Member ID")
            '    Response.End()
            'End If

            strSelectedFormNo = Request("DownLineFormNo")



            strQuery = getQuery(strSelectedFormNo, minDeptLevel)
            GenerateTree(strQuery)
            'getKits()
        End If
    End Sub

    'Private Sub getKits()
    '    Dim dsKit As New DataSet

    '    conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
    '    conn.Open()
    '    Comm = New SqlCommand("Select * from m_KitMaster where ActiveStatus='Y' order by kitid ", conn)

    '    Adp1 = New SqlDataAdapter(Comm)
    '    Adp1.Fill(dsKit)

    '    If (dsKit.Tables(0).Rows.Count > 0) Then
    '        img11.Visible = True
    '        img11.ImageUrl = "images/" & dsKit.Tables(0).Rows(0)("JoinColor")
    '        td21.InnerText = dsKit.Tables(0).Rows(0)("KitName")

    '        If (dsKit.Tables(0).Rows.Count > 1) Then
    '            img12.Visible = True
    '            img12.ImageUrl = "images/" & dsKit.Tables(0).Rows(1)("JoinColor")
    '            td22.InnerText = dsKit.Tables(0).Rows(1)("KitName")

    '            If (dsKit.Tables(0).Rows.Count > 2) Then
    '                img13.Visible = True
    '                img13.ImageUrl = "images/" & dsKit.Tables(0).Rows(2)("JoinColor")
    '                td23.InnerText = dsKit.Tables(0).Rows(2)("KitName")

    '                If (dsKit.Tables(0).Rows.Count > 3) Then
    '                    img14.Visible = True
    '                    img14.ImageUrl = "images/" & dsKit.Tables(0).Rows(3)("JoinColor")
    '                    td24.InnerText = dsKit.Tables(0).Rows(3)("KitName")

    '                    If (dsKit.Tables(0).Rows.Count > 4) Then
    '                        img15.Visible = True
    '                        img15.ImageUrl = "images/" & dsKit.Tables(0).Rows(4)("JoinColor")
    '                        td25.InnerText = dsKit.Tables(0).Rows(4)("KitName")

    '                        If (dsKit.Tables(0).Rows.Count > 5) Then
    '                            img16.Visible = True
    '                            img16.ImageUrl = "images/" & dsKit.Tables(0).Rows(5)("JoinColor")
    '                            td26.InnerText = dsKit.Tables(0).Rows(5)("KitName")

    '                            If (dsKit.Tables(0).Rows.Count > 6) Then
    '                                img17.Visible = True
    '                                img17.ImageUrl = "images/" & dsKit.Tables(0).Rows(6)("JoinColor")
    '                                td27.InnerText = dsKit.Tables(0).Rows(6)("KitName")
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If
    '    End If

    'End Sub

    Private Function CheckDownLineMemberTree() As Boolean

        CheckDownLineMemberTree = False
        strQuery = " Select FormnoDwn FROM R_MemTreeRelation WHERE  FormNoDwn=" & Request("DownLineFormNo") & " AND  FormNo=" & Session("FORMNO")

        Dim conn As New SqlConnection
        Dim Comm As New SqlCommand
        Dim Adp1 As SqlDataAdapter
        Dim ds1 As New DataSet

        conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        conn.Open()

        Comm = New SqlCommand(strQuery, conn)
        Adp1 = New SqlDataAdapter(Comm)
        Adp1.Fill(ds1)

        If ds1.Tables(0).Rows.Count <= 0 Then
            CheckDownLineMemberTree = False
        Else
            CheckDownLineMemberTree = True
        End If
        ds1.Dispose()
    End Function

    Private Sub GenerateTree(ByVal strQuery As String)
        Dim conn As New SqlConnection
        Dim Comm As New SqlCommand
        Dim Adp1 As SqlDataAdapter
        Dim ds1 As New DataSet
        conn = New SqlConnection(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))
        conn.Open()
        Comm = New SqlCommand(strQuery, conn)
        Adp1 = New SqlDataAdapter(Comm)
        Adp1.Fill(ds1)
        Dim I As Integer
        Dim UpgradeDate As String = ""
        Dim DirctLeftActive As String = ""
        Dim DirectRightActive As String = ""
        Dim ParentId As Integer
        Dim FormNo As Double
        Dim MemberName As String
        Dim LegNo As String
        Dim Doj As String
        Dim Category As String = ""
        Dim LeftBV As Double
        Dim RightBV As Double
        Dim UpLiner As String
        Dim Sponsor As String
        Dim NodeName As String
        Dim myRunTimeString As String = ""
        Dim ExpandYesNo As String
        Dim strImageFile As String = ""
        Dim strUrlPath As String = ""
        Dim tooltipstrig As String
        Dim GroupStatus As String = ""
        myRunTimeString = myRunTimeString + "<Script Language=Javascript>" + vbNewLine

        tooltipstrig = ToolTipTable()

        '--- Define Parent Setting ----------------
        ParentId = -1

        If Request("DownLineFormNo") <> "" Then
            FormNo = CType(Val(Request("DownLineFormNo")), Integer)
            Dim tmpDS As New DataSet
            strQuery = "SELECT M.idno as Sponsorid,M.MemFirstname as Sponsorname,a.*,b.JoinColor,B.kitname as Category,c.Direct,C.Indirect,c.ActiveDirect as DirectActive, c.Activeindirect as IndirectActive FROM m_MemberMaster as a Left Join M_MemberMaster as M on a.Refformno=M.Formno, M_KitMaster as b,V#DI as c " & _
            " WHERE a.KitID=b.KitID AND a.FORMNO=" & FormNo & " and a.Formno=c.Formno"

            Comm = New SqlCommand(strQuery, conn)
            Adp1 = New SqlDataAdapter(Comm)
            Adp1.Fill(tmpDS)
            If tmpDS.Tables(0).Rows.Count > 0 Then
                'MemberName = tmpDS.Tables(0).Rows(0)("MemName").ToString & "<BR> (" & tmpDS.Tables(0).Rows(0)("FormNo").ToString & ")"
                MemberName = tmpDS.Tables(0).Rows(0)("IdNo").ToString
                NodeName = tmpDS.Tables(0).Rows(0)("MemFirstName").ToString
                Sponsor = tmpDS.Tables(0).Rows(0)("Sponsorid").ToString + "(" + tmpDS.Tables(0).Rows(0)("SponsorName").ToString + ")"
                strImageFile = "images/" & tmpDS.Tables(0).Rows(0)("JoinColor").ToString
                Category = tmpDS.Tables(0).Rows(0)("Category").ToString
                LeftBV = tmpDS.Tables(0).Rows(0)("Direct").ToString
                RightBV = tmpDS.Tables(0).Rows(0)("Indirect").ToString
                GroupStatus = ""
                UpgradeDate = tmpDS.Tables(0).Rows(0)("UpgradeDate")

                DirctLeftActive = tmpDS.Tables(0).Rows(0)("DirectActive")
                DirectRightActive = tmpDS.Tables(0).Rows(0)("InDirectActive")
            End If
        Else
            FormNo = Val(Session("FormNo").ToString)
            'MemberName = Session("MemFirstName").ToString & "<BR> (" & Session("FormNo").ToString & ")"
            MemberName = Session("FormNo").ToString
            NodeName = Session("MemName").ToString
        End If

        myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" & strImageFile & "');" + vbNewLine
        myRunTimeString = myRunTimeString + "mytree.add(" & FormNo & "," & ParentId & "," & "'" & Category & "'" & "," & "'" & Doj & "'," & "'" & UpgradeDate & "','" & MemberName & "','" & NodeName & "','" & UpLiner & "'" & ",'" & Sponsor & "'," & LeftBV & "," & RightBV & "," & DirctLeftActive & "," & DirectRightActive & "," & "''" & ",'" & MemberName & "'," & "''" & "," & "'" & strImageFile & "'" & "," & "'" & strImageFile & "'" & "," & "'true'" & "," & "'" & GroupStatus & "'" & ");" + vbNewLine
        '----------------------------------------------


        Dim LoopValue As Integer
        Dim FolderFile As String = ""

        For Each dr As DataRow In ds1.Tables(0).Rows
            ParentId = Val(dr.Item("RefFormno").ToString)
            FormNo = Val(dr.Item("FormNoDwn").ToString)
            LegNo = dr.Item("Reflegno").ToString
            UpLiner = 0
            Sponsor = 0
            Doj = dr.Item("doj").ToString
            Category = dr.Item("Category").ToString
            LeftBV = dr.Item("Direct").ToString
            RightBV = dr.Item("Indirect").ToString
            strUrlPath = "Referaltree.aspx?DownLineFormNo=" & FormNo
            strImageFile = "images/" & dr.Item("JoinColor").ToString

            'MemberName = dr.Item("MemName").ToString & " <BR> (" & dr.Item("FormNoDwn").ToString & ")"

            MemberName = dr.Item("IdNO").ToString
            NodeName = dr.Item("MemFirstName").ToString
            Sponsor = dr.Item("Sponsorid").ToString + "(" + dr.Item("SponsorName").ToString + ")"
            UpgradeDate = dr.Item("UpgradeDate").ToString
            LoopValue = LoopValue + 1
            If Session("CompId") = "1055" Then
                GroupStatus = dr.Item("PositionStatus").ToString
            End If
            'ExpandYesNo = "true"
            If LoopValue <= 5 Then
                ExpandYesNo = "true"
            Else
                ExpandYesNo = "false"
            End If


            If FormNo <= 0 Then
                strImageFile = "images/empty.png"
                'strUrlPath = ""
                'If dr("ActiveStatus") = "N" Then
                '    strImageFile = "img/deact.jpg"
                'ElseIf dr("Kitid") = 1 Then
                '    strImageFile = "img/empty.jpg"
                'Else
                '    strImageFile = "img/empty.jpg"
                'End If
                'If LegNo = 1 Then
                '    MemberName = "Direct"
                'Else
                '    MemberName = "Direct"
                'End If
                MemberName = "Direct"
                strUrlPath = "" '"newjoining.aspx?RefFormNo=" & ParentId
            Else
                strImageFile = "images/" & dr.Item("JoinColor").ToString
                ''strUrlPath = ""
                'If dr("ActiveStatus") = "N" Then
                '    strImageFile = "img/deact.jpg"
                'ElseIf dr("Kitid") = 1 Then
                '    strImageFile = "img/red.jpg"
                'Else
                '    strImageFile = "img/blue.jpg"
                'End If
                strUrlPath = "ReferalTree.aspx?DownLineFormNo=" & FormNo
            End If
            myRunTimeString = myRunTimeString + " mytree.add(" & FormNo & "," & ParentId & "," & "'" & Category & "'" & "," & "'" & Doj & "'," & "'" & UpgradeDate & "','" & MemberName & "','" & NodeName & "','" & UpLiner & "'" & ",'" & Sponsor & "'," & LeftBV & "," & RightBV & "," & DirctLeftActive & "," & DirectRightActive & "," & "'" & strUrlPath & "'" & ",'" & MemberName & "'," & "'" & "" & "'" & "," & "'" & strImageFile & "'" & "," & "'" & strImageFile & "'" & "," & ExpandYesNo & "," & "'" & GroupStatus & "'" & ");" + vbNewLine
        Next

        myRunTimeString = myRunTimeString + vbNewLine & vbNewLine & vbNewLine & vbNewLine & " document.write(mytree);" + vbNewLine
        myRunTimeString = myRunTimeString + "</script> " + "<br> <br> <br> <br> "

        RegisterClientScriptBlock("clientScript", myRunTimeString)
    End Sub
    Private Function ToolTipTable() As String
        Dim strToolTip As String
        'strToolTip = "onMouseOver=""ddrivetip('<table width=100% border=0 cellpadding=5 cellspacing=1 bgcolor=#CCCCCC class=containtd>  <tr>     <td width=50% bgcolor=#999999><font color=#FFFFFF><strong>Member ID</strong></font></td>  </tr>  <tr>     <td>430</td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Name</strong></font></td>  </tr>  <tr>     <td>Mr-MAHESH BHARDWAJ </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Date of Joining</strong></font></td>  </tr>  <tr>     <td>2008-08-07 16:14:54 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Total Status</strong></font></td>  </tr>  <tr>     <td>LEFT:123 , RIGHT:2198 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Product</strong></font></td>  </tr>  <tr>     <td>CODE NO. 01-S.L. &nbsp;</td>  </tr></table>')"" onMouseOut=""hideddrivetip()"""
        Return strToolTip
    End Function


    Private Function getQuery(ByVal strSelectedFormNo As String, ByVal minDeptLevel As Integer) As String
        '---- check if user pass downline member than .. make according to downline member other wise show his tree



        'strQuery = " SELECT  * FROM " & _
        '          " ( SELECT a.FormNoDWn,b.RefFormNo,b.category,CONVERT(varchar,b.DOJ,6) AS DOJ,d.Direct,d.Indirect,Replace(b.MemName,'''','')as MemName,b.ActiveStatus,b.RefLegNo FROM  " & _
        '          " R_MemTreeRelation as a,m_MemberMaster as b ,V#DI d  " & _
        '          "  WHERE  a.FormNoDwn=b.FormNo AND  a.FormNo=" & strSelectedFormNo & " AND  MLevel<=" & minDeptLevel & _
        '          " AND  b.Formno=d.Formno " & _
        '          " UNION ALL" & _
        '          " SELECT cast(-a.formnodwn as varchar)+'1' as  FormNoDwn,a.FormNoDwn as RefFormNo,'****',CONVERT(varchar,getdate(),6) AS DOJ,0 as Direct,0 as Indirect,'Add New' as MemName,'B' as ActiveStatus,b.Reflegno FROM R_MemTreeRelation as a,m_MemberMaster as b WHERE  a.FormNoDwn=b.FormNo AND  a.FormNo=" & strSelectedFormNo & " AND  MLevel<=" & minDeptLevel & " AND a.FormNoDWn NOT IN (Select RefFormNo from m_MemberMaster) GROUP BY a.FormNoDwn,RefLegNo " & _
        '          ") as a" & _
        '          " ORDER BY  RefFormNo,RefLegNo "

        'strQuery = " SELECT  * FROM " & _
        '         "          ( SELECT a.FormNoDWn,b.RefFormNo,'' as category," & _
        '         "                  CONVERT(varchar,b.DOJ,6) AS DOJ,b.kitid as kitid,(c.Direct) as DIrect,(c.Indirect) as Indirect," & _
        '         "                  Replace(b.MemFirstName + ' ' + b.MemLastName,'''','') as MemFirstName," & _
        '         "                  b.ActiveStatus,b.RefLegNo   " & _
        '         "           FROM R_MemTreeRelation as a,m_MemberMaster as b,V#DI as c " & _
        '         "           WHERE  a.FormNoDwn=b.FormNo AND a.formnodwn = c.formno AND   a.FormNo=" & strSelectedFormNo & _
        '         "                  AND  MLevel<=" & minDeptLevel & _
        '         "          UNION ALL" & _
        '         "          SELECT cast(-a.formnodwn as varchar)+'1' as  FormNoDwn," & _
        '         "                  a.FormNoDwn as RefFormNo,'****' as category,CONVERT(varchar,getdate(),6) AS DOJ,0 as kitid, " & _
        '         "                  0 as Direct,0 as Indirect,'Add New' as MemFirstName," & _
        '         "                  'B' as ActiveStatus,b.Reflegno " & _
        '         "          FROM R_MemTreeRelation as a,m_MemberMaster as b " & _
        '         "          WHERE  a.FormNoDwn=b.FormNo AND  a.FormNo=" & strSelectedFormNo & _
        '         "          AND  MLevel<=" & minDeptLevel & " AND a.FormNoDWn NOT IN (Select RefFormNo from m_MemberMaster) " & _
        '         "          GROUP BY a.FormNoDwn,RefLegNo " & _
        '         "          ) as a" & _
        '         " ORDER BY  RefFormNo,RefLegNo "

        
        getQuery = " exec sp_ShowRefTree " & strSelectedFormNo & " , " & minDeptLevel

    End Function
    'Private Sub cmdSave1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSave1.Click
    '    ' Label1.Text = "new value :" & Request("DownLineFormNo")
    '    Response.Redirect("Referaltree.aspx?DownLineFormNo=" & DownLineFormNo.Value)
    'End Sub

    'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
    '    Response.Redirect("cpindex.aspx")
    'End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("Home.aspx")
    End Sub
End Class
