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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
