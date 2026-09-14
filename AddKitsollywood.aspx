<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddKitsollywood.aspx.vb" Inherits="App_UI_Application_Pages_AddKitsollywood" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />
    <title></title>
    <link href="css/Main.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="container body">
        <div class="main_container">
            <div class="right_col" role="main">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>
                                    Kit Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <% If Session("CompID") = "1033" Then%>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Plan :</div>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DDlPlan" runat="server" CssClass ="form-control"  style=" margin-left :60%">
                                        <asp:ListItem Text="Choose Plan" Value="0" Selected ="True"></asp:ListItem>
                                        <asp:ListItem Text="Plan A" Value ="1" ></asp:ListItem>
                                        <asp:ListItem Text="Plan B" Value="2"></asp:ListItem></asp:DropDownList>                                  </div>
                                    <div class="col-md-2">
                                       
                                    </div>
                                </div>
                                <% End If%>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Kit Name :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtkitName" runat="server" style=" margin-left :27%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtkitName"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Kit Amount :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtKitAmt" runat="server" style=" margin-left :21%"></asp:TextBox></div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Join Amount :&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtJoinAmt" runat="server" style=" margin-left :12%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                       Kit Unit : 
                                       </div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtKitUnit" runat="server" style=" margin-left :34%"></asp:TextBox>
                                        <asp:Label ID="LblKitDate" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Serial Start :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtSerialStart" runat="server" ReadOnly="true" style=" margin-left :22%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Ref. Income :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtRefIn" runat="server" style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Pool Income :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtPoolIn" runat="server" style=" margin-left :19%"></asp:TextBox></div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Spill Income :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtSpillIn" runat="server" style=" margin-left :18%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Binary Income :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtBinaryIn" runat="server" style=" margin-left :10%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        B.V. :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtBV" runat="server" style=" margin-left :45%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        P.V :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtPV" runat="server" style=" margin-left :46%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        R.P. :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtRP" runat="server" style=" margin-left :45%"></asp:TextBox></div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                 
                                
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Capping :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtCapping" runat="server" style=" margin-left :30%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Top Up Sequence :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="TxtTopUp" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <%--Smart Card Detail--%>
                            
                                  <% If Session("Compid") = "1010" Then%>
                     <div class="col-md-12" style="padding-top: 1%" runat ="server"  id="noofCoupon1">
                                    <div class="col-md-4">
                                      No.of Coupon :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="NoofCoupon" Text="0" runat="server" style=" margin-left :10%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                
                                
                                <div class="col-md-12" style="padding-top: 1%" runat ="server"  id="CouponAmount1">
                                    <div class="col-md-6">
                                        Coupon Amount:</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="CouponAmount" Text="0" runat="server" style=" margin-left :5%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                 <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Reward Point:
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="TxtRewardPonit" Text="0" runat="server" style=" margin-left :13%"></asp:TextBox></div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                       <div class="col-md-12" style="padding-top: 1%" runat ="server"  id="wellcoupon">
                                    <div class="col-md-4">
                                     Well Smart No. of Coupon :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtwellcoupon" Text="" runat="server" style=" margin-left :10%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                
                                
                                <div class="col-md-12" style="padding-top: 1%" runat ="server"  id="wellcouponamt">
                                    <div class="col-md-6">
                                      Well Smart Coupon Amount:</div>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" ID="txtwellcouponamt" Text="" runat="server" style=" margin-left :5%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                        <% End If%>
                                
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Remarks :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtRemarks" runat="server" style=" margin-left :29%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                               
                                 
                                
                                
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Join Color :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                    <div class="col-md-6">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButtonList ID="RbtColor" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Green" Selected="True" Value="Green.jpg"><img src="images/Green.jpg" /></asp:ListItem>
                                            <asp:ListItem Text="Red" Selected="True" Value="Red.jpg"><img src="images/Red.jpg" /></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Upload Package Image :</div>
                                    <div class="col-md-6">
                                        <asp:FileUpload ID="ImageUpload" CssClass="Btn" runat="server" Visible="True" />
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Partner :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtrank1" runat="server" style=" margin-left :29%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Master :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtrank2" runat="server" style=" margin-left :29%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Agency :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtrank3" runat="server" style=" margin-left :29%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Agent :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtrank4" runat="server" style=" margin-left :29%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Emall :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtrank5" runat="server" style=" margin-left :29%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Cash Back :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtcashback" runat="server" style=" margin-left :29%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                            <div class="col-md-4">
                                                Voucher Category :</div>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DDltype" runat="server" class="form-control" Width="180px" >
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Discount :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtdiscount" runat="server" style=" margin-left :29%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Status :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="rdblist" runat="server" Width="150px" CellPadding="0" CellSpacing="10"
                                            RepeatDirection="Horizontal" style="margin-left:16%">
                                            <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                            <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-6">
                                           <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtKitId" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" style="margin-left :100%" />
                                 
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </form>
</body>
</html>
