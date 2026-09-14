<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WUCMenu.ascx.vb" Inherits="WUCMenu" %>
<div class="col-md-3 left_col">
    <div class="left_col scroll-view">
        <div class="navbar nav_title" style="border: 0;">
            <a href="Home.aspx" class="site_title" id="ahome" runat="server"><span><%=Session("CompName")%></span></a>
        </div>
        <div class="clearfix"></div>
        <hr style="margin-bottom: -1px;">

        <!-- menu profile -->
        <div class="profile clearfix">
            <div class="profile_pic">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="Upload_photo.aspx"></a>
            </div>
            <div class="profile_info">
                <span>Welcome
                <h2>
                    <span id="NavigationLinks_lb_name"><%=Session("UserName")%></span>,<br /></span>
            </div>
        </div>
        <hr style="margin-bottom: -10px;">

        <!-- ================================================================
             DYNAMIC admin sidebar menu.
             Menu rows aate hain M_CompWiseWebMenuMaster se, aur
             M_UserPermissionMaster ke through login group ki permission
             ke hisaab se filter hote hain. Code-behind (Load_Menu) HTML banata hai.
             ================================================================ -->
        <div id="sidebar-menu" class="main_menu_side hidden-print main_menu">
            <div class="menu_section">
                <ul class="nav side-menu" id="menu" runat="server">
                    <!-- yahan code-behind se menu inject hoga -->
                </ul>
            </div>
        </div>
        <!-- /sidebar menu -->
    </div>
</div>