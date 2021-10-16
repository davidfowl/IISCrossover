using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MvcMusicStore.Core
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
                  })
                  .ConfigureAppConfiguration(config =>
                  {
                      // Fix up the data directory in config, EF6 on .NET Core doesn't seem to like it
                      // (we get ArgumentException: Invalid value for key 'attachdbfilename'.). Follow up with the EF team.

                      using var tempConfig = (ConfigurationRoot)config.Build();

                      var connectionStringKey = "ConnectionStrings:MusicStoreEntities";

                      var value = tempConfig[connectionStringKey];

                      // Fix up the data directory
                      value = value.Replace("|DataDirectory|", Path.Combine(basePath, "App_Data"));

                      config.AddInMemoryCollection(new Dictionary<string, string>
                      {
                          { connectionStringKey, value }
                      });
                  });

            basePath ??= AppContext.BaseDirectory;

            // This doesn't seem to work
            // AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(basePath, "App_Data"));

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
