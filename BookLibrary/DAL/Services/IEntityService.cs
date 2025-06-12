using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface IEntityService<T>
    {
        public bool Exists(int id);
        public T Get(int id);
        public T Create(T entity);
        public T Update(T entity);
        public T? Delete(int id);

        public IEnumerable<T> GetAll();
    }
}
