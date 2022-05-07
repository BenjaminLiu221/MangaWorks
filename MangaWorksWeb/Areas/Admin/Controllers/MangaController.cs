﻿using MangaWorks.DataAccess;
using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using MangaWorks.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MangaWorksWeb.Controllers
{
    [Area("Admin")]
    public class MangaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MangaController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
            MangaVM mangaVM = new()
            {
                Manga = new(),
                GenreList = _unitOfWork.Genre.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }),
                AuthorList = _unitOfWork.Author.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                //create manga
                //ViewBag.GenreList = GenreList;
                return View(mangaVM);
            }
            else
            {
                //update manga
            }

            return View(mangaVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(MangaVM mangaObj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\mangas");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    mangaObj.Manga.ImageUrl = @"\images\mangas\" + fileName + extension;
                }

                _unitOfWork.Manga.Add(mangaObj.Manga);
                _unitOfWork.Save();
                TempData["success"] = "Manga created successfully";
                return RedirectToAction("Index");
            }
            return View(mangaObj);
        }

        public IActionResult Remove(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var genreFromDb = _dbContext.Genres.Find(id);
            var mangaFromDbFirst = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == id);
            if (mangaFromDbFirst == null)
            {
                return NotFound();
            }
            return View(mangaFromDbFirst);
        }

        //POST
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePOST(int? id)
        {
            var mangaFromDbFirst = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == id);
            if (mangaFromDbFirst == null)
            {
                return NotFound();
            }
            _unitOfWork.Manga.Remove(mangaFromDbFirst);
            _unitOfWork.Save();
            TempData["success"] = "Manga deleted successfully";
            return RedirectToAction("Index");

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var mangaList = _unitOfWork.Manga.GetAll(includeProperties:"Genre,Author");
            return Json(new { data = mangaList });
        }
        #endregion

    }
}
