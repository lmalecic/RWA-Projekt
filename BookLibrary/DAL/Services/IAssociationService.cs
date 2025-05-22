using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL.DTO;

namespace DAL.Services
{
    public interface IAssociationService<T>
    {
        public IEnumerable<T> GetAll();
        public T Create(int id1, int id2);
        public T? Delete(int id1, int id2);
        public bool Exists(int id1, int id2);
    }
}
