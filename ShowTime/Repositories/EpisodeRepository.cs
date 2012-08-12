using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public class MockEpisodeRepository : IRepository<Episode, EpisodeId>
    {
        private Dictionary<EpisodeId, Episode> Episodes;

        public MockEpisodeRepository() :
            this(new Dictionary<EpisodeId, Episode>())
        {
        }

        public MockEpisodeRepository(Dictionary<EpisodeId, Episode> Episodes)
        {
            this.Episodes = Episodes;
        }

        public Episode Find(EpisodeId id)
        {
            Episode show;
            Episodes.TryGetValue(id, out show);
            return show;
        }

        public IQueryable<Episode> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Episode> Query(System.Linq.Expressions.Expression<Func<Episode, bool>> where)
        {
            return Episodes.Values.Where(where.Compile()).AsQueryable();
        }

        public void Delete(Episode target)
        {
            Episodes.Remove(target.Id);
        }

        public void Save(Episode target)
        {
            throw new NotImplementedException();
        }

        public void Insert(Episode target)
        {
            Episodes.Add(target.Id, target);
        }

        public IEnumerable<Episode> GetAll()
        {
            return Episodes.Values;
        }
    }
}
