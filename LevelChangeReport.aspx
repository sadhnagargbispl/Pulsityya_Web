<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="LevelChangeReport.aspx.vb" Inherits="LevelChangeReport" %>

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
                           Level Change Report</h2>
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
                                <div class="col-md-1">Choose Level:</div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="DDLLevel" runat="server"   class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">From Amount</div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="TxtFromAmount" runat="server" class="form-control" onkeypress="return isNumberKey(event);" ></asp:TextBox>
                                    </div>
                                     <div class="col-md-1">To Amount</div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="TxtToAmount" runat="server" class="form-control" onkeypress="return isNumberKey(event);" ></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        Page Size:</div>
                                    <div class="col-md-2">
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
                                </div>
                                 <div class="col-md-12"><br /></div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                       </div>
                                    <div class="col-md-9">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" ValidationGroup="Save" />
                                                                            <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                     
                                    </div>
                                </div>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                     <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label><asp:Label ID="lblCount"
                                            runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                <div id="gvContainer" runat="server" >
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="50" EmptyDataText="No data to display." AllowSorting ="true" OnSorting="GvData_Sorting">
                                        <Columns>
                                            <%--<asp:TemplateField HeaderText="S.No." HeaderStyle-Width="35px">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:BoundField DataField ="SNo" HeaderText="SNo" SortExpression ="SNo" />
                                            <asp:TemplateField HeaderText="GrpID" Visible="false">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IdNo" HeaderText="Id No." ControlStyle-Width="50px" SortExpression="IdNo" />
                                            <asp:BoundField DataField="MemberName" HeaderText="Member Name" SortExpression ="MemberName" />
                                            <asp:BoundField DataField ="MobileNo" HeaderText="MobileNo" SortExpression ="Mobileno" />
                                            <asp:BoundField DataField ="TotalCommission" HeaderText="Total Commission"  SortExpression ="TotalCommission"/>
                                             
                                           
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
                                        <asp:PostBackTrigger ControlID="btnExport" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
