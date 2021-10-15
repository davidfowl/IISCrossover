using Microsoft.AspNetCore.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Core.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly IHttpContext _httpContext;

        public CartSummaryViewComponent(IHttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public IViewComponentResult Invoke(int numberOfItems)
        {
            var cart = ShoppingCart.GetCart(_httpContext);

            ViewData["CartCount"] = cart.GetCount();
            return View();
        }
    }
}
