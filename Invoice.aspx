<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Invoice.aspx.vb" Inherits="Invoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .style1
        {
            width: 347px;
        }
        .topBorderOnly
        {
            border-top: 1px solid black;
        }
        .leftBorderOnly
        {
            border-left: none;
        }
        .rightBorderOnly
        {
            border-right: none;
            border-bottom: none;
        }
        .removeborder
        {
            border-left: none;
            border-bottom: none;
            border-right: none;
        }
        .removeallborder
        {
            border-style: none;
            border-color: inherit;
            border-width: medium;
            margin-left: 120px;
        }
        .bottomBorderOnly
        {
            border-bottom: 1px solid black;
        }
        .removeexceptleft
        {
            border-bottom: none;
            border-right: none;
            border-top: none;
        }
        .removetopRight
        {
            border-right: none;
            border-top: none;
        }
        .fontStyle
        {
            font-family: Verdana;
            font-size: 12px;
            font-weight: bold;
        }
        .fontHeader
        {
            font-family: Verdana;
            font-size: 24px;
            text-align: center;
            font-weight: bolder;
        }
        .gridalign
        {
            text-align: center;
        }
    </style>
</head>
<body class="fontStyle ">
    <form id="form2" runat="server">
    <div>
        <table border="1" align="center" cellspacing="0" width="70%">
            <tr align="center">
                <td class="removeborder" colspan ="2">
            
                    <asp:Image ID="Image1" runat="server" Height="65px" ImageUrl="~/images/logo.png" Width="140px"
                        BorderStyle="Solid" />
                        <asp:Label ID="LblGstIN" runat="server" Visible ="false"></asp:Label>
                      
                </td>
                <td colspan="4" class="removeborder">
                   <%-- <table border="1">
                    <tr >--%>
                            <asp:Label ID="lblRetailInvoice" runat="server" align="center" Height="32px" Text="INVOICE"
                                Width="730px"></asp:Label><br />
                       <%-- </tr>
                        <tr align="left" >--%>
                            <asp:Label ID="lblOfficeName" runat="server" align="Center" Height="30px" Width="730px"></asp:Label><br />
                      
                            <asp:Label ID="lblAddressTop" runat="server" Height="32px" align="Center" Width="730px"></asp:Label><br />
                      <%--  </tr>
                        
                    </table>--%>
                </td>
                <td class="removeborder" colspan="2"></td>
            </tr>
            <tr>
                <td rowspan="2" class="removeborder" colspan="2">
                Invoice NO:
                    <br />
                    Date Of Invoice:
                    <br />
                   Member ID:
                    <br />
                    Mobile No:
                    <br />
                    Order No:
                </td>
                <td class="removeborder" rowspan="2" colspan="2" >
                                    <asp:Label ID="LblInvoiceNo" runat="server" align="left"></asp:Label>
                <br />
                <asp:Label ID="LblInvoiceDate" runat="server" Text="" align="left"></asp:Label>
                    <br />
                   <asp:Label ID="lblDistributerID" runat="server" Text="" align="left"></asp:Label>
                    <br />
                    <asp:Label ID="LblMobileNo" runat="server" Text="" align="left"></asp:Label>
                    <br />
                    <asp:Label ID="LblOrderNo" runat="server" Text="" align="left"></asp:Label>
                    
                </td>
                <td rowspan="2" class="rightBorderOnly" colspan="2">
                Transport:
                    <br />
                    Vehicle No :
                    <br />
                   Pan No:<asp:Label ID="lblbill" runat="server" Visible="false" ></asp:Label>
                    <br />
                    Docket No:
                    <br />
                    Docket Date:
                </td>
                <td class="removeborder " rowspan="2" colspan="2" >
                                    <asp:Label ID="LblTransport" runat="server" align="left"></asp:Label>
                <br />
                <asp:Label ID="LblVechileNo" runat="server" Text="" align="left"></asp:Label>
                    <br />
                   <asp:Label ID="LblPanno" runat="server" Text="" align="left"></asp:Label>
                    <br />
                    <asp:Label ID="LblDocketNo" runat="server" Text="" align="left"></asp:Label>
                    <br />
                    <asp:Label ID="LblDocketDate" runat="server" Text="" align="left"></asp:Label>
                    
                </td>
             
            </tr>
            
            <tr>
                <td rowspan="1" colspan="6" class="removeborder">
                </td>
               
            </tr>
            
            <tr>
                <td rowspan="2" class="removeborder" colspan="4">
               Billed To:
                    <br />
                  <asp:Label ID="LblDistributorName" runat="server"></asp:Label>
                    <br />
                   <asp:Label ID="LblDistributorAddress" runat="server"></asp:Label>
                   <br />
                   GSTIN/UIN:
                </td>
                
                <td rowspan="2" class="rightBorderOnly" colspan="4">
                Shipped To:
                    <br />
                  <asp:Label ID="LblShippedName" runat="server"></asp:Label>
                    <br />
                   <asp:Label ID="LblshippedAddress" runat="server"></asp:Label>
                   <br />
                   
                   GSTIN/UIN:
                 </td>
             
            </tr>
            <tr>
                <td rowspan="1" colspan="6" class="removeborder">
                </td>
               
            </tr>
            <tr>
                <td colspan="5">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" PageSize="10"
                        CssClass="Grid" CellPadding="3" HorizontalAlign="Center" AllowPaging="True" Width="100%"
                        Height="100px">
                        <Columns>
                              <asp:TemplateField HeaderText="Item Code" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                               <%#Container.DataItemIndex + 1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                           
                           
                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" ItemStyle-CssClass="gridalign">
                            </asp:BoundField>
                             <asp:TemplateField HeaderText="Item Code" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblProductId" runat="server" Text='<%#Eval("ProductId") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField ="HSNCode" HeaderText="HSN Code" />
                           <asp:TemplateField HeaderText="Qty" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblQty" runat="server" Text='<%#Eval("Qty") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="MRP" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblRate" runat="server" Text='<%#Eval("MRP") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Basic Price" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblQty1" runat="server" Text='<%#Eval("Amount") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Discount" ItemStyle-CssClass="gridalign" Visible ="false">
                                <ItemTemplate>
                                    <asp:Label ID="LblDiscount" runat="server" Text='<%#Eval("Discount") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BV" ItemStyle-CssClass="gridalign" Visible ="false">
                                <ItemTemplate>
                                    <asp:Label ID="LblBV" runat="server" Text='<%#Eval("BV") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="Tax(%)" ItemStyle-CssClass ="gridalign"  >
                                               <ItemTemplate> 
                                               <%--<asp:Label ID="lblTaxVisible" runat="server" Visible ='<%# Eval("TaxVisible") %>'></asp:Label>
