using MangaWorks.DataAccess;
using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using MangaWorks.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaWorksWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class GenreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Genre> objGenreList = _unitOfWork.Genre.GetAll();
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
                _unitOfWork.Genre.Add(genreObj);
                _unitOfWork.Save();
                TempData["success"] = "Genre created successfully";
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
            //var genreFromDb = _dbContext.Genres.Find(id);
            var genreFromDbFirst = _unitOfWork.Genre.GetFirstOrDefault(a => a.Id == id);
            if (genreFromDbFirst == null)
            {
                return NotFound();
            } 
            return View(genreFromDbFirst);
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
                _unitOfWork.Genre.Update(genreObj);
                _unitOfWork.Save();
                TempData["success"] = "Genre updated successfully";
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
            //var genreFromDb = _dbContext.Genres.Find(id);
            var genreFromDbFirst = _unitOfWork.Genre.GetFirstOrDefault(a => a.Id == id);
            if (genreFromDbFirst == null)
            {
                return NotFound();
            }
            return View(genreFromDbFirst);
        }

        //POST
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePOST(int? id)
        {
            //var genreObj = _dbContext.Genres.Find(id);
            var genreFromDbFirst = _unitOfWork.Genre.GetFirstOrDefault(a => a.Id == id);
            if (genreFromDbFirst == null)
            {
                return NotFound();
            }
            _unitOfWork.Genre.Remove(genreFromDbFirst);
            _unitOfWork.Save();
            TempData["success"] = "Genre deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
