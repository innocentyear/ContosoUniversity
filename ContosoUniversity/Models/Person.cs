using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public abstract class Person //抽象类 abstract
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(50, ErrorMessage = "{0}不能多于{1}个字符")]
        [Display(Name = "姓")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0}是必需的")]
        [StringLength(50, ErrorMessage = "{0}不能多于{1}个字符", MinimumLength = 1)]
        [Column("FirstName")]
        [Display(Name = "名")]
        public string FirstMidName { get; set; }


        [Display(Name = "全名")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }
    }
}
