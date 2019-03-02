using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace WebStore.Infrastucture.Middleware
{
    public static class ApplicationBuildeExtension
    {
        public static IApplicationBuilder UseTestMiddleware(
            this IApplicationBuilder app, params object[] parameters)
        {
            if(app is null) throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<TestMiddleware>(parameters);
            return app;
        }
    }
}
