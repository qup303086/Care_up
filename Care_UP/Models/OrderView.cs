using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class OrderView
    {
        [Display(Name = "訂單狀態")]
        public OrderType Status { get; set; }
    }
}