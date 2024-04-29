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
    public class EmployeeRepositry : GenericRepositry<Employee>, IEmployeeRepositry
    {
        public EmployeeRepositry(AppDbContext context):base(context)
        {
            
        }

        public async Task< IEnumerable<Employee>> GetByName(string name)
        {
            return await _context.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower())).Include(E => E.Department).ToListAsync();
        }
    }
}
