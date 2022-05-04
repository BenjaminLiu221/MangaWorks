using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.DataAccess.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private ApplicationDbContext _dbContext;

        public GenreRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Genre obj)
        {
            _dbContext.Genres.Update(obj);
        }
    }
}
