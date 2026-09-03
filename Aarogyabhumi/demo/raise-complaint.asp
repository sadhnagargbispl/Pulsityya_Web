<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Raise Complaint | Pulastya Globals India (P) Limited | A Better Tomorrow Together</title>
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
                    <li><a class="active">Raise Complaint</a></li>
                </ul>
            </div>
        </div> 
    </div>
    <!--  -->
    <div class="content-row">  
        <div class="my-account">
            <div class="container"> 
               <div class="login-bar"> 
                <div class="login-col rgstr-col"> 
                    <h3>Raise Complaint</h3>
                    <p>Your Voice, Our Commitment - Every Concern Matters.</p> 
                    <div class="row">  
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Full Name</label>
                                <input type="text" class="form-control">
                            </div> 
                        </div>
                    </div> 
                    <!--  -->
                    <div class="row">
                        <div class="col-sm-6"> 
                             <div class="form-group">
                                <label>Email</label>
                                <input type="text" class="form-control">
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Mobile No.</label>
                                <input type="text" class="form-control" maxlength="10" onkeypress="return isNumber(event)">
                            </div> 
                        </div>
                    </div>  
                    <!--  --> 
                    <div class="row">
                        <div class="col-sm-6">
                             <div class="form-group">
                                <label>Complaint Type</label>
                                <select class="form-control">
                                    <option value="1">Normal Complaint</option>
                                    <option value="2">Payout Related</option>
                                    <option value="3">Product Related</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Subject</label>
                                <input type="text" class="form-control">
                            </div> 
                        </div> 
                    </div>    
                    <!--  --> 
                    <div class="form-group">
                        <label>Description</label>
                        <textarea class="form-control" rows="2"></textarea>
                    </div> 
                    <div class="row">
                        <div class="col-sm-3"><input type="submit" value="Submit" class="btn btn-sub"></div> 
                    </div> 
                </div> 
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

