using MangaWorks.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaWorks.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Genre = new GenreRepository(_dbContext);
            Author = new AuthorRepository(_dbContext);
        }
        public IGenreRepository Genre { get; private set; }
        public IAuthorRepository Author { get; set; }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
