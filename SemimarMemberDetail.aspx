<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SemimarMemberDetail.aspx.vb"
    Inherits="SemimarMemberDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="LblNo" runat="server" ForeColor="Black" Font-Size="6px"></asp:Label>
    </div>
    <div>
        <h4>
            Member Detail
        </h4>
    </div>
    <div style="margin-bottom: 10px; padding-right: 20px; padding-top: 20px">
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="5px"
            GridLines="Both" AllowPaging="true" class="table datatable" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" Font-Size="12px" Width="95%" ShowHeader="true"
            PageSize="50" EmptyDataText="No data to display.">
            <Columns>
                <asp:TemplateField HeaderText="S.No.">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>.
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Idno" HeaderText="Member Id"></asp:BoundField>
                <asp:BoundField DataField="Name" HeaderText="Name"></asp:BoundField>
                <asp:BoundField DataField="Mobile" HeaderText="Mobile"></asp:BoundField>
                <asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
                <asp:BoundField DataField="City" HeaderText="City"></asp:BoundField>
                <asp:BoundField DataField="PassCode" HeaderText="PassCode"></asp:BoundField>
               <asp:BoundField DataField="Rank" HeaderText="Rank"></asp:BoundField>
               <asp:BoundField DataField="GS" HeaderText="Total Gross InCome"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    </form>
</body>
</html>
