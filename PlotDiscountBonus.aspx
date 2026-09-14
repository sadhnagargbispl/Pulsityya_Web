<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="PlotDiscountBonus.aspx.vb" Inherits="PlotDiscountBonus" Title="" %>

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
                        <h2>Plot Achiever Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-3">
                            <asp:Label ID="Label2" runat="server" Text="Member ID"></asp:Label>
                            <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label>
                            <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblEndDate" runat="server" Text="End Date : "></asp:Label>
                            <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="Label1" runat="server" Text="Page Size:"></asp:Label>
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
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="false" GridLines="Both"
                            class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                            EmptyDataText="No data to display." AutoGenerateColumns="true" AllowSorting="true"
                            OnSorting="GvData_Sorting">
                        </asp:GridView>
                        <asp:Repeater ID="rptPager" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                    CssClass='<%# If(Convert.ToBoolean(Eval("Enabled")), "page_enabled", "page_disabled")%>'
                                    OnClick="Page_Changed" OnClientClick='<%# If(Not Convert.ToBoolean(Eval("Enabled")), "return false;", "") %>'
                                    ForeColor="Black"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
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
