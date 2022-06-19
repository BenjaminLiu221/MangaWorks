using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MangaWorks.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Manga>? Mangas { get; set; }
    }
}
