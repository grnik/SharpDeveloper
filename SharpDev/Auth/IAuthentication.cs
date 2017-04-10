using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BusnessEntities;

namespace SharpDev.Auth
{
    public interface IAuthentication
    {
        /// <summary>
        /// Конекст (тут мы получаем доступ к запросу и кукисам)
        /// </summary>
        HttpContext HttpContext { get; set; }

        HttpResponseMessage Authentication(HttpRequestMessage request, string userName, string password,
            bool isPersistent = false);

        void LogOut(HttpResponse response);

        IPrincipal CurrentUser(HttpRequestMessage request);
    }
}
