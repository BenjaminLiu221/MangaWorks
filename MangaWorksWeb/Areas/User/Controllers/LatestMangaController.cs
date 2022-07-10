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

        [Route("latestmanga/pageNumber-{pageNumber}")]
        public IActionResult Index(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 20;
            var mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author").Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).ToList();
            var lastChapterList = new List<Chapter>();
            var latestMangaList = new List<Manga>();

            var mangaListByVotesDesc = _unitOfWork.Manga.GetAll(includeProperties: "Author").OrderByDescending(a => a.NumberOfRatings).ToList();
            var topWeekMangaList = new List<Manga>();

            foreach (var manga in mangaList)
            {
                var lastChapterOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderBy(a => a.ChapterNumber).FirstOrDefault();
                if (lastChapterOfManga != null)
                {
                    lastChapterList.Add(lastChapterOfManga);
                }
            }

            List<Chapter> descendingLastChapterList = lastChapterList.OrderByDescending(a => a.Updated).ToList();
            
            foreach(var chapter in descendingLastChapterList)
            {
                latestMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            foreach (var manga in mangaListByVotesDesc)
            {
                var lastChapterOfManga = _unitOfWork.Chapter.GetAll().Where(a => a.MangaId == manga.Id).OrderByDescending(a => a.ChapterNumber).FirstOrDefault();
                if (lastChapterOfManga != null)
                {
                    lastChapterList.Add(lastChapterOfManga);
                }
            }

            //topWeekMangaList
            List<Chapter> orderedLastChapterList = lastChapterList.OrderByDescending(a => a.Updated).ToList();

            foreach (var chapter in orderedLastChapterList)
            {
                topWeekMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            LatestManga latestManga = new()
            {
                MangaList = latestMangaList,
                TopWeekManga = topWeekMangaList,
            };
            return View(latestManga);
        }
    }
}