--%>                                               <asp:Label ID="LblTax" runat="server" Text='<%#Eval("Tax") %>' Visible ='<%# Eval("IsVisible") %>' ></asp:Label>
                                                </ItemTemplate> 
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField HeaderText="TaxAmount" ItemStyle-CssClass ="gridalign"  >
                                               <ItemTemplate> 
                                               <asp:Label ID="LblTaxAmount" runat="server" Text='<%#Eval("TaxAmount") %>' Visible ='<%# Eval("IsVisible") %>'></asp:Label>
                                                </ItemTemplate> 
                                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="CGST" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblCGST" runat="server" Text='<%#Eval("CGST") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                               <asp:TemplateField HeaderText="CGST Amount" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblCGSTAmount" runat="server" Text='<%#Eval("CGSTAmt") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>   
                              <asp:TemplateField HeaderText="SGST" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblSGST" runat="server" Text='<%#Eval("SGST") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>   
                              <asp:TemplateField HeaderText="SGST Amount" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblSGSTAmount" runat="server" Text='<%#Eval("SGSTAmt") %>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                         
                          
                            <asp:TemplateField HeaderText="TotalAmount" ItemStyle-CssClass="gridalign">
                                <ItemTemplate>
                                    <asp:Label ID="LblTotalAmount" runat="server" Text='<%# Eval("TotalAmount")%>' Visible='<%# Eval("IsVisible") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
          <%--<tr style="border: none; border-bottom :white">
          <td colspan ="5">
          Payment Mode:<asp:Label ID="LblPaymentMode" runat="server" ></asp:Label><br />

                  <asp:Label ID="lblDispatchDetail" runat="server" Font-Underline="True" 
                      Text="Dispatch Detail :" align="left"></asp:Label>
                  <br />
                  <asp:Label ID="lblCourierName" runat="server" Text="COURIER NAME:" align="left"></asp:Label><br />
                  <asp:Label ID="lblCourierNameTxt" runat="server"></asp:Label>
                  <br />
                  <asp:Label ID="lblCNNo" runat="server" Text="Docket No.:" align="left"></asp:Label><br />
                  <asp:Label ID="lblCnNoTxt" runat="server"></asp:Label>
                  <br />
                  <asp:Label ID="lblCNDate" runat="server" Text="Docket Date:" align="left"></asp:Label><br />
                  <asp:Label ID="lblCNDatetxt" runat="server"></asp:Label>
                  <br />
                  <br />
               </td></tr>--%>
            <tr class="removeallborder">
                <td colspan="4" class="removeallborder">
                
                    <asp:Label ID="lblVatTax" runat="server" Font-Underline="True" Text="CST Tax Summary"></asp:Label>
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <table>
                    <tr id="TrTax" runat="server" visible="false" >
                          <td>Tax(%)&nbsp;</td>
                          <td>Amount&nbsp;</td>
                          <td>Tax Amount&nbsp;</td>
                          <td>Total Amount</td>
                          </tr>
                          <asp:Repeater runat="server" ID="RptTax1">
                          <ItemTemplate>
                          <tr>
                         <td><%# Eval("Tax") %> &nbsp;</td>
                         <td><%# Eval("Amount") %>&nbsp;</td>
                         <td><%# Eval("TaxAmount") %>&nbsp;</td>
                         <td><%# Eval("NetAmount") %></td>
                         </tr>
                         </ItemTemplate>
                          </asp:Repeater>
                        <tr id="TrCGST" runat="server" visible="false">
                            <td>
                              Tax(%)&nbsp;
                            </td>
                            <td>
                                Amount&nbsp;
                            </td>
                            <td>
                                CGST&nbsp;
                            </td>
                            <td>
                                SGST&nbsp;
                            </td>
                            <td>
                                Total Amount
                            </td>
                        </tr>
                        <asp:Repeater runat="server" ID="RptTax">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Eval("CGST")%>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <%# Eval("Amount") %>&nbsp;
                                    </td>
                                    <td>
                                        <%#Eval("CGSTAMOUNT")%>&nbsp;
                                    </td>
                                    <td>
                                        <%#Eval("SGSTAMounT")%>&nbsp;
                                    </td>
                                    <td>
                                        <%# Eval("NetAmount") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                         
                    </table>
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  
                </td>
                <td colspan="1" class="removeallborder" style="padding-left :250px">
                    <asp:Label ID="lblRoundOff" runat="server" Text="Round off :" text-align="Right"></asp:Label>
                    <asp:Label ID="lblRoundOfftxt" runat="server" text-align="Right"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblNetPayable" runat="server" Text="Net Payable :" text-align="Right"></asp:Label>
                    <asp:Label ID="lblNetPayabletxt" runat="server" text-align="Right"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="removeallborder" colspan="3">
             <asp:Label ID="lblinword" runat="server" ></asp:Label><br />
             <br />
                    
                </td>
            </tr>
      
            <tr>
                <td class="removeallborder" colspan="4">
                    Terms and Condition :
                    <br />
                    E. & O. E.<br />
                    1. All disputes are subject to jurisdiction of Pune only
                    <br />
                    2. Any inaccuracy in this bill must be notified immediately.
                    <br />
                   
                </td><td style="text-align :center; width:450px;  "> For   <%=Session("CompName")%> <br /><br /><br /><br />
                 <asp:Label ID="lblAuthorisedSign" runat="server" Text="Authorised Signatory" align="right"></asp:Label></td>
            </tr>
            <tr>
                <td class="removeallborder">
                   
                </td>
                <td colspan="4" class="removeallborder" align="right">
                   
                    <%--<asp:Label ID="Label11" runat="server" Text="Cashier" align="right"></asp:Label>--%>
                </td>
            </tr>
            <tr>
                <td colspan="5" class="removeallborder" align="center">
                    <asp:Label ID="lblRegdOffice" runat="server" Font-Size="10px" align="center"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
