<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="RedeemVoucherSummary.aspx.vb" Inherits="RedeemVoucherSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content2DT831731" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .PopCal
        {
            z-index: 100;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Redeem Voucher Report
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div class="table-responsive makeitresponsivegrid">
                                <div align="center">
                                    <div class="col-md-12">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-2">
                                                    Member ID
                                                    <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                                <%--    <asp:RequiredFieldValidator ID = "A" runat= "server" ControlToValidate="txtMemId" ValidationGroup="RR"
                                                    ForeColor = "Red" ErrorMessage="Please Enter Member ID.!"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <%-- <div class="col-md-2">
                                                    From Date
                                                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                        Format="dd-MMM-yyyy">
                                                    </AjaxToolkit:CalendarExtender>
                                                </div>--%>
                                                <%-- <div class="col-md-2">
                                                    To Date
                                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                                        Format="dd-MMM-yyyy">
                                                    </AjaxToolkit:CalendarExtender>
                                                </div>--%>
                                                <div class="col-md-2">
                                                    <br />
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" class="btn btn-primary"  />
                                                </div>
                                            </div>
                                        </div>
                                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                            AutoGenerateColumns="False" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            EmptyDataText="No data to display." GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Member ID" SortExpression="IdNo" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="MemberID" runat="server" Text='<%# Eval("Idno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Member Name" SortExpression="MemFirstName" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <%#Eval("MemFirstName")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                   <asp:TemplateField HeaderText="Mobile No." SortExpression="Mobl" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <%#Eval("Mobl")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Voucher" SortExpression="TotalVoucher" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TotalVoucher" runat="server" Text='<%# Eval("TotalVoucher") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Use Voucher" SortExpression="UseVoucher" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="UseVoucher" runat="server" Text='<%# Eval("UseVoucher") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Balance Voucher" SortExpression="BalVoucher" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="BalVoucher" runat="server" Text='<%# Eval("BalVoucher") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                  
                                                 <asp:TemplateField HeaderText="" SortExpression="CntD" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <a class="btn btn-primary btn-xs" href='RedeemVoucherDetails.aspx?Id=<%# Eval("Idno")%>'
                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 650,height: 600,marginTop : 0 } )">
                                                            View Detail</a>
                                                     
                                                    </ItemTemplate>
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
    </div>
</asp:Content>
