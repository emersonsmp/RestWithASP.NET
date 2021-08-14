using RestWithASPNET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestWithASPNET.Services.Implementations
{
    public class PersonServiceImplementation : IPersonService
    {
        private volatile int count;

        public PersonModel Create(PersonModel person)
        {
            return person;
        }

        public void Delete(long id)
        {
            
        }

        public List<PersonModel> FindAll()
        {
            List<PersonModel> persons = new List<PersonModel>();
            for(int i=0; i< 8; i++)
            {
                PersonModel person = MockPerson(i);
                persons.Add(person);
            }
            return persons;
        }

        private PersonModel MockPerson(int i)
        {
            return new PersonModel
            {
                Id = IncrementAndGet(),
                FirstName = "Person Name" + i,
                LastName = "Person LastName" + i,
                Address = "Some Address" + i,
                Gender = "Male"
            };
        }

        private long IncrementAndGet()
        {
            return Interlocked.Increment(ref count);
        }

        public PersonModel FindByID(long id)
        {
            return new PersonModel
            {
                Id = id,
                FirstName = "Leandro",
                LastName = "Costa",
                Address = "Uberlandia - Minas Gerais - Brasil",
                Gender = "Male"
            };
        }

        public PersonModel Update(PersonModel person)
        {
            return person;
        }
    }
}
