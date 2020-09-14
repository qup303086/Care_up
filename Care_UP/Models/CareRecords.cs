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

      public int OrdersID { get; set; }
        [ForeignKey("OrdersID")]
        [Display(Name = "訂單編號")]
        public virtual Orders Orders { set; get; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "病患心情")]
        public string Mood{ get; set; }

        
        [Display(Name = "建立時間")]
        public DateTime? InitDate { get; set; }

        [Display(Name = "修改時間")]
        public DateTime? EditDate { get; set; }

        [Display(Name = "備註")]
        public string Remark { get; set; }
      
    }
}