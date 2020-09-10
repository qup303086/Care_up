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

        public static string Service(string serviceCode)
        {
            string service = "";
            string a = "";
            if (serviceCode.Contains(','))
            {
                string[] aa = serviceCode.Split(',');

                foreach (string item in aa)
                {
                    a = Switch(item, a);
                    if (service.Length > 0)
                    {
                        service += "," + a;
                    }
                    else
                    {
                        service += a;
                    }
                }
            }
            else
            {
                service = Switch(serviceCode, a);
            }
            return service;
        }

        public static string Switch(string item, string a)
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
                    a = "備餐";
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
    }
}