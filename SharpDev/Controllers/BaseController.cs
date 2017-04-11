using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using BusnessEntities;
using SharpDev.Auth;
using BusnessServices.Exceptions;
using Ninject;

namespace SharpDev.Controllers
{
    public class BaseController : ApiController
    {
        protected static IKernel AppKernel = new StandardKernel(new NinjectConfig());

        public IAuthentication Auth { get; set; }
        public LoginOutEntity CurrentUser
        {
            get
            {
                return ((UserIndentity)Auth.CurrentUser(Request).Identity).User;
            }
        }

        public BaseController()
        {
            Auth = new CustomAuthentication();
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
