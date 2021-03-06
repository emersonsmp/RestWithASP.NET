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

        //INJEÇÃO DE DEPENDENCIA
        public PersonServiceImplementation(MySQLContext context)
        {
            _context = context;
        }

        public List<PersonModel> FindAll()
        {
            return _context.Persons.ToList();
        }

        public PersonModel FindByID(long id)
        {
            return _context.Persons.SingleOrDefault(p => p.Id == id);
        }

        public PersonModel Create(PersonModel person)
        {
            try
            {
                _context.Add(person);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }

            return person;
        }

        public PersonModel Update(PersonModel person)
        {
            if (!Exists(person.Id)) return new PersonModel();

            var result = _context.Persons.SingleOrDefault(p => p.Id.Equals(person.Id));

            if(result != null) { 
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(person);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            return person;
        }

        public void Delete(long id)
        {
            var result = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));

            if (result != null)
            {
                try
                {
                    _context.Persons.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        private bool Exists(long id)
        {
            return _context.Persons.Any(p => p.Id == id);
        }
    }
}
