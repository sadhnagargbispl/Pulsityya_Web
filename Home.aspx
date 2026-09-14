<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="Home.aspx.vb" Inherits="App_UI_Application_Pages_Home" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <!-- top tiles -->
        <% If Session("GroupId") = "1" Then%>
        <div class="row">
            <h5 class="text-danger">
                &nbsp;&nbsp;&nbsp;CURRENT INFO...</h5>
            <!-- -->
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Today's Registration
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="LblTodregister" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
            </div>
            <!-- -->
            <div id="Div1" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="True">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Today's Activation</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="LblTodActive" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
            </div>
            <div id="Div2" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="True">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Today's Deactivation</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="todaydeactive" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
            </div>
            <!-- -->
        </div>
        <div class="row">
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Total Registration</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="LblTotalRegister" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <!-- -->
            <div id="Div3" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="True">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Total Activation</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="LblTotalActive" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div4" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="true">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Total Deactive Id</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="LblTotalDeactive" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div id="Div5" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Main Wallet Credit</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblTotalCredit" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div6" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Main Wallet Debit</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblTotalDebit" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div7" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Main Wallet Balance</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblTotalBalance" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div id="Div18" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Fund Wallet Credit</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblTotalSCredit" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div19" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Fund Wallet Debit</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblTotalSDebit" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div20" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Fund Wallet Balance</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblTotalSBalance" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div id="Div11" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Repurchase Wallet Credit</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblrepurchcr" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div12" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Repurchase Wallet Debit</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblrepurchdr" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div13" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Repurchase Wallet Balance</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblrepurchbal" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div id="Div8" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Withdrawal Approve</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblWithApprove" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div9" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Withdrawal Pending</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblWithPending" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="Div17" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Joining Business</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblCurrSessnBV" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="Div14" runat="server" visible="false">
            <div  class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Repurchase Business</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="LblRepurchase" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div id="Div10" class="col-lg-3 col-md-3 col-sm-6 col-xs-12" runat="server" visible="false">
                <div class="x_panel">
                    <div class="x_title">
                        <h2 class="text-danger">
                            Total Income</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <p>
                            <asp:Label ID="lblTotalIncome" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div id="divbal">
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" style="display: none;">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Today Receive Amount</h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <asp:Label ID="todayreamt" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" style="display: none;">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Total Receive Amount</h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <asp:Label ID="TotalRecamt" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" style="display: none;">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Today Unit</h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <asp:Label ID="todayunit" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" style="display: none;">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Total Unit</h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <asp:Label ID="totalunit" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" style="display: none;">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Total Payout</h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <asp:Label ID="Label1" Text="0.00" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" style="display: none;">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Due Payout</h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <asp:Label ID="Label2" Text="0.00" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" style="display: none;">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Pending Pin Request</h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <asp:Label ID="LblPendingEpin" Text="0.00" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" style="display: none;">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Pending Payment Request</h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <asp:Label ID="LblPaymentRequest" Text="0.00" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <% Else%>
        <div class="row">
            <div class="col-md-12" style="text-align: center; margin-top: 20%">
                <h1>
                    Welcome To
                    <%= Session("Compname") %>
                </h1>
            </div>
        </div>
        <% End If%>
        <!-- end  -->
</asp:Content>
