using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class DetailedDates
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [Display(Name = "訂單ID")]
        public virtual Orders Orders { set; get; }


        [Display(Name = "照護紀錄")]
        public string CareRecord { get; set; }

        [Display(Name = "日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? NowDateTime { set; get; }



    }
}