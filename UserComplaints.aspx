<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="UserComplaints.aspx.vb" Inherits="UserComplaints" %>

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
    <style type="text/css">
        .DDl
        {
            display: block;
            width: 200px;
            height: 24px;
            padding: 3px 16px;
            font-size: 14px;
            line-height: 1.428571429;
            color: #8e8e93;
            vertical-align: middle;
            background-color: #ffffff;
            border: 1px solid #c7c7cc;
            border-radius: 4px;
            -webkit-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }
    </style>

    <script type="text/javascript">
        function DisplayImage(ctrl) {
            debugger;
        document.getElementById('imgLargeImage').src = ctrl.src;

        $("#dialog").dialog({
            title: "View Details",
            buttons: {
                Close: function () {
                    $(this).dialog('close');
                }
            },
            modal: true
        });
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Complaint</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive">
                            <div align="center">
                                <div class="col-md-12" style="display: inline; padding: 1%"">
                                    <div class="col-md-2">
                                        Search By
                                        <asp:DropDownList ID="ddlGroupFields" runat="server" class="form-control">
                                            <asp:ListItem Value="None" Selected="True">--Search By--</asp:ListItem>
                                            <asp:ListItem Value="m.IDNo">ID No.</asp:ListItem>
                                            <asp:ListItem Value="m.CType">Complaint Type</asp:ListItem>
                                            <asp:ListItem Value="m.Complaint">Complaint</asp:ListItem>
                                            <asp:ListItem Value="s.Solution">Solution</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Search Value
                                        <asp:TextBox ID="txtSearch" class="form-control" runat="server" Style="display: inline"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        Type :
                                        <asp:DropDownList ID="RbReplied" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                            class="form-control">
                                            <asp:ListItem Value="K" Text="All" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Replied"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Not yet Replied"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Status :
                                        <asp:DropDownList ID="RbtStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                            class="form-control">
                                            <asp:ListItem Value="K" Text="All" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="C" Text="Close"></asp:ListItem>
                                            <asp:ListItem Value="O" Text="Open"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12" style="display: inline">
                                    <div class="col-md-2">
                                        From
                                        <asp:TextBox ID="TxtFromDate" class="form-control" runat="server"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtFromDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="TxtFromDate"
                                            ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Save"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        To
                                        <asp:TextBox ID="TxtToDate" class="form-control" runat="server"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtToDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                            ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Save"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2" id="Divstate" runat="server" visible ="false" >
                                        Select State :
                                        <asp:DropDownList ID="DDlState" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="LblGroup" runat="server" Text="Search User Wise">
                                        </asp:Label>
                                        <asp:DropDownList ID="DDlGroup" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div style="width: 100%; float: right; margin-bottom: 20px;">
                                        <asp:Button ID="BtnSubmit" runat="server" class="btn btn-primary" Text="Search" ValidationGroup="Save" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" />&nbsp;
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                            Visible="false" />&nbsp;
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                            Visible="false" />&nbsp;
                                        <asp:Button ID="btnShowRecord" runat="server" CssClass="btn-btn primary " Text="View All"
                                            Visible="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div style="margin-bottom: 20px">
                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                            GridLines="None" AllowPaging="true" CssClass="table table-striped table-advance table-hover"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                            PageSize="20" EmptyDataText="No data to display.">
                            <Columns>
                                <asp:TemplateField HeaderText="ComplaintID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CID" HeaderText="Compl.ID" />
                                <asp:BoundField DataField="IDNo" HeaderText="ID No." ControlStyle-Width="50px" />
                                <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                <asp:BoundField DataField="CDate" HeaderText="Complaint Date" />
                                <asp:BoundField DataField="CType" HeaderText="Complaint Type" />
                                <asp:BoundField DataField="Complaint" HeaderText="Complaint" />
                                <asp:BoundField DataField="SDate" HeaderText="Reply Date" />
                                <asp:BoundField DataField="Solution" HeaderText="Previous Reply" />
                                <asp:BoundField DataField="StateName" HeaderText="State Name" />
                                <asp:TemplateField HeaderText="Reply" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                    ControlStyle-CssClass="btn-group ">
                                    <ItemTemplate>
                                        <a class="btn btn-primary" href='<%# "Reply.aspx?CId=" & (Eval("VCId"))  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                            <i class="icon-check-alt2"></i>
                                            <asp:Label ID="LBModify" runat="server" Text="Reply" />
                                        </a>
                                    </ItemTemplate>
                                    <HeaderStyle Width="55px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle " />
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
                        <asp:GridView ID="gvdatanew" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                            GridLines="None" AllowPaging="true" CssClass="table table-striped table-advance table-hover"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                            PageSize="20" EmptyDataText="No data to display.">
                            <Columns>
                                <asp:TemplateField HeaderText="ComplaintID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CID" HeaderText="Compl.ID" />
                                <asp:BoundField DataField="IDNo" HeaderText="ID No." ControlStyle-Width="50px" />
                                <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                <asp:BoundField DataField="Mobl" HeaderText="Mobile No." />
                                <asp:BoundField DataField="CDate" HeaderText="Complaint Date" />
                                <asp:BoundField DataField="CType" HeaderText="Complaint Type" />
                                <asp:BoundField DataField="Complaint" HeaderText="Complaint" />
                                <asp:BoundField DataField="SDate" HeaderText="Reply Date" />
                                <asp:BoundField DataField="Solution" HeaderText="Previous Reply" />
                                <asp:TemplateField HeaderText="Complain Proof" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a href='<%# "Img.aspx?ID=" & Eval("VCId") & "&Type=ComplainProof" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 470,marginTop : 0 } )">
                                            <asp:Image ID="Image3" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("ComplainProof")  %>' />
                                            <%--<a href='<%# "Img.aspx?ID=" & Eval("FormNo") & "&Type=FrontAddress" %>'  onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                  --%>
                                        </a>
                                        <br />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField ="Status" HeaderText="Status" />--%>
                                <asp:TemplateField HeaderText="Reply" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                    ControlStyle-CssClass="btn-group ">
                                    <ItemTemplate>
                                        <a class="btn btn-primary" href='<%# "Reply.aspx?CId=" & (Eval("VCId"))  %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                            <i class="icon-check-alt2"></i>
                                            <asp:Label ID="LBModify" runat="server" Text="Reply" />
                                        </a>
                                        <%--<asp:LinkButton ID="lnkModify" runat="server" Text="Modify" OnClick="ModifyGrp" CssClass="fancybox fancybox.iframe"></asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="55px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle " />
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
                    </div>
                    <br />
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
