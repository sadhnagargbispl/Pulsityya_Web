<%@ Page Title="" Language="VB" MasterPageFile="MasteMain.master" AutoEventWireup="false"
    CodeFile="profileWithPostal.aspx.vb" Inherits="profileWithPostal" %>

<asp:Content ID="Content2DT831731" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function Validation() {
            var a = document.getElementById('<%= txtPanNo.ClientId %>').value;
            if (a == "") {
                return true;
            }
            else {
                var regex1 = /^[A-Z]{5}\d{4}[A-Z]{1}$/;  //this is the pattern of regular expersion
                if (regex1.test(a) == false) {
                    alert('Please enter valid pan number');
                    return false;
                }
            }
        }
    </script>

    <script type="text/javascript">
        function Validation1() {
            var a = document.getElementById('<%= TxtIfsCode.ClientId %>').value;
            if (a == "") {
                return true;
            }
            else {
                var regex1 = / ^[A-Za-z]{4}[0][a-zA-Z0-9]{6}$/;  //this is the pattern of regular expersion
                if (regex1.test(a) == false) {
                    alert('Please enter valid IFSC Code');
                    return false;
                }
            }
        }
    </script>

    <script type="text/javascript" src="assets/jquery.min.js">
    </script>

    <%--   <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>--%>

    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>

    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var jq = $.noConflict();
        function pageLoad(sender, args) {
            jq(document).ready(function() {

                jq("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
            });

            jq("#<%=btnSubmit.ClientID %>").click(function() {


                var valid = jq("#aspnetForm").validationEngine('validate');
                var vars = jq("#aspnetForm").serialize();
                if (valid == true) {
                    return true;

                } else {
                    return false;
                }
            });
        }     
   

    </script>

    <script type="text/javascript">
        function FnVillageChange(val) {

            if (val == "381264") {

                document.getElementById("divVillage").style.display = "block";

            }
            else {
                document.getElementById("divVillage").style.display = "none";
            }

        }
    </script>

    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }
        .modal1
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center1
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center1 img
        {
            height: 128px;
            width: 128px;
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
                            Update Profile</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        Enter Member ID :</div>
                                    <div class="col-md-3">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtMemberId" runat="server" class="form-control"></asp:TextBox><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId"
                                                    runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-3">
                                                <asp:Button ID="btnShowMemDetail" runat="server" Text="Show Detail" class="btn btn-primary" />
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtMemberId" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="updatepanel3" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-2">
                                                <asp:button id="btnSubmit" runat="server" text="Update" class="btn btn-primary" visible="False"
                                                    validationgroup="Save" xmlns:asp="#unknown" />
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="updatepanel5" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-2">
                                                <asp:Button ID="Btncancel" runat="server" Text="Cancel" class="btn btn-primary" Enabled="false" />
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="Btncancel" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px">
                                            </asp:Label></div>
                                        <div id="divDetailSection" runat="server" style="margin-bottom: 30px; margin-right: 20px;">
                                            <div class="col-md-12">
                                                <div class="col-md-3">
                                                </div>
                                                <div class="col-md-6">
                                                    <h5 style="color: Black">
                                                        <strong>Sponsor Detail </strong>
                                                    </h5>
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                            </div>
                                            <div class="col-md-12" id="tblSpnsr" runat="server" visible="false">
                                                <div class="col-md-2">
                                                    Sponsor ID:</div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="lblUplinerId" class="form-control" runat="server" Enabled="false"></asp:TextBox></div>
                                                </div>
                                                <div class="col-md-2">
                                                    Sponsor Name:</div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="lblUplnrNm" class="form-control" runat="server" Enabled="false"></asp:TextBox></div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-2">
                                                    Sponsor ID:</div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="lblRefralId" class="form-control" runat="server" Enabled="false"></asp:TextBox></div>
                                                <div class="col-md-2">
                                                    Sponsor Name:</div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="lblRefralNm" class="form-control" runat="server" Enabled="false"></asp:TextBox></div>
                                            </div>
                                            <div class="col-md-12" id="trpinpoin" runat="server" visible="false">
                                                <div class="col-md-2">
                                                    Pin Point:</div>
                                                <div class="col-md-4">
                                                    <asp:RadioButtonList ID="RbtPinPoint" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="True" Text="Yes" Value="Y"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <div class="col-md-3">
                                                </div>
                                                <div class="col-md-6">
                                                    <h5 style="color: Black">
                                                        <strong>Personal Detail </strong>
                                                    </h5>
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <br />
                                                <div class="col-md-2">
                                                    Member Name :<span class="red">*</span>
                                                </div>
                                                <div class="col-md-1" style="padding-left: 1PX">
                                                    <asp:DropDownList class="form-control" ID="CmbPrefix" runat="server" TabIndex="7">
                                                        <asp:ListItem Value="Mr." Text="Mr." Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="Mrs." Text="Mrs."></asp:ListItem>
                                                        <asp:ListItem Value="Miss" Text="Miss"></asp:ListItem>
                                                        <asp:ListItem Value="M/S." Text="M/S."></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="MemfirstName" CssClass="form-control validate[required,custom[onlyLetterNumberChar]]"
                                                        runat="server" ForeColor="black" Font-Bold="true" Font-Size="12px" TabIndex="8"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnFormno" runat="server" />
                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="MemfirstName"
                                                runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="col-md-2">
                                                    Father's Name :</div>
                                                <div class="col-md-1" style="padding-left: 1PX">
                                                    <asp:DropDownList class="form-control" ID="CmbType" runat="server" TabIndex="9">
                                                        <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
                                                        <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
                                                        <asp:ListItem Value="D/O" Text="D/O"></asp:ListItem>
                                                        <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtFNm" runat="server" TabIndex="10" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                        ForeColor="black" Font-Bold="true" Font-Size="12px"></asp:TextBox></div>
                                            </div>
                                            <div class="col-md-12">
                                                <br />
                                                <div class="col-md-2" id="divDOBLabel" runat="server">
                                                    Date of Birth:</div>
                                                <div class="col-md-4" id="divDOBSection" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDOBdt" runat="server" class="form-control" AutoPostBack="true"
                                                                    TabIndex="11">
                                                                     <asp:ListItem Text="-SELECT DAY-" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDOBmnth" runat="server" class="form-control" AutoPostBack="true"
                                                                    TabIndex="12">
                                                                     <asp:ListItem Text="-SELECT MONTH-" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDOBYr" runat="server" class="form-control" AutoPostBack="true"
                                                                    TabIndex="13">
                                                                    <asp:ListItem Text="-SELECT YEAR-" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="col-md-6">
                                                    </div>
                                                </div>
                                                <div class="col-md-12" style="display: none">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <h5 style="color: Black">
                                                            <strong>Address Detail </strong>
                                                        </h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <h5 style="color: Black">
                                                            <strong>Address Detail </strong>
                                                        </h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="col-md-2">
                                                    Address :
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtAddLn1" CssClass="form-control" TabIndex="14"
                                                        runat="server">
                                                    </asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    Pincode:
                                                </div>
                                                <div class="col-md-4">
                                                    <%--<asp:TextBox ID="txtPinCode" CssClass="form-control validate[custom[pincode]]" AutoPostBack="true"
                                                        TabIndex="15" runat="server"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtPinCode" CssClass="form-control validate[custom[pincode]]" 
                                                        TabIndex="15" runat="server"></asp:TextBox>
                                                        </div>
                                                <%--<asp:UpdatePanel ID="Update1" runat="server">
                                                    <ContentTemplate>--%>
                                                        <div class="col-md-12">
                                                            <br />
                                                            <div class="col-md-2">
                                                                State:</div>
                                                            <div class="col-md-4">
                                                                <%--<asp:TextBox ID="txtStateName" runat="server" CssClass="form-control validate[required]"
                                                                    TabIndex="16" autocomplete="off" Enabled="false"></asp:TextBox>
                                                                <asp:HiddenField ID="StateCode" runat="server" />--%>
                                                                <asp:DropDownList ID="ddlstate" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-2">
                                                                District:</div>
                                                            <div class="col-md-4">
                                                                <asp:HiddenField ID="HDistrictCode" runat="server" />
                                                                <asp:TextBox ID="ddlDistrict" CssClass="form-control" TabIndex="17"
                                                                    runat="server"></asp:TextBox></div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <br />
                                                            <div class="col-md-2">
                                                                City:</div>
                                                            <div class="col-md-4">
                                                                <asp:HiddenField ID="HCityCode" runat="server" />
                                                                
                                                                <asp:TextBox ID="ddlTehsil" CssClass="form-control" TabIndex="18"
                                                                    runat="server"></asp:TextBox></div>
                                                                    
                                                            <div class="col-md-2" style=" display:none;">
                                                                Area:</div>
                                                            <div class="col-md-4" style=" display:none;">
                                                                <asp:DropDownList ID="DDlArea" runat="server" class="form-control" TabIndex="19"
                                                                    onchange="FnVillageChange(this.value);">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    <%--</ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="TxtPincode" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>--%>
                                                <div class="col-md-12" id="divVillage" style="display: none">
                                                    <div class="col-md-2">
                                                        Area Name</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtVillage" CssClass="form-control" TabIndex="20" runat="server"
                                                            autocomplete="off"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="col-md-2">
                                                        Address Proof:</div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="DDLAddressProof" runat="server" CssClass="form-control" TabIndex="21">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Address Proof No:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtIdProofNo" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                            TabIndex="22" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                              &nbsp;&nbsp;  &nbsp;&nbsp;  
                                               <div class="form-group">
    <div class="col-sm-6" style="padding-left:0;">
        <label style="color: Black"; font-weight:bold; margin-right:10px; font-size:16px;">
            Same As Above <span class="red">*</span>
        </label>

        <asp:CheckBox ID="ChkSame" runat="server" 
            onclick="return GetSameAsPostal()"
            TabIndex="21"
            style="transform: scale(1.3); margin-left:5px;" />
    </div>
</div> 
                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <h5 style="color: Black">
                                                            <strong>Postal Address Detail </strong>
                                                        </h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="col-md-2">
                                                    Address :
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="TxtPostalAddress" CssClass="form-control" TabIndex="14"
                                                        runat="server">
                                                    </asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    Pincode:
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="TxtPostPincode" CssClass="form-control validate[custom[pincode]]"
                                                        TabIndex="15" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="col-md-2">
                                                        State:</div>
                                                    <div class="col-md-4">
                                                        <%-- <asp:TextBox ID="txtStateName" runat="server" CssClass="form-control validate[required]"
                                                                    TabIndex="16" autocomplete="off" Enabled="false"></asp:TextBox>
                                                                <asp:HiddenField ID="StateCode" runat="server" />--%>
                                                        <asp:DropDownList ID="ddlPostSate" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        District:</div>
                                                    <div class="col-md-4">
                                                        <%-- <asp:HiddenField ID="HDistrictCode" runat="server" />--%>
                                                        <asp:TextBox ID="TxtPostDistrict" CssClass="form-control" TabIndex="17"
                                                            runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="col-md-2">
                                                        City:</div>
                                                    <div class="col-md-4">
                                                        <%--<asp:HiddenField ID="HCityCode" runat="server" />--%>
                                                        <asp:TextBox ID="TxtPostCity" CssClass="form-control" TabIndex="18"
                                                            runat="server"></asp:TextBox></div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <h5 style="color: Black">
                                                            <strong>Contact Detail </strong>
                                                        </h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="col-md-2">
                                                        Mobile No.:<span class="red">*</span></div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtMobileNo" CssClass="form-control validate[required,custom[mobile]]"
                                                            TabIndex="23" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="Dynamic" ControlToValidate="txtMobileNo"
                                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-2">
                                                        E-Mail ID:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtEmailID" CssClass="form-control validate[required,custom[email]]" TabIndex="25"
                                                            runat="server"></asp:TextBox></div>
                                                    <div class="col-md-2" style="display: none">
                                                        Phone No.:</div>
                                                    <div class="col-md-4" style="display: none">
                                                        <asp:TextBox ID="txtPhNo" CssClass="form-control " TabIndex="24" runat="server"></asp:TextBox></div>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div id="divCardNo" runat="server" visible="false">
                                                        <div class="col-md-2">
                                                            Card No.:<span class="red">*</span></div>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtCardNo" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                                TabIndex="23" runat="server" AutoPostBack="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6 ">
                                                        <h5 style="color: Black">
                                                            <strong>Nominee Detail</strong></h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="col-md-2">
                                                        Nominee Name:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="lblNominee" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                            TabIndex="26" runat="server"></asp:TextBox></div>
                                                    <div class="col-md-2">
                                                        Relation:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="lblRelation" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                            TabIndex="27" runat="server"></asp:TextBox></div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <h5 style="color: Black">
                                                            <strong>Bank Detail</strong></h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-2">
                                                        Account Type.:</div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control" TabIndex="28">
                                                            <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                                            <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Account No.:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtAccountNo" class="form-control" TabIndex="29" runat="server"></asp:TextBox></div>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="col-md-2">
                                                        Bank:</div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="CmbBank" class="form-control" runat="server" TabIndex="30">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Branch Name:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtBranchName" CssClass="form-control" TabIndex="31" runat="server"></asp:TextBox></div>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="col-md-2">
                                                        IFSC Code:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtIfsCode" runat="server" CssClass="form-control validate[custom[ifsccode]]"
                                                            TabIndex="32"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Enter Valid IFSC Code"
                                                            ControlToValidate="TxtIfsCode" ValidationGroup="Save" ValidationExpression="[A-Za-z]{4}[0][a-zA-Z0-9]{6}$"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="col-md-2">
                                                        PAN No.:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtPanNo" CssClass="form-control validate[custom[panno]]" TabIndex="33"
                                                            runat="server"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Enter Valid Pan No"
                                                            ControlToValidate="txtPanNo" ValidationExpression="[A-Za-z]{5}\d{4}[A-Za-z]{1}"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" runat="server" id="divGst" visible="false">
                                                    <br />
                                                    <div class="col-md-2">
                                                        GST No.:</div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtGstNo" runat="server" CssClass="form-control" TabIndex="32"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">
                                                    </div>
                                                    <div class="col-md-4">
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <br />
                                                    <div class="col-md-2">
                                                        Password:<span class="red">*</span></div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control validate[required]"
                                                            TabIndex="34"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" Display="Dynamic" ControlToValidate="TxtPassword"
                                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Transaction Password:<span class="red">*</span>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtTransactionPassword" CssClass="form-control validate[required]"
                                                            TabIndex="35" runat="server"></asp:TextBox></div>
                                                </div>
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                    ShowSummary="False" ValidationGroup="Save" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function GetSameAsPostal() {

            if (document.getElementById("<%= ChkSame.clientid %>").checked == true) {
                document.getElementById("<%= TxtPostalAddress.ClientID %>").value = document.getElementById("<%= txtAddLn1.ClientID %>").value;
                document.getElementById("<%= TxtPostPincode.ClientID %>").value = document.getElementById("<%= TxtPincode.ClientID %>").value;
                document.getElementById("<%= ddlPostSate.ClientID %>").value = document.getElementById("<%= ddlstate.ClientID %>").value;

                document.getElementById("<%= TxtPostDistrict.ClientID %>").value = document.getElementById("<%= ddlDistrict.ClientID %>").value;


                document.getElementById("<%= TxtPostCity.ClientID %>").value =
            document.getElementById("<%= ddlTehsil.ClientID %>").value;

            }
            else {
                document.getElementById("<%= TxtPostalAddress.ClientID %>").value = "";
                document.getElementById("<%= TxtPostPincode.ClientID %>").value = "";
                document.getElementById("<%= ddlPostSate.ClientID %>").value = "0";
                document.getElementById("<%= TxtPostDistrict.ClientID %>").value = "";

                document.getElementById("<%= TxtPostCity.ClientID %>").value = "";




                for (var i = 0; i < x; i++) {

                    rmOption.remove(0);
                }
                
                
                
                

            }
        }
        
      </script>
</asp:Content>
