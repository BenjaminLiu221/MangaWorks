using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.DataAccess.Repository
{
    public class MangaPageRepository : Repository<MangaPage>, IMangaPageRepository
    {
        private ApplicationDbContext _dbContext;

        public MangaPageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public void Update(MangaPage obj)
        {
            var objFromDb = _dbContext.MangaPages.FirstOrDefault(a => a.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.PageNumber = obj.PageNumber;
                objFromDb.MangaId = obj.MangaId;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
