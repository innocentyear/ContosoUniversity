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


        [Display(Name ="部门")]
        public virtual Department Department { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        //在模型类里面定义导航属性的时候，可以不使用 virtual关键字。但加入virtual关键字可以实现Lazy loading.
        // EF Core2.0及1.0版本不支持Lazy loading,但从EF Core2.1开始支持Lazy loading需要使用代理类和在导航属性上添加Virtual关键字，跟原来的EF6一样。
        public virtual ICollection<CourseAssignment> CourseAssignments { get; set; }

    }
}
