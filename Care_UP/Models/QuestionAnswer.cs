using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class QuestionAnswer
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "提問編號")]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual  Question Question { set; get; }
        
        [Display(Name = "照服員姓名")]
        public string Attendant { get; set; }

        [Display(Name = "回覆內容")]
        public string Answer { get; set; }

        [Display(Name = "回覆日期")]
        public  DateTime? ReplyTime { get; set; }

        [Display(Name = "回覆狀態")]
        public string Status { get; set; }
    }
}