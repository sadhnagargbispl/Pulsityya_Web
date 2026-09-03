<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Orders | Pulastya Globals India (P) Limited | A Better Tomorrow Together</title>
<meta name="viewport" content="width=device-width, initial-scale=1">
<link rel="icon" type="image/x-icon" href="img/favicon.ico"/> 
<meta name="theme-color" content="#176a3a" />
<meta name="language" content="EN" />
<meta name="audience" content="all">
<meta name="content-Language" content="English">
<meta name="distribution" content="global"> 
<!--css styles -->
<link rel="stylesheet" href="css/bootstrap.min.css">
<link rel="stylesheet" href="css/layout.css?ver=<%=Timer%>">
<link rel="stylesheet" href="css/layout-responsive.css?ver=<%=Timer%>"> 
<link rel="stylesheet" href="css/font-awesome.min.css">
<link rel="stylesheet" href="css/message.css" />
<!--nav styles-->
<link rel="stylesheet" href="css/menu.css?ver=<%=Timer%>"> 
<!--Plugins-->
<link rel="stylesheet" href="plugins/animation/css/animate.css"> 
<!--Fonts-->
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Nunito+Sans:ital,opsz,wght@0,6..12,200..1000;1,6..12,200..1000&display=swap" rel="stylesheet">
<!-- #include file="inc/other-inc.asp" -->
<style>
    .breadcrmb-loc li a {
        color: #0000008c !important;
    }
    .breadcrmb-loc li a:after { 
        color: #0000008c;
    }
</style>
</head>
<body>
<!-- #include file="inc/header.asp" -->
<section class="contents"> 
    <div class="breadcrmb">
        <div class="breadcrmb-content">
            <div class="container">
                <ul class="breadcrmb-loc">
                    <li><a href="/">Home</a></li>
                    <li><a class="active">Orders</a></li>
                </ul>
            </div>
        </div> 
    </div>
    <!--  -->
    <div class="content-row"> 
        <div class="my-account">
            <div class="container"> 
                <div class="table-responsive">
                    <table class="table mytable ordrtable">
                        <thead>
                            <tr>
                                <th>Order No</th>
                                <th>Order Date</th>
                                <th>Order Amount</th>
                                <th>Courier Charge</th>
                                <th>By Wallet</th>
                                <th>By Online</th>
                                <th>Paid SV</th>
                                <th>Dispatch Status</th>
                                <th>Remark</th>
                                <th>Pay Mode</th>
                                <th>Courier Name</th>
                                <th>Docket No</th>
                                <th>Docket Date</th>
                                <th>Track</th>
                                <th>Download Bill</th>
                            </tr> 
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <a class="view" href="javascript:void(0)" onclick="ShowGrdOrderNo(this,'494027','Pending','DS123456')">494027</a>  
                                </td>
                                <td>20-Aug-2026</td>
                                <td>1490.00</td>
                                <td>0.00</td>
                                <td>1490.00</td>
                                <td>0</td>
                                <td>370.00</td>
                                <td>Pending</td>
                                <td></td>
                                <td>Wallet</td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr> 
                            <tr>
                                <td>
                                    <a class="view" href="javascript:void(0)" onclick="ShowGrdOrderNo(this,'494027','Pending','DS123456')">494027</a>  
                                </td>
                                <td>20-Aug-2026</td>
                                <td>1490.00</td>
                                <td>0.00</td>
                                <td>1490.00</td>
                                <td>0</td>
                                <td>370.00</td>
                                <td>Pending</td>
                                <td></td>
                                <td>Wallet</td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr> 
                            <tr>
                                <td>
                                    <a class="view" href="javascript:void(0)" onclick="ShowGrdOrderNo(this,'494027','Pending','DS123456')">494027</a>  
                                </td>
                                <td>20-Aug-2026</td>
                                <td>1490.00</td>
                                <td>0.00</td>
                                <td>1490.00</td>
                                <td>0</td>
                                <td>370.00</td>
                                <td>Pending</td>
                                <td></td>
                                <td>Wallet</td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr> 
                        </tbody> 
                    </table>
                </div> 
            </div>
        </div> 
    </div>
    <!--modal box-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog" style="max-width: 700px;">
            <div class="modal-content">
                <button type="button" class="close" data-dismiss="modal"></button>
                <h4>Order No. #494027</h4>
                <div class="table-responsive">
                    <table class="table mytable">
                        <thead>
                            <tr> 
                                <th>ProductName</th>
                                <th>Quantity</th>
                                <th>Price</th>
                                <th>Dispatch Qty</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr> 
                                <td>Pratiraksha Juice (500ML)</td>
                                <td>1</td>
                                <td>1490.00</td>
                                <td>0</td>
                            </tr>
                             <tr> 
                                <td>Pratiraksha Juice (500ML)</td>
                                <td>1</td>
                                <td>1490.00</td>
                                <td>0</td>
                            </tr>
                        </tbody>
                    </table>
                </div> 
            </div>
        </div>
    </div> 
</section>  
<!-- #include file="inc/footer.asp" -->
<!-- scripts -->
<script src="js/jquery.min.js"></script>
<script src="js/bootstrap.min.js"></script>
<!--Plugins-->
<script src="plugins/animation/js/wow.min.js"></script>  
<!-- custom js -->
<script src="js/layout.js?ver=<%=Timer%>"></script>
<!-- disable js -->
<script src="js/inactive.js"></script> 
<!--  -->
<script type="text/javascript">
    function ShowGrdOrderNo(obj, OrderId, status, idno) {
       /*debugger; 

        var row = jQuery(obj).parents('tr:first');
        $.ajax({
            type: "Post",
            url: '/Home/SrchOrderDetail',
            dataType: "Json",
            data: { OrderId: OrderId },
            success: function (data) {
                debugger; 
                $('#GrdOrderNo').html(data.tblOrder); 
                $('#myModal').modal('show'); 
            }
        });*/
        $('#myModal').modal('show');
    }
</script>
</body>
</html>

