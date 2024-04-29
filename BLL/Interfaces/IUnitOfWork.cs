using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IDepartmentRepositry DepartmentRepositry { get; }
        public IEmployeeRepositry EmployeeRepositry { get; }        
        Task<int> Complete();
    }
}
