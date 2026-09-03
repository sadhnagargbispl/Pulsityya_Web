<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Product Details | Pulastya Globals India (P) Limited | A Better Tomorrow Together</title>
<meta name="viewport" content="width=device-width, initial-scale=1">
<link rel="icon" type="image/x-icon" href="img/favicon.ico"/>
<meta name="description" content="" />
<meta name="keywords" content="" />
<meta name="theme-color" content="#176a3a" />
<meta name="language" content="EN" />
<meta name="audience" content="all">
<meta name="content-Language" content="English">
<meta name="distribution" content="global">
<meta property="og:url" content="" />
<meta property="og:title" content="" />
<meta property="og:description" content="" />
<meta property="og:image" content="" />
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
<!-- -->
<link rel="stylesheet" href="plugins/accordion/css/woco-accordion.min.css">
<!-- -->
<link rel="stylesheet" href="plugins/spotlight-master/css/spotlight.min.css">
<!-- -->
<link rel="stylesheet" href="plugins/owl-carousel/owl.custom.css">
<link rel="stylesheet" href="plugins/owl-carousel/owl.carousel.min.css">
<link rel="stylesheet" href="plugins/owl-carousel/owl.theme.default.min.css">
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
                    <li><a href="product-list.asp">Product</a></li>
                    <li><a class="active">Product Name</a></li>
                </ul>
            </div>
        </div> 
    </div>
<div class="content-row">
	<div class="itm-dtls-row">
		<div class="item-container">
			<div class="container">
				<div class="row">
					<div class="col-xs-12 col-sm-5">
						<div class="owl-carousel itm-dtls-carousel">
							<a class="spotlight" href="img/sample-product.jpg"><img src="img/sample-product.jpg" alt="Sample Product" /></a>
							<a class="spotlight" href="img/sample-product.jpg"><img src="img/sample-product.jpg" alt="Sample Product" /></a>
							<a class="spotlight" href="img/sample-product.jpg"><img src="img/sample-product.jpg" alt="Sample Product" /></a>
						</div>
					</div>
					<div class="col-xs-12 col-sm-7">
						<h2 class="pr_name">Sample Product <span class="pr_sku">Product Code : 123456 </span></h2>
						<h3 class="pr_rate">
							<span class="pr_mrp">&#x20b9; 7000</span>
							<span class="pr_offerprice">&#x20b9; 6000</span>
							<span class="pr_discount">10% Off</span>
							<span class="pr_note">Inclusive all taxes </span>
						</h3>
						<p class="pr_stock"><strong>Availability :</strong> In Stock</p> 
						<!--  -->
						<div class="row">
							<div class="col-xs-3 col-md-3 col-lg-2 no-padding-right">
								<div class="num-incr-decr">
									<span class="decrMent" dt-min="1"></span> 
									<input step="1" value="1" type="text" name="quantity">
									<span class="incrMent" dt-max="50"></span> 
								</div>
							</div>
							<div class="col-xs-6 col-md-4 col-lg-4">
								<div class="cart-btn-col">
									<a class="btn-sub pr_add_cart"><span class="icon-add-cart"></span><span class="txt-add-cart">Add to cart</span></a>
								</div>                
							</div> 
                            <div class="col-xs-3 col-md-3 col-lg-2 no-padding-left">  
                                <a class="pr-shre" onclick="shareItem('Sample Product','https://Pulastyaglobal.com')"><i class="fa fa-share-alt"></i> Share</a> 
                            </div>
						</div>
						<!--  -->
						<div class="pr_desc">
							<h1>Product Description</h1>
							<div>
								<div class="desc-inf">
									<p>Description to be shown here Description to be shown here Description to be shown here Description to be shown here Description to be shown here Description to be shown here Description to be shown here Description to be shown here </p>
								</div>
							</div>
							<!--  -->
							<h1>Usage Instruction</h1>
							<div>
								<div class="desc-inf">
									<ul>
										<li>Warm water ke saath use kar sakte hai</li>
										<li>Milk ya tea me use kar sakte hai</li> 
									</ul>
								</div>
							</div>
							<!--  -->
							<h1>Key Features</h1>
							<div>
								<div class="desc-inf">
									<ul>
										<li>Feature</li>
										<li>Feature</li>
										<li>Feature</li>
										<li>Feature</li>
										<li>Feature</li>
									</ul>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div> 
