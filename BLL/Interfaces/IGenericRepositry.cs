using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGenericRepositry<T> where T : BaseEntity
    {
        Task< IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
