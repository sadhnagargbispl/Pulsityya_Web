<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="ChangeRef.aspx.vb" Inherits="App_UI_Application_Pages_ChangeRef" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

    <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
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
                            Change Sponsor</h2>
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
                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"
                                        Visible="false"></asp:Label>
                                    <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                                    </span>
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        Enter Member ID :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true">
                                        </asp:TextBox>
                                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox></div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        <strong>Current Sponsor:</strong></div>
                                    <div class="col-md-4">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="LblOldSponsor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        <strong>New Sponsor ID :</strong></div>
                                    <div class="col-md-4">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TxtSpIDNo" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                                <asp:Label ID="LblSponserName" runat="server" CssClass="label-text"></asp:Label>
                                                <asp:TextBox ID="TxtFormNos" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="TxtSpIDNo" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Sponser ID."
                                            ControlToValidate="TxtSpIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="BtnLegShift" runat="server" Text="Change Sponsor" OnClientClick="return confirmation();"
                                            class="btn btn-primary" ValidationGroup="Save" /></div>
                                    <div class="col-md-6">
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
