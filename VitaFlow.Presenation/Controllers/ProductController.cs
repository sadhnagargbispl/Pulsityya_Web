using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;

namespace VitaFlow.Presenation.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly I_Product i_Product;
        
        public ProductController(ILogger<ProductController> logger, I_Product iProduct)
        {
            _logger = logger;
            i_Product = iProduct;
            
        }
        public async Task<IActionResult> Productlist()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                int Selectedpackageid = 0;
                Selectedpackageid = await i_Product.CheckPackageSelection(Convert.ToString(HttpContext.Session.GetString("FCode")));
                if (Selectedpackageid == 0)
                {
                    return RedirectToAction("PackageSelection", "Cart");
                }

                M_product obj = new M_product();
                obj.ProductsList = await i_Product.Productlist(HttpContext.Session.GetString("ParentPartyCode"));
                obj.SubCategoriesList = await i_Product.GetSubCategories();   // NEW
                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        //public async Task<IActionResult> Productlist()
        //{
        //    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
        //    {
        //        int Selectedpackageid = 0;
        //        Selectedpackageid = await i_Product.CheckPackageSelection(Convert.ToString(HttpContext.Session.GetString("FCode")));
        //        if (Selectedpackageid == 0)
        //        {
        //            return RedirectToAction("PackageSelection", "Cart");
        //        }
        //        M_product obj = new M_product();
        //        obj.ProductsList = await i_Product.Productlist(HttpContext.Session.GetString("ParentPartyCode"));
        //        return View(obj);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //}

        [HttpPost]
        public async Task<ActionResult> Addtocart(string ProdId, string ProdName, string imagePath, string Qty,string BatchNo)
        {
            string Rstr = string.Empty;
            string msg = string.Empty;
            string total = string.Empty;
            try
            {
                M_CartDetails cartRequest = new M_CartDetails();
                cartRequest.ProdId = ProdId;
                cartRequest.ProdName = ProdName;
                cartRequest.imagePath = imagePath;
                cartRequest.qty = Convert.ToDecimal(Qty);
                cartRequest.BatchNo = BatchNo;
                cartRequest.FCode = HttpContext.Session.GetString("FCode");
                cartRequest.Action = "AddTocartDeatils";
                cartRequest.UnqiueId = "";
                cartRequest.userid = HttpContext.Session.GetString("UserId");
                cartRequest.IpAddress = "";
                Rstr = await i_Product.AddtoCart(cartRequest);
                if (Rstr == "S")
                {
                    cartRequest.Action = "GetCartCount";
                    total = await i_Product.GetCartTotal(cartRequest);
                    msg = "1";
                }
                else
                {
                    msg = "0";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { total,msg });
        }

        public async Task<IActionResult> GetCartCount()
        {
            string total = "0";
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
                {
                    M_CartDetails cartRequest = new M_CartDetails();
                    cartRequest.Action = "GetCartCount";
                    cartRequest.userid = HttpContext.Session.GetString("UserId");
                    total = await i_Product.GetCartTotal(cartRequest);
                }
                else
                {
                    total = "0";
                }
            }
            catch (Exception ex)
            {
                total = "0";
            }
            return Json(new { total });
        }
    }
}
