<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddPopup.aspx.vb" Inherits="App_UI_Application_Pages_AddPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/custom.min.css" rel="stylesheet" />

    <script type="text/javascript">
        function GetSelectedItem() {


            var rb = document.getElementById("<%=Rbtnshow.ClientID%>");


            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");

            for (var i = 0; i < radio.length; i++)
             {

                if (radio[i].checked == true) 
                {
                    if (radio[i].value == "D")
                     {
                        document.getElementById("PImageType").style.display = "block";
                    }
                    else 
                    {
                    var rb1 = document.getElementById("<%=RbtDistributor.ClientID%>");
                     var radio1 = rb1.getElementsByTagName("input");
                     var label1 = rb1.getElementsByTagName("label");
                     radio1[0].checked="A";
                        document.getElementById("PImageType").style.display = "none";
                        document.getElementById("DivDistId").style.display = "none";
                    }

                }
            }



        }

        function GetDistributorItem() {


            var rb = document.getElementById("<%=RbtDistributor.ClientID%>");


            var radio = rb.getElementsByTagName("input");
            var label = rb.getElementsByTagName("label");

            for (var i = 0; i < radio.length; i++) {

                if (radio[i].checked == true) {


                    if (radio[i].value == "S") {
                        document.getElementById("DivDistId").style.display = "block";
                    }
                    else {
                        document.getElementById("DivDistId").style.display = "none";
                    }

                }
            }



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
                                    Popup</h2>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="table-responsive makeitresponsivegrid">
                                    <div align="center">
                                    <div class="clearfix"></div>
                                        <div id="Dvshow" class="col-md-12" runat="server" style="padding: 1%" visible ="false">
                                            <div class="col-md-4">
                                                Show On</div>
                                            <div class="col-md-6">
                                                <asp:RadioButtonList ID="Rbtnshow" runat="server" RepeatDirection="Horizontal" onchange="return GetSelectedItem()">
                                                    <asp:ListItem Text="On Website" Value="W" ></asp:ListItem>
                                                    <asp:ListItem Text="On Distributor" Value="D" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Both" Value="B"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-md-12" id="PImageType" style="padding: 1%; display: none;">
                                            <div class="col-md-4">
                                                Distributor:</div>
                                            <div class="col-md-6">
                                                <asp:RadioButtonList ID="RbtDistributor" runat="server" RepeatDirection="Horizontal" onchange="return GetDistributorItem()">
                                                    <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Selected" Value="S"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="LblImagePath" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-md-12" id="DivDistId" style="padding: 1%; display: none">
                                            <div class="col-md-4">
                                                Distributor Id :</div>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="TxtIdNo" runat="server" Class="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-md-12" id="PimagePath" runat="server" style="padding: 1%">
                                            <div class="col-md-4">
                                                <asp:FileUpload ID="ImageUpload" runat="server" Style="height: 22px" />
                                                <asp:CustomValidator ID="CustomValidator1" OnServerValidate="ValidateFileSize" ForeColor="Red"
                                                    runat="server" />
                                            </div>
                                            <div class="col-md-4" id="PVideo" runat="server" visible="false">
                                              
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                    
                                    <div class="clearfix"></div>
                                    <br />
                                   
                                    <div class="col-md-12" style="padding: 1%">
                                        <div class="col-md-4">
                                            Status:
                                        </div>
                                        <div class="col-md-6">
                                            <asp:RadioButtonList ID="RbtStatus" runat="server" RepeatColumns ="2" RepeatDirection ="Horizontal" >
                                                <asp:ListItem Text="Active" Value="Y" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="col-md-12" style="padding: 1%">
                                        <div class="col-md-4">
                                            <asp:Button ID="BtnUpdate" class="btn btn-primary" Text="Save" runat="server" />
                                        </div>
                                        <div class="col-md-8">
                                        </div>
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
