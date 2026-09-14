<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewLevelIncome.aspx.vb"
    Inherits="ViewLevelIncome" %>

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
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="5px"
            GridLines="Both" AllowPaging="true" class="table datatable" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" Font-Size="12px" Width="100%" ShowHeader="true"
            PageSize="50" EmptyDataText="No data to display.">
            <Columns>
                <asp:TemplateField HeaderText="S.No.">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>.
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MLevel" HeaderText="Level"></asp:BoundField>
                <asp:BoundField DataField="IDNo" HeaderText="Member ID"></asp:BoundField>
                <asp:BoundField DataField="MemFirstName" HeaderText="Member Name"></asp:BoundField>
                <asp:BoundField DataField="MatchingIncome" HeaderText="Matching Income"></asp:BoundField>
                <asp:BoundField DataField="Slab" HeaderText="Slab"></asp:BoundField>
                <asp:BoundField DataField="Comm" HeaderText="Comm"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="Gvdatasolfit" runat="server" AutoGenerateColumns="False" RowStyle-Height="5px"
            GridLines="Both" AllowPaging="true" class="table datatable" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" Font-Size="12px" Width="100%" ShowHeader="true"
            PageSize="50" EmptyDataText="No data to display.">
            <Columns>
                <asp:TemplateField HeaderText="S.No.">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>.
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="IDNo" HeaderText="Member ID"></asp:BoundField>
                <asp:BoundField DataField="MemFirstName" HeaderText="Member Name"></asp:BoundField>
                <asp:BoundField DataField="BV" HeaderText="BV"></asp:BoundField>
                <asp:BoundField DataField="Slab" HeaderText="Slab"></asp:BoundField>
                <asp:BoundField DataField="Comm" HeaderText="Direct Referral Incentive"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" RowStyle-Height="5px"
            GridLines="Both" AllowPaging="true" class="table datatable" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" Font-Size="12px" Width="100%" ShowHeader="true"
            PageSize="50" EmptyDataText="No data to display.">
            <Columns>
                <asp:TemplateField HeaderText="S.No.">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>.
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    </form>
</body>
</html>
