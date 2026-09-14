<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Imgpan.aspx.vb" Inherits="App_UI_Application_Pages_Imgpan" %>

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
    
    
    
   
    
   
  
                <td>
                <asp:Label ID="LblNewPic" runat="server" Text="Upload New Image"></asp:Label>
                <br />
              
            
                   
                 <asp:FileUpload ID="ImageUpload" CssClass="Btn" runat="server" Visible="True" />
                 <br />
            
                    <asp:Button CssClass="Btn" ID="Upload" runat="server" Visible="True" Text="Upload" />
                  <asp:Button CssClass="Btn" ID="Cancel" runat="server" Visible="True" Text="Cancel" />
                  </td> 
                  </tr>
                  </table>
    </form>
</body>
</html>