</div>
<!--Related Product  -->
<div class="pr-related-row">
	<div class="container">
		<h2 class="first-head">Related Product</h2>
		<div class="product-carousel owl-carousel">
            <div class="item">
                <div class="item-container">
                    <a class="item_wshlst"><span class="icon-wshlst"></span><span class="txt-wshlst">Wishlist</span></a>
                    <div class="item-pic-container">
                        <a href="product-list.asp" class="item-back"></a>
                        <div class="item-pic"><img src="img/sample-product.jpg" alt="Sample Product" /></div>
                    </div>
                    <p class="item-name"><a href="product-list.asp" class="item-lnk">Sample Product</a></p>
                    <p class="item-price"><span class="price-before">₹1500</span><span class="price-now">₹1199</span><span class="discount">20% Off</span><span class="sv">SV : 250</span></p>
                    <div class="item-btn"><a href="product-list.asp" class="item_add"><span>Add to cart</span></a></div>
                </div>
            </div>
            <div class="item">
                <div class="item-container">
                    <a class="item_wshlst"><span class="icon-wshlst"></span><span class="txt-wshlst">Wishlist</span></a>
                    <div class="item-pic-container">
                        <a href="product-list.asp" class="item-back"></a>
                        <div class="item-pic"><img src="img/sample-product.jpg" alt="Sample Product" /></div>
                    </div>
                    <p class="item-name"><a href="product-list.asp" class="item-lnk">Sample Product</a></p>
                    <p class="item-price"><span class="price-before">₹1500</span><span class="price-now">₹1199</span><span class="discount">20% Off</span><span class="sv">SV : 250</span></p>
                    <div class="item-btn"><a href="product-list.asp" class="item_add"><span>Add to cart</span></a></div>
                </div>
            </div>
             <div class="item">
                <div class="item-container">
                    <a class="item_wshlst"><span class="icon-wshlst"></span><span class="txt-wshlst">Wishlist</span></a>
                    <div class="item-pic-container">
                        <a href="product-list.asp" class="item-back"></a>
                        <div class="item-pic"><img src="img/sample-product.jpg" alt="Sample Product" /></div>
                    </div>
                    <p class="item-name"><a href="product-list.asp" class="item-lnk">Sample Product</a></p>
                    <p class="item-price"><span class="price-before">₹1500</span><span class="price-now">₹1199</span><span class="discount">20% Off</span><span class="sv">SV : 250</span></p>
                    <div class="item-btn"><a href="product-list.asp" class="item_add"><span>Add to cart</span></a></div>
                </div>
            </div>
             <div class="item">
                <div class="item-container">
                    <a class="item_wshlst"><span class="icon-wshlst"></span><span class="txt-wshlst">Wishlist</span></a>
                    <div class="item-pic-container">
                        <a href="product-list.asp" class="item-back"></a>
                        <div class="item-pic"><img src="img/sample-product.jpg" alt="Sample Product" /></div>
                    </div>
                    <p class="item-name"><a href="product-list.asp" class="item-lnk">Sample Product</a></p>
                    <p class="item-price"><span class="price-before">₹1500</span><span class="price-now">₹1199</span><span class="discount">20% Off</span><span class="sv">SV : 250</span></p>
                    <div class="item-btn"><a href="product-list.asp" class="item_add"><span>Add to cart</span></a></div>
                </div>
            </div>
             <div class="item">
                <div class="item-container">
                    <a class="item_wshlst"><span class="icon-wshlst"></span><span class="txt-wshlst">Wishlist</span></a>
                    <div class="item-pic-container">
                        <a href="product-list.asp" class="item-back"></a>
                        <div class="item-pic"><img src="img/sample-product.jpg" alt="Sample Product" /></div>
                    </div>
                    <p class="item-name"><a href="product-list.asp" class="item-lnk">Sample Product</a></p>
                    <p class="item-price"><span class="price-before">₹1500</span><span class="price-now">₹1199</span><span class="discount">20% Off</span><span class="sv">SV : 250</span></p>
                    <div class="item-btn"><a href="product-list.asp" class="item_add"><span>Add to cart</span></a></div>
                </div>
            </div>
             <div class="item">
                <div class="item-container">
                    <a class="item_wshlst"><span class="icon-wshlst"></span><span class="txt-wshlst">Wishlist</span></a>
                    <div class="item-pic-container">
                        <a href="product-list.asp" class="item-back"></a>
                        <div class="item-pic"><img src="img/sample-product.jpg" alt="Sample Product" /></div>
                    </div>
                    <p class="item-name"><a href="product-list.asp" class="item-lnk">Sample Product</a></p>
                    <p class="item-price"><span class="price-before">₹1500</span><span class="price-now">₹1199</span><span class="discount">20% Off</span><span class="sv">SV : 250</span></p>
                    <div class="item-btn"><a href="product-list.asp" class="item_add"><span>Add to cart</span></a></div>
                </div>
            </div>
        </div>
	</div>
</div>
 
</section>
<!-- #include file="inc/footer.asp" -->
<!-- all scripts -->
<script src="js/jquery.min.js"></script>
<script src="js/bootstrap.min.js"></script>
<!--Plugins-->
<script src="plugins/nivo-slider/nivo.js"></script>
<!-- -->
<script src="plugins/animation/js/wow.min.js"></script>
<!-- -->
<script src="plugins/accordion/js/woco.accordion.min.js"></script>
<script>
	$('.pr_desc').accordion();  
</script>
<!-- -->
<script src="plugins/spotlight-master/js/spotlight.min.js"></script>
<!-- -->
<script src="plugins/owl-carousel/owl.carousel.js"></script>
<!-- custom js -->
<script src="js/layout.js?ver=<%=Timer%>"></script>
<!-- disable js -->
<script src="js/inactive.js"></script>  
</body>
</html>
