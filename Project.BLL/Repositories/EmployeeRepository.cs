using Project.BLL.Interfaces;
using Project.DAL.Data;
using Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
        {
            
        }

        public  IQueryable<Employee> SearchByName(string name)
        {
            return  _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name));
        }
    }
}
