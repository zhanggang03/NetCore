using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO;
using Microsoft.EntityFrameworkCore;
using Model;

namespace BLL
{
    public class UserBll : IUserBll
    {
        private IUserDao userDao;

        public UserBll(IUserDao _userDao)
        {
            userDao = _userDao;
        }

        public List<User> GetAllUsers()
        {
            return userDao.GetAllEntities().OrderBy(p => p.UIndex).ToList();
        }

        public List<User> GetUsersByDeptCode(Guid deptCode)
        {
            return userDao.GetEntities(p => p.DeptCode == deptCode).OrderBy(p => p.UIndex).ToList();
        }

        public User GetUser(Guid userCode)
        {
            return userDao.GetEntity(p => p.Code == userCode);
        }
    }
}
