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
        public int ChapterNumber { get; set; }

        [Required]
        public int MangaId { get; set; }
        [ValidateNever]
        public Manga Manga { get; set; }
    }
}
