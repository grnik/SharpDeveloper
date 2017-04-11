using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusnessEntities
{
    public class TransactionOutEntity : TransactionEntity
    {
        protected Guid LoginFromId { get; set; }
        protected Guid LoginToId { get; set; }

        public string LoginFromEmail { get; set; }
        public string LoginToEmail { get; set; }
    }
}
