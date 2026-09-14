<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="Home1.aspx.vb" Inherits="App_UI_Application_Pages_Home1" Title="" %>

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
            <%--<div class="row">
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Today Register
                            </h2>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                  
                                    <a id="TodayRegister" runat="server" style="color: White;" href="MemberProfile.aspx"
                                        class="btn btn-primary "></a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Today Active
                            </h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="TodayActive" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2 class="text-danger">
                                Today Deactive
                            </h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="TodayDeactive" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>--%>
            
            
            
            <div class="row">
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Today Register
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                  
                                    <a id="TodayRegister" runat="server" style="color: White;" href="MemberProfile.aspx"
                                        class="btn btn-primary "></a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                               PACKAGE EP 30
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP30" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                PACKAGE EP 50
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP50" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                
                
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                PACKAGE EP 120
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP120" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                               PACKAGE EP1200
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP1200" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Today Deactive
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="TodayDeactive" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <!-- -->
            
            
            
            
            <div class="row">
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total Register
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                    <a id="TotalRegister" runat="server" style="color: White;" href="MemberProfile.aspx"
                                        class="btn btn-primary "></a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total Package EP 30
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                    <a id="TEP30" runat="server" style="color: Black;"></a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                              Total Package EP 50
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                    <a id="TEP50" runat="server" style="color: Black;"></a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                               Total Package EP 120
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                    <a id="TEP120" runat="server" style="color: Black;"></a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total Package EP 1200
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                    <a id="TEP1200" runat="server" style="color: Black;"></a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total Deactive
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="TotalDeactive" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            
            
            
            
            <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="totalOrderEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Star EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="EP30Order" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Bronze EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP50Order" runat="server" style="color: Black;" href="MemberProfile.aspx"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Silver EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP120Order" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Gold EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP1200Order" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            
            
            
            
            
            
            
            <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                <a style="color: White;" href="GenerateEPReport.aspx" class="btn btn-primary ">Company
                                    EP</a>
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                    &nbsp;
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Generated EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="GeneratedEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Received From ID
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="ReceivedFromID" runat="server" style="color: Black;" href="MemberProfile.aspx">
                                </a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="TotalEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Debit EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="DebitEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Balance EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="BalanceEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                <a style="color: White;" href="WalletTransactionReport.aspx" class="btn btn-primary ">
                                    Shopping Wallet EP</a>
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                    <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                    <a id="A1" runat="server" style="color: Black;"></a>&nbsp;
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="TotalEPR" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Used EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="UsedEPR" runat="server" style="color: Black;" href="MemberProfile.aspx"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Balance EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="BalanceEPR" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                <a style="color: White;" href="#" class="btn btn-primary ">Main Wallet EP</a>
                            </h5>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                <p>
                                    &nbsp;
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="TotalEPM" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Used EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="UsedEPM" runat="server" style="color: Black;" href="MemberProfile.aspx"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Balance EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="BalanceEPM" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
              <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total CashBack EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="CBEPToatal" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                CashBack Star EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="CBEP30" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                CashBack Bronze EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="CBEP50" runat="server" style="color: Black;" href="MemberProfile.aspx"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                CashBack Silver EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="CBEP120" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                CashBack Gold EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="CBEP1200" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
               <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Total Gst EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="totalGstEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Star EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="EP30Gst" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Bronze EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP50Gst" runat="server" style="color: Black;" href="MemberProfile.aspx"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Silver EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP120Gst" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Gold EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="EP1200Gst" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                9th Level EP History
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="TotalEPLevel9" runat="server" style="color: Black;"></a>&nbsp;
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                1St Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level1" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                2nd Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level2" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                3rd Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level3" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                4th Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level4" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                5th Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level5" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                6th Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level6" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                7th Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level7" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                8th Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level8" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                9th Level
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Level9" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-1 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Extra EP
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="Extraep" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
            </div>
              <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Direct Level EP
                           </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="DLTotalEP" runat="server" style="color: Black;"></a>
                                &nbsp;
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Star EP
                           </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <%--   <asp:Label ID="lblTodayRegister" runat="server"></asp:Label>--%>
                                <a id="DLBStarEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Bronze EP
                           </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="DLBBronzeEP" runat="server" style="color: Black;" href="MemberProfile.aspx"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <!-- -->
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Silver EP
                           </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="DLBSilverEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bpm Gold EP
                           </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="DLBGoldEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                
                
                <div class="col-lg-2 col-md-2 col-sm-3 col-xs-6">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Extra EP
                           </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="DLBExtraEP" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                         <a id="A2" runat="server" style="color: White;" href="FundWithdrawReport.aspx"
                                        class="btn btn-primary "> Withdrawl Wallet</a>
                           <%-- <h5 class="text-danger">
                                Withdrawl Wallet
                            </h5>--%>
                            <div class="clearfix">
                            </div>
                            <div class="x_content">
                                 <p>
                                    &nbsp;
                                    <br />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- -->
                 <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                            Withdrawl Total                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                             <a id="Withdrawlsamount" runat="server" style="color: Black;"></a>  
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                              TDS Amount
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="withdrawlTdsAmount" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Bank Service
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="withdrawlBankService" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>
                
                
                <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                               Withdrawl Amount
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                             <a id="withdrawlNetAmount" runat="server" style="color: Black;"></a>
                                
                            </p>
                        </div>
                    </div>
                </div>
                
             
                <!-- -->
                 <%-- <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h5 class="text-danger">
                                Today Deactive
                            </h5>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <p>
                                <a id="A7" runat="server" style="color: Black;"></a>
                            </p>
                        </div>
                    </div>
                </div>--%>
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
    </div>
</asp:Content>
