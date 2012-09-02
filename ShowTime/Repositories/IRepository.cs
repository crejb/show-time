using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public interface IRepository<TKey, TValue> where TValue : Entity<TKey>
    {
        // Find an entity by its primary key
        // We assume and enforce that every Entity
        // is identified by an "Id" property of 
        // type long
        TValue Find(TKey id);

        // Query for a specific type of Entity
        // with Linq expressions.  More on this later
        IQueryable<TValue> Query();
        IQueryable<TValue> Query(Expression<Func<TValue, bool>> where);

        // Basic operations on an Entity
        void Delete(TValue target);
        void Insert(TValue target);

        IEnumerable<TValue> GetAll();

        void Save();
    }
}
