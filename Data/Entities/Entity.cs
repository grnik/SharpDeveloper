using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{ 
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.Empty;
        }

        public Guid Id { get; set; }

        public bool IsNew()
        {
            return Id == Guid.Empty;
        }
    }
}
