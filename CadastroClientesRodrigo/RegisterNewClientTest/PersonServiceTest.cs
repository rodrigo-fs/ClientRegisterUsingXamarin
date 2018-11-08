using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegisterNewClient.Service;
using RegisterNewClient.Droid;
using RegisterNewClient.Infrastructure;
using Prism;
using Prism.Unity;
using Prism.Ioc;
using RegisterNewClient.Database;
using Unity;
using System.Linq;
using System.IO;
namespace RegisterNewClientTest
{
    [TestClass]
    public class PersonServiceTest
    {
        public string pathDb = "C:\\teste\\Registry.db";

        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(pathDb))
            {
                File.Delete(pathDb);
            }
            
            // TODO: delete DB file
        }

        [TestMethod]
        public void AddPersonInDbTestMethod()
        {
            //Arrange
            UnityContainer Container = new UnityContainer();
            Container.RegisterType<IPersonService, PersonService>();

            //var options = BaseTestClass.GetDatabaseOptions();
            PersonContext db = new PersonContext(pathDb);
            Container.RegisterInstance(db);
            //Act
            Person p = new Person { Name = "Rodrigo", Age = 22, Telephone = "(41)99893-7894" };
            Container.Resolve<IPersonService>().AddPersons(p);
            var comp = Container.Resolve<IPersonService>().GetPersons(p.PersonId.ToString(), 0);
            //Assert
            Assert.AreEqual(p,comp.FirstOrDefault());
            Assert.AreEqual(1, db.Persons.Count());
        }

        [TestMethod]
        public void PersonService_ShuldValidateIncorrectPerson()
        {
            //Arrange
            UnityContainer Container = new UnityContainer();
            Container.RegisterType<IPersonService, PersonService>();

            //var options = BaseTestClass.GetDatabaseOptions();
            Container.RegisterInstance(new PersonContext(pathDb));
            //Act
            Person p = new Person { Age = 22, Telephone = "(41)99893-7894" };
            var saved = Container.Resolve<IPersonService>().AddPersons(p);
            //Assert
            Assert.AreEqual(1, saved);
        }
    }
}
