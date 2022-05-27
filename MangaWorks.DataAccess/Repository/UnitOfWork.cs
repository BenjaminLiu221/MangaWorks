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
            Manga = new MangaRepository(_dbContext);
            Page = new PageRepository(_dbContext);
            Chapter = new ChapterRepository(_dbContext);
        }
        public IGenreRepository Genre { get; private set; }
        public IAuthorRepository Author { get; private set; }

        public IMangaRepository Manga { get; private set; }
        public IPageRepository Page { get; private set; }
        public IChapterRepository Chapter { get; private set; }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
