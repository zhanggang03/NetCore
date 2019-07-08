using System;

namespace ModelDTO
{
    public class UserDTO
    {
        public Guid  Code { get; set; }

        public string UserName { get; set; }

        public string Tel { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }

        public Nullable<Guid> DeptCode { get; set; }

        public string DeptName { get; set; }

        public string LoginCode { get; set; }

        public string LoginPwd { get; set; }

        public Nullable<int> UIndex { get; set; }

        public string Memo { get; set; }
    }
}
