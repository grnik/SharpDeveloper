using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Http;

namespace SharpDev
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API

            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // отключаем возможность вывода данных в формате xml
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
