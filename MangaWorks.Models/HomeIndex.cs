﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models
{
    public class HomeIndex
    {
        public Dictionary<Manga, List<Chapter>> MangaIndex { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GenreList { get; set; }
        public List<Manga> NewManga { get; set; }
        public List<Manga> MostPopularManga { get; set; }
        public List<Manga> TopWeekManga { get; set; }
    }
}