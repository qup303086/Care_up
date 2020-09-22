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

        [Display(Name = "Email")]
        [MaxLength(length: 50)]//設定長度
        [Required(ErrorMessage = "{0}必填")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "格式錯誤")]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [StringLength(100, ErrorMessage = "{0}長度至少為{2}個字,不可超過{1}個字", MinimumLength = 6)]
        [Required(ErrorMessage = "{0}必填")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "密碼鹽")]
        public string PasswordSalt { get; set; }

        [Display(Name = "姓名")]
        [MaxLength(length: 50)]
        public string Name { get; set; }

        [Display(Name = "相片")]
        [MaxLength(length:50)]
        public string Photo { get; set; }

        [Display(Name = "日薪")]
        public int? Salary { get; set; }

        [Display(Name = "匯款帳號")]
        [MaxLength(length: 50)]
        public string Account { get; set; }

        [Display(Name = "提供服務")]
        public string Service { get; set; }

        [Display(Name = "資格文件")]
        [MaxLength(length: 50)]
        public string File { get; set; }

        [Display(Name = "服務時段")]
        public ServiceTime ServiceTime { get; set; }

        [Display(Name = "經驗")]
        public string Experience { get; set; }


        [Display(Name = "開始時間")]
        public DateTime? StartDateTime { set; get; }

        [Display(Name = "結束時間")]
        public DateTime? EndDateTime { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//+?允許空值(int,datetime)


        [Display(Name = "修改時間")]
        public DateTime? EditDate { set; get; }

        [Display(Name = "是否開啟基本資料")]
        public Whether Status { get; set; }

        public ICollection<Locations> Locationses { get; set; }

        
    }
}