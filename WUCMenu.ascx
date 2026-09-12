<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WUCMenu.ascx.cs" Inherits="WUCMenu" %>

<!-- =====================================================================
     Sidebar menu for SitePage.master.
     FIXED / HARD-CODED replica of M_CompWiseWebMenuMasterDis (CompanyID 1108).
     NOT fetched from the database. Names, hrefs (OnSelect) and parent/child
     grouping match the original menu rows exactly, in the same order.
     Edit the <li> entries below to add/rename/repoint items.
     ===================================================================== -->
<div runat="server" id="menuContainer">
    <% if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        { %>

    <ul class="sidebar-menu" id="menu" runat="server">

        <!-- 1. Home -->
        <li class="dropdown">
            <a href="Indext.aspx" class="nav-link"><i data-feather="home"></i><span>Home</span></a>
        </li>
          <li class="dropdown">
      <a href="ShoppingRedirect.aspx" class="nav-link"><i data-feather="upload"></i><span>Back To Main</span></a>
  </li>
        <!-- 2. Profile -->
        <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="user"></i><span>Profile</span></a>
            <ul class="dropdown-menu">
             <%--   <li><a href="NewJoiningFreeUc.aspx">New Registrartion</a></li>--%>
                <li><a href="profileWithPostal.aspx">Edit Profile</a></li>
                <li><a href="ChangePass.aspx">Change Login Password</a></li>
                <li><a href="ChangeTransPass.aspx">Change Trans. Password</a></li>
            </ul>
        </li>

        <!-- 3. Upload KYC -->
        <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="upload"></i><span>Upload KYC</span></a>
            <ul class="dropdown-menu">
                <li><a href="KYC.aspx">Upload KYC</a></li>
                <li><a href="gstdetail.aspx">Upload GSTIN</a></li>
                 <li><a href="fasaidetail.aspx">Upload FSSAI</a></li>
            </ul>
        </li>

        <!-- 4. Documents -->
        <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="file-text"></i><span>Documents</span></a>
            <ul class="dropdown-menu">
                <li><a href="welcome.aspx">Welcome Letter</a></li>
            </ul>
        </li>

        <!-- 5. My Team -->
        <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="users"></i><span>My Team</span></a>
            <ul class="dropdown-menu">
                <li><a href="Mydirects.aspx">My Direct</a></li>
                <li><a href="Groupdirects.aspx">Level Wise Direct</a></li>
                <li><a href="Newtree.aspx">Geneology</a></li>
                <li><a href="Downline.aspx">Downline Detail</a></li>
                <li><a href="DownlinePurchase.aspx">Downline Purchase</a></li>
            </ul>
        </li>

        <!-- 7. My Incentive -->
        <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="gift"></i><span>My Incentive</span></a>
            <ul class="dropdown-menu">
                <li><a href="DailyBinaryIncome.aspx">Daily Incentive</a></li>
                <li><a href="NTimeRewardDv9.aspx">MY Bonanza</a></li>
            </ul>
        </li>

        <!-- 8. Shopping -->
       <%-- <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="shopping-cart"></i><span>Shopping</span></a>
            <ul class="dropdown-menu">
                <li><a href="IdactivationPostal.aspx">ID Activation</a></li>
                <li><a href="RepurchaseNow.aspx">Repurchase Now</a></li>
                <li><a href="Productrequestdetaildocket.aspx">My Purchase Detail</a></li>
            </ul>
        </li>--%>

        <!-- 8. Payout Section -->
        <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="dollar-sign"></i><span>Payout Section</span></a>
            <ul class="dropdown-menu">
                <li><a href="Rptwithdrawls.aspx">Bank Withdrawal</a></li>
            </ul>
        </li>

        <!-- 9. Wallet -->
        <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="credit-card"></i><span>Wallet</span></a>
            <ul class="dropdown-menu">
                <li><a href="AllWalletReport.aspx">Wallet Transaction Report</a></li>
                <li><a href="walletrequest.aspx">Wallet Request</a></li>
                <li><a href="walletrequestdetail.aspx">Wallet Request Detail</a></li>
            </ul>
        </li>

        <!-- 10. Complaint -->
        <li class="dropdown">
            <a class="menu-toggle nav-link has-dropdown"><i data-feather="message-square"></i><span>Complaint</span></a>
            <ul class="dropdown-menu">
                <li><a href="Complain.aspx">Raise Ticket</a></li>
                <li><a href="ComplainSolution.aspx">My Ticket Status</a></li>
            </ul>
        </li>

        <!-- 11. Logout -->
        <li class="dropdown">
            <a href="logout.aspx" class="nav-link"><i data-feather="log-out"></i><span>Logout</span></a>
        </li>

    </ul>

    <% }
        else if (Session["CompID"] != null && Session["CompID"].ToString() == "1057")
        { %>

    <ul class="sidebar-menu">
        <asp:Label ID="kit" runat="server"></asp:Label>
        <li><a href="" runat="server" id="zaranewjoining">Sign Up</a></li>
        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
        <li><a href="Defaultzara.aspx">Sign In</a></li>
    </ul>

    <% }
        else if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
        { %>

    <ul class="sidebar-menu">
        <li><a href="" runat="server" id="AnewjoiningCashLess">Sign Up</a></li>
        <li><a href="Default.aspx">Sign In</a></li>
    </ul>

    <% }
        else
        { %>

    <ul class="sidebar-menu">
        <li><a href="" runat="server" id="Anewjoining">Registration</a></li>
        <li><a href="Default.aspx">Login</a></li>
    </ul>

    <% } %>
</div>
