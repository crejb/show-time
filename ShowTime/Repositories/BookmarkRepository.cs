using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public class BookmarkRepository : BaseDictionaryRepository<BookmarkId, Bookmark>
    {
        public BookmarkRepository(IRepositoryPersister<BookmarkId, Bookmark> persister)
            : base(persister)
        {
        }
    }
}
