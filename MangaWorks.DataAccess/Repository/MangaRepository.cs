using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.DataAccess.Repository
{
    public class MangaRepository : Repository<Manga>, IMangaRepository
    {
        private ApplicationDbContext _dbContext;

        public MangaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public void Update(Manga obj)
        {
            var objFromDb = _dbContext.Mangas.FirstOrDefault(a => a.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Status = obj.Status;
                objFromDb.Updated = obj.Updated;
                objFromDb.MangaGenres = obj.MangaGenres;
                objFromDb.Rating = obj.Rating;
                objFromDb.Views = obj.Views;
                objFromDb.AuthorId = obj.AuthorId;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
