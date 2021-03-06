using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryUpdateTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void Update_Should_Save_Modified_Business_Name(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);

            var contact2 = new Contact { Name = "Test User 2" };
            repository.Add(contact2);

            contact.Name = "Test User - Updated";
            repository.Update(contact);

            var updated = repository.Get(contact.ContactId);
            var notUpdated = repository.Get(contact2.ContactId);

            updated.Name.ShouldEqual("Test User - Updated");
            notUpdated.Name.ShouldEqual("Test User 2");
        }

        //[ExecuteForAllRepositories]
        //[ExpectedException(typeof(Exception))]
        //public void Update_Should_Throw_Exception_If_Item_Does_Not_Exist()
        //{
        //    Repository.Update(new Contact());
        //}

        [ExecuteForAllRepositories]
        public void Update_Should_Update_Multiple_Items(IRepository<Contact, string> repository)
        {
            IList<Contact> contacts = new List<Contact>
                                        {
                                            new Contact {Name = "Contact 1"},
                                            new Contact {Name = "Contact 2"},
                                            new Contact {Name = "Contact 3"},
                                        };

            repository.Add(contacts);
            var items = repository.GetAll().ToList();
            items.Count().ShouldEqual(3);

            foreach (var contact in contacts.Take(2))
            {
                contact.Name += "UPDATED";
            }

            repository.Update(contacts);
            items = repository.GetAll().ToList();
            items.Count(x => x.Name.EndsWith("UPDATED")).ShouldEqual(2);
        }
    }
}