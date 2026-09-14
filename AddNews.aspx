<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddNews.aspx.vb" Inherits="App_UI_Application_Pages_AddNews"
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
</head>
<body>
    <form id="form1" runat="server">
    <ajax:ToolkitScriptManager ID="scriptmanager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div style="height: 550px; padding-left: 10px">
        <h4 style="border-bottom: dashed 1px #666666; margin-bottom: 11px; margin-top: 8px">
            News Master</h4>
        <p style="color: #666666; line-height: 25px;" id="ptype" runat="server">
            Type :
            <br />
            <asp:DropDownList ID="DDLType" CssClass="DDl " runat="server" AutoPostBack="true"
                Width="350px">
                <asp:ListItem Text="News" Value="N" Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
            <%--  <asp:TextBox CssClass="TxtBox" ID="txtAchieverName" runat="server"></asp:TextBox>--%>
        </p>
        <p style="color: #666666; line-height: 25px;">
            Heading :
            <br />
            <asp:TextBox CssClass="TxtBox" ID="txtHeading" runat="server"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtHeading"
                runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
        </p>
        <p style="color: #666666; line-height: 25px;">
            Detail :
            <br />
            <asp:TextBox CssClass="TxtBox" ID="txtDetail" runat="server" TextMode="MultiLine"
                Height="200px"></asp:TextBox>
            <ajax:HtmlEditorExtender ID="HtmlEditorExtender1" runat="server" TargetControlID="txtDetail"
                EnableSanitization="false">
            </ajax:HtmlEditorExtender>
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtPswd" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    --%></p>
        <p style="color: #666666; line-height: 25px;">
            From Date :
            <br />
            <asp:TextBox CssClass="TxtBox" ID="txtFrmDate" runat="server"></asp:TextBox>
            <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFrmDate" Format="dd-MMM-yyyy"
                runat="server">
            </ajax:CalendarExtender>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFrmDate"
                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                ValidationGroup="Form-submit" Display="Dynamic"></asp:RegularExpressionValidator>
        </p>
        <p style="color: #666666; line-height: 25px;">
            To Date :
            <br />
            <asp:TextBox CssClass="TxtBox" ID="txtToDate" runat="server"></asp:TextBox>
            <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="dd-MMM-yyyy"
                runat="server">
            </ajax:CalendarExtender>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtToDate"
                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                ValidationGroup="Form-submit" Display="Dynamic"></asp:RegularExpressionValidator>
        </p>
        <p style="color: #666666; line-height: 25px;">
            Remarks :
            <br />
            <asp:TextBox CssClass="TxtBox" ID="txtRemarks" runat="server"></asp:TextBox>
        </p>
        <p style="color: #666666; line-height: 25px;">
            Status :
            <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                RepeatDirection="Horizontal">
                <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
            </asp:RadioButtonList>
        </p>
        <p style="color: #666666; line-height: 25px;" id="pVenue" visible="false" runat="server">
            Venue :
            <br />
            <asp:TextBox CssClass="TxtBox" ID="txtVenue" runat="server"></asp:TextBox>
        </p>
        <p style="color: #666666; line-height: 25px;" id="PState" visible="false" runat="server">
            State :
            <br />
            <asp:DropDownList ID="DlState" runat="server" CssClass="DDl " Width="350px">
            </asp:DropDownList>
            <%--           <asp:TextBox CssClass="TxtBox" ID="TxtState" runat="server"></asp:TextBox>--%>
        </p>
        <p style="color: #666666; line-height: 25px;" id="pcity" visible="false" runat="server">
            City :
            <br />
            <asp:TextBox CssClass="TxtBox" ID="TxtCity" runat="server"></asp:TextBox>
        </p>
        <p style="color: #666666; line-height: 25px;" id="pLeader" visible="false" runat="server">
            Leader Name :
            <br />
            <asp:TextBox CssClass="TxtBox" ID="txtLeader" runat="server"></asp:TextBox>
        </p>
        <p style="color: #666666; line-height: 25px;" id="pPhoneNo" visible="false" runat="server">
            PhoneNo:
            <br />
            <asp:TextBox CssClass="TxtBox" ID="txtPhone" runat="server"></asp:TextBox>
        </p>
        <p align="left">
            <asp:Button ID="BtnSave" CssClass="Btn" runat="server" Text="Save" ValidationGroup="Save" />
            <asp:TextBox ID="txtNewsID" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtAIdID" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
        </p>
    </div>
    </form>
</body>
</html>
