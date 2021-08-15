using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestWithASPNET.Services.Implementations
{
    public class PersonServiceImplementation : IPersonService
    {
        private MySQLContext _context;

        public PersonServiceImplementation(MySQLContext context)
        {
            _context = context;
        }

        public PersonModel Create(PersonModel person)
        {
            return person;
        }

        public void Delete(long id)
        {
            
        }

        public List<PersonModel> FindAll()
        {
            return _context.Persons.ToList();
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
