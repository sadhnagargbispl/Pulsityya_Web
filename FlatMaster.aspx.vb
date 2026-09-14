Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class FlatMaster
    Inherits System.Web.UI.Page
    Dim dtData As New DataTable
    Dim objDAL As DAL
    Dim objModuleFun As ModuleFunction

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Session("AStatus") = "OK" Then
                Session("PageName") = "Depo Master"
            Else
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objDAL = New DAL(HttpContext.Current.Session("MlmDatabase" & Session("CompID")))

            If Not Page.IsPostBack Then
                Session("Mode") = "Insert"
                FillFloor()
                Dim DepoID As Integer = 0
                If Request.QueryString.HasKeys And Not Request.QueryString("Key") Is Nothing Then
                    DepoID = Val(Crypto.Decrypt(Replace(Request.QueryString("key"), " ", "+")))
                    If DepoID <> 0 Then
                        FillDetail(DepoID)
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim bool As Boolean = False
            Dim scrname As String = ""
            If TxtProjectName.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter Project Name.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
            If TxtAddress.Text = "" Then
                scrname = "<SCRIPT language='javascript'>alert('Enter Address.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If

            ClearInject()
            Dim str As String = ""
            If Session("Mode") = "Update" Then
                bool = checkProject("and id<>'" & Val(LblId.Text) & "'")
            Else
                bool = checkProject()
            End If
            If bool Then

                Dim i As Integer = 0
                Dim imageurl As String = ""
                Dim File As String = ""
                Dim strextension As String = ""
                For i = 0 To Request.Files.Count - 1
                    Dim postedFile As HttpPostedFile = Request.Files(i)
                    If postedFile.ContentLength > 0 Then
                        strextension = System.IO.Path.GetExtension(postedFile.FileName)
                        If (strextension.ToUpper() = ".JPG") Or (strextension.ToUpper() = ".JPEG") Or (strextension.ToUpper() = ".PNG") Then
                            Dim img As System.Drawing.Image = System.Drawing.Image.FromStream(postedFile.InputStream)
                            Dim height As Integer = img.Height
                            Dim width As Integer = img.Width
                            Dim size As Decimal = Math.Round((CDec(postedFile.ContentLength) / CDec(1024)), 1)
                            If size > (5120) Then
                                scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png/ image of upto 5mb size only!! ');" & "</SCRIPT>"
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                                Exit Sub
                            Else
                                File = Format(Now, "yyMMddhhmmssfff") & i & Session("CompID") & Path.GetExtension(postedFile.FileName)
                                postedFile.SaveAs(Server.MapPath("images/UploadImage/") & File)
                                imageurl = imageurl & "http://" & HttpContext.Current.Request.Url.Host & "/images/UploadImage/" & File & ","

                            End If
                        Else
                            scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg and JPEG and PNG extension file!! ');" & "</SCRIPT>"
                            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                            Exit Sub
                        End If
                    End If
                Next
                If imageurl <> "" Then
                    imageurl = imageurl.Remove(imageurl.Length - 1, 1)
                End If

                If Session("Mode") = "Insert" Then
                    str = "Insert into M_projectmaster(ProjectName,Address1,LongLatitude,distance1,distance2,distance3,images," & _
                    " Descriptions,siteincharge,Name,mobileno,VideoLink,ActiveStatus,Userid)" & _
                " Values('" & TxtProjectName.Text & "','" & TxtAddress.Text & "','" & TxtLocation.Text & "','" & TxtDistance1.Text & "'," & _
                    " '" & TxtDistance2.Text & "','" & TxtDistance3.Text & "','" & imageurl & "','" & TxtDesc.Text & "','" & TxtSiteIncharge.Text & "'," & _
                    "'" & Txtname.Text.Trim & "','" & TxtMobileno.Text & "','" & txtVideolink.Text & "','" & ddlStatus.SelectedValue & "','" & Val(Session("userid")) & "');"
                ElseIf Session("Mode") = "Update" Then
                    Dim image As String
                    If imageurl <> "" Then
                        image = imageurl
                    Else
                        image = LblImage.text
                    End If
                    str = "Insert into Tempprojectmaster(id,ProjectName,Address1,LongLatitude,distance1,distance2,distance3,images," & _
                    " Descriptions,siteincharge,Name,mobileno,VideoLink,ActiveStatus,Userid,TUserid)" & _
                    "Select id,ProjectName,Address1,LongLatitude,distance1,distance2,distance3,images," & _
                    " Descriptions,siteincharge,Name,mobileno,VideoLink,ActiveStatus,Userid,'" & Val(Session("Userid")) & "' from M_Projectmaster where id='" & LblId.Text & "'"
                    str = str & "Update M_Projectmaster Set ProjectName='" & TxtProjectName.Text & "' ,Address1='" & TxtAddress.Text & "', " & _
                    " LongLatitude='" & TxtLocation.Text & "',distance1='" & TxtDistance1.Text & "',distance2='" & TxtDistance2.Text & "', " & _
                    " distance3='" & TxtDistance3.Text & "',images='" & image & "',Descriptions='" & TxtDesc.Text & "' ," & _
                    " siteincharge='" & TxtSiteIncharge.Text & "',Name='" & Txtname.Text.Trim & "',mobileno='" & TxtMobileno.Text & "', " & _
                    " VideoLink='" & txtVideolink.Text & "',ActiveStatus='" & ddlStatus.SelectedValue & "',Userid='" & Val(Session("userid")) & "' where id='" & LblId.Text & "'"
                    str = str & " Update M_FlatImageMaster Set activeStatus='N' where ProjectId='" & LblId.Text & "' "

                End If
                If Val(txtUnit1.Text) > 0 Then
                    For Each Gvr As GridViewRow In Gvfloor1.Rows
                        str = str & "Insert into M_FlatImageMaster(projectid,Floorid,FlattypeId,Flatno,Userid,Area,Videolink,Amount)" & _
                        " select top 1 Id ,'" & Val(DDlFloor1.SelectedValue) & "','" & DirectCast(Gvr.FindControl("DDlFlatType"), DropDownList).SelectedValue & "'," & _
                        " '" & DirectCast(Gvr.FindControl("txtFlatNo"), TextBox).Text & "','" & Val(Session("userid")) & "' ," & _
                        " '" & DirectCast(Gvr.FindControl("txtArea"), TextBox).Text & "','" & DirectCast(Gvr.FindControl("TxtVideoLink"), TextBox).Text & "','" & Val(DirectCast(Gvr.FindControl("TxtAmount"), TextBox).Text) & "' from " & _
                        " M_projectmaster where ProjectName='" & TxtProjectName.Text & "' Order by Id Desc;"

                    Next
                End If
                If Val(TxtUnit2.Text) > 0 Then
                    For Each Gvr As GridViewRow In GvFloor2.Rows
                        str = str & "Insert into M_FlatImageMaster(projectid,Floorid,FlattypeId,Flatno,Userid,Area,Videolink,Amount)" & _
                        " select top 1 Id ,'" & Val(DDlFloor2.SelectedValue) & "','" & DirectCast(Gvr.FindControl("DDlFlatType"), DropDownList).SelectedValue & "'," & _
                        "  '" & DirectCast(Gvr.FindControl("txtFlatNo"), TextBox).Text & "','" & Val(Session("userid")) & "'," & _
                        " '" & DirectCast(Gvr.FindControl("txtArea"), TextBox).Text & "','" & DirectCast(Gvr.FindControl("TxtVideoLink"), TextBox).Text & "','" & Val(DirectCast(Gvr.FindControl("TxtAmount"), TextBox).Text) & "' from " & _
                        " M_projectmaster where ProjectName='" & TxtProjectName.Text & "' Order by Id Desc;"

                    Next
                End If
                If Val(TxtUnit3.Text) > 0 Then
                    For Each Gvr As GridViewRow In GVFloor3.Rows
                        str = str & "Insert into M_FlatImageMaster(projectid,Floorid,FlattypeId,Flatno,Userid,Area,Videolink,AMount)" & _
                        " select top 1  Id ,'" & Val(DDlFloor3.SelectedValue) & "','" & DirectCast(Gvr.FindControl("DDlFlatType"), DropDownList).SelectedValue & "'," & _
                        " '" & DirectCast(Gvr.FindControl("txtFlatNo"), TextBox).Text & "','" & Val(Session("userid")) & "'," & _
                        " '" & DirectCast(Gvr.FindControl("txtArea"), TextBox).Text & "','" & DirectCast(Gvr.FindControl("TxtVideoLink"), TextBox).Text & "','" & Val(DirectCast(Gvr.FindControl("TxtAmount"), TextBox).Text) & "' from " & _
                        " M_projectmaster where ProjectName='" & TxtProjectName.Text & "'Order by Id Desc;"

                    Next
                End If
                If Val(TxtUnit4.Text) > 0 Then
                    For Each Gvr As GridViewRow In GvFloor4.Rows
                        str = str & "Insert into M_FlatImageMaster(projectid,Floorid,FlattypeId,Flatno,Userid,Area,Videolink,AMount)" & _
                        " select top 1 Id ,'" & Val(DDlFloor4.SelectedValue) & "','" & DirectCast(Gvr.FindControl("DDlFlatType"), DropDownList).SelectedValue & "'," & _
                        " '" & DirectCast(Gvr.FindControl("txtFlatNo"), TextBox).Text & "','" & Val(Session("userid")) & "'," & _
                        " '" & DirectCast(Gvr.FindControl("txtArea"), TextBox).Text & "','" & DirectCast(Gvr.FindControl("TxtVideoLink"), TextBox).Text & "','" & Val(DirectCast(Gvr.FindControl("TxtAmount"), TextBox).Text) & "' from " & _
                        " M_projectmaster where ProjectName='" & TxtProjectName.Text & "' Order by Id Desc;"

                    Next
                End If
                If Val(TxtUnit5.Text) > 0 Then
                    For Each Gvr As GridViewRow In GvFloor5.Rows
                        str = str & "Insert into M_FlatImageMaster(projectid,Floorid,FlattypeId,Flatno,Userid,Area,Videolink,Amount)" & _
                        " select top 1 Id ,'" & Val(ddlFloor5.SelectedValue) & "','" & DirectCast(Gvr.FindControl("DDlFlatType"), DropDownList).SelectedValue & "'," & _
                        " '" & DirectCast(Gvr.FindControl("txtFlatNo"), TextBox).Text & "','" & Val(Session("userid")) & "'," & _
                        " '" & DirectCast(Gvr.FindControl("txtArea"), TextBox).Text & "','" & DirectCast(Gvr.FindControl("TxtVideoLink"), TextBox).Text & "','" & Val(DirectCast(Gvr.FindControl("TxtAmount"), TextBox).Text) & "'  from " & _
                        " M_projectmaster where ProjectName='" & TxtProjectName.Text & "' Order by Id Desc;"

                    Next
                End If

                Dim k As Integer = objDAL.SaveData(str)
                If (k > 0) Then
                    If Session("Mode") = "Insert" Then
                        clear()
                        scrname = "<SCRIPT language='javascript'>alert('Save Successfully !!');" & "</SCRIPT>"

                    Else
                        scrname = "<SCRIPT language='javascript'>alert('Update Successfully !!');" & "</SCRIPT>"
                    End If

                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                    Response.Redirect("ListFlatmaster.aspx")
                Else
                    scrname = "<SCRIPT language='javascript'>alert('Try Again Later !!');" & "</SCRIPT>"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                End If
            Else
                scrname = "<SCRIPT language='javascript'>alert('Project Name already exist.');" & "</SCRIPT>"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.[GetType](), "Close", scrname, False)
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub clear()
        Try

            TxtProjectName.Text = ""
            TxtAddress.Text = ""
            TxtDesc.Text = ""
            txtUnit1.Text = ""
            TxtUnit2.Text = ""
            TxtUnit3.Text = ""
            TxtUnit4.Text = ""
            TxtUnit5.Text = ""
            TxtDistance1.Text = ""
            TxtDistance2.Text = ""
            TxtDistance3.Text = ""
            txtVideolink.Text = ""
            Txtname.Text = ""
            TxtMobileno.Text = ""
            TxtSiteIncharge.Text = ""
            TxtLocation.Text = ""
            Gvfloor1.DataSource = fillgridview(0)
            Gvfloor1.DataBind()
            GvFloor2.DataSource = fillgridview(0)
            GvFloor2.DataBind()
            GVFloor3.DataSource = fillgridview(0)
            GVFloor3.DataBind()
            GvFloor4.DataSource = fillgridview(0)
            GvFloor4.DataBind()
            GvFloor5.DataSource = fillgridview(0)
            GvFloor5.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ClearInject()
        Try

        
            TxtProjectName.Text = objDAL.ClearInject(TxtProjectName.Text)
            TxtAddress.Text = objDAL.ClearInject(TxtAddress.Text)
            TxtDesc.Text = objDAL.ClearInject(TxtDesc.Text)
            txtUnit1.Text = objDAL.ClearInject(txtUnit1.Text)
            TxtUnit2.Text = objDAL.ClearInject(TxtUnit2.Text)
            TxtUnit3.Text = objDAL.ClearInject(TxtUnit3.Text)
            TxtUnit4.Text = objDAL.ClearInject(TxtUnit4.Text)
            TxtUnit5.Text = objDAL.ClearInject(TxtUnit5.Text)
            TxtDistance1.Text = objDAL.ClearInject(TxtDistance1.Text)
            TxtDistance2.Text = objDAL.ClearInject(TxtDistance2.Text)
            TxtDistance3.Text = objDAL.ClearInject(TxtDistance3.Text)
            txtVideolink.Text = objDAL.ClearInject(txtVideolink.Text)
            Txtname.Text = objDAL.ClearInject(Txtname.Text)
            TxtLocation.Text = objDAL.ClearInject(TxtLocation.Text)
            TxtMobileno.Text = objDAL.ClearInject(TxtMobileno.Text)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Try
            Response.Redirect("ListFlatMaster.aspx")
        Catch ex As Exception

        End Try
    End Sub
    Public Sub FillDetail(ByVal DepoID As Integer)
        Try
            clear()
            FillFloor()
            Dim Qry As String = "Select * From V#ProjectMaster Where ID=" & DepoID
            dtData = New DataTable
            dtData = objDAL.GetData(Qry)
            If dtData.Rows.Count > 0 Then
                Session("Mode") = "Update"
                btnSave.Text = "Update"
                LblId.Text = dtData.Rows(0)("ID")
                TxtProjectName.Text = dtData.Rows(0)("Projectname")
                TxtAddress.Text = dtData.Rows(0)("Address1")
                TxtLocation.Text = dtData.Rows(0)("LongLatitude")
                TxtDistance1.Text = dtData.Rows(0)("Distance1")
                TxtDistance2.Text = dtData.Rows(0)("Distance2")
                TxtDistance3.Text = dtData.Rows(0)("Distance3")
                LblImage.Text = dtData.Rows(0)("images")
                txtVideolink.Text = dtData.Rows(0)("VideoLink")
                TxtDesc.Text = dtData.Rows(0)("Descriptions")
                TxtSiteIncharge.Text = dtData.Rows(0)("SiteIncharge")
                Txtname.Text = dtData.Rows(0)("name")

                TxtMobileno.Text = dtData.Rows(0)("Mobileno")


                ddlStatus.SelectedValue = dtData.Rows(0)("ActiveStatus")

                Dim dt1 As New DataTable
                Qry = "select floorid,count(FloorId)as Unit from M_Flatimagemaster where projectid=" & DepoID & "  and activeStatus='Y' Group by FloorId"
                dtData = New DataTable
                dtData = objDAL.GetData(Qry)
                If dtData.Rows.Count > 0 Then
                    For i = 0 To dtData.Rows.Count - 1
                        If i = 0 Then

                            txtUnit1.Text = dtData.Rows(0)("Unit")
                            DDlFloor1.SelectedValue = dtData.Rows(0)("floorid")
                            dt1 = New DataTable
                            dt1 = fillgridview(txtUnit1.Text)
                            ViewState("CurrentTable") = dt1
                            Gvfloor1.DataSource = dt1
                            Gvfloor1.DataBind()
                        End If
                        If i = 1 Then
                            TxtUnit2.Text = dtData.Rows(1)("Unit")
                            DDlFloor2.SelectedValue = dtData.Rows(1)("Floorid")
                            dt1 = New DataTable
                            dt1 = fillgridview(TxtUnit2.Text)
                            ViewState("Floor2") = dt1
                            GvFloor2.DataSource = dt1
                            GvFloor2.DataBind()
                        End If
                        If i = 2 Then
                            TxtUnit3.Text = dtData.Rows(2)("Unit")
                            DDlFloor3.SelectedValue = dtData.Rows(2)("Floorid")
                            dt1 = New DataTable
                            dt1 = fillgridview(TxtUnit3.Text)
                            ViewState("Floor3") = dt1
                            GVFloor3.DataSource = dt1
                            GVFloor3.DataBind()
                        End If
                        If i = 3 Then
                            TxtUnit4.Text = dtData.Rows(3)("Unit")
                            DDlFloor4.SelectedValue = dtData.Rows(3)("Floorid")
                            dt1 = New DataTable
                            dt1 = fillgridview(TxtUnit4.Text)
                            ViewState("Floor4") = dt1
                            GvFloor4.DataSource = dt1
                            GvFloor4.DataBind()
                        End If
                        If i = 4 Then
                            TxtUnit5.Text = dtData.Rows(4)("Unit")
                            ddlFloor5.SelectedValue = dtData.Rows(4)("Floorid")
                            dt1 = New DataTable
                            dt1 = fillgridview(TxtUnit5.Text)
                            ViewState("Floor5") = dt1
                            GvFloor5.DataSource = dt1
                            GvFloor5.DataBind()
                        End If
                    Next

                End If



            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function checkProject(Optional ByVal Cond As String = "") As Boolean
        Try
            Dim str As String = "select *from V#ProjectMaster where Projectname='" & TxtProjectName.Text.Trim & "' " & Cond & " "
            Dim dt As New DataTable
            dt = objDAL.GetData(str)
            If dt.Rows.Count = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

        End Try
    End Function
    Protected Sub FillFloor()
        Try
            Dim str As String = ""
            str = "select Id,FloorType from M_FloorMaster where ActiveStatus='Y' "
            dtData = New DataTable
            dtData = objDAL.GetData(str)
            If dtData.Rows.Count > 0 Then
                DDlFloor1.DataSource = dtData
                DDlFloor1.DataValueField = "Id"
                DDlFloor1.DataTextField = "Floortype"
                DDlFloor1.DataBind()
                DDlFloor2.DataSource = dtData
                DDlFloor2.DataValueField = "Id"
                DDlFloor2.DataTextField = "Floortype"
                DDlFloor2.DataBind()
                DDlFloor3.DataSource = dtData
                DDlFloor3.DataValueField = "Id"
                DDlFloor3.DataTextField = "Floortype"
                DDlFloor3.DataBind()
                DDlFloor4.DataSource = dtData
                DDlFloor4.DataValueField = "Id"
                DDlFloor4.DataTextField = "Floortype"
                DDlFloor4.DataBind()

                ddlFloor5.DataSource = dtData
                ddlFloor5.DataValueField = "Id"
                ddlFloor5.DataTextField = "Floortype"
                ddlFloor5.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub txtUnit1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnit1.TextChanged
        Try
            Dim dt As DataTable = New DataTable()
            dt = fillgridview(txtUnit1.Text)
            'Dim dr As DataRow = Nothing
            'Dim s As Integer = 0
            'For s = 1 To txtUnit1.Text
            '    dt.Columns.Add(New DataColumn("Column" & s & "", GetType(String)))
            '    dt.Columns.Add(New DataColumn("Column1" & s & "", GetType(String)))

            '    dr = dt.NewRow()
            '    'dr("RowNumber") = 1
            '    dr("Column" & s & "") = String.Empty
            '    dr("Column1" & s & "") = String.Empty

            '    dt.Rows.Add(dr)
            'Next

            ViewState("CurrentTable") = dt
            Gvfloor1.DataSource = dt
            Gvfloor1.DataBind()
        Catch ex As Exception

        End Try
    End Sub
   



    Protected Sub Gvfloor1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gvfloor1.RowDataBound
        Try
            Dim dt As New DataTable
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim theDropDownList As DropDownList = CType(e.Row.FindControl("DDlFlatType"), DropDownList)
                theDropDownList.DataSource = fillFlattype()
                theDropDownList.DataTextField = "FlatType"
                theDropDownList.DataValueField = "Id"
                theDropDownList.DataBind()
                theDropDownList.Visible = True
                If Session("Mode") = "Update" Then
                    Dim str As String = ""
                    str = "select * from M_Flatimagemaster where ProjectId= '" & Val(LblId.Text) & "' and Floorid='" & DDlFloor1.SelectedValue & "' and activeStatus='Y' "
                    dt = objDAL.GetData(str)
                    If dt.Rows.Count > 0 Then
                        For i = 0 To Gvfloor1.Rows.Count
                            Try


                                If dt.Rows.Count <> Gvfloor1.Rows.Count Then
                                    CType(e.Row.FindControl("DDlFlatType"), DropDownList).SelectedValue = dt.Rows(i)("Flattypeid")
                                    DirectCast(e.Row.FindControl("txtFlatNo"), TextBox).Text = dt.Rows(i)("Flatno")
                                    DirectCast(e.Row.FindControl("txtArea"), TextBox).Text = dt.Rows(i)("Area")
                                    DirectCast(e.Row.FindControl("TxtVideoLink"), TextBox).Text = dt.Rows(i)("VideoLink")
                                    DirectCast(e.Row.FindControl("TxtAmount"), TextBox).Text = dt.Rows(i)("Amount")
                                End If
                            Catch ex As Exception

                            End Try
                        Next
                    End If
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function fillFlattype() As DataTable
        Try
            Dim str As String = "select * from V#Flattypemaster"
            Dim dtdata As DataTable
            dtdata = objDAL.GetData(str)
            Return dtdata
        Catch ex As Exception

        End Try
    End Function


    Private Function fillgridview(ByVal Unit As Integer) As DataTable
        Try
            Dim dt As DataTable = New DataTable()
            Dim dr As DataRow = Nothing
            Dim s As Integer = 0
            For s = 1 To Unit
                dt.Columns.Add(New DataColumn("Column" & s & "", GetType(String)))
                dt.Columns.Add(New DataColumn("Column1" & s & "", GetType(String)))
                dt.Columns.Add(New DataColumn("Column2" & s & "", GetType(String)))
                dt.Columns.Add(New DataColumn("Column3" & s & "", GetType(String)))

                dt.Columns.Add(New DataColumn("Column4" & s & "", GetType(String)))
                dr = dt.NewRow()
                'dr("RowNumber") = 1
                dr("Column" & s & "") = String.Empty
                dr("Column1" & s & "") = String.Empty
                dr("Column2" & s & "") = String.Empty
                dr("Column3" & s & "") = String.Empty
                dr("Column4" & s & "") = String.Empty
                dt.Rows.Add(dr)
            Next
            Return dt
        Catch ex As Exception

        End Try
    End Function

    Protected Sub GvFloor2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvFloor2.RowDataBound
        Try
            Dim dt As New DataTable
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim theDropDownList As DropDownList = CType(e.Row.FindControl("DDlFlatType"), DropDownList)
                theDropDownList.DataSource = fillFlattype()
                theDropDownList.DataTextField = "FlatType"
                theDropDownList.DataValueField = "Id"
                theDropDownList.DataBind()
                theDropDownList.Visible = True
                If Session("Mode") = "Update" Then
                    Dim str As String = ""
                    str = "select * from M_Flatimagemaster where ProjectId= '" & Val(LblId.Text) & "' and Floorid='" & DDlFloor2.SelectedValue & "' and activeStatus='Y' "
                    dt = objDAL.GetData(str)
                    If dt.Rows.Count > 0 Then
                        For i = 0 To GvFloor2.Rows.Count
                            Try
                                If dt.Rows.Count <> GvFloor2.Rows.Count Then
                                    CType(e.Row.FindControl("DDlFlatType"), DropDownList).SelectedValue = dt.Rows(i)("Flattypeid")
                                    DirectCast(e.Row.FindControl("txtFlatNo"), TextBox).Text = dt.Rows(i)("Flatno")
                                    DirectCast(e.Row.FindControl("txtArea"), TextBox).Text = dt.Rows(i)("Area")
                                    DirectCast(e.Row.FindControl("TxtVideoLink"), TextBox).Text = dt.Rows(i)("VideoLink")
                                    DirectCast(e.Row.FindControl("TxtAmount"), TextBox).Text = dt.Rows(i)("Amount")
                                End If
                            Catch ex As Exception

                            End Try
                        Next
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GVFloor3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVFloor3.RowDataBound
        Try
            Dim dt As New DataTable
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim theDropDownList As DropDownList = CType(e.Row.FindControl("DDlFlatType"), DropDownList)
                theDropDownList.DataSource = fillFlattype()
                theDropDownList.DataTextField = "FlatType"
                theDropDownList.DataValueField = "Id"
                theDropDownList.DataBind()
                theDropDownList.Visible = True
                If Session("Mode") = "Update" Then
                    Dim str As String = ""
                    str = "select * from M_Flatimagemaster where ProjectId= '" & Val(LblId.Text) & "' and Floorid='" & DDlFloor3.SelectedValue & "' and activeStatus='Y' "
                    dt = objDAL.GetData(str)
                    If dt.Rows.Count > 0 Then
                        For i = 0 To GVFloor3.Rows.Count
                            Try
                                If dt.Rows.Count <> GVFloor3.Rows.Count Then
                                    CType(e.Row.FindControl("DDlFlatType"), DropDownList).SelectedValue = dt.Rows(i)("Flattypeid")
                                    DirectCast(e.Row.FindControl("txtFlatNo"), TextBox).Text = dt.Rows(i)("Flatno")
                                    DirectCast(e.Row.FindControl("txtArea"), TextBox).Text = dt.Rows(i)("Area")
                                    DirectCast(e.Row.FindControl("TxtVideoLink"), TextBox).Text = dt.Rows(i)("VideoLink")
                                    DirectCast(e.Row.FindControl("TxtAmount"), TextBox).Text = dt.Rows(i)("Amount")
                                End If
                            Catch ex As Exception

                            End Try
                        Next
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvFloor4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvFloor4.RowDataBound
        Try
            Dim dt As New DataTable
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim theDropDownList As DropDownList = CType(e.Row.FindControl("DDlFlatType"), DropDownList)
                theDropDownList.DataSource = fillFlattype()
                theDropDownList.DataTextField = "FlatType"
                theDropDownList.DataValueField = "Id"
                theDropDownList.DataBind()
                theDropDownList.Visible = True
                If Session("Mode") = "Update" Then
                    Dim str As String = ""
                    str = "select * from M_Flatimagemaster where ProjectId= '" & Val(LblId.Text) & "' and Floorid='" & DDlFloor4.SelectedValue & "' and activeStatus='Y' "
                    dt = objDAL.GetData(str)
                    If dt.Rows.Count > 0 Then
                        For i = 0 To GvFloor4.Rows.Count
                            Try
                                If dt.Rows.Count <> GvFloor4.Rows.Count Then
                                    CType(e.Row.FindControl("DDlFlatType"), DropDownList).SelectedValue = dt.Rows(i)("Flattypeid")
                                    DirectCast(e.Row.FindControl("txtFlatNo"), TextBox).Text = dt.Rows(i)("Flatno")
                                    DirectCast(e.Row.FindControl("txtArea"), TextBox).Text = dt.Rows(i)("Area")
                                    DirectCast(e.Row.FindControl("TxtVideoLink"), TextBox).Text = dt.Rows(i)("VideoLink")
                                    DirectCast(e.Row.FindControl("TxtAmount"), TextBox).Text = dt.Rows(i)("Amount")
                                End If

                            Catch ex As Exception

                            End Try
                        Next
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvFloor5_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvFloor5.RowDataBound
        Try
            Dim dt As New DataTable
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim theDropDownList As DropDownList = CType(e.Row.FindControl("DDlFlatType"), DropDownList)
                theDropDownList.DataSource = fillFlattype()
                theDropDownList.DataTextField = "FlatType"
                theDropDownList.DataValueField = "Id"
                theDropDownList.DataBind()
                theDropDownList.Visible = True
                If Session("Mode") = "Update" Then
                    Dim str As String = ""
                    str = "select * from M_Flatimagemaster where ProjectId= '" & Val(LblId.Text) & "' and Floorid='" & ddlFloor5.SelectedValue & "' and activeStatus='Y' "
                    dt = objDAL.GetData(str)
                    If dt.Rows.Count > 0 Then
                        For i = 0 To GvFloor5.Rows.Count
                            Try

                                If dt.Rows.Count <> GvFloor5.Rows.Count Then
                                    CType(e.Row.FindControl("DDlFlatType"), DropDownList).SelectedValue = dt.Rows(i)("Flattypeid")
                                    DirectCast(e.Row.FindControl("txtFlatNo"), TextBox).Text = dt.Rows(i)("Flatno")
                                    DirectCast(e.Row.FindControl("txtArea"), TextBox).Text = dt.Rows(i)("Area")
                                    DirectCast(e.Row.FindControl("TxtVideoLink"), TextBox).Text = dt.Rows(i)("VideoLink")
                                    DirectCast(e.Row.FindControl("TxtAmount"), TextBox).Text = dt.Rows(i)("Amount")
                                End If
                            Catch ex As Exception

                            End Try
                        Next
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub TxtUnit2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtUnit2.TextChanged
        Try
            Dim Dt As New DataTable
            Dt = fillgridview(TxtUnit2.Text)
            ViewState("Floor2") = Dt
            GvFloor2.DataSource = Dt
            GvFloor2.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub TxtUnit3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtUnit3.TextChanged
        Try
            Dim Dt As New DataTable
            Dt = fillgridview(TxtUnit3.Text)
            ViewState("Floor3") = Dt
            GVFloor3.DataSource = Dt
            GVFloor3.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TxtUnit4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtUnit4.TextChanged
        Try

        
            Dim Dt As New DataTable
            Dt = fillgridview(TxtUnit4.Text)
            ViewState("Floor4") = Dt
            GvFloor4.DataSource = Dt
            GvFloor4.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub TxtUnit5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtUnit5.TextChanged
        Try
            Dim Dt As New DataTable
            Dt = fillgridview(TxtUnit5.Text)
            ViewState("Floor5") = Dt
            GvFloor5.DataSource = Dt
            GvFloor5.DataBind()
        Catch ex As Exception

        End Try
    End Sub
End Class
