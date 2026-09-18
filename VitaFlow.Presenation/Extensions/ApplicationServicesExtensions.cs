using VitaFlow.Domain.Interface;
using VitaFlow.Infrastructure.DapperContext;
using VitaFlow.Infrastructure.Repository;
using VitaFlow.Presenation.Services;

namespace VitaFlow.Presenation.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
            services.AddSingleton<DapperDbContext>();
            services.AddScoped<I_Login, LoginRepository>();
            services.AddScoped<I_Product, ProductRepository>();
            services.AddScoped<I_Report, ReportRepository>();
            services.AddScoped<I_Transaction, TransactionRepository>();
            services.AddMemoryCache();
            services.AddScoped<ICompanyInfoProvider, CompanyInfoProvider>();
            return services;
        }
    }
}
