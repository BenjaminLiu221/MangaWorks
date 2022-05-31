using MangaWorks.DataAccess;
using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using MangaWorks.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MangaWorksWeb.Controllers
{
    [Area("Admin")]
    public class ChapterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ChapterController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page(int? chapterId)
        {
            IEnumerable<Page> pageList = _unitOfWork.Page.GetDataFromDbSetUsingFk(a => a.ChapterId == chapterId);
            if (pageList.Count() > 0)
            {
                return View(pageList);
            }
            TempData["error"] = "Pages Not Found";
            return RedirectToAction("Upsert", new { id = chapterId });
        }

        public IActionResult Upsert(int? id)
        {
            ChapterVM chapterVM = new()
            {
                Chapter = new(),
                MangaList = _unitOfWork.Manga.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Title,
                    Value = a.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                //create manga
                //ViewBag.GenreList = GenreList;
                return View(chapterVM);
            }
            else
            {
                chapterVM.Chapter = _unitOfWork.Chapter.GetFirstOrDefault(a => a.Id == id);
                return View(chapterVM);
                //update manga
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ChapterVM chapterObj)
        {
            if (ModelState.IsValid)
            {
                if (chapterObj.Chapter.Id == 0)
                {
                    _unitOfWork.Chapter.Add(chapterObj.Chapter);
                    TempData["success"] = "Chapter created successfully";
                }
                else
                {
                    _unitOfWork.Chapter.Update(chapterObj.Chapter);
                    TempData["success"] = "Chapter updated successfully";
                }
                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(chapterObj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var chapterList = _unitOfWork.Chapter.GetAll(includeProperties: "Manga");
            return Json(new { data = chapterList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var chapterFromDbFirst = _unitOfWork.Chapter.GetFirstOrDefault(a => a.Id == id);
            if (chapterFromDbFirst == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Chapter.Remove(chapterFromDbFirst);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });

        }
        #endregion

    }
}
