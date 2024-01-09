using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using Project.DAL.Data.Configurations;
using Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseSqlServer("Server =.; Database = MvcApplicationDb; Trusted_Connection = true");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call Congiguration Classes
            //modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public object AsNoTracking()
        {
            throw new NotImplementedException();
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }
        //public DbSet<IdentityUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
    }
}
