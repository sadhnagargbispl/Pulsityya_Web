<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb" Inherits="App_UI_Application_Pages_ChangePassword" title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <link href="../Resources/CSS/Main.css" rel="stylesheet" type="text/css" />
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
 <%-- <div style="height:200px;padding-left:10px">
    <h5 style="border-bottom:dashed 1px #666666;margin-bottom:11px;margin-top:8px" >Change Password</h5>
   <p style="color: #666666;line-height: 25px;"> Enter Old Password : <asp:TextBox CssClass="TxtBox" ID="txtOldPaswd" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtOldPaswd" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p> 
     <p style="color: #666666;line-height: 25px;"> Enter New Password : <asp:TextBox CssClass="TxtBox" ID="txtNewPswd" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="txtNewPswd" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p> 
    <p style="color: #666666;line-height: 25px;"> Re-enter New Password : <asp:TextBox CssClass="TxtBox" ID="txtReNewPswd" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtNewPswd" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
     </p> 
     
   <p align="left">
    <asp:Button ID="btnchngpswd" CssClass="Btn"  runat="server" Text="Change Password" ValidationGroup="Save" />
     </p>
     
     <p>
     <asp:Label ID="lblMsg" runat="server" Text="" Visible="false"></asp:Label>
     <asp:TextBox  ID="txtGrpID" runat="server" Visible="false"></asp:TextBox>
     <asp:TextBox  ID="txtusername" runat="server" Visible="false"></asp:TextBox>
     <asp:TextBox  ID="txtRemarks" runat="server" Visible="false"></asp:TextBox>
     <asp:TextBox  ID="txtIPAddress" runat="server" Visible="false"></asp:TextBox>
     </p>
  </div>--%>
  
  
   <ol class="breadcrumb" style =" margin-right:20px">
						<li><i class="fa fa-home"></i><a href="Home.aspx">Home</a></li>
						<li>Change Password</li>
						
					</ol>
  
    <div id="divDetailSection" runat="server" style="margin-bottom: 30px; margin-right: 20px;">
            <table cellspacing="10px" cellpadding="0%" style =" background-color :White; height:50%" width ="100%" ">
                <tbody>
                             
              <tr>
                <td colspan="4" style=" width: 100%">
                   <%-- <asp:Label runat="server" Style="margin-bottom: 11px" ID="lblMessage"
                        Text="Transfer Amount" Font-Bold="true" Font-Size="15px"></asp:Label>--%>
                    <br />
                </td>
            </tr>
            
                
                 <tr  style ="margin-top :20px; padding-top :20px">
                 
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>Enter Old Password : &nbsp;</strong>
                        </td>
                        <td>                           
                       <asp:TextBox CssClass="TxtBox" ID="txtOldPaswd" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtOldPaswd" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>   </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4"> <br /></td></tr>
                  
                <tr style ="margin-top :20px;padding-top :20px" >
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong> Enter New Password :&nbsp;</strong>
                        </td>
                        <td>                           
                          <asp:TextBox CssClass="TxtBox" ID="txtNewPswd" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="txtNewPswd" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator></td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4">
                    <br /></td></tr>
                    <%--<br />--%>
                <tr  style ="margin-top :20px; padding-top :20px">
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>Re-enter New Password :&nbsp;</strong>
                        </td>
                        <td>                           
                        <asp:TextBox CssClass="TxtBox" ID="txtReNewPswd" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtNewPswd" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>  </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4"> <br /></td></tr>
                    
                    <tr  style ="margin-top :20px; padding-top:20px">
                        <td width="14%" align="left" valign="middle" style="padding-left :20px;" >
                           <%--<strong>Transfer Amount :&nbsp;</strong>--%>
                        </td>
                        <td>  
                         <asp:Button ID="btnchngpswd" CssClass="Btn"  runat="server" Text="Change Password" ValidationGroup="Save" />                         
                           
                            <%--<asp:TextBox ID="TextBox1" CssClass="TxtBox" runat="server" Width="200px" ForeColor="black" Font-Bold="true" Font-Size="12px" ></asp:TextBox>--%>
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    
                   <tr>
                <td colspan="4" style=" width: 100%">
                <asp:Label ID="lblMsg" runat="server" Text="" Visible="false"></asp:Label>
     <asp:TextBox  ID="txtGrpID" runat="server" Visible="false"></asp:TextBox>
     <asp:TextBox  ID="txtusername" runat="server" Visible="false"></asp:TextBox>
     <asp:TextBox  ID="txtRemarks" runat="server" Visible="false"></asp:TextBox>
     <asp:TextBox  ID="txtIPAddress" runat="server" Visible="false"></asp:TextBox>
                   <%-- <asp:Label runat="server" Style="margin-bottom: 11px" ID="lblMessage"
                        Text="Transfer Amount" Font-Bold="true" Font-Size="15px"></asp:Label>--%>
                    <br />
                </td>
            </tr>
                    <tr >
                    <td colspan ="4"></td></tr>
                    <tr >
                        <td align="Left" valign="middle">
                        <asp:Label ID="LblError" runat="server" visible="false"></asp:Label>
                        </td>
                        </tr> 
                        
                        
                            <tr >
                        <td align="Left" valign="middle">
                       &nbsp;
                        </td>
                        </tr> 
                    
                                   </tbody>
            </table>
        </div>
</asp:Content>

