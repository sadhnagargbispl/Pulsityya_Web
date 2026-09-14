<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCityNew.aspx.vb" Inherits="AddCityNew" %>

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
                                    City Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-6">
                                        State Name&nbsp;&nbsp;&nbsp;:
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-6">
                                        District Name:
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlDistrictCode" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="--Choose District Name--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        City Name:
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtCityName" class="form-control" runat="server" AutoPostBack="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCityName"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding: 10px">
                                    <div class="col-md-4">
                                        Status :
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
                                <div class="col-md-12" style="padding-left: 32%">
                                    <div class="col-md-4">
                                        <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" />
                                        <asp:TextBox ID="txtCityCode" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-8">
                                    </div>
                                </div>
                            </div>
    </form>
</body>
</html>
