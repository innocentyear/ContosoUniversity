using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
    public class Instructor
    {
        public int ID { get; set; }

        [Display(Name ="姓")]
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name ="名")]
        [Required]
        [StringLength(50)]
        [Column("FirstName")]
        public string FirstMidName { get; set; }

        [Display(Name ="入职时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime HireDate { get; set; }

        [Display(Name ="全名")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }


        public virtual ICollection<CourseAssignment> CourseAssignments { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }


        





    }
}
