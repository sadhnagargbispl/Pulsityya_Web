<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="DailyIncentiveDetailReportNew.aspx.vb" Inherits="DailyIncentiveDetailReportNew" %>

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
                            Daily Incentive Detail
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
                                    <div class="col-md-2">
                                        Member ID Wise :
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Start Date :
                                        <asp:DropDownList ID="DDlFromDate" runat="server" class="form-control" Style="display: inline">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        End Date :
                                        <asp:DropDownList ID="DDltodate" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2" id="SelectTypeDiv" runat="server" visible="false">
                                        Search Type :
                                        <asp:DropDownList ID="ddlsearchtype" CssClass="form-control" runat="server" AutoPostBack ="true" >
                                            <asp:ListItem Value="1" Text="Self"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Team"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Page Size :
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
                                            <asp:ListItem Text="1000" Value="1000" />
                                            <asp:ListItem Text="2000" Value="2000" />
                                        </asp:DropDownList>
                                    </div>
                                     <div class="col-md-2" id="LegWiyDiv" runat="server" visible="false">
                                        Leg Wise :
                                        <asp:DropDownList ID="DDlLagno" CssClass="form-control" runat="server">
                                            <asp:ListItem Value="0" Text="Both"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Left"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Right"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-12">
                                        <br />
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-12">
                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px;
                                                    color: Gray"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>
                                            <div style="padding: 10px 10px 20px 10px">
                                                <asp:GridView ID="GrdTotal" Width="100%" runat="server" AllowPaging="false" PageSize="200"
                                                    GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                    ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="true"
                                                    AllowSorting="true" OnSorting="GrdTotal_Sorting">
                                                </asp:GridView>
                                                <asp:GridView ID="GrdTotaleco" Width="100%" runat="server" AllowPaging="false" PageSize="200"
                                                    GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                    ShowHeader="true" EmptyDataText="No data to display." AutoGenerateColumns="false"
                                                    AllowSorting="true" OnSorting="GrdTotaleco_Sorting">
                                                    <Columns>
                                                        <asp:BoundField DataField="SNo" HeaderText="SNo." SortExpression="SNo" />
                                                        <asp:TemplateField HeaderText="Payout Date" SortExpression="FromDate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="StartDate" runat="server" Text='<%# Eval("Payout Date") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       
                                                        <asp:TemplateField HeaderText="Member ID" SortExpression="IdNo">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblMemberId" runat="server" Text='<%# Eval("Member Id") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Member Name" SortExpression="Mem_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblMemberName" runat="server" Text='<%# Eval("Member Name") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mobile No." SortExpression="SelfIncome">
                                                            <ItemTemplate>
                                                            
                                                                <asp:Label ID="lblmobileno" runat="server" Text='<%# Eval("Mobile No") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Initial Signup Benefit" SortExpression="SelfIncome">
                                                            <ItemTemplate>
                                                            <a href='<%# "ViewsignupTeam.aspx?Idno="& Eval("Member Id")&"&SessId="& Eval("SessId")  %>'
                                                                    onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 620,height: 450,marginTop : 0 } )">
                                                                    <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text='<%# Eval("Initial Signup Benefit") %>'></asp:Label></a><br />
                                                              
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       
                                                       <asp:TemplateField HeaderText="Team Income Benefit" SortExpression="SelfIncome">
                                                            <ItemTemplate>
                                                            
                                                                <asp:Label ID="lblteamIncome" runat="server" Text='<%# Eval("Team Income Benefit") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                             <asp:TemplateField HeaderText="Repurchase Level Income" SortExpression="SelfIncome">
                                                            <ItemTemplate>
                                                            
                                                                <asp:Label ID="lblrepurchIncome" runat="server" Text='<%# Eval("Repurchase Level Income") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                       
                                                        
                                                         <asp:TemplateField HeaderText="TDS Amount" SortExpression="SelfIncome">
                                                            <ItemTemplate>
                                                            
                                                                <asp:Label ID="lblSelfIncome" runat="server" Text='<%# Eval("TDS Amount") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Gross Income" SortExpression="SelfIncome">
                                                            <ItemTemplate>
                                                            
                                                                <asp:Label ID="lblgrossIncome" runat="server" Text='<%# Eval("Gross Income") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Net Income" SortExpression="SelfIncome">
                                                            <ItemTemplate>
                                                            
                                                                <asp:Label ID="lblSelfIncome" runat="server" Text='<%# Eval("Net Income") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>                                                           
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
