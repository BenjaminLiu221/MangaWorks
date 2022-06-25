using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using Microsoft.AspNetCore.Mvc;

namespace MangaWorksWeb.Controllers
{
    [Area("User")]
    public class LatestMangaController : Controller
    {
        private readonly ILogger<LatestMangaController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public LatestMangaController(ILogger<LatestMangaController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").ToList();
            var lastChapterList = new List<Chapter>();
            var latestMangaList = new List<Manga>();

            foreach (var manga in mangaList)
            {
                var lastChapterOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderBy(a => a.ChapterNumber).LastOrDefault();
                lastChapterList.Add(lastChapterOfManga);
            }

            List<Chapter> orderedLastChapterList = lastChapterList.OrderByDescending(a => a.Updated).ToList();
            
            foreach(var chapter in orderedLastChapterList)
            {
                latestMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            LatestManga latestManga = new()
            {
                MangaList = latestMangaList,
            };
            return View(latestManga);
        }
    }
}
