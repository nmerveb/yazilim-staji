﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Helpers
{
    //Extention metotlar statik olmalıdır.
    public static class JwtExtentions
    {

        public static void AddApplicationError(this HttpResponse response, string message) {
            response.Headers.Add("Application-Error",message);
            //Cors sıkıntısı olmaması için herkesin istekte bulunabileceğini ve sıkıntı olduğunda kullanıcının göreceği yeri oluşturur.
            response.Headers.Add("Access-Control-Allow-Origin","*");
            response.Headers.Add("Access-Control-Expose-Header", "Application-Error");

        }

    }
}
