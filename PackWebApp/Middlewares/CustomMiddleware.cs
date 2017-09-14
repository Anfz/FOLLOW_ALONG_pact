using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PackWebApp.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next; 
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Debug.WriteLine($" --> Request asked for {httpContext.Request.Path}");

            await _next.Invoke(httpContext);
        }
    }
}
