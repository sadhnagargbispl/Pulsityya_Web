<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="Showpayoutweb.aspx.vb" Inherits="App_UI_Application_Pages_Showpayoutweb" EnableEventValidation="false" %>
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
 <link href="../Resources/CSS/Grid.css" rel="stylesheet" type="text/css" />
 <ol class="breadcrumb" >
						<li><i class="fa fa-home"></i><a href="Home.aspx">Home</a></li>
						<li>Member</li>
						<li>Payout Show On Website</li>
					</ol>
  
       <%-- <table width="100%" cellspacing="10px" align="left" style="margin: 0px; padding: 0px;">--%>
   <div >
            <table cellspacing="10px" cellpadding="0%" >
                <tbody>
              <tr>
                 <td width="25%">
                 
            Payout No:
            <asp:DropDownList ID="DDlPayout" runat="server"></asp:DropDownList>
            </td>
         
           <%-- <asp:TextBox ID="TxtSession" runat="server" CssClass=" TxtBox " Enabled="false" Width="150px" ></asp:TextBox> </td>
           --%> <td style="padding-left :20px">
           <asp:Label ID="LblWeb" runat="server" Text="Show On Website"></asp:Label>
           <asp:RadioButtonList ID="RbtOnWeb" runat="server" RepeatDirection ="Horizontal" RepeatLayout ="Flow" >
           <asp:ListItem Text ="Yes" Value="Y" Selected= "True"></asp:ListItem>
           <asp:ListItem Text="No" Value="N"></asp:ListItem></asp:RadioButtonList>
          </td>
       
                        <td align ="left" >                   
                        
                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="Btn" OnClientClick="return confirmation();"/>
            </td> </tr>
            <tr><td colspan="3" style ="padding-bottom :30px"></td></tr>
             </tbody></table> 
             </div>
           
            <%-- <table  style="color: #666666; line-height: 25px;">
                 <tr>
                 <td>
                 
            Last Session:
         
            <asp:TextBox ID="TxtSession1" runat="server" CssClass=" TxtBox " Enabled="false" Width="150px" ></asp:TextBox> </td>
            <td>
           <asp:Label ID="LblSessStart" runat="server" Text=" Session Start Date : "></asp:Label>
                    <asp:TextBox ID="TxtSessStart" runat="server" CssClass="TxtBox" Width="150px" Enabled="false" ></asp:TextBox>
                    </td>
                    <td>
                     <asp:Label ID="LblSessClose" runat="server" Text=" Session Close Date : "></asp:Label>
                    <asp:TextBox ID="TxtSessClose" runat="server" CssClass="TxtBox" Width="150px" Enabled="false" ></asp:TextBox>
         
                                  
                        
                          </td> 
                          <td> <asp:Button ID="BtnPayoutCalc" runat="server" Text="Payout Calculate" CssClass="Btn" OnClientClick="return confirmation();"/>
         </td></tr></table>--%>
     <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px;
        margin-left: 25px; margin-bottom: 25px;">        
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
            GridLines="None" AllowPaging="true"  CssClass="table table-striped table-advance table-hover" PagerStyle-CssClass="pgr"
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
                  
                    <%--<asp:CheckBox ID="chwebsite"  runat="server" Checked ='<%#Eval("Status") %>' AutoPostBack ="true" OnCheckedChanged ="UpdateRecord"/>
              --%>      </ItemTemplate> 
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" Visible="false" >
                    <ItemTemplate>
                    <asp:Button ID="BtnModify" runat="server" text="Modify" CssClass ="Btn "   />
                    </ItemTemplate> 
                </asp:TemplateField> 
              
                
           
            </Columns>
        </asp:GridView>
            <br />
            
            <%-- <div style="margin-bottom:20px">--%>
             
            
    </div>
            
            
          
    <%--</div>--%>
</asp:Content>
