using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class RepositoryBase<TEntity,TCnt> : IRepository<TEntity>
        where TEntity : Entity where TCnt:DbContext
    {
        protected readonly TCnt Context;
        internal DbSet<TEntity> DbSet;

        public RepositoryBase(TCnt context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual bool Save(TEntity entity)
        {
            if (entity.IsNew())
            {
                entity.Id = Guid.NewGuid();
                DbSet.Add(entity);
            }
            else
            {
                DbSet.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }

            return true;
        }


        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(Guid id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        protected virtual void CopyEntry(TEntity dbEntity, TEntity entity)
        {
            foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
            {
                propertyInfo.SetValue(dbEntity, propertyInfo.GetValue(entity));
            }
        }
    }
}
