<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="AddressSearch.aspx.vb" Inherits="AddressSearch" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .right-mid .welcome
        {
            height: 23px;
            border-top: 1px solid #203454;
            border-bottom: 1px solid #203454;
            background-color: #395e96;
            color: #e4efff;
            font-weight: bold;
            margin: 0px 0px 5px 0px;
        }
        .right-mid .welcome span
        {
            padding: 3px 0px 0px 15px;
            display: block;
        }
        .box-body1-clear
        {
            width: 100%;
            background-image: url(../Resources/images/add-new-btn-bg-hover.jpg);
            font-family: Verdana,Arial;
            color: black;
            font-weight: bold;
            border-bottom: 1px solid;
        }
        .Table-lblNew
        {
            border: solid 1px;
        }
        .box-lbl
        {
            font-family: Verdana,Arial;
            color: Black;
            font-size: 11px;
            font-weight: bold;
            border: solid 1px #D0D0D0;
        }
        .box-lblNew
        {
            font-family: Verdana,Arial;
            color: Black;
            font-size: 10px;
            font-weight: bold;
            border: solid 1px #D0D0D0;
        }
        p
        {
            font-weight: bold;
            color: #666666;
            margin: 0px;
            line-height: 25px;
            width: 800px;
            padding-bottom: 8px;
            padding-top: 5px;
            text-align: left;
            padding-left: 10px;
        }
        .label_Err
        {
            padding-left: 10px;
            font-size: 11px;
            text-transform: capitalize;
            color: red;
            font-family: Verdana, Monospace;
            font-weight: bold;
        }
        .GridViewStyle
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            font-size: 10.5px;
            border-collapse: collapse;
            width: 500px;
        }
        .HeaderStyle, .PagerStyle
        {
            background-image: url(../Images/td.jpg);
            background-position: center;
            background-repeat: repeat-x;
            background-color: #ffffff;
            font-weight: bold;
            text-align: center;
            width: 00px;
        }
        .HeaderStyle a
        {
            text-decoration: none;
            color: #000000;
            display: block;
            text-align: center;
            font-weight: normal;
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
        .RowStyle td, .AltRowStyle td, .SelectedRowStyle td, .EditRowStyle td /*Common Styles*/
        {
            padding: 5px;
            border-right: solid 1px #1d1d1d;
            text-decoration: none;
            color: #000000;
            text-transform: capitalize;
            text-align: left;
        }
        .RowStyle td
        {
            background-color: transparent;
            background-image: url(../Images/rdc_hd.jpg);
        }
        .AltRowStyle td
        {
            background-color: #E4EEDB;
            background-image: url(../Images/rdc_hd1.jpg);
        }
        .right-mid .form-heading
        {
            height: 24px;
            border-top: 1px solid #5c0871;
            border-bottom: 1px solid #4671b4;
            font: bold 12px Tahoma, Geneva, sans-serif;
            color: #ffffff;
            text-shadow: #445f8a 1px 2px 0px;
            text-align: center;
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
                            Address Check</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div id="divSingle" runat="server" class="col-md-12">
                                <div class="col-md-12" style="margin-bottom: 2%">
                                    <div class="col-md-2">
                                        <strong>Wallet Address. :</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="TxtWalletAddress" runat="server" ReadOnly="false"
                                            AutoPostBack="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ControlToValidate="TxtWalletAddress"
                                            runat="server" ValidationGroup="Save" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12" style="margin-bottom: 2%">
                                    <div class="col-md-2">
                                        <strong>Member ID. :</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="TxtMemberID" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="TxtMemberID"
                                            runat="server" ValidationGroup="Save" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                                <div class="col-md-12" style="margin-bottom: 2%">
                                    <div class="col-md-2">
                                        <strong>Member Name. :</strong></div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="TxtMemberName" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="TxtMemberName"
                                            runat="server" ValidationGroup="Save" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
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
