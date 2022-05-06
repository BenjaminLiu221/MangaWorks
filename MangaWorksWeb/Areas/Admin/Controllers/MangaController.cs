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
            IEnumerable<Manga> objMangaList = _unitOfWork.Manga.GetAll();
            return View(objMangaList);
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
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\mangas");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
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
