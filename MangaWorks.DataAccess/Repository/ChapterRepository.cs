using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.DataAccess.Repository
{
    public class ChapterRepository : Repository<Chapter>, IChapterRepository
    {
        private ApplicationDbContext _dbContext;

        public ChapterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public void Update(Chapter obj)
        {
        }
    }
}
