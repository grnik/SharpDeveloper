using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusnessEntities;
using Data;

namespace BusnessServices
{
    public interface ILoginService
    {

        IEnumerable<BusnessEntities.TransactionEntity> Transactions(BusnessEntities.LoginEntity login);

        #region Check
        string CheckEmail(string email);
        string CheckName(string name);
        string CheckPassword(string password);

        /// <summary>
        /// Проверяем почту на уникальность. Если пусто - то всегда не уникально.</summary>
        /// <returns></returns>
        string CheckUniqueEmail(string email);

        #endregion

        decimal Balance(string email);
        decimal Balance(BusnessEntities.LoginEntity login);

        bool CheckEnough(LoginEntity login, decimal amoun);

        LoginOutEntity Authentication(string email, string password);

        LoginOutEntity GetByMail(string mail);

        LoginOutEntity GetById(Guid id);

        IEnumerable<LoginOutEntity> GetAll();

        IEnumerable<LoginOutEntity> GetByName(string startName);

        /// <summary>
        /// Проверяем и сохраняем логин.
        /// </summary>
        LoginOutEntity Insert(LoginInEntity login);
    }
}
