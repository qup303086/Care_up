using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class AttendantLocation
    {
        [Key]//強制主鍵
        [Display(Name = "編號")]//欄位名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//流水號
        public int Id { get; set; }


        public int AttendantId { get; set; }
        [ForeignKey("AttendantId")]
        [Display(Name = "照服員Id")]
        [Required(ErrorMessage = "{0}必填")]
        public virtual Attendants Attendants{ set; get; }

        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        [Display(Name = "地區Id")]
        [Required(ErrorMessage = "{0}必填")]
        public virtual Locations Locations { set; get; }

    }
}