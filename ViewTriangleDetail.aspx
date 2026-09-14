<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="ViewTriangleDetail.aspx.vb" Inherits="ViewTriangleDetail"  EnableEventValidation="false" %>

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
  

    <div style="margin-bottom:20px; margin-top :20px; margin-left :20px; margin-right :20px; padding-right :20px">
    <asp:Button ID="BtnExportToExcel"  runat="server" Text="Export To Excel"  CssClass ="Btn"/>
    <br />
 <asp:GridView ID="GvData" runat="server"  
    AutoGenerateColumns="False" RowStyle-Height="25px" 
    GridLines="Both"  
    AllowPaging="true"
   CssClass="table table-striped table-advance table-hover"
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" Width="95%" ShowHeader="true" PageSize="50" EmptyDataText="No data to display.">  
<Columns>
<asp:TemplateField HeaderText="S.No." HeaderStyle-Width="35px">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

<asp:BoundField DataField="SponsorId" HeaderText="SponsorId" ControlStyle-Width="50px" />
<asp:BoundField DataField="SponsorName" HeaderText="SponsorName" ControlStyle-Width="50px" />
<asp:BoundField DataField="DirectId" HeaderText="DirectId" />
<asp:BoundField DataField="DirectName" HeaderText="DirectName" />

<asp:BoundField DataField="PackageName" HeaderText="Package Name" />
<asp:BoundField DataField="PackageAmount" HeaderText="Package Amount" />
<asp:BoundField DataField ="PackageBv" HeaderText="PackageBv" />
<asp:BoundField DataField="LegNo" HeaderText="LegNo" />
<asp:BoundField DataField="DateOfJoining" HeaderText="DateOfJoining" />
</Columns>
</asp:GridView>
</div>
    <br />

    <br />
</form></body></html>

