<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductView.aspx.vb" Inherits="ProductView" %>

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
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style-responsive.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="LblNo" runat="server" ForeColor="Black" Font-Size="6px"></asp:Label>
    </div>
    <div style="margin-bottom: 10px; padding-right: 20px; padding-top: 20px">
        <p>
            <b>Order No : </b><b>
                <asp:Label ID="LblOrderNo" runat="server" ForeColor="Black"></asp:Label></b></p>
        <p>
            <b>Member ID : </b><b>
                <asp:Label ID="LblMemberID" runat="server" ForeColor="Black"></asp:Label></b></p>
        <p>
            <b>Member Name : </b><b>
                <asp:Label ID="LblMemberName" runat="server" ForeColor="Black"></asp:Label></b></p>
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="5px"
            GridLines="Both" AllowPaging="true" class="table datatable" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" Font-Size="12px" Width="100%" ShowHeader="true"
            PageSize="50" EmptyDataText="No data to display.">
            <Columns>
                <asp:TemplateField HeaderText="S.No.">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>.
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Product Code" HeaderText="Product Code" />
                <asp:BoundField DataField="Product Name" HeaderText="Product Name" />
                <asp:BoundField DataField="DP(Rs.)" HeaderText="DP(Rs.)" />
                <asp:BoundField DataField="BV" HeaderText="BV" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="Total Amount(Rs.)" HeaderText="Total Amount(Rs.)" />
                <asp:BoundField DataField="Total BV" HeaderText="Total BV" />
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    </form>
</body>
</html>
