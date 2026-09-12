<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="DownlinePurchase.aspx.cs" Inherits="DownlinePurchase" %>


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <style>
        .red {
            color: red;
            font-size: 1.5em;
            padding-left: 4px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Downline Purchase</h5>
                        </div>
                        <div class="card-body">
                            <div class="profile-bar clearfix" style="background: #fff;">
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="inputdefault">
                                            Select
                                        </label>
                                        <div class="form-group  pull-none">
                                            <asp:DropDownList ID="RbtProduct" runat="server" TabIndex="3" RepeatColumns="2"
                                                RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RbtProduct_SelectedIndexChanged" CssClass="form-control">
                                                <asp:ListItem Text="Repurchase" Value="R" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="TopUp" Value="T"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group  pull-none">
                                            <label for="inputdefault">
                                                <asp:Label ID="LblLevel" runat="server" Text="Level Wise* "></asp:Label></label>
                                            <asp:DropDownList
                                                ID="DdlLevel"
                                                runat="server"
                                                CssClass="form-control"
                                                TabIndex="1"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="DdlLevel_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:DropDownList ID="RbtLegNo" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                Visible="false" CssClass="form-control">
                                                <asp:ListItem Text="Both" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group  pull-none">
                                            <label for="inputdefault">
                                                From:</label>
                                            <asp:TextBox ID="TxtFromDate" CssClass="form-control" TabIndex="2" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtFromDate"
                                                Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtFromDate"
                                                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group  pull-none">
                                            <label for="inputdefault">
                                                To :</label>
                                            <asp:TextBox ID="TxtToDate" CssClass="form-control" TabIndex="3" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtToDate"
                                                Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate"
                                                ErrorMessage="Invalid To Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-sm-12 pull-none">
                                        <div class="form-group  pull-none">
                                            <asp:Button ID="BtnSearch" Text="Search" runat="server" class="btn btn-primary" OnClick="BtnSearch_Click" />
                                            <asp:Button ID="BtnExportToExcel" runat="server" Text="Export To Excel" class="btn btn-primary" OnClick="BtnExportToExcel_Click" />
                                        </div>
                                        <div class="form-group col-sm-9">
                                        </div>
                                    </div>
                                </div>

                                <div id="divtotal" runat="server" visible="false" class="single-line" style="display: none;">
                                    <b>Total BV:
                                    </b>
                                    <asp:Label ID="lblTotalBV" Font-Bold="true" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <b>Total PV:
                                    </b>
                                    <asp:Label ID="lblTotalBV1" Font-Bold="true" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                                <div class="row" id="divT" runat="server" visible="false" style="display: none;">
                                    <div class="col-md-12 total-summary">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Total Record :"></asp:Label>
                                        <asp:Label ID="LblttlRcd" Font-Bold="true" runat="server" Style="font-weight: bold; font-size: 14px;"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%-- <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Left <%=  Session("ColName1") %>:"></asp:Label>--%>
                                        <span class="me-4">Left PV:

                                    <asp:Label ID="Lblleft" Font-Bold="true" runat="server"></asp:Label>
                                        </span>

                                        <%-- <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Right <%=  Session("ColName1") %>:"></asp:Label>--%>
                                        <span class="me-4">Right BV:

                                    <asp:Label ID="Lblright" Font-Bold="true" runat="server"></asp:Label>
                                        </span>

                                        <div id="divRoyalty" runat="server" visible="false">
                                            <%-- <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Left <%=  Session("ColName1") %>:"></asp:Label>--%>
                                        Left Royalty BV:
                                        <asp:Label ID="LblLeftRoyalty" Font-Bold="true" runat="server" Text="0.00"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <%-- <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Right <%=  Session("ColName1") %>:"></asp:Label>--%>
                                        Right Royalty BV:
                                        <asp:Label ID="LblRightRoyalty" Font-Bold="true" runat="server" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <div class="row" id="divR" runat="server" visible="false">
                                    <div class="col-md-12 total-summary">

                                        <span class="me-4">
                                            <asp:Label ID="LeftSmartcardbv" runat="server" Style="font-weight: bold; font-size: 14px;"></asp:Label>
                                            <asp:Label ID="rightSmartcardbv" runat="server" Style="font-weight: bold; font-size: 14px;"></asp:Label></span>
                                        <span class="me-4">
                                            <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold; font-size: 14px;" Visible="false"></asp:Label></span>
                                    </div>




                                </div>

                                <div class="row">
                                    <div class="col-md-10">
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlPazeSize" runat="server" CssClass="form-control" AutoPostBack="true">
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                            <asp:ListItem Text="150" Value="150"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row">
                                    <style>
                                        .total-summary {
                                            display: flex;
                                            flex-wrap: wrap;
                                            gap: 14px;
                                            align-items: stretch;
                                            margin-top: 10px;
                                        }

                                        .ts-card {
                                            background: #fff;
                                            border: 1px solid #e5e9ee;
                                            border-radius: 10px;
                                            padding: 12px 18px;
                                            display: flex;
                                            align-items: center;
                                            box-shadow: 0 2px 8px rgba(16,40,32,.05);
                                        }

                                        .ts-group {
                                            display: flex;
                                            flex-wrap: wrap;
                                            align-items: center;
                                            gap: 18px;
                                            border-radius: 10px;
                                            padding: 12px 18px;
                                            box-shadow: 0 2px 8px rgba(16,40,32,.05);
                                        }

                                            .ts-group .ts-head {
                                                width: 100%;
                                                font-size: .7rem;
                                                font-weight: 700;
                                                text-transform: uppercase;
                                                letter-spacing: .5px;
                                                margin-bottom: 2px;
                                            }
                                        /* BV = green */
                                        .ts-bv {
                                            background: #eef7f2;
                                            border: 1px solid #cfe9dd;
                                            border-left: 4px solid #0d8a5f;
                                        }

                                            .ts-bv .ts-head {
                                                color: #0a6e4b;
                                            }

                                            .ts-bv span {
                                                color: #2f4a40;
                                            }
                                        /* PV = blue */
                                        .ts-pv {
                                            background: #eef2fb;
                                            border: 1px solid #cdddf4;
                                            border-left: 4px solid #185fa5;
                                        }

                                            .ts-pv .ts-head {
                                                color: #0c447c;
                                            }

                                            .ts-pv span {
                                                color: #2c3e50;
                                            }
                                    </style>

                                    <div class="col-md-12 total-summary">

                                        <!-- Total Record -->
                                        <div class="ts-card">
                                            <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Total Record : "></asp:Label>
                                            <asp:Label ID="LblttlRcd1" runat="server" Font-Bold="true"></asp:Label>
                                        </div>

                                        <!-- BV Group (green) -->
                                        <div class="ts-group ts-bv">
                                            <span class="ts-head">Business Volume (BV)</span>
                                            <asp:Label ID="lblleftbv" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <asp:Label ID="lblbv" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <asp:Label ID="lblroyaltileftbv" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <asp:Label ID="lblroyaltirightbv" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <span>
                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text=""></asp:Label>
                                                <asp:Label ID="LblttlBv" runat="server" Font-Bold="true"></asp:Label>
                                            </span>
                                        </div>

                                        <!-- PV Group (blue) -->
                                        <div class="ts-group ts-pv">
                                            <span class="ts-head">Point Volume (PV)</span>
                                            <asp:Label ID="lblleftbv1" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <asp:Label ID="lblbv1" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <asp:Label ID="lblroyaltileftbv1" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <asp:Label ID="lblroyaltirightbv1" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <span>
                                                <asp:Label ID="Label9" runat="server" Font-Bold="true" Text=""></asp:Label>
                                                <asp:Label ID="LblttlBv1" runat="server" Font-Bold="true">0</asp:Label>
                                            </span>
                                        </div>

                                    </div>
                                    <%--<div class="col-md-12 total-summary">

                                        <span class="me-4">
                                            <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Total Record : "></asp:Label>
                                            <asp:Label ID="LblttlRcd1" runat="server" Font-Bold="true"></asp:Label>
                                        </span>
                                        <span class="me-4">
                                            <asp:Label ID="lblleftbv" runat="server" Style="font-weight: bold;"></asp:Label>
                                        </span>
                                        <span class="me-4">
                                            <asp:Label ID="lblbv" runat="server" Style="font-weight: bold;"></asp:Label>
                                        </span>
                                        <span class="me-4">
                                            <asp:Label ID="lblroyaltileftbv" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <asp:Label ID="lblroyaltirightbv" runat="server" Style="font-weight: bold;"></asp:Label>
                                        </span>
                                        <span>
                                            <asp:Label ID="Label2" runat="server" Font-Bold="true" Text=""></asp:Label>
                                            <asp:Label ID="LblttlBv" runat="server" Font-Bold="true"></asp:Label>
                                        </span>
                                        <span class="me-4">
                                            <asp:Label ID="lblleftbv1" runat="server" Style="font-weight: bold;"></asp:Label>
                                        </span>
                                        <span class="me-4">
                                            <asp:Label ID="lblbv1" runat="server" Style="font-weight: bold;"></asp:Label>
                                        </span>
                                        <span class="me-4">
                                            <asp:Label ID="lblroyaltileftbv1" runat="server" Style="font-weight: bold;"></asp:Label>
                                            <asp:Label ID="lblroyaltirightbv1" runat="server" Style="font-weight: bold;"></asp:Label>
                                        </span>
                                        <span>
                                            <asp:Label ID="Label9" runat="server" Font-Bold="true" Text=""></asp:Label>
                                            <asp:Label ID="LblttlBv1" runat="server" Font-Bold="true">0</asp:Label>
                                        </span>
                                    </div>--%>
                                </div>

                                <br />


                                <div id="DivSideA" class="table-responsive" runat="server">

                                    <asp:DataGrid ID="GrdDirects" runat="server" PageSize="10" CssClass="table table-striped table-bordered"
                                        CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="true" AllowPaging="True"
                                        Width="100%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle" OnPageIndexChanging="ddlPazeSize_SelectedIndexChanged" OnPageIndexChanged="GrdDirects_PageIndexChanged">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Container.DataSetIndex + 1%>.
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>
                                        <ItemStyle CssClass="RowStyle" HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />
                                    </asp:DataGrid>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>
