using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Care_UP.Migrations;
using Care_UP.Models;
using Care_UP.Models.Util;
using Newtonsoft.Json;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Care_UP.Controllers
{
    public class MoneyController : ApiController
    {
        private Model1 db = new Model1();

        /// <summary>
        /// 金流基本資料(可再移到Web.config或資料庫設定)
        /// </summary>
        private BankInfoModel _bankInfoModel = new BankInfoModel
        {
            MerchantID = "MS113893343",
            HashKey = "6gfdegvpE7wwJ2zKz9GpxxVCijHgFPvz",
            HashIV = "Cxyx7qJ1IdkQNyFP",
            ReturnURL = "http://careup.rocket-coding.com/", //給前台
            NotifyURL = "http://careup.rocket-coding.com/SpgatewayNotify", //隱藏執行欄位 給資料庫
            CustomerURL = "http://yourWebsitUrl/Bank/SpgatewayNotify",
            AuthUrl = "https://ccore.spgateway.com/MPG/mpg_gateway",
            CloseUrl = "https://core.newebpay.com/API/CreditCard/Close"
        };


        /// <summary>
        /// [智付通支付]金流介接
        /// </summary>
        /// <param name="ordernumber">訂單單號</param>
        /// <param name="amount">訂單金額</param>
        /// <param name="payType">請款類型</param>
        /// <returns></returns>
        [HttpPost]
        [System.Web.Http.Route("SpgatewayPayBill")]
        public HttpResponseMessage SpgatewayPayBill(PayView payView)
        {
            Pay pay_ = new Pay();
            ModelState.Remove("Status");
            ModelState.Remove("Message");

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ModelState);

            }
            pay_.OrderId = payView.OrderId;
            db.Pays.Add(pay_);
            db.SaveChanges();

            Orders orders = db.Orders.Where(x => x.Id == payView.OrderId).FirstOrDefault();
            string version = "1.5";
            string payType = "CREDIT";
            // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
            DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));

            TradeInfo tradeInfo = new TradeInfo()
            {
                // * 商店代號
                MerchantID = _bankInfoModel.MerchantID,
                // * 回傳格式
                RespondType = "String",
                // * TimeStamp
                TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                // * 串接程式版本
                Version = version,
                // * 商店訂單編號
                MerchantOrderNo = pay_.Id.ToString(),
                // MerchantOrderNo = ordernumber,
                // * 訂單金額
                Amt = (int)orders.Total,
                // * 商品資訊
                ItemDesc = pay_.OrderId.ToString(),
                // 繳費有效期限(適用於非即時交易)
                ExpireDate = null,
                // 支付完成 返回商店網址
                ReturnURL = _bankInfoModel.ReturnURL,
                // 支付通知網址
                NotifyURL = _bankInfoModel.NotifyURL,
                // 商店取號網址
                CustomerURL = _bankInfoModel.CustomerURL,
                // 支付取消 返回商店網址
                ClientBackURL = null,
                // * 付款人電子信箱
                Email = string.Empty,
                // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                EmailModify = 0,
                // 商店備註
                OrderComment = "商店備註",
                // 信用卡 一次付清啟用(1=啟用、0或者未有此參數=不啟用)
                CREDIT = 1,
                // WEBATM啟用(1=啟用、0或者未有此參數，即代表不開啟)
                //WEBATM = null,
                // ATM 轉帳啟用(1=啟用、0或者未有此參數，即代表不開啟)
                //VACC = null,
                // 超商代碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 30 元或超過 2 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                //CVS = 1,
                // 超商條碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 20 元或超過 4 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                //BARCODE = 1
            };

            if (string.Equals(payType, "CREDIT"))
            {
                tradeInfo.CREDIT = 1;
            }
            else if (string.Equals(payType, "WEBATM"))
            {
                tradeInfo.WEBATM = 1;
            }
            else if (string.Equals(payType, "VACC"))
            {
                // 設定繳費截止日期
                tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                tradeInfo.VACC = 1;
            }
            else if (string.Equals(payType, "CVS"))
            {
                // 設定繳費截止日期
                tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                tradeInfo.CVS = 1;
            }
            else if (string.Equals(payType, "BARCODE"))
            {
                // 設定繳費截止日期
                tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                tradeInfo.BARCODE = 1;
            }

            Atom<string> result = new Atom<string>()
            {
                IsSuccess = true
            };

            var inputModel = new SpgatewayInputModel
            {
                MerchantID = _bankInfoModel.MerchantID,
                Version = version
            };

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
            // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
            var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            inputModel.TradeInfo =
                CryptoUtil.EncryptAESHex(tradeQueryPara, _bankInfoModel.HashKey, _bankInfoModel.HashIV);
            // SHA256 加密
            inputModel.TradeSha =
                CryptoUtil.EncryptSHA256(
                    $"HashKey={_bankInfoModel.HashKey}&{inputModel.TradeInfo}&HashIV={_bankInfoModel.HashIV}");

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> postData =
                LambdaUtil.ModelToKeyValuePairList<SpgatewayInputModel>(inputModel);


            //StringBuilder s = new StringBuilder();
            //s.Append("<html>");
            //s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            //s.AppendFormat("<form name='form' action='{0}' method='post'>", _bankInfoModel.AuthUrl);
            //foreach (KeyValuePair<string, string> item in postData)
            //{
            //    s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", item.Key, item.Value);
            //}

            //s.Append("</form></body></html>");


            return Request.CreateResponse(HttpStatusCode.OK, postData);


        }


        /// <summary>
        /// [智付通]金流介接(結果: 支付通知網址)
        /// </summary>
        /// 
        [HttpPost]
        [System.Web.Http.Route("SpgatewayNotify")]
        public HttpResponseMessage SpgatewayNotify()
        {

            // Status 回傳狀態 
            // MerchantID 回傳訊息
            // TradeInfo 交易資料AES 加密
            // TradeSha 交易資料SHA256 加密
            // Version 串接程式版本
            var collection = HttpContext.Current.Request;
            if (collection["MerchantID"] != null && string.Equals(collection["MerchantID"], _bankInfoModel.MerchantID) &&
                collection["TradeInfo"] != null && string.Equals(collection["TradeSha"], CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{collection["TradeInfo"]}&HashIV={_bankInfoModel.HashIV}")))
            {
                var decryptTradeInfo = CryptoUtil.DecryptAESHex(collection["TradeInfo"], _bankInfoModel.HashKey, _bankInfoModel.HashIV);

                // 取得回傳參數(ex:key1=value1&key2=value2),儲存為NameValueCollection
                NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(decryptTradeInfo);
                SpgatewayOutputDataModel convertModel =
                    LambdaUtil.DictionaryToObject<SpgatewayOutputDataModel>(
                        decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));

                Pay pay = db.Pays.Find(Convert.ToInt32(convertModel.MerchantOrderNo));
                pay.Message = convertModel.Message;
                pay.Status = convertModel.Status;
                db.Pays.Add(pay);

                if (pay.Status == "SUCCESS")
                {
                    Orders orders = db.Orders.Find(pay.OrderId);
                    orders.Status = OrderType.已付款;
                    db.Entry(orders).State = EntityState.Modified;

                    db.SaveChanges();
                }
                // TODO 將回傳訊息寫入資料庫



            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }



    }


}
