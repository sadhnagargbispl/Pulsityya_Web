<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Wallet Detail | Pulastya Globals India (P) Limited | A Better Tomorrow Together</title>
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
                    <li><a class="active">Wallet Detail</a></li>
                </ul>
            </div>
        </div> 
    </div>
    <!--  -->
    <div class="content-row"> 
        <div class="my-account">
            <div class="container"> 
                <div class="wallet-dtl"> 
                    <div class="row">
                        <div class="col-lg-3 col-sm-4 col-xs-8">
                            <div class="form-group">
                                <select class="form-control">
                                        <option value="M">Main Wallet</option>
                                        <option value="P">Point Wallet</option>
                                        <option value="R">Shopping Wallet</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-lg-1 col-sm-2 col-xs-4">
                            <a href="javascript:void(0);" onclick="GetWalletDetail()" class="btn btn-sub"><span>Search</span></a>
                        </div>
                    </div>
                    <!--  -->
                    <div class="row">
                        <div class="col-xs-4">
                            <div class="wallet-bal">Credit : <b>207902.00</b></div>
                        </div>
                        <div class="col-xs-4">
                            <div class="wallet-bal">Debit : <b>207902.00</b></div>
                        </div>
                        <div class="col-xs-4">
                            <div class="wallet-bal">Balance : <b>0.00</b></div>
                        </div>
                    </div>  
                </div>
                <!--  -->
                <div class="table-responsive">
                    <table class="table mytable">
                        <thead>
                            <tr>
                                <th>Sr. No.</th>
                                <th>Date</th>
                                <th>Voucher No.</th>
                                <th>Remark</th>
                                <th>Deposit</th>
                                <th>Used</th>
                            </tr> 
                        </thead>
                        <tbody> 
                            <tr>
                                <td>1</td>
                                <td>29-08-2026 2:02PM</td>
                                <td>20260828234146371</td>
                                <td>Fund Debited Against Bank Withdrawal on 29-Aug-2026 with req. no.1403</td>
                                <td>0.00</td>
                                <td>15601.00</td>
                            </tr>  
                            <tr class="">
                                <td>2</td>
                                <td>29-08-2026 2:02PM</td>
                                <td>0</td>
                                <td>Weekly Closing Incentive Credited on 29-Aug-2026</td>
                                <td>800.00</td>
                                <td>0.00</td>
                            </tr>
                            <tr class="">
                                <td>3</td>
                                <td>27-08-2026 4:10PM</td>
                                <td>20260827142931022</td>
                                <td>Withdrawal Rejected  Req. No. 1364</td>
                                <td>14801.00</td>
                                <td>0.00</td>
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
</body>
</html>

