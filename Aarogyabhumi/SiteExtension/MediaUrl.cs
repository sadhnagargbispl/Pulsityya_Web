using System;
using System.Configuration;

namespace Shopinv.SiteExtension
{
    /// <summary>
    /// Product / cart / category images come out of the database as absolute URLs
    /// that still point at the old franchise portal. Rather than rewriting the
    /// data, we re-host those URLs on the way out using the FranchiseUrl key in
    /// Web.config, so moving domains stays a config-only change.
    ///
    /// Only the host is swapped - the path and query are kept exactly as stored.
    /// Relative paths, non-http values and URLs that already point at the
    /// configured host are returned untouched.
    ///
    /// Set RehostMediaUrls to "false" in Web.config to switch this off once the
    /// database itself holds the correct URLs.
    /// </summary>
    public static class MediaUrl
    {
        private static readonly string FranchiseUrl = ConfigurationManager.AppSettings["FranchiseUrl"];

        private static readonly bool Enabled =
            !string.Equals(ConfigurationManager.AppSettings["RehostMediaUrls"], "false",
                           StringComparison.OrdinalIgnoreCase);

        private static readonly Uri Target = BuildTarget();

        private static Uri BuildTarget()
        {
            Uri uri;
            if (!string.IsNullOrWhiteSpace(FranchiseUrl) &&
                Uri.TryCreate(FranchiseUrl.Trim(), UriKind.Absolute, out uri))
            {
                return uri;
            }
            return null;
        }

        /// <summary>
        /// True when a mapped property should be treated as a media URL.
        /// Deliberately narrow: it must not catch fields such as
        /// E_OrderReport.Website, which is a courier tracking link.
        /// </summary>
        public static bool IsMediaProperty(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName)
                && propertyName.IndexOf("image", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Swaps the host of an absolute media URL for the configured FranchiseUrl host.
        /// Anything it cannot safely rewrite is returned unchanged.
        /// </summary>
        public static string Rehost(string value)
        {
            if (!Enabled || Target == null || string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Uri source;
            if (!Uri.TryCreate(value.Trim(), UriKind.Absolute, out source))
            {
                return value;   // relative path - leave it for the caller to prefix
            }

            if (source.Scheme != Uri.UriSchemeHttp && source.Scheme != Uri.UriSchemeHttps)
            {
                return value;   // data:, file:, mailto: etc.
            }

            if (string.Equals(source.Host, Target.Host, StringComparison.OrdinalIgnoreCase))
            {
                return value;   // already on the right host
            }

            try
            {
                UriBuilder rebuilt = new UriBuilder(source)
                {
                    Scheme = Target.Scheme,
                    Host = Target.Host,
                    Port = Target.IsDefaultPort ? -1 : Target.Port
                };
                return rebuilt.Uri.AbsoluteUri;
            }
            catch
            {
                return value;
            }
        }
    }
}
