using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class OrderView 
    {

        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "照服員姓名")]
        public string AttendantsName { get; set; }

        [Display(Name = "匯款帳號")]
        public string account { get; set; }

        [Display(Name = "訂單ID")]
        public string orderId { get; set; }
        
        [Display(Name = "薪資")]
        public string salary { get; set; }

        [Display(Name = "狀態")]
        public OrderType satus { get; set; }

    }
}