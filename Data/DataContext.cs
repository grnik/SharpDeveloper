using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DbConnection")
        {
        }
        public DataContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Login> Logins { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
