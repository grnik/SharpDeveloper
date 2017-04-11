using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using BusnessEntities;
using BusnessServices;
using BusnessServices.Exceptions;
using Ninject;

namespace SharpDev.Auth
{
    public class CustomAuthentication : IAuthentication
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private const string cookieName = "__AUTH_COOKIE";
        private readonly TimeSpan ExpiresTime = new TimeSpan(1, 0, 0, 0);

        public HttpContext HttpContext { get; set; }

        private static IKernel AppKernel = new StandardKernel(new NinjectConfig());
        public ILoginService LoginService = AppKernel.Get<ILoginService>();

        #region IAuthentication Members

        //TODO:Сделать обновление куки
        public HttpResponseMessage Authentication(HttpRequestMessage request, string userName, string password, bool isPersistent = false)
        {
            LoginOutEntity retUser = LoginService.Authentication(userName, password);

            if (retUser == null)
            {
                return request.CreateResponse<LoginOutEntity>(HttpStatusCode.Unauthorized, null);
            }

            var ticket = new FormsAuthenticationTicket(
                  1,
                  userName,
                  DateTime.Now,
                  DateTime.Now.Add(FormsAuthentication.Timeout),
                  isPersistent,
                  string.Empty,
                  FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            DateTime expires = DateTime.Now + ExpiresTime;
            //Create the cookie.
            var cookie = new CookieHeaderValue(cookieName, encTicket); // имя куки - id, значение - 12345
            cookie.Expires = expires; // время действия куки - 1 день
            cookie.Domain = request.RequestUri.Host; // домен куки
            cookie.Path = "/"; // путь куки
            HttpResponseMessage response = request.CreateResponse<LoginOutEntity>(HttpStatusCode.OK, retUser);
            response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return response;
        }

        public void LogOut(HttpResponse response)
        {
            var httpCookie = response.Cookies[cookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private IPrincipal _currentUser;
        public IPrincipal CurrentUser(HttpRequestMessage request)
        {
            if (_currentUser == null)
            {
                try
                {
                    CookieHeaderValue authCookie = request.Headers.GetCookies(cookieName).FirstOrDefault();
                    if (authCookie != null)
                    {
                        var ticket = FormsAuthentication.Decrypt(authCookie[cookieName].Value);
                        _currentUser = new UserProvider(ticket.Name, LoginService);
                    }
                    else
                    {
                        _currentUser = new UserProvider(null, null);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Failed authentication: " + ex.Message);
                    _currentUser = new UserProvider(null, null);
                }
            }
            return _currentUser;
        }

        #endregion
    }
}
