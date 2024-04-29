using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.Interfaces
{
    public interface IDepartmentRepositry : IGenericRepositry<Department>
    {
        public Task< IEnumerable<Department>> GetByName(string name);

    }
}
