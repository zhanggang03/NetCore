using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace DAO
{
    public class UserDao : BaseDao<User>, IUserDao
    {
        public UserDao(DataContext context) : base(context)
        {
        }
    }
}
