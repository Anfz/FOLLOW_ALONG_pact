using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PackWebApp.Services;

namespace PackWebApp.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }

        public static async void AddSeedData(this IApplicationBuilder app)
        {
            var seedDataService = app.ApplicationServices.GetRequiredService<ISeedDataService>();
            await seedDataService.EnsureSeedData();
        }
    }
}
