using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [ValidateNever]
        public List<Genre>? Genres { get; set; }
        [ValidateNever]
        public Dictionary<Genre, bool> GenreBool{ get; set; }
        [ValidateNever]
        public List<string> GenresList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AuthorList { get; set; }
    }
}
