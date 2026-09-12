<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="kyc.aspx.cs" Inherits="kyc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script>
        function openPopup(element) {
            var url = element.href;
            hs.htmlExpand(element, {
                objectType: 'iframe',
                width: 620,
                height: 450,
                marginTop: 0
            });
            return false;
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <style>
            .red {
                color: red;
                font-size: 1.5em;
                padding-left: 4px;
                font-weight: bold;
            }
        </style>
        <section class="section">
            <ul class="breadcrumb breadcrumb-style ">
            </ul>
            <div class="row">
                <div class="col-12 col-sm-12 col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>KYC Detail</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <!-- Genex Business -->
                                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                        <div class="profile-bar-simple red-border clearfix">
                                            <h6>KYC Detail
                                            </h6>
                                        </div>
                                        <div class="col-md-12">
                                            Dear
                            <%=Session["MemName"].ToString()%>
                                            <asp:HiddenField ID="hdnSessn" runat="server" />
                                            (<asp:Label ID="lblid" runat="server"></asp:Label>) , Update Your KYC (<asp:Label
                                                ID="LblIdproofText" runat="server"></asp:Label>)
                            <br />
                                        </div>
                                        <div class="col-md-8">
                                            <div class="profile-bar-simple red-border clearfix">
                                                <h6>Address Proof
                                   <% if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
                                       { %>
    (Same as document)
                                                    <% } %>

                                                </h6>
                                            </div>
                                            <div class="form-group">
                                                <label for="pwd">
                                                    Address:</label>
                                                <asp:TextBox ID="txtaddrs" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="pincode">
                                                    Pincode</label>
                                                <asp:TextBox ID="Txtpincode" runat="server" CssClass="form-control validate[required,custom[pincode]]"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="pwd">
                                                    State:</label>

                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                </asp:DropDownList>

                                                <asp:HiddenField ID="StateCode" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label for="district">
                                                    District</label>
                                                <asp:HiddenField ID="HDistrictCode" runat="server" />
                                                <asp:TextBox ID="Txtdistrict" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="city">
                                                    City</label>
                                                <asp:TextBox ID="Txtcity" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                                <asp:HiddenField ID="HCityCode" runat="server" />
                                            </div>
                                            <div class="form-group " style="display: none;">
                                                <label for="inputdefault">
                                                    Area</label>
                                                <asp:DropDownList ID="DDlVillage" CssClass="form-control" runat="server" ValidationGroup="eInformation"
                                                    autocomplete="off" onchange="FnVillageChange(this.value);">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group" id="divVillage" style="display: none">
                                                <label for="inputdefault">
                                                    Area Name</label>
                                                <asp:TextBox ID="TxtVillage" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="addressproof">
                                                    Address Proof</label>
                                                <asp:DropDownList ID="DDLAddressProof" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group">
                                                <label for="idproof">
                                                    <asp:Label ID="LblAddresProof" runat="server"></asp:Label>
                                                </label>
                                                <asp:TextBox ID="TxtIdProofNo" CssClass="form-control validate[required]" runat="server"
                                                    MaxLength="16" AutoPostBack="true" OnTextChanged="TxtIdProofNo_TextChanged"></asp:TextBox>
                                            </div>

                                            <div class="form-group">
                                                <label for="email">
                                                    Front Address Proof Upload:</label>
                                                <asp:FileUpload ID="Fuidentity" runat="server" CssClass="form-control validate[required]" />
                                                <asp:RequiredFieldValidator ID="rfvImage" runat="server" ErrorMessage="Please Select  Front Address proof.!!"
                                                    Enabled="false" ControlToValidate="Fuidentity" ValidationGroup="eInformation">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblimage" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div class="form-group">
                                                <label for="pwd">
                                                    Back Address Proof Upload:</label>
                                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control validate[required]" />
                                                <asp:RequiredFieldValidator ID="rfvImage1" runat="server" ErrorMessage="Please Select  Back Address proof.!!"
                                                    Enabled="false" ControlToValidate="FileUpload1" ValidationGroup="eInformation">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="LblBackImage" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div class="profile-bar-simple red-border clearfix">
                                                <h6>Bank Detail<% if (Session["CompId"].ToString() != null && Session["CompId"].ToString() == "1074")
                                                                   { %>(Cancel cheque)<% } %></h6>
                                            </div>
                                            <div class="form-group" id="Accountype" runat="server" visible="false">
                                                <label for="inputdefault">
                                                    Account Type</label>
                                                <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                                    <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Account No:</label>
                                                <asp:TextBox ID="Txtacno" runat="server" CssClass="form-control validate[required,custom[onlyNumberSp]]"
                                                    MaxLength="20"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Bank:</label>
                                                <asp:DropDownList ID="cmbbank" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group" id="divBank" runat="server" visible="false">
                                                <label for="inputdefault">
                                                    Bank Name</label>
                                                <asp:TextBox ID="Txtbank" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Branch Name :</label>
                                                <asp:TextBox ID="Txtbranch" runat="server" CssClass="form-control validate[required,custom[onlyLetterNumberChar]]"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    IFSC Code :</label>
                                                <div class="form-group">
                                                    <asp:TextBox ID="Txtcode" runat="server" CssClass="form-control validate[required,custom[ifsccode]]"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Bank KYC Upload</label>
                                            </div>
                                            <div class="form-group">
                                                <asp:FileUpload ID="BankKYCFileUpload3" runat="server" CssClass="form-control validate[required]" />
                                                <asp:Label ID="LblBankImage" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div class="profile-bar-simple red-border clearfix">
                                                <h6>PAN Card Detail
                                                </h6>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-group">
                                                        <label for="inputdefault">
                                                            Pan Card No. :</label>
                                                        <%--AutoPostBack ="true"--%>
                                                        <asp:TextBox ID="txtpan" runat="server" CssClass="form-control validate[required,custom[panno]]"
                                                            AutoPostBack="true" OnTextChanged="txtpan_TextChanged"></asp:TextBox>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtpan" EventName="TextChanged" />
                                                    <%-- <asp:AsyncPostBackTrigger ControlID="BtnIdentity" EventName="Click" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    PanCard Upload :</label>
                                                <asp:FileUpload ID="Pankyc" runat="server" CssClass="form-control validate[required]" />
                                                <asp:Label ID="LblPanImage" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <asp:Button ID="BtnIdentity" runat="server" ValidationGroup="eInformation" CssClass="btn btn-primary"
                                                Text="submit" OnClientClick="return SaveButton();" OnClick="BtnIdentity_Click" />
                                        </div>
                                    </div>
                                    <!-- end dashboards/dashboard -->
                                </div>
                                <div class="col-md-4">
                                    <!-- Genex Business -->
                                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                        <div class="profile-bar-simple red-border clearfix">
                                            <h6>Uploaded Images
                                            </h6>
                                        </div>
                                        <div class="col-md-12">
                                            <%-- <div class="col-md-6">
                                <div class="image">--%>

                                            <script src="popupassets/popper.min.js"></script>

                                            <script src="popupassets/lib.js"></script>

                                            <script src="popupassets/jquery.flagstrap.min.js"></script>

                                            <script type="text/javascript" src="popupassets/jquery.themepunch.tools.min.js"></script>

                                            <script type="text/javascript" src="popupassets/jquery.themepunch.revolution.min.js"></script>

                                            <script src="js/functions1.js"></script>
                                            <div class="col-md-12">
                                                Front Address Proof<br />
                                                <a id="FrontAddress" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="ShowIdentity" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>

                                            <div class="col-md-12">
                                                Back Address Proof<br />
                                                <a id="BackAddress" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="showBackImage" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>

                                            <div class="col-md-12">
                                                Bank Address Proof<br />
                                                <a id="BankProof" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="bANKiMAGE" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>

                                            <div class="col-md-12">
                                                Pan Card<br />
                                                <a id="PanCard" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="pANiMAGE" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>

                                        </div>
                                        <div class="col-md-12">
                                            <div id="DivVerify" runat="server">
                                                <br />
                                                <asp:Label ID="LblVerification" Text="Verification Status :  " Font-Bold="true" runat="server"></asp:Label>
                                                <asp:Label ID="lblverstatus" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="VerifyDate" runat="server" Text="Verify/Reject Date : " Visible="false"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="Lblverdate" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblVerfRemark" Text="Reject Remark : " Visible="false" runat="server"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="LblRemark" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblVerfReason" Text="Reject Reason : " Visible="false" runat="server"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="LbLrejectRemark" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </section>


    </div>
    <div class="modal fade" id="imagePreviewModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Image Preview</h5>
                    <button type="button"
                        class="btn-close"
                        data-bs-dismiss="modal">
                    </button>
                </div>

                <div class="modal-body text-center">
                    <img id="modalImage"
                        class="img-fluid"
                        style="max-height: 500px;" />
                </div>

            </div>
        </div>
    </div>
    <script>

        // Pehle se server par uploaded ASLI image hai ya sirf placeholder — src normalize karke check.
        // src me se saare hyphen/underscore/space/dot hata kar match karte hain, taaki
        // "Photo-Not-Available.png" jaisa placeholder bhi sahi pakda jaye.
        function hasExistingImage(imgClientId) {
            var src = $("#" + imgClientId).attr("src");
            if (!src || src.trim() == "") return false;

            // Normalize: sirf letters/digits rakho (hyphen, underscore, space, dot sab hata do)
            var norm = src.toLowerCase().replace(/[^a-z0-9]/g, "");

            // Placeholder / no-image patterns -> "koi image nahi" maano
            if (norm.indexOf("notavailable") > -1 ||   // photo-not-available
                norm.indexOf("photonot") > -1 ||
                norm.indexOf("nophoto") > -1 ||
                norm.indexOf("noimage") > -1 ||
                norm.indexOf("noimg") > -1 ||
                norm.indexOf("default") > -1 ||
                norm.indexOf("dummy") > -1 ||
                norm.indexOf("blank") > -1 ||
                norm.indexOf("placeholder") > -1) {
                return false;
            }
            return true;
        }

        function SaveButton() {

            if ($("#<%=txtaddrs.ClientID%>").val().trim() == "") {
                alert("Please enter Address");
                $("#<%=txtaddrs.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Txtpincode.ClientID%>").val().trim() == "") {
                alert("Please enter Pincode");
                $("#<%=Txtpincode.ClientID%>").focus();
                return false;
            }

            // State: "--Choose State Name--" ya khaali/0 par rok
            <%--var stateVal = $("#<%=ddlState.ClientID%>").val();
            var stateTxt = $("#<%=ddlState.ClientID%> option:selected").text().toLowerCase();
            if (stateVal == null || stateVal == "" || stateVal == "0" || stateTxt.indexOf("choose") > -1) {
                alert("Please select State");
                $("#<%=ddlState.ClientID%>").focus();
                return false;
            }--%>
            // State: default "--Choose State Name--" par hi rok, valid state par nahi
            var stateVal = $("#<%=ddlState.ClientID%>").val();
            var stateTxt = $.trim($("#<%=ddlState.ClientID%> option:selected").text());
            if (stateVal == null || $.trim(stateVal) == "" ||
                stateTxt == "" ||
                stateTxt.indexOf("--") > -1 ||          // "--Choose State Name--" jaisa default
                stateTxt.toLowerCase() == "choose state name") {
                alert("Please select State");
                $("#<%=ddlState.ClientID%>").focus();
                return false;
            }
            // District: khaali ya "0" invalid
            var distVal = $("#<%=Txtdistrict.ClientID%>").val().trim();
            if (distVal == "" || distVal == "0") {
                alert("Please enter District");
                $("#<%=Txtdistrict.ClientID%>").focus();
                return false;
            }

            // City: khaali ya "0" invalid
            var cityVal = $("#<%=Txtcity.ClientID%>").val().trim();
            if (cityVal == "" || cityVal == "0") {
                alert("Please enter City");
                $("#<%=Txtcity.ClientID%>").focus();
                return false;
            }

            if ($("#<%=TxtIdProofNo.ClientID%>").val().trim() == "") {
                alert("Please enter Address Proof Number");
                $("#<%=TxtIdProofNo.ClientID%>").focus();
                return false;
            }

            // Front: naya file na ho AUR pehle se saved image bhi na ho -> tabhi rok
            if ($("#<%=Fuidentity.ClientID%>").val() == "" && !hasExistingImage("<%=ShowIdentity.ClientID%>")) {
                alert("Please upload Front Address Proof");
                $("#<%=Fuidentity.ClientID%>").focus();
                return false;
            }

            // Back: naya file na ho AUR pehle se saved image bhi na ho -> tabhi rok
            if ($("#<%=FileUpload1.ClientID%>").val() == "" && !hasExistingImage("<%=showBackImage.ClientID%>")) {
                alert("Please upload Back Address Proof");
                $("#<%=FileUpload1.ClientID%>").focus();
                return false;
            }
            // Account Type: sirf tab check karo jab field page par visible ho
            var $acctType = $("#<%=DDLAccountType.ClientID%>");
            if ($acctType.length > 0 && $acctType.is(":visible")) {
                if ($acctType.val() == "" || $acctType.val() == "0") {
                    alert("Please select Account Type");
                    $acctType.focus();
                    return false;
                }
            }
            if ($("#<%=Txtacno.ClientID%>").val().trim() == "") {
                alert("Please enter Account Number");
                $("#<%=Txtacno.ClientID%>").focus();
                return false;
            }

            if ($("#<%=cmbbank.ClientID%>").val() == "" || $("#<%=cmbbank.ClientID%>").val() == "0") {
                alert("Please select Bank");
                $("#<%=cmbbank.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Txtbranch.ClientID%>").val().trim() == "") {
                alert("Please enter Branch Name");
                $("#<%=Txtbranch.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Txtcode.ClientID%>").val().trim() == "") {
                alert("Please enter IFSC Code");
                $("#<%=Txtcode.ClientID%>").focus();
                return false;
            }

            // Bank KYC: naya file na ho AUR pehle se saved image bhi na ho -> tabhi rok
            if ($("#<%=BankKYCFileUpload3.ClientID%>").val() == "" && !hasExistingImage("<%=bANKiMAGE.ClientID%>")) {
                alert("Please upload Bank KYC");
                $("#<%=BankKYCFileUpload3.ClientID%>").focus();
                return false;
            }

            if ($("#<%=txtpan.ClientID%>").val().trim() == "") {
                alert("Please enter PAN Card Number");
                $("#<%=txtpan.ClientID%>").focus();
                return false;
            }
            // PAN format: 5 letters + 4 digits + 1 letter (e.g. ABCDE1234F)
            var panNo = $("#<%=txtpan.ClientID%>").val().trim().toUpperCase();
            if (!/^[A-Z]{5}[0-9]{4}[A-Z]$/.test(panNo)) {
                alert("Invalid PAN Number. Format: ABCDE1234F");
                $("#<%=txtpan.ClientID%>").focus();
                return false;
            }
            $("#<%=txtpan.ClientID%>").val(panNo);

            // PAN: naya file na ho AUR pehle se saved image bhi na ho -> tabhi rok
            if ($("#<%=Pankyc.ClientID%>").val() == "" && !hasExistingImage("<%=pANiMAGE.ClientID%>")) {
                alert("Please upload PAN Card");
                $("#<%=Pankyc.ClientID%>").focus();
                return false;
            }

            return true;

        }

    </script>
    <script>
        function openPopup(anchor) {

            // anchor ke andar jo img hai uska src lo
            var img = anchor.querySelector("img");
            if (!img || !img.src) return false;

            var modalImg = document.getElementById("modalImage");

            // reset + set src (cache bypass)
            modalImg.src = "";
            modalImg.src = img.src + "?t=" + new Date().getTime();

            // show modal
            var modal = new bootstrap.Modal(
                document.getElementById("imagePreviewModal")
            );
            modal.show();

            return false; // important (page reload roke)
        }
    </script>

</asp:Content>
