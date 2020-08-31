using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    /// <summary>
    /// 開始日期 結束日期不用getdate
    /// </summary>
    public class Orders
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "家屬帳號ID")]
        public int ElderId { get; set; }
        
        [Display(Name = "照服員ID")]
        public int AttendantId { get; set; }

        [Display(Name = "開始日期")]
       
        public DateTime? StartDate { set; get; }

        [Display(Name = "結束日期")]
        public DateTime? EndDate { set; get; }

        [Display(Name = "中止日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? StopDate { set; get; }

        [Display(Name = "訂單總額")]
        public int Total { get; set; }

        [Display(Name = "性別")]
        public GenderType Gender { set; get; }

        [Display(Name = "年齡")]
        public int Age { set; get; }

        [Display(Name = "身高")]
        public int Height { set; get; }

        [Display(Name = "身體狀況")]
        public BodyType Body { set; get; }

        [Display(Name = "設備")]
        public EquipmentType Equipment { set; get; }

        [Display(Name = "關係")]
        public string Relationship { set; get; }

        [Display(Name = "緊急聯絡人")]
        public string Urgent { set; get; }


        [Display(Name = "電話")]
        public int Phone { set; get; }

        [Display(Name = "服務項目")]
        public ServiceItemsType ServiceItems { set; get; }


        [Display(Name = "備註")]
        public string Remarks { get; set; }

        [Display(Name = "評價")]
        public string Comment { get; set; }
        
        [Display(Name = "星星")]
        public int Star { get; set; }

        [Display(Name = "訂單取消理由")]
        public string Cancel { get; set; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { get; set; }

        [Display(Name = "修改時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? EditDate { get; set; }

        [Display(Name = "訂單狀態")]
        public OrderType Status { get; set; }
        
        
    }
}