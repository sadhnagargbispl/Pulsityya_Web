using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitaFlow.Application.IServices;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;

namespace VitaFlow.Application.Services
{
    public class ProductServices : I_Product_Service
    {
        private readonly I_Product I_Product;
        public ProductServices(I_Product i_Product)
        {
            i_Product = i_Product;
        }

        public Task<List<Product>> Productlist(string FCode)
        {
            return I_Product.Productlist(FCode);
        }

        public Task<string> AddtoCart(M_CartDetails req)
        {
            return I_Product.AddtoCart(req);
        }
        public Task<List<M_CartDetails>> CartDetails(M_CartDetails req)
        {
            return I_Product.CartDetails(req);
        }
        public Task<string> GetCartTotal(M_CartDetails req)
        {
            return I_Product.GetCartTotal(req);
        }
        public Task<string> Updatecart(M_CartDetails req)
        {
            return I_Product.Updatecart(req);
        }
        public Task<string> DeleteCart(M_CartDetails req)
        {
            return I_Product.DeleteCart(req);
        }
        public Task<ShippingDetail> GetShippingDetail(ShippingDetail req)
        {
            return I_Product.GetShippingDetail(req);
        }
        public Task<List<M_City>> GetCityList(string Statecode)
        {
            return I_Product.GetCityList(Statecode);
        }
        public Task<string> SaveFranchiseShippingDetail(ShippingDetail req)
        {
            return I_Product.SaveFranchiseShippingDetail(req);
        }
        public Task<ResponseDetail> SaveDistributorBill(DistributorBillModel req)
        {
            return I_Product.SaveDistributorBill(req);
        }
        public Task<M_CompanyMaster> GetCompanyDetail()
        {
            return I_Product.GetCompanyDetail();
        }
        public Task<ResponseDetail> SavePartyOrderDetails(OrderReq req)
        {
            return I_Product.SavePartyOrderDetails(req);
        }
        public Task<int> GetOrderCount(string Fcode)
        {
            return I_Product.GetOrderCount(Fcode);
        }
        public Task<string> GetorderMethodSelection(int Userid)
        {
            return I_Product.GetorderMethodSelection(Userid);
        }
        public Task<decimal> GetPartyWalletBalance(string LoginPartyCode, string vtype)
        {
            return I_Product.GetPartyWalletBalance(LoginPartyCode,vtype);
        }
        public Task<string> GetOrderNo(string LoginPartyCode)
        {
            return I_Product.GetOrderNo(LoginPartyCode);
        }
        public Task<string> SaveOrderMethod(int userid, string Ordermethod)
        {
            return I_Product.SaveOrderMethod(userid,Ordermethod);
        }
        public Task<int> CheckPackageSelection(string Partycode)
        {
            return I_Product.CheckPackageSelection(Partycode);
        }
        public Task<int> SaveFranchisepackage(string Fcode, int Packageid)
        {
            return I_Product.SaveFranchisepackage(Fcode, Packageid);
        }
        public Task<PackageMasterDetail> GetPartyPackageAmount(string Partycode)
        {
            return I_Product.GetPartyPackageAmount(Partycode);
        }


    }
}
