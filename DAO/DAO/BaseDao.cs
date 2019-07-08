using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Common;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DAO
{
    public class BaseDao<T> : IBaseDao<T> where T : class
    {
        private readonly DataContext Entities;
        public BaseDao(DataContext context)
        {
            Entities = context;
        }

        /// <summary>
        /// 得到所有记录数
        /// </summary>
        /// <returns>总条数</returns>
        public int GetAllCount()
        {
            return Entities.Set<T>().Count();
        }

        /// <summary>
        /// 得到所有记录
        /// </summary>
        /// <returns>实体对象列表</returns>        
        public IEnumerable<T> GetAllEntities()
        {
            return Entities.Set<T>().ToList();
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        ///<param name="exp">linq表达式</param>
        /// <returns>总条数</returns>
        public int GetCount(Func<T, bool> exp)
        {
            return Entities.Set<T>().Where(exp).Count();
        }

        /// <summary>
        /// 获取记录(列表)
        /// </summary>
        ///<param name="exp">linq表达式</param>
        /// <returns>实体对象列表</returns>
        public IEnumerable<T> GetEntities(Func<T, bool> exp)
        {
            return Entities.Set<T>().Where(exp).ToList();
        }

        /// <summary>
        /// 获取记录(单个)
        /// </summary>
        ///<param name="exp">linq表达式</param>
        /// <returns>实体对象</returns>
        public T GetEntity(Func<T, bool> exp)
        {
            return Entities.Set<T>().FirstOrDefault(exp);
        }

        /// <summary>
        /// 添加纪录
        /// </summary>
        /// <param name="t"></param>
        /// <returns>true：成功，false失败</returns>
        public bool Insert(T t)
        {
            Entities.Set<T>().Add(t);
            return Entities.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="t"></param>
        /// <returns>true：成功，false失败</returns>
        public bool Update(T t)
        {
            Entities.Set<T>().Attach(t);
            Entities.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return Entities.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="t"></param>
        /// <returns>true：成功，false失败</returns>
        public bool Delete(T t)
        {
            Entities.Set<T>().Attach(t);
            Entities.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return Entities.SaveChanges() > 0;
        }

        /// <summary>
        /// 根据页数得到对应信息
        /// </summary>
        ///<param name="pageIndex">页索引</param>
        ///<param name="pageSize">页长</param>
        ///<param name="totalCount">总条数</param>
        ///<param name="exp">linq表达式，条件</param>
        ///<param name="order">linq表达式，排序</param>
        ///<param name="descending">true：倒序排列，false正序排列</param>
        ///<returns>列表</returns>
        public IEnumerable<T> GetEntitiesByPage<TS>(int pageIndex, int pageSize, out int totalCount, Func<T, bool> exp, Func<T, TS> order, bool descending)
        {
            var temp = exp == null ? Entities.Set<T>() : Entities.Set<T>().Where(exp);
            totalCount = temp.Count();
            if (descending)
            {
                temp = temp.OrderByDescending(order)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize).ToList();
            }
            else
            {
                temp = temp.OrderBy(order)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize).ToList();
            }
            return temp;

        }

        ///<summary>
        ///执行Sql语句(增删改)
        ///</summary>
        ///<param name="sql">T-Sql where语句,不能为空</param>
        ///<returns>true 成功， false 失败</returns>
        public bool ExecuteSQL(string sql)
        {
            SqlConnection connection = Entities.Database.GetDbConnection() as SqlConnection;
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            int ret = cmd.ExecuteNonQuery();
            if (ret > 0)
                return true;
            else
                return false;


        }

        ///<summary>
        ///执行Sql语句(查询)
        ///</summary> 
        ///<param name="sql">T-Sql where语句,不能为空</param>
        ///<returns>DataSet数据</returns>
        public DataSet ExecuteQuery(string sql)
        {
            DataSet ds = new DataSet();
            SqlConnection connection = Entities.Database.GetDbConnection() as SqlConnection;
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        ///<summary>
        ///执行存储过程
        ///</summary>
        ///<param name="Procedure">存储过程</param>
        ///<param name="parameters">参数</param>
        ///<returns>DataSet数据</returns>
        public DataSet ExecuteProcedure(string Procedure, IDataParameter[] parameters)
        {
            DataSet ds = new DataSet();
            SqlConnection connection = Entities.Database.GetDbConnection() as SqlConnection;
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = Procedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public DataTable PagingList(ref ProcPage pb)
        {
            try
            {
                SqlParameter[] sqlparametrs = new SqlParameter[]{
                    new SqlParameter("@TableName",DbType.AnsiString),
                    new SqlParameter("@Fields",DbType.AnsiString),
                    new SqlParameter("@OrderField",DbType.AnsiString),
                    new SqlParameter("@sqlWhere",DbType.AnsiString),
                    new SqlParameter("@PageSize",DbType.Int32),
                    new SqlParameter("@PageIndex",DbType.Int32),
                    new SqlParameter("@TotalPage",DbType.Int32),
                    new SqlParameter("@Counts",DbType.Int32),

                };
                sqlparametrs[0].Value = pb.TableName;
                sqlparametrs[1].Value = pb.FieldNames;
                sqlparametrs[2].Value = pb.FieldSort;
                sqlparametrs[3].Value = pb.Condition;
                sqlparametrs[4].Value = pb.PageSize;
                sqlparametrs[5].Value = pb.Page;
                sqlparametrs[6].Direction = ParameterDirection.Output;
                sqlparametrs[7].Direction = ParameterDirection.Output;
                DataSet ds = ExecuteProcedure("up_Page_2005", sqlparametrs);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                pb.PageCount = int.Parse(sqlparametrs[6].Value.ToString());
                pb.Counts = int.Parse(sqlparametrs[7].Value.ToString());
                return ds.Tables[0];
            }
            catch
            {
                return null;
            }
        }
    }
}
