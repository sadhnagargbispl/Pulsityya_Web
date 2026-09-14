<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="Enable_disable.aspx.vb" Inherits="Enable_disable" %>

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
                           OnLine Withdrawal On/Off</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                              <%--  <div class="col-md-12">
                                    <div class="col-md-3">
                                        <asp:CheckBox ID="ChkMem" runat="server" Text="Member ID Wise :" Font-Bold="true" /></div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>--%>
                                <div class="col-md-12">
                                    <br />
                                    <div class="col-md-3" style="display:none">
                                        Approve Status:</div>
                                    <div class="col-md-3" style="display:none">
                                        <asp:DropDownList ID="ddlstate" runat="server" class="form-control">
                                                </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Button runat="server" ID="BtnSearch" class="btn btn-primary" Text="Search" visible=false/>
                                    </div>
                                    <%--<div class="col-md-2">
                                        <asp:Button runat="server" ID="BtnExport" class="btn btn-primary" Text="Export To Excel"
                                            Enabled="false" /></div>--%>
                                    <div class="col-md-1">
                                        <asp:Button ID="BtnVerifiy" runat="server" Text="Enable" class="btn btn-primary"
                                            Enabled="true" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="BtnUnVerify" runat="server" Text="Disable" class="btn btn-primary" />
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
