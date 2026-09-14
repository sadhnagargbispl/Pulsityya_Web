<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="UpgradeReport.aspx.vb" Inherits="UpgradeReport" Title="Untitled Page" %>
    
    
    <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
</asp:Content>
               
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <link href="../Resources/CSS/Grid.css" rel="stylesheet" type="text/css" />
         
               <div style="background-color: White ">
        <table width="100%" cellspacing="10px" align="left" style="margin: 0px; padding: 0px;">
            
            
            <tr>
              
                <td  style="width :20%; padding-top:15px">
                      <asp:Label ID="LblMemberID" runat="server" Text ="Member Id:"></asp:Label>
                        
                        <asp:TextBox Id="txtMemberId" runat="server" CssClass="TxtBox" AutoPostBack="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldvalidator1" runat="server" ControlToValidate="txtMemberId" ErrorMessage ="Enter Member Id" SetFocusOnError ="true" ></asp:RequiredFieldValidator>
                        
                        </td>
                        <td>
                       <%-- <asp:Label ID="LblLevel" runat="server" Text=" Choose Level:"  ></asp:Label>
                        <asp:DropDownList ID="DDLLevel" runat="server" CssClass="DDl" > 
                         </asp:DropDownList>--%>
                        
                         </td>
                         <td></td>
                         </tr>
                         
                         <tr id="TrDate" runat="server" visible="false">
                         <td>
                         <asp:CheckBox runat="server" ID="ChkDate" AutoPostBack ="true"   />
                          <asp:Label ID="LblDate" runat="server" Text=" Choose Date Type:"  ></asp:Label>
                        <asp:DropDownList ID="DDlDate" runat="server" CssClass="DDl"  > 
                        <asp:ListItem Text ="Date Of Activation" Value ="A"></asp:ListItem>
                        <asp:ListItem Text="Date Of Joining" Value="J"></asp:ListItem>
                         </asp:DropDownList></td>
                       <td style="width :20%; padding-top :15px">
           
           <asp:Label ID="lblSessionDate" runat="server" Text="Choose From Date : "></asp:Label>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="TxtBox" ></asp:TextBox>
         
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                        ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator> 
                        
                       </td> 
         <td style="padding-top :15px">
      
           <asp:Label ID="lblToDate" runat="server" Text="Choose To Date : "></asp:Label>
                    <asp:TextBox ID="TxtToDate" runat="server" CssClass="TxtBox"  AutoPostBack ="true"></asp:TextBox>
         
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                        ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>  
                        <br /> 
                   </td>
                   <%--<td></td>--%>
                   </tr>
                  
                         <tr>
                      
                      <td colspan="3">
                    
                        <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="Btn" />
                        <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" CssClass="Btn" />
                         </td>
                         </tr>
                         <tr>
                    <td colspan="4"><asp:Label ID="LblError" runat="server" Visible="false"></asp:Label></td>
                </tr></table> 
            
            </div>
       <asp:GridView ID="GrdDirects" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px" 
    GridLines="Both"  
    AllowPaging="true"
   CssClass="table table-striped table-advance table-hover"
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" Width="95%" ShowHeader="true" PageSize="50" EmptyDataText="No data to display.">
          
                                            <Columns>                                
                                                <asp:TemplateField >
                                             
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                              </asp:TemplateField>
                                              
                                                <asp:BoundField  DataField="IDNo" HeaderText="ID No"></asp:BoundField>
                                                <asp:BoundField  DataField="MemberName" HeaderText="Member Name"></asp:BoundField>
                                                                                            
                                               <asp:BoundField  DataField="KitName" HeaderText="Package Name"></asp:BoundField>    
                                               <asp:BoundField  DataField="Bv" HeaderText="Bv" />                                                                                                                                         
                                                                  <asp:BoundField  DataField="UpgradeDate" HeaderText="Date Of Activation"></asp:BoundField> 
                                                    
                                                                                                       </Columns>
                                        </asp:GridView>
            <br />
             <div style="margin-bottom:20px">
             
            
    </div>
            
            
          
    </div>
</asp:Content>


