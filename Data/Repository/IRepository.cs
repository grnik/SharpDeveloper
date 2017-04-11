using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> GetAll();
        bool Save(T entity);
        void Delete(Guid id);
        void Delete(T entity);
    }
}
