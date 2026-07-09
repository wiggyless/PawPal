using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Options
{
    public sealed class AppUrlsOptions
    {
        public const string SectionName = "AppUrls";
        public string ClientBaseUrl { get; init; } = string.Empty;
    }
}
