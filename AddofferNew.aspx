<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddofferNew.aspx.vb" Inherits="AddofferNew"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

    <title></title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <AjaxToolkit:ToolkitScriptManager ID="scriptmanager1" runat="server">
    </AjaxToolkit:ToolkitScriptManager>
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Add Package Offer</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Offer Name:</div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtoffer" runat="server" class="form-control" Width="150px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtoffer" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Start Date:&nbsp&nbsp&nbsp</div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtStartDate" runat="server" Width="150px" class="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtStartDate" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        End Date :&nbsp&nbsp&nbsp&nbsp</div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control" Width="150px">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtEndDate" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy">
                                        </AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" runat="server" id="selfbv">
                                    <div class="col-md-4">
                                        Self BV:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtselfbv" runat="server" class="form-control" Width="150px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtselfbv" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" runat="server" id="DirectBV">
                                    <div class="col-md-4">
                                        Direct BV:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtdirectbv" runat="server" class="form-control" Width="150px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtdirectbv" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" runat="server" id="Div4">
                                    <div class="col-md-4">
                                        Reward:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtReward" runat="server" class="form-control" Width="150px" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtdirectbv" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" runat="server" id="DivStatus">
                                    <div class="col-md-4">
                                        Active Status
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="rbtnstatus" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Active" Value="Y" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" runat="server" id="DivOfferFor">
                                    <div class="col-md-4">
                                        Offer For
                                    </div>
                                    <div class="col-md-6">
                                        <% If Session("CompID") = "1010"  Or Session("CompID") = "1103" Then %>
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true">
                                            <asp:ListItem Text="All Rank matching" Value="T" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="All Rank Direct" Value="A"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <% Else%>
                                        <asp:RadioButtonList ID="rbtOfferFor" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true">
                                            <asp:ListItem Text="Rank" Value="P" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="All Rank matching" Value="T"></asp:ListItem>
                                            <asp:ListItem Text="All Rank Direct" Value="A"></asp:ListItem>
                                            <asp:ListItem Text="Normal" Value="B"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <% End If%>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div id="divAll" runat="server" visible="false">
                                    <div class="col-md-12" style="display: none">
                                        <div class="col-md-4">
                                            Left Joining:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtAllLeftJoin" runat="server" class="form-control" Width="150px"
                                                onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="rfvAllLeftJoin" runat="server" ErrorMessage="*" ControlToValidate="txtAllLeftJoin"
                                                ValidationGroup="Save" Enabled="false"></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="display: none;">
                                        <div class="col-md-4">
                                            Right Joining:
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtAllRightJoing" runat="server" class="form-control" Width="150px"
                                                onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="rfvAllRightJoing" runat="server" ErrorMessage="*"
                                                ControlToValidate="txtAllRightJoing" ValidationGroup="Save" Enabled="false"></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="display: none">
                                        <div class="col-md-4">
                                            Matching BV:&nbsp&nbsp&nbsp&nbsp&nbsp
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtAllDirectJoining" runat="server" class="form-control" Width="150px"
                                                onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            <%--  <asp:RequiredFieldValidator ID="rfvdirectLeft" runat="server" ErrorMessage="*"
                                                ControlToValidate="txtAllDirectJoining" ValidationGroup="Save" Enabled="false"></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="display: none">
                                        <div class="col-md-4">
                                            Direct Left :&nbsp&nbsp
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtalldirectleft" runat="server" class="form-control" Width="150px"
                                                onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="rfvdirectRight" runat="server" ErrorMessage="*" ControlToValidate="txtalldirectleft"
                                                ValidationGroup="Save" Enabled="false"></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                    <div class="col-md-12" style="display: none">
                                        <div class="col-md-4">
                                            Direct Right :&nbsp&nbsp
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtalldirectright" runat="server" class="form-control" Width="150px"
                                                onkeypress="return isNumberKey(event);"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                                                ControlToValidate="txtalldirectright" ValidationGroup="Save" Enabled="false"></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                </div>
                                <div id="divPackage" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" class="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S. No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rank">
                                                    <ItemTemplate>
                                                        <%# Eval("rank") %>
                                                        <asp:HiddenField ID="hdnKitName" runat="server" Value='<%# Eval("rank")  %>' />
                                                        <asp:HiddenField ID="hdnKitId" runat="server" Value='<%# Eval("Rankid")  %>' />
                                                        <%-- <%# Eval("kitname") %>
                                                        <asp:HiddenField ID="hdnKitName" runat="server" Value='<%# Eval("kitname")  %>' />
                                                        <asp:HiddenField ID="hdnKitId" runat="server" Value='<%# Eval("KitID")  %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Matching BV">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLeftActive" runat="server" Text="0" Width="50px" MaxLength="8"
                                                            onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtActiveLeft_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActive" FilterType="Numbers, Custom" FilterMode="ValidChars"
                                                            ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRightActive" runat="server" Text="0" Width="50px"></asp:TextBox>
                                                        <%--<ajax:FilteredTextBoxExtender ID="txtRightActive_FilteredTextBoxExtender1" runat="server"
                                                            Enabled="True" TargetControlID="txtRightActive" FilterType="Custom" FilterMode="ValidChars"
                                                            ValidChars="0123456789">
                                                        </ajax:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Direct Active" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDirectActive" runat="server" Text="0" Width="50px" MaxLength="4"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtDirectActive_FilteredTextBoxExtender1" runat="server"
                                                            Enabled="True" TargetControlID="txtDirectActive" FilterType="Custom" FilterMode="ValidChars"
                                                            ValidChars="0123456789">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Direct Left Active" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdirLeftActive" runat="server" Text="0" Width="50px" MaxLength="4"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtdirLeftActive_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtdirLeftActive" FilterType="Custom" FilterMode="ValidChars"
                                                            ValidChars="0123456789">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Direct Right Active" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdirRightActive" runat="server" Text="0" Width="50px" MaxLength="4"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtdirRightActive_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtdirRightActive" FilterType="Custom" FilterMode="ValidChars"
                                                            ValidChars="0123456789">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div id="div1" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" class="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S. No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Matching BV">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLeftActivess" runat="server" Text="0" Width="100px" MaxLength="8"
                                                            onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtLeftActivess_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActivess" FilterType="Numbers, Custom"
                                                            FilterMode="ValidChars" ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLeftActivessf" runat="server" Text="0" Width="100px"></asp:TextBox>
                                                        <%-- <ajax:FilteredTextBoxExtender ID="txtLeftActivessf_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActivessf" FilterType="Numbers, Custom" FilterMode="ValidChars"
                                                            ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Level" Visible =false >
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRightActfive" runat="server" Text="0" Width="50px" MaxLength="8"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtRightActfive_FilteredTextBoxExtender1" runat="server"
                                                            Enabled="True" TargetControlID="txtRightActfive" FilterType="Custom" FilterMode="ValidChars"
                                                            ValidChars="0123456789">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div id="div2" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" class="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S. No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DirectBV">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="LeftActiveSSES" runat="server" Text="0" Width="100px" MaxLength="8"
                                                            onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="LeftActiveSSES_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="LeftActiveSSES" FilterType="Numbers, Custom"
                                                            FilterMode="ValidChars" ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="LeftActivessfES" runat="server" Text="0" Width="100px"></asp:TextBox>
                                                        <%-- <ajax:FilteredTextBoxExtender ID="txtLeftActivessf_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActivessf" FilterType="Numbers, Custom" FilterMode="ValidChars"
                                                            ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Level" Visible =false >
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRightActfive" runat="server" Text="0" Width="50px" MaxLength="8"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtRightActfive_FilteredTextBoxExtender1" runat="server"
                                                            Enabled="True" TargetControlID="txtRightActfive" FilterType="Custom" FilterMode="ValidChars"
                                                            ValidChars="0123456789">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div id="div3" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="false" class="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S. No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DirectBV">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="LeftActiveSSES" runat="server" Text="0" Width="100px" MaxLength="8"
                                                            onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="LeftActiveSSES_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="LeftActiveSSES" FilterType="Numbers, Custom"
                                                            FilterMode="ValidChars" ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="LeftActivessfES" runat="server" Text="0" Width="100px"></asp:TextBox>
                                                        <%-- <ajax:FilteredTextBoxExtender ID="txtLeftActivessf_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActivessf" FilterType="Numbers, Custom" FilterMode="ValidChars"
                                                            ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Level" Visible =false >
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRightActfive" runat="server" Text="0" Width="50px" MaxLength="8"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtRightActfive_FilteredTextBoxExtender1" runat="server"
                                                            Enabled="True" TargetControlID="txtRightActfive" FilterType="Custom" FilterMode="ValidChars"
                                                            ValidChars="0123456789">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div id="div5" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false" class="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S. No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward Point">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLeftActivess" runat="server" Text="0" Width="100px" MaxLength="8"
                                                            onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtLeftActivess_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActivess" FilterType="Numbers, Custom"
                                                            FilterMode="ValidChars" ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLeftActivessf" runat="server" Text="0" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div id="div7" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="false" class="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S. No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward Point">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLeftActivess" runat="server" Text="0" Width="100px" MaxLength="8"
                                                            onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtLeftActivess_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActivess" FilterType="Numbers, Custom"
                                                            FilterMode="ValidChars" ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Direct">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLeftActivess1" runat="server" Text="0" Width="100px" MaxLength="8"
                                                            onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtLeftActivess1_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActivess1" FilterType="Numbers, Custom"
                                                            FilterMode="ValidChars" ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLeftActivessf" runat="server" Text="0" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div id="div6" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="false" class="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S. No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward Point">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="LeftActiveSSES" runat="server" Text="0" Width="100px" MaxLength="8"
                                                            onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="LeftActiveSSES_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="LeftActiveSSES" FilterType="Numbers, Custom"
                                                            FilterMode="ValidChars" ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="LeftActivessfES" runat="server" Text="0" Width="100px"></asp:TextBox>
                                                        <%-- <ajax:FilteredTextBoxExtender ID="txtLeftActivessf_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" TargetControlID="txtLeftActivessf" FilterType="Numbers, Custom" FilterMode="ValidChars"
                                                            ValidChars=".">
                                                        </ajax:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Level" Visible =false >
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRightActfive" runat="server" Text="0" Width="50px" MaxLength="8"></asp:TextBox>
                                                        <ajax:FilteredTextBoxExtender ID="txtRightActfive_FilteredTextBoxExtender1" runat="server"
                                                            Enabled="True" TargetControlID="txtRightActfive" FilterType="Custom" FilterMode="ValidChars"
                                                            ValidChars="0123456789">
                                                        </ajax:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Add Offer" class="btn btn-primary"
                                            ValidationGroup="Save" OnClientClick="this.disabled=true; this.value='Sending Request…';"
                                            UseSubmitBehavior="false" /></div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                        <asp:Label ID="LblDateMsg" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">

function IsOneDecimalPoint(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
                var parts = evt.srcElement.value.split('.');
                if(parts.length > 1 && charCode==46)
                    return false;
                return true;
            }
        </script>
    </form>
</body>
</html>
