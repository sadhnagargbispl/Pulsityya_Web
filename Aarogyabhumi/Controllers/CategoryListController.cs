using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using Shopinv.SiteExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopinv.Controllers
{
    [KycRequired]
    public class CategoryListController : Controller
    {
        private readonly I_Category _icateogry = null;
        private readonly I_Product _iprod = null;
        private readonly I_Login _ilogin = null;
        CompanyDetail companyDetail;

        public CategoryListController(I_Category i_Category, I_Product i_Product)
        {
            this._icateogry = i_Category;
            this._iprod = i_Product;
            companyDetail = new CompanyDetail(this._iprod);
            companyDetail.GetCompanydetail();
        }
        // GET: CategoryList
        public ActionResult CategoryList(string CatName, string Subcate)
        {
            M_Category objg = new M_Category();
            TempData["CatName"] = CatName;
            TempData.Keep("CatName");
            TempData["Subcate"] = Subcate;
            TempData.Keep("Subcate");
            if (string.IsNullOrEmpty(CatName))
            {
                objg.ProductList = _iprod.DDLProductList();
                // Session["ProductList"] = objg.ProductList;
            }
            else
            {
                ViewBag.CatName = CatName;
                objg.ProductList = _icateogry.SrchProduct(CatName);
            }
            if (!string.IsNullOrEmpty(Subcate))
            {
                var filterdata = objg.ProductList.Where(p => Subcate.Contains(p.SubcatName));
                objg.ProductList = filterdata;
            }
            else
            {
                objg.ProductList = objg.ProductList;
            }
            Session["ddlCatName"] = CatName;
            Session["DDLProductList"] = objg.ProductList;
            var Unqid = Session["UniqueId"];
            var Sessionid = Session["CurrentUserSessionID"];
            return View(objg);
        }
        public ActionResult GetCategorySideBar()
        {
            M_Category objg = new M_Category();
            try
            {
                objg.DDLCategory = _icateogry.DDLCategory();
                foreach (var item in objg.DDLCategory)
                {
                    item.subCategory = _icateogry.DDLSubCategory(Convert.ToString(item.CatId));
                }
                Session["DDLCategory"] = objg.DDLCategory;
                objg.DDLCategory = (IEnumerable<E_Category>)Session["DDLCategory"];
                objg.DDLCategory = (IEnumerable<E_Category>)Session["DDLCategory"];
                objg.GetColors = _icateogry.getColors(Convert.ToString(TempData["CatName"]));
                objg.GetSizes = _icateogry.GetSize(Convert.ToString(TempData["CatName"]));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return PartialView("_CategorySideBar", objg);
        }
        public ActionResult HighToLowSrchProduct(M_Category obj)
        {
            List<E_Product> FilterProductList = new List<E_Product>();
            decimal Price = 0;
            decimal MRP = 0;
            int BV = 0;
            string CatName = Convert.ToString(TempData["CatName"]);
            TempData["CatName"] = CatName;
            TempData.Keep("CatName");
            if (string.IsNullOrEmpty(CatName))
            {
                obj.ProductList = _iprod.DDLProductList();
            }
            else
            {
                obj.ProductList = _icateogry.SrchProduct(CatName);
            }
            //obj.ProductList = _iprod.DDLProductList();
            //Session["ProductList"] = obj.ProductList;
            foreach (var item in obj.ProductList)
            {
                if (item.BunchQty > 0)
                {
                    Price = item.Price * item.BunchQty;
                    MRP = item.MRP * item.BunchQty;
                    BV = Convert.ToInt32(item.BV * item.BunchQty);
                }
                else
                {
                    Price = item.Price;
                    MRP = item.MRP;
                    BV = Convert.ToInt32(item.BV);
                }
                E_Product prod = new E_Product()
                {
                    ProdId = item.ProdId,
                    ProductName = item.ProductName,
                    ImagePath = item.ImagePath,
                    Price = Price,
                    MRP = MRP,
                    BV = BV,
                    SubcatName = item.SubcatName,
                    CatName = item.CatName,
                    Discount = item.Discount,
                    Liner = item.Liner,
                    ProductDiscription = item.ProductDiscription,
                    BunchQty = item.BunchQty,
                    Weight = item.Weight
                };
                FilterProductList.Add(prod);
            }

            string Subcate = Convert.ToString(TempData["Subcate"]);
            TempData["Subcate"] = Subcate;
            TempData.Keep("Subcate");
            if (!string.IsNullOrEmpty(Subcate))
            {
                var filterdata = FilterProductList.Where(p => Subcate.Contains(p.SubcatName));
                var orderByDescendingResult = from s in filterdata
                                              orderby s.Price descending
                                              select s;
                obj.SrchFilterProductList = orderByDescendingResult.ToList();
            }
            else
            {
                var orderByDescendingResult = from s in FilterProductList
                                              orderby s.Price descending
                                              select s;
                obj.SrchFilterProductList = orderByDescendingResult.ToList();
            }

            //var orderByDescendingResult = from s in FilterProductList
            //                              orderby s.Price descending
            //                              select s;

            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "_FilterProductRangeWise", obj);
            // return Json(new { tblOrder });
            // obj.FeaturedProduct = iprod.GetFeaturedProduct();
            return PartialView("_FilterProductRangeWise", obj);
        }

        public ActionResult LowToHighSrchProduct(M_Category obj)
        {
            List<E_Product> FilterProductList = new List<E_Product>();
            decimal Price = 0;
            decimal MRP = 0;
            int BV = 0;
            string CatName = Convert.ToString(TempData["CatName"]);
            TempData["CatName"] = CatName;
            TempData.Keep("CatName");
            if (string.IsNullOrEmpty(CatName))
            {
                obj.ProductList = _iprod.DDLProductList();
            }
            else
            {
                obj.ProductList = _icateogry.SrchProduct(CatName);
            }
            //obj.ProductList = _iprod.DDLProductList();
            //obj.ProductList = (IEnumerable<E_Product>)Session["ProductList"];

            foreach (var item in obj.ProductList)
            {
                if (item.BunchQty > 0)
                {
                    Price = item.Price * item.BunchQty;
                    MRP = item.MRP * item.BunchQty;
                    BV = Convert.ToInt32(item.BV * item.BunchQty);
                }
                else
                {
                    Price = item.Price;
                    MRP = item.MRP;
                    BV = Convert.ToInt32(item.BV);
                }
                E_Product prod = new E_Product()
                {
                    ProdId = item.ProdId,
                    ProductName = item.ProductName,
                    ImagePath = item.ImagePath,
                    Price = Price,
                    MRP = MRP,
                    BV = BV,
                    SubcatName = item.SubcatName,
                    CatName = item.CatName,
                    Discount = item.Discount,
                    Liner = item.Liner,
                    ProductDiscription = item.ProductDiscription,
                    BunchQty = item.BunchQty,
                    Weight = item.Weight,
                    StockQTY = item.StockQTY

                };
                FilterProductList.Add(prod);
            }
            string Subcate = Convert.ToString(TempData["Subcate"]);
            TempData["Subcate"] = Subcate;
            TempData.Keep("Subcate");
            if (!string.IsNullOrEmpty(Subcate))
            {
                var filterdata = FilterProductList.Where(p => Subcate.Contains(p.SubcatName));
                var ProductsInAscOrder = from s in filterdata
                                         orderby s.Price
                                         select s;
                obj.SrchFilterProductList = ProductsInAscOrder.ToList();
            }
            else
            {
                var ProductsInAscOrder = from s in FilterProductList
                                         orderby s.Price
                                         select s;
                obj.SrchFilterProductList = ProductsInAscOrder.ToList();
            }
            
            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "_FilterProductRangeWise", obj);
            // obj.FeaturedProduct = iprod.GetFeaturedProduct();
            return PartialView("_FilterProductRangeWise", obj);
        }
    }
}