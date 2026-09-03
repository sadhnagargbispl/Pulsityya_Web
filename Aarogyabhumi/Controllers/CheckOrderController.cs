using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using Shopinv.SiteExtension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Shopinv.Controllers
{
    //[KycRequired]
    public class CheckOrderController : Controller
    {
        private readonly I_OrderReport iorderrept = null;
        private readonly I_Product _iprod = null;
        CompanyDetail companyDetail;
        public CheckOrderController(I_OrderReport iorderrept, I_Product iprod)
        {
            this.iorderrept = iorderrept;
            _iprod = iprod;
            companyDetail = new CompanyDetail(this._iprod);
            companyDetail.GetCompanydetail();
        }
        // GET: CheckOrder
        public ActionResult CheckOrder(M_Category obj)
        {
            //obj.CartDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
            //obj.ProductList = (IEnumerable<E_Product>)Session["DDLProductList"];
            //obj.DDLCategory = (IEnumerable<E_Category>)Session["DDLCategory"];
            //var Lst = (List<E_SaveOrderDetail>)Session["CheckOrderlst"];
            return View();
        }

        public ActionResult Checkofflineorder(int id)
        {
            M_Category obj = new M_Category();
            var userid = Session["UserId"];
            obj.OrderNoDetail = iorderrept.Showofflineorderdetail(Convert.ToString(id), Convert.ToString(userid));
            return View(obj);
        }
    }
}