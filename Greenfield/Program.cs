using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Greenfield
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string basePath = null;

            var builder = Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseStartup<Startup>();

                      // This will be non-null is we're running in IIS since UseIIS will set a content root
                      basePath = webBuilder.GetSetting(WebHostDefaults.ContentRootKey);
                  });

            basePath ??= AppContext.BaseDirectory;

            using var argConfig = (ConfigurationRoot)new ConfigurationBuilder()
                        .AddCommandLine(args)
                        .Build();

            var contentRoot = argConfig[HostDefaults.ContentRootKey];

            if (!string.IsNullOrEmpty(contentRoot))
            {
                // Make the content root relative to the IIS root
                contentRoot = Path.GetFullPath(Path.Combine(basePath, contentRoot));

                builder.UseContentRoot(contentRoot);
            }

            return builder;
        }
    }
}
