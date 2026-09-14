<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="BonanzaReportWellValue.aspx.vb" Inherits="BonanzaReportWellValue" Title="" %>

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
                            Bonanza Detail
                        </h2>
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
                                    <div class="col-md-2">
                                        Bonanza
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Member ID:
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox></div>
                                    <div class="col-md-2">
                                        Page Size
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed"
                                            class="form-control">
                                            <asp:ListItem Text="10" Value="10" />
                                            <asp:ListItem Text="20" Value="20" />
                                            <asp:ListItem Text="50" Value="50" />
                                            <asp:ListItem Text="100" Value="100" />
                                            <asp:ListItem Text="200" Value="200" />
                                            <asp:ListItem Text="300" Value="300" />
                                            <asp:ListItem Text="400" Value="400" />
                                            <asp:ListItem Text="500" Value="500" />
                                            <asp:ListItem Text="1000" Value="1000" />
                                            <asp:ListItem Text="2000" Value="2000" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2" id="Div3" runat="server" visible="false">
                                        Paid Status Check
                                        <asp:DropDownList ID="DDlPaidStatus" CssClass="form-control" runat="server">
                                            <asp:ListItem Value="Z" Selected="True" Text="All"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Paid"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="UnPaid"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <br />
                                    <div class="col-md-4">
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                    <div class="col-md-12">
                                        <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px;
                                            color: Gray"></asp:Label>
                                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>
                                    <div style="padding: 10px 10px 20px 10px" id="DivOfferT" visible="true" runat="server">
                                        <asp:GridView ID="GrdTotal" Width="100%" runat="server" AllowPaging="false" PageSize="200"
                                            GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false"
                                            AllowSorting="true" OnSorting="GrdTotal_Sorting" ForeColor="Black">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblOffertype" runat="server" Text='<%# Eval("Offertype") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MemberID" HeaderText="Member ID" />
                                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="OfferName" HeaderText="Offer Name" />
                                                <asp:TemplateField HeaderText="Matching RP">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblMatchingRP" runat="server" Text='<%# Eval("MatchingRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Left RP">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblLeftRP" runat="server" Text='<%# Eval("LeftRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Right RP">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblRightRP" runat="server" Text='<%# Eval("RightRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                                <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                    ControlStyle-CssClass="btn-group">
                                                    <ItemTemplate>
                                                        <a href='<%# "UpdateBoNanzaPaid.aspx?Offerid=" & Eval("Offerid") & "&formno=" & Eval("formno") & "&aid=" & Eval("aid") & "&Offertype=" & Eval("offertype")  %>'
                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                            <asp:Button ID="Button1" runat="server" Text="Paid" Visible='<%# Eval("VisibleButton") %>'
                                                                Style="color: #337ab7; font-size: 14px;" />
                                                        </a>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="55px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="GridView1" Width="100%" runat="server" AllowPaging="false" PageSize="200"
                                            GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false"
                                            AllowSorting="true" OnSorting="GridView1_Sorting" ForeColor="Black" Visible="true"
                                            OnRowCreated="GridView1_RowCreated">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                     <asp:Label ID="LblOffertype1" runat="server" Text='<%# Eval("sno") %>'></asp:Label>
                                                    <%--    <%# Container.DataItemIndex + 1 %>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblOffertype" runat="server" Text='<%# Eval("Offertype") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MemberID" HeaderText="Member ID" />
                                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="OfferName" HeaderText="Offer Name" />
                                                <asp:TemplateField HeaderText="Required Direct BV">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblMatchingRP" runat="server" Text='<%# Eval("MatchingRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Achieve BV">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblLeftRP" runat="server" Text='<%# Eval("LeftRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                                <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                    ControlStyle-CssClass="btn-group">
                                                    <ItemTemplate>
                                                        <a href='<%# "UpdateBoNanzaPaid.aspx?Offerid=" & Eval("Offerid") & "&formno=" & Eval("formno") & "&aid=" & Eval("aid") & "&Offertype=" & Eval("offertype")  %>'
                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                            <asp:Button ID="Button1" runat="server" Text="Paid" Visible='<%# Eval("VisibleButton") %>'
                                                                Style="color: #337ab7; font-size: 14px;" />
                                                        </a>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="55px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="GridView2" Width="100%" runat="server" AllowPaging="false" PageSize="200"
                                            GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false"
                                            AllowSorting="true" OnSorting="GridView2_Sorting" ForeColor="Black">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                    <asp:Label ID="LblOffertype1" runat="server" Text='<%# Eval("sno") %>'></asp:Label>
                                                      <%--  <%# Container.DataItemIndex + 1 %>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblOffertype" runat="server" Text='<%# Eval("Offertype") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MemberID" HeaderText="Member ID" />
                                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="OfferName" HeaderText="Offer Name" />
                                                <asp:TemplateField HeaderText=" Direct Activation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblDirectRP" runat="server" Text='<%# Eval("MatchingReqDirect") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Matched">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblMatchingRP" runat="server" Text='<%# Eval("MatchingRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Left Activation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblLeftRP" runat="server" Text='<%# Eval("LeftRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Right Activation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblRightRP" runat="server" Text='<%# Eval("RightRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                                <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                    ControlStyle-CssClass="btn-group">
                                                    <ItemTemplate>
                                                        <a href='<%# "UpdateBoNanzaPaid.aspx?Offerid=" & Eval("Offerid") & "&formno=" & Eval("formno") & "&aid=" & Eval("aid") & "&Offertype=" & Eval("offertype")  %>'
                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                            <asp:Button ID="Button1" runat="server" Text="Paid" Visible='<%# Eval("VisibleButton") %>'
                                                                Style="color: #337ab7; font-size: 14px;" />
                                                        </a>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="55px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="GridView3" Width="100%" runat="server" AllowPaging="false" PageSize="200"
                                            GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false"
                                            AllowSorting="true" OnSorting="GrdTotal_Sorting" ForeColor="Black">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblOffertype" runat="server" Text='<%# Eval("Offertype") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MemberID" HeaderText="Member ID" />
                                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="OfferName" HeaderText="Offer Name" />
                                                <asp:TemplateField HeaderText="Matching BV">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblMatchingRP" runat="server" Text='<%# Eval("MatchingRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Left BV">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblLeftRP" runat="server" Text='<%# Eval("LeftRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Right BV">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblRightRP" runat="server" Text='<%# Eval("RightRP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                                <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" />
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                    ControlStyle-CssClass="btn-group">
                                                    <ItemTemplate>
                                                        <a href='<%# "UpdateBoNanzaPaid.aspx?Offerid=" & Eval("Offerid") & "&formno=" & Eval("formno") & "&aid=" & Eval("aid") & "&Offertype=" & Eval("offertype")  %>'
                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                            <asp:Button ID="Button1" runat="server" Text="Paid" Visible='<%# Eval("VisibleButton") %>'
                                                                Style="color: #337ab7; font-size: 14px;" />
                                                        </a>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="55px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Repeater ID="rptPager" runat="server">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                    CssClass='<%# If(Convert.ToBoolean(Eval("Enabled")), "page_enabled", "page_disabled")%>'
                                                    OnClick="Page_Changed" OnClientClick='<%# If(Not Convert.ToBoolean(Eval("Enabled")), "return false;", "") %>'
                                                    ForeColor="Black"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
