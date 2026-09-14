<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="UserMaster.aspx.vb" Inherits="App_UI_Application_Pages_UserMaster"
    Title="" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .PagerStyle
        {
            background-image: url(../Images/td.jpg);
            background-position: center;
            background-repeat: repeat-x;
            background-color: #ffffff;
            font-weight: bold;
            text-align: center;
            width: 00px;
        }
        .PagerStyle table
        {
            text-align: center;
            margin: auto;
        }
        .PagerStyle table td
        {
            border: 0px;
            padding: 5px;
        }
        .PagerStyle td
        {
            border-top: #1d1d1d 3px solid;
        }
        .PagerStyle a
        {
            color: #000000;
            text-decoration: none;
            padding: 2px 10px 2px 10px;
            border-top: solid 1px #777777;
            border-right: solid 1px #333333;
            border-bottom: solid 1px #333333;
            border-left: solid 1px #777777;
        }
        .PagerStyle span
        {
            font-weight: bold;
            color: #000000;
            text-decoration: none;
            padding: 2px 10px 2px 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            New User</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12">
                                    <div class="col-md-6" style="display: flex; padding: 1%">
                                        <asp:DropDownList ID="ddlSearchFields" runat="server" class="form-control" Style="width: 28%;
                                            padding: 0px; border-radius: 5px; display: inline">
                                            <asp:ListItem Selected="True" Value="showall">--Search By--</asp:ListItem>
                                            <%--  <asp:ListItem Value="GroupName">Group Name</asp:ListItem>--%>
                                            <asp:ListItem Value="UserName">User Name</asp:ListItem>
                                            <asp:ListItem>Remarks</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                            <asp:ListItem>ShowAll</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtSearch" class="form-control" runat="server" Style="width: 28%;
                                            margin-right: 1%; padding: 0px; border-radius: 5px; display: inline; margin-left: 1%"></asp:TextBox>
                                        <asp:ImageButton ID="imgSearch" runat="server" CssClass="imgSearchButton" ImageUrl="Images/search.png"
                                            Width="32px" Height="32px" />
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                    <div class=" col-md-12" style="padding: 2%; padding-right: 5%">
                                        <div class="col-md-2">
                                         <% If Session("CompID") = "1066" Then%>
                                            <asp:Label ID="lblText" runat="server" Text="Select Department :" Font-Bold="True"></asp:Label></div>
                                        <% Else%>
                                       <asp:Label ID="lblText1" runat="server" Text="Select Group :" Font-Bold="True"></asp:Label>&nbsp;&nbsp;</div>
                                       <% End If%>
                                       
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ddlGroup" runat="server" class="form-control" AutoPostBack="true"
                                                Style="display: inline">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-7">
                                        </div>
                                    </div>
                                    <div class=" col-md-12" style="padding: 1%">
                                        <div class="col-md-10">
                                            <asp:Button ID="BtnShowAll" runat="server" class="btn btn-primary" Text="Show All" />
                                            <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                            <a href="AddUser.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 460,marginTop : 0 } )">
                                                <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add User" /></a>
                                            <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                                            <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" visible="false">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="GrpID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="LblUserID" runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="GroupName" HeaderText="Group Name" ControlStyle-Width="50px" />--%>
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                        <asp:BoundField DataField="Passw" HeaderText="Password" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                        <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:TemplateField HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                            <ItemTemplate>
                                                <a href='<%# "AddUser.aspx?UserId=" & Crypto.Encrypt(Eval("VUserId"))  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 460,marginTop : 0 } )">
                                                    <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px">
                                                    </i>
                                                    <asp:Label ID="LBModify" runat="server" /></a>
                                                <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="85px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- end of weather widget -->
        </div>
    </div>
</asp:Content>
