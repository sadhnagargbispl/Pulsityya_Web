<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="LegShift.aspx.vb" Inherits="App_UI_Application_Pages_LegShift" Title="Untitled Page" %>

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
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Change Upliner</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div id="divDetailSection" runat="server" style="margin-bottom: 30px; margin-right: 20px;">
                            <table cellspacing="10px" cellpadding="0%" style="background-color: #f2f2f2; height: 50%"
                                width="100%" ">
                                <tbody>
                                    <tr>
                                        <td colspan="4" style="width: 100%">
                                            <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                                            </span>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr style="margin-top: 20px; padding-top: 20px">
                                        <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                            <strong>Member ID :</strong>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <%--  Member ID :
            <br />--%>
                                                    <asp:TextBox ID="TxtIDNo" runat="server" CssClass="TxtBox" AutoPostBack="true"></asp:TextBox>
                                                    <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                                    <asp:TextBox ID="TxtFormNo" runat="server" CssClass="TxtBox" Visible="false"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                                ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr style="margin-top: 20px; padding-top: 20px">
                                        <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                        </td>
                                        <td>
                                            <%-- <p  style="color: #666666; line-height: 25px;" >  --%>
                                            <span id="Span1" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                                            </span>
                                        </td>
                                    </tr>
                                    <tr style="margin-top: 20px; padding-top: 20px">
                                        <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                            <strong>Upliner ID :</strong>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="TxtSpIDNo" runat="server" CssClass="TxtBox" AutoPostBack="true"></asp:TextBox>
                                                    <asp:Label ID="LblSponserName" runat="server" CssClass="label-text"></asp:Label>
                                                    <asp:TextBox ID="TxtFormNos" runat="server" CssClass="TxtBox" Visible="false"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="TxtSpIDNo" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Sponser ID."
                                                ControlToValidate="TxtSpIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr style="margin-top: 20px; padding-top: 20px">
                                        <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                            <strong>Side :</strong>
                                        </td>
                                        <td>
                                            <%--<p>
       <p style="color: #666666; line-height: 25px;">--%>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblReg" Text="Leg No" Visible="false" runat="server"></asp:Label>
                                                    <asp:RadioButtonList ID="RbLeg" runat="server" Visible="True" RepeatDirection="Horizontal"
                                                        RepeatLayout="Flow">
                                                        <asp:ListItem Value="1" Text="Left" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Right"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="RbLeg" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RbLeg"
                                                ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="margin-top: 20px; padding-top: 20px">
                                        <td width="14%" align="left" valign="middle" style="padding-left: 20px;">
                                            <%--<strong>Transfer Amount :&nbsp;</strong>--%>
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnLegShift" runat="server" Text="Leg Shift" OnClientClick="return confirmation();"
                                                CssClass="btn btn-primary" ValidationGroup="Save" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <br />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
