using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IRepositories
    {
        RepositoryBase<Login, DataContext> Login { get; }
        RepositoryBase<Transaction, DataContext> Transaction { get; }

        int Save();

    }
}
