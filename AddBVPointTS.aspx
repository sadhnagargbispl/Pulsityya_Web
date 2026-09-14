<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddBVPointTS.aspx.vb" Inherits="AddBVPointTS"
    Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

    <title></title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Add BV R</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Member Id :</div>
                                    <div class="col-md-6">
                                        <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                        <asp:TextBox ID="TxtFormNo" runat="server" CssClass="TxtBox" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;BV Type:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="RbtType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" ValidationGroup="Save" AutoPostBack="true">
                                            <asp:ListItem Text="Self" Value="S" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Tree" Value="T"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <br />
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Leg No:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="RbtLeg" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                            ValidationGroup="Save">
                                            <asp:ListItem Text="Left" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        &nbsp;&nbsp;&nbsp;&nbsp; BV Point:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtFund" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter BV Point."
                                            ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        &nbsp;&nbsp;&nbsp;&nbsp; Remarks :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtRemarks" runat="server" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remrks"
                                            ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Add BV" class="btn btn-primary"
                                            ValidationGroup="Save" /></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
