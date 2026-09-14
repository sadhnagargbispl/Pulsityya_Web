<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="MyReferDirects.aspx.vb" Inherits="MyReferDirects"
    Title="" EnableEventValidation="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
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
    <style type="text/css">
        #doublescroll
        {
            overflow: auto;
            overflow-y: hidden;
        }
        #doublescroll p
        {
            margin: 0;
            padding: 1em;
            white-space: nowrap;
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
                            Level Wise Direct Report</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="clr">
                            <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                        </div>
                        <div class="col-md-12">
                            <table class="table-responsive">
                                <div class="col-md-3">
                                    <%-- <asp:CheckBox ID="ChkMem" runat="server"  Text="MemberId :" TextAlign="Left" />--%>
                                    MemberId :
                                    <asp:TextBox ID="txtMember" runat="server" AutoPostBack="true" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMember"
                                        runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px;
                                        color: Red"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    Search By
                                    <asp:DropDownList ID="rbtnsearch" AutoPostBack="true" runat="server" class="form-control">
                                        <asp:ListItem Text="Level Wise" Selected="True" Value="L"></asp:ListItem>
                                        <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3" id="lbllevel" runat="server">
                                    Level
                                    <asp:DropDownList ID="DdlLevel" CssClass="form-control" TabIndex="1" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3" id="divSearch" runat="server">
                                    Search
                                    <asp:DropDownList ID="DDlSearchby" CssClass="form-control" TabIndex="2" runat="server">
                                        <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                        </div>
                        <div class="col-md-12">
                            <%--<div class="col-md-3">
                                       
                                            PageSize:
                                       
                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="form-control">
                                                <asp:ListItem Text="100" Value="100" />
                                                <asp:ListItem Text="200" Value="200" />
                                                <asp:ListItem Text="300" Value="300" />
                                                <asp:ListItem Text="400" Value="400" />
                                                <asp:ListItem Text="500" Value="500" />
                                                <asp:ListItem Text="600" Value="600" />
                                            </asp:DropDownList>
                                        </div>--%>
                            <div class="col-md-3" style="padding-top: 1.8%">
                                <asp:Button ID="BtnSubmit" runat="server" Text="Search" TabIndex="3" ValidationGroup="Save"
                                    class="btn btn-primary" />
                                    <asp:Button ID="BtnExport" runat="server" Text="Excel" TabIndex="4" ValidationGroup="Save"
                                    class="btn btn-primary" />
                            </div>
                            <div class="col-md-6">
                            </div>
                        </div>
                        <br>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <div class="col-md-3">
                            </div>
                            <div class="sda-content-3">
                                <table id="table" class="table table-bordered">
                                    <tbody>
                                        <tr>
                                            <td>
                                            </td>
                                            <th style="text-align: center">
                                                Left
                                            </th>
                                            <th style="text-align: center">
                                                Right
                                            </th>
                                            <th style="text-align: center">
                                                Total
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>
                                                Total Direct
                                            </th>
                                            <td id="tdDirectleft" runat="server" style="text-align: center">
                                                0
                                            </td>
                                            <td id="tdDirectright" runat="server" style="text-align: center">
                                                0
                                            </td>
                                            <td id="TotalDirect" runat="server" style="text-align: center">
                                                0
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Active Direct
                                            </th>
                                            <td id="tddirectActive" runat="server" style="text-align: center">
                                                0
                                            </td>
                                            <td id="tdindirectActive" runat="server" style="text-align: center">
                                                0
                                            </td>
                                            <td id="TotalActive" runat="server" style="text-align: center">
                                                0
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Unit Direct
                                            </th>
                                            <td id="Directunit" runat="server" style="text-align: center">
                                                0
                                            </td>
                                            <td id="indirectunit" runat="server" style="text-align: center">
                                                0
                                            </td>
                                            <td id="totalunit" runat="server" style="text-align: center">
                                                0
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="col-md-3">
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div id="DivSideA" runat="server">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="Label1" runat="server" Text="Total Records"></asp:Label>
                                            <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                                GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                ShowHeader="true" AllowSorting="true" PageSize="40" EmptyDataText="No data to display."
                                                ForeColor="Black">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="MLevel" HeaderText=" Level" />
                                                    <asp:BoundField DataField="SponsorId" HeaderText="Sponsor ID" />
                                                    <asp:BoundField DataField="MemberName" HeaderText="Sponsor Name" />
                                                    <asp:BoundField DataField="IDNo" HeaderText=" ID No" />
                                                    <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                                    <asp:BoundField DataField="Mobileno" HeaderText="MobileNo" />
                                                    <asp:BoundField DataField="Position" HeaderText="Position" />
                                                    <asp:BoundField DataField="BV" HeaderText="RV" />
                                                    <asp:BoundField DataField="PackageName" HeaderText="Package Name" />
                                                    <asp:BoundField DataField="Status" HeaderText="Active Status" />
                                                    <asp:BoundField DataField="UpgradeDate" HeaderText="Activation Date" />
                                                </Columns>
                                            </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
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
    </div>
    </div> </div> </div>
</asp:Content>
