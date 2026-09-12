using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;

namespace VitaFlow.Presenation.Services
{
    public interface ICompanyInfoProvider
    {
        Task<M_CompanyMaster> GetAsync();
    }

    /// <summary>
    /// Single source of the company details (name / address / phone / mail / website / logo)
    /// that used to be hard coded in the views. Reads M_CompanyMaster once and caches it,
    /// so every view can use it without hitting the DB on each request.
    /// </summary>
    public class CompanyInfoProvider : ICompanyInfoProvider
    {
        private const string CacheKey = "M_CompanyMaster.Current";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        private readonly I_Product i_Product;
        private readonly IMemoryCache cache;

        public CompanyInfoProvider(I_Product iProduct, IMemoryCache memoryCache)
        {
            i_Product = iProduct;
            cache = memoryCache;
        }

        public async Task<M_CompanyMaster> GetAsync()
        {
            if (cache.TryGetValue(CacheKey, out M_CompanyMaster cached) && cached != null)
            {
                return cached;
            }

            var comp = await i_Product.GetCompanyDetail() ?? new M_CompanyMaster();
            cache.Set(CacheKey, comp, CacheDuration);
            return comp;
        }
    }

    /// <summary>
    /// Display helpers so the views stay readable and every field has a safe fallback
    /// when the corresponding M_CompanyMaster column is blank.
    /// </summary>
    public static class CompanyInfoExtensions
    {
        private const string DefaultLogo = "~/assets/img/logo (1).png";

        public static string DisplayName(this M_CompanyMaster comp)
        {
            return First(comp?.CompName, comp?.CompTitle);
        }

        public static string DisplayAddress(this M_CompanyMaster comp)
        {
            return First(comp?.CompAdd, comp?.CompRegOffAdd);
        }

        public static string DisplayPhone(this M_CompanyMaster comp)
        {
            return First(comp?.MobileNo, comp?.ContactNo);
        }

        public static string DisplayEmail(this M_CompanyMaster comp)
        {
            return First(comp?.CompMail);
        }

        public static string DisplayWebsite(this M_CompanyMaster comp)
        {
            var site = First(comp?.WebSite, comp?.WebPortal);
            return site.Replace("https://", "").Replace("http://", "").TrimEnd('/');
        }

        public static string WebsiteHref(this M_CompanyMaster comp)
        {
            var site = First(comp?.WebSite, comp?.WebPortal);
            if (string.IsNullOrWhiteSpace(site))
            {
                return "#";
            }
            return site.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? site : "https://" + site;
        }

        public static string LogoUrl(this M_CompanyMaster comp, IUrlHelper url)
        {
            var logo = comp?.CompLogo;
            if (string.IsNullOrWhiteSpace(logo))
            {
                return url.Content(DefaultLogo);
            }
            if (logo.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                return logo;
            }
            return url.Content("~/" + logo.TrimStart('~', '/'));
        }

        private static string First(params string[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }
            return string.Empty;
        }
    }
}
