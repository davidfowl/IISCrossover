namespace MvcMusicStore.Core
{
    /// <summary>
    /// https://gist.github.com/davidfowl/0e0372c3c1d895c3ce195ba983b1e03d#i-like-the-startup-class-can-i-keep-it
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // REVIEW: This is bad, and we should feel bad, but we're making minimal change at the moment
        public static IConfiguration Configuration { get; private set; }
    }
}