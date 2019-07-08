using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ProcPage
    {
        /// <summary>
        /// 要显示的表或多个表的连接
        /// </summary> 
        public string TableName { get; set; }
        /// <summary>
        /// 要显示的字段列表
        /// </summary> 
        public string FieldNames { get; set; }
        /// <summary>
        /// 每页显示的记录个数
        /// </summary> 
        public int PageSize { get; set; }
        /// <summary>
        /// 要显示那一页的记录
        /// </summary> 
        public int Page { get; set; }
        /// <summary>
        /// 查询结果分页后的总页数
        /// </summary> 
        public int PageCount { get; set; }
        /// <summary>
        /// 查询到的总记录数
        /// </summary> 
        public int Counts { get; set; }
        /// <summary>
        /// 排序字段列表或条件
        /// </summary> 
        public string FieldSort { get; set; }
        /// <summary>
        /// 排序方法，0为升序，1为降序--程序传参如：' SortA Asc,SortB Desc,SortC ')
        /// </summary> 
        public string Sort { get; set; }
        /// <summary>
        /// 查询条件,不需WHERE
        /// </summary> 
        public string Condition { get; set; }
        /// <summary>
        /// 主表的主键
        /// </summary> 
        public string KeyId { get; set; }
        /// <summary>
        /// 是否添加查询字段的 DISTINCT 默认0不添加/1添加
        /// </summary> 
        public int Distinct { get; set; }
        /// <summary>
        /// Group语句,不带Group By
        /// </summary> 
        public string Group { get; set; }
    }
}
