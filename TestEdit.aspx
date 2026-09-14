<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TestEdit.aspx.vb" Inherits="App_UI_Application_Pages_TestEdit" %>

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
  <%--  <h5 style="border-bottom:dashed 1px #666666;margin-bottom:11px;margin-top:8px" >State Master</h5>--%>
   <p style="color: #666666;line-height: 25px;"> Member ID : <br />
    <asp:TextBox CssClass="TxtBox" ID="txtmemid" runat="server" Enabled="false"></asp:TextBox>
    </p> 
  
    <p style="color: #666666;line-height: 25px;"> Description: <br />
    <asp:TextBox CssClass="TxtBox" ID="txtdesc" runat="server" MaxLength="250"  TextMode="MultiLine" Width="450Px" height="120"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtdesc" runat="server" ValidationGroup="save" >*</asp:RequiredFieldValidator>
  <asp:RegularExpressionValidator runat='server' id='regex1' ControlToValidate='txtdesc' ValidationExpression="^[\s\S]{0,250}$" ValidationGroup="save" ErrorMessage="Max 250 Characters Allowed" />
     </p> 
     
    
    <p align="left">
    <asp:Button ID="BtnSave" CssClass="Btn"  runat="server" Text="Save" ValidationGroup="save" />

    </p>
    </div>
    </form>
</body>
</html>
