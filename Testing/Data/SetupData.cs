using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Data;

namespace Testing.Data
{
    [SetUpFixture]
    class SetupData
    {
        public const string ConnectionTestDB = "DBTest";

        [SetUp]
        void RunBeforeAnyTests()
        {
            DataContext context = new DataContext(ConnectionTestDB);
            if (context.Database.Exists())
                context.Database.Delete();
            context.Database.Create();

        }

        [TearDown]
        void RunAfterAnyTests()
        {
            DataContext context = new DataContext(ConnectionTestDB);
            if (context.Database.Exists())
                context.Database.Delete();
        }
    }
}
