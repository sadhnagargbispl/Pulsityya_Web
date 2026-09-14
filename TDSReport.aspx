<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="TDSReport.aspx.vb" Inherits="TDSReport" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            TDS Report
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div align="center">
                            <div class="col-md-12">
                                <div class="col-md-2">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Member ID Wise :" Font-Bold="true" />
                                    &nbsp;&nbsp;
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtMemId" runat="server" class="form-control" Width="160px"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="CheckBox2" runat="server" Text="Session Wise :" Font-Bold="true"
                                        Checked="true" />
                                    &nbsp;&nbsp;</div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlMonth" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="DDlYear" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="clearfix">
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-2">
                                    <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" /></div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPrintAll" runat="server" class="btn btn-primary" Text="Print" /></div>
                                <div class="col-md-6">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>
                            </div>
                        </div>
                        <br />
                        <div id="divHeader" runat="server" visible="false" style="text-align: center">
                            <strong style="font-size: 18px">
                                <%=Session("Compname")%>
                            </strong>
                            <br />
                            <%=Session("CompAdd")%>
                            <br />
                            <strong style="font-size: 14px">TDS Report for the Payout Period of
                                <asp:Label ID="LblPayPeriod" runat="server"></asp:Label></strong></div>
                        <div style="padding: 10px 10px 20px 10px">
                            <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="false" PageSize="200"
                                                    GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                    ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false" >
                                <Columns>
                                    <asp:TemplateField HeaderText="SNo.">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSess" runat="server" Text='<%# Eval("SessId") %>' Visible="false"></asp:Label>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Session">
                                        <ItemTemplate>
                                            <asp:Label ID="StartDate" runat="server" Text='<%# Eval("PayoutDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Idno" HeaderText="Member Id" />
                                    <asp:BoundField DataField="MemberName" HeaderText="MemberName" />
                                    <asp:BoundField DataField="Address1" HeaderText="Address" />
                                    <asp:BoundField DataField="PanNo" HeaderText="PANNO" />
                                    <asp:BoundField DataField="NetIncome" HeaderText="GrossIncome" />
                                    <asp:BoundField DataField="TdsAmount" HeaderText="TDS Amount" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
