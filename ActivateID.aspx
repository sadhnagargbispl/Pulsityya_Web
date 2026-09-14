<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="ActivateID.aspx.vb" Inherits="App_UI_Application_Pages_ActivateID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">
    function SelectAll(id) {
        //get reference of GridView control
        var grid = document.getElementById("<%= GvData.ClientID %>");
        //variable to contain the cell of the grid
        var cell;

        if (grid.rows.length > 0) {
            //loop starts from 1. rows[0] points to the header.
            for (i = 1; i < grid.rows.length; i++) {
                //get the reference of first column
                cell = grid.rows[i].cells[0];

                //loop according to the number of childNodes in the cell
                for (j = 0; j < cell.childNodes.length; j++) {
                    //if childNode type is CheckBox                 
                    if (cell.childNodes[j].type == "checkbox") {
                        //assign the status of the Select All checkbox to the cell 
                        //checkbox within the grid
                        cell.childNodes[j].checked = document.getElementById(id).checked;
                    }
                }
            }
        }
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     p {
font-weight: bold;
color: #666666;
margin: 0px;
line-height: 25px;
width: 600px;
padding-bottom:8px;
padding-top:5px;
text-align:left;
padding-left:10px;
}
</style>

<asp:Panel ID="pnlChoice" runat="server" BackColor="Transparent" 
        GroupingText="Choose option to activate members :" BorderColor="#999999" 
        Font-Bold="True" BorderStyle="None" BorderWidth="0px">
<%--<p> <asp:Label ID="lbltext" runat="server" Text="Choose option to activate members : "></asp:Label></p>--%>
<p><asp:RadioButtonList ID="rdblistChoice" runat="server" AutoPostBack="True" 
        CellPadding="2" CellSpacing="5"> 
    <asp:ListItem Selected="True" Value="single">Activate Single Member</asp:ListItem>
    <asp:ListItem Value="multiple">Activate Multiple Members</asp:ListItem>
</asp:RadioButtonList>
</p>
</asp:Panel>
<div style="padding-left:10px;padding-top:10px;padding-bottom:10px;font-size:13px;" align="center">
 <asp:Label style="padding-left:10px;padding-top:5px" ID="lblError" runat="server" Visible="False" Font-Bold="True"></asp:Label>
 </div>
    <div id="divSingle" runat="server">
     <p style="color: #666666;line-height: 25px;"> &nbsp;&nbsp; Enter Member ID No. :&nbsp;&nbsp; 
    <asp:TextBox CssClass="TxtBox" ID="txtMemberId" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p>
    <p style="color: #666666;line-height: 25px;"> &nbsp;&nbsp; Select Kit :&nbsp;&nbsp; 
    <asp:DropDownList CssClass="ddl" ID="CmbKit" runat="server"></asp:DropDownList>
    &nbsp; Paymode : <asp:DropDownList CssClass="ddl" ID="DdlPaymode" runat="server" AutoPostBack="true"></asp:DropDownList>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
    <div id="DvPaymode" runat="server" visible="false">
    <table><tr><td>
    <asp:Label ID="LblDDNo" runat="server" Text="Cheque/D.D. No.:"></asp:Label>
    <asp:TextBox ID="TxtDDNo" runat="server" />
    </td>
    <td>
    <asp:Label ID="LblDDDate" runat="server" Text="Cheque/D.D. Date:"></asp:Label>
    <asp:TextBox ID="TxtDDDate" runat="server" />
    <ajaxtoolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TxtDdDate" 
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TxtDdDate"
                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True" Display="Dynamic"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
    </td>
    </tr>
    <tr><td>
    Issued Bank Name:
    <asp:TextBox ID="TxtIssuedBank" runat="server" />
    </td>
    <td>
    Issued Branch Name:
    <asp:TextBox ID="TxtIssuedBranch" runat="server" />
    </td>
    </tr>
    </table>
    </div>
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="DdlPaymode" EventName="SelectedIndexChanged" />
    </Triggers>
        </asp:UpdatePanel>
        </p>
      <p align="left">
      <asp:Button ID="btnShowSingleDetail" CssClass="Btn"  runat="server" Text="View Detail" ValidationGroup="Save" />
        <asp:Button ID="BtnActivateSingle" CssClass="Btn"  runat="server" Text="Activate Member" Visible="false" ValidationGroup="Form-submit" />
     </p>
      </div>
    <div id="divMultiple" runat="server">
   <div>
   <table>
   <tr>
   <td>
   <asp:CheckBox ID="ChkDateWise" runat="server" AutoPostBack="true" Text="Joining Date Wise" />
   
             
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate> <asp:TextBox ID="txtStartDate" runat="server" CssClass="TxtBox" Width="150px"></asp:TextBox>
       <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                        ErrorMessage="Invalid Start Date" Display="Dynamic" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                To
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="TxtBox" Width="150px"></asp:TextBox>
                   <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True" Display="Dynamic"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
       </ContentTemplate>
       <Triggers>
       <asp:AsyncPostBackTrigger ControlID="ChkDateWise" EventName="CheckedChanged" />
       </Triggers>
       </asp:UpdatePanel>
                    
                        <br />
   <asp:DropDownList ID="DdlSearch" runat="server">
    <asp:ListItem Text="--Search By--" Value="0" />
   <asp:ListItem Text="Upliner Wise" Value="UplineIDNo"></asp:ListItem>
   <asp:ListItem Text="Referral Wise" Value="ReferalIDNo" />
   <asp:ListItem Text="City" Value="City"></asp:ListItem>
   <asp:ListItem Text="State" Value="StateName"></asp:ListItem>
   </asp:DropDownList>&nbsp;
   <asp:TextBox ID="TxtSearch" runat="server"></asp:TextBox> <br />
   <asp:Button ID="btnSearch" style="margin-left:0px" runat="server" CssClass="Btn" Text="Search" />&nbsp;
   <asp:Button ID="BtnActivate" runat="server" CssClass="Btn" Text="Activate" 
           ToolTip="Activate selected members." CausesValidation="False" /></td>
   </tr>
   
   </table>   
    </div>
       &nbsp;&nbsp;&nbsp;&nbsp;  <asp:Label Text="Select Kit :" ID="Lblkit" runat="server"></asp:Label><asp:DropDownList CssClass="ddl" ID="CmbMKit" runat="server">  </asp:DropDownList>
     &nbsp; <asp:Label Text=" Paymode :" ID="LblPMode" runat="server" /> <asp:DropDownList CssClass="ddl" ID="ddlCash" runat="server" ><asp:ListItem Text="Cash" Value="1"></asp:ListItem></asp:DropDownList>
    
   </div>
     
    <div >
    <table width="100%">
    <tr>
    <td> 
        </td>
    <td style="float:right;padding-right:30px;"><asp:Label ID="lblrecordcount" runat="server" Text="" Font-Bold="True"></asp:Label></td></tr><tr>
  <td colspan="2">
   <asp:GridView ID="GvData" runat="server"  
    AutoGenerateColumns="false" RowStyle-Height="25px" 
    GridLines="None"  
    AllowPaging="true"
    CssClass="table table-striped table-advance table-hover"  
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="20" EmptyDataText="No data to display." Visible="false" >  
    <Columns>
     <asp:TemplateField HeaderText="CheckAll" >
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("Status") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="KitName" HeaderText="Kit Name" />
                    <asp:BoundField DataField="IDNo" HeaderText="ID No." />
                    <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                    <asp:BoundField DataField="DOJ" HeaderText="Joining Date" />
                             <asp:BoundField DataField="Paymode" HeaderText="Paymode" />
                    <asp:BoundField DataField="ChDDNo" HeaderText="Chq./D.D. No." />
                    <asp:BoundField DataField="ChDDDate" HeaderText="Chq./D.D. Date" />
                    <asp:BoundField DataField="ChDDBank" HeaderText="Chq./D.D. Bank" />
                    <asp:BoundField DataField="TopupStatus" HeaderText="Top-up Status" />
            </Columns></asp:GridView>
    </td></tr></table>
    </div><br />
</asp:Content>

