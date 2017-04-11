using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Login : Entity
    {
        internal Login() { }

        public Login(string name, string email, string password)
        {
            Id = Guid.Empty;
            Name = name;
            Email = email;
            Password = password;
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        //public virtual ICollection<Transaction> LoginFrom { get; set; }
        //public virtual ICollection<Transaction> LoginTo { get; set; }
    }
}
