using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models
{
    public class PageManga
    {
        public List<Page> PageList { get; set; }
        public Chapter NextChapter { get; set; }
        public Chapter PreviousChapter { get; set; }
        public int MangaId { get; set; }
        public string MangaTitle { get; set; }
        public int ChapterId { get; set; }
        public int ChapterNumber { get; set; }
        public List<Manga> TopWeekManga { get; set; }
        public IEnumerable<SelectListItem> ChapterList { get; set; }
    }
}
