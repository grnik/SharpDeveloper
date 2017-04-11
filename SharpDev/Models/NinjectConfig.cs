using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using BusnessServices;

namespace SharpDev
{
    public class NinjectConfig : NinjectModule
    {

        public override void Load()
        {
            this.Bind<ILoginService>().To<LoginService>();
            this.Bind<ITransactionService>().To<TransactionService>();
        }
    }
}
