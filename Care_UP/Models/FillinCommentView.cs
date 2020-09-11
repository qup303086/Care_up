using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class FillinCommentView
    {
        
        [Display(Name = "編號")]
        public int Id { get; set; }

        [Display(Name = "評價")]
        public string Comment { get; set; }

        [Display(Name = "星星")]
        public int? Star { get; set; }

        [Display(Name = "修改時間")]
        public DateTime? EditDate { get; set; }

        [Display(Name = "訂單狀態")]
        public string Status { get; set; }


    }
}