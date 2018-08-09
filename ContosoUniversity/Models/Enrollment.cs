using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        public int CourseID { get; set; }

        public int StudentID { get; set; }

        [DisplayFormat(NullDisplayText ="无等级")]
        public Grade? Grade { get; set; }



         //在模型类里面定义导航属性的时候，可以不使用 virtual关键字。
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }

    }

    public enum Grade
    {
        A,B,C,D,F
    }
}
