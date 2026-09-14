<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false" CodeFile="UploadKyc.aspx.vb" Inherits="UploadKyc" title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div class="right_col" role="main">
 <div class="row">
   <div class="col-md-12" runat="server" id="droputr">
   <div class="col-md-6">
                                           
                                             <div class="form-group">
                                                Member ID :
                                            
                                                <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" TabIndex="2" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="LblMemName" runat="server" Style="text-align: left;"></asp:Label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                    ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                <asp:Label ID="LblKitId" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblNewKitid" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                                            </div>
                                             <div class="form-group">
                                                Choose Kyc Type :
                                                <asp:DropDownList ID="ddlkyctype" runat="server" class="form-control" TabIndex="1" AutoPostBack="true">
                                                    <asp:ListItem Text="Select Kyc Type" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Address Proof" Value="A"></asp:ListItem>
                                                    <asp:ListItem Text="Bank Proof" Value="B"></asp:ListItem>
                                                     <asp:ListItem Text="Pan Card" Value="P"></asp:ListItem>
                                                      <asp:ListItem Text="Upload Form" Value="F"></asp:ListItem>
                                                       <asp:ListItem Text="Upload GSTN" Value="G"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            </div>
                                           
                                        </div>
 </div> 
        <div class="row">
             <div class="col-md-12" id="divAddress" runat="server" visible="false" >
                    <!-- Genex Business -->
                    
                         <div class="x_title">
                            <h2>
                                Address Proof
                            </h2>
                            <div class="clearfix">
                        </div>
                        </div>

                        <div class="col-md-6">
                        
                                           <%-- <div class="form-group">
                                                Member Name :
                                            
                                                </div>--%>
                                            <div class="form-group">
                                <label for="pwd">
                                    Address:</label>
                                <asp:TextBox ID="txtaddrs" runat="server" CssClass="form-control validate[required]"
                                    TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="pincode">
                                    Pincode</label>
                                <asp:TextBox ID="Txtpincode" runat="server" CssClass="form-control validate[required,custom[pincode]]"
                                    TabIndex="4"  autocomplete="off" ></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="pwd">
                                    State:</label>
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" TabIndex="5" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvstate" runat="server" ControlToValidate="ddlState" ErrorMessage="Please Select State"
                                    InitialValue="0" ValidationGroup="eInformation" ></asp:RequiredFieldValidator>
                                <%--   <asp:TextBox ID="txtStateName" runat="server" CssClass="form-control" TabIndex="3"
                                    autocomplete="off" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="StateCode" runat="server" />--%>
                            </div>
                            <div class="form-group">
                                <label for="district">
                                    District</label>
                                <asp:TextBox ID="txtDistrict" CssClass="form-control validate[required]" TabIndex="6"
                                    runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group" id="divcity" runat="server">
                                <label for="city">
                                    City</label>
                                <asp:TextBox ID="txtCity" CssClass="form-control validate[required]" TabIndex="7"
                                    runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="email">
                                    Front Address Proof Upload:</label>
                                <asp:FileUpload ID="Fuidentity" runat="server" class="form-control" TabIndex="8" />
                                <asp:Label ID="lblimage" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label for="pwd">
                                    Back Address Proof Upload:</label>
                                <asp:FileUpload ID="FileUpload1" runat="server" TabIndex="9" class="form-control" />
                                <asp:Label ID="LblBackImage" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label for="addressproof">
                                    Address Proof</label>
                                <asp:DropDownList ID="DDLAddressProof" runat="server" CssClass="form-control" TabIndex="10">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="idproof">
                                    <asp:Label ID="LblAddresProof" runat="server"></asp:Label>
                                </label>
                                <asp:TextBox ID="TxtIdProofNo" CssClass="form-control validate[required]" TabIndex="11"
                                    runat="server"></asp:TextBox>
                            </div>
                          
                                        </div>
                       <div class="col-md-4">
                        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                        <div class="profile-bar-simple red-border clearfix">
                            <h6>
                                Uploaded Images
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

                            <a id="FrontAddress" runat="server" class="fbox" rel="group" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                <asp:Image ID="ShowIdentity" Width="150px" Height="150px" runat="server" />
                                
                            </a>
                             <a id="BackAddress" runat="server" class="fbox" rel="group" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                <asp:Image ID="showBackImage" Width="150px" Height="150px" runat="server" />
                            </a>
                            <%-- </div>
                            </div>
                            <div class="col-md-6">
                                <div class="image">--%>
                           
                            <%--</div>--%>
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
                <!-- end dashboards/dashboard -->
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" Display="None" ControlToValidate="Txtpincode"
                                runat="server" ErrorMessage="PinCode Required" SetFocusOnError="true" ValidationGroup="eInformation"></asp:RequiredFieldValidator>&nbsp;
        </div>
        <div class="col-md-12" id="divbankproof" runat="server" visible="false" >
                    <!-- Genex Business -->
                    
                         <div class="x_title">
                            <h2>
                                Bank Proof
                            </h2>
                            <div class="clearfix">
                        </div>
                        </div>

                        <div class="col-md-6">
                        <div class="form-group">
                                <label for="inputdefault">
                                    Upload Bank Proof</label></div>
                            <div class="form-group">
                                <asp:FileUpload ID="upbank" runat="server" class="form-control validate[required]" />
                                <asp:Label ID="lblbank" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Account Type</label>
                                <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control" TabIndex="1">
                                    <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                    <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Account No:</label>
                                <asp:TextBox ID="Txtacno" runat="server" CssClass="form-control validate[required,custom[onlyNumberSp]]"
                                    TabIndex="2" MaxLength="17"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Bank:</label>
                                <asp:DropDownList ID="cmbbank" runat="server" Class="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" id="divBank" runat="server" visible="false">
                                <label for="inputdefault">
                                    Bank Name</label>
                                <asp:TextBox ID="Txtbank" CssClass="form-control" TabIndex="4" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Branch Name :</label>
                                <asp:TextBox ID="Txtbranch" runat="server" CssClass="form-control validate[required,custom[onlyLetterNumberChar]]"
                                    TabIndex="5"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    IFSC Code :</label>
                                <div class="form-group">
                                    <asp:TextBox ID="Txtcode" runat="server" TabIndex="6" CssClass="form-control validate[required,custom[ifsccode]]"></asp:TextBox>
                                </div>
                            </div>
                         </div>
                       <div class="col-md-4">
                        <div id="Div3" class="clearfix gen-profile-box">
                        <div class="profile-bar-simple red-border clearfix">
                            <h6>
                                Uploaded Images
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

                           <a id="BankProof" runat="server" class="fbox" rel="group" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                <asp:Image ID="Showbankid" Width="130px" Height="130px" Style="margin-left: 30%"
                                    runat="server" />
                            </a>
                            
                        </div>
                         <div class="col-md-12" id="DivBankVerify" runat="server">
                                <br />
                                <asp:Label ID="LblbankVerification" Text="Verification Status :  " Font-Bold="true" runat="server"></asp:Label>
                                <asp:Label ID="lblbankverstatus" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="BankVerifyDate" runat="server" Text="Verify/Reject Date : " Visible="false"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="Lblbankverdate" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="LblbankVerfRemark" Text="Reject Remark : " Visible="false" runat="server"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="LblBankRemark" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="LblbankVerfReason" Text="Reject Reason : " Visible="false" runat="server"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="LbLbankrejectRemark" runat="server"></asp:Label>
                            </div>
                    </div>
                                        </div>
                <!-- end dashboards/dashboard -->
        </div>
        <div class="col-md-12" id="divPanproof" runat="server" visible="false" >
                    <!-- Genex Business -->
                    
                         <div class="x_title">
                            <h2>
                                Pan Proof
                            </h2>
                            <div class="clearfix">
                        </div>
                        </div>

                        <div class="col-md-6">
                        <div class="profile-bar-simple red-border clearfix">
                            <h6>
                                Pan Card <% If Session("CompId")=1007 then %>
                            <span id="panlink" runat="server" visible="false">
                                
                                    (<a href="https://eportal.incometax.gov.in/iec/foservices/#/pre-login/link-aadhaar-status" target="_blank" style=" color :Blue;" >Click here</a> for Link Aadhaar Status)
                            </span>
                            
                            <%End If %>
                            </h6>
                        </div>
                                <div class="form-group">
                                <label for="inputdefault">
                                    Upload :</label>
                                <asp:FileUpload ID="uppan" runat="server" class="form-control validate[required]" />
                                <asp:Label ID="lblpan" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Pan Card No. :</label>
                                <asp:TextBox ID="txtpan" runat="server" CssClass="form-control validate[required,custom[panno]]"></asp:TextBox>          
                          
                                        </div>
                                         </div>
                       <div class="col-md-4">
                        <div id="Div2" class="clearfix gen-profile-box">
                        <div class="profile-bar-simple red-border clearfix">
                            <h6>
                                Uploaded Images
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

                            <div class="image">
                                <a id="PanCard" runat="server" class="fbox" rel="group" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                    <asp:Image ID="ShowpanIdentity" Width="130px" Height="130px" runat="server" Style="margin-left: 30%" />
                                </a>
                                <br />
                            </div>
                        </div>
                        <div class="col-md-12" id="Divpanverify" runat="server">
                                <br />
                                <asp:Label ID="LblpanVerification" Text="Verification Status :  " Font-Bold="true" runat="server"></asp:Label>
                                <asp:Label ID="lblpanverstatus" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="panVerifyDate" runat="server" Text="Verify/Reject Date : " Visible="false"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="Lblpanverdate" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="LblpanVerfRemark" Text="Reject Remark : " Visible="false" runat="server"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="LblpanRemark" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="LblpanVerfReason" Text="Reject Reason : " Visible="false" runat="server"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="LbLpanrejectRemark" runat="server"></asp:Label>
                            </div>
                    </div>
                                        </div>
                <!-- end dashboards/dashboard -->
        </div>
        </div>
        <div class="col-md-12" id="divform" runat="server" visible="false" >
                    <!-- Genex Business -->
                    
                         <div class="x_title">
                            <h2>
                                Form Proof
                            </h2>
                            <div class="clearfix">
                        </div>
                        </div>

                        <div class="col-md-6">
                         <div class="form-group">
                                <label for="email">
                                    Front Page Form Upload:</label>
                                <asp:FileUpload ID="upfrontform" runat="server" class="form-control" TabIndex="8" />
                                <asp:Label ID="lblfrontform" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label for="pwd">
                                    Back Page Form Upload:</label>
                                <asp:FileUpload ID="upbackform" runat="server" TabIndex="9" class="form-control" />
                                <asp:Label ID="lblbackform" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label for="pwd">
                                    Remark
                                </label>
                                <asp:TextBox ID="txtRemark" CssClass="form-control" TextMode="MultiLine" MaxLength="200"
                                    runat="server"></asp:TextBox>
                            </div>
                                          
                          
                                        </div>
                       <div class="col-md-4">
                        <div id="Div6" class="clearfix gen-profile-box">
                        <div class="profile-bar-simple red-border clearfix">
                            <h6>
                                Uploaded Images
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

                            <a id="Frontform" runat="server" class="fbox" rel="group" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                <asp:Image ID="Showfrontform" Width="150px" Height="150px" runat="server" />
                            </a>
                            <a id="Backform" runat="server" class="fbox" rel="group" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                <asp:Image ID="Showbackform" Width="150px" Height="150px" runat="server" />
                            </a>
                        </div>
                        <div class="col-md-12">
                            <div id="Divformverify" runat="server">
                                <br />
                                <asp:Label ID="LblformVerification" Text="Verification Status :  " Font-Bold="true" runat="server"></asp:Label>
                                <asp:Label ID="lblformverstatus" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="formVerifyDate" runat="server" Text="Verify/Reject Date : " Visible="false"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="Lblformverdate" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="LblformVerfRemark" Text="Reject Remark : " Visible="false" runat="server"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="LblformRemark" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="LblformVerfReason" Text="Reject Reason : " runat="server"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="LbLformrejectRemark" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                                        </div>
                <!-- end dashboards/dashboard -->
        </div>
        <div class="col-md-12" id="divgstn" runat="server" visible="false" >
                    <!-- Genex Business -->
                    
                         <div class="x_title">
                            <h2>
                                GSTN Proof
                            </h2>
                            <div class="clearfix">
                        </div>
                        </div>

                        <div class="col-md-6">
                         <div class="form-group">
                                <label for="inputdefault">
                                    Upload :</label>
                                <asp:FileUpload ID="upgst" runat="server" />
                                <asp:Label ID="lblgstimage" runat="server" Visible="false"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                   GST No. :</label>
                                <asp:TextBox ID="txtgst" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please check PAN Format"
                                                        SetFocusOnError="true" ControlToValidate="txtpan" ValidationExpression="[A-Za-z]{5}\d{4}[A-Za-z]{1}"
                                                        ValidationGroup="eInformation"></asp:RegularExpressionValidator>--%>
                            </div>
                                          
                          
                                        </div>
                       <div class="col-md-4">
                        <div id="Div8" class="clearfix gen-profile-box">
                        <div class="profile-bar-simple red-border clearfix">
                            <h6>
                                Uploaded Images
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

                           <div class="image">
                              
                                <a id="GstCard" runat="server" class="fbox" rel="group" onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                    <asp:Image ID="ShowgstIdentity" Width="130px" Height="130px" runat="server" Style="margin-left: 30%" />
                                </a>
                             <%--   <a id="GStPdf" runat="server" target ="_blank" ><asp:Image ID="img1" runat="server"  ImageUrl ="images/pdf.png" ImageAlign="Middle"  /></a>
                          --%>      <br />
                            </div>
                        </div>
                        <div class="col-md-12" id="Divgstverify" runat="server">
                                <br />
                                <asp:Label ID="LblgstVerification" Text="Verification Status :  " Font-Bold="true" runat="server"></asp:Label>
                                <asp:Label ID="lblgstverstatus" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="gstVerifyDate" runat="server" Text="Verify/Reject Date : " Visible="false"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="Lblgstverdate" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="LblgstVerfRemark" Text="Reject Remark : " Visible="false" runat="server"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="LblgstRemark" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="LblgstVerfReason" Text="Reject Reason : " Visible="false" runat="server"
                                    Style="font-weight: bold"></asp:Label>
                                <asp:Label ID="LbLgstrejectRemark" runat="server"></asp:Label>
                            </div>
                    </div>
                                        </div>
                <!-- end dashboards/dashboard -->
        </div>
          <asp:Button ID="BtnIdentity" runat="server" ValidationGroup="eInformation" CssClass="btn btn-primary"
                                Text="submit" OnClientClick="return Page_ClientValidate('eInformation') && SaveButton();" Visible="false" />
        </div>
    </div>
    
   
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>--%>

