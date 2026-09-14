<%@ Page Language="VB" MasterPageFile="~/MasteMain.master" AutoEventWireup="false"
    CodeFile="KitProductMaster.aspx.vb" Inherits="App_UI_Application_Pages_KitProductMaster"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }  
    </script>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <script type="text/javascript">
        var jq = $.noConflict();
        jq(document).ready(function() {
            //Enable Disable all TextBoxes when Header Row CheckBox is checked.  
            jq("[id*=chkSelectAll]").bind("click", function() {
                var chkSelectAll = jq(this);
                //Find and reference the GridView.  
                var grid = jq(this).closest("table");
                //Loop through the CheckBoxes in each Row.  
                jq("td", grid).find("input[type=checkbox]").each(function() {
                    //If Header CheckBox is checked.  
                    //Then check all CheckBoxes and enable the TextBoxes.  
                    if (chkSelectAll.is(":checked")) {
                        jq(this).attr("checked", "checked");
                        var td = jq("td", jq(this).closest("tr"));
                        td.css({
                            "background-color": "#D8EBF2"
                        });
                        jq("input[type=text]", td).removeAttr("disabled");
                        jq("input[type=text]", td).focus();
                    } else {
                        jq(this).removeAttr("checked");
                        var td = jq("td", jq(this).closest("tr"));
                        td.css({
                            "background-color": "#FFF"
                        });
                        jq("input[type=text]", td).attr("disabled", "disabled");
                        jq("input[type=text]", td).val('0');
                        var row = jq(this).closest("tr");
                        jq("[id*=LblAmt]", row).html('0');


                        var updtval = jq(this).val();

                        var grandTotal = 0;
                        var grandbv = 0;


                        jq("[id*=LblAmt]").each(function() {
                            grandTotal = grandTotal + parseFloat(jq(this).html());

                        });
                        jq("[id*=LblBV1]").each(function() {
                            grandbv = grandbv + parseFloat(jq(this).html());

                        });
                        grandbv = parseFloat(grandbv).toPrecision(2);

                        jq("[id*=LblTotalAmount1]").html(grandTotal.toString());

                        jq(this).attr('value', updtval.toString());
                    }
                });
            });

            jq("[id*=chkProd]").bind("click", function() {

                //Find and reference the GridView.
                var grid = jq(this).closest("table");

                //Find and reference the Header CheckBox.
                var chkHeader = jq("[id*=chkSelectAll]", grid);

                //If the CheckBox is Checked then enable the TextBoxes in thr Row.
                if (!jq(this).is(":checked")) {
                    var td = jq("td", jq(this).closest("tr"));
                    td.css({ "background-color": "#FFF" });
                    jq("input[type=text]", td).attr("disabled", "disabled");
                    jq("input[type=text]", td).val('0');
                    var row = jq(this).closest("tr");
                    jq("[id*=LblAmt]", row).html('0');

                    var updtval = jq(this).val();

                    var grandTotal = 0;
                    var grandbv = 0;


                    jq("[id*=LblAmt]").each(function() {
                        grandTotal = grandTotal + parseFloat(jq(this).html());

                    });



                    jq("[id*=LblTotalAmount1]").html(grandTotal.toString());

                    jq(this).attr('value', updtval.toString());

                } else {
                    var td = jq("td", jq(this).closest("tr"));
                    td.css({ "background-color": "#D8EBF2" });
                    jq("input[type=text]", td).removeAttr("disabled");

                }

                //Enable Header Row CheckBox if all the Row CheckBoxes are checked and vice versa.
                if (jq("[id*=chkProd]", grid).length == jq("[id*=chkProd]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                } else {
                    chkHeader.removeAttr("checked");
                }
            });
        });



        jq("[id*=TxtQty]").live("keyup", function() {
            if (!jQuery.trim(jq(this).val()) == '') {

                if (!isNaN(parseFloat(jq(this).val()))) {
                    var row = jq(this).closest("tr");

                    jq("[id*=LblAmt]", row).html(parseFloat(jq(".rate", row).html()) * parseFloat(jq(this).val()));

                }
            }
            else {
                jq(this).val('');
                var row = jq(this).closest("tr");
                jq("[id*=LblAmt]", row).html('0');



            }

            var updtval = jq(this).val();
            var grandTotal = 0;
            var grandbv = 0;


            jq("[id*=LblAmt]").each(
            function() {
                grandTotal = grandTotal + parseFloat(jq(this).html());

            });



            jq("[id*=LblTotalAmount1]").html(grandTotal.toString());

            jq(this).attr('value', updtval.toString());




        });


       

        
         

    
    
    </script>

    <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right_col" role="main">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Kit Product Master</h2>
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
                                    <table>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lblText" runat="server" Text="Select Kit : " Font-Bold="True"></asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlGroup" runat="server" class="form-control" AutoPostBack="true"
                                                    Style="width: 150px; text-indent: 1px;" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <%--<td><asp:Button ID="btnShow" runat="server" CssClass="Btn" Text="Show" /></td>--%>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" />
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                    </table>
                                </div>
                                <div align="center">
                                    <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
                                </div>
                                <div>
                                    <div style="float: right; padding-right: 10px" runat="server" id="DvTota1s" visible="true"
                                        class="col-md-12">
                                        Total Amount:
                                        <asp:Label runat="server" ID="LblTotalAmount1" Font-Bold="true"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;<%-- Total Discount:--%>
                                        <%-- <asp:Label runat="server" ID="LblTotalBv1" Font-Bold="true"></asp:Label>--%>
                                    </div>
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        AllowPaging="false" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        PagerStyle-CssClass="pgr" ShowHeader="true" PageSize="20" EmptyDataText="No data to display.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="MenuId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProdId" runat="server" Text='<%# Eval("ProdId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="70px">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkProd" runat="server" Checked='<%# Eval("Status") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ProductCode" HeaderText="ProductCode" />
                                            <asp:BoundField DataField="ProductName" HeaderText="ProductName" />
                                            <asp:TemplateField HeaderText="Barcode" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDlBarcode" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtQty" runat="server" Text='<%# Eval("Qty") %>' AutoComplete="Off"
                                                        MaxLength="3" onkeypress="return isNumberKey(event);"></asp:TextBox></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MRP">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="LblRate" CssClass="rate" Text='<%# Eval("MRP") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="Discount Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtDisc" runat="server" Text='<%# Eval("DiscAmt") %>'></asp:TextBox></ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                              <asp:TemplateField HeaderText="Total Amount(Rs.)" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="LblAmt" Text='<%# Eval("MRP")* Eval("Qty") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                            <asp:TemplateField HeaderText="Tax" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Tax" runat="server" Text='<%# Eval("Tax") %>'></asp:TextBox></ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
