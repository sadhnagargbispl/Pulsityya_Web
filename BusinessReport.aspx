<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="BusinessReport.aspx.vb" Inherits="BusinessReport" Title="" %>

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
                            Business Detail
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
                                    <div class="col-md-3">
                                        Member ID :
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox></div>
                                    <div class="col-md-4">
                                        Session Wise:
                                        <asp:DropDownList ID="DDlFromDate" runat="server" class="form-control" Style="display: inline">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <br />
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" />
                                    </div>
                                    <div class="col-md-2">
                                        <br />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" /></div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                </div>
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
                                                AllowSorting="true">
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
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
    </div> </div>
</asp:Content>
