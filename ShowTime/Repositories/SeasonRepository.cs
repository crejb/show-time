using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public class MockSeasonRepository : IRepository<Season, SeasonId>
    {
        private Dictionary<SeasonId, Season> Seasons;

        public MockSeasonRepository() :
            this(new Dictionary<SeasonId, Season>())
        {
        }

        public MockSeasonRepository(Dictionary<SeasonId, Season> Seasons)
        {
            this.Seasons = Seasons;
        }

        public Season Find(SeasonId id)
        {
            Season show;
            Seasons.TryGetValue(id, out show);
            return show;
        }

        public IQueryable<Season> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Season> Query(System.Linq.Expressions.Expression<Func<Season, bool>> where)
        {
            return Seasons.Values.Where(where.Compile()).AsQueryable();
        }

        public void Delete(Season target)
        {
            Seasons.Remove(target.Id);
        }

        public void Save(Season target)
        {
            throw new NotImplementedException();
        }

        public void Insert(Season target)
        {
            Seasons.Add(target.Id, target);
        }

        public IEnumerable<Season> GetAll()
        {
            return Seasons.Values;
        }
    }
}
