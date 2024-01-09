using Project.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Project.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }
        // Navigational Property => [Many]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
