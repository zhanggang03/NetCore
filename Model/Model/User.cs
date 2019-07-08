using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    [Serializable]
    [Table("User", Schema = "dbo")]
    public class User
    {
        [Key]
        [Required]
        [Display(Description = "User表主键")]
        public Guid Code { get; set; }

        [MaxLength(64)]
        [Display(Description = "User姓名")]
        public string UserName { get; set; }

        [MaxLength(20)]
        [Display(Description = "固定电话")]
        public string Tel { get; set; }

        [MaxLength(20)]
        [Display(Description = "移动电话")]
        public string Phone { get; set; }

        [MaxLength(200)]
        [Display(Description = "地址")]
        public string Address { get; set; }

        [MaxLength(64)]
        [Display(Description = "职称")]
        public string Title { get; set; }

        [Display(Description = "科室代码")]
        public Nullable<Guid> DeptCode { get; set; }
        [ForeignKey("DeptCode")]
        public virtual Dept Dept { get; set; }

        [MaxLength(64)]
        [Display(Description = "科室名称")]
        public string DeptName { get; set; }


        [MaxLength(32)]
        [Display(Description = "登录名称")]
        public string LoginCode { get; set; }

        [MaxLength(32)]
        [Display(Description = "登录密码")]
        public string LoginPwd { get; set; }

        [Column(TypeName = "int")]
        [Display(Description = "编排顺序")]
        public Nullable<int> UIndex { get; set; }

        [MaxLength(1)]
        [Display(Description = "是否使用")]
        public char IsUse { get; set; }

        [MaxLength(1024)]
        [Display(Description = "备注")]
        public string Memo { get; set; }

        //public ICollection<Role> Roles { get; set; }
    }
}
