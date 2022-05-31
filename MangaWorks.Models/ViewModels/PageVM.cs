using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models.ViewModels
{
    public class PageVM
    {
        public Page Page { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ChapterList { get; set; }
    }
}
