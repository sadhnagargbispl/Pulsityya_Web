<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="MasteMain.master" CodeFile="AddCircular.aspx.vb" Inherits="App_UI_Application_Pages_AddCircular" Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" >
  function SelectAll(id)
        {
            //get reference of GridView control
            var grid = document.getElementById("<%=GvProd.ClientID%>");
            //variable to contain the cell of the grid
            var cell;
            
            if (grid.rows.length > 0)
            {
                //loop starts from 1. rows[0] points to the header.
                for (i=1; i<grid.rows.length; i++)
                {
                    //get the reference of first column
                    cell = grid.rows[i].cells[0];
                    
                    //loop according to the number of childNodes in the cell
                    for (j=0; j<cell.childNodes.length; j++)
                    {           
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type =="checkbox")
                        {
                        //assign the status of the Select All checkbox to the cell 
                        //checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

            </script>
           <link href="css/Grid.css" rel="Stylesheet" type="text/css" />
<link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
  --%>
   
    <%--height:200px;--%>
    <div style="padding-left:10px;width:100%">
<ol class="breadcrumb" >
						<li><i class="fa fa-home"></i><a href="Home.aspx">Home</a></li>
						<li>Master</li>
						<li>Circular Master</li>
						<%--<li><i class="fa fa-list-alt"></i>Transfer Amount</li>--%>
					</ol>
    <p style="color: #666666;line-height: 25px; margin-top :0px">
    <asp:RadioButtonList ID="RbtCircular" runat="server" RepeatDirection ="Horizontal"  AutoPostBack ="true"  >
    <asp:ListItem Text="New Circular" Selected ="True" Value="N"></asp:ListItem>
    <asp:ListItem  Text="Modify Circular" Value="M"></asp:ListItem></asp:RadioButtonList>
    </p>
<p style="color: #666666;" id="PCircular" runat="server"  visible="false" >
    Circular Name:<asp:DropDownList ID="CmbCircular" runat="server" Width="250px" CssClass ="TxtBox "></asp:DropDownList>
    <br />
    <asp:Button ID="BtnShow" runat="server" Text="Show" CssClass ="Btn " />
    <br />
   
    
 </p>
  <p style="color: #666666;" id="pCircularName" runat ="server">
 Circular Name :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
       <asp:TextBox CssClass="TxtBox" ID="txtCircularName" runat="server" Width="150px"></asp:TextBox> 
   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCircularName" runat="server" ValidationGroup="Save" >*</asp:RequiredFieldValidator>
    </p>
    <p style="color: #666666;" id="PStartDate" runat="server">
   
    Choose Start Date :
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="TxtBox" Width="150px"></asp:TextBox>
         
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        <br />
                    <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="TxtBox" Width="150px" AutoPostBack="true"></asp:TextBox>
                    <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
             
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                       
    <br />
      Status:<asp:RadioButtonList ID="RbtStatus" runat="server" RepeatDirection ="Horizontal" RepeatLayout="Flow"  >
      <asp:ListItem Selected="True" Text ="Active" Value="Y"></asp:ListItem>
      <asp:ListItem  Text="Deactive" Value ="N"></asp:ListItem></asp:RadioButtonList>
  </p> 
 
   
   
    
    
    <div style="margin-bottom:20px;" > 
    <asp:GridView ID="GvProd" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px" 
    GridLines="None" AllowPaging="true" CssClass="table table-striped table-advance table-hover"  PagerStyle-CssClass="pgr"  
    AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="20" EmptyDataText="No data to display." Visible ="true" >
    <Columns >
     <asp:TemplateField HeaderText="CheckAll" >
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server"  />
                            <asp:Label ID="LblProdCode" runat="server" Visible="false" Text='<%# Eval("ProdID") %>'  ></asp:Label>
    
                        </ItemTemplate>
                    </asp:TemplateField>
                   
    <asp:BoundField DataField ="ProductName" HeaderText="ProductName" /></Columns>
   
  
   </asp:GridView>
    
    </div>
    <p align="left">
    <asp:Button ID="BtnSave" CssClass="Btn"  runat="server" Text="Save" ValidationGroup="Save" />
    
    <asp:Button ID="BtnModify" CssClass="Btn"  runat="server" Text="Modify" Visible="false" />
   
    </p>
    </div>
    </asp:Content>
    
   
