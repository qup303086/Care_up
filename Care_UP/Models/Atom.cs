using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Care_UP.Models
{
    /// <summary>執行結果基元</summary>
    public class Atom <T> : HttpResponseMessage
    {
        /// <summary>是否成功</summary>
        public bool IsSuccess { get; set; }

        /// <summary>訊息</summary>
        public string Message { get; set; }
    }
}