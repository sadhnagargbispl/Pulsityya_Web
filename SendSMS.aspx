<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="SendSMS.aspx.vb" Inherits="App_UI_Application_Pages_SendSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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

 <div style="padding:  10px 10px 20px 20px">
   <table width="100%"><tr><td  valign="top" style="font-weight:bold;padding:2px;width:50px">SMS</td>
            <td><asp:TextBox TextMode="MultiLine" onkeyup="CountChar();" ID="TxtSMS" runat="server" Height="50px" Width="80%" style="padding-left:4px;padding-top:2px;"></asp:TextBox>
        &nbsp;<span style="font-family:Verdana;font-weight:lighter;">Length: <span id='remainingC'></span></span> 
            </td></tr>
                        <tr><td></td><td><asp:Button runat="server" ID="BtnSubmit" CssClass="Btn" Text="Send" /></td></tr></table>
                        
            <asp:GridView ID="GrdMemList" runat="server" Width="100%"  AllowPaging="false"
                AutoGenerateColumns="False" CellPadding="4"   CssClass="table table-striped table-advance table-hover">
             
                <Columns>
                    <asp:TemplateField HeaderText="CheckAll" >
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("Status") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IDNo" HeaderText="Member ID" SortExpression="IDNo"  />
                    <asp:BoundField DataField="MemName" HeaderText="Member Name" SortExpression="MemName" /> 
                    <asp:BoundField DataField="JoinDate" HeaderText="Joining Date" SortExpression="DOJ" /> 
                    <asp:BoundField DataField="RefFormNo" HeaderText="Referal ID" SortExpression="RefFormNo" /> 
                     <asp:BoundField DataField="City" HeaderText="City" SortExpression="StGuardian" /> 
                    
                      <asp:TemplateField HeaderText="Mobile No." >
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
          
            
        </div>

</asp:Content>

