using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Care_UP.Models
{
    public class Question
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "照服員編號")]
        public int AttendantId { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "會員")]
        public string MemberAccount { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "提問")]
        public string Quiz { get; set; }

        [Display(Name = "提問日期")]
        public DateTime? InitDateTime { get; set; }

       
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    }
}