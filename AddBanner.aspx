<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddBanner.aspx.vb" Inherits="App_UI_Application_Pages_AddBanner"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        td
        {
            font-weight: bold;
            color: #666666;
            margin: 0px;
            line-height: 25px;
            padding-bottom: 8px;
            text-align: left;
        }
    </style>
    <title></title>
    <link href="css/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 200px; padding-left: 10px">
        <h4 style="border-bottom: dashed 1px #666666; margin-bottom: 11px; margin-top: 8px">
            Banner Master</h4>
        <p style="color: #666666; line-height: 25px;">
            <table>
                <tr>
                    <td width="10%">
                        Banner Image :
                    </td>
                    <td width="50%">
                        <asp:FileUpload runat="server" ID="FlUpld1" multiaccept="Image/*" />
                        Expected Size: 1920 x 600 &nbsp;&nbsp;
                        <asp:Label ID="LblPreImage" Text="Previous Image" runat="server" Visible="false"></asp:Label>
                        <%--  <asp:RequiredFieldValidator runat="server" ValidationGroup="Save" ID="REqFV1" ControlToValidate="FlUpld1" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                    </td>
                    <tr>
                        <td>
                            Remarks :
                        </td>
                        <td>
                            <asp:TextBox CssClass="TxtBox" ID="txtRemarks" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Status :
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="5"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="true" Text="Active" Value="Y">Active</asp:ListItem>
                                <asp:ListItem Text="DeActive" Value="N">DeActive</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
            </table>
            <asp:Button ID="BtnSave" CssClass="Btn" runat="server" Text="Save" ValidationGroup="Save" />
            <asp:TextBox ID="txtBID" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
        </p>
    </div>
    </form>
</body>
</html>
