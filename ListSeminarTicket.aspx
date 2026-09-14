<%@ Page Title="" Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="ListSeminarTicket.aspx.vb" Inherits="ListSeminarTicket" %>

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
                            List Seminar Ticket</h2>
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
                                    <div class=" col-md-12" style="padding: 2%; padding-right: 25%">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblText" runat="server" Text="Select Group :" Font-Bold="True"></asp:Label>&nbsp;&nbsp;</div>
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ddlProgram" runat="server" class="form-control" >
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" visible="false">
                              
                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true" PageSize="10" EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="LblUserID" runat="server" Text='<%# Container.DataItemIndex  +1%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="GroupName" HeaderText="Group Name" ControlStyle-Width="50px" />--%>
                                        <asp:BoundField DataField="Program" HeaderText="Program Name" />
                                        <asp:BoundField DataField="Date" HeaderText="Date" />
                                        <asp:BoundField DataField="Time" HeaderText="Time" />
                                        <asp:BoundField DataField="StateName" HeaderText="StateName" />
                                        <asp:BoundField DataField="cityName" HeaderText="cityName" />
                                        <asp:BoundField DataField="TicketNo" HeaderText="TicketNo" />
                                        <asp:BoundField DataField="ScratchNo" HeaderText="ScratchNo" />
                                        
                                         <asp:BoundField DataField="Idno" HeaderText="IDNo" />
                                        <asp:BoundField DataField="Name" HeaderText="Member Name" />
                                        <asp:BoundField DataField="SoldDAte" HeaderText="Sold Date" />
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- end of weather widget -->
        </div>
    </div>
</asp:Content>
