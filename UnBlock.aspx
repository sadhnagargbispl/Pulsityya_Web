<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="UnBlock.aspx.vb" Inherits="App_UI_Application_Pages_UnBlock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            UnBlock</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlChoice" runat="server" BackColor="Transparent">
                                    <asp:RadioButtonList ID="rdblistChoice" runat="server" AutoPostBack="True" CellPadding="2"
                                        CellSpacing="5" RepeatColumns="2" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Value="single">Unblock Single Member</asp:ListItem>
                                        <asp:ListItem Value="multiple">Unblock Tree</asp:ListItem>
                                    </asp:RadioButtonList>
                                </asp:Panel>
                            </div>
                            <div id="divSingle" runat="server" class="col-md-12">
                                <div class="col-md-12" style="margin-bottom: 2%">
                                    <div class="col-md-2">
                                        <strong>Member ID :</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtMemberId" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="TxtFormNo" runat="server" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12" style="margin-bottom: 2%" id="divremark" runat="server" visible="false">
                                    <div class="col-md-2">
                                        <strong>Remarks :</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txremarks" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txremarks"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12" style="margin-bottom: 2%">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="btnShowSingleDetail" class="btn btn-primary" runat="server" Text="View Detail"
                                            ValidationGroup="Save" />
                                        <asp:Button ID="BtnBlock" class="btn btn-primary" runat="server" Text="Unblock" Width="70px"
                                            Visible="false" ValidationGroup="Save" />
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblrecordcount" runat="server" Text="" Font-Bold="True"></asp:Label></div>
                                    <div class="col-md-4">
                                        <asp:Label Style="padding-left: 10px; padding-top: 5px" ForeColor="Red" ID="lblError"
                                            runat="server" Visible="False" Font-Bold="True"></asp:Label></div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                        GridLines="None" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="20" EmptyDataText="No data to display." Visible="false">
                                        <%-- <Columns>
        <asp:BoundField DataField="IdNo" HeaderText="Member ID" />
        <asp:BoundField DataField="MemFirstName" HeaderText="Member First Name" />
        <asp:BoundField DataField="MemLastName" HeaderText="Member Last Name" />
    </Columns>--%>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
