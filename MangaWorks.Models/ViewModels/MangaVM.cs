using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models.ViewModels
{
    public class MangaVM
    {
        public Manga Manga { get; set; }
        public IEnumerable<SelectListItem> GenreList { get; set; }
        public IEnumerable<SelectListItem> AuthorList { get; set; }
    }
}
