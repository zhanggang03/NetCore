using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace DAO
{
    /// <summary>
    /// Dao公共接口
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    public interface IBaseDao<T>
    {
        /// <summary>
        /// 根据页数得到对应信息
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        //IEnumerable<T> GetEntitiesByPage(Func<T,bool> exp,int page, int size);
        IEnumerable<T> GetEntitiesByPage<TS>(int pageIndex, int pageSize, out int totalCount, Func<T, bool> exp, Func<T, TS> order, bool descending);


        /// <summary>
        /// 得到所有记录数
        /// </summary>
        /// <returns></returns>
        int GetAllCount();

        /// <summary>
        /// 得到所有记录
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAllEntities();

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        int GetCount(Func<T, bool> exp);

        /// <summary>
        /// 获取记录(列表)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        IEnumerable<T> GetEntities(Func<T, bool> exp);

        /// <summary>
        /// 获取记录(单个)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        T GetEntity(Func<T, bool> exp);

        /// <summary>
        /// 添加纪录
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Insert(T t);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Update(T t);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Delete(T t);
    }
}
