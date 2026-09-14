<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="WellSmartCard.aspx.vb" Inherits="WellSmartCard" %>

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


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Well Smart Coupon</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-3">
                            Member ID:
                            <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            Coupon No:
                            <asp:TextBox ID="TxtCoupon" runat="server" class="form-control"></asp:TextBox>
                        </div>
                      
                        <div class="col-md-3" >
                            Status
                            <asp:DropDownList ID="ddllist" runat="server" class="form-control">
                             <asp:ListItem Selected="True" Value="A">--Search By--</asp:ListItem>
                                            <asp:ListItem Value="Y">Used</asp:ListItem>
                                            <asp:ListItem Value="N">UnUsed</asp:ListItem>
                                           
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3" style="padding-top :18px">
                           <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                        <asp:Button ID="Button1" runat="server" class="btn btn-primary" Text="Export To Excel" />
                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                            color: Red"></asp:Label>
                        </div>
                        
                    </div>
                    <div class="col-md-12">
                        
                    </div>
                    <div style="margin-top: 20px; margin-bottom: 20px;">
                        <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                            color: Gray"></asp:Label>
                    </div>
                    <div style="padding: 10px 10px 20px 10px" id="divDetail" runat="server">
                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="true" GridLines="Both"
                            class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                            EmptyDataText="No data to display." AutoGenerateColumns="false" PageSize="10"
                            PagerStyle-CssClass="PagerStyle">
                            <Columns>
                                <asp:TemplateField HeaderText="SNo.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IdNo" HeaderText="Member ID" />
                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                <asp:BoundField DataField="Date" HeaderText="Date" />
                                <asp:BoundField DataField="EndDate" HeaderText="End Date" />
                                            <asp:BoundField DataField="CouponNo" HeaderText="Coupon No" />
                                            <asp:BoundField DataField="AMount" HeaderText="Amount" />
                                            
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="UsedDate" HeaderText="Used Date" />
                                            <asp:BoundField DataField="Package Name" HeaderText="Package Name" />
                                             <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center"
                                                ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                   
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Modify" 
                                                     Visible='<%# Eval("VisibleStatus") %>'>
                                                      <a href='<%# "AddWellSmartCard.aspx?CouponNo=" & Eval("CouponNo")  %>'  
                                                      onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                     <i class="fa fa-edit" style=" color:#d9534f; font-size :20px"></i>
                                                     </a>
                                                     </asp:LinkButton>
                                                     
                                                     </ItemTemplate>
                                                <HeaderStyle Width="85px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                          
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
</asp:Content>