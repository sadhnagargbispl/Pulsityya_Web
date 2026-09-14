<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddAchievers.aspx.vb" Inherits="App_UI_Application_Pages_AddAchievers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
 <style type="text/css">
     p {
font-weight: bold;
color: #666666;
margin: 0px;
line-height: 25px;
width: 400px;
padding-bottom:8px;
text-align:left;
}
</style>
    <title></title>
     <link href="css/Main.css" rel="stylesheet" type="text/css" />
    <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:350px;padding-left:10px">
    <h5 style="border-bottom:dashed 1px #666666;margin-bottom:11px;margin-top:8px" >Achievers Master</h5>
    <p style="color: #666666;line-height: 25px;"> Session Id : <br />
 <asp:TextBox  ID="TxtSession" runat="server" CssClass ="TxtBox "></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="TxtSession" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p> 
    <p style="color: #666666;line-height: 25px;"> ID No : <br />
    <asp:TextBox CssClass="TxtBox" ID="TxtIdNo" runat="server" AutoPostBack ="true" ></asp:TextBox><br /><asp:Label ID="LblError" runat="server" Visible ="false"></asp:Label>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="TxtIdNo" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p> 
   <p style="color: #666666;line-height: 25px;"> Achiever Name : <br />
    <asp:TextBox CssClass="TxtBox" ID="txtMemberName" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberName" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p> 
   
    <p style="color: #666666;line-height: 25px;"> Status : <br />
    <asp:RadioButtonList id="rdblist" runat="server" CellPadding="0" CellSpacing="10" 
            RepeatDirection="Horizontal">
    <asp:ListItem selected="true" Text="Active">Active</asp:ListItem>
    <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
    </asp:RadioButtonList>
    </p>
    <p align="left">
    <asp:Button ID="BtnSave" CssClass="Btn"  runat="server" Text="Save" ValidationGroup="Save" />
    <asp:TextBox  ID="txtBankCode" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox  ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox  ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
    </p>
    </div>
    </form>
</body>
</html>
