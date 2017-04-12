using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using BusnessEntities;
using BusnessServices;
using BusnessServices.Exceptions;
using Ninject;
using SharpDev.Filters;

namespace SharpDev.Controllers
{
    public class BaseController : ApiController
    {
        protected static IKernel AppKernel = new StandardKernel(new NinjectConfig());
        protected ILoginService loginService;

        public string CurrentUserEmail
        {
            get
            {
                if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if (basicAuthenticationIdentity != null)
                    {
                        var email = basicAuthenticationIdentity.UserName;
                        return email;
                    }
                }
                return null;
            }
        }
        public Guid CurrentUserId
        {
            get
            {
                if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if (basicAuthenticationIdentity != null)
                    {
                        var userId = basicAuthenticationIdentity.UserId;
                        return userId;
                    }
                }
                return Guid.Empty;
            }
        }

        public BaseController()
        {
            loginService = AppKernel.Get<ILoginService>();
        }

        #region Exception result

        public System.Web.Http.IHttpActionResult Results(MyException exception)
        {
            if (exception.InnerException == null)
                return new BadRequestErrorMessageResult(exception.Message, this);
            else
                return new BadRequestErrorMessageResult(exception.Message + exception.InnerException.Message, this);
        }

        public System.Web.Http.IHttpActionResult Results(Exception exception)
        {
            return new InternalServerErrorResult(this);
        }

        #endregion
    }
}
