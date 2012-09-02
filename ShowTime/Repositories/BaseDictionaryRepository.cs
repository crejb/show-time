using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public abstract class BaseDictionaryRepository<TKey, TValue> : IRepository<TKey, TValue> where TValue : Entity<TKey>
    {
        protected IRepositoryPersister<TKey, TValue> persister;
        protected Dictionary<TKey, TValue> dataStore;

        public BaseDictionaryRepository(IRepositoryPersister<TKey, TValue> persister)
        {
            this.dataStore = new Dictionary<TKey, TValue>();
            this.persister = persister;
            persister.Load(this);
        }

        public TValue Find(TKey id)
        {
            TValue entity;
            dataStore.TryGetValue(id, out entity);
            return entity;
        }

        public IQueryable<TValue> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TValue> Query(System.Linq.Expressions.Expression<Func<TValue, bool>> where)
        {
            return dataStore.Values.Where(where.Compile()).AsQueryable();
        }

        public void Delete(TValue target)
        {
            dataStore.Remove(target.Id);
        }

        public void Insert(TValue target)
        {
            dataStore.Add(target.Id, target);
        }

        public IEnumerable<TValue> GetAll()
        {
            return dataStore.Values;
        }

        public void Save()
        {
            persister.Save(this);
        }
    }
}
