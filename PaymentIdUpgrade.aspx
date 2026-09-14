<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="PaymentIdUpgrade.aspx.vb" Inherits="PaymentIdUpgrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            ID Upgrade</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="table-responsive ">
                            <div class="row">
                                <div class="col-md-10">
                                    <div class="col-md-3">
                                        Company EP Balance :</div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="TxtCompanyEP" runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                            Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-10">
                                    <div class="col-md-3">
                                        Member ID :
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-10">
                                            <div class="col-md-3">
                                                Member Name:</div>
                                            <div class="col-md-5">
                                                <asp:Label ID="LblKitId" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblMemName" runat="server" class="form-control"></asp:Label>
                                                <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label></div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-10">
                                            <div class="col-md-3">
                                                Package Name :</div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="TxtPackage" runat="server" class="form-control" Enabled="false"></asp:TextBox></div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-10">
                                            <div class="col-md-3">
                                                Package Amount :</div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="TxtAmount" runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                                    Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-10">
                                            <div class="col-md-3">
                                                Package EP :</div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="TxtEP" runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                                    Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <br />
                            <div class="row">
                                <div class="col-md-10">
                                    <div class="col-md-3">
                                        Payment Id. :</div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="TxtpaymentId" runat="server" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                            ControlToValidate="TxtpaymentId" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-10">
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-5">
                                        <asp:Button ID="BtnUpgrade" runat="server" Text="Upgrade" class="btn btn-primary"
                                            ValidationGroup="Save" />
                                        <asp:Button ID="BtnCancel" class="btn btn-primary" runat="server" Text="Cancel" />
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-10">
                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
     </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
</asp:Content>
