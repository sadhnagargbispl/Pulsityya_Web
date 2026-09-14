<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="BecomeFranchise.aspx.vb" Inherits="App_UI_Application_Pages_BecomeFranchise"
    Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
}
</style>
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
                 
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>Enter Member Id : &nbsp;</strong>
                        </td>
                        <td width="22%">                           
                       <asp:TextBox CssClass="TxtBox" ID="txtMemberId" runat="server" AutoPostBack="True"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>   </td>
                        <td align="left">
                        <asp:Label ID="LblMemName" runat="server" ></asp:Label>
                                                <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="LblPassw" runat="server" Visible="false"></asp:Label>
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
                           <strong> Group Name:&nbsp;</strong>
                        </td>
                        <td>                           
                        <asp:DropDownList Id="DDlGroup" runat="server" cssClass="DDl" AutoPostBack ="true"></asp:DropDownList>                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4">
                    <br /></td></tr>
                
                <tr  style ="margin-top :20px; padding-top :20px">
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>Parent Id :&nbsp;</strong>
                        </td>
                        <td>  
                        <asp:DropDownList Id="DDlParentId" runat="server" cssClass="DDl" ></asp:DropDownList>                    
 </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4"> <br /></td></tr>
                      <tr  style ="margin-top :20px; padding-top :20px">
                        <td width="20%" align="left" valign="middle" style="padding-left :20px;" >
                           <strong>Remark :&nbsp;</strong>
                        </td>
                        <td>  
                   <asp:TextBox ID="TxtRemark" runat="server" CssClass ="TxtBox " ></asp:TextBox>                 
 </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr >
                    <td colspan ="4"><br /></td></tr>
                    
                    <tr  style ="margin-top :20px; padding-top:20px">
                        <td width="14%" align="left" valign="middle" style="padding-left :20px;" >
                           <%--<strong>Transfer Amount :&nbsp;</strong>--%>
                        </td>
                        <td>  
                         <asp:Button ID="btnchngpswd" CssClass="Btn"  runat="server" Text="Become Franchisee" ValidationGroup="Save" />                         
                           
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