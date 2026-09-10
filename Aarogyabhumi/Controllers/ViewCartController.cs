using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using Shopinv.SiteExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Shopinv.Controllers
{
    [KycRequired]
    public class ViewCartController : Controller
    {
        private readonly I_Category icateogry = null;
        private readonly I_Product iprod = null;
        List<E_CartDetails> objcart = new List<E_CartDetails>();
        CompanyDetail companyDetail;
        public ViewCartController(I_Category icateogry, I_Product iprod)
        {
            this.iprod = iprod;
            companyDetail = new CompanyDetail(this.iprod);
            companyDetail.GetCompanydetail();
        }

        public ActionResult ViewCart(M_Category obj)
        {

            if (Session["UserId"] != null && Session["RefId"] == null)
            {
                return View(obj);
            }
            else if (Session["UserId"] == null && Session["RefId"] != null)
            {
                return RedirectToAction("CartProductsOuter", "ViewCart");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CartProducts(M_Category obj, string Actiontype, string UNQId, string ScRegid)
        {
            try
            {
                var userid = Session["UserId"];
                var idno = Session["IDNO"];
                var Sessionid = Session["CurrentUserSessionID"];

                DataSet ds1 = iprod.UpdateCartDetail(Convert.ToString(userid), idno.ToString());
                obj.CartDetail = iprod.Cartdetailsftch(Convert.ToString(userid)); //Convert.ToString(Sessionid),
                Session["Cartdetailsftch"] = obj.CartDetail;
                decimal CourierCharge = 0;
                if (Convert.ToDecimal(Session["totalamount"]) < 5000)
                {
                    int totalqty = Convert.ToInt32(obj.CartDetail.Sum(s => s.qty));
                    CourierCharge = 65 * totalqty;
                }
                Session["CourierCharge"] = CourierCharge;

                obj.ProductList = (IEnumerable<E_Product>)Session["DDLProductList"];
                obj.DDLCategory = (IEnumerable<E_Category>)Session["DDLCategory"];
                obj.DeleteDetail = objcart;
                TempData["objcart"] = obj.DeleteDetail;
                TempData.Keep("objcart");
                var totalbv = obj.CartDetail.Sum(p => p.bv * p.qty);

                Session["remainbv"] = totalbv;
                Session["couponamount"] = 0;
                Session["coupon"] = "";
                Session["totalamount"] = obj.CartDetail.Sum(s => s.Price * s.qty).ToString();
                DataSet ds = iprod.GetAllparty();
                List<SelectListItem> ParentParty = new List<SelectListItem>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ParentParty.Add(new SelectListItem
                    {
                        Text = "--Select --",
                        Value = "0"
                    });
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ParentParty.Add(new SelectListItem
                        {
                            Text = Convert.ToString(row["Party"]),

                            Value = Convert.ToString(row["PartyCode"])
                        });
                    }
                }
                Session["ParentPartyList"] = ParentParty;
                ViewBag.ParentPartyList = ParentParty;
            }
            catch (Exception ex)
            {

            }
            return PartialView("CartProducts", obj);
        }

        [HttpPost]
        public ActionResult ProductWiseStock(string SpecialInstruction)
        {
            M_Category objg = new M_Category();
            objg.CartDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
            Session["SpecialInstruction"] = SpecialInstruction == null ? "" : SpecialInstruction;
            string msg = "";
            try
            {
                DataSet ds = new DataSet();
                foreach (var item in objg.CartDetail)
                {
                    ds = iprod.Get_ProductWiseStock(item.ProdId);
                    var SumQty = objg.CartDetail.Where(s => s.ProdId == item.ProdId).ToList();
                    var QtyPord = SumQty[0].qty;
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            object val = ds.Tables[0].Rows[0]["trnqty"];
                            if (val == DBNull.Value)
                            {
                                if (Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]) < Convert.ToInt32(QtyPord))
                                {
                                    msg = item.ProdName + ",";
                                }
                            }
                            else
                            {
                                var remqty = Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]) - Convert.ToInt32(ds.Tables[0].Rows[0]["trnqty"]);
                                if (remqty < Convert.ToInt32(QtyPord))
                                {
                                    msg = item.ProdName + ",";
                                }
                                //if (Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]) < Convert.ToInt32(ds.Tables[0].Rows[0]["trnqty"]))
                                //{
                                //    msg = item.ProdName + ",";
                                //}
                            }

                        }
                        else if (Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]) < Convert.ToInt32(QtyPord))
                        {
                            msg = item.ProdName + ",";
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (msg != "")
            {
                msg = msg.TrimEnd(',');
                msg = "Please remove " + msg + " from the cart because it is out of stock";
            }

            if (Convert.ToString(Session["MemMode"]) == "D" && msg == "")
            {
                decimal totalbv = objg.CartDetail.Sum(s => (s.Price * s.qty));
                DataSet dscount = iprod.CheckKitOnPurchase(Convert.ToInt32(Session["FormNo"]), totalbv);
                if (dscount.Tables[0].Rows.Count > 0)
                {
                    //if (Convert.ToString(dscount.Tables[0].Rows[0]["Status"])== "FAILED")
                    //{
                    //    msg = Convert.ToString(dscount.Tables[0].Rows[0]["msg"]);
                    //}
                    if (Convert.ToString(dscount.Tables[0].Rows[0]["Status"]) == "SUCCESS")
                    {
                        Session["Newkitid"] = Convert.ToString(dscount.Tables[0].Rows[0]["NewKitID"]);
                    }
                }
            }

            return Json(new { msg });
        }
        public ActionResult CheckStatus(string Billtype)
        {
            string msg = "";
            string code = "201";
            try
            {
                Session["Billtype"] = Billtype;
                DataTable dtidstatus = iprod.GetIDStatus(Convert.ToString(Session["IDNO"]));
                if (Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]) == "Y")
                {

                }
                //else if (Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]) == "N")
                //{
                //    M_Category objg = new M_Category();
                //    objg.CartDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
                //    var totalbv = objg.CartDetail.Sum(p => p.bv * p.qty);
                //    if (totalbv < 1000)
                //    {
                //        code = "101";
                //        msg = "You can not proceed you order beacuse BV is less than 1000";
                //    }
                //}
            }
            catch (Exception ex)
            {

            }
            return Json(new { msg, code }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpadteCartdetail(M_Category objm, string ProdId, string Quantity)
        {
            var TotPrice = "";
            var Totbv = "";
            var TotPv = "";
            try
            {
                var userid = Session["UserId"];
                int i = 0;
                var productId = ProdId.Split(',');
                var Quant = Quantity.Split(',');
                for (i = 0; i < productId.Length; i++)
                {
                    objm.CartDetail = iprod.updateQuantity(productId[i], Quant[i], Convert.ToString(userid));
                    Session["Cartdetailsftch"] = objm.CartDetail;
                    TotPrice = objm.CartDetail.Sum(s => s.Price * s.qty).ToString();
                    Totbv = objm.CartDetail.Sum(s => s.bv * s.qty).ToString();
                    TotPv = objm.CartDetail.Sum(s => s.PV * s.qty).ToString();

                    Session["remainbv"] = Totbv;
                    Session["couponamount"] = 0;
                    Session["coupon"] = "";
                    Session["totalamount"] = TotPrice;
                    Session["Cartdetailsftch"] = objm.CartDetail;
                    decimal CourierCharge = 0;
                    //var totbv = objm.CartDetail.Sum(s => (s.bv * s.qty));
                    //if (totbv >= 1000)
                    //{
                    //    CourierCharge = 0;
                    //}
                    //else
                    //{
                    //    var totWeight = objm.CartDetail.Sum(s => (s.Weight * s.qty));
                    //    if (totWeight >= 0 && totWeight <= 250)
                    //    {
                    //        CourierCharge = 40;
                    //    }
                    //    else if (totWeight >= 251 && totWeight <= 500)
                    //    {
                    //        CourierCharge = 60;
                    //    }
                    //    else if (totWeight >= 501 && totWeight <= 1000)
                    //    {
                    //        CourierCharge = 80;
                    //    }
                    //    else if (totWeight >= 1000)
                    //    {
                    //        var Wt = totWeight - 1000;

                    //        var Wtg = Wt / 500;
                    //        var Wtres = Math.Ceiling(Wtg) * 20;
                    //        CourierCharge = Wtres + 80;
                    //    }
                    //}
                    Session["CourierCharge"] = CourierCharge;
                }
            }
            catch (Exception ex)
            {

            }
            var ParentParty = Session["ParentPartyList"] as List<SelectListItem>;
            ViewBag.ParentPartyList = ParentParty;
            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "CartProducts", objm);
            return Json(new { tblOrder, TotPrice, Totbv, TotPv });
        }
        [HttpPost]
        public ActionResult ClearCoupon(decimal totalamount, decimal totalbv)
        {
            Session["remainbv"] = totalbv;
            Session["couponamount"] = 0;
            Session["coupon"] = "";
            Session["totalamount"] = totalamount;
            return Json(new { success = true });
        }
        public ActionResult GetCouponInfo(string CouponNo, decimal TotalAmount, decimal totalBv)
        {
            decimal amount = 0;
            string SuccessMsg = "";
            string FailedMsg = "";
            decimal remainbv = 0;
            DataTable dt = iprod.CouponDetail(Session["FormNo"].ToString(), CouponNo, TotalAmount, totalBv);

            if (dt.Rows.Count > 0)
            {
                amount = Convert.ToDecimal(dt.Rows[0]["ShippingAmount"]);
                SuccessMsg = dt.Rows[0]["SuccessMsg"].ToString();
                FailedMsg = dt.Rows[0]["FailedMsg"].ToString();

                remainbv = Convert.ToDecimal(dt.Rows[0]["remainbv"]);
                Session["remainbv"] = remainbv;
                Session["couponamount"] = amount;
                Session["coupon"] = dt.Rows[0]["coupon"].ToString();
                if ((TotalAmount - amount) > 0)
                {
                    Session["totalamount"] = TotalAmount - amount;
                }
                else
                {
                    Session["totalamount"] = 0;
                }
            }
            else
            {
                amount = 0;
                SuccessMsg = "";
                FailedMsg = "";
                remainbv = 0;
                Session["remainbv"] = totalBv;
                Session["couponamount"] = amount;
                Session["coupon"] = "";
                Session["totalamount"] = TotalAmount;
            }
            return Json(new { amount, SuccessMsg, FailedMsg, remainbv }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteLstRow(M_Category objm, string Id, string Action, string ProdId, string ProdName, string imagePath, string qty, string Price, string bv, string IpAddress)
        {
            var userid = Session["UserId"];
            var uniqueId = Session["UniqueId"];
            var Sessionid = Session["CurrentUserSessionID"];
            //var Sessionid = Session.SessionID;
            string save = iprod.deleteProd(Action, Id, ProdId, ProdName, imagePath, qty, Price, bv, IpAddress, "0", Convert.ToString(Sessionid), Convert.ToString(userid));
            objm.CartDetail = iprod.Cartdetailsftch(Convert.ToString(userid));//Convert.ToString(Sessionid),
            Session["Cartdetailsftch"] = objm.CartDetail;
            var ParentParty = Session["ParentPartyList"] as List<SelectListItem>;
            ViewBag.ParentPartyList = ParentParty;
            var Totbv = objm.CartDetail != null ? objm.CartDetail.Sum(s => s.bv * s.qty).ToString() : "0";
            var cartCount = objm.CartDetail != null ? objm.CartDetail.Count() : 0;
            var TotPrice = objm.CartDetail != null ? objm.CartDetail.Sum(s => s.Price * s.qty).ToString() : "0";
            decimal CourierCharge = 0;
            Session["remainbv"] = Totbv;
            Session["couponamount"] = 0;
            Session["coupon"] = "";
            Session["totalamount"] = TotPrice;
            //   DataSet weight = iprod.getCourierCharge();
            //var totbv = objm.CartDetail.Sum(s => (s.bv * s.qty));
            //if (totbv >= 1000)
            //{
            //    CourierCharge = 0;
            //}
            //else
            //{
            //    var totWeight = objm.CartDetail.Sum(s => (s.Weight * s.qty));
            //    if (totWeight >= 0 && totWeight <= 250)
            //    {
            //        CourierCharge = 40;
            //    }
            //    else if (totWeight >= 251 && totWeight <= 500)
            //    {
            //        CourierCharge = 60;
            //    }
            //    else if (totWeight >= 501 && totWeight <= 1000)
            //    {
            //        CourierCharge = 80;
            //    }
            //    else if (totWeight >= 1000)
            //    {
            //        var Wt = totWeight - 1000;

            //        var Wtg = Wt / 500;
            //        var Wtres = Math.Ceiling(Wtg) * 20;
            //        CourierCharge = Wtres + 80;
            //    }
            //}

            Session["CourierCharge"] = CourierCharge;
            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "CartProducts", objm);
            return Json(new { tblOrder, cartCount, TotPrice });
            //return PartialView("CartProducts", objm);
        }

        public ActionResult GetCoupon(string Formno)
        {
            DataSet ds = iprod.GetCoupon(Formno);
            string code = "101";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                code = "201";
                var promocode = new
                {
                    Promocode = "",
                    Amount = ""
                };
                return Json(new { code });
            }
            return Json(new { code });
        }

        public ActionResult Wishlist()
        {
            if (Session["UserId"] != null && Session["RefId"] == null)
            {
                M_Category obj = new M_Category();
                IEnumerable<E_CartDetails> Wishlist = iprod.CheckUserwiseWishlist(Convert.ToInt32(Session["FormNo"]));
                obj.CartDetail = Wishlist;
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}