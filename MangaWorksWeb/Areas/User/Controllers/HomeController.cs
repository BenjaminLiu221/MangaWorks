﻿using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using MangaWorks.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace MangaWorksWeb.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //IEnumerable<Manga> mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author");
            //return View(mangaList);

            MangaIndexGenreIndex mangaIndexGenreIndexVM = new()
            {
                MangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").ToList(),
                GenreList = _unitOfWork.Genre.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }
            )};
            return View(mangaIndexGenreIndexVM);
        }

        public IActionResult Details(int id)
        {
            MangaDetails mangaDetailsObj = new()
            {
                Manga = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == id, includeProperties: "Author"),
                ChapterList = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == id).Select(a => new SelectListItem
                {
                    Text = a.ChapterNumber.ToString(),
                    Value = a.Id.ToString()
                })
            };
            return View(mangaDetailsObj);
        }

        public IActionResult MangaByGenres(int id)
        {
            var genreObj = _unitOfWork.Genre.GetFirstOrDefault(a => a.Id == id);
            var mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").ToList();
            List<Manga> mangaListByGenre = new();
            foreach (var manga in mangaList)
            {
                List<string> mangaGenres = manga.MangaGenres.Split('*').ToList();
                if(mangaGenres.Contains(genreObj.Name))
                {
                    mangaListByGenre.Add(manga);
                }
            }

            MangaByGenre mangaByGenre = new()
            {
                MangaList = mangaListByGenre
            };
            return View(mangaByGenre);
        }

        public IActionResult Page(int id)
        {
            IEnumerable<Page> pageList = _unitOfWork.Page.GetDataFromDbSetUsingFk(a => a.ChapterId == id);
            return View(pageList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}