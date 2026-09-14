<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="PackageUpdate.aspx.vb" Inherits="App_UI_Application_Pages_PackageUpdate" title="Update Package" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     p {
font-weight: bold;
color: #666666;
margin: 0px;
line-height: 25px;
width: 800px;
padding-bottom:8px;
padding-top:5px;
text-align:left;
padding-left:10px;
}
</style>
<asp:Panel ID="pnlChoice" runat="server" BackColor="Transparent" 
        GroupingText="Choose option to Change Package :" BorderColor="#999999" 
        Font-Bold="True" BorderStyle="None" BorderWidth="0px">
<%--<p> <asp:Label ID="lbltext" runat="server" Text="Choose option to activate members : "></asp:Label></p>--%>
<p><asp:RadioButtonList ID="rdblistChoice" runat="server" AutoPostBack="True" 
        CellPadding="2" CellSpacing="5"> 
    <asp:ListItem Selected="True" Value="single">Change Package of Single Member</asp:ListItem>
    <asp:ListItem Value="multiple">Change Package of Multiple Members</asp:ListItem>
</asp:RadioButtonList>
</p>
</asp:Panel>
<div style="padding-left:10px;padding-top:10px;padding-bottom:10px;font-size:13px;" align="center">
 <asp:Label style="padding-left:10px;padding-top:5px" ID="lblError" runat="server" Visible="False" Font-Bold="True"></asp:Label>
 </div>
 
   <div id="divSingle" runat="server">
     <p style="color: #666666;line-height: 25px;"> &nbsp;&nbsp; Enter Member ID :&nbsp;&nbsp; 
    <asp:TextBox CssClass="TxtBox" ID="txtMemberId" runat="server" Width="200px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    <asp:Button ID="btnShowSingleDetail" CssClass="Btn"  runat="server" Text="View Detail" ValidationGroup="Save" 
    style="float:right;margin-right:370px;margin-top:2px;" />
    </p>
    </div>
    
   <div id="divMultiple" runat="server">
   <div>
   <table>
   <tr>
   <td><asp:Button ID="btnTemplate" runat="server" CssClass="Btn" 
           Text="Download template" ToolTip="Download excel template for Package updation process." 
           CausesValidation="False" /></td>
   <td><asp:Button ID="btnUpload" runat="server" CssClass="Btn" Text="Upload Excel" 
           ToolTip="upload excel file which contains members Id for update process." 
           CausesValidation="False" /></td>
   <td></td>
   </tr>
   <tr>
   <td colspan="3">
   <div id="divUpload" runat="server">
   <table>
   <tr>
   <td><asp:FileUpload ID="fileUpload" runat="server"/></td>
   <td><asp:Button ID="btnUpload1" runat="server" CssClass="Btn" Text="Upload" ToolTip="upload excel file which contains members Id for Update process." CausesValidation="False" /> </td>
    </tr> </table>
   </div>
   </td>
   </tr>
   </table>   
    </div>
   </div>
   
   <div id="divDropDown" runat="server" visible="false">
 <table>
 <tr>
 <td></td>
 <td> <asp:Label ID="lblpack" runat="server" Text="Select Package : " 
         Font-Bold="True" ></asp:Label> </td>
 <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlPackage" runat="server" CssClass="ddlSearch" Width="335px"></asp:DropDownList> </td>
 <td> <asp:Button ID="btnUpdate" CssClass="Btn"  runat="server" Text="Update Package" ValidationGroup="Save" /></td>
 </tr>
 </table>
 </div>
   
    <div>
    <table style="margin-left:0px;margin-top:0px;">
    <tr>
    <td style="float:right;padding-right:30px;"><asp:Label ID="lblrecordcount" runat="server" Text="" Font-Bold="True"></asp:Label> </td>
    </tr>
  <tr>
  <td>
   <asp:GridView ID="GvData" runat="server"  
    AutoGenerateColumns="true" RowStyle-Height="25px" 
    GridLines="None"  
    AllowPaging="true"
    CssClass="table table-striped table-advance table-hover"  
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="20" EmptyDataText="No data to display." Visible="false" >  
   <%-- <Columns>
        <asp:BoundField DataField="IdNo" HeaderText="Member ID" />
        <asp:BoundField DataField="MemFirstName" HeaderText="Member First Name" />
        <asp:BoundField DataField="MemLastName" HeaderText="Member Last Name" />
    </Columns>--%>
</asp:GridView>
</td>
</tr>
</table>
</div>
    <br />
</asp:Content>

