using MangaWorks.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaWorks.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Manga> Mangas { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
    }
}
