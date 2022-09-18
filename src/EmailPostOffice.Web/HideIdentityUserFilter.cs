using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmailPostOffice.Web
{
    public class HideIdentityUserFilter : IDocumentFilter
    {
        private List<string> pathToHides = new List<string> {
            "/api/account/profile-picture-file" ,
            "/api/gdpr/requests/data/",
        };

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var pathToHide in pathToHides)
            {
                var identityUserPaths = swaggerDoc
                    .Paths
                    .Where(pathItem => pathItem.Key.Contains(pathToHide, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var item in identityUserPaths)
                {
                    swaggerDoc.Paths.Remove(item.Key);
                }
            }
        }
    }
}
