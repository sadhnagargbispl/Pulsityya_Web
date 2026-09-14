<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="offersummeryNew.aspx.vb" Inherits="App_UI_Application_Pages_offersummeryNew"
    Title="" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .PagerStyle
        {
            background-image: url(../Images/td.jpg);
            background-position: center;
            background-repeat: repeat-x;
            background-color: #ffffff;
            font-weight: bold;
            text-align: center;
            width: 00px;
        }
        .PagerStyle table
        {
            text-align: center;
            margin: auto;
        }
        .PagerStyle table td
        {
            border: 0px;
            padding: 5px;
        }
        .PagerStyle td
        {
            border-top: #1d1d1d 3px solid;
        }
        .PagerStyle a
        {
            color: #000000;
            text-decoration: none;
            padding: 2px 10px 2px 10px;
            border-top: solid 1px #777777;
            border-right: solid 1px #333333;
            border-bottom: solid 1px #333333;
            border-left: solid 1px #777777;
        }
        .PagerStyle span
        {
            font-weight: bold;
            color: #000000;
            text-decoration: none;
            padding: 2px 10px 2px 10px;
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
                            Package Wise Offer
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div align="center">
                            <div class="col-md-12">
                                <div class="col-md-3">
                                    Select Offer
                                    <asp:DropDownList ID="ddloffer" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                         <div class="col-md-3" style=" display: none;">
                            From Date :
                       
                            <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                                ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-3" style=" display: none;">
                            To Date :
                        
                            <asp:TextBox ID="TxtToDate" runat="server" class="form-control"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                        </div> 
                                <div class="col-md-3">
                                    Page Size:
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed"
                                        class="form-control">
                                        <asp:ListItem Text="10" Value="10" />
                                        <asp:ListItem Text="20" Value="20" />
                                        <asp:ListItem Text="50" Value="50" />
                                        <asp:ListItem Text="100" Value="100" />
                                        <asp:ListItem Text="200" Value="200" />
                                        <asp:ListItem Text="300" Value="300" />
                                        <asp:ListItem Text="400" Value="400" />
                                        <asp:ListItem Text="500" Value="500" />
                                        <asp:ListItem Text="600" Value="600" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                </div>
                            </div>
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                <asp:Button ID="btnPrintCurrent" runat="server" class="btn btn-primary" Text="Print Current Page"
                                    Visible="false" />
                                <asp:Button ID="btnPrintAll" runat="server" class="btn btn-primary" Text="Print All Pages"
                                    Visible="false" />
                                <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                    color: Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <div style="margin-top: 20px; margin-bottom: 20px;">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                    Visible="false"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                    color: Gray"></asp:Label>
                            </div>
                            <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                margin-bottom: 25px;">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="false" class="table table-bordered" PagerStyle-CssClass="PagerStyle"
                                    AlternatingRowStyle-CssClass="alt" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                    PageSize="10" EmptyDataText="No data to display." AllowSorting="true" OnSorting="GvData_Sorting">
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <Columns>
                                        <asp:BoundField DataField="SNo" HeaderText="SNo." SortExpression="SNo" />
                                        <asp:BoundField DataField="IDno" HeaderText="ID No." SortExpression="IDno" />
                                        <asp:BoundField DataField="MemFirstName" HeaderText="Member Name" SortExpression="MemFirstName" />
                                        <asp:BoundField DataField="SelfBv" HeaderText="Offer Self BV" SortExpression="SelfBv" />
                                        <asp:BoundField DataField="DirectBV" HeaderText="Direct BV" SortExpression="DirectBV" />
                                        <asp:BoundField DataField="MatchingBV" HeaderText="Matching BV" SortExpression="MatchingBV" />
                                        <asp:BoundField DataField="Reward" HeaderText="Reward" SortExpression="Reward" />
                                        <asp:BoundField DataField="OfferId" HeaderText="Offer ID" SortExpression="OfferId" />
                                        <%--<asp:BoundField DataField="SelfBvA" HeaderText="Act. Self BV" SortExpression="SelfBvA" />--%>
                                        <%--<asp:TemplateField HeaderText="Offer Left Active" SortExpression="LeftActive">
                                            <ItemTemplate>
                                                <a style="color: Black;font-weight:bold;color:Blue;text-decoration:underline;" href='OfferDetails.aspx?id=<%# Eval("OfferID") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 480,height: 490,marginTop : 0 } )">
                                                    <%#Eval("LeftActive")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Act. Left Active" SortExpression="LeftActiveA">
                                            <ItemTemplate>
                                                <a style="color: Black;font-weight:bold;color:Blue;text-decoration:underline;" href='GetOfferMemberDetail.aspx?id=<%# Eval("OfferID") %>&idno=<%# Eval("idno") %>&leg=1' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 480,height: 490,marginTop : 0 } )">
                                                    <%#Eval("LeftActiveA")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Offer Right Active" SortExpression="RightActive">
                                            <ItemTemplate>
                                                <a style="color: Black;font-weight:bold;color:Blue;text-decoration:underline;" href='OfferDetails.aspx?id=<%# Eval("OfferID") %>'
                                                    onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 480,height: 490,marginTop : 0 } )">
                                                    <%#Eval("RightActive")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                         <asp:TemplateField HeaderText="Act. Right Active" SortExpression="RightActiveA">
                                            <ItemTemplate>
                                                <a style="color: Black;font-weight:bold;color:Blue;text-decoration:underline;" href='GetOfferMemberDetail.aspx?id=<%# Eval("OfferID") %>&idno=<%# Eval("idno") %>&leg=2'
                                                    onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 480,height: 490,marginTop : 0 } )">
                                                    <%#Eval("RightActiveA")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        --%>
                                        
                                        <%--<asp:BoundField DataField="RightActiveA" HeaderText="Act. Right Active" SortExpression="RightActiveA" />--%>
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
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlPageSize" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    </div> </div> </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
</asp:Content>
