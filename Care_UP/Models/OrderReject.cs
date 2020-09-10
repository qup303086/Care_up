using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class OrderReject
    {
        [Display(Name = "編號")]
        public int Id { get; set; }


        [Display(Name = "訂單取消理由")]
        public string Cancel { get; set; }
    }
}