using Microsoft.EntityFrameworkCore;
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
    public class GenericRepository<T>  : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContect)
        {
            _dbContext = dbContect;
        }

        

        public void Add(T entity)
           => _dbContext.Add(entity);
          

        public void Update(T entity)
           => _dbContext.Update(entity);
       

        public void Delete(T entity)
           => _dbContext.Remove(entity);
       

        public async Task<T> Get(int id)
        {
            /// var Employee = _dbContext.Employees.Local.Where(d => d.Id == id).FirstOrDefault();
            /// if(Employee is null)
            ///      Employee = _dbContext.Employees.Where(d => d.Id == id).FirstOrDefault();
            /// return Employee;

            //return _dbContext.Employees.Find(id);
            return await _dbContext.FindAsync<T>(id); // Ef Core 3.1 Feature
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return  (IEnumerable<T>) await _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
            }
        }
    }
}
