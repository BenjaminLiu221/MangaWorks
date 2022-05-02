using MangaWorks.Data;
using MangaWorks.Models;
using Microsoft.AspNetCore.Mvc;

namespace MangaWorks.Controllers
{
    public class BrowseController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BrowseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Genre> objGenreList = _dbContext.Genres;
            return View(objGenreList);
        }
    }
}
