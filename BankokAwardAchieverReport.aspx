<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="BankokAwardAchieverReport.aspx.vb" Inherits="App_UI_Application_Pages_BankokAwardAchieverReport"
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
            
          
            <%--<tr>
                <td>
                </td>
                <td style ="width :200px">
                    <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="TxtBox" Width="150px"></asp:TextBox>
                   
                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                </td>
                <td>
                    <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="TxtBox" Width="150px"></asp:TextBox>
                                   <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                        Format="dd-MMM-yyyy">
                    </ajaxtoolkit:CalendarExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                </td>
                <td>
                </td>
            </tr>--%>
            <tr>
                <td>
                </td>
                <td>
                   <asp:CheckBox ID="ChkMem" runat="server" Text="MemberId :" TextAlign="Left" />
                    <asp:TextBox ID="txtMember" runat="server" CssClass ="TxtBox " Width="150px"></asp:TextBox>
                </td>
                <td  style="height:5%">
               
                </td>
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
                    <asp:Button ID="btnExport" runat="server" CssClass="Btn" Text="Export To Excel" />
                   <%-- <asp:Button ID="BtnExportCsv" runat="server" CssClass ="Btn" Text="Export To CSV" />--%>
                    
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
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
            GridLines="Both" AllowPaging="true" CssClass="table table-striped table-advance table-hover" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="25" EmptyDataText="No data to display.">
            <Columns >
          <asp:TemplateField  HeaderText="S.No">
          <ItemTemplate ><%#Container.DataItemIndex + 1%> .</ItemTemplate></asp:TemplateField>
                </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
</asp:Content>
