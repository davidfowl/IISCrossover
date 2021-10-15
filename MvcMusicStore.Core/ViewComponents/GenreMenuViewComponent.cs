using Microsoft.AspNetCore.Mvc;
using MvcMusicStore.Models;
using System.Linq;

namespace MvcMusicStore.Core.ViewComponents
{
    public class GenreMenuViewComponent : ViewComponent
    {
        private MusicStoreEntities storeDB = new MusicStoreEntities();

        public GenreMenuViewComponent()
        {
        }

        public IViewComponentResult Invoke(int numberOfItems)
        {
            var genres = storeDB.Genres.ToList();
            return View(genres);
        }
    }
}
