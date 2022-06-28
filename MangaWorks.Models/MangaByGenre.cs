using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models
{
    public class MangaByGenre
    {
        public List<Manga> MangaList { get; set; }
        public List<Manga> TopWeekManga { get; set; }
    }
}
