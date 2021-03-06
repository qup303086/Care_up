﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{

    
    public enum GenderType
    {
        男 = 0,
        女 = 1,
        其他 = 2,
    }

    public enum Whether
    {
        否 = 1,
        是 = 2,
    }
    public enum OrderType
    {
        等待照服員確認訂單=10,
        待付款=11,
        已付款=12,
        照服員待收款 = 13,
        未於訂單開始前付款 = 19,
        服務進行中=22,
        已取消=01, 
        已完成=02,
        家屬中斷=03,
        照服員中斷=07,
        待退款 =04,
        照服員拒接 =05,
   
    }
    public enum ServiceTime
    {
        白天 = 01,
        傍晚 = 02,
        凌晨 = 03,
    }
}
