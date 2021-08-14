using RestWithASPNET.Model;
using System.Collections.Generic;

namespace RestWithASPNET.Services.Implementations
{
    public interface IPersonService
    {
        PersonModel Create(PersonModel person);
        PersonModel FindByID(long id);
        List<PersonModel> FindAll();
        PersonModel Update(PersonModel person);
        void Delete(long id);

    }
}
