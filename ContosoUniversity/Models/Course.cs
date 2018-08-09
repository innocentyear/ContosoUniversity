using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ContosoUniversity.Models
{
    public class Course
    {
        [Display(Name ="课程代码")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }

        [Display(Name ="课程名称")]
        [StringLength(50,MinimumLength =3)]
        public string Title { get; set; }

        [Display(Name ="学分")]
        [Range(0,5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }


        public virtual Department Department { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        //在模型类里面定义导航属性的时候，EF Core中可以不使用 virtual关键字。
        public virtual ICollection<CourseAssignment> CourseAssignments { get; set; }

    }
}
