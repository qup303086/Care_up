using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class AttendantDetails
    {
        [Key]//強制主鍵
        [Display(Name = "編號")]//欄位名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//流水號
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [MaxLength(length: 50)]
        public string Name { get; set; }

        [Display(Name = "日薪")]
        [MaxLength(length: 50)]
        public string Salary { get; set; }

        [Display(Name = "匯款帳號")]
        [MaxLength(length: 50)]
        public string Account { get; set; }

        [Display(Name = "提供服務")]
        public string Service { get; set; }

        [Display(Name = "資格文件")]
        [MaxLength(length: 50)]
        public string File { get; set; }

        [Display(Name = "服務時段")]
        public string ServiceTime { get; set; }

        [Display(Name = "經驗")]
        public string Experience { get; set; }

        [Display(Name = "開始時間")]
        public DateTime? StartDateTime { set; get; }

        [Display(Name = "結束時間")]
        public DateTime? EndDateTime { set; get; }

        [Display(Name = "修改時間")]
        public DateTime? EditDate { set; get; }

        [Display(Name = "是否開啟基本資料")]
        public string Status { get; set; }
    }
}