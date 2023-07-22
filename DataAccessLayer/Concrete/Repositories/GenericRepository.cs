using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        Context c = new Context();
        DbSet<T> _object;

        public GenericRepository()
        {
            _object = c.Set<T>();
        }

        public void Delete(T p)
        {
            var deletedEntity = c.Entry(p);
            deletedEntity.State = EntityState.Deleted;
            //_object.Remove(p);
            c.SaveChanges();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            return _object.SingleOrDefault(filter);
        }

        public T GetFirstOrDefaultInclude(Expression<Func<T, bool>> filter, Expression<Func<T, object>>[] children)
        {
            var query = _object.FirstOrDefault();
            foreach (var child in children)
                query = _object.Include(child).Where(filter).FirstOrDefault();

            return query;
        }

        public void Insert(T p)
        {
            var addedEntity = c.Entry(p);
            addedEntity.State = EntityState.Added;
            //_object.Add(p);
            c.SaveChanges();
        }

        public List<T> List()
        {
            return _object.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter).ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter, Expression<Func<T, DateTime>> order)
        {
            return _object.Where(filter).OrderByDescending(order).ToList();
        }

        public List<TResult> ListChart<TResult>(Expression<Func<T, int>> groupBy, Expression<Func<IGrouping<int, T>, TResult>> selectExpression)
        {
            return _object.GroupBy(groupBy).Select(selectExpression).ToList();
        }

        public List<T> ListOrderAsc(Expression<Func<T, bool>> filter, Expression<Func<T, DateTime>> order)
        {
            return _object.Where(filter).OrderBy(order).ToList();
        }

        public List<T> ListOrderInt(Expression<Func<T, bool>> filter, Expression<Func<T, int>> order)
        {
            return _object.Where(filter).OrderByDescending(order).ToList();
        }

        public void Update(T p)
        {
            var updatedEntity = c.Entry(p);
            updatedEntity.State = EntityState.Modified;
            c.SaveChanges();
        }
    }
}
