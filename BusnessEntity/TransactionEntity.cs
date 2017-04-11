using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusnessEntities
{
    public class TransactionEntity
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public virtual Guid? LoginFromId { get; set; }
        public virtual Guid? LoginToId { get; set; }
    }
}
