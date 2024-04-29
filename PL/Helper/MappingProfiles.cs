using AutoMapper;
using DAL.Models;
using PL.ViewModels;

namespace PL.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            //CreateMap<Employee ,EmployeeViewModel>();

            CreateMap<Department, DepartmentViewModel>().ReverseMap();
        }

    }
}
