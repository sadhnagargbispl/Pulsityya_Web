<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="Masterincome.aspx.vb" Inherits="Masterincome" Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                           Master Income Report
                        </h2>
                        <div class="clearfix">
                        </div>
                        
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12">
                                    <%--<div class="col-md-2"> Enter IdNo</div>--%>
                                     
                                    <div class="col-md-4">
                                    <asp:Label ID="lblenteridno" runat="server" Text="Enter IdNo: "></asp:Label>
                                        <asp:TextBox ID="txtMember" runat="server" class="form-control" AutoPostBack="true" ></asp:TextBox></div>
                                    <div class="col-md-4">
                                    <asp:Label ID="lblSessionDate" runat="server" Text="Choose From Date : "></asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                                        ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblToDate" runat="server" Text="Choose To Date : "></asp:Label>
                                    <asp:TextBox ID="TxtToDate" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                        ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                    <div class="col-md-6">
                                     
                                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Excel" />
                              
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print" class="btn btn-primary" Visible ="false" />
                                   <a href="Addmasterincome.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 700,height: 430,marginTop : 0 } )">
                                    <asp:Button ID="BtADd" runat="server" class="btn btn-primary" Text="Add Income" /></a> 
                                </div>
                                <asp:Label ID="lblmessage" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label>
                                </div>
                                
                                <div class="col-md-12">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label>
                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray"></asp:Label>
                                </div>
                            </div>
                            <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                margin-bottom: 25px;" class="col-md-12">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false"  RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                    PageSize="25" EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1%>.</ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField ="Idno" HeaderText="IdNo" />
                                        <asp:BoundField DataField ="MemberName" HeaderText="Member Name" />
                                        <asp:BoundField DataField ="Amount" HeaderText="Amount" />
                                          <asp:BoundField DataField ="Date" HeaderText="Date" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <br />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
