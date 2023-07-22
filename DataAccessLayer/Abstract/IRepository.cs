using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IRepository<T>
    {
        List<T> List();
        void Insert(T p);
        void Update(T p);
        void Delete(T p);
        T Get(Expression<Func<T, bool>> filter);
        List<T> List(Expression<Func<T, bool>> filter);
        List<T> List(Expression<Func<T, bool>> filter, Expression<Func<T, DateTime>> order);
        List<T> ListOrderAsc(Expression<Func<T, bool>> filter, Expression<Func<T, DateTime>> order);
        List<T> ListOrderInt(Expression<Func<T, bool>> filter, Expression<Func<T, int>> order);
        List<TResult> ListChart<TResult>(Expression<Func<T, int>> groupBy, Expression<Func<IGrouping<int, T>, TResult>> selectExpression);
        T GetFirstOrDefaultInclude(Expression<Func<T, bool>> filter,params Expression<Func<T, object>>[] children);
    }
}
