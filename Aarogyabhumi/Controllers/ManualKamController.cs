using Newtonsoft.Json;
using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using Shopinv.SiteExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Shopinv.Controllers
{
    public class ManualKamController : Controller
    {
        private readonly static string SiteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];
        private readonly static string CpanelUrl = System.Configuration.ConfigurationManager.AppSettings["CpanelUrl"];
        private readonly I_Category icateogry = null;
        private readonly I_Product iprod = null;
        private readonly I_Login _ilogin = null;
        private readonly I_PayMode ipaymode = null;
        CompanyDetail companyDetail;
        public ManualKamController(I_Category icateogry, I_Product iprod, I_Login _ilogin, I_PayMode ipaymode)
        {
            this.icateogry = icateogry;
            this.iprod = iprod;
            this._ilogin = _ilogin;
            this.ipaymode = ipaymode;
            companyDetail = new CompanyDetail(this.iprod);
            companyDetail.GetCompanydetail();
        }
        // GET: ManualKam
        public ActionResult SaveManualPGOrder()
        {   //return View (); 
            //ManualKam/SaveManualPGOrder
            try
            {
                string Temporderdatapgid = "33";
                string formno = "595747";
                string PgTxnid = "order_SL3khL0WtS9c37";
                M_Category obj = new M_Category();
                obj.CartDetail = iprod.GetTempAddtocart(Temporderdatapgid);
                var Sessionid = Session["CurrentUserSessionID"];
                decimal CourierCharge = 0;
                DataSet Ds = iprod.GetOrderbyPgTxnid(PgTxnid);
                var idNo = Ds.Tables[0].Rows[0]["idNo"].ToString();
                IEnumerable<E_CartDetails> CheckOutDetail = null;
                CheckOutDetail = FinalCalualtePg(Temporderdatapgid, formno);
                
                Session["totalbv"] = CheckOutDetail.Sum(s => s.bv * s.qty).ToString();
                Session["totalpv"] = CheckOutDetail.Sum(s => s.PV * s.qty).ToString();
                if (Convert.ToDecimal(Session["totalpv"]) <= 99)
                {
                    CourierCharge = 100;
                }
                decimal totalgst = CheckOutDetail.Sum(s => (s.Gst * s.qty));
                decimal ToTpayAmount = CheckOutDetail.Sum(s => (s.Netamount));
                decimal Totalamount = CheckOutDetail.Sum(s => (s.amount));
                
                string apiurl = CpanelUrl + "/CheckLogin?token=abUnMar5489pidlAewUF4875brlE8a4i5n61106&UserName=" + Convert.ToString(idNo) + "&Password=" + Convert.ToString(Ds.Tables[0].Rows[0]["Passw"]) + "&action=addbv&amount=" + Convert.ToString(Totalamount) + "&billtype=R&kitid=0&totalpv=" + Convert.ToString(Session["totalpv"]) + "&gst=" + totalgst + "&netamount=" + ToTpayAmount + "&TxnData=" + Convert.ToString(Ds.Tables[0].Rows[0]["txnid"]) + ";" + Convert.ToString(Session["totalbv"]) + ";BVCredit";
                var response = Callgetfunction(apiurl);
                var output = JsonConvert.DeserializeObject<Bsnaddbresponse>(response);
                iprod.SaveAarogyaidactivationLog(idNo, apiurl, response);
                if (output.status == "SUCCESS")
                {
                    DataSet dsproduct = iprod.GetTransId(Convert.ToString(Ds.Tables[0].Rows[0]["txnid"]));
                    // status = payment.status,          // created | authorized | captured | failed
                    var ShopType = Ds.Tables[0].Rows[0]["ShopType"].ToString();
                    var userid = Ds.Tables[0].Rows[0]["userid"].ToString();

                    var FormNo = Ds.Tables[0].Rows[0]["FormNo"].ToString();
                    var PartyCode = Ds.Tables[0].Rows[0]["PartyCode"].ToString();
                    var CourierCharge1 = Convert.ToDecimal(Ds.Tables[0].Rows[0]["CourierCharge"]);
                    var Deliveryid = Ds.Tables[0].Rows[0]["DecliveryId"].ToString();
                    var Amount = Convert.ToDouble(Ds.Tables[0].Rows[0]["Amount"]);
                    var OrderType = string.Empty;
                    string myIP = "";
                    if (ShopType.ToString() == "1")
                    {
                        OrderType = "T";
                    }
                    else
                    {
                        OrderType = "O";
                    }
                    decimal qty = 0;
                    StringBuilder sb = new StringBuilder();
                    if (CheckOutDetail.Count() > 0)
                    {
                        foreach (var item in CheckOutDetail)
                        {
                            if (item.BunchQty > 0)
                            {
                                qty = item.qty * item.BunchQty;
                            }
                            else
                            {
                                qty = item.qty;
                            }
                            if (Convert.ToDecimal(Session["remainbv"]) == 0)
                            {
                                Session["remainbv"] = (item.qty * item.bv);
                            }
                            sb.AppendLine("<orders>");
                            sb.AppendLine("<OrderData>");
                            sb.AppendLine(("<ProdId>" + item.ProdId + "</ProdId>"));
                            sb.AppendLine(("<ProdName>" + item.ProdName.Replace("&", "").ToString() + "</ProdName>"));
                            sb.AppendLine(("<Qty>" + qty + "</Qty>"));
                            sb.AppendLine(("<Price>" + (item.Netamount) + "</Price>"));
                            sb.AppendLine(("<BV>" + (item.qty * item.bv) + "</BV>"));
                            sb.AppendLine(("<PV>" + (item.qty * item.PV) + "</PV>"));
                            sb.AppendLine(("<myIP>" + myIP + "</myIP>"));
                            sb.AppendLine(("<Sessionid>" + Convert.ToString(Sessionid) + "</Sessionid>"));
                            sb.AppendLine(("<userid>" + Convert.ToString(userid) + "</userid>"));
                            sb.AppendLine(("<idNo>" + Convert.ToString(idNo) + "</idNo>"));
                            sb.AppendLine(("<FormNo>" + Convert.ToString(FormNo) + "</FormNo>"));
                            sb.AppendLine(("<ShopingBillType>" + Convert.ToString(ShopType) + "</ShopingBillType>"));
                            sb.AppendLine(("<OrderType>" + Convert.ToString(OrderType) + "</OrderType>"));
                            sb.AppendLine(("<PartyCode>" + Convert.ToString(PartyCode) + "</PartyCode>"));
                            sb.AppendLine(("<Mode>" + "PaymentGateway" + "</Mode>"));
                            sb.AppendLine(("<CourierCharge>" + Convert.ToString(CourierCharge) + "</CourierCharge>"));
                            sb.AppendLine(("<Color>" + Convert.ToString(item.Color) + "</Color>"));
                            sb.AppendLine(("<Size>" + Convert.ToString(item.Size) + "</Size>"));
                            sb.AppendLine(("<TRNCharge>" + Convert.ToString("0.00") + "</TRNCharge>"));
                            sb.AppendLine(("<coupon>" + Convert.ToString("0") + "</coupon>"));
                            sb.AppendLine(("<couponamount>" + Convert.ToString("0") + "</couponamount>"));
                            sb.AppendLine(("<paidbv>" + Convert.ToString(Session["remainbv"]) + "</paidbv>"));
                            sb.AppendLine(("<Shoppingwallet>" + Convert.ToString("0") + "</Shoppingwallet>"));
                            sb.AppendLine(("<Repurchasewallet>" + Convert.ToString("0") + "</Repurchasewallet>"));
                            sb.AppendLine(("<Earnbase>" + Convert.ToString(item.Earnbase) + "</Earnbase>"));
                            sb.AppendLine(("<LessEB>" + Convert.ToString(item.Lesseb) + "</LessEB>"));
                            sb.AppendLine(("<Gst>" + Convert.ToString(item.Gst) + "</Gst>"));
                            sb.AppendLine(("<finalprice>" + Convert.ToString(item.finalprice) + "</finalprice>"));
                            sb.AppendLine("</OrderData>");
                            sb.AppendLine("</orders>");
                        }
                    }
                    var ordertransId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    DataSet ds = iprod.InserttblTrnOrderWeb(sb.ToString(), Convert.ToDecimal(FormNo), Convert.ToDecimal(Amount), Convert.ToDecimal(ordertransId), "P", idNo.ToString(), "", output.voucherno);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var id = ds.Tables[0].Rows[0][0];
                        DataSet Order = iprod.UpdatePaymentOrderId(id.ToString(), Convert.ToString(Ds.Tables[0].Rows[0]["txnid"]));
                        DataSet Status = iprod.UpdateStatus("Paid", Convert.ToString(Ds.Tables[0].Rows[0]["txnid"]), "", "");
                        //message = "Order save successfully";

                        Session["coupon"] = "";
                        Session["couponamount"] = "0";
                        Session["remainbv"] = "0";
                        E_SaveOrderDetail obg = new E_SaveOrderDetail();
                        List<E_SaveOrderDetail> lst = new List<E_SaveOrderDetail>();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int qty1 = 0;
                            if (Convert.ToInt32(dr["BunchQty"]) > 0)
                            {
                                qty1 = Convert.ToInt32(dr["qty"]) / Convert.ToInt32(dr["BunchQty"]);
                            }
                            else
                            {
                                qty1 = Convert.ToInt32(dr["qty"]);
                            }

                            lst.Add(new E_SaveOrderDetail
                            {
                                OrderId = Convert.ToInt32(dr["OrderId"]),
                                ProdName = Convert.ToString(dr["ProdName"]),
                                Price = Convert.ToDecimal(dr["Price"]),
                                qty = Convert.ToDecimal(qty1),//Convert.ToDecimal(dr["qty"]),
                                Mode = Convert.ToString(dr["Mode"]),
                                OrderDate = Convert.ToString(dr["OrderDate"]),
                                ImagePath = SiteExtension.MediaUrl.Rehost(Convert.ToString(dr["ImagePath"])),
                                MRP = Convert.ToDecimal(dr["MRP"]),
                                BV = Convert.ToDecimal(dr["BV"]),
                                PV = Convert.ToDecimal(dr["PV"]),
                                LessEB = Convert.ToDecimal(dr["LessEB"]),
                                DP = Convert.ToDecimal(dr["DP"]),
                                finalprice = Convert.ToDecimal(dr["finalprice"]),
                                //CourierCharge = Convert.ToDecimal(dr["CourierCharge"])
                                //Imagepath = Convert.ToString(dr["ImagePath"]),
                            });
                        }
                        DataTable dtidstatus = iprod.GetIDStatus(Convert.ToString(Session["IDNO"]));
                        Session["OrderId"] = lst[0].OrderId;
                        Session["BillNo"] = output.voucherno;
                        Session["ActiveStatus"] = Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]);
                        Session["CheckOrderlst"] = lst;
                        Session["Status"] = "PAID";
                        var CartDetail = iprod.Cartdetailsftch(Convert.ToString(userid));//Convert.ToString(Sessionid),
                        Session["Cartdetailsftch"] = CartDetail;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public string Callgetfunction(string Url)
        {
            string sResponseFromServer = string.Empty;
            try
            {
                WebRequest tRequest;
                Stream dataStream;
                tRequest = WebRequest.Create(Url);
                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                sResponseFromServer = tReader.ReadToEnd();
            }
            catch
            {

            }
            return sResponseFromServer;
        }

        public IEnumerable<E_CartDetails> FinalCalualtePg(string Temporderdatapgid,string formno)
        {
            M_Category objg = new M_Category();
            objg.CartDetail = iprod.GetTempAddtocart(Temporderdatapgid);
            try
            {
                var Totalpv = objg.CartDetail.Sum(s => s.PV * s.qty).ToString();
                DataSet dsrange = iprod.GetWholeIncomeRange(Convert.ToInt32(formno), Convert.ToDecimal(Totalpv));
                if (dsrange != null && dsrange.Tables.Count > 0 && dsrange.Tables[0].Rows.Count > 0)
                {
                    decimal discount = Convert.ToDecimal(dsrange.Tables[0].Rows[0]["Discount"]);
                    foreach (var item in objg.CartDetail)
                    {
                        item.Dp = item.Price;
                        item.Totalvp = item.qty * item.PV;
                        //totalbv = qty * item.bv;
                        item.Earnbase = item.ProdCommssn;
                        item.Lesseb = (item.Earnbase * discount) / 100;
                        item.finalprice = item.Price - item.Lesseb;
                        item.amount = item.qty * item.finalprice;
                        item.Gst = (item.amount * item.Gst) / 100;
                        item.Netamount = item.amount + item.Gst;
                        item.Discount = discount;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objg.CartDetail;
        }
    }
}