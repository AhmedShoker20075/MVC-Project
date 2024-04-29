using BLL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositries
{
    public class DepartmentRepositry : GenericRepositry<Department> , IDepartmentRepositry
    {
        public DepartmentRepositry(AppDbContext context):base(context)
        {
            
        }
        public async Task<IEnumerable<Department>> GetByName(string name)
        {
            return await _context.Departments.Where(D => D.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
