using System.Data.Entity;
#if NETCOREAPP
using Microsoft.Extensions.Configuration;
using MvcMusicStore.Core;
#endif

namespace MvcMusicStore.Models
{
    public class MusicStoreEntities : DbContext
    {
#if NETCOREAPP
        // TODO: Use constructor injection...
        public MusicStoreEntities() :
            base(Startup.Configuration.GetConnectionString(nameof(MusicStoreEntities)))
        {

        }
#endif
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}