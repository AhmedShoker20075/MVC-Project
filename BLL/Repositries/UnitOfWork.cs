using BLL.Interfaces;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Lazy<IDepartmentRepositry> departmentRepositry;
        private Lazy<IEmployeeRepositry> employeeRepositry;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            departmentRepositry = new Lazy<IDepartmentRepositry>(new DepartmentRepositry(context));
            employeeRepositry = new Lazy<IEmployeeRepositry>(new EmployeeRepositry(context));
        }
        public IDepartmentRepositry DepartmentRepositry => departmentRepositry.Value;
        public IEmployeeRepositry EmployeeRepositry => employeeRepositry.Value;

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
