<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="AssignRankAllotReport.aspx.vb" Inherits="AssignRankAllotReport" Title="" %>

<asp:Content ID="Content2DT831731" ContentPlaceHolderID="head" runat="Server">
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
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                            Assign Rank Report
                        </h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <div class="table-responsive makeitresponsivegrid">
                                <div class="col-md-2">
                                    Member ID
                                    <asp:TextBox ID="txtMemberID" runat="server" class="form-control">
                                    </asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    Rank
                                    <asp:DropDownList ID="ddlRank" runat="server" class="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <br />
                                    <asp:Button ID="btnShow" runat="server" Text="Show" class="btn btn-primary" />
                                    <a href="AssignRankAllot.aspx" class="btn btn-primary">Assign Rank</a>
                                </div>
                                <div align="center">
                                    <div class="col-md-12">
                                        <br />
                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                            GridLines="None" AllowPaging="True" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" PageSize="20" EmptyDataText="No data to display." Width="100%"
                                            PagerStyle-CssClass="PagerStyle">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
    </div>
</asp:Content>
