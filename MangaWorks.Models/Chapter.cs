using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        [Required]
        [Range(1,10000)]
        public int ChapterNumber { get; set; }
        public DateTime Updated { get; set; }

        [Required]
        [Display(Name = "Manga")]
        public int MangaId { get; set; }
        [ValidateNever]
        public Manga Manga { get; set; }
    }
}
