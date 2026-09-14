<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="SmartCardBV.aspx.vb" Inherits="SmartCardBV" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="0">
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
                            Well Smart BV</h2>
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
                                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text" ForeColor="Red"></asp:Label>
                                        <asp:HiddenField ID="hdnFormno" runat="server" />
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
                                        Amount :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAmount" runat="server" class="form-control" >
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Amount."
                                            ControlToValidate="txtAmount" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <AjaxToolkit:FilteredTextBoxExtender ID="rr" runat="server" ValidChars=".0123456789"
                                            TargetControlID="txtAmount">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                 <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        Matching BV% :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TXtmatchingperc" runat="server" class="form-control" AutoPostBack="true" >
                                        </asp:TextBox>
                                         <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" ValidChars=".0123456789"
                                            TargetControlID="TXtmatchingperc">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="* Enter Matching BV %."
                                            ControlToValidate="TXtmatchingperc" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        Matching BV :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtBV" runat="server" class="form-control" ReadOnly="true">
                                        </asp:TextBox>
                                         <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" ValidChars=".0123456789"
                                            TargetControlID="txtBV">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter BV."
                                            ControlToValidate="txtBV" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                 <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        Royalty% :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtRoyaltyper" runat="server" class="form-control"  AutoPostBack ="true">
                                        </asp:TextBox>
                                         <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" ValidChars=".0123456789"
                                            TargetControlID="TxtRoyaltyper">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="* Enter Royalty %."
                                            ControlToValidate="TxtRoyaltyper" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        Smart Card BV :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtRoyalty" runat="server" class="form-control" ReadOnly="true">
                                        </asp:TextBox>
                                         <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" ValidChars=".0123456789"
                                            TargetControlID="TxtRoyalty">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="* Enter Royalty."
                                            ControlToValidate="TxtRoyalty" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                 <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        Reward% :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtRewardPerc" runat="server" class="form-control" AutoPostBack="true" >
                                        </asp:TextBox>
                                         <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" ValidChars=".0123456789"
                                            TargetControlID="TxtRewardPerc">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="* Enter Reward %."
                                            ControlToValidate="TxtRewardPerc" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        Reward  :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtReward" runat="server" class="form-control" ReadOnly="true">
                                        </asp:TextBox>
                                         <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" ValidChars=".0123456789"
                                            TargetControlID="TxtReward">
                                        </AjaxToolkit:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="* Enter Reward."
                                            ControlToValidate="TxtReward" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        Remark :</div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtRemark" runat="server" class="form-control" TextMode="MultiLine">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* Enter Remark."
                                            ControlToValidate="txtBV" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="BtnLegShift" runat="server" Text="Add" OnClientClick="return confirmation();"
                                            class="btn btn-primary" ValidationGroup="Save" />
                                            <asp:Button ID="BtnLegShiftLess" runat="server" Text="Less" OnClientClick="return confirmation();"
                                            class="btn btn-primary" ValidationGroup="Save" />
                                            </div>
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
            </ContentTemplate> 
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID ="BtnLegShift" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel> 
</asp:Content>

