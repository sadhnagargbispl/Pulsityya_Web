<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WithdrawlLimit.aspx.vb" Inherits="WithdrawlLimit"
     %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    
    </script>

    <script type="text/javascript" language="javascript">
        function isNumberKey1(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;

            return true;
        }
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
</head>
<body>
    <form id="form1" runat="server">
     <ajaxtoolkit:ToolkitScriptManager id="scriptmanager1" runat="server">
</ajaxtoolkit:ToolkitScriptManager>
 
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
   
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Withdrawl Limit</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                       
                                <div class="col-md-12" style="padding-top: 3%">
                                    <div class="col-md-4">
                                        Id No :</div>
                                    <div class="col-md-6">
                                        <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtIDNo" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                        <asp:TextBox ID="TxtFormNo" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 3%">
                                    <div class="col-md-4">
                                        Amount :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtFund" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Loan Amount."
                                            ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                    <div class="col-md-12" style="padding-top: 3%">
                                        <div class="col-md-4">
                                            Remarks :
                                            </div>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="TxtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remarks"
                                                    ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="padding-top: 3%">
                                            <asp:Button ID="BtnFundTransfer" runat="server" Text="Save" OnClientClick="return confirmation();"
                                                CssClass="btn btn-primary" ValidationGroup="Save" />
                                            <asp:Label ID="LblAmount" runat="server" CssClass="label-text"></asp:Label>
                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                        </div>
                                        </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="BtnFundTransfer" EventName="Click" />
                                                        </Triggers>
                        </asp:UpdatePanel>

                                    </div>
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
