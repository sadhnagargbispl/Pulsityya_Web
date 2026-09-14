<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="ViewPinRqs.aspx.vb" Inherits="ViewPinRqs" title="" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <link href="css/bootstrap.min.css" rel="stylesheet">
    <!-- bootstrap theme -->
    <link href="css/bootstrap-theme.css" rel="stylesheet">
    <!--external css-->
    <!-- font icon -->
    <link href="css/Grid.css" rel="Stylesheet" type="text/css" />
    
    <link href="css/elegant-icons-style.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
   <!-- Custom styles -->
    <link href="css/style.css" rel="stylesheet"/>
    <link href="css/style-responsive.css" rel="stylesheet" />
    <script type ="text/javascript">
    function ToggleEnable(src)
{
var result = confirm('Are you sure you want to update this task?');
src.disabled = result;
alert(src.disabled);
return result;
}
</script>
    
    </head>
<body><form id="form1" runat="server">
   
  
   <div>
  <asp:Label ID="LblNo" runat="server" ForeColor ="Black" Font-Size ="14px"  ></asp:Label>
   </div>

    <div style="margin-bottom:20px">
    <asp:GridView ID="GvData" runat="server"  
    AutoGenerateColumns="False" RowStyle-Height="25px" 
    GridLines="Both"   
    AllowPaging="true"
    CssClass="table table-striped table-advance table-hover"  
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="20" EmptyDataText="No data to display." >  
<Columns>
<asp:TemplateField HeaderText="Req.No." Visible="false">
<ItemTemplate>
<asp:Label ID="LblFormNo" Visible="false" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
<asp:Label ID="LblKitID" Visible="false" runat="server" Text='<%# Eval("KitID") %>'></asp:Label>
<asp:Label ID="LblReqNo" runat="server" Text='<%# Eval("ReqNo") %>'></asp:Label>
<asp:Label ID="LblIdNo" Visible="false" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label>
<asp:Label ID="LblQty" Visible="false" runat="server" Text='<%#Eval("Qty") %>'></asp:Label>

<asp:Label ID="LblDispQty" Visible="false" runat="server" Text='<%#Eval("DispQty") %>'></asp:Label>

</ItemTemplate>
</asp:TemplateField>
<%--<asp:BoundField DataField="BankCode" HeaderText="Bank Code" Visible="false"/>--%>

<asp:BoundField DataField="KitName" HeaderText="Kit Name" />
<asp:BoundField DataField="Qty" HeaderText="Req.Qty" />
<asp:BoundField DataField="DispQty" HeaderText="Sent Qty" />
<asp:BoundField DataField="Status" HeaderText="Status" />
<asp:TemplateField HeaderText="Send" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass ="btn-group" >
<ItemTemplate><%--OnTextChanged ="TxtPinQty_TextChanged" AutoPostBack ="true"--%>
<asp:TextBox ID="TxtPinQty" runat="server"  Text='<%# Eval("RemainQty") %>' Width="35px" Enabled ="false" ></asp:TextBox>
<asp:LinkButton ID="lnkSend" runat="server" Text="Send" OnClick="SendPin" CssClass="btn" Visible ='<%# Eval("SentStatus") %>' OnClientClick="this.disabled=true;"  UseSubmitBehavior="false"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>
</div>
    <br />

    <br />
</form></body></html>

