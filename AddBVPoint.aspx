<%@ Page Language="VB" AutoEventWireup="false"
    CodeFile="AddBVPoint.aspx.vb" Inherits="AddBVPoint" Title="Untitled Page" %>
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
  <script type="text/javascript" language="javascript">
      function isNumberKey(evt) {
          var charCode = (evt.which) ? evt.which : event.keyCode
          if (charCode > 31 && (charCode < 48 || charCode > 57))
              return false;

          return true;
      }
    </script>
    <title></title>
      <link href="css/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:400px;padding-left:10px">
    <h5 style="border-bottom:dashed 1px #666666;margin-bottom:11px;margin-top:8px" >Add BV</h5>
   <p style="color: #666666;line-height: 25px;"> 
                        <asp:Label ID="LblAmount" runat="server" CssClass="label-text"></asp:Label>
                         <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;">
                        </span>
                      </p>
                <p style="color: #666666;line-height: 25px;">Member ID : <br />
                       
                        <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                        <asp:TextBox ID="TxtIDNo" runat="server" CssClass="TxtBox" AutoPostBack="true"></asp:TextBox><br />
                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                        <asp:TextBox ID="TxtFormNo" runat="server" CssClass="TxtBox" Visible="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                  </p>
                 <p style="color: #666666;line-height: 25px;">Leg No: <br />
                    
                        <asp:RadioButtonList ID="RbtLeg" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ValidationGroup ="Save">
                            <asp:ListItem Text="Left" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                   </p>
                 <p style="color: #666666;line-height: 25px;"> BV Point:<br />
                   
                        <asp:TextBox ID="TxtFund" runat="server" CssClass="TxtBox" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter BV Point."
                            ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                 </p>
                 
                   <p style="color: #666666;line-height: 25px;"> PV Point:<br />
                   
                        <asp:TextBox ID="txtPV" runat="server" CssClass="TxtBox" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* Enter PV Point."
                            ControlToValidate="txtPV" ValidationGroup="Save"></asp:RequiredFieldValidator>
                 </p>
                 
                 
                 <p style="color: #666666;line-height: 25px;"> Remarks : <br />
                        <asp:TextBox ID="TxtRemarks" runat="server" CssClass="TxtBox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remrks"
                            ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator>
                   </p>
                  <p align="left">
                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Add BV" OnClientClick="return confirmation();"
                            CssClass="Btn" ValidationGroup="Save" />
                    </p>
                 <p align="left">
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                       </p>
    </div>
   </form>
   </body></html> 