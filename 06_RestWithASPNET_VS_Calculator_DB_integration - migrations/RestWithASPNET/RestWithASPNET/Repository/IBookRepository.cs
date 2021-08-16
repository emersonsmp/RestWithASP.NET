using RestWithASPNET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Repository
{
    public interface IBookRepository
    {
        Books Create(Books person);
        Books FindByID(long id);
        List<Books> FindAll();
        Books Update(Books book);
        void Delete(long id);
        public bool Exists(long id);
    }
}
