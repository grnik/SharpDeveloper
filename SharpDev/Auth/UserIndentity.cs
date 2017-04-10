using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BusnessEntities;
using BusnessServices;

namespace SharpDev.Auth
{
    public class UserIndentity : IIdentity
    {
        public LoginOutEntity User { get; set; }

        public string AuthenticationType
        {
            get
            {
                return typeof(LoginOutEntity).ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Email;
                }
                //иначе аноним
                return "anonym";
            }
        }

        public void Init(string email, ILoginService loginService)
        {
            if (!string.IsNullOrEmpty(email))
            {
                User = loginService.GetByMail(email);
            }
        }
    }
}

