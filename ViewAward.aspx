<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="ViewAward.aspx.vb" Inherits="ViewAward"  EnableEventValidation="false" %>

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
   
  


    <div style="margin:20px; padding-right :20px">
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

<asp:BoundField DataField="Idno" HeaderText="Idno" ControlStyle-Width="50px" />
<asp:BoundField DataField="MemberName" HeaderText="MemberName" ControlStyle-Width="50px" />


</Columns>
</asp:GridView>
</div>
    <br />

    <br />
</form></body></html>

