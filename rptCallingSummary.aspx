<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="rptCallingSummary.aspx.vb" Inherits="rptCallingSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
        }
    </style>
    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }
        .modal1
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center1
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center1 img
        {
            height: 128px;
            width: 128px;
        }
        .close
        {
            display: block;
            background: url(img/close.png) no-repeat 0px 0px;
            left: -5px;
            width: 26px;
            text-indent: -1000em;
            position: absolute;
            top: -7px;
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="../images/loading.gif" />
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
                            All calls</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="Span1" class="text-danger"></span>
                        </div>
                        <div>
                            <div align="center">
                                <div class="col-md-12">
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel"
                                                Enabled="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div id="Div1" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                        margin-bottom: 25px;" class="col-md-12">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                            GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" AllowSorting="true" EmptyDataText="No data to display.">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>.</ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Calls">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkTotal" Style="color: Black;" runat="server" Text='<%# Eval("Total") %>'
                                                            CommandName="Total"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Product Booked">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkTotalProduct" Style="color: Black;" runat="server" Text='<%# Eval("BookedProduct") %>'
                                                            CommandName="BookedProduct"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Calls Yesterday">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkTotalCallsYesterday" Style="color: Black;" runat="server"
                                                            Text='<%# Eval("CallYesterday") %>' CommandName="CallYesterday"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Booked Yesterday">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkTotalBookedYestersay" Style="color: Black;" runat="server"
                                                            Text='<%# Eval("BookedYesterDay") %>' CommandName="BookedYesterday"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkClose" runat="server"></asp:LinkButton>
                                    <AjaxToolkit:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" BehaviorID="mpe"
                                        TargetControlID="lnkFake" CancelControlID="lnkClose" BackgroundCssClass="modalBackground">
                                    </AjaxToolkit:ModalPopupExtender>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExport" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="x_title">
                        <h2>
                            Calls details</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div>
                            <div align="center">
                                <div class="col-md-12">
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="height: 800px;
                                        overflow: auto;">
                                        <div class="col-md-12">
                                            <asp:Button ID="BtnExportSkipped" runat="server" class="btn btn-primary" Text="Export To Excel"
                                                Enabled="false" />
                                        </div>
                                        <div class="col-sm-12">
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Close" Style="color: black;
                                                float: right; font-size: 16px;" OnClientClick="$find('mp1').hide(); return false;" />
                                        </div>
                                        <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                            margin-bottom: 25px;" class="col-md-12">
                                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                                GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                ShowHeader="true" AllowSorting="true" EmptyDataText="No data to display.">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1%>.</ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Idno" HeaderText="ID NO." />
                                                    <asp:BoundField DataField="Name" HeaderText="Name of customer" />
                                                    <asp:BoundField DataField="ProductName" HeaderText="Product" />
                                                    <asp:BoundField DataField="Mobl" HeaderText="Customer mobile no." />
                                                    <asp:BoundField DataField="Address1" HeaderText="Address" />
                                                    <asp:BoundField DataField="City" HeaderText="City" />
                                                    <asp:BoundField DataField="District" HeaderText="District" />
                                                    <asp:BoundField DataField="StateName" HeaderText="Name of state" />
                                                    <asp:BoundField DataField="DOJ" HeaderText="Registration date" />
                                                    <asp:BoundField DataField="Remark" HeaderText="User remarks" />
                                                    <asp:BoundField DataField="DAddress" HeaderText="Deliver address" />
                                                    <asp:BoundField DataField="CallDate" HeaderText="Date of call" />
                                                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="BtnExportSkipped" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
