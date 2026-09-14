<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCity.aspx.vb" Inherits="App_UI_Application_Pages_AddCity" %>

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
    <link href="../Resources/CSS/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:200px;padding-left:10px">
    <h4 style="border-bottom:dashed 1px #666666;margin-bottom:11px;margin-top:8px" >City Master</h4>
   <p style="color: #666666;line-height: 25px;"> State Name : <br />
    <asp:TextBox CssClass="TxtBox" ID="txtStateName" ReadOnly="true" runat="server"></asp:TextBox>
    </p> 
   <p style="color: #666666;line-height: 25px;"> City Name : <br />
    <asp:TextBox CssClass="TxtBox" ID="txtCityName" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCityName" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p> 
   <p style="color: #666666;line-height: 25px;"> Remarks : <br />
    <asp:TextBox CssClass="TxtBox" ID="txtRemarks" runat="server"></asp:TextBox>
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
    <asp:TextBox  ID="txtStateCode" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox  ID="txtCityCode" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox  ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox  ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
    </p>
    </div>
    </form>
</body>
</html>
