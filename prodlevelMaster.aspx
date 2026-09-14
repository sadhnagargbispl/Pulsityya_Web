<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="prodlevelMaster.aspx.vb" Inherits="App_UI_Application_Pages_prodlevelMaster" Title=""
    EnableEventValidation="false" %>

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
                            Product Level Master</h2>
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
                                    <div class="col-md-6" style="display: none; padding: 1%">
                                        <asp:DropDownList ID="ddlSearchFields" runat="server" class="form-control" Style="width: 28%;
                                            padding: 0px; border-radius: 5px; display: inline">
                                            <asp:ListItem Selected="True" Value="showall">--Search By--</asp:ListItem>
                                            <asp:ListItem Value="KitName">Kit Name</asp:ListItem>
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
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    <a href="levelprod.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                        <asp:Button ID="BtnAddNew" runat="server" class="btn btn-primary" Text="Create New Product Level" /></a>
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
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
                                        <Columns>
                                         <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>.
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="KitId" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("prodId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                         
                                            <asp:BoundField DataField="ProductName" HeaderText="ProductName" />
                                            <asp:BoundField DataField="Level_1" HeaderText="Level_1" />
                                            <asp:BoundField DataField="Level_2" HeaderText="Level_2" />
                                            
                                            <asp:BoundField DataField="Level_3" HeaderText="Level_3" />
                                            <asp:BoundField DataField="Level_4" HeaderText="Level_4" />
                                            <asp:BoundField DataField="Level_5" HeaderText="Level_5" />
                                            <asp:BoundField DataField="Level_6" HeaderText="Level_6" />
                                            <asp:BoundField DataField="Level_7" HeaderText="Level_7" />
                                            <asp:BoundField DataField="Level_8" HeaderText="Level_8" />
                                            
                                            <asp:BoundField DataField="Level_9" HeaderText="Level_9" />
                                            <asp:BoundField DataField="Level_10" HeaderText="Level_10" />
                                           
                                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <a href='<%# "levelprod.aspx?prodId=" & Eval("prodId")  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                        <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px">
                                                        </i>
                                                        <asp:Label ID="LBModify" runat="server" />
                                                    </a>
                                                    <%--<asp:LinkButton ID="lnkModify" runat="server" Text="Modify" OnClick="ModifyGrp" CssClass="fancybox fancybox.iframe"></asp:LinkButton>--%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Status" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
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
</asp:Content>
