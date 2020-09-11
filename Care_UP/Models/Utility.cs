using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Jose;

namespace Care_UP.Models
{
    public class Utility
    {
        #region "將使用者資料寫入cookie,產生AuthenTicket"
        /// <summary>
        /// 將使用者資料寫入cookie,產生AuthenTicket
        /// </summary>
        /// <param name="userData">使用者資料</param>
        /// <param name="userId">UserAccount</param>
        static public void SetAuthenTicket(string userData, string userId)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立Cookie
            HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將Cookie寫入回應
            HttpContext.Current.Response.Cookies.Add(authenticationcookie);
        }
        #endregion
        #region "密碼加密"
        public const int DefaultSaltSize = 5;
        /// <summary>
        /// 產生Salt
        /// </summary>
        /// <returns>Salt</returns>
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[DefaultSaltSize];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
        /// <summary>
        /// Computes a salted hash of the password and salt provided and returns as a base64 encoded string.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use in the hash.</param>
        public static string GenerateHashWithSalt(string password, string salt)
        {
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            HashAlgorithm algorithm = new SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // return the has as a base 64 encoded string
            return Convert.ToBase64String(hash);
        }
        #endregion

        #region 搜尋錯誤

        //try { 
        //    db.SaveChanges();
        //}
        //catch (DbEntityValidationException ex)
        //{
        //    var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
        //    var getFullMessage = string.Join("; ", entityError);
        //    var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);

        //}

        #endregion

        public static string[] Service(string serviceCode)
        {
            string a = "";
            if (serviceCode.Contains(','))
            {
                string[] service = serviceCode.Split(',');

                for (int i = 0; i < service.Length; i++)
                {
                    service[i] = ServiceSwitch(service[i], a);
                }
                return service;
            }
            else
            {
                string[] service = new[] { ServiceSwitch(serviceCode, a) };
                return service;
            }
        }

        private static string ServiceSwitch(string item, string a)
        {
            switch (item)
            {
                case "01":
                    a = "協助如廁";
                    break;
                case "02":
                    a = "協助進食";
                    break;
                case "03":
                    a = "代購物品";
                    break;
                case "04":
                    a = "備餐";
                    break;
                case "05":
                    a = "身心靈陪伴";
                    break;
                case "06":
                    a = "環境整理";
                    break;
            }
            return a;
        }

        public static string ServiceTime(string timeCode)
        {
            switch (timeCode)
            {
                case "01":
                    timeCode = "白天(09:00-18:00";
                    break;

                case "02":
                    timeCode = "傍晚(15:00-23:00)";
                    break;

                case "03":
                    timeCode = "凌晨(23:00-07:00)";
                    break;
            }
            return timeCode;
        }

        public static string[] EldersBody(string serviceCode)
        {
            string a = "";
            if (serviceCode.Contains(','))
            {
                string[] service = serviceCode.Split(',');

                for (int i = 0; i < service.Length; i++)
                {
                    service[i] = BodySwitch(service[i], a);
                }
                return service;
            }
            else
            {
                string[] service = new[] { BodySwitch(serviceCode, a) };
                return service;
            }

        }

        private static string BodySwitch(string item, string a)
        {
            switch (item)
            {
                case "01":
                    a = "糖尿病";
                    break;
                case "02":
                    a = "骨折";
                    break;
                case "03":
                    a = "高血壓";
                    break;
                case "04":
                    a = "身心障礙";
                    break;
                case "05":
                    a = "行動不便";
                    break;
                case "06":
                    a = "精神疾病";
                    break;
            }
            return a;
        }

        public static string[] EldersEquipment(string serviceCode)
        {
            string a = "";
            if (serviceCode.Contains(','))
            {
                string[] service = serviceCode.Split(',');

                for (int i = 0; i < service.Length; i++)
                {
                    service[i] = EquipmentSwitch(service[i], a);
                }
                return service;
            }
            else
            {
                string[] service = new[] { EquipmentSwitch(serviceCode, a) };
                return service;
            }

        }

        private static string EquipmentSwitch(string item, string a)
        {
            switch (item)
            {
                case "01":
                    a = "成人紙尿布";
                    break;
                case "02":
                    a = "輪椅";
                    break;
                case "03":
                    a = "拐杖";
                    break;
                case "04":
                    a = "夜壺";
                    break;
                case "05":
                    a = "輔助器";
                    break;
                case "06":
                    a = "護具";
                    break;
            }
            return a;
        }

        public static string ArrayToString(string[] array)
        {
            string cc = "";
            if (array.Length<2)
            {
                cc = array[0];
            }
            foreach (string item in array)
            {
                if (cc == "")
                {
                    cc += item;
                }
                else
                {
                    cc += "," + item;
                }
            }
            return cc;
        }
    }
}