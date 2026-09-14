<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Bills.aspx.vb" Inherits="Bills" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    
    <style type="text/css">
        .style1 {
            width: 347px;
        }
        
        .topBorderOnly {
    border-top: 1px solid black;
   
}
.leftBorderOnly
{
	 border-left:none;
}
.rightBorderOnly
{
	border-right :none;
	border-bottom :none;
}
.removeborder
{
	border-left :none;
	border-bottom :none;
	border-right:none;
	
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
	border-bottom :none;
	border-right:none;
	border-top :none;
}
.removetopRight
{
	border-right:none;
	border-top :none;
}
.fontStyle
{
	font-family:Verdana ;
	font-size :12px ;

	font-weight:bold ;
}
.fontHeader
{
	font-family:Verdana ;
	font-size :24px;
	text-align:center ;
	font-weight :bolder ;
}
    </style>
</head>
<body class ="fontStyle ">
    <form id="form2" runat="server">
    <div>     
    <table border="1"  align="center" cellspacing="0">
    <tr align ="center"  >
        <td class="removeborder"><asp:Image ID="Image1" runat="server" Height="65px" ImageUrl="~/images/logo.png" Width="140px" BorderStyle="Solid" /></td>
            <td colspan="4" class= "removeborder" >
                <table border="1" >
                <tr align="left" class="fontHeader " >
                <asp:Label ID="lblOfficeName" runat="server" 
                        align="Center" Height="30px" 
                        Width="730px"  ></asp:Label>
                </tr>
               <tr align ="left" class="fontHeader "  >
               <asp:Label ID="lblAddressTop" runat="server" Height="32px" align="Center" 
                      
                       Width="730px"></asp:Label>
               </tr>
               <tr class="fontHeader " ><asp:Label ID="lblRetailInvoice"  runat="server" align="center"  Height="32px" Text="RETAIL INVOICE" Width="730px" ></asp:Label>
               </tr>
               </table> 
             </td>    
       </tr>
      <tr >
      <td rowspan="1" class="removeborder"  ><asp:Label ID="lbldistributerId" 
              runat="server" Text="Member ID:" align="left" ></asp:Label>
      <br />
             <asp:Label ID="lblDistributerName" runat="server" Text="Member Name:" align="left" ></asp:Label>
             <br />
             <asp:Label ID="lblAddress" runat="server" Text="Address:" align="left"></asp:Label>
             <br /></td>
             <td rowspan="1" class="removeborder" >
            <asp:Label ID="lblDistIdtxt" runat="server" align="left"></asp:Label>
            <br />
            <asp:Label ID="lblDistNametxt" runat="server" align="left"></asp:Label>
            <br />
             <asp:Label ID="lblDistAddresstxt" runat="server" align="left"></asp:Label>
             
              <br />
              </td>
              <td colspan="2" class="rightBorderOnly"  >
                 <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No." text-align="right"></asp:Label><br />
                 <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date" text-align="right"></asp:Label>
                 </td>        
                      
                           
                  
                <td class="removeborder" >            
                <asp:Label ID="lblInvoiceNoTxt" runat="server" text-align="right"></asp:Label>
                <br />               
                 <asp:Label ID="lblInvoiceDateText" runat="server" text-align="right"></asp:Label></td>
                                        </tr>    
                 <tr>
                 <td  rowspan="1" colspan="3" class="removeborder" ></td>
                 <td colspan="2" class="removeborder">
                     <asp:Label ID="lblRemarks" runat="server" Text="Remarks" align="left"></asp:Label>
                 </td></tr>
              <tr><td colspan ="5">
              <asp:Gridview ID="GridView1" runat="server" AutoGenerateColumns="False" PageSize="10" CssClass="Grid" 
                                            CellPadding="3" HorizontalAlign="Center"  AllowPaging="True"
                                            Width="100%" Height="100px">               
                                             
                                           <Columns>
                                               <asp:TemplateField HeaderText="SNo." >
                                               <ItemTemplate> <%#Container.DataItemIndex + 1%></ItemTemplate></asp:TemplateField>
                                                
                                                <asp:BoundField DataField="ProductId" HeaderText="P.Code">                                                
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" >                                                    
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Rate" HeaderText="Rate">                                                
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Qty" HeaderText="Qty" >                                                    
                                                </asp:BoundField>
                                              
                                               <asp:TemplateField HeaderText="Amount" >
                                               <ItemTemplate> <%#Eval("Rate") * Eval("Qty")%></ItemTemplate></asp:TemplateField>
                                                
                                                
                                                <asp:BoundField DataField="BV" HeaderText="BV" >                                                    
                                                </asp:BoundField>
                                                
                                              <asp:BoundField DataField="DiscountPer" HeaderText="Disc%">                                                
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Discount" HeaderText="Disc amount" >                                                    
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="NetAmount" HeaderText="Amount">                                                
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Tax" HeaderText="Tax(%)" >                                                    
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="TaxAmount" HeaderText="Tax Amount">                                                
                                                </asp:BoundField>
                                                
                                                <asp:TemplateField HeaderText="Total Amount" >
                                               <ItemTemplate> <%#Eval("TaxAmount") + Eval("NetAmount")%></ItemTemplate></asp:TemplateField>
                                                
                                             </Columns>
                                          
                                           
                                          
                                        </asp:Gridview>
                  </td></tr>
              <tr><td class="removeallborder" >
                  <asp:Label ID="lblDispatchDetail" runat="server" Font-Underline="True" 
                      Text="Dispatch Detail :" align="left"></asp:Label>
                  <br />
                  <asp:Label ID="lblCourierName" runat="server" Text="COURIER NAME:" align="left"></asp:Label>
                  <asp:Label ID="lblCourierNameTxt" runat="server"></asp:Label>
                  <br />
                  <asp:Label ID="lblCNNo" runat="server" Text="CN No.:" align="left"></asp:Label>
                  <asp:Label ID="lblCnNoTxt" runat="server"></asp:Label>
                  <br />
                  <asp:Label ID="lblCNDate" runat="server" Text="CN Date:" align="left"></asp:Label>
                  <asp:Label ID="lblCNDatetxt" runat="server"></asp:Label>
                  <br />
                  <br />
                  </td>
                  <td colspan ="2" class="removeallborder" ></td>
                  <td colspan="2" class="removeallborder"  >
                      <asp:Label ID="lblRoundOff" runat="server" Text="Round off :" text-align="Right"></asp:Label>
                      <asp:Label ID="lblRoundOfftxt" runat="server" text-align="Right"></asp:Label>
                      <br />
                      <br />
                      <asp:Label ID="lblNetPayable" runat="server" Text="Net Payable :" text-align="Right"></asp:Label>
                      <asp:Label ID="lblNetPayabletxt" runat="server" text-align="Right"></asp:Label>
                      </td>
                      </tr>
                      <tr class ="removeallborder" >
                      <td  colspan="5" class="removeallborder"  >
                          <asp:Label ID="lblVatTax" runat="server" Font-Underline="True" 
                              Text="Tax Summary"></asp:Label>
                          <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        
                          <table><tr>
                          <td>Tax(%)&nbsp;</td>
                          <td>Amount&nbsp;</td>
                          <td>Tax Amount&nbsp;</td>
                          <td>Total Amount</td>
                          </tr>
                          <asp:Repeater runat="server" ID="RptTax">
                          <ItemTemplate>
                          <tr>
                         <td><%# Eval("Tax") %> &nbsp;</td>
                         <td><%# Eval("Amount") %>&nbsp;</td>
                         <td><%# Eval("TaxAmount") %>&nbsp;</td>
                         <td><%# Eval("NetAmount") %></td>
                         </tr>
                         </ItemTemplate>
                          </asp:Repeater>
                         
                          </table>
                          <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       <%--   <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                      &nbsp;<asp:Label ID="lblAmountTotaltxt" runat="server"></asp:Label><asp:Label 
                              ID="lblTaxAmountTotaltxt" runat="server"></asp:Label><asp:Label 
                              ID="lblTotalAmounttotaltxt" runat="server"></asp:Label>--%></td>
                      </tr>
                      <tr><td class="removeexceptleft"  >
                         <%-- <asp:Label ID="lblAmountWord" runat="server" Font-Bold="True" 
                              Text="Amount in words (Rupees) :" colspan="5" align="center"></asp:Label>--%>
                          </td>
                         </tr>
                          <tr><td class="removeborder" >
                              <asp:Label ID="lblTinNo" runat="server" Text="TIN No.:" align="left"></asp:Label>
                              <asp:Label ID="lblTinNotxt" runat="server"></asp:Label>
                              <br />
                             <%-- <asp:Label ID="lblCINNo" runat="server" Text="CIN No.:" align="left"></asp:Label>
                              <asp:Label ID="lblCINNotxt" runat="server"></asp:Label>
                              <br />--%>
                              <asp:Label ID="lblCSTNo" runat="server" Text="CST No. :" align="left"></asp:Label>
                              <asp:Label ID="lblCSTNotxt" runat="server"></asp:Label>
                              <br />
                              <asp:Label ID="lblPANNo" runat="server" Text="PAN No.:" align="left"></asp:Label>
                              <asp:Label ID="lblPANNotxt" runat="server"></asp:Label>
                              </td>
                              <td colspan="2" class="removeborder" ></td>
                              <td colspan="2" class="removeborder" >
                                  <asp:Label ID="lblEOE" runat="server" Text="E. &amp; O.E." align="Right"></asp:Label>
                                  <br />
                                  <asp:Label ID="lblCNMI" runat="server" align="left"
                                      ></asp:Label>
                              </td></tr>
                              <tr>
                              <td class="removeallborder"  colspan="5">
                                  Terms and Condition :
                                  <br />
                                 
                                      1. All disputes are subject to jurisdiction of Jaipur only
                                  <br />
                                  
                                      2. Any inaccuracy in this bill must be notified immediately.
                                  <br />
                                  <br />
                                  <br />
                                 
                                      We look forward to a lifetime relationship with you.
                                  </td></tr>
                                  <tr><td class="removeallborder" >
                                      <asp:Label ID="lblPrepare" runat="server" Text="Prepared By :" align="left"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:Label ID="lblCashier" runat="server"></asp:Label>
                                      </td>
                                      <td colspan="4" class="removeallborder" align="right" >
                                          <asp:Label ID="lblAuthorisedSign" runat="server" Text="Authorised Signatory" align="right"></asp:Label>
                                          <%--<asp:Label ID="Label11" runat="server" Text="Cashier" align="right"></asp:Label>--%>
                                      </td></tr>       
                                      <tr>
                                      <td colspan="5" class ="removeallborder" align="center"  >
                                          <asp:Label ID="lblRegdOffice" runat="server" Font-Size="10px" align="center"></asp:Label>
                                      </td></tr>                                                              
                                          
          </table>   
    
    
    
    </div>
    </form>
</body>

</html>
