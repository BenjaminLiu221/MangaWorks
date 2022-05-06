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

        public double Rating { get; set; }
        public int Views { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [Required]
        public int GenreId { get; set; }
        [ValidateNever]
        public Genre Genre { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [ValidateNever]
        public Author Author { get; set; }
    }
}
