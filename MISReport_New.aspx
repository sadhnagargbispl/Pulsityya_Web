<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MISReport_New.aspx.vb" Inherits="MISReport_New" Title="" %>

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
                            MIS Report
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
                                        <asp:CheckBox ID="CheckBox2" runat="server" Text="Session Wise :" Font-Bold="true"
                                            Checked="true"  Visible="false" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control" Style="text-indent: 1px;">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="BtnShow" runat="server" CssClass="btn btn-primary" Text="Show Detail" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Export To Excel"  />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <div style="padding: 10px 10px 20px 10px; overflow: scroll">
                                        <asp:GridView ID="GvActivation" Width="100%" runat="server" RowStyle-Height="25px"
                                            Caption="" CaptionAlign="Top" GridLines="Both" AllowPaging="true"
                                            CssClass="table table-bordered" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt" ShowHeader="true" PageSize="50" EmptyDataText="No data to display."
                                            AutoGenerateColumns="true">
                                        </asp:GridView>
                                       
                                    </div>
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
    
</asp:Content>
