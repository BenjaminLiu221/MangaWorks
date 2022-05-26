using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models
{
    public class MangaPage
    {
        public int Id { get; set; }

        [Required]
        //[Range(1,100)]
        public int PageNumber { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int MangaId { get; set; }
        public Manga Manga { get; set; }
    }
}
