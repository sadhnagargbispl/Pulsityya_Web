<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="DownlinePurchaseLife.aspx.vb" Inherits="DownlinePurchaseLife" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Downline Purchase</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div style="background-color: White">
                        <div class="col-md-12" style="padding: 1%">
                            <div class="col-md-2" >
                                Member Id:
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtMemberId" runat="server" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldvalidator1" runat="server" ControlToValidate="txtMemberId"
                                    ErrorMessage="Enter Member Id" SetFocusOnError="true" ValidationGroup ="Save"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-2">
                                <asp:CheckBox runat="server" ID="ChkDate" Text="From Date : " /></div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                                <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                    Format="dd-MMM-yyyy">
                                </AjaxToolkit:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFromDate"
                                    ErrorMessage="Invalid From Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-2">
                                To Date :</div>
                            <div class="col-md-2">
                                <asp:TextBox ID="TxtToDate" runat="server" class="form-control"></asp:TextBox>
                                <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                    Format="dd-MMM-yyyy">
                                </AjaxToolkit:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                    ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="col-md-2" >
                                Request Type:
                            </div>
                            <div class="col-md-5">
                            <% If Session("CompID") = "1096" Then%>
                             <asp:RadioButtonList ID="Rbtbsntype" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" AutoPostBack ="true">
                                   <%-- <asp:ListItem Text="Joining" Value="T" Selected ="True"></asp:ListItem>--%>
                                    <asp:ListItem Text="Repurchase Request" Value="R" Selected ="True" ></asp:ListItem>
                                </asp:RadioButtonList>
                            <%Else%>
                             <asp:RadioButtonList ID="RbtRequestType" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" AutoPostBack ="true">
                                    <asp:ListItem Text="Joining" Value="T" Selected ="True"></asp:ListItem>
                                    <asp:ListItem Text="Repurchase Request" Value="R"></asp:ListItem>
                                </asp:RadioButtonList>
                            <% End If%>
                               
                            </div>
                            <div class="col-md-2" >
                                <asp:Label ID="LblType" runat="server" Text="Leg:"></asp:Label>
                            </div>
                            <div class="col-md-3">
                            <asp:DropDownList ID="ddlLevel" runat="server" Visible ="false" CssClass="form-control"></asp:DropDownList>
                                <asp:RadioButtonList ID="RbtLeg" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Both" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Group A" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Group B" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            
                        </div>
                        <div class="col-md-12">
                            <div class="col-md-4">
                                <asp:Button ID="BtnSearch" runat="server" Text="Search" class="btn btn-primary" ValidationGroup ="Save" />
                                <asp:Button ID="BtnExport" runat="server" Text="Export To Excel" class="btn btn-primary" /></div>
                            <div class="col-md-8">
                           <asp:Label ID="lbltotal" runat="server" Text="Total PV:" ></asp:Label><asp:Label ID="LblTotalEP" runat="server" Text="0" ></asp:Label>
                                <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
                                <span runat="server" id="LBlLeftEp">
                                <% If Session("CompID") = "1096" Then%>
                                 Group A BV:
                                <%Else%>
                                 Group A PV:
                                <% End If%>
                               
                                
                                </span>
                                    <asp:Label ID="LblTotalLeftEP" Font-Bold="true" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <span runat="server" id="LblRightEP">
                                    <% If Session("CompID") = "1096" Then%>
                                 Group B BV:
                                <%Else%>
                                Group B PV:
                                <% End If%></span>
                                    <asp:Label ID="LblTotalRightEP" Font-Bold="true" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </div>
                        </div>
                    </div>
                    <asp:GridView ID="GrdDirects" Width="100%" runat="server" AllowPaging="false" GridLines="Both"
                        class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                        EmptyDataText="No data to display." AutoGenerateColumns="false" AllowSorting="true">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1%>.</ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BillDate" HeaderText="BillDate"></asp:BoundField>
                            <asp:BoundField DataField="Idno" HeaderText="IdNo"></asp:BoundField>
                            <asp:BoundField DataField="MemberName" HeaderText="MemberName"></asp:BoundField>
                            <asp:BoundField DataField="LegNo" HeaderText="Group/Level"></asp:BoundField>
                            <asp:BoundField DataField="PackageName" HeaderText="PackageName" />
                            <asp:BoundField DataField="Amount" HeaderText="BillAmount"></asp:BoundField>
                            <asp:BoundField DataField="BV" HeaderText="BV"></asp:BoundField>
                            <asp:BoundField DataField="RequestType" HeaderText="Status"></asp:BoundField>
                            <%--	 <asp:BoundField DataField ="SP" HeaderText="SP" />--%>
                            <%--     <asp:BoundField DataField ="RP" HeaderText="RP" />--%>
                            <%--<asp:BoundField DataField="TP" HeaderText="TP" />--%>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <div style="margin-bottom: 20px">
                    </div>
                </div>
</asp:Content>
