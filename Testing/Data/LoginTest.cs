using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using NUnit.Framework;

namespace Testing.Data
{
    [TestFixture]
    public class LoginTest
    {
        DataContext context = new DataContext(SetupData.ConnectionTestDB);

        [TestCase("email1", "name1", "password1")]
        [TestCase("2", "2", "2")]
        [TestCase("very long email", "very long name", "very long password")]
        public void Create(string email, string name, string password)
        {
            Guid newId = Guid.NewGuid();
            Login loginCreate = new Login(name, email, password);
            loginCreate.Id = newId;
            context.Logins.Add(loginCreate);
            context.SaveChanges();

            Login loginRead = context.Logins.Single(l => l.Id == newId);
            Compare(loginCreate, loginRead, true);
        }

        public Login CreateItem()
        {
            Login login = new Login(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            login.Id = Guid.NewGuid();
            context.Logins.Add(login);
            context.SaveChanges();

            return login;
        }

        [TestCase("change1", "new name 1", "new password 1")]
        [TestCase("1", @"asd$$$^^^", @"@#$_asd_")]
        [TestCase("very long email", "very long name", "very long password")]
        public void Change(string newEmail, string newName, string newPassword)
        {
            Login login = CreateItem();
            Assert.That(login.Email,Is.Not.EqualTo(newEmail));
            Assert.That(login.Name, Is.Not.EqualTo(newName));
            Assert.That(login.Password, Is.Not.EqualTo(newPassword));
            login.Email = newEmail;
            login.Name = newName;
            login.Password = newPassword;
            context.SaveChanges();

            Login loginRead = context.Logins.Single(l => l.Id == login.Id);
            Assert.That(loginRead.Email, Is.EqualTo(newEmail));
            Assert.That(loginRead.Name, Is.EqualTo(newName));
            Assert.That(loginRead.Password, Is.EqualTo(newPassword));
        }

        [Test,Repeat(5)]
        public void Delete()
        {
            Login login = CreateItem();

            context.Logins.Remove(login);
            context.SaveChanges();

            Login loginRead = context.Logins.FirstOrDefault(l => l.Id == login.Id);
            Assert.IsNull(loginRead);
        }

        /// <summary>
        /// Сравнение двух объектов
        /// </summary>
        /// <param name="login1"></param>
        /// <param name="login2"></param>
        void Compare(Login login1, Login login2, bool compareId)
        {
            if (compareId)
                Assert.That(login1.Id, Is.EqualTo(login2.Id));
            Assert.That(login1.Name, Is.EqualTo(login2.Name));
            Assert.That(login1.Email, Is.EqualTo(login2.Email));
            Assert.That(login1.Name, Is.EqualTo(login2.Name));
        }
    }
}
