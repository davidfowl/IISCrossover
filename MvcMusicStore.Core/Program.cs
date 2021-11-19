using MvcMusicStore;
using MvcMusicStore.Core;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
builder.WebHost.ConfigureAppConfiguration(config =>
{
    // Fix up the data directory in config, EF6 on .NET Core doesn't seem to like it
    // (we get ArgumentException: Invalid value for key 'attachdbfilename'.). Follow up with the EF team.

    using var tempConfig = (ConfigurationManager)config.Build();

    var connectionStringKey = "ConnectionStrings:MusicStoreEntities";

    var value = tempConfig[connectionStringKey];

    // Fix up the connection string's path to the data directory
    value = value.Replace("|DataDirectory|", $"{Directory.GetCurrentDirectory()}\\App_Data");

    config.AddInMemoryCollection(new Dictionary<string, string>
                      {
                          { connectionStringKey, value }
                      });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(IISCrossoverAuthenticationDefaults.AuthenticationScheme)
    .AddIISCrossoverAuthentication();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IHttpContext), typeof(HttpContextImpl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseIISCrossoverSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();