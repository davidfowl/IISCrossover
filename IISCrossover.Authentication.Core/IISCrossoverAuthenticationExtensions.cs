using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISCrossover.Authentication.Core
{
    public static class IISCrossoverAuthenticationExtensions
    {
        public static void AddCrossoverAuthentication(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddIdentity<PocoUser, PocoRole>();

            var memoryStore = new InMemoryStore<PocoUser, PocoRole>();
            services.AddSingleton<IUserStore<PocoUser>>(serviceProvider => memoryStore);
            services.AddSingleton<IRoleStore<PocoRole>>(serviceProvider => memoryStore);
        }

        public static IApplicationBuilder UseCrossoverAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CrossoverAuthenticationMiddleware>();
        }
    }
}
