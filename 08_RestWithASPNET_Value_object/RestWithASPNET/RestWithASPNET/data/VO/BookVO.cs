using RestWithASPNET.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Data.VO
{
    public class BookVO
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public DateTime LauchDate { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
    }
}
