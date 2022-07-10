using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using Microsoft.AspNetCore.Mvc;

namespace MangaWorksWeb.Controllers
{
    [Area("User")]
    public class NewestMangaController : Controller
    {
        private readonly ILogger<NewestMangaController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public NewestMangaController(ILogger<NewestMangaController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [Route("newest/pageNumber-{pageNumber}")]
        public IActionResult Index(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 20;
            var allFirstChaptersOfMangaByNewestUpdated = _unitOfWork.Chapter.GetAll().Where(a => a.ChapterNumber == 1).OrderByDescending(a => a.Updated).Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).ToList();
            List<Manga> newestMangaList = new List<Manga>();

            foreach (var chapter in allFirstChaptersOfMangaByNewestUpdated)
            {
                newestMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
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

            NewestManga newestManga = new()
            {
                MangaList = newestMangaList,
                TopWeekManga = topWeekMangaList,
            };
            return View(newestManga);
        }
    }
}
