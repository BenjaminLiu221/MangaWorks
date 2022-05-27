using MangaWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.DataAccess.Repository.IRepository
{
    public interface IPageRepository : IRepository<Page>
    {
        void Update(Page obj);
    }
}
