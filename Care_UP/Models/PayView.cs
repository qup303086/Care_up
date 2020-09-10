using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class PayView
    {

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "訂單ID")]
        public int OrderId { get; set; }

        [Display(Name = "訊息")]
        public string Status { get; set; }

        [Display(Name = "付款訊息")]
        public string Message { get; set; }
    }
}