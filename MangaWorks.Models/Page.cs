using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.Models
{
    public class Page
    {
        public int Id { get; set; }

        [Required]
        //[Range(1,100)]
        public int PageNumber { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        public int ChapterId { get; set; }
        [ValidateNever]
        public Chapter Chapter { get; set; }
    }
}
