using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models
{

    //需要使用Fulent API来配置组合主键
      /*modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID
           });
    */

    public class CourseAssignment
    {
        public int InstructorID { get; set; }

        public int CourseID { get; set; }

        public virtual Instructor Instructor { get; set; }

        public virtual Course Course { get; set; }
    }
}
