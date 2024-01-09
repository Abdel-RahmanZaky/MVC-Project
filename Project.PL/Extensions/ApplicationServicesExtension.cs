using Microsoft.Extensions.DependencyInjection;
using Project.BLL.Interfaces;
using Project.BLL.Repositories;

namespace Project.PL.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddSingleton<IDepartmentRepository, DepartmentRepository>();


            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
