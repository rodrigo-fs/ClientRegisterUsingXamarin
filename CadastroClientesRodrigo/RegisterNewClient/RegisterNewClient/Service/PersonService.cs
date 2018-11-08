using Newtonsoft.Json;
using RegisterNewClient.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace RegisterNewClient.Service
{
    public class PersonService : IPersonService
    {
        private readonly PersonContext context;

        public PersonService(PersonContext pessoaContext)
        {
            context = pessoaContext;
        }

        public IQueryable<Person> GetPersons(string id, int ind)
        {
            if (ind == 0)
            {
                int Aux;
                try
                {
                    Aux = int.Parse(id);
                }
                catch (Exception ex)
                {
                    return context.Persons.Where(p => p.PersonId == 0);
                }
                return context.Persons.Where(p => p.PersonId == Aux);
            }
            else
            {
                return context.Persons.Where(p => p.Name.Equals(id));
            }

        }

        public IQueryable<Person> GetAllPersons()
        {
           return from p in context.Persons select p;
        }

        public IQueryable<Person> GetAllPersonsOrderByAge()
        {
            return from p in context.Persons orderby p.Age select p;
        }

        public int DeletePersons(string id, int ind)
        {
            if (ind == 0)
            {
                int Aux;
                try
                {
                    Aux = int.Parse(id);
                }
                catch (Exception ex)
                {
                    return -1;
                }
                var Person = context.Persons.SingleOrDefault(p => p.PersonId == Aux);
                if (Person != null)
                {
                    if (Person.ImagePath != null)
                    {
                        if (!Person.ImagePath.Equals("userIcon.png"))
                        {
                            try
                            {
                                File.Delete(Person.ImagePath);
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(@"                  ERROR {0}", e.Message);
                            }
                        }
                    }
                    context.Persons.Remove(Person);
                    context.SaveChanges();
                    return 1;

                }
                else
                {
                    return 0;
                }
            }
            else
            {
                var Persons = GetPersons(id, ind);
                var Count = Persons.Count();
                if (Count > 1)
                {
                    for (int x = 0; x < Count; x++)
                    {
                        var Person = Persons.First();
                        if (!Person.ImagePath.Equals("userIcon.png"))
                        {
                            try
                            {
                                File.Delete(Person.ImagePath);
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(@"                  ERROR {0}", e.Message);
                            }
                        }
                        context.Persons.Remove(Person);
                        context.SaveChanges();
                    }
                    return 1;
                }
                else if (Count == 1)
                {
                    var Person = Persons.SingleOrDefault(p => p.Name.Equals(id));
                    if (!Person.ImagePath.Equals("userIcon.png"))
                    {
                        try
                        {
                            File.Delete(Person.ImagePath);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(@"                  ERROR {0}", e.Message);
                        }
                    }
                    context.Persons.Remove(Person);
                    context.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int AddPersons(Person p)
        {
            try
            {
                context.Persons.Add(p);
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
  
        }

        public string GenerateJson()
        {
            var Persons = GetAllPersons();
            var Query = from Person in Persons
                        select new
                        {
                            Name = Person.Name,
                            Age = Person.Age,
                            Telephone = Person.Telephone
                        };
            return JsonConvert.SerializeObject(Query.ToArray());
        }



        public int DeleteAllPersons()
        {
            var People = GetAllPersons();
            foreach (var person in People)
            {
                //context.Persons.SingleOrDefault();
                try
                {
                    if (person != null)
                    {
                        if (!person.ImagePath.Equals("userIcon.png") || !string.IsNullOrEmpty(person.ImagePath))
                        {
                            File.Delete(person.ImagePath);
                            context.Persons.Remove(person);
                            context.SaveChanges();
                        }

                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(@"                  ERROR {0}", e.Message);
                }
            }
        
            if (context.Persons.Count() == 0)
                return 1;
            else
                return 0;
        }        
    }
}
