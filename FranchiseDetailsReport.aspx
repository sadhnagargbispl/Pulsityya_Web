<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="FranchiseDetailsReport.aspx.vb" Inherits="App_UI_Application_Pages_FranchiseDetailsReport"
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
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/start/jquery-ui.css" />
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
<script type="text/javascript">
    $(function () {
        $("#dialog").dialog({
            autoOpen: false,
            modal: true,
            height: 600,
            width: 600,
            title: "Zoomed Image"
        });
        $("[id*=GvData] img").click(function () {
            $('#dialog').html('');
            $('#dialog').append($(this).clone());
            $('#dialog').dialog('open');
        });
    });
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
                        <h2>
                            Franchise Details Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div style="background-color: White">
                            <div class="col-md-12">
                                <div class="col-md-2">
                                    <asp:CheckBox ID="ChkMember" runat="server" Text="Member Id:" /></div>
                                <div class="col-md-2">
                                    <asp:TextBox class="form-control" ID="txtMemberId" runat="server"></asp:TextBox></div>
                                <div class="col-md-2">
                                    State Name:</div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="RbtWalletType" runat="server" AutoPostBack="true" CssClass="form-control"
                                        Style="width: 200px">
                                    </asp:DropDownList>
                                    <%--<asp:RadioButtonList ID="RbtWalletType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow">
                                        <asp:ListItem Text="M Wallet" Value="M" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Shopping Wallet" Value="R"></asp:ListItem>
                                    </asp:RadioButtonList>--%>
                                </div>
                                <div class="col-md-3">
                                </div>
                            </div>
                            <div class="col-md-12" style="padding: 10px">
                                <div class="col-md-2">
                                    <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label></div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                        ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblEndDate" runat="server" Text=" End Date : "></asp:Label></div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                        Format="dd-MMM-yyyy">
                                    </AjaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                        ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-1">
                                    Page Size</div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed"
                                        class="form-control">
                                        <asp:ListItem Text="10" Value="10" />
                                        <asp:ListItem Text="20" Value="20" />
                                        <asp:ListItem Text="50" Value="50" />
                                        <asp:ListItem Text="100" Value="100" />
                                        <asp:ListItem Text="500" Value="500" />
                                        <asp:ListItem Text="1000" Value="1000" />
                                        <asp:ListItem Text="2000" Value="2000" />
                                        <asp:ListItem Text="5000" Value="5000" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12" style="padding: 5px">
                                <div class="col-md-2">
                                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" ValidationGroup="Save" /></div>
                                <%-- <asp:Button ID="btnshowall" runat="server" CssClass="Btn" Text="Show All" />--%>
                                <div class="col-md-2">
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                                <%--   <asp:Button ID="BtnExportCsv" runat="server" CssClass ="Btn" Text="Export To CSV" />--%>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                        Visible="false" /></div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                        Visible="false" /></div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label></div>
                                <div class="col-md-2">
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <div style="margin-top: 20px; margin-bottom: 20px;" >
                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray; margin-right: 10px"></asp:Label>
                                    <br />
                                    <asp:Label ID="LblCredit" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray; margin-right: 10px"></asp:Label>
                                    <asp:Label ID="lblDebit" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray; margin-right: 10px"></asp:Label>
                                    <asp:Label ID="LblBalance" runat="server" Style="font-weight: bold; font-size: 14px;
                                        color: Gray; margin-right: 10px"></asp:Label>
                                </div>
                                <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                    margin-bottom: 25px;">
                                    <%--<asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        PagerStyle-CssClass="PagerStyle" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                        PageSize="10" EmptyDataText="No data to display." AllowSorting="true" OnSorting="GvData_Sorting" >--%>
                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        PagerStyle-CssClass="PagerStyle" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                        PageSize="10" EmptyDataText="No data to display." AllowSorting="true" OnSorting="GvData_Sorting" >
                                         <Columns>
                                         <%--<asp:TemplateField HeaderText="Sno." >
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSno" runat="server" Text='<%# Eval("Sno") %>'></asp:Label>
                                                        </ItemTemplate> 
                                                        </asp:TemplateField> --%>
                                         <asp:BoundField DataField="Id" HeaderText="ID" Visible ="false" />
                                         <asp:BoundField DataField="Sno" HeaderText="Sno"></asp:BoundField>
                                            <%-- <asp:TemplateField HeaderText="IDNo">
                                                 <ItemTemplate>
                                                     <asp:Label ID="LblIDNo" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label>
                                                 </ItemTemplate>
                                             </asp:TemplateField>--%>
                                             <asp:BoundField DataField="IDNo" HeaderText="IDNo"></asp:BoundField>
                                         <asp:BoundField DataField="RequestFor" HeaderText="RequestFor" ></asp:BoundField>
                                        <asp:BoundField DataField="PartyName" HeaderText="Party Name" ></asp:BoundField>
                                        <asp:BoundField DataField="Address" HeaderText="Address for Franchise" ></asp:BoundField>
                                        <asp:BoundField DataField="City" HeaderText="City" ></asp:BoundField>
                                        <asp:BoundField DataField="StateName" HeaderText="Statename" ></asp:BoundField>
                                        <asp:BoundField DataField="Tahsil" HeaderText="Tahsil" ></asp:BoundField>
                                        <asp:BoundField DataField="Pincode" HeaderText="Pincode" ></asp:BoundField>
                                        <asp:BoundField DataField="Mobileno" HeaderText="MobileNo" />
                                        <%--<asp:BoundField DataField="Mobileno" HeaderText="MobileNo" ></asp:BoundField>--%>
                                        <asp:BoundField DataField="GstNo" HeaderText="GSTNO" />
                                        <asp:BoundField DataField="EmailId" HeaderText="EMailID" />
                                        <asp:BoundField DataField="SponsorId" HeaderText="SponsorId" />
                                         <asp:BoundField DataField="SName" HeaderText="SponsorName" />
                                        <asp:BoundField DataField="NomineeName" HeaderText="Nominee Name" />
                                         <asp:BoundField DataField="Education" HeaderText="Education" />
                                         <asp:BoundField DataField="CurrentJob" HeaderText="Current Job" />
                                         <asp:BoundField DataField="Workexprience" HeaderText="Work Exprience" />
                                         <asp:BoundField DataField="InvestmentCapacity" HeaderText="Investment Capacity" />
                                         <asp:BoundField DataField="location" HeaderText="Location" />
                                         <asp:TemplateField HeaderText="Image">
                                                   <ItemTemplate>
                                                        <a href='<%# "Img.aspx?&type=TransImg&ID="& Eval("ID") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 785,height: 580,marginTop : 0 } )">
                                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>' Height="80px"
                                                                Width="80px" />
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Franchise Date" HeaderText="Franchise Date" />
                                                <asp:BoundField DataField="ActiveStatus" HeaderText="Active Status" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" />                                               
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
    </div>
    <br />
    <br />
</asp:Content>
