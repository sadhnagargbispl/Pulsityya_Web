<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Imgcompwise.aspx.vb" Inherits="Imgcompwise" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table style="height: 320px; width:403px">
    <tr>
    <td>
  <asp:Label ID="LblPic" runat="server" Text="Previous Image"></asp:Label>
  <asp:Label ID="LblUpdatePic" runat="server" Text="Updated Image" Visible="false" ></asp:Label>
    <%--Previous Profile--%>
    <br />
    <asp:Image runat="server" Width="100%" ID="Image1" />
    </td>
    
   <td>
   </td>
    
    
    
   
    
    <%--<table id="tblImage" runat="server" visible="false"  >--%>
  
                <td>
                <asp:Label ID="LblNewPic" runat="server" Text="Upload New Image"></asp:Label>
                  <br /><br /><br />
              
                    <%--<asp:Image ID="DistImage" runat="server" Height="100px" Width="111px" 
                        Visible="true" />--%>
                <%--<<%--/td>--%>
            <%--</tr>            
            <tr>
                <td>--%>
                   
                 <asp:FileUpload ID="ImageUpload" CssClass="Btn" runat="server" Visible="True" />
                 <br /><br /><br />
               <%-- </td>
            </tr>
            <tr>
                <td>--%>
                    <asp:Button CssClass="Btn" ID="Upload" runat="server" Visible="True" Text="Upload" />
                  <asp:Button CssClass="Btn" ID="Cancel" runat="server" Visible="True" Text="Cancel" />
                  </td> 
                  
                  
                  </tr>
                    <tr style=" margin: 5px; " id="divsponsor" runat="server" visible="false" >
                   <td >
  <asp:Label ID="lblsponsor" runat="server" Text="Sponsor :"></asp:Label>
  <asp:Label ID="lblsponsoridno" runat="server"></asp:Label>
  
    <br />
   
    </td>
                  </tr>
                   <tr style=" margin: 5px; " id="divsponsorname" runat="server" visible="false" >
                   <td >
 <asp:Label ID="lbln" runat="server" Text="Sponsor Name :"></asp:Label>
  <asp:Label ID="lblsponsorname" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
               <tr id="dividno" runat="server" style="margin-top:10px;">
                    <td style="padding-top:15px;">
  <asp:Label ID="Label2" runat="server" Text="IdNo :"></asp:Label>
  <asp:Label ID="lblidno" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="divname" runat="server">
                  <td >
  <asp:Label ID="Label1" runat="server" Text="Name :"></asp:Label>
  <asp:Label ID="lblname" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  
                      <tr style=" margin: 5px; " id="divTransactionNo" runat="server">
                   <td >
  <asp:Label ID="LabeTransaction" runat="server" Text="Cheque/ TransactionNo :"></asp:Label>
  <asp:Label ID="LabeTransactionNo" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  
                      <tr style=" margin: 5px; " id="divAmount" runat="server">
                   <td >
  <asp:Label ID="LabeAmount" runat="server" Text="Amount :"></asp:Label>
  <asp:Label ID="LabelAmount" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="divfname" runat="server">
                  <td>
  <asp:Label ID="lbname" runat="server" Text="Fathername :"></asp:Label>
  <asp:Label ID="lblfname" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                   <tr style=" margin: 5px; " id="type" runat="server">
                  <td>
  <asp:Label ID="lbl4" runat="server" Text="Id Type :"></asp:Label>
  <asp:Label ID="lblidtype" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  
                   <tr style=" margin: 5px; " id="idproofno" runat="server">
                  <td>
  <asp:Label ID="Label3" runat="server" Text="Address Proof No :"></asp:Label>
  <asp:Label ID="lblidproofno" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                   <tr style=" margin: 5px; " id="divaddress" runat="server">
                  <td>
  <asp:Label ID="Label4" runat="server" Text="Address :"></asp:Label>
  <asp:Label ID="lbladdress" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="divpincode" runat="server">
                  <td>
  <asp:Label ID="lbl8" runat="server" Text="Pincode :"></asp:Label>
  <asp:Label ID="lblpincode" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="divcity" runat="server">
                  <td>
  <asp:Label ID="lbl5" runat="server" Text="City Name :"></asp:Label>
  <asp:Label ID="lblcity" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                   <tr style=" margin: 5px; " id="divdistrict" runat="server">
                  <td>
  <asp:Label ID="lbl7" runat="server" Text="District Name :"></asp:Label>
  <asp:Label ID="lbldistrict" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="divstate" runat="server">
                  <td>
  <asp:Label ID="lbl6" runat="server" Text="State Name :"></asp:Label>
  <asp:Label ID="lblstate" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  
                  
                  
                   
                  
                  <tr style=" margin: 5px; " id="divbankname" runat="server">
                  <td>
  <asp:Label ID="Label5" runat="server" Text="Bank Name :"></asp:Label>
  <asp:Label ID="lblbankname" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="divaccno" runat="server">
                  <td>
  <asp:Label ID="Label6" runat="server" Text="Account No :"></asp:Label>
  <asp:Label ID="lblaccno" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                   <tr style=" margin: 5px; " id="divbranchname" runat="server">
                  <td>
  <asp:Label ID="Label7" runat="server" Text="Branch Name :"></asp:Label>
  <asp:Label ID="lblbranchname" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="divifsccode" runat="server">
                  <td>
  <asp:Label ID="Label8" runat="server" Text="IFSC Code :"></asp:Label>
  <asp:Label ID="lblifsccode" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                   <tr style=" margin: 5px; " id="divmobile" runat="server" visible="false" >
                  <td>
  <asp:Label ID="lblipf" runat="server" Text="Mobile No :"></asp:Label>
  <asp:Label ID="lblmobl" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                   <tr style=" margin: 5px; " id="divemail" runat="server" visible="false" >
                  <td>
  <asp:Label ID="lblem" runat="server" Text="Email :"></asp:Label>
  <asp:Label ID="lblemail" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="divpan" runat="server">
                  <td>
  <asp:Label ID="Label9" runat="server" Text="Pan No :"></asp:Label>
  <asp:Label ID="lblpanno" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  <tr style=" margin: 5px; " id="addharno" runat="server">
                  <td>
  <asp:Label ID="lblaadhar" runat="server" Text="Aadhar No :"></asp:Label>
  <asp:Label ID="lablaadharno" runat="server"></asp:Label>
    <br />
   
    </td>
                  </tr>
                  
                  </table>
                  
    </form>
</body>
</html>
