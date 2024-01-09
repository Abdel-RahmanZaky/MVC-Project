using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IDepartmentRepository DepartmentRepository { get; set; }

        public IEmployeeRepository EmployeeRepository { get; set; }

        Task<int> Complete();
    }
}
