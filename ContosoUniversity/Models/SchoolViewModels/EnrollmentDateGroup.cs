using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.SchoolViewModels
{
    public class EnrollmentDateGroup
    {
        [Display(Name ="入学日期")]
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }

        [Display(Name ="人数")]
        public int StudentCount { get; set; }
    }
}
