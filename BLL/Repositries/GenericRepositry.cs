using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositries
{
    public class GenericRepositry<T> :IGenericRepositry<T> where T : BaseEntity
    {
        private protected AppDbContext _context;
       
        public GenericRepositry(AppDbContext context) //Ask CLR To Create Object From AppDbContext
        {
            //_context = new AppDbContext();
            _context = context;
        }
        public async void Add(T entity)
        {
            _context.Add(entity);
        }
        public void Delete(T entity)
        {
                _context.Remove(entity);
        }
        public async Task<T> Get(int id)
        {
            //var department = _context.Departments.FirstOrDefault(D => D.Id == id);
            var result = await _context.Set<T>().FindAsync(id);
            return result;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            //var department = _context.Set<T>().ToList();

            if(typeof(T)==typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }
            else
            {
                return await _context.Set<T>().ToListAsync();
            }          
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }  

    }
}
