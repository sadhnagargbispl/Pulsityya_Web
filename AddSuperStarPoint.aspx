<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddSuperStarPoint.aspx.vb"
    Inherits="AddSuperStarPoint" Title="Untitled Page" %>

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
                                    Super Star</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 2%">
                                    <div class="col-md-4">
                                        Super Star Amount:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtFund" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter BV Point."
                                            ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 2%">
                                    <div class="col-md-4">
                                        Remark:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtRemarks" runat="server" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remrks"
                                            ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 2%">
                                    <div class="col-md-4">
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Add Amount" OnClientClick="return confirmation();"
                                            class="btn btn-primary" ValidationGroup="Save" /></div>
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
