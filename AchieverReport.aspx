<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="AchieverReport.aspx.vb" Inherits="AchieverReport" Title="" EnableEventValidation="False" %>

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
                            Level Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div>
                            <div align="center">
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="CheckBox2" runat="server" Text="Session Wise :" Font-Bold="true"
                                            Checked="true" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlSession" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        Level
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="DDlLevel" runat="server" class="form-control">
                                        </asp:DropDownList>
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
                                <div class="col-md-12">
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                     
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                        <%-- <asp:Button ID="btnshowall" runat="server" class="btn btn-primary" Text="Show All" />--%>
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                        <%-- <asp:Button ID="BtnExportCsv" runat="server" CssClass ="Btn" Text="Export To CSV" />--%>
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                            Enabled="false" Visible="false" />
                                    </div>
                                </div>
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                <div id="gvContainer" runat="server" style="overflow: scroll;" class="col-md-12">
                                   
                                   <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                            color: Gray"></asp:Label>
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" AllowSorting="true" OnSorting="GvData_Sorting" PageSize="25"
                                        EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:BoundField DataField="SNo" HeaderText="SNo" SortExpression="SNo" />
                                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                            <asp:BoundField DataField="Level1Fund" HeaderText="Level1Fund" SortExpression="Level1Fund" />
                                            <asp:BoundField DataField="Level1Ach" HeaderText="Level1Ach" SortExpression="Level1Ach" />
                                            <asp:BoundField DataField="Level1Rate" HeaderText="Level1Rate" SortExpression="Level1Rate" />
                                            <asp:BoundField DataField="Level2Fund" HeaderText="Level2Fund" SortExpression="Level2Fund" />
                                            <asp:BoundField DataField="Level2Ach" HeaderText="Level2Ach" SortExpression="Level2Ach" />
                                            <asp:BoundField DataField="Level2Rate" HeaderText="Level2Rate" SortExpression="Level2Rate" />
                                            <asp:BoundField DataField="Level3Fund" HeaderText="Level3Fund" SortExpression="Level3Fund" />
                                            <asp:BoundField DataField="Level3Ach" HeaderText="Level3Ach" SortExpression="Level3Ach" />
                                            <asp:BoundField DataField="Level3Rate" HeaderText="Level3Rate" SortExpression="Level3Rate" />
                                            <asp:BoundField DataField="Level4Fund" HeaderText="Level4Fund" SortExpression="Level4Fund" />
                                            <asp:BoundField DataField="Level4Ach" HeaderText="Level4Ach" SortExpression="Level4Ach" />
                                            <asp:BoundField DataField="Level4Rate" HeaderText="Level4Rate" SortExpression="Level4Rate" />
                                            <asp:BoundField DataField="Level5Fund" HeaderText="Level5Fund" SortExpression="Level5Fund" />
                                            <asp:BoundField DataField="Level5Ach" HeaderText="Level5Ach" SortExpression="Level5Ach" />
                                            <asp:BoundField DataField="Level5Rate" HeaderText="Level5Rate" SortExpression="Level5Rate" />
                                            <asp:BoundField DataField="TotalFund" HeaderText="TotalFund" SortExpression="TotalFund" />
                                            <asp:BoundField DataField="Distribution" HeaderText="Distribution" SortExpression="Distribution" />
                                            <asp:BoundField DataField="RemainFund" HeaderText="RemainFund" SortExpression="RemainFund" />
                                            <asp:BoundField DataField="TotalActive" HeaderText="TotalActive" SortExpression="TotalActive" />
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
        </div>
    </div>
    <br />
    <br />
</asp:Content>
