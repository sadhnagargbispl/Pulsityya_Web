<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="App_UI_Application_Pages_Default" %>

<%@ Register TagPrefix="uc" Namespace="ASPNET_Captcha" Assembly="ASPNET_Captcha" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <!--META-->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%=Session("Title")%></title>
    <!--STYLESHEETS-->
    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <!--SCRIPTS-->

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $(".username").focus(function() {
                $(".user-icon").css("left", "-48px");
            });
            $(".username").blur(function() {
                $(".user-icon").css("left", "0px");
            });

            $(".password").focus(function() {
                $(".pass-icon").css("left", "-48px");
            });
            $(".password").blur(function() {
                $(".pass-icon").css("left", "0px");
            });
        });
    </script>

    <script language="javascript" type="text/javascript">
        function PutCursor() {
            document.frm.uid.focus();
        }
        function ChkForm() {
            if (document.frm.uid.value == "" || document.frm.uid.value == 'Username') {
                alert("Please Enter Username");
                document.frm.uid.focus();
                return false;
            }
            if (document.frm.pwd.value == "" || document.frm.pwd.value == 'Password') {
                alert("Please Enter Password");
                document.frm.pwd.focus();
                return false;
            }
            return true;
        }
    </script>

    <script type="text/javascript">
        function disableBackButton() {
            window.history.forward();
        }
        setTimeout("disableBackButton()", 0);
    </script>
  <script type="text/javascript">
        window.history.forward();
        function noBack() {
            window.history.forward();
        } 
    </script>
</head>
<body>
    <div id="wrapper">
        <div class="user-icon">
        </div>
        <div class="pass-icon">
        </div>
        <form name="frm" id="frm" runat="server" class="login-form">
        <AjaxToolkit:ToolkitScriptManager ID="scriptmanager1" runat="server">
        </AjaxToolkit:ToolkitScriptManager>
        <div class="header" align="center">
            <img src="images/logo.png" style="max-width: 150px;" runat="server" id="imgLogo" />
            <br />
            <br />
            <span>Fill out the form below to login to your account.</span>
        </div>
        <div class="content" id="DVLgin" runat="server">
            
            <asp:TextBox ID="TxtUID" runat="server" class="input password" placeholder="Enter User ID"></asp:TextBox>
        </div>
        <div class="content" id="DivPassword" runat="server" visible="true">
            <asp:TextBox ID="TxtPWD" runat="server" class="input" placeholder="Enter Password"
                TextMode="Password"></asp:TextBox>
            <br />
        </div>
        <div class="footer" align="center" id="DVLginF" runat="server">
            <asp:Button ID="BtnGenOTP" runat="server" OnClientClick="return ChkForm();" OnClick="BtnGenOTP_Click"
                ValidationGroup="Save" class="button" Text="Login"></asp:Button>
        </div>
        <div class="content" id="DVOtp" runat="server" visible="false">
        <span style="font-size: 12px; font-family: Tahoma; color: Red;">
                <asp:Label ID="LblEmailMsg" runat="server" Text=""></asp:Label></span>
                <br />
            <asp:TextBox ID="TxtOTP" runat="server" class="input username" placeholder="Enter OTP"></asp:TextBox>
            <br />
            <span style="font-size: 11px; font-family: Tahoma">You can try 3 times only.</span>
        </div>
        <div class="footer" align="center" id="DVOtpLogin" runat="server" visible="false">
            <asp:Button ID="BtnLogin" runat="server" ValidationGroup="Save" class="button" Text="Login">
            </asp:Button>
        </div>
        </form>
    </div>
    <div class="gradient">
    </div>
    <% If Request("Error") <> "" Then%>

    <script language="javascript" type="text/javascript">
        alert('Invalid Login');
    </script>

    <% End If%>
</body>
</html>
