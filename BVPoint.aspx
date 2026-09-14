<%@ Page Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="BVPoint.aspx.vb" Inherits="BVPoint" Title="" EnableEventValidation="false" %>

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
                            BV Point
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
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" CssClass="Btn" Visible="false" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" CssClass="Btn" Visible="false" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary " Text="Export To Excel" />
                                            </td>
                                            <td>
                                                <a href="AddBVPoint.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 470,marginTop : 0 } )">
                                                    <asp:Button ID="BtnAddNew" runat="server" CssClass="btn btn-primary" Text="Add New Bv" /></a>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnShowRecord" runat="server" CssClass="Btn" Text="View All" Visible="false" />
                                                <%--<asp:Button ID="BtnAdvSearch" runat="server" CssClass="Btn" Text="Advanced Search" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" id="divContent"
                                    runat="server" visible="false">
                                    <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                </div>
                                <div style="margin-bottom: 20px">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" DataKeyNames="Sessid" CssClass="table table-striped table-advance table-hover"
                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                        PageSize="20" EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="BId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("BID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="BankCode" HeaderText="Bank Code" Visible="false"/>--%>
                                            <asp:BoundField DataField="Sessid" HeaderText="Session No" />
                                            <asp:BoundField DataField="Idno" HeaderText="IdNo" />
                                            <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                            <asp:BoundField DataField="LegNo" HeaderText="LegNo" />
                                            <asp:BoundField DataField="BV" HeaderText="BV" />
                                              <asp:BoundField DataField="PV" HeaderText="PV" />
                                            <asp:BoundField DataField="Remark" HeaderText="Remarks" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"
                                                        class="btn btn-danger"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
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
