<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Complaints | Pulastya Globals India (P) Limited | A Better Tomorrow Together</title>
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
                    <li><a class="active">Complaints</a></li>
                </ul>
            </div>
        </div> 
    </div>
    <!--  -->
    <div class="content-row"> 
        <div class="my-account">
            <div class="container"> 
                <div class="table-responsive">
                    <table class="table mytable">
                        <thead>
                            <tr> 
                                <th>Complaint ID</th>
                                <th>Date</th>
                                <th>Complaint</th>
                                <th>Action</th>
                            </tr> 
                        </thead>
                        <tbody>
                        <tr>
                            <td>10017</td>
                            <td>20-Jun-2026</td>
                            <td>Test complaint</td>
                            <td> 
                                <a class="view" href="javascript:void(0)" onclick="ShowReply('10017')">494027</a>
                            </td>
                        </tr>
                        <tr>
                            <td>10017</td>
                            <td>20-Jun-2026</td>
                            <td>Test complaint</td>
                            <td> 
                                <a class="view" href="javascript:void(0)" onclick="ShowReply('10017')">494027</a>
                            </td>
                        </tr>
                        <tr>
                            <td>10017</td>
                            <td>20-Jun-2026</td>
                            <td>Test complaint</td>
                            <td> 
                                <a class="view" href="javascript:void(0)" onclick="ShowReply('10017')">494027</a>
                            </td>
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
                <h4>Reply - Complaint ID 123456</h4>
                <div class="table-responsive">
                    <table class="table mytable">
                        <thead>
                            <tr> 
                                <th>Date</th>
                                <th>Reply</th>  
                            </tr>
                        </thead>
                        <tbody>
                            <tr> 
                                <td>August 31, 2026</td>
                                <td>Test reply message</td>
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
    function ShowReply(complaintid) {
        /*$.ajax({
            type: "Post",
            url: '/Account/GetComplaintReplyDetails',
            dataType: "Json",
            data: { complaintid: complaintid },
            success: function (data) {
                $('#GrdOrderNo').html(data.tblOrder);
                $('#myModal').modal('show');
            }
        });*/
        $('#myModal').modal('show');
    }
</script>
</body>
</html>

