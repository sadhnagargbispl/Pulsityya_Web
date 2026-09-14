<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCountry.aspx.vb" Inherits="App_UI_Application_Pages_AddCountry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <ajaxtoolkit:ToolkitScriptManager id="scriptmanager1" runat="server">
</ajaxtoolkit:ToolkitScriptManager>
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Country Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Country Name :
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtStateName" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtStateName"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Country Code:
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtStateCode" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="txtStateCode"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                        <ajax:FilteredTextBoxExtender ID="RR1" runat="server" ValidChars="1123456789" TargetControlID="txtStateCode">
                                        </ajax:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Std Code:
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtStdCode" class="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtStdCode"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding: 1%">
                                    <div class="col-md-2">
                                        Remarks :&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtRemarks" class="form-control" runat="server"></asp:TextBox></div>
                                    <div class="col-md-8">
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
                                        <%--<asp:TextBox ID="txtStateCOde" runat="server" Visible="false"></asp:TextBox>--%>
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
