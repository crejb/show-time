using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public class MockBookmarkRepository : IRepository<Bookmark, BookmarkId>
    {
        private Dictionary<BookmarkId, Bookmark> Bookmarks;

        public MockBookmarkRepository() :
            this(new Dictionary<BookmarkId, Bookmark>())
        {
        }

        public MockBookmarkRepository(Dictionary<BookmarkId, Bookmark> Bookmarks)
        {
            this.Bookmarks = Bookmarks;
        }

        public Bookmark Find(BookmarkId id)
        {
            Bookmark show;
            Bookmarks.TryGetValue(id, out show);
            return show;
        }

        public IQueryable<Bookmark> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Bookmark> Query(System.Linq.Expressions.Expression<Func<Bookmark, bool>> where)
        {
            return Bookmarks.Values.Where(where.Compile()).AsQueryable();
        }

        public void Delete(Bookmark target)
        {
            Bookmarks.Remove(target.Id);
        }

        public void Save(Bookmark target)
        {
            throw new NotImplementedException();
        }

        public void Insert(Bookmark target)
        {
            Bookmarks.Add(target.Id, target);
        }

        public IEnumerable<Bookmark> GetAll()
        {
            return Bookmarks.Values;
        }
    }
}
