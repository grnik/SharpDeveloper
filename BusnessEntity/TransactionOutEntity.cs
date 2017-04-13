using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusnessEntities
{
    public class TransactionOutEntity : TransactionEntity
    {
        public string LoginFromEmail { get; set; }
        public string LoginToEmail { get; set; }
    }
}
