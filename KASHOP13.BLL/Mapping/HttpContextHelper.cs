using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Mapping
{
    public static class HttpContextHelper
    {
        public static IHttpContextAccessor Accessor { get; set; }
    }
}
