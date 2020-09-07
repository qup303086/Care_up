using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{
    public class ElderView
    {
       
        public int Id { get; set; }

        [Display(Name = "家屬ID")]
        public int MemberId { get; set; }


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


        [Display(Name = "手機")]
        public int Phone { set; get; }


    }
}