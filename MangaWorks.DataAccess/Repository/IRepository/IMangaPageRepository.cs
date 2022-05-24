using MangaWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.DataAccess.Repository.IRepository
{
    public interface IMangaPageRepository : IRepository<MangaPage>
    {
        void Update(MangaPage obj);
    }
}
