using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public interface IBaseBll<T>
    {
        IEnumerable<T> GetEntitiesByPage<TS>(int pageIndex, int pageSize, out int totalCount, Func<T, bool> exp, Func<T, TS> order, bool descending);
        int GetAllCount();
        IEnumerable<T> GetAllEntities();
        int GetCount(Func<T, bool> exp);
        IEnumerable<T> GetEntities(Func<T, bool> exp);
        T GetEntity(Func<T, bool> exp);
        bool Insert(T t);
        bool Update(T t);
        bool Delete(T t);
    }
}
