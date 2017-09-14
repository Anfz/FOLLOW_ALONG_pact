using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace PackWebApp.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MyConfiguration _myConfig; 

        public CustomMiddleware(RequestDelegate next, IOptions<MyConfiguration> myconfig)
        {
            _next = next;
            _myConfig = myconfig.Value;

        }

        public async Task Invoke(HttpContext httpContext)
        {
            Debug.WriteLine($" --> Request asked for {httpContext.Request.Path} from {_myConfig.FirstName} {_myConfig.LastName}");

            await _next.Invoke(httpContext);
        }
    }
}
