using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
    public class Instructor:Person
    {
        [Display(Name ="入职时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime HireDate { get; set; }


        //在模型类里面定义导航属性的时候，可以不使用 virtual关键字。但加入virtual关键字可以实现Lazy loading.
        // EF Core2.0及1.0版本不支持Lazy loading,但从EF Core2.1开始支持Lazy loading需要使用代理类和在导航属性上添加Virtual关键字，跟原来的EF6一样。
        public virtual ICollection<CourseAssignment> CourseAssignments { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}
