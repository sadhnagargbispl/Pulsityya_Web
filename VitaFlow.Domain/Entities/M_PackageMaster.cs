using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class M_PackageMaster
    {
        public List<PackageMasterDetail> PackageMasters { get; set; }
    }
    public class PackageMasterDetail
    {
       public int PackageId { get; set; }
       public string  PackageName { get; set; }
       public decimal  PackageAmount { get; set; }
       public string ActiveStatus { get; set; }

    }
}
