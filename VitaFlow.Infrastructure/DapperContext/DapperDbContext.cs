using System.Data;
using System.Data.SqlClient;
using VitaFlow.Infrastructure.Helper;

namespace VitaFlow.Infrastructure.DapperContext
{
	public class DapperDbContext
	{
		private readonly string LiveConn;
		private readonly string LiveconnInv;
		public DapperDbContext()
		{
			LiveConn = ConfigurationManager.AppSetting.GetSection("ConnectionStrings:LiveConn").Value;
			LiveconnInv = ConfigurationManager.AppSetting.GetSection("ConnectionStrings:LiveconnInv").Value;
		}
		public IDbConnection CreateConnection()
		=> new SqlConnection(LiveConn);
		public IDbConnection CreateLiveconnInv()
			=> new SqlConnection(LiveconnInv);
	}
}
