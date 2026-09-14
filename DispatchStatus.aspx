<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DispatchStatus.aspx.vb" Inherits="DispatchStatus" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
    <link href="css/Main.css" rel="stylesheet" type="text/css" />
    <title>Dispatch Master</title>

    <style type="text/css">
        
        .dispatch-form-wrapper {
            max-width: 620px;
            margin: 0 auto;
            padding: 10px 0;
        }

        .form-row {
            display: table;
            width: 100%;
            margin-bottom: 14px;
        }

        .form-label-cell {
            display: table-cell;
            width: 170px;
            padding: 8px 14px 8px 0;
            text-align: right;
            vertical-align: middle;
            font-size: 13px;
            font-weight: 500;
            color: #555;
            white-space: nowrap;
        }

        .form-input-cell {
            display: table-cell;
            vertical-align: middle;
            padding: 0;
        }

        .form-input-cell .form-control {
            width: 100%;
            height: 36px;
            font-size: 13px;
            padding: 6px 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
            color: #333;
            background-color: #fff;
        }

        .form-input-cell .form-control:focus {
            border-color: #337ab7;
            outline: none;
            box-shadow: 0 0 4px rgba(51, 122, 183, 0.35);
        }

        /* ===== OTP Section ===== */
        .otp-info-text {
            font-size: 12px;
            color: #c0392b;
            margin-bottom: 5px;
            display: block;
        }

        .otp-hint-text {
            font-size: 11px;
            color: #888;
            margin-top: 4px;
            display: block;
        }

        /* ===== Radio Button ===== */
        .radio-inline-group label {
            font-size: 13px;
            font-weight: normal;
            color: #444;
            margin-right: 18px;
            cursor: pointer;
        }

        .radio-inline-group input[type="radio"] {
            margin-right: 5px;
            vertical-align: middle;
        }

        /* ===== Buttons ===== */
        .form-btn-row {
            display: table;
            width: 100%;
            margin-top: 20px;
        }

        .form-btn-cell {
            display: table-cell;
            padding-left: 170px;
            vertical-align: middle;
        }

        .btn-dispatch {
            padding: 8px 32px;
            font-size: 13px;
            font-weight: 500;
            background-color: #337ab7;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            letter-spacing: 0.2px;
        }

        .btn-dispatch:hover {
            background-color: #286090;
        }

        /* ===== Panel ===== */
        .x_panel {
            border: 1px solid #ddd;
            border-radius: 4px;
            background: #fff;
        }

        .x_title {
            padding: 12px 18px;
            border-bottom: 1px solid #e5e5e5;
            background-color: #f7f7f7;
            border-radius: 4px 4px 0 0;
        }

        .x_title h2 {
            font-size: 16px;
            font-weight: 600;
            color: #333;
            margin: 0;
        }

        .panel-body {
            padding: 22px 18px;
        }

        /* ===== Validation ===== */
        .field-error {
            font-size: 11px;
            color: #c0392b;
            margin-top: 3px;
            display: block;
        }

        /* ===== Divider ===== */
        .form-section-divider {
            border: none;
            border-top: 1px solid #eee;
            margin: 18px 0;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
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
                                <h2>Dispatch Master</h2>
                                <div class="clearfix"></div>
                            </div>

                            <div class="panel-body">

                                <%-- Error / Info Message --%>
                                <div style="text-align: center; margin-bottom: 10px;">
                                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    <span id="lblt" class="text-danger" style="font-size: 13px;"></span>
                                </div>

                                <div class="dispatch-form-wrapper">

                                    <%-- Dispatch Date --%>
                                    <div class="form-row">
                                        <div class="form-label-cell">
                                            Dispatch Date :
                                        </div>
                                        <div class="form-input-cell">
                                            <asp:TextBox ID="txtDispatchDate" runat="server" CssClass="form-control" placeholder="dd-MMM-yyyy"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server"
                                                TargetControlID="txtDispatchDate"
                                                Format="dd-MMM-yyyy">
                                            </AjaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator
                                                ID="RegularExpressionValidator3"
                                                runat="server"
                                                ControlToValidate="txtDispatchDate"
                                                ErrorMessage="Invalid Date format"
                                                CssClass="field-error"
                                                SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>

                                    <%-- Dispatch Remark --%>
                                    <div class="form-row">
                                        <div class="form-label-cell">
                                            Dispatch Remark :
                                        </div>
                                        <div class="form-input-cell">
                                            <asp:TextBox ID="TxtDispatchRemark" runat="server" CssClass="form-control" placeholder="Enter remark..."></asp:TextBox>
                                        </div>
                                    </div>

                                    <%-- Dispatch Status --%>
                                    <div class="form-row">
                                        <div class="form-label-cell">
                                            Dispatch Status :
                                        </div>
                                        <div class="form-input-cell">
                                            <div class="radio-inline-group" style="padding-top: 6px;">
                                                <asp:RadioButtonList ID="rdblist" runat="server"
                                                    RepeatDirection="Horizontal"
                                                    CellPadding="0"
                                                    CellSpacing="10"
                                                    >
                                                    <asp:ListItem Selected="true" Text="Dispatch" Value="C"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- Hidden Fields --%>
                                    <asp:TextBox ID="txtCatId" runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>

                                    <%-- Send OTP Button --%>
                                    <div class="form-btn-row" id="DVLginF" runat="server">
                                        <div class="form-btn-cell">
                                            <asp:Button ID="BtnSave" runat="server"
                                                CssClass="btn-dispatch"
                                                Text="Send OTP"
                                                ValidationGroup="Save" />
                                        </div>
                                    </div>

                                    <%-- OTP Section --%>
                                    <div id="DVOtp" runat="server" visible="false" style="margin-top: 18px;">
                                        <hr class="form-section-divider" />
                                        <div class="form-row">
                                            <div class="form-label-cell">
                                                OTP :
                                            </div>
                                            <div class="form-input-cell">
                                                <asp:Label ID="LblEmailMsg" runat="server" Text=""
                                                    CssClass="otp-info-text"></asp:Label>
                                                <asp:TextBox ID="TxtOTP" runat="server"
                                                    CssClass="form-control"
                                                    placeholder="Enter OTP"
                                                    onkeypress="return isNumberKey(event)"
                                                    MaxLength="6">
                                                </asp:TextBox>
                                                <span class="otp-hint-text">You can try 3 times only.</span>
                                            </div>
                                        </div>

                                        <%-- Dispatch Button --%>
                                        <div class="form-btn-row" id="DVOtpLogin" runat="server" visible="false">
                                            <div class="form-btn-cell">
                                                <asp:Button ID="BtnLogin" runat="server"
                                                    CssClass="btn-dispatch"
                                                    Text="Dispatch"
                                                    ValidationGroup="Save" />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <%-- end dispatch-form-wrapper --%>

                            </div>
                            <%-- end panel-body --%>

                        </div>
                        <%-- end x_panel --%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    </form>
</body>
</html>