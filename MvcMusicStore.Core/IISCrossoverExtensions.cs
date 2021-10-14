using IISCrossover;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text.Json;

namespace MvcMusicStore.Core
{
    public static class IISCrossoverExtensions
    {
        public static IServiceCollection AddIISCrossover(this IServiceCollection services)
        {
            services.AddIISCrossoverAuthentication();
            services.AddIISCrossoverSession();
            return services;
        }

        public static IServiceCollection AddIISCrossoverSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();

            return services;
        }

        public static IServiceCollection AddIISCrossoverAuthentication(this IServiceCollection services)
        {
            var uniqueId = "8eddae4a-3f82-4276-afd5-714104e7a43a";
            var scheme = $"iAmNotACookie{uniqueId}";
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = scheme;
            })
            .AddScheme<IISCrossoverAuthenticationSchemeOptions, IISCrossoverAuthenticationHandler>(scheme, op => { });

            return services;
        }

        public static IApplicationBuilder UseIISCrossoverSession(this IApplicationBuilder app)
        {
            app.UseSession();
            app.UseMiddleware<IISCrossoverSessionMiddleware>();

            return app;
        }
    }
}
