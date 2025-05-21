using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class GenreService : IEntityService<Genre>
    {
        public IActionResult Create(Genre entity)
        {
            throw new NotImplementedException();
        }

        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Genre> GetAll()
        {
            throw new NotImplementedException();
        }

        public IActionResult Update(int id, object updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
