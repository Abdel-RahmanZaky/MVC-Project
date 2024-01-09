using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.DAL.Models;
using Project.PL.Controllers;
using Project.PL.ViewModels;

namespace Project.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<RoleViewModel, IdentityRole>()
                .ForMember(D => D.Name , O => O.MapFrom(S => S.RoleName))
                .ReverseMap();
        }
    }
}
