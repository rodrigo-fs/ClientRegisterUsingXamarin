using RegisterNewClient.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegisterNewClient.Service
{
    public interface IPersonService
    {
        int DeletePersons(string id, int ind);
        IQueryable<Person> GetPersons(string id, int ind);
        IQueryable<Person> GetAllPersons();
        IQueryable<Person> GetAllPersonsOrderByAge();
        int AddPersons(Person p);
        string GenerateJson();
        int DeleteAllPersons();
    }
}
