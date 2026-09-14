<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="ViewProductDetail.aspx.vb" Inherits="ViewProductDetail" title="" EnableEventValidation="false" %>

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
    <link href="css/style-responsive.css" rel="stylesheet" /></head>
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
<%--<asp:TemplateField HeaderText="Req.No." Visible="false">
<ItemTemplate>
<asp:Label ID="LblFormNo" Visible="false" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
<asp:Label ID="LblKitID" Visible="false" runat="server" Text='<%# Eval("KitID") %>'></asp:Label>
<asp:Label ID="LblReqNo" runat="server" Text='<%# Eval("ReqNo") %>'></asp:Label>

<asp:Label ID="LblQty" Visible="false" runat="server" Text='<%#Eval("Qty") %>'></asp:Label>

<asp:Label ID="LblDispQty" Visible="false" runat="server" Text='<%#Eval("DispQty") %>'></asp:Label>

</ItemTemplate>
</asp:TemplateField>--%>
<%--<asp:BoundField DataField="BankCode" HeaderText="Bank Code" Visible="false"/>--%>

<asp:BoundField DataField="ProductName" HeaderText="Product Name" />
<asp:BoundField DataField="Qty" HeaderText="Qty" />

<asp:BoundField DataField="Status" HeaderText="Status" />


</Columns>
</asp:GridView>
</div>
    <br />

    <br />
</form></body></html>

