using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Jose;

namespace Care_UP.Models
{
    #region getToken
    public class Token
    {
        public string GenerateToken(int id, string email,string identity)
        {
            string secret = "careUpppp";//加解密的key,如果不一樣會無法成功解密
            Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            claim.Add("ID", id);
            claim.Add("Email", email);
            claim.Add("Identity", identity);
            claim.Add("Exp", DateTime.Now.AddSeconds(Convert.ToInt32("86400")).ToString());//Token 時效設定秒
            var payload = claim;
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS512);//產生token
            return token;
        }
        public TokenPayload GetToken(string token)
        {
            string secret = "careUpppp";//加解密的key,如果不一樣會無法成功解密
            var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                token,
                Encoding.UTF8.GetBytes(secret),
                JwsAlgorithm.HS512);
            TokenPayload tokenPayload = new TokenPayload();
            tokenPayload.ID =Convert.ToInt32(jwtObject["ID"]);
            tokenPayload.Identity = jwtObject["Identity"].ToString();
            return tokenPayload;
        }

    }
    #endregion

    #region 查察token
    public class JwtAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string secret = "careUpppp";//加解密的key,如果不一樣會無法成功解密
            var request = actionContext.Request;
            if (!WithoutVerifyToken(request.RequestUri.ToString()))
            {
                if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
                {
                  
                    throw new System.Exception("沒token");
                }
                else
                {
                    //解密後會回傳Json格式的物件(即加密前的資料)
                    var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                        request.Headers.Authorization.Parameter,
                        Encoding.UTF8.GetBytes(secret),
                        JwsAlgorithm.HS512);

                    if (IsTokenExpired(jwtObject["Exp"].ToString()))
                    {
                        throw new System.Exception("token過期");
                    }
                }
            }

            base.OnActionExecuting(actionContext);
        }


        //加例外
        public bool WithoutVerifyToken(string requestUri)
        {
            //if (requestUri.EndsWith("/MemberLogin"))
            //{
            //    return true;
            //}
            //return false;
            return false;
        }

        //驗證token時效
        public bool IsTokenExpired(string dateTime)
        {
            return Convert.ToDateTime(dateTime) < DateTime.Now;
        }

    }


    #endregion

}