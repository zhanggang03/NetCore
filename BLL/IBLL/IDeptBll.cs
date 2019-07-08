using System;
using System.Collections.Generic;
using System.Text;
using DAO;
using Model;

namespace BLL
{
    public interface IDeptBll
    {
        List<Dept> GetAllDepts();

        bool CreateDept(Dept dept);
    }
}
