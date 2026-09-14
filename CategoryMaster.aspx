<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="CategoryMaster.aspx.vb" Inherits="CategoryMaster" Title="" %>
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
                            Category Master</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div >
                                <div class="col-md-12" style="display: none">
                                    <div class="col-md-6" style="display: flex; padding: 1%">
                                        <asp:DropDownList ID="ddlSearchFields" runat="server" class="form-control" Style="width: 28%;
                                            padding: 0px; border-radius: 5px; display: inline">
                                            <asp:ListItem Selected="True" Value="showall">--Search By--</asp:ListItem>
                                            <asp:ListItem Value="KitName">Category Name</asp:ListItem>
                                            <asp:ListItem>Remarks</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                            <asp:ListItem Value="showall">ShowAll</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtSearch" class="form-control" runat="server" Style="width: 28%;
                                            margin-right: 1%; padding: 0px; border-radius: 5px; display: inline; margin-left: 1%"></asp:TextBox>
                                        <asp:ImageButton ID="imgSearch" runat="server" CssClass="imgSearchButton" ImageUrl="Images/search.png"
                                            Width="32px" Height="32px" />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                        Visible="false" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                        Visible="false" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel"
                                        Visible="false" />
                                    <a href="addcat.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 600,height: 450,marginTop : 0 } )">
                                        <asp:Button ID="BtnAddNew" runat="server" class="btn btn-primary" Text="Create New Category" /></a>
                                    <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                        Visible="false" />
                                </div>
                                <div class="col-md-12" id="divContent" runat="server" visible="false">
                                    <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                </div>
                                <div style="margin-bottom: 20px;">
                                    <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="true" GridLines="Both"
                                        class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                        EmptyDataText="No data to display." AutoGenerateColumns="false" PageSize="10"
                                        PagerStyle-CssClass="PagerStyle">
                                        <Columns>
                                            <asp:TemplateField HeaderText="KitId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("catid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>.
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CatName" HeaderText="Category Name" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <a href='<%# "addcat.aspx?catid=" & Eval("VKitId")  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                        <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px">
                                                        </i>
                                                        <asp:Label ID="LBModify" runat="server" />
                                                    </a>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
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
    </div>
</asp:Content>
