<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="ViewBusiness.aspx.vb" Inherits="ViewBusiness"  EnableEventValidation="false" %>

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
   <center><br />
  <asp:Label ID="LblNo" runat="server" ForeColor ="Black" Font-Size ="14px" style="margin :20px; padding :20px"  ></asp:Label>
  </center>
   </div>
  

    <div style="margin:20px; ">
 <asp:GridView ID="GvData" runat="server"  
    AutoGenerateColumns="False" RowStyle-Height="25px" 
    GridLines="Both"  
    AllowPaging="true"
   CssClass="table table-striped table-advance table-hover"
    PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" Width="95%" ShowHeader="true" PageSize="50" EmptyDataText="No data to display.">  
<Columns>
   <asp:BoundField DataField="SNo" HeaderText="SNo" SortExpression="SNo" />
                                                    <asp:BoundField DataField="IdNo" HeaderText="IdNo." />
                                                    <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                    
                                                     <asp:BoundField DataField ="TotalBusiness" HeaderText="Total Business" />
								
								
								 </Columns>
</asp:GridView>
</div>
    <br />

    <br />
</form></body></html>

