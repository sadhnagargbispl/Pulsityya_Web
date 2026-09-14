<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="SMSSending.aspx.vb" Inherits="App_UI_Application_Pages_SMSSending" ValidateRequest="false" %>

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
}</style>
<script type="text/javascript">
    //$(document).ready(function() { $('[id$=chkSelectAll]').click(function() { $("[id$='chkSelect']").attr('checked', this.checked); }); });
//    function reset() {
//        $("[id$='chkSelect']").prop('checked', false);
//    }

    function CountChar() {
               var SmsTxt = document.getElementById('<%=TxtSMS.ClientID%>').value;
               var cnt = SmsTxt.length;
               var smsNo;
               var cnt_;
               cnt_ = cnt;
               if (cnt > 160) {
                   smsNo = cnt / 160;
                   cnt = cnt % 160;
                   cnt_ = cnt + '/' + parseInt(smsNo);
               }
               
        document.getElementById("remainingC").innerHTML = cnt_;
    }

        function SelectAll(id)
        {
            //get reference of GridView control
            var grid = document.getElementById("<%= GrdMemList.ClientID %>");
            //variable to contain the cell of the grid
            var cell;
            
            if (grid.rows.length > 0)
            {
                //loop starts from 1. rows[0] points to the header.
                for (i=1; i<grid.rows.length; i++)
                {
                    //get the reference of first column
                    cell = grid.rows[i].cells[0];
                    
                    //loop according to the number of childNodes in the cell
                    for (j=0; j<cell.childNodes.length; j++)
                    {           
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type =="checkbox")
                        {
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


 
   <table >
   <tr>
   <td>Search Type: </td>
   <td><asp:DropDownList ID="CmbSrchType"  runat="server" AutoPostBack ="true" CssClass ="DDl" >
   <asp:ListItem Value="I" Text="ID Wise"></asp:ListItem>
   <asp:ListItem Value="R" Text="Referral Wise"></asp:ListItem>
   <asp:ListItem Value="S" Text="Sponsor Wise"></asp:ListItem>
   </asp:DropDownList>
   </td>
   <td> 
   <asp:Label ID ="lblReg" Text="Leg No" Visible="false" runat="server"></asp:Label> </td>
   <td> 
   <asp:RadioButtonList ID ="RbLeg" runat="server" Visible="false" RepeatDirection="Horizontal" RepeatLayout="Flow"  >
            <asp:ListItem Value="1" Text="Left" Selected="True" ></asp:ListItem>    
            <asp:ListItem Value="2" Text="Right"></asp:ListItem>  
    </asp:RadioButtonList> 
    </td>
   </tr>
   <tr><td>Member ID</td>
   <td><asp:TextBox ID="TxtIDNo" runat="server"  CssClass ="TxtBox"></asp:TextBox>
   <asp:RequiredFieldValidator ValidationGroup="Add" runat="server" ID="ReqFV1" ControlToValidate="TxtIDNo" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td><td>&nbsp;<asp:Button ID="BtnAdd" Text="OK" CssClass="Btn" ValidationGroup="Add" runat="server" /> </td>
   </tr>
   </table>
                        
   <div id="DvData" runat="server" visible="false">
            <asp:GridView ID="GrdMemList" runat="server" Width="100%"  AllowPaging="false"
                AutoGenerateColumns="False" CellPadding="4"  CssClass="table table-striped table-advance table-hover">
               
                <Columns>
                    <asp:TemplateField HeaderText="CheckAll" >
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("Status") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                   <%-- <asp:TemplateField HeaderText="Member ID">
                    <ItemTemplate>
                    <asp:Label ID="lblIDNo" runat="server" Text='<%# Eval("IDNo")%>'  />
                    </ItemTemplate>
                     </asp:TemplateField>--%>
                     <asp:BoundField DataField="IdNo" HeaderText="Member ID" />
                     <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                     <asp:BoundField DataField="JoinDate" HeaderText="Joining Date" />
                     <asp:BoundField DataField="Leg" HeaderText="Side" />
                     <asp:BoundField DataField="Passw" HeaderText="Password" />
                     <asp:BoundField DataField="City" HeaderText="City" />
                     <asp:BoundField DataField="Mobl" HeaderText="Mobile No." />
                   <%--  <asp:TemplateField HeaderText="Member Name">
                    <ItemTemplate>
                    <asp:Label ID="lblMemName" runat="server" Text='<%# Eval("MemName") %>' /> 
                    </ItemTemplate>
                     </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="Joining Date">
                    <ItemTemplate>
                    <asp:Label ID ="lblJoinDate" runat="server"  Text='<%# Eval("JoinDate")%>' /> 
                    </ItemTemplate>
                     </asp:TemplateField>
                     
                      <asp:TemplateField  HeaderText="Side">
                    <ItemTemplate>                     
                     <asp:Label ID="lblLeg" runat="server" Text='<%# Eval("Leg")%>' /> 
                     </ItemTemplate>
                     </asp:TemplateField>
                     
                      <asp:TemplateField HeaderText ="Password">
                    <ItemTemplate>                         
                     <asp:Label ID ="lblPassw" runat ="server" Text='<%# Eval("Passw")%>'  />
                     </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField  HeaderText ="City">
                    <ItemTemplate>                         
                     <asp:Label ID ="lblCity" runat ="server" Text='<%# Eval("City")%>' />
                     </ItemTemplate>
                     </asp:TemplateField>
                     --%>
                      <asp:TemplateField HeaderText="Mobile No." Visible="false" >
                        <ItemTemplate>
                            <asp:Label ID="LblMobl" runat="server" Text='<%# Eval("Mobl") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="MemFormNo" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                </Columns>
            </asp:GridView>
            <br />
          <table width="100%">
          <tr><td colspan="2">If you want to send custom SMS, enter <b>Header text</b> between <b><</b> and <b>></b> .<br />
          <b>Example: </b> Dear &lt;Member Name&gt;, you have successfully registered on &lt;Joining Date&gt;. Your Id No. is &lt;Member ID&gt; and password is &lt;Password&gt;.
          <br />
          </td></tr>
            <tr><td  valign="top" style="font-weight:bold;padding:2px;width:50px">SMS</td>
            <td><asp:TextBox TextMode="MultiLine" onkeyup="CountChar();" ID="TxtSMS" runat="server" Height="50px" Width="80%" style="padding-left:4px;padding-top:2px;"></asp:TextBox>
        &nbsp;<span style="font-family:Verdana;font-weight:lighter;">Length: <span id='remainingC'></span></span> 
            </td></tr>
                        <tr><td></td><td><asp:Button runat="server" ID="BtnSubmit" CssClass="Btn" Text="Send" /></td></tr></table>
           </div>          
        

</asp:Content>

