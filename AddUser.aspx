<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddUser.aspx.vb" Inherits="App_UI_Application_Pages_AddUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                                    User Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 2%">
                                    <div class="col-md-4">
                                        Department Name :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtGrpName" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 2%">
                                    <div class="col-md-4">
                                        User Name :&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</div>
                                    <div class="col-md-6">
                                        <%--<asp:TextBox ID="txtUsrName" class="form-control" runat="server" AutoPostBack="true"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtUsrName" runat="server" CssClass="form-control" AutoPostBack="true"
                                            onkeypress="return NoSpace(event);">
                                        </asp:TextBox>

                                        <script type="text/javascript">
    function NoSpace(e) {
        var key = e.which || e.keyCode;
        if (key == 32) { // Space key
            return false;
        }
        return true;
    }
                                        </script>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtUsrName"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 2%">
                                    <div class="col-md-4">
                                        Password :&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtPswd" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtPswd"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 2%">
                                    <div class="col-md-4">
                                        Mobile No :&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtMobileNo" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="TxtMobileNo"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator></div>
                                </div>
                                <div class="col-md-2">
                                </div>
                            </div>
                            <div class="col-md-12" style="padding-top: 2%">
                                <div class="col-md-4">
                                    Remarks :&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtRemarks" class="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                </div>
                            </div>
                            <div class="col-md-12" style="padding: 2%">
                                <div class="col-md-4">
                                    Status :&nbsp&nbsp&nbsp&nbsp
                                </div>
                                <div class="col-md-6">
                                    <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                        <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-2">
                                </div>
                            </div>
                            <div class="col-md-12" style="padding: 2%">
                                <div class="col-md-6">
                                    <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" />
                                    <asp:TextBox ID="txtGrpID" runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtUserID" runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                </div>
                            </div>
                        </div>
    </form>
</body>
</html>
