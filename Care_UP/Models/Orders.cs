using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

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
        public int ElderId { get; set; }

        [ForeignKey("ElderId")]
        [Display(Name = "病患ID")]
        public virtual Elders Elders { set; get; }

        public int AttendantId { get; set; }
        [ForeignKey(" AttendantId")]
        [Display(Name = "照服員ID")]
        public virtual Attendants Attendants { set; get; }

        [Required(ErrorMessage = "{0}必填")]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [Display(Name = "開始日期")]
        public DateTime StartDate { set; get; }

        [Required(ErrorMessage = "{0}必填")]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [Display(Name = "結束日期")]
        public DateTime EndDate { set; get; }

        [Display(Name = "中止日期")]
        public DateTime? StopDate { set; get; }

        [Display(Name = "訂單總額")]
        public int? Total { get; set; }


        [Display(Name = "評價")]
        public string Comment { get; set; }


        [Display(Name = "星星")]
        public int? Star { get; set; }


        [Display(Name = "訂單取消理由")]
        public string Cancel { get; set; }

        [Display(Name = "建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { get; set; }

        [Display(Name = "修改時間")]
        public DateTime? EditDate { get; set; }

        [Display(Name = "訂單狀態")]
        public OrderType Status { get; set; }

        [Display(Name = "備註")]
        public string Remark { get; set; }

        public virtual ICollection<CareRecords> CareRecordses { set; get; }
    }
}