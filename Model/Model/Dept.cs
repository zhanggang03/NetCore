using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    [Serializable]
    [Table("Dept", Schema = "dbo")]
    public class Dept
    {
        [Key]
        [Required]
        [Display(Description = "Dept表主键")]
        public Guid Code { get; set; }

        [MaxLength(64)]
        [Display(Description = "科室名称")]
        public string DeptName { get; set; }

        [MaxLength(200)]
        [Display(Description = "图标")]
        public string Icon { get; set; }

        [Column(TypeName = "int")]
        [Display(Description = "排序")]
        public Nullable<int> UIndex { get; set; }

        [Column(TypeName = "Guid")]
        [Display(Description = "父科室编码")]
        public Nullable<Guid> ParentCode { get; set; }
        [ForeignKey("ParentCode")]
        public virtual Dept Parent { get; set; }

        [MaxLength(64)]
        [Display(Description = "父科室名称")]
        public string ParentName { get; set; }

        [MaxLength(1)]
        [Display(Description = "是否使用中，暂时不用")]
        public char IsUse { get; set; }

        [MaxLength(2048)]
        [Display(Description = "备注")]
        public string Memo { get; set; }

        public virtual  ICollection<User> Users { get; set; }
    }
}
