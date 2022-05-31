using MangaWorks.DataAccess.Repository.IRepository;
using MangaWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.DataAccess.Repository
{
    public class PageRepository : Repository<Page>, IPageRepository
    {
        private ApplicationDbContext _dbContext;

        public PageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

        public void Update(Page obj)
        {
        }
    }
}
