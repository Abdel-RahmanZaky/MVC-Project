using Project.BLL.Interfaces;
using Project.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public IDepartmentRepository DepartmentRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }

        public UnitOfWork(AppDbContext dbContext) // Ask CLR for Creating Object form AppDbContext
        {
            _dbContext = dbContext;
            DepartmentRepository = new DepartmentRepository(_dbContext);
            EmployeeRepository = new EmployeeRepository(_dbContext);
        }

        public async Task<int> Complete()
        {
           return await _dbContext.SaveChangesAsync();
        }


        public  async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
