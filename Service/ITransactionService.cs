using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusnessEntities;

namespace BusnessServices
{
    public interface ITransactionService

    {
        IEnumerable<TransactionOutEntity> GetByLoginFrom(string email);

        IEnumerable<TransactionOutEntity> GetByLoginTo(string email);

        IEnumerable<TransactionOutEntity> GetByLogin(string email);

        TransactionEntity GetById(Guid id);


        /// <summary>
        /// Добавить новую транзакцию.
        /// </summary>
        TransactionOutEntity Insert(TransactionInEntity transaction, LoginOutEntity currentLogin);
    }
}
