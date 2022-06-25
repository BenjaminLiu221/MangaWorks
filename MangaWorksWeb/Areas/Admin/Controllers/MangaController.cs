using MangaWorks.DataAccess;
using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using MangaWorks.Models.ViewModels;
using MangaWorks.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MangaWorksWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class MangaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MangaController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChapterDetails(int? chapterId)
        {
            ChapterDetailsVM chapterDetailsVM = new()
            {
                Chapter = new(),
                MangaList = _unitOfWork.Manga.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Title,
                    Value = a.Id.ToString()
                })
            };

            if (chapterId == null || chapterId == 0)
            {
                return View(chapterDetailsVM);
            }
            else
            {
                chapterDetailsVM.Chapter = _unitOfWork.Chapter.GetFirstOrDefault(a => a.Id == chapterId);
                return View(chapterDetailsVM);
                //update manga
            }
        }

        public IActionResult Chapter(int? mangaId)
        {
            IEnumerable<Chapter> chapterList = _unitOfWork.Chapter.GetDataFromDbSetUsingFk(a => a.MangaId == mangaId);
            if (chapterList.Count() > 0)
            {
                return View(chapterList);
            }
            TempData["error"] = "Chapters Not Found";
            return RedirectToAction("Upsert", new { id = mangaId });
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                var genreBool = new Dictionary<Genre, bool>();
                foreach (var item in _unitOfWork.Genre.GetAll())
                {
                    genreBool.Add(item, false);
                }
                MangaVM mangaVM = new()
                {
                    Manga = new(),
                    GenreBool = genreBool,
                    AuthorList = _unitOfWork.Author.GetAll().Select(a => new SelectListItem
                    {
                        Text = a.Name,
                        Value = a.Id.ToString()
                    })
                };
                return View(mangaVM);
            }
            else
            {
                var listOfAllGenres = new List<string>();
                var genreBool = new Dictionary<Genre, bool>();
                foreach (var item in _unitOfWork.Genre.GetAll())
                {
                    listOfAllGenres.Add(item.Name);
                }
                MangaVM mangaVM = new()
                {
                    Manga = new(),
                    GenreBool = genreBool,
                    AuthorList = _unitOfWork.Author.GetAll().Select(a => new SelectListItem
                    {
                        Text = a.Name,
                        Value = a.Id.ToString()
                    })
                };
                mangaVM.Manga = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == id);

                var mangaGenres = mangaVM.Manga.MangaGenres;
                List<string> mangaGenresList = mangaGenres.Split(' ').ToList();

                foreach (string genre in listOfAllGenres)
                {
                    if (mangaGenresList.Contains(genre))
                    {
                        foreach (var item in _unitOfWork.Genre.GetAll())
                        {
                            if (item.Name == genre)
                            {
                                genreBool.Add(item, true);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in _unitOfWork.Genre.GetAll())
                        {
                            if (item.Name == genre)
                            {
                                genreBool.Add(item, false);
                            }
                        }
                    }
                }
                mangaVM.GenreBool = genreBool;
                return View(mangaVM);
                //update manga
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(MangaVM mangaObj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\mangas");
                    var extension = Path.GetExtension(file.FileName);

                    if (mangaObj.Manga.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, mangaObj.Manga.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    mangaObj.Manga.ImageUrl = @"\images\mangas\" + fileName + extension;
                }
                if (mangaObj.Manga.Id == 0)
                {
                    //post method needs var genres
                    var genres = new List<Genre>();
                    string mangaGenres = "";
                    foreach (var item in mangaObj.GenresList)
                    {
                        genres.Add(_unitOfWork.Genre.GetAll().Where(a => a.Name == item).First());
                    }
                    foreach (var genre in genres)
                    {
                        if (mangaGenres == "")
                        {
                            mangaGenres = String.Concat(mangaGenres, genre.Name);
                        }
                        else
                        {
                            mangaGenres = String.Concat(mangaGenres, " ", genre.Name);
                        }
                    }
                    mangaObj.Manga.Genres = genres;
                    mangaObj.Manga.MangaGenres = mangaGenres;
                    _unitOfWork.Manga.Add(mangaObj.Manga);
                    TempData["success"] = "Manga created successfully";
                }
                else
                {
                    string mangaGenres = "";
                    foreach (var genre in mangaObj.GenresList)
                    {
                        if (mangaGenres == "")
                        {
                            mangaGenres = String.Concat(mangaGenres, genre);
                        }
                        else
                        {
                            mangaGenres = String.Concat(mangaGenres, " ", genre);
                        }
                    }
                    mangaObj.Manga.MangaGenres = mangaGenres;
                    _unitOfWork.Manga.Update(mangaObj.Manga);
                    TempData["success"] = "Manga updated successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(mangaObj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var mangaList = _unitOfWork.Manga.GetAll(includeProperties: "Author");
            return Json(new { data = mangaList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var mangaFromDbFirst = _unitOfWork.Manga.GetFirstOrDefault(a => a.Id == id);
            if (mangaFromDbFirst == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, mangaFromDbFirst.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Manga.Remove(mangaFromDbFirst);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });

        }
        #endregion

    }
}
