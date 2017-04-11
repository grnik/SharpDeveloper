using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Repositories : IRepositories, IDisposable
    {
        private readonly DataContext _context;

        public Repositories()
        {
            _context = new DataContext();
        }

        private RepositoryBase<Login, DataContext> _login;

        public RepositoryBase<Login, DataContext> Login
        {
            get
            {
                if (_login == null)
                {
                    _login = new RepositoryBase<Login, DataContext>(_context);
                }
                return _login;
            }
        }

        private RepositoryBase<Transaction, DataContext> _transaction;
        public RepositoryBase<Transaction, DataContext> Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new RepositoryBase<Transaction, DataContext>(_context);
                }
                return _transaction;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }


        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
