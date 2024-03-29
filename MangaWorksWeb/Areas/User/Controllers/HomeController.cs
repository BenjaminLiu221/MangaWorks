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

            var mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").ToList();
            var lastChapterList = new List<Chapter>();
            var latestMangaList = new List<Manga>();
            var mangaIndexDict = new Dictionary<Manga, List<Chapter>>();
            var newMangaList = new List<Manga>();
            int newMangaListCount = 0;
            var mostPopularMangaList = new List<Manga>();
            var mangaListByVotesDesc = _unitOfWork.Manga.GetAll(includeProperties: "Author").OrderByDescending(a => a.NumberOfRatings).ToList();
            int mostPopularMangaListCount = 0;
            //var mostPopularMangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").Where(a => a.NumberOfRatings == 0).OrderByDescending(a => a.NumberOfRatings).ToList();

            foreach (var manga in mangaListByVotesDesc)
            {
                if (mostPopularMangaListCount < 10)
                {
                    mostPopularMangaList.Add(manga);
                    mostPopularMangaListCount++;
                }
            }

            foreach (var manga in mangaList)
            {
                var lastChapterOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderByDescending(a => a.ChapterNumber).FirstOrDefault();
                if (lastChapterOfManga != null)
                {
                    lastChapterList.Add(lastChapterOfManga);
                }
            }

            List<Chapter> orderedLastChapterList = lastChapterList.OrderByDescending(a => a.Updated).ToList();

            foreach (var chapter in orderedLastChapterList)
            {
                latestMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            foreach (var manga in latestMangaList)
            {
                var allChaptersOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderByDescending(a => a.ChapterNumber).ToList();
                var chapterList = new List<Chapter>();
                int chapterListCount = 0;
                if (allChaptersOfManga.Count() == 0)
                {
                    mangaIndexDict.Add(manga, chapterList);
                }
                else
                {
                    foreach (var chapter in allChaptersOfManga)
                    {
                        if (chapterListCount < 3)
                        {
                            chapterList.Add(chapter);
                            chapterListCount++;
                        }
                    }
                    mangaIndexDict.Add(manga, chapterList);
                }
            }

            foreach (var manga in mangaList)
            {
                if (newMangaListCount < 10)
                {
                    var allFirstChaptersOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id && a.ChapterNumber == 1).OrderByDescending(a => a.Updated).ToList();
                    newMangaList.Add(manga);
                    newMangaListCount++;
                }
            }

            //TopWeekManga
            var hotMangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").OrderByDescending(a => a.NumberOfRatings).ToList();
            var topWeekMangaList = new List<Manga>();
            var topWeekLastChapterList = new List<Chapter>();

            foreach (var manga in hotMangaList)
            {
                var lastChapterOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderByDescending(a => a.ChapterNumber).FirstOrDefault();
                if (lastChapterOfManga != null)
                {
                    topWeekLastChapterList.Add(lastChapterOfManga);
                }
            }

            List<Chapter> topWeekOrderedLastChapterList = topWeekLastChapterList.OrderByDescending(a => a.Updated).ToList();

            foreach (var chapter in topWeekOrderedLastChapterList)
            {
                topWeekMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            HomeIndex homeIndexVM = new()
            {
                MangaIndex = mangaIndexDict,
                NewManga = newMangaList,
                MostPopularManga = mostPopularMangaList,
                TopWeekManga = topWeekMangaList,
                GenreList = _unitOfWork.Genre.GetAll().OrderBy(a => a.Name).Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }
            ) };
            return View(homeIndexVM);
        }

        [Route("manga-{id}")]
        public IActionResult Manga(int id)
        {
            var mostPopularMangaList = new List<Manga>();
            var mangaListByVotesDesc = _unitOfWork.Manga.GetAll(includeProperties: "Author").OrderByDescending(a => a.NumberOfRatings).ToList();
            int mostPopularMangaListCount = 0;
            var lastChapterList = new List<Chapter>();
            var topWeekMangaList = new List<Manga>();

            foreach (var manga in mangaListByVotesDesc)
            {
                if (mostPopularMangaListCount < 10)
                {
                    mostPopularMangaList.Add(manga);
                    mostPopularMangaListCount++;
                }
            }

            foreach (var manga in mangaListByVotesDesc)
            {
                var lastChapterOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderByDescending(a => a.ChapterNumber).FirstOrDefault();
                if (lastChapterOfManga != null)
                {
                    lastChapterList.Add(lastChapterOfManga);
                }
            }

            List<Chapter> orderedLastChapterList = lastChapterList.OrderByDescending(a => a.Updated).ToList();

            foreach (var chapter in orderedLastChapterList)
            {
                topWeekMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            MangaDetails mangaDetailsObj = new()
            {
                Manga = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == id, includeProperties: "Author"),
                ChapterList = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == id).OrderByDescending(a => a.ChapterNumber).ToList(),
                MostPopularManga = mostPopularMangaList,
                TopWeekManga = topWeekMangaList,
                GenreList = _unitOfWork.Genre.GetAll().OrderBy(a => a.Name).Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }
            ) };
            return View(mangaDetailsObj);
        }

        [Route("genre-{id}/pageNumber-{pageNumber}")]
        public IActionResult Genre(int id, int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 20;
            var genreObj = _unitOfWork.Genre.GetFirstOrDefault(a => a.Id == id);
            var mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").ToList();
            List<Manga> mangaListByGenre = new();
            foreach (var manga in mangaList)
            {
                List<string> mangaGenres = manga.MangaGenres.Split(' ').ToList();
                if(mangaGenres.Contains(genreObj.Name))
                {
                    mangaListByGenre.Add(manga);
                }
            }

            //TopWeekManga
            var hotMangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").OrderByDescending(a => a.NumberOfRatings).ToList();
            var topWeekMangaList = new List<Manga>();
            var lastChapterList = new List<Chapter>();

            foreach (var manga in hotMangaList)
            {
                var lastChapterOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderByDescending(a => a.ChapterNumber).FirstOrDefault();
                if (lastChapterOfManga != null)
                {
                    lastChapterList.Add(lastChapterOfManga);
                }
            }

            List<Chapter> orderedLastChapterList = lastChapterList.OrderByDescending(a => a.Updated).ToList();

            foreach (var chapter in orderedLastChapterList)
            {
                topWeekMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            MangaByGenre mangaByGenre = new()
            {
                MangaList = mangaListByGenre.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).ToList(),
                TopWeekManga = topWeekMangaList,
            };
            return View(mangaByGenre);
        }

        [HttpGet]
        //[Route("manga-{manga_Id}/chapter-{chapterNumber}/{chapter_Id}")]
        public IActionResult PageManga(int chapter_Id, int chapterNumber, int manga_Id)
        {
            //previousChapter
            var previousChapter = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga_Id).Where(b => b.ChapterNumber < chapterNumber).OrderBy(c => c.ChapterNumber).FirstOrDefault();

            //nextChapter
            var nextChapter = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga_Id).Where(b => b.ChapterNumber > chapterNumber).OrderBy(c => c.ChapterNumber).FirstOrDefault();

            var pageList = _unitOfWork.Page.GetDataFromDbSetUsingFk(a => a.ChapterId == chapter_Id).ToList();
            //return View(pageList);

            //topWeekManga
            var hotMangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").OrderByDescending(a => a.NumberOfRatings).ToList();

            //TopWeekManga
            var topWeekMangaList = new List<Manga>();
            var lastChapterList = new List<Chapter>();

            foreach (var manga in hotMangaList)
            {
                var lastChapterOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderByDescending(a => a.ChapterNumber).FirstOrDefault();
                if (lastChapterOfManga != null)
                {
                    lastChapterList.Add(lastChapterOfManga);
                }
            }

            List<Chapter> orderedLastChapterList = lastChapterList.OrderByDescending(a => a.Updated).ToList();

            foreach (var chapter in orderedLastChapterList)
            {
                topWeekMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            if (pageList.Count() == 0)
            {
                var firstChapter = _unitOfWork.Chapter.GetFirstOrDefault(a => a.Id == chapter_Id);
                var mangaObj = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == firstChapter.MangaId);
                PageManga pageManga = new()
                {
                    MangaId = mangaObj.Id,
                    MangaTitle = mangaObj.Title,
                    ChapterId = firstChapter.Id,
                    ChapterNumber = firstChapter.ChapterNumber,
                    TopWeekManga = topWeekMangaList,
                };
                return View(pageManga);
            }
            else
            {
                var firstChapter = _unitOfWork.Chapter.GetFirstOrDefault(a => a.Id == chapter_Id);
                var mangaObj = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == firstChapter.MangaId);

                PageManga pageManga = new()
                {
                    PageList = pageList,
                    PreviousChapter = previousChapter,
                    NextChapter = nextChapter,
                    MangaId = mangaObj.Id,
                    MangaTitle = mangaObj.Title,
                    ChapterId = firstChapter.Id,
                    ChapterNumber = firstChapter.ChapterNumber,
                    TopWeekManga = topWeekMangaList,
                    ChapterList = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga_Id).Select(a => new SelectListItem
                    {
                        Text = a.ChapterNumber.ToString(),
                        Value = a.Id.ToString()
                    })
                };
                return View(pageManga);
            }
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