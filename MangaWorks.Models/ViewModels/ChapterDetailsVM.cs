﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models.ViewModels
{
    public class ChapterDetailsVM
    {
        public Chapter Chapter { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> MangaList { get; set; }
    }
}
