using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class PasswordView
    {
        [Display(Name = "編號")]//欄位名稱
        public int Id { get; set; }

        [Display(Name = "密碼")]
        [StringLength(100, ErrorMessage = "{0}長度至少為{2}個字,不可超過{1}個字", MinimumLength = 6)]
        [Required(ErrorMessage = "{0}必填")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "密碼鹽")]
        public string PasswordSalt { get; set; }


        [Display(Name = "修改時間")]
        public DateTime? EditDate { set; get; }
    }
}