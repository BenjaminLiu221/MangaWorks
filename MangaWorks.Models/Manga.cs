using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models
{
    public class Manga
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Status { get; set; }
        public int Chapters { get; set; }
        public DateTime Updated { get; set; }
        [Range(1,5)]
        public double Rating { get; set; }
        public int Views { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public ICollection<Genre>? Genres { get; set; }
        [Required]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }
        [ValidateNever]
        public Author Author { get; set; }
    }
}
