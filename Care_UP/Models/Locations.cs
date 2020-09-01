using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class Locations
    {
        [Key]//強制主鍵
        [Display(Name = "編號")]//欄位名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//流水號
        public int Id { get; set; }

 
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        [Display(Name = "城市Id")]
        [Required(ErrorMessage = "{0}必填")]
        public virtual Cities Cities { set; get; }
       
       

        [Display(Name = "地區")]
        [MaxLength(length: 50)]//設定長度
        [Required(ErrorMessage = "{0}必填")]
        public string Area { get; set; }
        public virtual ICollection<Attendants> Attendants { get; set; }

    }
}