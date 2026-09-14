<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="MonthlyPayoutCommission.aspx.vb" Inherits="MonthlyPayoutCommission" title="Untitled Page" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                         Monthly Payout Commission </h2>
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
                                    <asp:GridView ID="GvBatchMaster" Width="100%" runat="server" GridLines="Both" AllowPaging="true"
                                        class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                        PageSize="100" EmptyDataText="No data to display." ForeColor="Black" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SessId">
                                                <ItemTemplate>
                                                <asp:Label Id="LblSessid" runat="server" Text='<%# Eval("Sessid") %>' Visible="true"></asp:Label>
                                                   <%-- <asp:Label ID="lbluseddate" runat="server" Text='<%# Eval("date") %>'></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField ="fromdate" HeaderText="From Date" />
                                            <asp:BoundField DataField ="ToDate" HeaderText="To Date" />
                                            
                                            <%--<asp:TemplateField HeaderText="Commission %">
                                                <ItemTemplate>
                                                   <asp:TextBox  ID="txtslab" runat="server" Text='<%# Eval("Commission") %>' Enabled='<%# Eval("Status") %>'></asp:TextBox> 
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                             <asp:TemplateField HeaderText="Show On WebSite">
                                                <ItemTemplate>
                                                  <asp:CheckBox   ID="ChkShow" runat="server" Checked='<%# Eval("CheckStatus") %>' OnCheckedChanged="WebsiteData" AutoPostBack="true"  ></asp:CheckBox> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             
                                            <asp:TemplateField HeaderText="Calculate Payout">
                                            <ItemTemplate >
                                            
                                            <asp:Button ID="BtnPayout" runat="server" Text="Calculate Payout" Visible='<%# Eval("PayoutStatus") %>' CssClass="btn btn-primary" OnClick="DispatchPayoutData" />
                                            <%--<asp:Button ID="BtnPayout" runat="server" Text="Calculate Payout" Visible='<%# Eval("CheckStatus1") %>' CssClass="btn btn-primary" OnClick="DispatchPayoutData" />--%>
                                            
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate >
                                            
                                            <%--<asp:Button ID="BtnEdit" runat="server" Text="Update" Visible='<%# Eval("Status") %>' CssClass="btn btn-primary" OnClick="DispatchData" />--%>
                                            <asp:Button ID="BtnEdit" runat="server" Text="Transfer To Wallet" Visible='<%# Eval("PayoutStatus") %>' CssClass="btn btn-primary" OnClick="DispatchData" />
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
            <div class="row">
            </div>
        </div>
    </div>
</asp:Content>
