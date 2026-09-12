using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitaFlow.Domain.Entities;

namespace VitaFlow.Application.IServices
{
    public interface I_Product_Service
    {
        Task<List<Product>> Productlist(string FCode);
        Task<string> AddtoCart(M_CartDetails req);
        Task<List<M_CartDetails>> CartDetails(M_CartDetails req);
        Task<string> GetCartTotal(M_CartDetails req);
        Task<string> Updatecart(M_CartDetails req);
        Task<string> DeleteCart(M_CartDetails req);
        Task<ShippingDetail> GetShippingDetail(ShippingDetail req);
        Task<List<M_City>> GetCityList(string Statecode);
        Task<string> SaveFranchiseShippingDetail(ShippingDetail req);
        Task<ResponseDetail> SaveDistributorBill(DistributorBillModel req);
        Task<M_CompanyMaster> GetCompanyDetail();
        Task<ResponseDetail> SavePartyOrderDetails(OrderReq req);
        Task<int> GetOrderCount(string Fcode);
        Task<string> GetorderMethodSelection(int Userid);
        Task<decimal> GetPartyWalletBalance(string LoginPartyCode, string vtype);
        Task<string> GetOrderNo(string LoginPartyCode);
        Task<string> SaveOrderMethod(int userid, string Ordermethod);
        Task<int> CheckPackageSelection(string Partycode);
        Task<int> SaveFranchisepackage(string Fcode, int Packageid);
        Task<PackageMasterDetail> GetPartyPackageAmount(string Partycode);
    }
}
