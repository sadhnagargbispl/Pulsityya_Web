<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false" CodeFile="AchieversMaster.aspx.vb" Inherits="App_UI_Application_Pages_AchieversMaster" title="" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="css/Grid.css" rel="Stylesheet" type="text/css" />
<link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<link href="css/style-responsive.css" rel="stylesheet" type="text/css" />--%>
    
    <ol class="breadcrumb" >
						<li><i class="fa fa-home"></i><a href="Home.aspx">Home</a></li>
						<li>Master</li>
						<li>Achievers Master</li>
						<%--<li><i class="fa fa-list-alt"></i>Transfer Amount</li>--%>
					</ol>
   <div>
   <table>
   <tr>
  <td><asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" CssClass="Btn" /></td>
    <td><asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" CssClass="Btn" /></td>
   <td><asp:Button ID="btnExport" runat="server" CssClass="Btn" Text="Export To Excel" /></td>
   <td><a href="AddAchievers.aspx"  onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0} )"><asp:Button ID="BtnAddNew" runat="server" CssClass="Btn" Text="Add Achievers" /></a></td>
   <td>
    <asp:Button ID="btnShowRecord" runat="server" CssClass="Btn" Text="View All" Visible="false" />
   <%--<asp:Button ID="BtnAdvSearch" runat="server" CssClass="Btn" Text="Advanced Search" />--%></td>
   </tr>
   </table>   
    </div>
      <div style="margin-left:25px;margin-top:20px;margin-bottom:20px;">
<asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red color are deactivated."></asp:Label>
<br />
<br />
<asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
</div>
    <div style="margin-bottom:20px">
    <asp:GridView ID="GvData" runat="server"  
    AutoGenerateColumns="False" RowStyle-Height="25px" 
    GridLines="None"  
    AllowPaging="true"
     CssClass="table table-striped table-advance table-hover" 
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="20" EmptyDataText="No data to display." >  
<Columns>
<asp:TemplateField HeaderText="AcId" Visible="false">
<ItemTemplate>
<asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("AcId") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<%--<asp:BoundField DataField="BankCode" HeaderText="Bank Code" Visible="false"/>--%>
<asp:BoundField DataField="SessId" HeaderText="Session Id" />
<asp:BoundField DataField="IdNo" HeaderText="Achiever Id" />
<asp:BoundField DataField="MemberName" HeaderText="Achiever Name" />

<asp:BoundField DataField="Status" HeaderText="Status" />
<asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass ="btn-group " >
<ItemTemplate>
<a class="btn btn-primary" href='<%# "AddAchievers.aspx?AcId=" & Crypto.Encrypt(Eval("VAcCode"))  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 350,marginTop : 0} )"><i class="icon_plus_alt2"></i>
<asp:Label ID="LBModify" runat="server" Text="Modify"/>
</a>
<%--<asp:LinkButton ID="lnkModify" runat="server" Text="Modify" OnClick="ModifyGrp" CssClass="fancybox fancybox.iframe"></asp:LinkButton>--%>
</ItemTemplate>

<HeaderStyle Width="55px"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup" class="btn btn-danger"><i class="icon_close_alt2"></i></asp:LinkButton>

</ItemTemplate>
<HeaderStyle Width="55px"></HeaderStyle>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>
</Columns>
</asp:GridView>
</div>
    <br />

    <br />
</asp:Content>

