using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class Attendants
    {
        [Key]//強制主鍵
        [Display(Name = "編號")]//欄位名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//流水號
        public int Id { get; set; }

        public int MemberId { get; set; }
        [ForeignKey(" MemberId")]
        [Display(Name = "帳號ID")]
        public virtual Members Members { set; get; }
        
        public int LocationsId { get; set; }
        [ForeignKey("LocationId")]
        [Display(Name = "地區ID")]
        public virtual Locations Locations { set; get; }

        [Display(Name = "姓名")]
        [MaxLength(length: 50)]
        [Required(ErrorMessage = "{0}必填")]
        public string Name { get; set; }

        [Display(Name = "日薪")]
        [MaxLength(length: 50)]
        [Required(ErrorMessage = "{0}必填")]
        public string Salary { get; set; }

        [Display(Name = "匯款帳號")]
        [MaxLength(length: 50)]
        [Required(ErrorMessage = "{0}必填")]
        public string Account { get; set; }

        [Display(Name = "提供服務")]
        [Required(ErrorMessage = "{0}必填")] 
        public string Service { get; set; }

        [Display(Name = "資格文件")]
        [MaxLength(length: 50)]
        [Required(ErrorMessage = "{0}必填")]
        public string File { get; set; }

        [Display(Name = "服務時段")]
        public string ServiceTime { get; set; }

        [Display(Name = "經驗")]
        [Required(ErrorMessage = "{0}必填")]
        public string Experience { get; set; }


        [Display(Name = "開始時間")]
        public DateTime? StartDateTime { set; get; }

        [Display(Name = "結束時間")]
        public DateTime? EndDateTime{ set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//+?允許空值(int,datetime)


        [Display(Name = "修改時間")]
        public DateTime? EditDate { set; get; }

        [Display(Name = "是否開啟基本資料")]
        [Required(ErrorMessage = "{0}必填")]
        public string Status { get; set; }

    }
}