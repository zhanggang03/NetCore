using System;
using System.Collections.Generic;
using System.Text;
using DAO;

namespace BLL
{
    public class BaseBll<T> : IBaseBll<T>
    {
        private IBaseDao<T> dao;

        public BaseBll(IBaseDao<T> _dao)
        {
            dao = _dao;
        }

        public IEnumerable<T> GetEntitiesByPage<TS>(int pageIndex, int pageSize, out int totalCount, Func<T, bool> exp, Func<T, TS> order, bool descending)
        {
            return dao.GetEntitiesByPage(pageIndex, pageSize, out totalCount, exp, order, descending);
        }


        //public IEnumerable<T> GetEntitiesByPage(Func<T, bool> exp, int page, int size)
        //{
        //    return Dao.GetEntitiesByPage(exp, page, size);
        //}

        public int GetAllCount()
        {
            return dao.GetAllCount();
        }

        public IEnumerable<T> GetAllEntities()
        {
            return dao.GetAllEntities();
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        public int GetCount(Func<T, bool> exp)
        {
            return dao.GetCount(exp);
        }
        /// <summary>
        /// 获取Entities（列表）
        /// </summary>
        public IEnumerable<T> GetEntities(Func<T, bool> exp)
        {
            return dao.GetEntities(exp);
        }
        /// <summary>
        /// 查询Entity（单个）
        /// </summary>
        public T GetEntity(Func<T, bool> exp)
        {
            return dao.GetEntity(exp);
        }
        /// <summary>
        /// 新增Entity
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Insert(T t)
        {
            return dao.Insert(t);
        }
        /// <summary>
        /// 更新Entity
        /// </summary>
        public bool Update(T t)
        {
            return dao.Update(t);
        }
        /// <summary>
        /// 删除Entity
        /// </summary>
        public bool Delete(T t)
        {
            return dao.Delete(t);
        }
    }
}
