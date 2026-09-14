<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddFund.aspx.vb" Inherits="App_UI_Application_Pages_AddFund" %>

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
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                 Add Fund</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                           <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-3" style="padding-top: 1%">
                                    <div class="col-md-4">
                               Session:</div>
                                  <div class="col-md-6">
                                 <asp:DropDownList ID="ddlSession" runat="server" class="form-control" Style="text-indent: 1px;">
                            </asp:DropDownList>
                                    </div>
                                    </div>
                                  
                           
                               <%-- <div class="col-md-12" style="padding-top: 3%">
                                    <div class="col-md-4">
                                        Id No :</div>
                                    <div class="col-md-4
                                    ">
                                        <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtIDNo" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                        <asp:TextBox ID="TxtFormNo" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>--%>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                       Master Repurehes:</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtMasterRepurehes" runat="server" style=" margin-left :21%"></asp:TextBox></div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Brand Amount :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtBrandAmount" runat="server" style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                
                               </div>
                               
                                    
                                <%--<div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Join Color :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                    <div class="col-md-6">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButtonList ID="RbtColor" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Green" Selected="True" Value="Green.jpg"><img src="images/Green.jpg" /></asp:ListItem>
                                            <asp:ListItem Text="Red" Selected="True" Value="Red.jpg"><img src="images/Red.jpg" /></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>--%>
                                   <%-- <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Status :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="rdblist" runat="server" Width="150px" CellPadding="0" CellSpacing="10"
                                            RepeatDirection="Horizontal" style="margin-left:16%">
                                            <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                            <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-6">
                                           <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtKitId" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                    </div>--%>
                                    <br />
                                   
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" style="margin-left :50%" />
                                 
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
