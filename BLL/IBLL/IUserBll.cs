using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace BLL
{
    public interface IUserBll
    {
        List<User> GetAllUsers();

        List<User> GetUsersByDeptCode(Guid deptCode);

        User GetUser(Guid userCode);
    }
}
