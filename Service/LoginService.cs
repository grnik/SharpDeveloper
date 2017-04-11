using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusnessEntities;
using BusnessServices.Exceptions;
using Data;
using Ninject;
using Ninject.Parameters;
using Login = Data.Login;

namespace BusnessServices
{
    public class LoginService : ILoginService
    {
        #region Properties

        //TODO: вынести в загружочный файл.
        public const decimal StartBalance = 500;

        private static IKernel AppKernel = new StandardKernel(new NinjectConfig());
        IRepositories repositories;
        ITransactionService transactionService;

        #endregion Properties

        #region Constructors

        public LoginService()
        {
            repositories = AppKernel.Get<IRepositories>();
            ConstructorArgument param = new ConstructorArgument("repositories", repositories);
            transactionService = AppKernel.Get<ITransactionService>(param);
        }

        public LoginService(IRepositories repositories)
        {
            this.repositories = repositories;
            transactionService = AppKernel.Get<ITransactionService>(new[] {
                new ConstructorArgument("repositories", repositories) });
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<BusnessEntities.TransactionEntity> Transactions(BusnessEntities.LoginEntity login)
        {
            return transactionService.GetByLogin(login.Email);
        }

        internal IEnumerable<LoginOutEntity> Translate(IEnumerable<Login> logins)
        {
            List<LoginOutEntity> res = new List<LoginOutEntity>();
            foreach (Login login in logins)
            {
                res.Add(Translate(login));
            }

            return res;
        }

        internal LoginOutEntity Translate(Login login)
        {
            LoginOutEntity loginOut = new LoginOutEntity()
            {
                Id = login.Id,
                Email = login.Email,
                Name = login.Name
            };

            return loginOut;
        }

        #region Check
        public string CheckEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return "Email is null";

            string pattern = @".+@.+\..+";
            if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
            {
                throw new MyException("Email is wrong");
            }

            return string.Empty;
        }
        public string CheckName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return "Name is null";

            return string.Empty;
        }
        public string CheckPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
                return "Password is null";

            return string.Empty;
        }

        /// <summary>
        /// Проверяем почту на уникальность. Если пусто - то всегда не уникально.</summary>
        /// <returns></returns>
        public string CheckUniqueEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return "Email is nor unique!";

            if (repositories.Login.GetAll().Any(l => (l.Email.ToUpper() == email.ToUpper())))
                return "Email is nor unique!";
            else
                return string.Empty;
        }

        #endregion

        #region Balance
        private decimal Debit(string email)
        {
            return transactionService.GetByLoginTo(email).Sum(t => t.Amount);
        }
        private decimal Credit(string email)
        {
            return transactionService.GetByLoginFrom(email).Sum(t => t.Amount);
        }
        private decimal Balance(string email)
        {
            return Debit(email) - Credit(email);
        }
        public decimal Balance(BusnessEntities.LoginEntity login)
        {
            return Balance(login.Email);
        }

        public bool CheckEnough(LoginEntity login, decimal amoun)
        {
            if (amoun <= 0)
                throw new Exception("The amount must be positive");
            return Balance(login.Email) >= amoun;
        }

        #endregion

        public LoginOutEntity Authentication(string email, string password)
        {
            if (String.IsNullOrEmpty(email))
                return null;
            if (String.IsNullOrEmpty(password))
                return null;

            Login login = repositories.Login.GetAll()
                .FirstOrDefault(l => (l.Email.ToUpper() == email.ToUpper()) && (l.Password == password));

            return login == null ? null : Translate(login);
        }

        internal Login GetDataByMail(string mail)
        {
            var loginData = repositories.Login.GetAll().FirstOrDefault(l => l.Email.ToUpper() == mail.ToUpper());
            return loginData;
        }

        public LoginOutEntity GetByMail(string mail)
        {
            var loginData = GetDataByMail(mail);
            return loginData != null ? Translate(loginData) : null;
        }

        public LoginOutEntity GetById(Guid id)
        {
            var loginData = repositories.Login.GetAll().FirstOrDefault(l => l.Id == id);
            return Translate(loginData);
        }

        public IEnumerable<LoginOutEntity> GetAll()
        {
            List<LoginOutEntity> logins = new List<LoginOutEntity>();

            return Translate(repositories.Login.GetAll());
        }

        public IEnumerable<LoginOutEntity> GetByName(string startName)
        {
            if (string.IsNullOrEmpty(startName))
                return GetAll();

            List<LoginOutEntity> logins = new List<LoginOutEntity>();

            return Translate(repositories.Login.GetAll().Where(l => l.Name.StartsWith(startName)));
        }

        /// <summary>
        /// Проверяем и сохраняем логин.
        /// </summary>
        public LoginOutEntity Insert(LoginInEntity login)
        {
            string error = CheckEmail(login.Email);
            error += CheckUniqueEmail(login.Email);
            error += CheckName(login.Name);
            error += CheckPassword(login.Password);

            if (!String.IsNullOrEmpty(error))
                throw new MyException(error);

            Login dataLogin = new Login(login.Name, login.Email, login.Password);
            repositories.Login.Save(dataLogin);
            CreateStartBalance(dataLogin);
            repositories.Save();

            return Translate(dataLogin);
        }

        /// <summary>
        /// Создаем начальный баланс
        /// </summary>
        /// <param name="login"></param>
        private void CreateStartBalance(Login login)
        {
            Transaction newTransaction = new Transaction(DateTime.Now, null, login.Id, StartBalance);
            repositories.Transaction.Save(newTransaction);
        }

        #endregion Methods
    }
}
