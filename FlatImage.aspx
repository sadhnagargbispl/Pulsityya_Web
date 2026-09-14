<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FlatImage.aspx.vb" Inherits="FlatImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
          <script src="popupassets/popper.min.js"></script>

                            <script src="popupassets/lib.js"></script>

                            <script src="popupassets/jquery.flagstrap.min.js"></script>

                            <script type="text/javascript" src="popupassets/jquery.themepunch.tools.min.js"></script>

                            <script type="text/javascript" src="popupassets/jquery.themepunch.revolution.min.js"></script>

                            <script src="js/functions1.js"></script>
    <asp:Repeater ID="RptPhotos" runat="server">
                <ItemTemplate>
                <div class="col-md-12">
                   <div class="col-sm-3" style=" margin-bottom :15px; margin-top :15px;">
              <a class="fbox" rel="group" href='<%# Eval("ImgPath") %>'>  <img src='<%# Eval("ImgPath") %>'  class="img-responsive"/></a>
              
              </div>
             </div>
            
                </ItemTemplate>
                </asp:Repeater>
                  <script src="assets/jquery.inbox.js"></script>

    <script type="text/javascript" src="popupassets/jquery.fancybox.pack.js"></script>

    <script type="text/javascript" src="popupassets/jquery.fancybox.pack1.js"></script>

    <link rel="stylesheet" href="popupassets/jquery.fancybox.css" type="text/css" media="screen" />

    <script type="text/javascript">
        $(document).ready(function() {
            $(".fbox").fancybox({
                openEffect: 'elastic',
                closeEffect: 'elastic'
            });
        });
    </script>
    </form>
</body>
</html>
