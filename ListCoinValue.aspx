<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="ListCoinValue.aspx.vb" Inherits="ListCoinValue" %>

<asp:Content ID="Content2DT831731" ContentPlaceHolderID="head" runat="Server">
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
                            List Coin Value
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
                                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="True" PageSize="20"
                                            AutoGenerateColumns="False" ForeColor="Black" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            EmptyDataText="No data to display." GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Coin" SortExpression="Coin" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Coin" runat="server" Text='<%# Eval("Coin") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Coin Value" SortExpression="CValue" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <%#Eval("CValue")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                <asp:TemplateField HeaderText="Status" SortExpression="Status" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="stateName" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Remark" SortExpression="Remark" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <asp:Label ID="stateName" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                
                                                <asp:TemplateField HeaderText="Date" SortExpression="Date" HeaderStyle-ForeColor="White">
                                                    <ItemTemplate>
                                                        <a class="btn btn-primary btn-xs" href='CoinValue.aspx?id=<%# Eval("ID")%>'>Edit</a>
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
