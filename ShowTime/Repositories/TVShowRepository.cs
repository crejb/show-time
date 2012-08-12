using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public class MockTVShowRepository : IRepository<TVShow, TVShowId>
    {
        private Dictionary<TVShowId, TVShow> TVShows;

        public MockTVShowRepository() :
            this(new Dictionary<TVShowId, TVShow>())
        {
        }

        public MockTVShowRepository(Dictionary<TVShowId, TVShow> tvShows)
        {
            this.TVShows = tvShows;
        }

        public TVShow Find(TVShowId id)
        {
            TVShow show;
            TVShows.TryGetValue(id, out show);
            return show;
        }

        public IQueryable<TVShow> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TVShow> Query(System.Linq.Expressions.Expression<Func<TVShow, bool>> where)
        {
            return TVShows.Values.Where(where.Compile()).AsQueryable();
        }

        public void Delete(TVShow target)
        {
            TVShows.Remove(target.Id);
        }

        public void Save(TVShow target)
        {
            throw new NotImplementedException();
        }

        public void Insert(TVShow target)
        {
            TVShows.Add(target.Id, target);
        }

        public IEnumerable<TVShow> GetAll()
        {
            return TVShows.Values;
        }
    }
}
