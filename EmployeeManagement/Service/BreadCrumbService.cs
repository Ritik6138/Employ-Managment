using EmpInfrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EmployeeManagement.Service
{
    public class BreadCrumbService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly LinkGenerator _link;
        public BreadCrumbService(IHttpContextAccessor httpContext, LinkGenerator link)
        {
            _httpContext = httpContext;
            _link = link;
        }

        public List<BreadCumbItem> GetBreadCumbs()
        {
            var breadCumbs = new List<BreadCumbItem>();
            var path = _httpContext.HttpContext.Request.Path.Value ?? "";
            var segments = path.Split('/')
                               .Where(segment => !string.IsNullOrEmpty(segment))
                               .ToList();

            breadCumbs.Add(new BreadCumbItem
            {
                Tittle = "Dashboard",
                Url = _link.GetPathByAction("Index", "DashBoard")
            });
            var filteredSegments = segments
                .Where(segment => !string.Equals(segment, "dashboard", System.StringComparison.OrdinalIgnoreCase)
                               && !string.Equals(segment, "index", System.StringComparison.OrdinalIgnoreCase))
                .ToList();
            for (int i = 0; i < filteredSegments.Count; i++)
            {
                bool isLast = i == filteredSegments.Count - 1;
                var url = isLast ? null : "/" + string.Join("/", filteredSegments.Take(i + 1));

                breadCumbs.Add(new BreadCumbItem
                {
                    Tittle = Humanize(filteredSegments[i]),
                    Url = url
                });
            }

            return breadCumbs;
        }

        private string Humanize(string segment)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(segment.Replace("-", " ").ToLower());
        }
    }
}
