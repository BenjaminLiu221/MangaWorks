using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models.ViewModels
{
    public class ChapterVM
    {
        public Chapter Chapter { get; set; }
        //public int MangaId { get; set; }
        //public Manga Manga { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GenreList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AuthorList { get; set; }
    }
}
