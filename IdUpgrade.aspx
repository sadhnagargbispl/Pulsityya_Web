<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="IdUpgrade.aspx.vb" Inherits="App_UI_Application_Pages_IdUpgrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <div id="divDetailSection" runat="server" style="margin-bottom: 30px; margin-right: 20px;">
            <table cellspacing="10px" cellpadding="0%" style =" background-color :#f2f2f2; height:50%" width ="100%" ">
                <tbody>
                             
              <tr>
                <td colspan="4" style=" width: 100%">
                   <%-- <asp:Label runat="server" Style="margin-bottom: 11px" ID="lblMessage"
                        Text="Transfer Amount" Font-Bold="true" Font-Size="15px"></asp:Label>--%>
                    <br />
                </td>
            </tr>
            
                
                 <tr  style ="margin-top :20px; padding-top :20px">
                 
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" ><strong>
            Member ID : </strong>
            <td>
            <br />
            <asp:TextBox ID="TxtIDNo" runat="server" CssClass="TxtBox" AutoPostBack="true"  ></asp:TextBox>
            <asp:Label ID="LblKitId" runat="server" Visible ="false"></asp:Label>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
            <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
            <asp:Label ID="LblFormno" runat="server" Visible ="false" ></asp:Label>
            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                    </Triggers>
            </asp:UpdatePanel>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator> </td>
                </tr>
            <tr style ="margin-top :20px;padding-top :20px" >
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>
        
            Pin No. :</strong>
            </td>
            <td>
            
            <asp:TextBox ID="TxtPinNo" runat="server" CssClass="TxtBox" onkeypress="return isNumberKey(event);" AutoPostBack="true"></asp:TextBox>
             <asp:UpdatePanel ID="UpdtPanel2" runat="server">
                <ContentTemplate>
            <asp:Label ID="LblErr" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            <asp:TextBox ID ="LblScratchNo" runat="server" CssClass="label-text" Visible="false"></asp:TextBox>
            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="TxtPinNo" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                ControlToValidate="TxtPinNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
        </td>
        </tr>
           <tr style ="margin-top :20px;padding-top :20px" >
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>
            Scratch No. :</strong>
        </td>
        <td>
            <asp:TextBox ID="TxtScratchNo" runat="server" CssClass="TxtBox"></asp:TextBox>        
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                ControlToValidate="TxtScratchNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CV1" runat="server" ControlToCompare="LblScratchNo" ControlToValidate="TxtScratchNo" ValidationGroup="Save" ErrorMessage="Invalid Scratch No."></asp:CompareValidator>
        </td>
        </tr>
    <tr  style ="margin-top :20px; padding-top:20px">
                        <td width="14%" align="left" valign="middle" style="padding-left :20px;" >
                         </td>
                        <td> 
          <asp:Button ID="BtnUpgrade" runat="server" Text="Upgrade" CssClass="Btn"  ValidationGroup="Save" />
                     
            <asp:Button ID="BtnCancel" CssClass="Btn" runat="server" Text="Cancel" /></td>
            </tr>
           <%-- <br />
        </p>--%>
        <tr style ="margin-top :20px;padding-top :20px" >
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                   
            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
       </td>
       </tr>
       <tr><td>&nbsp;</td></tr>
       </tbody> 
       </table> 
       
    </div>
</asp:Content>
