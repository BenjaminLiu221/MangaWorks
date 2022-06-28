using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models
{
    public class MangaDetails
    {
        public Manga Manga { get; set; }
        [ValidateNever]
        public List<Chapter> ChapterList { get; set; }
        public List<Manga> MostPopularManga { get; set; }
        public IEnumerable<SelectListItem> GenreList { get; set; }
    }
}
