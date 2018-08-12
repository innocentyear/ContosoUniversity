using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [Display(Name = "部门名称")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "成立时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime StartDate { get; set; }

        [Display(Name="经费预算")]
        [DataType(DataType.Currency)]
        [Column(TypeName ="money")]
        public decimal Budget { get; set; }

        public int? InstructorID{ get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual Instructor Administrator { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
