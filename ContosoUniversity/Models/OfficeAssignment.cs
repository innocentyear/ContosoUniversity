using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
    public class OfficeAssignment
    {
        [Key]
        public int InstructorID { get; set; }

        [Display(Name ="办公室地点")]
        [StringLength(50)]
        public string Location { get; set; }


        public virtual Instructor Instructor { get; set; }
    }
}
