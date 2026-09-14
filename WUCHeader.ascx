<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WUCHeader.ascx.vb" Inherits="WUCHeader" %>
<div class="top_nav">
    <div class="nav_menu">
        <nav>
                    <div class="nav toggle">
                        <a id="menu_toggle"><i class="fa fa-bars" style="color:Black"></i></a>
                    </div>


                    <div class="nav toggle hidden-xs" style="padding-top :4px">
                    <asp:Image ID="Image2" runat="server" Style="border-radius: 30%; height:40px" class="rounded-circle" />
                <%-- <img src="" alt="Home" style="width:120px;" runat="server" id="imgLogo" />--%>
                    </div>

                    <ul class="nav navbar-nav navbar-right" style="width:80%">
                        <li class="">
                            <a href="" class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                
                              <%--  <img id="NavigationLinks_Image2" alt="" src="images/user.png" />--%>
                                <span id="NavigationLinks_lb_name2"><%=Session("UserName")%></span>
                                <span class=" fa fa-angle-down"></span>
                            </a>
                            <ul class="dropdown-menu dropdown-usermenu pull-right">
                                <li><a href="logout.aspx"><i class="fa fa-sign-out pull-right"></i>Log Out</a></li>
                            </ul>
                        </li>
                    </ul>
                </nav>
    </div>
</div>
