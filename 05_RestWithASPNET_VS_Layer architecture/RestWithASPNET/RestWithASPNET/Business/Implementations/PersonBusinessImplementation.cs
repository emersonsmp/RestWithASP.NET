using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNET.Business.Implementations
{
    //CAMADA DE NEGOCIO: PARA COLOCAR AS REGRAS DE NEGOCIO E VALIDAÇÕES
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IPersonRepository _repository;

        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;
        }

        public List<PersonModel> FindAll()
        {
            return _repository.FindAll();
        }

        public PersonModel FindByID(long id)
        {
            return _repository.FindByID(id);
        }

        public PersonModel Create(PersonModel person)
        {
            //REGRA EXEMPLO: SÓ ADD SE ANO DE NASC > 1900
                return _repository.Create(person);
        }

        public PersonModel Update(PersonModel person)
        {
            return _repository.Update(person);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
