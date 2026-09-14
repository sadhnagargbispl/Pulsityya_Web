<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddSmartCard.aspx.vb" Inherits="AddSmartCard" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
    <title></title>
    <link href="css/Main.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
     <AjaxToolkit:ToolkitScriptManager ID="scriptmanager1" runat="server">
    </AjaxToolkit:ToolkitScriptManager>
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Smart Coupon</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                              
                                 <div class="col-md-12" style="padding-top: 1%">
                                   <div class="col-md-12">
                                   Date
                                        <asp:TextBox ID="TxtDate" class="form-control" runat="server"></asp:TextBox>
                                        <asp:TextBox Visible="false" Width="10px" runat="server" ID="TxtMeetingID"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="TxtDate">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="TxtDate"
                                            runat="server" ErrorMessage="*" ValidationGroup="Form-submit"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtDate"
                                            ErrorMessage="Invalid" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            Display="Dynamic" ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                            <br />
                                            <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" />
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
