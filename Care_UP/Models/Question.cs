using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class Question
    {
        [Key]//強制主鍵
        [Display(Name = "編號")]//欄位名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//流水號
        public int Id { get; set; }

        [Display(Name = "會員")]
        public string MemberAccount { get; set; }

        [Display(Name = "提問")]
        public string Quiz { get; set; }

        [Display(Name = "提問日期")]
        public DateTime InitDateTime { get; set; }
    }
}