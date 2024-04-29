using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IEmployeeRepositry : IGenericRepositry<Employee>
    {
        public Task< IEnumerable<Employee>> GetByName(string name);
    }
}
