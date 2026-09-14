<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="IncentiveDetailReportNew.aspx.vb" Inherits="App_UI_Application_Pages_IncentiveDetailReportNew"
    Title="" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        //$(document).ready(function() { $('[id$=chkSelectAll]').click(function() { $("[id$='chkSelect']").attr('checked', this.checked); }); });
        //    function reset() {
        //        $("[id$='chkSelect']").prop('checked', false);
        //    }



        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%= GvData.ClientID %>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell 
                            //checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

    </script>

    <style>
        rr
        {
            color: Black !important;
            text-decoration: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <img alt="" src="images/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            <% If Session("compid")="1096" then %>
                            Core Team Bonus Report
                            <% Else %>
                            Incentive Detail Report
                            <%End If  %>
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <%--<%If Session("CompID") = 1007 Then%>
            
            <%End If%>--%>
                    <div class="col-md-12">
                        <div class="col-md-1">
                            <asp:CheckBox ID="CheckBox1" runat="server" Text="Member ID :" Font-Bold="true" />
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:CheckBox ID="CheckBox2" runat="server" Text="Session Wise :" Font-Bold="true"
                                Checked="true" />
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlSession" runat="server" class="form-control" Style="text-indent: 1px;">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            Page Size:</div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" AutoPostBack="true"
                                OnSelectedIndexChanged="PageSize_Changed">
                                <asp:ListItem Text="10" Value="10" />
                                <asp:ListItem Text="20" Value="20" />
                                <asp:ListItem Text="50" Value="50" />
                                <asp:ListItem Text="100" Value="100" />
                                <asp:ListItem Text="500" Value="500" />
                                <asp:ListItem Text="1000" Value="1000" />
                                <asp:ListItem Text="2000" Value="2000" />
                                <asp:ListItem Text="5000" Value="5000"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-12" style="padding: 5px">
                        <div class="col-md-2">
                            <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" /></div>
                        <div class="col-md-2">
                            <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                        <div class="col-md-2">
                            <asp:Button ID="btnSendSms" runat="server" class="btn btn-primary" Text="Send Sms"
                                Visible="false" /></div>
                        <div class="col-md-2">
                            <asp:Button ID="BtnSendSmsToAll" runat="server" class="btn btn-primary" Text="Send Sms To All"
                                Visible="false" /></div>
                        <div class="col-md-2">
                            <asp:Label ID="LblSessionNo" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>
                        <div class="col-md-2">
                        </div>
                    </div>
                </div>
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="padding: 10px 10px 20px 10px; overflow: scroll" id="divSummary" runat="server"
                            visible="false">
                            <asp:GridView ID="GvSummary" Width="100%" runat="server" AllowPaging="True" PageSize="200"
                                GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="PayoutDate">
                                        <ItemTemplate>
                                            <asp:Label ID="LblGrpId" runat="server" Text='<%# Eval("Sessid") %>' Visible="False"></asp:Label>
                                            <asp:Label ID="LblPayoutDate" runat="server" Text='<%# Eval("PayoutDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TotalCount" HeaderText="Total Paid Id" />
                                    <asp:BoundField DataField="NetIncome" HeaderText="Net Income" />
                                    <asp:BoundField DataField="TDSAmount" HeaderText="TDS Amount" />
                                    <asp:BoundField DataField="NetAmount" HeaderText="Payable Amount" />
                                    <asp:TemplateField HeaderText="View Detail" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBDelete" runat="server" Text="View Detail" OnClick="ViewDetail"
                                                ForeColor="Black"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle Width="55px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div style="padding: 10px 10px 20px 10px; overflow: scroll" id="divDetail" runat="server"
                            visible="false">
                            <%--<asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="false" GridLines="Both"
                                class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                EmptyDataText="No data to display." AutoGenerateColumns="true" AllowSorting="true"
                                OnSorting="GvData_Sorting">                                 
                            </asp:GridView>--%>
                            <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="false" GridLines="Both"
                                class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                EmptyDataText="No data to display." AutoGenerateColumns="false" AllowSorting="true"
                                OnSorting="GvData_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="SNo" SortExpression="SNo">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Payoutno" HeaderText="PayoutNo" SortExpression="PayoutNo">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Idno" HeaderText="IDNo" SortExpression="IDNo"></asp:BoundField>
                                    <asp:BoundField DataField="MemFirstName" HeaderText="Member Name" SortExpression="MemFirstName">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Mobl" HeaderText="Mobile No" SortExpression="Mobl"></asp:BoundField>
                                    <asp:BoundField DataField="Panno" HeaderText="Panno" SortExpression="Panno"></asp:BoundField>
                                    <asp:BoundField DataField="SALES INCENTIVES" HeaderText="SALES INCENTIVES" SortExpression="SALES INCENTIVES">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SALES BOOSTER BONUS" HeaderText="SALES BOOSTER BONUS"
                                        SortExpression="SALES BOOSTER BONUS"></asp:BoundField>
                                    <asp:BoundField DataField="RECOGNITION ACHIEVEMENT BONUS" HeaderText="RECOGNITION ACHIEVEMENT BONUS"
                                        SortExpression="RECOGNITION ACHIEVEMENT BONUS"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Mission 365" SortExpression="Mission 365">
                                        <ItemTemplate>
                                            <a href='Mission365view.aspx?idno=<%# Eval("idno") %>&PayoutNo=<%# Eval("PayoutNo")%>'
                                                target="_blank" style="color: Blue;" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                <%# Eval("Mission 365")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Gross Prev." HeaderText="Gross Prev." SortExpression="Gross Prev.">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Gross Income" HeaderText="Gross Income" SortExpression="Gross Income">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Gross Closing" HeaderText="Gross Closing" SortExpression="Gross Closing">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TRD" HeaderText="TRD" SortExpression="TRD"></asp:BoundField>
                                    <asp:BoundField DataField="TDS Amount" HeaderText="TDS Amount" SortExpression="TDS Amount">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Admin Charge" HeaderText="Admin Charge" SortExpression="Admin Charge">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Repurchase" HeaderText="Repurchase" SortExpression="Repurchase">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total Deduction" HeaderText="Total Deduction" SortExpression="Total Deduction">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Previous Balance" HeaderText="Previous Balance" SortExpression="Previous Balance">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Net Income" HeaderText="Net Income" SortExpression="Net Income">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Carry Forward Bal" HeaderText="Carry Forward Bal" SortExpression="Carry Forward Bal">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Left B/F PV" HeaderText="Left B/F PV" SortExpression="Left B/F PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Right B/F PV" HeaderText="Right B/F PV" SortExpression="Right B/F PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Left Joining PV" HeaderText="Left Joining PV" SortExpression="Left Joining PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Right Joining PV" HeaderText="Right Joining PV" SortExpression="Right Joining PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Left C/F PV" HeaderText="Left C/F PV" SortExpression="Left C/F PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Right C/F PV" HeaderText="Right C/F PV" SortExpression="Right C/F PV">
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="Left Repurchase PV" HeaderText="Left Repurchase PV" SortExpression="Left Repurchase PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Right Repurchase PV" HeaderText="Right Repurchase PV"
                                        SortExpression="Right Repurchase PV"></asp:BoundField>
                                    <asp:BoundField DataField="Self Repurchase PV" HeaderText="Self Repurchase PV" SortExpression="RECOGNITION ACHIEVEMENT BONUS">
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="Total Left PV" HeaderText="Total Left PV" SortExpression="Total Left PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total Right PV" HeaderText="Total Right PV" SortExpression="Total Right PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Till Matched PV" HeaderText="Till Matched PV" SortExpression="Till Matched PV">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PanStatus" HeaderText="Pan Status" SortExpression="PanStatus">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BankStatus" HeaderText="BankStatus" SortExpression="BankStatus">
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <asp:Repeater ID="rptPager" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                        CssClass='<%# If(Convert.ToBoolean(Eval("Enabled")), "page_enabled", "page_disabled")%>'
                                        OnClick="Page_Changed" OnClientClick='<%# If(Not Convert.ToBoolean(Eval("Enabled")), "return false;", "") %>'
                                        ForeColor="Black"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlPageSize" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div style="padding: 10px 10px 20px 10px; overflow: scroll" id="DivBsnReport" runat="server"
                            visible="false">
                            <%--<style>
                                a
                                {
                                    color: #3d3a3a !important;
                                    text-decoration: none;
                                }
                            </style>--%>
                            <asp:GridView ID="GRDBSNVIEW" Width="100%" runat="server" AllowPaging="True" PageSize="200"
                                GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false"
                                OnRowDataBound="GRDBSNVIEW_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="SNo" SortExpression="SNo">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PayoutDate">
                                        <ItemTemplate>
                                            <asp:Label ID="LblGrpId" runat="server" Text='<%# Eval("Sessid") %>' Visible="False"></asp:Label>
                                            <asp:Label ID="LblPayoutDate" runat="server" Text='<%# Eval("PayoutDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TotalBV" HeaderText="Total Business" />
                                    <asp:BoundField DataField="totalfund" HeaderText="Total Fund" />
                                    <asp:TemplateField HeaderText="View Detail" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                        ControlStyle-ForeColor="black">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBDelete" runat="server" Text="View Detail" OnClick="ViewDetail"
                                                ForeColor="Black" Font-Bold="true" Style="color: black !important; text-decoration: none;"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle Width="55px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div style="padding: 10px 10px 20px 10px; overflow: scroll" id="div1d" runat="server"
                            visible="false">
                            <asp:GridView ID="GridView1" Width="100%" runat="server" AllowPaging="false" GridLines="Both"
                                class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                EmptyDataText="No data to display." AutoGenerateColumns="true" AllowSorting="true"
                                OnSorting="GridView1_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="SNo" SortExpression="SNo">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%>.
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                        CssClass='<%# If(Convert.ToBoolean(Eval("Enabled")), "page_enabled", "page_disabled")%>'
                                        OnClick="Page_Changed" OnClientClick='<%# If(Not Convert.ToBoolean(Eval("Enabled")), "return false;", "") %>'
                                        ForeColor="Black"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlPageSize" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
</asp:Content>
