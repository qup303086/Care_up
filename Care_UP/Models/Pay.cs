using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class Pay
    {
        [Key]//強制主鍵
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [Display(Name = "訂單ID")]
        public virtual Orders Orders { set; get; }


        [Display(Name = "訊息")]
        public string Status { get; set; }

        [Display(Name = "付款訊息")]
        public string Message { get; set; }

    }
}