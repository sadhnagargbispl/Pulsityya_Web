<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="ViewPinRequest.aspx.vb" Inherits="ViewPinRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
      <ol class="breadcrumb" >
						<li><i class="fa fa-home"></i><a href="Home.aspx">Home</a></li>
						<li>Pin</li>
						<li>View Approve Pin Request</li>
					</ol>
    <div style="background-color: White; padding :20px ">
      <%--<asp:RadioButtonList ID="RbReqStatus" runat="server" RepeatDirection ="Horizontal"  RepeatLayout="Table" >
                    <asp:ListItem Selected ="True"  Text ="Pending" Value ="N"></asp:ListItem>
                    <asp:ListItem Text ="Approved" Value="Y"></asp:ListItem>
                    <asp:ListItem Text ="Rejected" Value ="R"></asp:ListItem>
                    </asp:RadioButtonList>--%>
  <table width="100%" cellspacing="10px" align="left" style="margin: 0px; padding: 0px;">
          <tr>
           
                  <td> <asp:CheckBox ID="ChkMem" runat="server" AutoPostBack="true" Text="Member Id:" /> 
                  <asp:TextBox runat="server" Enabled="false" ID="TxtMemID" CssClass ="TxtBox" Width="150px"></asp:TextBox>                  </td>
           <td >
               
                  </td>
            </tr>
            <tr>
              
                <td width="200px">
                    <asp:Label ID="lblStartDate" runat="server" Text="Request Date : "></asp:Label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="TxtBox" Width="150px"></asp:TextBox>
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
               
                  </td>
                  <td>  to
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="TxtBox" Width="150px"></asp:TextBox>
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                </td>
              
            </tr>
          <tr>
               
                <td colspan="2" style="padding-top :15px;">
                    <asp:Button ID="BtnSearch" runat="server" CssClass="Btn" Text="Search" />
                    <asp:Button ID="btnshowall" Visible="false" runat="server" CssClass="Btn" Text="Show All" />
                    <asp:Button ID="btnExport" runat="server" CssClass="Btn" Text="Export To Excel" />
                    <asp:Button ID="BtnExportCsv" Visible="false" runat="server" CssClass ="Btn" Text="Export To CSV" />
                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" CssClass="Btn" />
                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" CssClass="Btn" />
                </td>
                <%--<td><asp:Button ID="BtnAdvSearch" runat="server" CssClass="Btn" Text="Advanced Search" /></td>--%>
            </tr>
            <tr>
                <td colspan="4" > 
                <asp:Label ID="lblErr" runat="server" style="font-weight:bold;font-size:12px;color:Red" ></asp:Label>
                </td>
            </tr>
        </table>
      <br />
        
                     <span style="font-size: 10px; font-weight: bold; margin-bottom: 11px; margin-top: 8px;
                padding-left: 10px"> Please Verify Page By Page</span>
                <div align="center">
 <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
 </div>
 <div style="margin-top:15px">
    <asp:GridView ID="GvData" runat="server"  
    AutoGenerateColumns="False" RowStyle-Height="25px" 
    GridLines="None"  
    AllowPaging="True"
   CssClass="table table-striped table-advance table-hover" 
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="50" EmptyDataText="No data to display." >  
<Columns>
<%--<asp:TemplateField HeaderText="CheckAll" >
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server"  />
                        </ItemTemplate>
                    </asp:TemplateField>--%>

<asp:TemplateField HeaderText="IDNo" Visible="false">
<ItemTemplate>
<asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField DataField="IDNo" HeaderText="ID No." />
<asp:BoundField DataField="MemName" HeaderText="Member Name" />
<asp:BoundField DataField ="MobileNo" HeaderText="Mobileno" />
<asp:BoundField DataField="ReqNo" HeaderText="Req.No" />
<asp:BoundField DataField="TotalAmount" HeaderText="Total Amt." />
<asp:BoundField DataField="WalletAmt" HeaderText="Wallet Amt." />
<asp:BoundField DataField="BankAmt" HeaderText="Bank Amt." />
<asp:BoundField DataField="OtherAmt" HeaderText="Other Amt." />
<asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass ="btn-group" >
<ItemTemplate>
<a class="btn btn-primary" href='<%# "ViewPinRqs.aspx?ReqNo=" & Crypto.Encrypt(Eval("ReqNo"))  %>'   onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0 } )" ><i class="icon_plus_alt2"></i>
<asp:Label ID="LBModify" runat="server" Text=" View Detail"/>
</a>
<%--<asp:LinkButton ID="lnkModify" runat="server" Text="Modify" OnClick="ModifyGrp" CssClass="fancybox fancybox.iframe"></asp:LinkButton>--%>
</ItemTemplate>

<HeaderStyle Width="55px"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>

</Columns>
</asp:GridView>
</div>

  </div>
</asp:Content>

