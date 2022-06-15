using MangaWorks.DataAccess;
using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using MangaWorks.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaWorksWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AuthorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Author> objAuthorList = _unitOfWork.Author.GetAll();
            return View(objAuthorList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author authorObj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Author.Add(authorObj);
                _unitOfWork.Save();
                TempData["success"] = "Author created successfully";
                return RedirectToAction("Index");
            }
            return View(authorObj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var authorFromDbFirst = _unitOfWork.Author.GetFirstOrDefault(a => a.Id == id);
            if (authorFromDbFirst == null)
            {
                return NotFound();
            } 
            return View(authorFromDbFirst);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author authorObj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Author.Update(authorObj);
                _unitOfWork.Save();
                TempData["success"] = "Author updated successfully";
                return RedirectToAction("Index");
            }
            return View(authorObj);
        }

        //GET
        public IActionResult Remove(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var authorFromDbFirst = _unitOfWork.Author.GetFirstOrDefault(a => a.Id == id);
            if (authorFromDbFirst == null)
            {
                return NotFound();
            }
            return View(authorFromDbFirst);
        }

        //POST
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePOST(int? id)
        {
            var authorFromDbFirst = _unitOfWork.Author.GetFirstOrDefault(a => a.Id == id);
            if (authorFromDbFirst == null)
            {
                return NotFound();
            }
            _unitOfWork.Author.Remove(authorFromDbFirst);
            _unitOfWork.Save();
            TempData["success"] = "Author deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
