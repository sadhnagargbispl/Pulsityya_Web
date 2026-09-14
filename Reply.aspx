<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Reply.aspx.vb" Inherits="App_UI_Application_Pages_Reply" %>

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
    <div style="height:450px;padding-left:10px">
    <h4 style="border-bottom:dashed 1px #666666;margin-bottom:11px;margin-top:8px" >Reply</h4>
   <p style="color: #666666;line-height: 15px;vertical-align:middle;"> To : 
    <asp:label CssClass="TxtBox" ID="LblMemName" Width="350px" runat="server"></asp:label>
    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
    </p> 
    <p style="color: #666666;line-height: 15px;"> Complaint type : 
    <asp:label CssClass="TxtBox"  ID="LblCType" Width="278px" runat="server"></asp:label>
    </p>
      <p style="color: #666666;"> Complaint: <br />
    <asp:TextBox CssClass="TxtBox" ID="TxtComplaint" ReadOnly="true" TextMode="MultiLine" Height="50px" runat="server"></asp:TextBox>
    </p>
     <p style="color: #666666;"> Reply: <br />
    <asp:TextBox CssClass="TxtBox" ID="TxtReply" MaxLength="5000"  TextMode="MultiLine" Height="70px" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RFV1" runat="server" style="vertical-align:top" ControlToValidate="TxtReply" ValidationGroup="Send" ErrorMessage="*"></asp:RequiredFieldValidator>
    </p>
     <p align="left" style="margin-top:0px;">
    <asp:Button ID="BtnSave" CssClass="Btn"  runat="server" Text="Send" ValidationGroup="Send" />
    <asp:TextBox  ID="txtGrpID" runat="server" Visible="false"></asp:TextBox><br />
    </p>
     <p style="border-bottom:dashed 1px #666666;margin-bottom:5px;margin-top:15px;line-height:normal" >Previous Reply</p>
       <asp:TextBox CssClass="TxtBox" ID="TxtPreReply" ReadOnly="true" TextMode="MultiLine" Height="100px" runat="server"></asp:TextBox>
   
   
    </div>
    </form>
</body>
</html>
