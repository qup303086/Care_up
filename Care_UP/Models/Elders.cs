using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Care_UP.Models
{
    public class Elders
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "家屬ID")]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Members Members { set; get; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "性別")]
        public GenderType Gender { set; get; }

        [Display(Name = "年齡")]
        public int Age { set; get; }

        [Display(Name = "身高")]
        public int Height { set; get; }

        [Display(Name = "體重")]
        public int Weight { set; get; }


        [Display(Name = "照護地點")]
        public string Place { set; get; }

        [Display(Name = "照護地址")]
        public string Address { set; get; }


        [Display(Name = "身體狀況")]
        public string Body { set; get; }

        [Display(Name = "設備")]
        public string Equipment { set; get; }

        [Display(Name = "服務項目")]
        public string ServiceItems { set; get; }

        [Display(Name = "緊急聯絡人")]
        public string Urgent { set; get; }

        [Display(Name = "關係")]
        public string Relationship { set; get; }

        [Display(Name = "手機")]
        public string Phone { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//+?允許空值(int,datetime)

        [Display(Name = "修改時間")]
        public DateTime? EditDate { set; get; }


    }
}