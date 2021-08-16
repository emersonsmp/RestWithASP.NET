using RestWithASPNET.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Business
{
    public interface IBookBusiness
    {
        Books Create(Books book);
        Books FindByID(long id);
        List<Books> FindAll();
        Books Update(Books book);
        void Delete(long id);
    }
}
