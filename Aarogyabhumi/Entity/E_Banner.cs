using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity 
{ 
    public class E_Banner
    {
        public int BannerId { get; set; }
        public int CompanyId { get; set; }
        public int BannerDetailId { get; set; }
        public string ImagePath { get; set; }
        public string BannerName { get; set; }
        public string Url { get; set; }
         public Int32 SeqNo { get; set; }
    }
}