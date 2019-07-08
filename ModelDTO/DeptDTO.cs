using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDTO
{
    public class DeptDTO
    {
        public Guid Code { get; set; }

        public string DeptName { get; set; }

        public string Icon { get; set; }

        public Nullable<int> UIndex { get; set; }

        public Nullable<Guid> ParentCode { get; set; }

        public string ParentName { get; set; }

        public string Memo { get; set; }

    }
}
