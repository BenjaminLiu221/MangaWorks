using MangaWorks.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaWorks.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
    }
}
