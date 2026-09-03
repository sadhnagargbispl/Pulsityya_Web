using System.Data;

namespace Shopinv.Interface
{
    interface I_Package
    {
        DataSet savepackage(string Kitid, string formno);
        DataSet getPackage(string FormNo);
    }
}
