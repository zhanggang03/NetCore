using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO;
using Model;

namespace BLL
{
    public class DeptBll : IDeptBll
    {
        private IDeptDao deptDao;

        public DeptBll(IDeptDao _deptDao)
        {
            deptDao = _deptDao;
        }

        public List<Dept> GetAllDepts()
        {
            return deptDao.GetAllEntities().ToList();
        }

        public bool CreateDept(Dept dept)
        {
            return deptDao.Insert(dept);
        }

    }
}
