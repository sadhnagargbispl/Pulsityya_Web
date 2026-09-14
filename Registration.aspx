<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="Registration.aspx.vb" Inherits="Registration" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function DivOnOff() {
            if (document.getElementById("<%= chkterms.clientid %>").checked == true) {
                debugger;
                var hiddenField = document.getElementById('<%= HiddenCompID.ClientID %>');
                var compid = parseInt(hiddenField.value) || 0;
                var btn = document.getElementById('<%= CmdSave.ClientID %>');
                btn.value = "Submit";
                document.getElementById("DivTerms").style.display = "block";
            }
            else {
                document.getElementById("DivTerms").style.display = "none";
            }
        }

        function FnBankChange(val) {
            if (val == "97") {
                document.getElementById("divBank").style.display = "block";
            }
            else {
                document.getElementById("divBank").style.display = "none";
            }
        }
        function FnVillageChange(val) {
            if (val == "381264") {
                document.getElementById("divVillage").style.display = "block";
            }
            else {
                document.getElementById("divVillage").style.display = "none";
            }
        }
        function FnPostVillageChange(val) {
            var ddlVillage = document.getElementById("<%=DDlPostVillage.ClientID %>");
            var selectedText = ddlVillage.options[ddlVillage.selectedIndex].innerHTML;
            var selectedValue = ddlVillage.value;
            document.getElementById("<%= HPostVillage.ClientID %>").value = selectedValue;
            if (val == "381264") {
                document.getElementById("divPostVillage").style.display = "block";
            }
            else {
                document.getElementById("divPostVillage").style.display = "none";
            }
        }

        function GetSelectedItem() {
            var rb = document.getElementById("<%=RbtMarried.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked == true) {
                    if (radio[i].value == "Y") {
                        document.getElementById("divMarriageDate").style.display = "block";
                    }
                    else {
                        document.getElementById("divMarriageDate").style.display = "none";
                    }
                }
            }
        }
        function GetRegistrationAs() {
            var rb = document.getElementById("<%=RbCategory.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked == true) {
                    if (radio[i].value == "IN") {
                        document.getElementById("RegType").style.display = "none";
                        document.getElementById("CompName").style.display = "none";
                        document.getElementById("CompRegistrationNo").style.display = "none";
                        document.getElementById("Div1").style.display = "block";
                        document.getElementById("divFName").style.display = "block";
                        document.getElementById("TrPrtnrCap").style.display = "none";
                        document.getElementById("<%=LblName.ClientID %>").textContent = "";
                        document.getElementById("<%=LblRegistDate.ClientID %>").textContent = "Date Of Birth";
                        var rb2 = document.getElementById("<%=CbSubCategory.ClientID%>");
                        var radio2 = rb2.getElementsByTagName("input");
                        radio2[0].checked = true;
                    }
                    else {
                        document.getElementById("RegType").style.display = "block";
                        document.getElementById("CompName").style.display = "block";
                        document.getElementById("CompRegistrationNo").style.display = "block";
                        document.getElementById("Div1").style.display = "none";
                        document.getElementById("divFName").style.display = "none";
                        document.getElementById("TrPrtnrCap").style.display = "block";
                        document.getElementById("<%=LblName.ClientID %>").textContent = "Properitor ";
                        document.getElementById("<%=LblRegistDate.ClientID %>").textContent = "Date Of Company Registration";
                        var rb2 = document.getElementById("<%=CbSubCategory.ClientID%>");
                        var radio2 = rb2.getElementsByTagName("input");
                        radio2[0].checked = true;
                    }
                }
            }
        }

        function GetRegistrationType() {
            var rb = document.getElementById("<%=CbSubCategory.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked == true) {
                    if (radio[i].value == "SP") {
                        document.getElementById("Div1").style.display = "none";
                        document.getElementById("divFName").style.display = "none";
                        document.getElementById("TrPrtnrCap").style.display = "none";
                        document.getElementById("<%=LblName.ClientID %>").textContent = "Properitor ";
                        document.getElementById("<%=LblRegistDate.ClientID %>").textContent = "Date Of Company Registration";
                    }
                    else if (radio[i].value == "PF") {
                        document.getElementById("Div1").style.display = "none";
                        document.getElementById("divFName").style.display = "none";
                        document.getElementById("TrPrtnrCap").style.display = "block";
                        document.getElementById("<%=LblName.ClientID %>").textContent = "Partner ";
                        document.getElementById("<%=LblRegistDate.ClientID %>").textContent = "Date Of Company Registration";
                    }
                    else {
                        document.getElementById("Div1").style.display = "none";
                        document.getElementById("divFName").style.display = "none";
                        document.getElementById("TrPrtnrCap").style.display = "none";
                        document.getElementById("<%=LblName.ClientID %>").textContent = "Owner ";
                        document.getElementById("<%=LblRegistDate.ClientID %>").textContent = "Date Of Company Registration";
                    }
                }
            }
        }
    </script>

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>

    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {
            jq(document).ready(function () {
                jq("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
            });
            jq("#<%=CmdSave.ClientID %>").click(function () {
                var valid = jq("#aspnetForm").validationEngine('validate');
                if (valid == true) { return true; } else { return false; }
            });
        }
    </script>

    <%-- ══════════════════════════════════════════════════════════
     SOLFIT ENERGY — Registration Page CSS (v3 — Final Fix)
     Directly in Content1/head — highest possible specificity
══════════════════════════════════════════════════════════ --%>
    <style type="text/css">
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;
         500;600;700&
        display=swap'); /* ─── GLOBAL ─────────────────────────────── */ *
        {
            box-sizing: border-box;
        }
        body
        {
            font-family: 'Inter' , sans-serif !important;
            background: #f0f4f9 !important;
            font-size: 14px !important;
        }
        /* ─── SIDEBAR ────────────────────────────── */.left_col
        {
            background: #1a2332 !important;
            box-shadow: 2px 0 12px rgba(0,0,0,.22) !important;
        }
        .left_col .nav_menu, .left_col nav, .left_col .navbar, .left_col > div
        {
            background: #1a2332 !important;
        }
        .site_title, .site_title a
        {
            background: #131c28 !important;
            color: #fff !important;
            font-weight: 700 !important;
            border-bottom: 1px solid rgba(255,255,255,.07) !important;
        }
        /* All sidebar links */.nav.side-menu > li > a, .nav.child_menu > li > a, .left_col a
        {
            color: rgba(255,255,255,.62) !important;
            font-size: 13px !important;
            border-left: 3px solid transparent !important;
        }
        .nav.side-menu > li > a:hover, .nav.child_menu > li > a:hover, .left_col a:hover
        {
            color: #fff !important;
            background: rgba(255,255,255,.06) !important;
            border-left-color: rgba(255,255,255,.25) !important;
            text-decoration: none !important;
        }
        .nav.side-menu > li.active > a
        {
            color: #fff !important;
            background: rgba(37,99,235,.22) !important;
            border-left-color: #2563eb !important;
            font-weight: 600 !important;
        }
        .left_col .badge, .nav.side-menu .badge
        {
            background: #2563eb !important;
            color: #fff !important;
            border-radius: 10px !important;
            font-size: 10px !important;
            padding: 2px 7px !important;
            font-weight: 700 !important;
        }
        .left_col p, .left_col .profile
        {
            color: rgba(255,255,255,.4) !important;
            font-size: 12px !important;
        }
        /* ─── TOP NAVBAR ─────────────────────────── */.top_nav, .nav_menu
        {
            background: #fff !important;
            border-bottom: 1px solid #e2e8f0 !important;
            box-shadow: 0 1px 5px rgba(0,0,0,.06) !important;
        }
        .top_nav .navbar-right a, .top_nav .navbar-right span
        {
            color: #374151 !important;
            font-size: 13px !important;
            font-weight: 500 !important;
        }
        /* ─── MAIN AREA ──────────────────────────── */.right_col
        {
            background: #f0f4f9 !important;
            padding: 20px 24px !important;
        }
        /* ─── TITLE BAR ──────────────────────────── */.profile-bar-simple, .profile-bar-simple.red-border
        {
            background: #fff !important;
            border: none !important;
            border-left: 4px solid #2563eb !important;
            border-radius: 10px !important;
            box-shadow: 0 1px 4px rgba(0,0,0,.07) !important;
            padding: 13px 20px !important;
            margin-bottom: 16px !important;
        }
        .profile-bar-simple h6
        {
            font-family: 'Inter' , sans-serif !important;
            font-size: 14px !important;
            font-weight: 700 !important;
            color: #1e293b !important;
            margin: 0 !important;
            text-transform: uppercase !important;
            letter-spacing: .06em !important;
        }
        /* ─── WHITE CARD ─────────────────────────── */.profile-bar.clearfix
        {
            background: #fff !important;
            border-radius: 12px !important;
            border: 1px solid #e2e8f0 !important;
            box-shadow: 0 2px 10px rgba(0,0,0,.06) !important;
            padding: 24px 28px 32px !important;
            margin-top: 0 !important;
        }
        /* ─── SECTION HEADINGS ───────────────────── */.right_col h4, .centered h4, .col-md-12 h4
        {
            font-family: 'Inter' , sans-serif !important;
            font-size: 10.5px !important;
            font-weight: 700 !important;
            color: #2563eb !important;
            text-transform: uppercase !important;
            letter-spacing: .12em !important;
            margin: 24px 0 14px !important;
            padding-bottom: 9px !important;
            border-bottom: 2px solid #eff6ff !important;
            position: relative !important;
        }
        .right_col h5
        {
            font-family: 'Inter' , sans-serif !important;
            font-size: 10.5px !important;
            font-weight: 600 !important;
            color: #64748b !important;
            text-transform: uppercase !important;
            letter-spacing: .09em !important;
            margin: 18px 0 10px !important;
        }
        /* ─── LABELS ─────────────────────────────── */.form-group label.control-label
        {
            font-family: 'Inter' , sans-serif !important;
            font-size: 12px !important;
            font-weight: 600 !important;
            color: #374151 !important;
            padding-top: 10px !important;
            line-height: 1.4 !important;
        }
        /* ─── GREY ALTERNATE ROW ─────────────────── */.form-group.greybt
        {
            background: #f7f9ff !important;
            border-radius: 8px !important;
            padding: 7px 10px 7px 10px !important;
            margin: 0 -6px 14px !important;
            border: 1px solid #eef2ff !important;
        }
        /* ─── FORM CONTROLS — THE BIG FIX ───────── *//* Target every possible input/select that exists on this page */.right_col input[type="text"], .right_col input[type="email"], .right_col input[type="password"], .right_col input[type="tel"], .right_col input[type="number"], .right_col select, .right_col .form-control, .centered input[type="text"], .centered input[type="email"], .centered input[type="password"], .centered select, .centered .form-control, div.col-sm-10 input, div.col-sm-10 select, div.col-sm-3 select, div.col-sm-7 input, div.col-sm-4 select, #aspnetForm input[type="text"], #aspnetForm input[type="email"], #aspnetForm input[type="password"], #aspnetForm select
        {
            height: 38px !important;
            font-family: 'Inter' , sans-serif !important;
            font-size: 13px !important;
            color: #1e293b !important;
            background: #ffffff !important;
            border: 1.5px solid #cbd5e1 !important;
            border-radius: 7px !important;
            padding: 0 12px !important;
            box-shadow: none !important;
            outline: none !important;
            transition: border-color .15s, box-shadow .15s !important;
            -webkit-appearance: none !important;
            appearance: none !important;
            width: 100% !important;
            display: block !important;
            line-height: 38px !important;
        }
        /* Textarea */.right_col textarea, .centered textarea, #aspnetForm textarea
        {
            font-family: 'Inter' , sans-serif !important;
            font-size: 12.5px !important;
            color: #1e293b !important;
            background: #ffffff !important;
            border: 1.5px solid #cbd5e1 !important;
            border-radius: 7px !important;
            padding: 10px 12px !important;
            box-shadow: none !important;
            outline: none !important;
            width: 100% !important;
            line-height: 1.5 !important;
            resize: vertical !important;
        }
        /* Focus state */.right_col input[type="text"]:focus, .right_col input[type="email"]:focus, .right_col input[type="password"]:focus, .right_col select:focus, .right_col .form-control:focus, .centered input[type="text"]:focus, .centered input[type="email"]:focus, .centered select:focus, .centered .form-control:focus, div.col-sm-10 input:focus, div.col-sm-10 select:focus, div.col-sm-3 select:focus, div.col-sm-7 input:focus, div.col-sm-4 select:focus, #aspnetForm input[type="text"]:focus, #aspnetForm input[type="email"]:focus, #aspnetForm select:focus
        {
            border-color: #2563eb !important;
            box-shadow: 0 0 0 3px rgba(37,99,235,.13) !important;
            background: #fff !important;
            outline: none !important;
        }
        /* Hover state */.right_col input[type="text"]:hover:not(:focus), .right_col select:hover:not(:focus), .centered input[type="text"]:hover:not(:focus), .centered select:hover:not(:focus)
        {
            border-color: #94a3b8 !important;
        }
        /* Custom select arrow */.right_col select, .centered select, #aspnetForm select
        {
            background-image: url(        "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='10' height='6' viewBox='0 0 10 6'%3E%3Cpath d='M1 1l4 4 4-4' stroke='%2364748b' stroke-width='1.5' fill='none' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E" ) !important;
            background-repeat: no-repeat !important;
            background-position: right 11px center !important;
            padding-right: 32px !important;
            cursor: pointer !important;
        }
        /* Disabled input */.right_col input[disabled], .centered input[disabled], .right_col select[disabled], .centered select[disabled]
        {
            background: #f1f5f9 !important;
            color: #94a3b8 !important;
            cursor: not-allowed !important;
        }
        /* Readonly textarea */.right_col textarea[readonly], .centered textarea[readonly]
        {
            background: #f8faff !important;
            border: 1px solid #e2e8f0 !important;
            color: #64748b !important;
            cursor: default !important;
        }
        /* ─── RADIO & CHECKBOX ───────────────────── */input[type="radio"]
        {
            accent-color: #2563eb !important;
            width: 15px !important;
            height: 15px !important;
            cursor: pointer !important;
            vertical-align: middle !important;
            margin-right: 5px !important;
            display: inline !important;
            -webkit-appearance: auto !important;
            appearance: auto !important;
        }
        input[type="checkbox"]
        {
            accent-color: #2563eb !important;
            width: 15px !important;
            height: 15px !important;
            cursor: pointer !important;
            vertical-align: middle !important;
            margin-right: 6px !important;
            display: inline !important;
            -webkit-appearance: auto !important;
            appearance: auto !important;
        }
        /* Required star */span[style*="color: red"], span[style*="color:red"]
        {
            color: #ef4444 !important;
            font-weight: 700 !important;
        }
        /* Lookup label (was pink) */span[style*="#D11F7B"], label[style*="#D11F7B"], .right_col span[style*="D11F7B"]
        {
            color: #2563eb !important;
            font-size: 12px !important;
            font-weight: 600 !important;
            display: inline-block !important;
            margin-top: 4px !important;
        }
        /* ─── FORM GROUP SPACING ─────────────────── */.form-group
        {
            margin-bottom: 14px !important;
            position: relative !important;
        }
        /* ─── TERMS ROW ──────────────────────────── */label > center
        {
            display: flex !important;
            align-items: center !important;
            justify-content: flex-start !important;
            background: #fffbeb !important;
            border: 1px solid #fde68a !important;
            border-radius: 8px !important;
            padding: 11px 16px !important;
            gap: 8px !important;
            margin-top: 6px !important;
        }
        label > center font
        {
            font-family: 'Inter' , sans-serif !important;
            font-size: 13px !important;
            color: #374151 !important;
        }
        /* ─── BUTTONS ────────────────────────────── */.btn, input[type="submit"]
        {
            font-family: 'Inter' , sans-serif !important;
            font-size: 13px !important;
            font-weight: 600 !important;
            border-radius: 7px !important;
            padding: 9px 22px !important;
            border: none !important;
            cursor: pointer !important;
            transition: all .18s ease !important;
            height: auto !important;
            line-height: normal !important;
            display: inline-block !important;
            width: auto !important;
            -webkit-appearance: none !important;
            appearance: none !important;
        }
        /* Primary / Submit */.btn.btn-primary
        {
            background: #2563eb !important;
            color: #fff !important;
            box-shadow: 0 2px 8px rgba(37,99,235,.30) !important;
        }
        .btn.btn-primary:hover
        {
            background: #1d4ed8 !important;
            box-shadow: 0 4px 14px rgba(37,99,235,.40) !important;
            transform: translateY(-1px) !important;
            color: #fff !important;
        }
        /* Cancel — second btn-primary used as cancel */#DivTerms .btn-primary + .btn-primary, #DivTerms input[id$="CmdCancel"]
        {
            background: #fff !important;
            color: #475569 !important;
            border: 1.5px solid #cbd5e1 !important;
            box-shadow: none !important;
        }
        #DivTerms .btn-primary + .btn-primary:hover
        {
            background: #f1f5f9 !important;
            transform: none !important;
        }
        /* btn-danger (OTP) */.btn.btn-danger
        {
            background: #2563eb !important;
            color: #fff !important;
            border: none !important;
        }
        /* Submit/Cancel wrapper */#DivTerms
        {
            display: flex !important;
            gap: 10px !important;
            justify-content: flex-end !important;
            padding-top: 10px !important;
            margin-top: 4px !important;
        }
        /* ─── ERROR MESSAGES ─────────────────────── */.error, span.error, label.error
        {
            color: #ef4444 !important;
            font-size: 11px !important;
            font-weight: 500 !important;
            display: block !important;
            margin-top: 3px !important;
        }
        /* ─── LOADER OVERLAY ─────────────────────── */.modal1
        {
            position: fixed !important;
            inset: 0 !important;
            background: rgba(15,23,42,.52) !important;
            display: flex !important;
            align-items: center !important;
            justify-content: center !important;
            z-index: 99999 !important;
            backdrop-filter: blur(3px) !important;
        }
        .center1
        {
            background: #fff !important;
            border-radius: 12px !important;
            padding: 32px 44px !important;
            box-shadow: 0 8px 32px rgba(0,0,0,.18) !important;
        }
        /* ─── SCROLLBAR ──────────────────────────── */::-webkit-scrollbar
        {
            width: 5px;
            height: 5px;
        }
        ::-webkit-scrollbar-track
        {
            background: transparent;
        }
        ::-webkit-scrollbar-thumb
        {
            background: #cbd5e1;
            border-radius: 4px;
        }
        ::-webkit-scrollbar-thumb:hover
        {
            background: #94a3b8;
        }
        /* ════════════════════════════════════════════
   RESPONSIVE — Mobile & Tablet
════════════════════════════════════════════ *//* Tablet (max 992px) */@media (max-width: 992px)
        {
            .right_col
            {
                padding: 14px 16px !important;
            }
            .profile-bar.clearfix
            {
                padding: 18px 16px 24px !important;
            }
            /* Stack label above input on tablet */    .form-group label.control-label
            {
                width: 100% !important;
                text-align: left !important;
                padding-top: 0 !important;
                padding-bottom: 4px !important;
                float: none !important;
            }
            .form-group .col-sm-10, .form-group .col-sm-3, .form-group .col-sm-7, .form-group .col-sm-4
            {
                width: 100% !important;
                padding-left: 0 !important;
                padding-right: 0 !important;
            }
        }
        /* Mobile (max 768px) */@media (max-width: 768px)
        {
            /* Hide sidebar on mobile */    .left_col
            {
                display: none !important;
            }
            /* Full width main */    .right_col, .col-md-12
            {
                width: 100% !important;
                padding: 10px 12px !important;
                margin-left: 0 !important;
            }
            .profile-bar.clearfix
            {
                padding: 14px 12px 20px !important;
                border-radius: 8px !important;
            }
            /* All columns full width */ [
            class*="col-sm-"], [
            class*="col-md-"], [
            class*="col-lg-"]
            {
                width: 100% !important;
                float: none !important;
                padding-left: 0 !important;
                padding-right: 0 !important;
            }
            /* Full width inputs */    .right_col input[type="text"], .right_col input[type="email"], .right_col input[type="password"], .right_col select, .right_col .form-control, .centered input, .centered select, .centered .form-control
            {
                width: 100% !important;
                min-width: 0 !important;
            }
            /* Labels stack */    .form-group label.control-label
            {
                display: block !important;
                width: 100% !important;
                text-align: left !important;
                padding-top: 0 !important;
                padding-bottom: 5px !important;
                float: none !important;
                margin-bottom: 4px !important;
            }
            /* Buttons full width on mobile */    #DivTerms
            {
                flex-direction: column !important;
                gap: 8px !important;
            }
            #DivTerms .btn
            {
                width: 100% !important;
                text-align: center !important;
            }
            /* Section headings */    .right_col h4, .centered h4
            {
                font-size: 10px !important;
            }
            /* Card */    .profile-bar-simple
            {
                padding: 10px 14px !important;
            }
            /* Row fix */    .row
            {
                margin-left: 0 !important;
                margin-right: 0 !important;
            }
            /* Father name row — stack on mobile */    #divFName .row > div
            {
                width: 100% !important;
                padding-left: 0 !important;
                padding-right: 0 !important;
                margin-bottom: 8px !important;
            }
            /* DOB row */    #Divdob .row > div
            {
                width: 32% !important;
                display: inline-block !important;
                padding: 0 3px !important;
                float: left !important;
            }
        }
        /* Small mobile (max 480px) */@media (max-width: 480px)
        {
            body
            {
                font-size: 13px !important;
            }
            .profile-bar.clearfix
            {
                padding: 12px 10px 16px !important;
            }
            #Divdob .row > div
            {
                width: 100% !important;
                display: block !important;
                float: none !important;
                margin-bottom: 8px !important;
            }
            .right_col h4, .centered h4
            {
                font-size: 9.5px !important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="0">
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
            <div class="col-md-12">
                <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                    <div class="profile-bar-simple red-border clearfix">
                        <h6>
                            New joining
                        </h6>
                    </div>
                    <div class="centered">
                        <div class="clr">
                            <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                            <asp:HiddenField ID="HiddenCompID" runat="server" />
                        </div>
                        <div class="col-md-12" class="form-horizontal" action="/action_page.php">
                            All fields are mandatory with (*) sign.
                            <h4>
                                Sponsor Detail</h4>
                            <div>
                                <!-- ================= Sponsor ================= -->
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div class="form-group">
                                                    <label>
                                                        Sponsor ID *</label>
                                                    <asp:TextBox ID="txtRefralId" runat="server" CssClass="form-control validate[required,custom[onlyLetterNumber]]"
                                                        AutoPostBack="True"></asp:TextBox>
                                                    <asp:Label ID="lblRefralNm" runat="server" ForeColor="#D11F7B"></asp:Label>
                                                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-6">
                                        <div runat="server" id="DivUserName" visible="false">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-group greybt">
                                                        <label class="control-label col-sm-2">
                                                            User Name <span style="color: red  !important; font-weight: bold; font-size: 1.4em">
                                                                *</span>
                                                        </label>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtUName" CssClass="form-control validate[required,custom[onlyLetterNumber]]"
                                                                runat="server" MaxLength="50" autocomplete="off" AutoPostBack="true" oninput="this.value = this.value.toUpperCase()"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtUName" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="CmdSave" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                  
                                    <div class="col-md-6" id="rwSpnsr" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <div class="form-group">
                                                    <label>
                                                        Upliner ID *</label>
                                                    <asp:TextBox ID="txtUplinerId" runat="server" CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                                                    <asp:Label ID="lblUplnrNm" runat="server" ForeColor="#D11F7B"></asp:Label>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <!-- ================= Leg + Name ================= -->
                                <div class="row">
                                
                                  <div class="col-md-2" id="DivLeg1" runat="server">
                                        <div class="form-group">
                                            <label>
                                                Leg *</label>
                                            <asp:RadioButtonList ID="RbtnLegNo" runat="server" RepeatDirection="Horizontal">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>
                                                <asp:Label ID="LblName" runat="server"></asp:Label>
                                                Name *
                                            </label>
                                            <asp:TextBox ID="txtFrstNm" runat="server" CssClass="form-control validate[required,custom[onlyLetterNumberChar]]">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6" id="divFName" runat="server">
                                        <div class="form-group">
                                            <label>
                                                Father/Husband Name</label>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="CmbType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="S/O">S/O</asp:ListItem>
                                                        <asp:ListItem Value="D/O">D/O</asp:ListItem>
                                                        <asp:ListItem Value="W/O">W/O</asp:ListItem>
                                                        <asp:ListItem Value="C/O">C/O</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:TextBox ID="txtFNm" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- ================= Father Name ================= -->
                                <!-- ================= Contact ================= -->
                                <div class="row">
                                    <div class="col-md-6" runat="server" id="divAddress" visible="false">
                                        <div class="form-group">
                                            <label>
                                                Address</label>
                                            <asp:TextBox ID="txtAddLn1" runat="server" CssClass="form-control" autocomplete="off">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div id="dvpin" runat="server">
                                    <h4>
                                        Contact Detail</h4>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-group">
                                                        <label>
                                                            Pin Code</label>
                                                        <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-group">
                                                        <label>
                                                            State</label>
                                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>
                                                    District</label>
                                                <asp:TextBox ID="ddlDistrict" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:HiddenField ID="HDistrictCode" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-6" id="divcity" runat="server">
                                            <div class="form-group">
                                                <label>
                                                    City</label>
                                                <asp:TextBox ID="ddlTehsil" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:HiddenField ID="HCityCode" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- ================= Mobile + Email ================= -->
                                <div class="row">
                                    <div class="col-md-6" id="divMobile" runat="server">
                                        <div class="form-group">
                                            <label>
                                                Mobile No.
                                                <asp:Label ID="lblmobilestyle" runat="server"></asp:Label>
                                            </label>
                                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" MaxLength="10">
                                            </asp:TextBox>
                                            <asp:Label ID="lblMobileNo" runat="server" ForeColor="#D11F7B"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-6" id="Div2" runat="server">
                                        <div class="form-group">
                                            <label>
                                                E-Mail ID.
                                                <asp:Label ID="lblemailstyle" runat="server"></asp:Label>
                                            </label>
                                            <asp:TextBox ID="txtEMailId" runat="server" CssClass="form-control">
                                            </asp:TextBox>
                                            <asp:Label ID="lblemail" runat="server" ForeColor="#D11F7B"></asp:Label>
                                            <asp:RegularExpressionValidator ID="EmailExpressionValidator" runat="server" ControlToValidate="txtEMailId"
                                                ErrorMessage="Enter Valid Email ID!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ValidationGroup="eInformation">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                <!-- ================= PAN + Aadhar ================= -->
                                <div class="row">
                                    <div class="col-md-6" id="divPan" runat="server">
                                        <div class="form-group">
                                            <label>
                                                PAN No.</label>
                                            <asp:TextBox ID="txtPanNo" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:Label ID="lblpan" runat="server" ForeColor="#D11F7B"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-6" id="divAdhar" runat="server">
                                        <div class="form-group">
                                            <label>
                                                AADHAR No. *</label>
                                            <!-- Main -->
                                            <asp:TextBox ID="TxtAAdhar1" runat="server" CssClass="form-control" MaxLength="16">
                                            </asp:TextBox>
                                            <!-- Hidden but REQUIRED -->
                                            <asp:TextBox ID="TxtAadhar2" runat="server" CssClass="form-control" Style="display: none;">
                                            </asp:TextBox>
                                            <asp:TextBox ID="TxtAadhar3" runat="server" CssClass="form-control" Style="display: none;">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <asp:Label ID="lblErrEpin" runat="server" CssClass="error"></asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group ">
                                    <textarea style="width: 100%; height: 80px; text-align: left;" cols="5" rows="10"
                                        runat="server" visible="false" id="divTermAll" readonly="readonly">
   Terms & Conditions :-
•It is kind advise to you that you promote Business as per actual. Company will not responsible for your miss commitments in the market through any manner. 
•Registration is FREE in our system.
•Company provides you online account as your ID with password. It contains all your legal information , Transaction Balance, Team detail, Bonus details etc.
•Year starts from 1st April every year.
•KYC Documents is mandatory.
•You must sign your application form & submitted in nearest company Branch/office along with one colour passport size photo & copy of self attested ID proof or address proof / PAN No.
                                                </textarea>
                                    <textarea style="width: 100%; height: 80px; text-align: left;" cols="5" rows="10"
                                        runat="server" visible="false" id="divTermVisionAllright" readonly="readonly">
   Terms &amp; Conditions: - (Registration form)
•It is kind declaration to newly registered members that you are informed to promote Business plans as per ARVPL Guidelines. 
•Registration with ARVPL is FREE.
•KYC Documents is mandatory for Registration.
                                                </textarea>
                                    <textarea style="width: 100%; height: 80px; text-align: left;" cols="5" rows="10"
                                        runat="server" visible="false" id="divTermVadic" readonly="readonly">Contract with Direct Sellers/Distributors - VADIC NETWORK PRIVATE LIMITED</textarea>
                                    <textarea style="width: 100%; height: 80px; text-align: left;" cols="5" rows="10"
                                        runat="server" visible="false" id="TxtLife" readonly="readonly">OVERVIEW - Astrolife Limited Terms of Service</textarea>
                                    <textarea style="width: 100%; height: 80px; text-align: left;" cols="5" rows="10"
                                        runat="server" visible="false" id="divTermConditionNIGT" readonly="readonly">
   Terms &amp; Conditions: 
•Registration is free. KYC documents are mandatory.
•Subject to Muzaffarnagar (UP) jurisdiction only.
                                                </textarea>
                                </div>
                                <label>
                                    <center>
                                        <asp:CheckBox ID="chkterms" runat="server" onclick="DivOnOff();" TabIndex="53" />
                                        <font face="Verdana" color="#000000" size="1"><b>I Agree With Terms And Condition</b></font>
                                    </center>
                                </label>
                            </div>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div id="DivTerms" style="display: none">
                                        <asp:Button ID="CmdSave" runat="server" Text="Submit" CssClass="btn btn-primary"
                                            TabIndex="54" />
                                        &nbsp;<asp:Button ID="CmdCancel" runat="server" Text="Cancel" CssClass="btn btn-primary"
                                            ValidationGroup="eCancel" TabIndex="55" Visible="false" />
                                    </div>
                                    <div id="divOtp" runat="server" visible="false">
                                        <div class="col-sm-10">
                                            <div class="form-group">
                                                <label class="control-label col-sm-4">
                                                    OTP Sent on your E-mail Id and Mobile.<span style="color: red  !important; font-weight: bold;
                                                        font-size: 1.4em">*</span></label>
                                                <asp:TextBox ID="TxtPassword" autocomplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:Label ID="lblOTPMsg" runat="server" Visible="false" ForeColor="red"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rbtnbsa" runat="server" ControlToValidate="TxtPassword"
                                                    SetFocusOnError="true" Text="*" ValidationGroup="Submitbtn"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-12 col-md-12">
                                            <asp:Button ID="BtnPassword" runat="server" ValidationGroup="Submitbtn" Text="Submit"
                                                class="btn btn-danger" Width="150px" />
                                            <asp:Button ID="btngenerate" runat="server" Text="Resend Otp" class="btn btn-danger"
                                                Width="150px" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="CmdSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div id="Div1" class="form-group" visible="false" runat="server">
                        <label class="control-label col-sm-2">
                            Phone No.</label>
                        <asp:TextBox ID="txtPhNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                            TabIndex="30" runat="server" MaxLength="10" autocomplete="off"></asp:TextBox>
                    </div>
                    <div id="Div3" class="form-group" runat="server" style="display: none">
                        <label class="control-label col-sm-2">
                            PAN No Available<span style="color: red  !important; font-weight: bold; font-size: 1.4em">*</span></label>
                        <div class="col-sm-10">
                            <asp:RadioButtonList ID="RbtPan" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                RepeatLayout="Table" TabIndex="41">
                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                            <span style="color: red  !important; font-weight: bold; font-size: 1.4em">
                                <asp:Label ID="LblPanNoAvail" runat="server" Text="Payout will deduct 20%, If you not enter PAN NO."></asp:Label></span>
                        </div>
                    </div>
                    <div id="dvname" runat="server" visible="false">
                        <h4>
                            Nominee Detail</h4>
                        <div class="form-group">
                            <label class="control-label col-sm-2">
                                Nominee Name</label>
                            <asp:TextBox ID="txtNominee" CssClass="form-control" TabIndex="32" runat="server"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group greybt">
                            <label class="control-label col-sm-2">
                                Relation</label>
                            <asp:TextBox ID="txtRelation" CssClass="form-control" TabIndex="33" runat="server"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div id="divBankDetail" runat="server">
                            <h4>
                                Bank Detail</h4>
                            <div class="form-group">
                                <label class="control-label col-sm-2">
                                    Account No.</label>
                                <asp:TextBox ID="TxtAccountNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                                    TabIndex="34" runat="server" MaxLength="16" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label class="control-label col-sm-2">
                                    Account Type</label>
                                <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control" TabIndex="21">
                                    <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                    <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-2">
                                    Bank</label>
                                <asp:DropDownList ID="CmbBank" runat="server" CssClass="form-control" TabIndex="36"
                                    onchange="FnBankChange(this.value);" autocomplete="off">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" id="divBank" style="display: none">
                                <label class="control-label col-sm-2">
                                    Bank Name</label>
                                <asp:TextBox ID="TxtBank" CssClass="form-control" TabIndex="37" runat="server" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-2">
                                    Branch Name</label>
                                <asp:TextBox ID="TxtBranchName" CssClass="form-control" TabIndex="38" runat="server"
                                    autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-2">
                                    IFSC Code</label>
                                <asp:TextBox ID="txtIfsCode" runat="server" CssClass="form-control" TabIndex="39"
                                    autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="form-group" visible="false">
                                <asp:TextBox ID="TxtMICR" CssClass="form-control" Visible="false" TabIndex="40" runat="server"
                                    autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div id="divpay" runat="server" visible="false">
                        <h4>
                            Payment Deposit Detail</h4>
                        <div class="form-group">
                            <label class="control-label col-sm-2">
                                Select Paymode</label>
                            <asp:DropDownList ID="DdlPaymode" runat="server" AutoPostBack="true" CssClass="form-control"
                                TabIndex="46" autocomplete="off">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">
                                <asp:Label ID="LblDDNo" runat="server" Text="Draft/CHEQUE No. *"></asp:Label></label>
                            <asp:TextBox ID="TxtDDNo" CssClass="form-control" TabIndex="47" runat="server" MaxLength="15"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">
                                <asp:Label ID="LblDDDate" runat="server" Text="Draft/CHEQUE Date *"></asp:Label></label>
                            <asp:TextBox ID="TxtDDDate" runat="server" TabIndex="48" CssClass="form-control"
                                autocomplete="off"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDDDate"
                                Format="dd-MMM-yyyy">
                            </AjaxToolkit:CalendarExtender>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2">
                                Issued Bank Name</label>
                            <asp:TextBox ID="TxtIssueBank" CssClass="form-control" TabIndex="49" runat="server"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group greybt">
                            <label class="control-label col-sm-2">
                                Issued Bank Branch</label>
                            <asp:TextBox ID="TxtIssueBranch" CssClass="form-control" TabIndex="50" runat="server"
                                autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div id="divlogin" runat="server" visible="false">
                        <h4>
                            Login Information</h4>
                        <div class="form-group ">
                            <label class="control-label col-sm-2">
                                Password<span style="color: red  !important; font-weight: bold; font-size: 1.4em">*</span></label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="TxtPasswd" class="validate[required,minSize[5],maxSize[10]] form-control"
                                    TabIndex="51" runat="server" TextMode="Password" ValidationGroup="eInformation"
                                    autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div id="Div4" class="form-group" visible="false" runat="server">
                            <label class="control-label col-sm-2">
                                Transaction Password<span style="color: red  !important; font-weight: bold; font-size: 1.4em">*</span></label>
                            <asp:TextBox ID="TxtTransactionPassword" class="validate[required,minSize[5],maxSize[10]] form-control"
                                TabIndex="52" runat="server" TextMode="Password" ValidationGroup="eInformation"
                                autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div id="Dvfld" runat="server" visible="false">
                        <div class="form-group">
                            <label class="control-label col-sm-2">
                                Same As Above<span style="color: red  !important; font-weight: bold; font-size: 1.4em">*</span></label>
                            <asp:CheckBox ID="ChkSame" runat="server" onclick="return GetSameAsPostal()" TabIndex="21" />
                        </div>
                        <h5>
                            Postal Address</h5>
                        <div class="form-group ">
                            <label class="control-label col-sm-2">
                                Address</label>
                            <asp:TextBox ID="TxtPostalAddress" CssClass="form-control" TabIndex="22" runat="server"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <div class="form-group ">
                                    <label class="control-label col-sm-2">
                                        Pin code<span style="color: red  !important; font-weight: bold; font-size: 1.4em">*</span></label>
                                    <asp:TextBox ID="TxtPostPincode" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                        TabIndex="23" runat="server" MaxLength="6" autocomplete="off" AutoPostBack="true"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="CmdSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="form-group ">
                                    <label class="control-label col-sm-2">
                                        State</label>
                                    <asp:TextBox ID="TxtpostState" runat="server" CssClass="form-control" TabIndex="24"
                                        autocomplete="off" Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="HPostStateCode" runat="server" />
                                </div>
                                <div class="form-group ">
                                    <label class="control-label col-sm-2">
                                        District</label>
                                    <asp:TextBox ID="TxtPostDistrict" CssClass="form-control" TabIndex="25" runat="server"
                                        autocomplete="off" Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="HPostDistrict" runat="server" />
                                </div>
                                <div class="form-group ">
                                    <label class="control-label col-sm-2">
                                        City</label>
                                    <asp:TextBox ID="TxtPostCity" CssClass="form-control" TabIndex="26" runat="server"
                                        ValidationGroup="eInformation" autocomplete="off" Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="HPostCity" runat="server" />
                                </div>
                                <div class="form-group ">
                                    <label class="control-label col-sm-2">
                                        Area</label>
                                    <asp:DropDownList ID="DDlPostVillage" CssClass="form-control" TabIndex="27" runat="server"
                                        ValidationGroup="eInformation" autocomplete="off" onchange="FnPostVillageChange(this.value);">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="HPostVillage" runat="server" />
                                </div>
                                <div class="form-group" id="divPostVillage" style="display: none">
                                    <label class="control-label col-sm-2">
                                        Area Name</label>
                                    <asp:TextBox ID="TxtPostVillage" CssClass="form-control" TabIndex="28" runat="server"
                                        autocomplete="off"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TxtPostPincode" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="CmdSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group " style="display: none">
                        <label class="control-label col-sm-2">
                            Area</label>
                        <div class="col-sm-10">
                            <asp:DropDownList ID="DDlVillage" CssClass="form-control" TabIndex="19" runat="server"
                                ValidationGroup="eInformation" autocomplete="off" onchange="FnVillageChange(this.value);">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="divVillage" style="display: none">
                        <label class="control-label col-sm-2">
                            Area Name</label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="TxtVillage" CssClass="form-control" TabIndex="20" runat="server"
                                autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" id="TrPrtnrCap" style="display: none">
                        <label class="control-label col-sm-2">
                            <asp:Label ID="LblPartnerName" runat="server" Text="Partner Name Seperated By Comma(,)"></asp:Label></label>
                    </div>
                    <div class="form-group greybt" visible="false" runat="server" id="Divdob">
                        <label class="control-label col-sm-2">
                            <asp:Label ID="LblRegistDate" runat="server" Text="Date Of Birth"></asp:Label></label>
                        <div class="row">
                            <div class="col-sm-4  p0 pl10">
                                <asp:DropDownList ID="ddlDOBdt" runat="server" CssClass="form-control" TabIndex="9"
                                    autocomplete="off">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-4  p0 pl10">
                                <asp:DropDownList ID="ddlDOBmnth" runat="server" CssClass="form-control" TabIndex="10"
                                    autocomplete="off">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-4  p0 pl10">
                                <asp:DropDownList ID="ddlDOBYr" runat="server" Style="padding-right: 30px;" CssClass="form-control"
                                    TabIndex="11" autocomplete="off">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="Div23" visible="false" runat="server">
                        <label class="control-label col-sm-2">
                            <span style="color: red  !important; font-weight: bold; font-size: 1.4em">*</span>
                        </label>
                        <asp:RadioButtonList ID="RbtMarried" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" TabIndex="12" onchange="return GetSelectedItem()" autocomplete="off">
                            <asp:ListItem Text="Married" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="UnMarried" Value="N"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="form-group greybt" id="divMarriageDate" visible="false" style="display: none;">
                        <label class="control-label col-sm-2">
                            Marriage Date</label>
                        <div class="row">
                            <div class="col-sm-4  p0 pl10">
                                <asp:DropDownList ID="DDlMDay" runat="server" CssClass="form-control" TabIndex="13">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-4  p0 pl10">
                                <asp:DropDownList ID="DDLMMonth" runat="server" CssClass="form-control" TabIndex="14">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-4  p0 pl10">
                                <asp:DropDownList ID="DDLMYear" runat="server" Style="padding-right: 30px;" CssClass="form-control"
                                    TabIndex="15">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="CompName" style="display: none">
                        <label class="control-label col-sm-2">
                            Company Name</label>
                        <asp:TextBox ID="TxtCompanyName" runat="server" CssClass="form-control" TabIndex="16"></asp:TextBox>
                    </div>
                    <div class="form-group" id="CompRegistrationNo" style="display: none">
                        <label class="control-label col-sm-2">
                            Company Registration No</label>
                        <asp:TextBox ID="TxtRegistrationNo" runat="server" CssClass="form-control" TabIndex="17"></asp:TextBox>
                    </div>
                    <h4 visible="false" runat="server">
                        Personal Detail</h4>
                    <div id="dvreg" runat="server" visible="false">
                        <div class="form-group ">
                            <label class="control-label col-sm-2">
                                Registration As</label>
                            <asp:RadioButtonList ID="RbCategory" runat="server" RepeatDirection="Horizontal"
                                TabIndex="4" onchange="return GetRegistrationAs()">
                                <asp:ListItem Text="Individual" Value="IN" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Company" Value="C"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="form-group" id="RegType" style="display: none">
                            <label class="control-label col-sm-2">
                                <asp:Label ID="LblRegType" Text="Registration Type" runat="server"></asp:Label></label>
                            <asp:RadioButtonList ID="CbSubCategory" runat="server" TabIndex="5" RepeatDirection="Horizontal"
                                onchange="return GetRegistrationType()">
                                <asp:ListItem Text="ProprietorShip" Value="SP" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Partnership Firm" Value="PF"></asp:ListItem>
                                <asp:ListItem Text="Private Limited Company" Value="PL"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
