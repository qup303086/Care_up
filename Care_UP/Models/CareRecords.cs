using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class CareRecords
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

      public int DetailedDateId { get; set; }
        [ForeignKey("DetailedDateId")]
        [Display(Name = "訂單詳細日期")]
        public virtual DetailedDates DetailedDates { set; get; }

        [Display(Name = "病患心情")]
        public string Mood{ get; set; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string InitDate { get; set; }

        [Display(Name = "修改時間")]
        public string EditDate { get; set; }

        [Display(Name = "是否填寫")]
        public string Whether { get; set; }
    }
}