
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loveproject.dataa.Abstract
{
    public interface IRepository<T>
    {
        public List<T> GetAll();

        public T GetById(int id);

        public void Delete(T entity);

        public void Add(T entity);


    }
}
