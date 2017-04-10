using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BusnessServices;

namespace SharpDev.Auth
{
    public class UserProvider : IPrincipal
    {
        private UserIndentity userIdentity { get; set; }

        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return userIdentity; }
        }

        public bool IsInRole(string role)
        {
            if (userIdentity.User == null)
            {
                return false;
            }
            return true; //userIdentity.User.InRoles(role);
        }

        #endregion


        public UserProvider(string name, ILoginService loginService)
        {
            userIdentity = new UserIndentity();
            userIdentity.Init(name, loginService);
        }


        public override string ToString()
        {
            return userIdentity.Name;
        }
    }
}