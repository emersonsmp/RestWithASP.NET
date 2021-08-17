using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Repository
{
    public interface IRepository
    {
        PersonModel Create(PersonModel person);
        PersonModel FindByID(long id);
        List<PersonModel> FindAll();
        PersonModel Update(PersonModel person);
        void Delete(long id);
        public bool Exists(long id);
    }
}
