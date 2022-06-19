using MangaWorks.DataAccess.Repository.IRepository;
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
            IEnumerable<Manga> mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author");

            return View(mangaList);
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