<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="DownlineVadic.aspx.vb" Inherits="DownlineVadic" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .right-mid .welcome
        {
            height: 23px;
            border-top: 1px solid #203454;
            border-bottom: 1px solid #203454;
            background-color: #395e96;
            color: #e4efff;
            font-weight: bold;
            margin: 0px 0px 5px 0px;
        }
        .right-mid .welcome span
        {
            padding: 3px 0px 0px 15px;
            display: block;
        }
        .box-body1-clear
        {
            width: 100%;
            background-image: url(../Resources/images/add-new-btn-bg-hover.jpg);
            font-family: Verdana,Arial;
            color: black;
            font-weight: bold;
            border-bottom: 1px solid;
        }
        .Table-lblNew
        {
            border: solid 1px;
        }
        .box-lbl
        {
            font-family: Verdana,Arial;
            color: Black;
            font-size: 11px;
            font-weight: bold;
            border: solid 1px #D0D0D0;
        }
        .box-lblNew
        {
            font-family: Verdana,Arial;
            color: Black;
            font-size: 10px;
            font-weight: bold;
            border: solid 1px #D0D0D0;
        }
        p
        {
            font-weight: bold;
            color: #666666;
            margin: 0px;
            line-height: 25px;
            width: 800px;
            padding-bottom: 8px;
            padding-top: 5px;
            text-align: left;
            padding-left: 10px;
        }
        .label_Err
        {
            padding-left: 10px;
            font-size: 11px;
            text-transform: capitalize;
            color: red;
            font-family: Verdana, Monospace;
            font-weight: bold;
        }
        .GridViewStyle
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            font-size: 10.5px;
            border-collapse: collapse;
            width: 500px;
        }
        .HeaderStyle, .PagerStyle
        {
            background-image: url(../Images/td.jpg);
            background-position: center;
            background-repeat: repeat-x;
            background-color: #ffffff;
            font-weight: bold;
            text-align: center;
            width: 00px;
        }
        .HeaderStyle a
        {
            text-decoration: none;
            color: #000000;
            display: block;
            text-align: center;
            font-weight: normal;
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
        .RowStyle td, .AltRowStyle td, .SelectedRowStyle td, .EditRowStyle td /*Common Styles*/
        {
            padding: 5px;
            border-right: solid 1px #1d1d1d;
            text-decoration: none;
            color: #000000;
            text-transform: capitalize;
            text-align: left;
        }
        .RowStyle td
        {
            background-color: transparent;
            background-image: url(../Images/rdc_hd.jpg);
        }
        .AltRowStyle td
        {
            background-color: #E4EEDB;
            background-image: url(../Images/rdc_hd1.jpg);
        }
        .right-mid .form-heading
        {
            height: 24px;
            border-top: 1px solid #5c0871;
            border-bottom: 1px solid #4671b4;
            font: bold 12px Tahoma, Geneva, sans-serif;
            color: #ffffff;
            text-shadow: #445f8a 1px 2px 0px;
            text-align: center;
        }
    </style>

    <script type="text/javascript">
        function showMe() {
            var ids = ['DivSideA', 'DivSideB'];
            var inp = document.getElementById('myform').getElementsByTagName('input'), el, i = 0, k = 0;
            while (el = inp[i++]) {
                if (el.name == 'mype' || el.name == 'modtype') {
                    document.getElementById(ids[k]).style.display = el.checked ? 'block' : 'none';
                    k++;
                }
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Member Downline Report</h2>
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
                                    <div class="col-md-2">
                                        Enter Member ID :</div>
                                    <div class="col-md-3">
                                        <asp:TextBox class="form-control" ID="txtMemberId" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package :" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RadioButtonList AutoPostBack="True" ID="rbleg" RepeatDirection="Horizontal"
                                            runat="server" RepeatColumns="3">
                                            <asp:ListItem Selected="True" Value="0" Text="Both"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Left"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Right"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                    <div class="col-md-3">
                                        <br />
                                        <asp:RadioButtonList ID="RbtSearch" runat="server" AutoPostBack="true" RepeatColumns="2"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Date wise" Value="D" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Session Wise" Value="S"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-3">
                                        <br />
                                        <asp:DropDownList ID="DDlDate" runat="server" class="form-control" AutoPostBack="true">
                                            <asp:ListItem Text="Joining" Value="J"></asp:ListItem>
                                            <asp:ListItem Text="Activation" Value="A"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="DDLSession" runat="server" class="form-control" Visible="false">
                                        </asp:DropDownList>
                                        <asp:Label ID="LblSDate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblTDate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblSTDate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblToDate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
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
                                        <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <asp:Button ID="btnShowDownline" class="btn btn-primary" runat="server" Text="Show Downline Report"
                                        ValidationGroup="Save" />
                                </div>
                                <div class="col-md-9">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>
                            </div>
                            <div id="divMemDownline" runat="server" visible="false" style="overflow: auto; width: 95%;
                                margin-left: 15px; height: 500px; margin-bottom: 20px;">
                                <center>
                                    <div align="center">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <table class="Table-lblNew" cellspacing="2" cellpadding="2" width="900">
                                                        <tbody>
                                                            <tr valign="top">
                                                                <td class="box-body1-clear" align="center" colspan="8">
                                                                    Status
                                                                </td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td class="box-body1-clear" align="center" colspan="5" style="width: 40%;">
                                                                    Member Status
                                                                </td>
                                                                <td class="box-body1-clear" align="center" style="width: 20%; border: 1px solid black;
                                                                    border-bottom: 0">
                                                                </td>
                                                                <td class="box-body1-clear" align="center" colspan="2" style="width: 40%">
                                                                    Carry Forward
                                                                </td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td align="center" style="border: 1px solid black; border-bottom: 0">
                                                                    Left Join PV
                                                                    <br />
                                                                </td>
                                                                <td align="center" style="border: 1px solid black; border-bottom: 0">
                                                                    Right Join PV
                                                                    <br />
                                                                </td>
                                                                <td align="center" style="border: 1px solid black; border-bottom: 0">
                                                                    Left Repur. PV
                                                                    <br />
                                                                </td>
                                                                <td align="center" style="border: 1px solid black; border-bottom: 0">
                                                                    Right Repur. PV
                                                                    <br />
                                                                </td>
                                                                <td align="center" style="border: 1px solid black; border-bottom: 0">
                                                                    Self PV
                                                                    <br />
                                                                </td>
                                                                <td align="center" style="border: 1px solid black; border-bottom: 0">
                                                                    Matching PV
                                                                    <br />
                                                                </td>
                                                                <td align="center" style="border: 1px solid black; border-bottom: 0">
                                                                    Left
                                                                    <br />
                                                                </td>
                                                                <td align="center" style="border: 1px solid black; border-bottom: 0">
                                                                    Right
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td align="center" style="padding: 10px; border: 1px solid black; border-bottom: 0;
                                                                    border-top: 0">
                                                                    <span id="LblLeftBV" runat="server">0</span>
                                                                </td>
                                                                <td align="center" style="padding: 10px; border: 1px solid black; border-bottom: 0;
                                                                    border-top: 0">
                                                                    <span id="LblRightBV" runat="server">0</span>
                                                                </td>
                                                                <td align="center" style="padding: 10px; border: 1px solid black; border-bottom: 0;
                                                                    border-top: 0">
                                                                    <span id="LblLeftRBV" runat="server">0</span>
                                                                </td>
                                                                <td align="center" style="padding: 10px; border: 1px solid black; border-bottom: 0;
                                                                    border-top: 0">
                                                                    <span id="LblRightRBV" runat="server">0</span>
                                                                </td>
                                                                
                                                                 <td align="center" style="padding: 10px; border: 1px solid black; border-bottom: 0;
                                                                    border-top: 0">
                                                                    <span id="LblSelfBv" runat="server">0</span>
                                                                </td>
                                                                <td align="center" style="padding: 10px; border: 1px solid black; border-bottom: 0;
                                                                    border-top: 0">
                                                                    <span id="LblMatchingBv" runat="server">0</span>
                                                                </td>
                                                                <td align="center" style="padding: 10px; border: 1px solid black; border-bottom: 0;
                                                                    border-top: 0">
                                                                    <span id="LblCarryA" runat="server">0</span>
                                                                </td>
                                                                <td align="center" style="padding: 10px; border: 1px solid black; border-bottom: 0;
                                                                    border-top: 0">
                                                                    <span id="LblCarryB" runat="server">0</span>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                                <br />
                                <br />
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr style="width: 700px" id="TrLeftHeading" runat="server">
                                            <td class="form-heading" colspan="4" style="width: 700px">
                                                <h4>
                                                    Downline Left</h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Leftactive" Font-Bold="true" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LeftDeactive" Font-Bold="true" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="tr-1">
                                            <td align="right" valign="middle" colspan="4">
                                                <div id="DivSideA" runat="server">
                                                    <table border="0" width="100%" cellspacing="2" cellpadding="0">
                                                        <tr>
                                                            <td align="center" valign="top">
                                                                <div id="gvContainer" runat="server" style="overflow: auto; width: 100%; margin-top: 10px;
                                                                    margin-bottom: 10px;">
                                                                    <asp:DataGrid ID="GrdDirects1" runat="server" class="table table-bordered" RowStyle-Height="25px"
                                                                        CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="False" AllowPaging="True"
                                                                        Width="100%" ShowHeader="true" PageSize="5" EmptyDataText="No data to display."
                                                                        GridLines="Both" HeaderStyle-CssClass="bg-primary">
                                                                        <Columns>
                                                                            <asp:TemplateColumn>
                                                                                <ItemTemplate>
                                                                                    <%#Container.DataSetIndex + 1%>.
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:BoundColumn DataField="IDNO" HeaderText="ID No">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="MemName" HeaderText="Member Name">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <%--  <asp:BoundColumn DataField="Sponsor" HeaderText="Upliner ID" >
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                  <asp:BoundColumn DataField="SponsorName" HeaderText="Upliner Name" >
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>--%>
                                                                            <asp:BoundColumn DataField="RefFormno" HeaderText="Sponsor ID">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="referalName" HeaderText="Sponsor Name">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="DateofJoining" HeaderText="Date Of Joining">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Topup" HeaderText="Topup Date">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="KitName" HeaderText="Package">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="KitAmount" HeaderText="Pkg. MRP"></asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="BV" HeaderText="Unit"></asp:BoundColumn>
                                                                        </Columns>
                                                                        <PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>
                                                                        <%--   <ItemStyle CssClass="RowStyle" HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />--%>
                                                                    </asp:DataGrid>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <asp:Button ID="BtnExportA" runat="server" Text="Export to Excel" class="btn btn-primary"
                                                                    Style="margin-left: 400px;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px;" colspan="4">
                                            </td>
                                        </tr>
                                        <tr id="trRightHeading" runat="server">
                                            <td class="form-heading" colspan="4" style="width: 700px">
                                                <h4>
                                                    Downline Right</h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="RActive" Font-Bold="true" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="RDeactive" Font-Bold="true" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </tr>
                                        <tr class="tr-1">
                                            <td align="right" valign="middle" colspan="4">
                                                <div id="DivSideB" runat="server">
                                                    <table border="0" width="100%" cellspacing="2" cellpadding="0">
                                                        <tr>
                                                            <td align="center" valign="top">
                                                                <div id="Div1" runat="server" style="overflow: auto; width: 100%; margin-top: 10px;
                                                                    margin-bottom: 10px;">
                                                                    <asp:DataGrid ID="GrdDirects2" runat="server" class="table table-bordered" RowStyle-Height="25px"
                                                                        CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="False" AllowPaging="True"
                                                                        Width="100%" ShowHeader="true" PageSize="5" EmptyDataText="No data to display."
                                                                        GridLines="Both" HeaderStyle-CssClass="bg-primary">
                                                                        <Columns>
                                                                            <asp:TemplateColumn>
                                                                                <ItemTemplate>
                                                                                    <%#Container.DataSetIndex + 1%>.
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:BoundColumn DataField="IDNO" HeaderText="ID No">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="MemName" HeaderText="Member Name">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <%-- <asp:BoundColumn DataField="Sponsor" HeaderText="Upliner ID" >
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                  <asp:BoundColumn DataField="SponsorName" HeaderText="Upliner Name" >
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>--%>
                                                                            <asp:BoundColumn DataField="RefFormno" HeaderText="Sponsor ID">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="referalName" HeaderText="Sponsor Name">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="DateofJoining" HeaderText="Date Of Joining">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Topup" HeaderText="Topup Date">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="KitName" HeaderText="Package">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="KitAmount" HeaderText="Pkg. MRP"></asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="BV" HeaderText="Unit"></asp:BoundColumn>
                                                                        </Columns>
                                                                        <PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>
                                                                        <%--      <ItemStyle CssClass="RowStyle" HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />--%>
                                                                    </asp:DataGrid>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <asp:Button ID="BtnExportB" runat="server" Text="Export to Excel" class="btn btn-primary"
                                                                    Style="margin-left: 400px;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
</asp:Content>
