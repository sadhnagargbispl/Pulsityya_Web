<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="OfferdetailNew.aspx.vb" Inherits="OfferdetailNew" Title="" EnableEventValidation="false" %>

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
                            Package Offer Detail</h2>
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
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                        Visible="false" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                        Visible="false" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    <a href="AddofferNew.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 580,height: 490,marginTop : 0 } )">
                                        <asp:Button ID="BtnAddNew" runat="server" class="btn btn-primary" Text="Add Offer" /></a></td>
                                    <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                        Visible="false" />
                                </div>
                                <div style="margin-bottom: 20px" class="col-md-12" runat="server" visible="true"
                                    id="DivAllUser">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" DataKeyNames="OfferID" class="table table-bordered"
                                        HeaderStyle-CssClass="bg-primary" ShowHeader="true" PageSize="20" EmptyDataText="No data to display."
                                        PagerStyle-CssClass="PagerStyle">
                                        <Columns>
                                            <asp:TemplateField HeaderText="offerno" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbloffer" runat="server" Text='<%# Eval("OfferID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="offername" HeaderText="Rank" />
                                            <asp:BoundField DataField="Startdate" HeaderText="Startdate" />
                                            <asp:BoundField DataField="EndDate" HeaderText="EndDate" />
                                            <asp:BoundField DataField="Selfbv" HeaderText="Self BV" />
                                            <asp:BoundField DataField="DirectBv" HeaderText="Direct BV" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="View">
                                                <ItemTemplate>
                                                    <a href='OfferDetails.aspx?id=<%# Eval("OfferID") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 480,height: 490,marginTop : 0 } )">
                                                        <i class="fa fa-search" style="color: #d9534f; font-size: 20px"></i></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"
                                                        class="btn btn-danger"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div style="margin-bottom: 20px" class="col-md-12" runat="server" visible="False"
                                    id="DivWellValueUser">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" DataKeyNames="OfferID" class="table table-bordered"
                                        HeaderStyle-CssClass="bg-primary" ShowHeader="true" PageSize="20" EmptyDataText="No data to display."
                                        PagerStyle-CssClass="PagerStyle">
                                        <Columns>
                                            <asp:TemplateField HeaderText="offerno" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbloffer" runat="server" Text='<%# Eval("OfferID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="offername" HeaderText="Rank" />
                                            <asp:BoundField DataField="Startdate" HeaderText="Startdate" />
                                            <asp:BoundField DataField="EndDate" HeaderText="EndDate" />
                                            <asp:BoundField DataField="Selfbv" HeaderText="Self BV" />
                                            <asp:BoundField DataField="DirectBv" HeaderText="Direct BV" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="View">
                                                <ItemTemplate>
                                                    <a href='OfferDetails.aspx?id=<%# Eval("OfferID") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 480,height: 490,marginTop : 0 } )">
                                                        <i class="fa fa-search" style="color: #d9534f; font-size: 20px"></i></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <a href='<%# "AddofferNew.aspx?OfferID=" & Eval("OfferID")  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                        <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px">
                                                        </i>
                                                        <asp:Label ID="LBModify" runat="server" />
                                                    </a>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup1"
                                                        class="btn btn-danger"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
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
        </div>
    </div>
    <br />
    <br />
</asp:Content>
