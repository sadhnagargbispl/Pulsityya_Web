<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="SessionClosing.aspx.vb" Inherits="App_UI_Application_Pages_SessionClosing" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
      
    </script>
    <meta http-equiv="refresh" content="600" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <div style="background-color: White ">
        <table width="100%" cellspacing="10px" align="left" style="margin: 0px; padding: 0px;">
            
            <tr>
                
                <td>
                 
            Current Session:
            </td>
            <td>
         
            <asp:TextBox ID="TxtSession" runat="server" CssClass=" TxtBox " Enabled="false" Width="150px" ></asp:TextBox> </td>
            <td>
           <asp:Label ID="lblSessionDate" runat="server" Text="Choose Session Closing Date : "></asp:Label></td>
           <td>
                    <asp:TextBox ID="txtSessionDate" runat="server" CssClass="TxtBox" Width="150px"></asp:TextBox>
         
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtSessionDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtSessionDate"
                        ErrorMessage="Invalid Session Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>   </td>
                        <td >                   
                        
                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="Btn" OnClientClick="return confirmation();"/>
            </td> </tr>
                 <tr>
                 <td>
                 
            Last Session:
         </td>

<td>            <asp:TextBox ID="TxtSession1" runat="server" CssClass=" TxtBox " Enabled="false" Width="150px" ></asp:TextBox> </td>
            <td>
           <asp:Label ID="LblSessStart" runat="server" Text=" Session Start Date : "></asp:Label>
           </td>
           <td>
                    <asp:TextBox ID="TxtSessStart" runat="server" CssClass="TxtBox" Width="150px" Enabled="false" ></asp:TextBox>
                    </td>
                    <td>
                     <asp:Label ID="LblSessClose" runat="server" Text=" Session Close Date : "></asp:Label>
                     </td>
                     <td>
                    <asp:TextBox ID="TxtSessClose" runat="server" CssClass="TxtBox" Width="150px" Enabled="false" ></asp:TextBox>
         
                                  
                        
                          </td> 
                          <td> <asp:Button ID="BtnPayoutCalc" runat="server" Text="Payout Calculate" CssClass="Btn"  OnClientClick="return confirm('Are you sure you want to continue')" />
         </td></tr></table>
            
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
            GridLines="None" AllowPaging="true" CssClass="table table-striped table-advance table-hover" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="20" EmptyDataText="No data to display.">
            <Columns>
                <asp:TemplateField HeaderText="GrpID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="LblSessId" runat="server" Text='<%# Eval("SessId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SessId" HeaderText=" Session ID" ControlStyle-Width="50px" />
               
                <asp:BoundField DataField="FrmDate" HeaderText="From Date" />
                  <asp:BoundField DataField="ToDate" HeaderText="To Date" />
                  
                  <asp:TemplateField HeaderText=" On WebSite " HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                    <asp:Label ID="LblOnWebsite" runat="server" Text='<%#Eval("WebSite") %>' ></asp:Label>
                  
                    <asp:CheckBox ID="chwebsite"  runat="server" Checked ='<%#Eval("Status") %>' AutoPostBack ="true" OnCheckedChanged ="UpdateRecord"/>
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" Visible="false" >
                    <ItemTemplate>
                    <asp:Button ID="BtnModify" runat="server" text="Modify" CssClass ="Btn "   />
                    </ItemTemplate> 
                </asp:TemplateField> 
              
                
           
            </Columns>
        </asp:GridView>
            <br />
             <div style="margin-bottom:20px">
             
            
    </div>
            
            
          
    </div>
</asp:Content>
