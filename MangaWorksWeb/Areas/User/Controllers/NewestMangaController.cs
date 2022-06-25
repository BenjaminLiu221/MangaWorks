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

        public IActionResult Index()
        {
            var allFirstChaptersOfMangaByNewestUpdated = _unitOfWork.Chapter.GetAll().Where(a => a.ChapterNumber == 1).OrderByDescending(a => a.Updated).ToList();
            List<Manga> newestMangaList = new List<Manga>();

            foreach (var chapter in allFirstChaptersOfMangaByNewestUpdated)
            {
                newestMangaList.Add(_unitOfWork.Manga.GetFirstOrDefault(a => a.Id == chapter.MangaId, includeProperties: "Author"));
            }

            NewestManga newestManga = new()
            {
                MangaList = newestMangaList,
            };
            return View(newestManga);
        }
    }
}
