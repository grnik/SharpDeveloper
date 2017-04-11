using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Ninject.Modules;
using Ninject.Extensions.Factory;

namespace BusnessServices
{
    public class NinjectConfig : NinjectModule
    {
        public IRepositories repository { get; set; }

        public override void Load()
        {
            this.Bind<IRepositories>().To<Repositories>();
            this.Bind<ILoginService>().To<LoginService>();//.WithConstructorArgument(repository);
            this.Bind<ITransactionService>().To<TransactionService>();//.WithConstructorArgument(repository);
        }
    }
}
