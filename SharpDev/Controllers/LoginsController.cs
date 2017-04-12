using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using BusnessServices;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BusnessEntities;
using BusnessServices.Exceptions;
using Ninject;
using SharpDev.Filters;

namespace SharpDev.Controllers
{
    //[AttributeRouting.RoutePrefix("v1/Logins/{action}/{id}")]
    public class LoginsController : BaseController
    {

        private ILoginService loginService;

        public LoginsController()
             : base()
        {
            this.loginService = AppKernel.Get<ILoginService>();
        }

        /// <summary>
        /// Возвращает список логинов начинающихся с символов.
        /// http://localhost:44683/v1/Logins/List?start=5
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [GET("Get")]
        [System.Web.Mvc.HttpGet]
        public IHttpActionResult Get(string term)
        {
            try
            {
                return Ok<IEnumerable<LoginOutEntity>>(loginService.GetByName(term).ToList());
            }
            catch (MyException e)
            {
                return Results(e);
            }
            catch (System.Exception e)
            {
                return Results(e);
            }
        }

        [GET("All")]
        [System.Web.Mvc.HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                return Get(string.Empty);
            }
            catch (MyException e)
            {
                return Results(e);
            }
            catch (System.Exception e)
            {
                return Results(e);
            }
        }

        [ApiAuthenticationFilter(true)]
        public IHttpActionResult GetBalance()
        {
            try
            {
                return Ok(loginService.Balance(CurrentUserEmail));
            }
            catch (MyException e)
            {
                return Results(e);
            }
            catch (System.Exception e)
            {
                return Results(e);
            }
        }

        //[System.Web.Mvc.HttpPost]
        //public HttpResponseMessage Authentication([FromBody]LoginInEntity loginIn)
        //{
        //    return Auth.Authentication(Request, loginIn.Email, loginIn.Password, false);
        //}

        [System.Web.Mvc.HttpPost]
        public IHttpActionResult Post([FromBody]LoginInEntity login)
        {
            try
            {
                LoginOutEntity loginOut = loginService.Insert(login);
                return Ok(loginOut);
            }
            catch (MyException e)
            {
                return Results(e);
            }
            catch (System.Exception e)
            {
                return Results(e);
            }
        }
    }
}