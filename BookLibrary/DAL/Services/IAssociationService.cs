using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface IAssociationService<T>
    {
        public IEnumerable<T> GetAll();
        public T Create(T entity);
        public T? Delete(T entity);
        public bool Exists(T entity);
    }
}
