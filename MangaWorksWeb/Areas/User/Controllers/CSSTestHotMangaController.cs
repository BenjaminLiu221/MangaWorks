using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using Microsoft.AspNetCore.Mvc;

namespace MangaWorksWeb.Areas.User.Controllers
{
    [Area("User")]
    public class CSSTestHotMangaController : Controller
    {
        private readonly ILogger<CSSTestHotMangaController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public CSSTestHotMangaController(ILogger<CSSTestHotMangaController> logger, IUnitOfWork unitOfWork)
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
