<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="UploadKYCReport_New.aspx.vb" Inherits="App_UI_Application_Pages_UploadKYCReport_New"
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
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            KYC Report
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
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:CheckBox ID="ChkMember" runat="server" Text="Choose MemberId :" TextAlign="Left" />
                                                <asp:TextBox ID="TxtMember" runat="server" Class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <b>
                                                    <asp:Label ID="Label2" runat="server" Text="Search By"></asp:Label></b>
                                                <asp:RadioButtonList ID="DDlSerchBy" Class="form-control" runat="server" RepeatColumns="4"
                                                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <asp:ListItem Text="Verify " Value="Y" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Rejected " Value="R"></asp:ListItem>
                                                    <asp:ListItem Text="Pending " Value="P"></asp:ListItem>
                                                    <asp:ListItem Text="Not Uploaded " Value="N"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b>
                                                    <asp:Label ID="Label4" runat="server" Text="Search"></asp:Label></b>
                                                <br />
                                                <asp:DropDownList ID="ddlKyc" runat="server" Class="form-control">
                                             <%--   <asp:ListItem Text="--All--" Value="4">--All--</asp:ListItem>--%>
                                                <asp:ListItem Text="Address Proof" Value="2">Address Proof</asp:ListItem>
                                                <asp:ListItem Text="Bank Detail" Value="3">Bank Detail</asp:ListItem>
                                                <asp:ListItem Text="Pan Card" Value="1">Pan Card</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-2" style="display:none" >
                                            <div class="form-group">
                                                <b>
                                                    <asp:Label ID="Label3" runat="server" Text="Search Type"></asp:Label></b>
                                                <br />
                                                <asp:RadioButtonList ID="RbtSummary" runat="server" Class="form-control" RepeatColumns="2"
                                                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <asp:ListItem Text="Summary" Value="S" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Detail" Value="D"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding: 5px">
                                    <div class="col-md-1">
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" /></div>
                                    <div class="col-md-1">
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" /></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                            color: Red"></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                            </div>
                            <div style="margin-top: 20px; margin-bottom: 20px;">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                    Visible="false"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px;
                                    color: Gray"></asp:Label>
                            </div>
                            <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px;
                                margin-bottom: 25px;">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" PagerStyle-CssClass="PagerStyle"
                                    AlternatingRowStyle-CssClass="alt" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                    PageSize="10" EmptyDataText="No data to display.">
                                    <PagerSettings Mode="NumericFirstLast" />
                                </asp:GridView>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" PagerStyle-CssClass="PagerStyle"
                                    AlternatingRowStyle-CssClass="alt" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                    PageSize="10" EmptyDataText="No data to display.">
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <Columns>
                                        <asp:BoundField DataField="Idno" HeaderText="Idno" />
                                        <asp:BoundField DataField="Name" HeaderText="Name" />
                                        <asp:BoundField DataField="Type" HeaderText="Type" />
                                        <asp:TemplateField HeaderText="Image">
                                            <ItemTemplate>
                                                <img src='<%# Eval("Imgpath") %>' width="100" height="100" /></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Image">
                                            <ItemTemplate>
                                                <img src='<%# Eval("Imgpath1") %>' width="100" height="100" /></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:BoundField DataField="RejectReason" HeaderText="Reject Reason" />
                                        <asp:BoundField DataField="RejectRemark" HeaderText="Reject Remark" />
                                        <asp:BoundField DataField="Date" HeaderText="Date" />
                                    </Columns>
                                </asp:GridView>
                            </div>
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
