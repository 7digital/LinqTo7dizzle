using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LinqTo7Dizzle;
using LinqTo7Dizzle.Entities;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
		private readonly SevenDizzleContext _context = new SevenDizzleContext("http://api.7digital.com/1.2/");

        public ActionResult Index()
        {
            // Get most popular albums
            var albums = GetTopSellingAlbums(5);

            return View(albums);
        }

        private List<Release> GetTopSellingAlbums(int count)
        {
        	var topSellingAlbums = _context.Chart<Release>()
				.Take(count)
        		.ToList();

        	return topSellingAlbums;
        }
    }
}
