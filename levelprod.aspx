<%@ Page Language="VB" AutoEventWireup="false" CodeFile="levelprod.aspx.vb" Inherits="levelprod" %>

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
                          <asp:Label ID="LblDate" runat="server" Visible="false"></asp:Label>
                            <div class="x_title">
                                <h2>
                                    Level Product Master</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                            
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                       Select Product :</div>
                                    <div class="col-md-4">
                                            <asp:DropDownList CssClass="form-control" ID="DDlprod" runat="server" style=" margin-left :35%"></asp:DropDownList>
                                        </div>
                                    <div class="col-md-2">
                                       
                                    </div>
                                
                                </div>
                               <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Level  :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                     <div class="col-md-4" style="padding-top: 1%">
                                        Level Bv Comm. :</div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="DDlprod"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Level 1 :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtlevelone" runat="server" style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="DDlprod"
                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        Level 2 :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtleveltwo" runat="server" style=" margin-left :21%"></asp:TextBox></div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        level 3 :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtlevelthr" runat="server" style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                   
 level 4 :
 </div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtlevelfour" runat="server" style=" margin-left :21%"></asp:TextBox>
                                        
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                      level 5 :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtlevelfiv" runat="server"  style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                       level 6 :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtlevelsi" runat="server" style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                              
                                   <div class="col-md-4">
                                   level 7 :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtlevelsev" runat="server" style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                    </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        level 8 :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtlevele" runat="server" style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                        level 9 :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtleveln" runat="server" style=" margin-left :21%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 1%">
                                    <div class="col-md-4">
                                      level 10:</div>
                                    <div class="col-md-6">
                                        <asp:TextBox class="form-control" ID="txtleveltn" runat="server" style=" margin-left :21%"></asp:TextBox>
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
                                        <asp:TextBox ID="txtprodId" runat="server" Visible="false"></asp:TextBox>
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
