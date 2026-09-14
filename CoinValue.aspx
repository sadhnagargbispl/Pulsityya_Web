<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="CoinValue.aspx.vb" Inherits="App_UI_Application_Pages_CoinValue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
                            Coin Value</h2>
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
                                        Coin Value :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCoin" runat="server" class="form-control">
                                        </asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="RR" runat="server" TargetControlID="txtCoin" ValidChars=".0123456789">
                                        </ajax:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Coin Value.!"
                                            ControlToValidate="txtCoin" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <br />
                                    <div class="col-md-12" style="margin-bottom: 1%">
                                        <div class="col-md-2">
                                            Remark :</div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRemark" runat="server" class="form-control">
                                            </asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Remark.!"
                                                ControlToValidate="txtRemark" ValidationGroup="Save"></asp:RequiredFieldValidator>
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
