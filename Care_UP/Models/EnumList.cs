using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Care_UP.Models
{

    public enum IdType
    {
        家屬= 1,
        照服員= 2,
        
    }
    public enum GenderType
    {
        男 = 0,
        女 = 1,
        其他 = 2,
    }
    public enum WhetherType
    {
        是 = 0,
        否 = 1,

    }
    public enum BodyType
    {
        糖尿病 = 0,
        骨折 = 1,
        高血壓 = 2,
        身心靈障礙 = 3,
        目前有服用藥物 = 4,
    }
    public enum EquipmentType
    {
        成人紙尿布 = 0,
        血壓檢測器 = 1,
        拐杖 = 2,
    }
    public enum ServiceItemsType
    {
        協助如廁 = 0,
        協助進食用藥 = 1,
        帶購物品 = 2,
        備餐 = 3,
        身心靈陪伴 = 4,
        環境整理 = 5,
    }
    public enum OrderType
    {
        成立訂單 = 10,//家屬新增訂單
        確認訂單 = 20, //服務員確認訂單
        待付款 = 21,//服務員等待家屬確認是否付款
        已付款 = 11,//家屬已付款
        待服務進行 = 22, //照服員等待服務進行
        服務進行中 = 23, //照服員照顧中
        待評價 = 14, //家屬已評價
        待收款 = 24,//照服員等待收款
        已取消 = 01,
        已完成 = 02,

    }
}
