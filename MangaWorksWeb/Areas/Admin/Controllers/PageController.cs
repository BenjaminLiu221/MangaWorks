using MangaWorks.DataAccess;
using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using MangaWorks.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MangaWorksWeb.Controllers
{
    [Area("Admin")]
    public class PageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PageController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            PageVM pageVM = new()
            {
                Page = new(),
                ChapterList = _unitOfWork.Chapter.GetAll().Select(a => new SelectListItem
                {
                    Text = "Manga Id: " + a.MangaId.ToString() + ", Chapter Id: " + a.Id.ToString() + ", ChapterNumber: " + a.ChapterNumber.ToString(),
                    //+ "" + a.ChapterNumber.ToString(),
                    Value = a.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                //create manga
                //ViewBag.GenreList = GenreList;
                return View(pageVM);
            }
            else
            {
                pageVM.Page = _unitOfWork.Page.GetFirstOrDefault(a => a.Id == id);
                return View(pageVM);
                //update manga
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PageVM pageObj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\pages");
                    var extension = Path.GetExtension(file.FileName);

                    if (pageObj.Page.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, pageObj.Page.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    pageObj.Page.ImageUrl = @"\images\pages\" + fileName + extension;
                }
                if (pageObj.Page.Id == 0)
                {
                    _unitOfWork.Page.Add(pageObj.Page);
                    TempData["success"] = "Page created successfully";
                }
                else
                {
                    _unitOfWork.Page.Update(pageObj.Page);
                    TempData["success"] = "Page updated successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(pageObj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var pageList = _unitOfWork.Page.GetAll();
            return Json(new { data = pageList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var pageFromDbFirst = _unitOfWork.Page.GetFirstOrDefault(a => a.Id == id);
            if (pageFromDbFirst == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Page.Remove(pageFromDbFirst);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });

        }
        #endregion

    }
}
