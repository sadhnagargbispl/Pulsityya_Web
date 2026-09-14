<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="WeeklyIncentive.aspx.vb" Inherits="WeeklyIncentive"
    Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css">
    
.PagerStyle 
{
    background-image: url(../Images/td.jpg);
    background-position:center;
    background-repeat:repeat-x;
    background-color:#ffffff; 
    font-weight:bold;
    text-align: center;
    width: 00px;   
}

.PagerStyle table
{
	text-align:center;
    margin:auto;
}
.PagerStyle table td
{
    border:0px;
    padding:5px;
}
.PagerStyle td
{
    border-top: #1d1d1d 3px solid;
}
.PagerStyle a
{
    color:#000000;
    text-decoration:none;
    padding:2px 10px 2px 10px;
    border-top:solid 1px #777777;
    border-right:solid 1px #333333;
    border-bottom:solid 1px #333333;
    border-left:solid 1px #777777;
}
.PagerStyle span
{
    font-weight:bold;
    color:#000000;
    text-decoration:none;
    padding:2px 10px 2px 10px;
}



</style>
<style type="text/css" >
#doublescroll { overflow: auto; overflow-y: hidden; } #doublescroll p { margin: 0; padding: 1em; white-space: nowrap; }</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                           Member List</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
 
                <div class="col-md-4">
                    <asp:Label ID="Label2" runat="server" Text="Choose Date: "></asp:Label>
                    <asp:DropDownList ID="CmbType" runat="server" class="form-control">
                        <asp:ListItem Selected="True" Value="J">Joining Date</asp:ListItem>
                        <asp:ListItem Value="A">Activation Date</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                 <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                   
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                
                </div>
                <div class="col-md-4">
                <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                   
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                </div>
                <div class="col-md-4">
                 <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package :" TextAlign="Left" />
                    <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                 <asp:CheckBox ID="ChkBank" runat="server" Text="Choose Bank :" TextAlign="Left" />
                    <asp:DropDownList ID="DdlBank" runat="server" class="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                 <asp:CheckBox ID="ChkMem" runat="server" Text="MemberId :" TextAlign="Left" />
                    <asp:TextBox ID="txtMember" runat="server" class="form-control"></asp:TextBox>
                </div>
     <div class="col-md-12"><br /></div>
          <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                    <asp:Button ID="btnshowall" runat="server" class="btn btn-primary" Text="Show All" />
                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                    <asp:Button ID="BtnExportCsv" runat="server" class="btn btn-primary" Text="Export To CSV" />
                    <asp:Button ID="BtnBankDetail" runat="server" class="btn btn-primary" Text="View Bank Detail " Visible ="false" />
                    <asp:Button ID="BtnExportBank" runat="server" class="btn btn-primary" Text="Export Bank Detail" Visible="false" />
                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" />
                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" />
                   
               </div>
             
            <div class="col-md-12">
                <asp:Label ID="lblErr" runat="server" style="font-weight:bold;font-size:12px;color:Red" ></asp:Label>
               </div>
   
    <div  class="col-md-12">
        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated." Visible ="false"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                     <asp:Label ID="lblCount" runat="server" style="font-weight:bold;font-size:14px;color:Gray" ></asp:Label>
    </div>
      <div id="doublescroll" class="col-md-12">
        <p>
     
    <div id="gvContainer" runat="server" class="table table-bordered" >
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
            GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary" 
            ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
            <Columns >
            <asp:BoundField  DataField ="Sessid" HeaderText="Sessid" />
            <asp:BoundField DataField ="KitName" HeaderText="PackageName" />
            <asp:BoundField DataField ="Idnumber" HeaderText="IdNo" />
            <asp:BoundField DataField ="MemberName" HeaderText="MemberName" />
          <%--  <asp:BoundField DataField ="UplinerId" HeaderText="UplinerId" />
            <asp:BoundField DataField ="UplinerName" HeaderText="UplinerName" />--%>
            <asp:BoundField DataField ="SponsorId" HeaderText="SponsorId" />
            <asp:BoundField DataField ="Sponsorname" HeaderText="SponsorName" />
          <%--   <asp:BoundField DataField ="LegName" HeaderText="LegName" />--%>
            <asp:BoundField DataField ="DateOfBirth" HeaderText="DateOfBirth" />
            <asp:BoundField DataField ="Gender" HeaderText="Gender" />
            <asp:BoundField DataField ="Address" HeaderText="Address" />
            <asp:BoundField DataField ="City" HeaderText="City" />
            <asp:BoundField DataField ="State" HeaderText="State" />
            <asp:BoundField DataField ="PinCode" HeaderText="PinCode" />
            <asp:BoundField DataField ="MobileNo" HeaderText="MobileNo" />
            <asp:BoundField DataField ="Password" HeaderText="Password" />
            <asp:BoundField DataField ="DOJ" HeaderText="DOJ" />
             <asp:BoundField DataField ="NomineeName" HeaderText="NomineeName" />
             <asp:BoundField DataField ="PANNo" HeaderText="PANNo" />
             <asp:BoundField DataField ="BankName" HeaderText="BankName" />
             <asp:BoundField DataField ="AccountNo" HeaderText="AccountNo" />            
             <asp:BoundField DataField ="IFSCCode" HeaderText="IFSC Code" />
             <asp:BoundField DataField ="BV" HeaderText="BV" />
             <asp:BoundField DataField ="UpgradeSessid" HeaderText="UpgradeSessid" />
             <asp:BoundField DataField ="UpgradeDate" HeaderText="UpgradeDate" />
             <asp:BoundField DataField ="Status" HeaderText="Status" />
            
             </Columns>
             <PagerStyle CssClass ="PagerStyle" />
               <PagerSettings Mode ="NumericFirstLast" />
           
        </asp:GridView>
    </div>
      </p>
            </div>

     <script type="text/javascript" >

        function DoubleScroll(element) {
             var scrollbar = document.createElement('div');
            scrollbar.appendChild(document.createElement('div'));
            scrollbar.style.overflow = 'auto';
            scrollbar.style.overflowY = 'hidden';
            scrollbar.firstChild.style.width = element.scrollWidth + 'px';
            scrollbar.firstChild.style.paddingTop = '1px';
            scrollbar.firstChild.appendChild(document.createTextNode('\xA0'));
            scrollbar.onscroll = function() { element.scrollLeft = scrollbar.scrollLeft; };
            element.onscroll = function() { scrollbar.scrollLeft = element.scrollLeft; }; 
            element.parentNode.insertBefore(scrollbar, element);
        } DoubleScroll(document.getElementById('doublescroll'));</script>
        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- end of weather widget -->
        </div>
    </div>
</asp:Content>
