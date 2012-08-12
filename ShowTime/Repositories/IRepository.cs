using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using ShowTime.Model;

namespace ShowTime.Repositories
{
    public interface IRepository<T, TKey> where T : Entity<TKey>
    {
        // Find an entity by its primary key
        // We assume and enforce that every Entity
        // is identified by an "Id" property of 
        // type long
        T Find(TKey id);

        // Query for a specific type of Entity
        // with Linq expressions.  More on this later
        IQueryable<T> Query();
        IQueryable<T> Query(Expression<Func<T, bool>> where);

        // Basic operations on an Entity
        void Delete(T target);
        void Save(T target);
        void Insert(T target);

        IEnumerable<T> GetAll();
    }
}
