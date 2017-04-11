using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusnessEntities;
using BusnessServices.Exceptions;
using Data;
using Ninject;
using Ninject.Parameters;

namespace BusnessServices
{
    public class TransactionService : ITransactionService
    {
        #region Properties

        private static IKernel AppKernel = new StandardKernel(new NinjectConfig());
        IRepositories repositories;

        #endregion Properties

        #region Constructors

        public TransactionService()
        {
            repositories = AppKernel.Get<IRepositories>();
        }

        public TransactionService(IRepositories repositories)
        {
            this.repositories = repositories;
        }

        #endregion Constructors

        #region Methods

        private BusnessEntities.TransactionOutEntity Translate(Data.Transaction transactionData)
        {
            ILoginService loginService = AppKernel.Get<ILoginService>(new[] {
                new ConstructorArgument("repositories", repositories) });
            return new BusnessEntities.TransactionOutEntity()
            {
                Id = transactionData.Id,
                Date = transactionData.Date,
                Amount = transactionData.Amount,
                LoginFromId = transactionData.LoginFromId,
                LoginToId = transactionData.LoginToId,
                LoginFromEmail = transactionData.LoginFromId.HasValue ? loginService.GetById(transactionData.LoginFromId.Value).Email : string.Empty,
                LoginToEmail = transactionData.LoginToId.HasValue ? loginService.GetById(transactionData.LoginToId.Value).Email : string.Empty
            };
        }

        internal IEnumerable<BusnessEntities.TransactionOutEntity> Translate(IEnumerable<Data.Transaction> trans)
        {
            List<BusnessEntities.TransactionOutEntity> res = new List<BusnessEntities.TransactionOutEntity>();
            foreach (Data.Transaction transactionData in trans.ToList())
            {
                res.Add(Translate(transactionData));
            }

            return res;
        }

        public IEnumerable<TransactionOutEntity> GetByLoginFrom(string email)
        {
            return Translate((repositories.Transaction.GetAll().Where(t => t.LoginFrom.Email == email)));
        }

        public IEnumerable<TransactionOutEntity> GetByLoginTo(string email)
        {
            return Translate(repositories.Transaction.GetAll().Where(t => t.LoginTo.Email == email));
        }

        public IEnumerable<TransactionOutEntity> GetByLogin(string email)
        {
            var trans = GetByLoginFrom(email);
            trans = trans.Concat(GetByLoginTo(email));
            return trans;
        }

        public TransactionEntity GetById(Guid id)
        {
            var trans = repositories.Transaction.GetAll().FirstOrDefault(t => t.Id == id);

            if (trans == null)
                return null;
            else
            {
                return Translate(trans);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public TransactionOutEntity Insert(TransactionInEntity transaction, LoginOutEntity currentLogin)
        {
            if (currentLogin == null)
                throw new MyException("You must registration");
            if (string.IsNullOrEmpty(transaction.ToLogin))
            {
                throw new MyException("You must select the payment receiver");
            }

            if (transaction.Amount <= 0)
                throw new MyException("The amount must be greater than 0");

            ILoginService loginService = AppKernel.Get<ILoginService>(new[] {
                new ConstructorArgument("repositories", repositories) }); ;

            LoginOutEntity fromLogin = loginService.GetByMail(currentLogin.Email);
            if (!loginService.CheckEnough(fromLogin, transaction.Amount))
                throw new MyException("Not enough funds");

            LoginOutEntity toLogin = loginService.GetByMail(transaction.ToLogin);

            Transaction newTransaction = new Transaction(DateTime.Now, fromLogin.Id, toLogin.Id, transaction.Amount);

            repositories.Transaction.Save(newTransaction);
            repositories.Save();

            return Translate(newTransaction);
        }

        #endregion Methods
    }
}
