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

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre genreObj)
        {
            if (genreObj.Name == genreObj.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _dbContext.Genres.Add(genreObj);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genreObj);
        }
    }
}
