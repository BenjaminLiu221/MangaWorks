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

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var genreFromDb = _dbContext.Genres.Find(id);
            if (genreFromDb == null)
            {
                return NotFound();
            }
            return View(genreFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Genre genreObj)
        {
            if (genreObj.Name == genreObj.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _dbContext.Genres.Update(genreObj);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genreObj);
        }

        //GET
        public IActionResult Remove(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var genreFromDb = _dbContext.Genres.Find(id);
            if (genreFromDb == null)
            {
                return NotFound();
            }
            return View(genreFromDb);
        }

        //POST
        [HttpPost,ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePOST(int? id)
        {
            var genreObj = _dbContext.Genres.Find(id);
            if (genreObj == null)
            {
                return NotFound();
            }
            _dbContext.Genres.Remove(genreObj);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
