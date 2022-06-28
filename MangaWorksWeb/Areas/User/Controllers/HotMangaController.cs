using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using Microsoft.AspNetCore.Mvc;

namespace MangaWorksWeb.Controllers
{
    [Area("User")]
    public class HotMangaController : Controller
    {
        private readonly ILogger<HotMangaController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HotMangaController(ILogger<HotMangaController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
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

            HotManga hotManga = new()
            {
                MangaList = hotMangaList,
                TopWeekManga = topWeekMangaList,
            };
            return View(hotManga);
        }
    }
}
