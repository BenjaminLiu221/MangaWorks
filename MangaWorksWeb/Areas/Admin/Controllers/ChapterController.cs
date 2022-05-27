﻿using MangaWorks.DataAccess;
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

        //GET
        public IActionResult Upsert(int? id)
        {
            ChapterVM chapterVM = new()
            {
                Chapter = new(),
                //ChapterList = _unitOfWork.Chapter.GetAll().Select(a => new SelectListItem
                //{
                //    Text = a.Name,
                //    Value = a.Id.ToString()
                //}),
                //PageList = _unitOfWork.Page.GetAll().Select(a => new SelectListItem
                //{
                //    Text = a.Name,
                //    Value = a.Id.ToString()
                //})
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(MangaVM mangaObj, IFormFile? file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _hostEnvironment.WebRootPath;
        //        if (file != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString();
        //            var uploads = Path.Combine(wwwRootPath, @"images\mangas");
        //            var extension = Path.GetExtension(file.FileName);

        //            if (mangaObj.Manga.ImageUrl != null)
        //            {
        //                var oldImagePath = Path.Combine(wwwRootPath, mangaObj.Manga.ImageUrl.TrimStart('\\'));
        //                if (System.IO.File.Exists(oldImagePath))
        //                {
        //                    System.IO.File.Delete(oldImagePath);
        //                }
        //            }

        //            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
        //            {
        //                file.CopyTo(fileStreams);
        //            }
        //            mangaObj.Manga.ImageUrl = @"\images\mangas\" + fileName + extension;
        //        }
        //        if (mangaObj.Manga.Id == 0)
        //        {
        //            _unitOfWork.Manga.Add(mangaObj.Manga);
        //        }
        //        else
        //        {
        //            _unitOfWork.Manga.Update(mangaObj.Manga);
        //        }
        //        _unitOfWork.Save();
        //        TempData["success"] = "Manga created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(mangaObj);
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Genre,Author");
            return Json(new { data = mangaList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var mangaFromDbFirst = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == id);
            if (mangaFromDbFirst == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, mangaFromDbFirst.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Manga.Remove(mangaFromDbFirst);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });

        }
        #endregion

    }
}