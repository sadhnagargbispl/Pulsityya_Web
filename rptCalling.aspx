<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="rptCalling.aspx.vb" Inherits="rptCalling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                                <asp:BoundField DataField="Idno" HeaderText="ID NO." />
                                                <asp:BoundField DataField="Name" HeaderText="Name of customer" />
                                                <asp:BoundField DataField="Mobl" HeaderText="Customer mobile no." />
                                                <asp:BoundField DataField="ProductName" HeaderText="Product" />
                                                <asp:BoundField DataField="Address1" HeaderText="Address" />
                                                <asp:BoundField DataField="City" HeaderText="City" />
                                                <asp:BoundField DataField="District" HeaderText="District" />
                                                <asp:BoundField DataField="StateName" HeaderText="Name of state" />
                                                <asp:BoundField DataField="DOJ" HeaderText="Registration date" />
                                                <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                                <asp:BoundField DataField="Remark" HeaderText="User remarks" />
                                                <asp:BoundField DataField="DAddress" HeaderText="Deliver address" />
                                                <asp:BoundField DataField="CallDate" HeaderText="Date of call" />
                                                <asp:BoundField DataField="SKIP" HeaderText="Skip status" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExport" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="x_title">
                        <h2>
                            All skipped calls</h2>
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
                                <div class="col-md-12">
                                    <asp:Button ID="BtnExportSkipped" runat="server" class="btn btn-primary" Text="Export To Excel"
                                        Enabled="false" />
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
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
