using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Repository.Implementations
{
    
    public class BookRepositoryImplementation : IBookRepository
    {
        private MySQLContext _context;

        public BookRepositoryImplementation(MySQLContext context)
        {
            _context = context;
        }

        public List<Book> FindAll()
        {
            try
            {
                return _context.Books.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Book FindByID(long id)
        {
            return _context.Books.SingleOrDefault(p => p.Id.Equals(id));
        }

        public Book Create(Book book)
        {
            try
            {
                _context.Add(book);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return book;
        }

        public void Delete(long id)
        {
            var result = FindByID(id);

            if (result != null)
            {
                try
                {
                    _context.Books.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }      

        public Book Update(Book book)
        {
            if (!Exists(book.Id)) return null;

            var result = FindByID(book.Id);

            if (result != null) {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(book);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return book;
        }

        public bool Exists(long id)
        {
            return _context.Books.Any(book => book.Id == id);
        }
    }
}
