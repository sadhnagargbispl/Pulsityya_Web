<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="CummulativeReport.aspx.vb" Inherits="App_UI_Application_Pages_CummulativeReport"
    Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css" >
.DDl {
  display: block;
  width: 200px;
  height: 24px;
  padding: 3px 16px;
  font-size: 14px;
  line-height: 1.428571429;
  color: #8e8e93;
  vertical-align: middle;
  background-color: #ffffff;
  border: 1px solid #c7c7cc;
  border-radius: 4px;  
  -webkit-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
  transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Resources/CSS/Grid.css" rel="stylesheet" type="text/css" />
    
     
    <div style="background-color: White ">
        <table width="100%" cellspacing="10px" align="left" style="margin: 20px; padding: 0px;">
            
          
            <tr>
            <td></td>
                <td>
                <asp:RadioButtonList ID="RbtSearch" runat="server" AutoPostBack="true" RepeatColumns ="2" RepeatDirection ="Horizontal" RepeatLayout ="Flow" >
                <asp:ListItem Text="Member Wise" Value="M" Selected ="True"></asp:ListItem>
                <asp:ListItem Text="Date Wise" Value="D" ></asp:ListItem></asp:RadioButtonList>
                </td>
                <td></td><td></td>
                </tr>
                <tr>
                <td></td>
                <td style ="width :200px">
                     <asp:DropDownList  ID="DDlFromDate" runat="server" CssClass ="DDl" Width="160px"></asp:DropDownList> 
                </td>
                <td>
                     <%-- <asp:CheckBox ID="ChkMem" runat="server" Text="Member Id :" TextAlign="Left" />--%>
                    <asp:TextBox ID="txtMember" runat="server" CssClass ="TxtBox " Width="150px"></asp:TextBox>               </td>
                <td>
                </td>
            </tr>
            
           <%-- <tr>
                <td>
                </td>
                <td>
                   
                   
                </td>
                <td colspan="2">
                </td>
            </tr>--%>
            <tr>
                <td style="padding-top :25px;">
                </td>
                <td colspan="3" style="padding-top :15px;">
                    <asp:Button ID="btnSearch" runat="server" CssClass="Btn" Text="Search" />
                   <%-- <asp:Button ID="btnshowall" runat="server" CssClass="Btn" Text="Show All" />--%>
                    <asp:Button ID="btnExport" runat="server" CssClass="Btn" Text="Export To Excel"  />
             
                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" CssClass="Btn" />
                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" CssClass="Btn" />
                   
                </td>
                <%--<td><asp:Button ID="BtnAdvSearch" runat="server" CssClass="Btn" Text="Advanced Search" /></td>--%>
            </tr>
            <tr>
                <td colspan="4" > 
                <asp:Label ID="lblErr" runat="server" style="font-weight:bold;font-size:12px;color:Red" ></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style=" margin-top: 20px; margin-bottom: 20px;">
        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated." Visible ="false"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                     <asp:Label ID="lblCount" runat="server" style="font-weight:bold;font-size:14px;color:Gray" ></asp:Label>
    </div>
    <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px;
        margin-left: 25px; margin-bottom: 25px;">
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
            GridLines="Both" AllowPaging="true" CssClass="table table-striped table-advance table-hover" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="25" EmptyDataText="No data to display.">
            <Columns >
            <asp:BoundField DataField ="UserId" HeaderText="Member Id" />
            <asp:BoundField DataField ="MemName" HeaderText="MemberName" />
            <asp:BoundField DataField ="KitName" HeaderText="Package Name" />
            <asp:BoundField DataField ="KitBv" HeaderText="PackageBv" />
            <asp:BoundField DataField ="MobileNo" HeaderText="MobileNo" />
            <asp:BoundField DataField ="DateOfJoining" HeaderText ="Date Of Joining" />
            <asp:BoundField DataField ="LeftJoining" HeaderText="LeftTeam" />
            <asp:BoundField DataField ="RightJoining" HeaderText="RightTeam" />
            <asp:BoundField DataField ="LeftBv" HeaderText="Left Bv" />
            <asp:BoundField DataField ="RightBv" HeaderText="Right Bv" />
            <asp:BoundField DataField ="matchedBv" HeaderText="Matching Bv" />
            <asp:BoundField DataField ="DirectIncome" HeaderText="Matching Income" />
            <asp:BoundField DataField ="RepurchaseIncome" HeaderText="RepurchaseIncome" />
            <asp:BoundField DataField ="TotalIncome" HeaderText="Total Income" />
            <asp:BoundField DataField ="WalletAmount" HeaderText ="WalletAmount" />
            <asp:BoundField DataField ="ReportDate" HeaderText="ReportDate" />
            
            
            
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
</asp:Content>
