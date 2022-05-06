using MangaWorks.DataAccess;
using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using Microsoft.AspNetCore.Mvc;

namespace MangaWorksWeb.Controllers
{
    [Area("Admin")]
    public class MangaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MangaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Manga> objMangaList = _unitOfWork.Manga.GetAll();
            return View(objMangaList);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Manga manga = new();
            if (id == null || id == 0)
            {
                //create manga
                return View(manga);
            }
            else
            {
                //update manga
            }
            
            return View(manga);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Genre genreObj)
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

        ////GET
        //public IActionResult Remove(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //var genreFromDb = _dbContext.Genres.Find(id);
        //    var genreFromDbFirst = _unitOfWork.Genre.GetFirstOrDefault(a => a.Id == id);
        //    if (genreFromDbFirst == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(genreFromDbFirst);
        //}

        ////POST
        //[HttpPost, ActionName("Remove")]
        //[ValidateAntiForgeryToken]
        //public IActionResult RemovePOST(int? id)
        //{
        //    //var genreObj = _dbContext.Genres.Find(id);
        //    var genreFromDbFirst = _unitOfWork.Genre.GetFirstOrDefault(a => a.Id == id);
        //    if (genreFromDbFirst == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Genre.Remove(genreFromDbFirst);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Genre deleted successfully";
        //    return RedirectToAction("Index");
        //}
    }
}
