using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Business
{
    public interface IPersonBusiness
    {
        PersonModel Create(PersonModel person);
        PersonModel FindByID(long id);
        List<PersonModel> FindAll();
        PersonModel Update(PersonModel person);
        void Delete(long id);

    }
}
