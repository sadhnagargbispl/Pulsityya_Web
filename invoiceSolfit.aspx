<%@ Page Language="VB" AutoEventWireup="false" CodeFile="invoiceSolfit.aspx.vb" Inherits="invoiceSolfit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Receipt</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
        rel="stylesheet">

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        body
        {
            font-family: Arial, sans-serif;
            background-color: #f2f2f2;
        }
        .invoice
        {
            box-shadow: 0px 0px 20px 0px #616161;
            border-radius: 5px;
            padding: 20px;
            background: white;
            width: 100%;
            max-width: 800px;
            margin: auto;
        }
        table
        {
            width: 100%;
            border-collapse: collapse;
        }
        th, td
        {
            border: 1px solid black;
            text-align: left;
            padding: 8px;
        }
        th
        {
            background-color: #f2f2f2;
        }
        .header, .footer
        {
            font-weight: bold;
        }
        .right
        {
            text-align: right;
        }
        /* Print Styles */@media print
        {
            body
            {
                background: none;
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }
            .container
            {
                width: 100%;
                margin: 0;
                padding: 0;
            }
            .invoice
            {
                box-shadow: none;
                border: none;
                width: 100%;
                margin: 0;
                padding: 10px;
            }
            table, th, td
            {
                border: 1px solid black;
            }
            .no-print
            {
                display: none;
            }
            .page-break
            {
                page-break-before: always;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="container mt-5">
            <div class="row justify-content-center">
                <div class="col-lg-10 col-md-12">
                    <div class="invoice">
                        <div class="d-flex justify-content-between">
                            <div class="name">
                                <h3>
                                    Solfit Energy Pvt. Ltd.</h3>
                                <p>
                                    For pickup center or product delivery challan Complete payment receipt</p>
                            </div>
                            <div class="logo mt-4">
                                <img src="https://solfit.in/design_img/logo.png" alt="Logo" class="img-responsive"
                                    height="50">
                            </div>
                        </div>
                        <hr>
                        <p>
                            Address : 4th Floor, 582 Hanuwant A, Marg No. 7, BJS Colony, Jodhpur (RAJ) - 342001
                        </p>
                        <p>
                            GST No. : 08ABOCS1114K1Z6</p>
                        <p>
                            This is not GST bill, Only Automated Payment Receipt</p>
                        <table>
                            <tr>
                                <td class="header">
                                    Date:
                                </td>
                                <td>
                                    <asp:Label ID="LblDate" runat="server"></asp:Label>
                                </td>
                                <td class="header">
                                    Receipt No.:
                                </td>
                                <td>
                                    <asp:Label ID="LblOderNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="header">
                                    Customer:
                                </td>
                                <td>
                                    <asp:Label ID="LblName" runat="server"></asp:Label>
                                </td>
                                <td class="header">
                                    User ID:
                                </td>
                                <td>
                                    <asp:Label ID="LblID" runat="server"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="header">
                                    Customer Mobile No.:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="LblMobileNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="header">
                                    Customer Address:
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="LblAddress" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br>
                        <asp:GridView ID="gv" runat="server" CssClass="table datatable" AutoGenerateColumns="true"
                            ShowHeaderWhenEmpty="true">
                        </asp:GridView>
                        <br>
                        <table>
                            <tr>
                                <td class="header">
                                    Total BV:
                                </td>
                                <td>
                                    <asp:Label ID="LblTotalBv" runat="server"></asp:Label>
                                </td>
                                <td class="header right">
                                    Total Amount:
                                </td>
                                <td class="right">
                                    <asp:Label ID="LblTOtalAmount" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br>
                        <%--<button class="btn btn-primary no-print" onclick="confirmRedirect()">
                            Back</button>--%>
                        <asp:Button ID="btnBack" runat="server" Text="Back" class="btn btn-primary no-print"
                            OnClientClick="return confirmRedirect();" />
                        <button class="btn btn-primary no-print" onclick="window.print()">
                            Print</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>

<script>
function confirmRedirect() {
    if (confirm("Do you want to go back to the homepage?")) {
        window.location.href = "WithoutApproveProductReport.aspx";
        return false; // Prevents postback
    }
    return false; // Stay on the page if the user cancels
}
</script>

</html>
