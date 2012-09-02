using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public interface IRepositoryPersister<TKey, TValue> where TValue : Entity<TKey>
    {
        void Load(IRepository<TKey, TValue> repository);
        void Save(IRepository<TKey, TValue> repository);
    }
}
