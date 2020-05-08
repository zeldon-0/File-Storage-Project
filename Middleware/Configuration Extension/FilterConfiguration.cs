using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Middleware.Exception_Filtering;
namespace Middleware.Configuration_Extension
{
    public static class FilterConfiguration
    {
        public static void ConfigureFilters(this IServiceCollection services)
        {
            services.AddControllers(options =>
               options.Filters.Add(new HttpResponesExceptionFilter()));
        }
    }
}
