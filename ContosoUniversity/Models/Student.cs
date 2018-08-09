using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="{0}不能为空")]
        [StringLength(50,ErrorMessage ="{0}不能多于{1}个字符")]
        [Display(Name = "姓")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="{0}是必需的")]
        [StringLength(50,ErrorMessage ="{0}不能多于{1}个字符",MinimumLength =1)]
        [Column("FirstName")]
        [Display(Name ="名")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [Display(Name ="入学日期")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name="全名")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

         //在模型类里面定义导航属性的时候，可以不使用 virtual关键字。
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
