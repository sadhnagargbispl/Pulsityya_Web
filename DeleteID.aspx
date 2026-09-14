<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="DeleteID.aspx.vb" Inherits="App_UI_Application_Pages_DeleteID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Delete ID</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                            </span>
                        </div>
                        <div style="min-height: 200px; padding-left: 10px">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            Member ID :
                                            <br />
                                            <asp:TextBox ID="TxtIDNo" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                            <asp:TextBox ID="TxtFormNo" runat="server" Visible="false"></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                        ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="BtnDelete" runat="server" Text="Delete" CssClass="btn btn-primary" ValidationGroup="Save" />
                                    <asp:Button ID="BtnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" ValidationGroup="Cancel" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                           </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
