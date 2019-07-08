using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace DAO
{
    public class DeptDao : BaseDao<Dept>, IDeptDao
    {
        public DeptDao(DataContext context) : base(context)
        {
        }
    }
}
