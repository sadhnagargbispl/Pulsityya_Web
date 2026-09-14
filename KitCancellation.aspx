<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="KitCancellation.aspx.vb" Inherits="App_UI_Application_Pages_KitCancellation" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <style type="text/css" >
       .DDl {
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
}</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<ol class="breadcrumb" style =" margin-right:20px">
						<li><i class="fa fa-home"></i><a href="Home.aspx">Home</a></li>
						<li>Member</li>
						<li>Kit Cancellation </li>
						
					</ol>
  <div id="divDetailSection" runat="server" style="margin-bottom: 30px; margin-right: 20px;">
            <table cellspacing="10px" cellpadding="0%" style =" background-color :White; height:50%" width ="100%" ">
                <tbody>
                             
              <tr>
                <td colspan="4" style=" width: 100%">
                     <span id="lblStock" runat="server" style="color: #000; font-weight:bold;font-size:14px;"></span>               
              
                   <%-- <asp:Label runat="server" Style="margin-bottom: 11px" ID="lblMessage"
                        Text="Transfer Amount" Font-Bold="true" Font-Size="15px"></asp:Label>--%>
                    <br />
                </td>
            </tr>
          <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                  <tr  style ="margin-top :20px; padding-top :20px">
                 
                        
                 
               <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
            <strong>Member ID :
            </strong>
            </td>
            <td>
            
            <asp:TextBox ID="TxtIDNo" runat="server" CssClass="TxtBox" AutoPostBack="true"  ></asp:TextBox>
            <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label> </td>
            </tr>
            <tr></tr>
            <tr >
                    <td colspan ="4"> <br /></td></tr>
        <tr  style ="margin-top :20px; padding-top :20px">
                <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                <strong >
            Package :</strong>
          <td>
            <asp:DropDownList ID="CmbKit"  runat="server"  CssClass ="DDl">
            </asp:DropDownList>  
            </td>                     
       
            
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                    </Triggers>
         
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
      </tr> 
      </ContentTemplate>
         </asp:UpdatePanel>
      <tr >
                    <td colspan ="4"> <br /></td></tr>
       
      <tr  style ="margin-top :20px; padding-top:20px">
                        <td width="14%" align="left" valign="middle" style="padding-left :20px;" >
                           <%--<strong>Transfer Amount :&nbsp;</strong>--%>
                        </td>
                        <td>
            <asp:Button ID="BtnGenerate" runat="server" Text="Issue" CssClass="Btn"  ValidationGroup="Cancel Activation" />
            <asp:Button ID="BtnCancel" CssClass="Btn" runat="server" Text="Cancel" ValidationGroup="Cancel" />
            </td>
            </tr> 
          <tr style ="margin-top :20px;padding-top :20px" >
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                   
            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
            </td> 
            </tr>
            <tr>
            <td>&nbsp;</td>
            </tr>
            </tbody> </table> 
       
      
    </div>
</asp:Content>

