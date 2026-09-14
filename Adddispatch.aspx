<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Adddispatch.aspx.vb" Inherits="App_UI_Application_Pages_Adddispatch"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        p
        {
            font-weight: bold;
            color: #666666;
            margin: 0px;
            line-height: 25px;
            width: 400px;
            padding-bottom: 8px;
            text-align: left;
        }
    </style>
    <style type="text/css">
        .DDl
        {
            display: block;
            width: 200px;
            height: 24px;
            padding: 3px 16px;
            font-size: 14px;
            line-height: 1.428571429;
            color: #8e8e93;
            vertical-align: middle;
            background-color: #ffffff;
            border: 1px solid #c7c7cc;
            border-radius: 4px;
            -webkit-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }
    </style>
    <link href="css/Main.css" rel="stylesheet" type="text/css" />
    <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <ajax:ToolkitScriptManager ID="scriptmanager1" runat="server">
    </ajax:ToolkitScriptManager>
    <%--<div style="height: 550px; padding-left: 10px">
        <h4 style="border-bottom: dashed 1px #666666; margin-bottom: 11px; margin-top: 8px">--%>
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Dispatch Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <%--<p style="color: #666666; line-height: 25px;">--%>
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%; margin-bottom :5px;">
                                    <div class="col-md-4">
                                        Idno : &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</div>
                                    <div class="col-md-6">
                                        <%--<br />--%>
                                        <asp:TextBox CssClass="TxtBox" ID="txtIdno" runat="server" AutoPostBack ="true" ></asp:TextBox><br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtIdno"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    <asp:Label ID="lblRefralNm" runat="server" ForeColor="#D11F7B"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%; margin-bottom :5px;">
                                    <div class="col-md-4">
                                        Package Name :</div>
                                    <div class="col-md-6">
                                        <%--<br />--%>
                                        <asp:TextBox CssClass="TxtBox" ID="txtpackage" runat="server" ReadOnly ="true" ></asp:TextBox><br />
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtpackage"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="col-md-2">
                                    <%--<asp:Label ID="Label1" runat="server" ForeColor="#D11F7B"></asp:Label>--%>
                                    </div>
                                </div>
                                <%--
        </p>--%>
                                <%--<p style="color: #666666; line-height: 25px;">--%>
                                <div class="col-md-12" style="padding-top: 1%; margin-bottom :5px;">
                                    <div class="col-md-4">
                                        Date :&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</div>
                                    <div class="col-md-6">
                                        <%--  <br />--%>
                                        <asp:TextBox CssClass="TxtBox" ID="txtFrmDate" runat="server"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFrmDate" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </ajax:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFrmDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit" Display="Dynamic"></asp:RegularExpressionValidator>
                                        <%-- </p>--%>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <%-- <p style="color: #666666; line-height: 25px;">--%>
                                <div class="col-md-12" style="padding-top: 1%; margin-bottom :5px;">
                                    <div class="col-md-4">
                                        Remarks :&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</div>
                                    <div class="col-md-6">
                                        <%-- <br />--%>
                                        <asp:TextBox CssClass="TxtBox" ID="txtRemarks" TextMode ="MultiLine" runat="server"></asp:TextBox>
                                        <%--</p>--%>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <%--  <p style="color: #666666; line-height: 25px;">--%>
                                <div class="col-md-12" style="padding-top: 1%; visibility:hidden ;">
                                    <div class="col-md-4">
                                        Status :</div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                            <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                        </asp:RadioButtonList>
                                       <%-- </p>--%>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <%--  <p align="left">--%>
                                <br />
                                <br />
                                <div class="col-md-12" style="padding-top: 1%;">
                                    <div class="col-md-4">
                                     <asp:TextBox ID="txtDID" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtAIdID" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtkitid" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                       
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" />
                                        <%--</p>--%>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <!-- end of weather widget -->
            </div>
        </div>
    </div>
    </form>
</body>
</html>
