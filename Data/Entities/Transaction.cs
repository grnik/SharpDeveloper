using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Transaction : Entity
    {
        internal Transaction() { }
        public Transaction(DateTime date, Guid? loginFromId, Guid? loginToId, decimal amount)
        {
            Date = date;
            LoginFromId = loginFromId;
            LoginToId = loginToId;
            Amount = amount;
        }

        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public Guid? LoginFromId { get; set; }
        public virtual Login LoginFrom { get; set; }

        public Guid? LoginToId { get; set; }
        public virtual Login LoginTo { get; set; }
    }
}
