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

            HotManga hotManga = new()
            {
                MangaList = hotMangaList,
            };
            return View(hotManga);
        }
    }
}
